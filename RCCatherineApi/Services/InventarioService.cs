using RCCatherineApi.Data;
using RCCatherineApi.DTOs;
using RCCatherineApi.Models;
using RCCatherineApi.Repositories;

namespace RCCatherineApi.Services
{
    public class InventarioService
    {
        private readonly AppDbContext _context;
        private readonly IProductoRepository _productoRepo;

        public InventarioService(AppDbContext context, IProductoRepository productoRepo)
        {
            _context = context;
            _productoRepo = productoRepo;
        }
        //ingresos con prorrateo de costos logísticos
        public async Task<Ingreso> RegistrarIngresoAsync(CrearIngresoDto dto, int trabajadorId)
        {
            if (dto.Items.Count == 0)
                throw new ArgumentException("Debe registrar al menos un producto en el ingreso.");

            var proveedor = await _context.Proveedores.FindAsync(dto.ProveedorId)
                ?? throw new KeyNotFoundException("Proveedor no encontrado.");

            await using var tx = await _context.Database.BeginTransactionAsync();

            var ingreso = new Ingreso
            {
                NumeroDocumento = dto.NumeroDocumento.Trim(),
                TipoIngreso = dto.TipoIngreso.ToUpper(),
                ProveedorId = proveedor.Id,
                TrabajadorId = trabajadorId,
                FleteInternacional = dto.FleteInternacional,
                Seguro = dto.Seguro,
                Aranceles = dto.Aranceles,
                GastosDesaduanaje = dto.GastosDesaduanaje,
                Justificacion = dto.Justificacion
            };

            decimal totalCostosLogisticos = Math.Round(
                dto.FleteInternacional + dto.Seguro + dto.Aranceles + dto.GastosDesaduanaje, 2);

            // Calculamos total FOB para prorrateo proporcional por valor FOB
            var itemsPreparados = new List<(DetalleIngresoItemDto Item, Producto Producto, decimal Factor, decimal SubtotalFob, decimal UnidadesBase)>();
            decimal subtotalFobTotal = 0m;

            foreach (var item in dto.Items)
            {
                if (item.Cantidad <= 0)
                    throw new ArgumentException("La cantidad debe ser mayor a cero.");

                var producto = await _context.Productos.FindAsync(item.ProductoId)
                    ?? throw new KeyNotFoundException($"Producto {item.ProductoId} no encontrado.");

                decimal factor = await _productoRepo.ObtenerFactorConversionAsync(item.ProductoId, item.UnidadMedidaSunatId);
                decimal unidadesBase = item.Cantidad * factor;
                decimal subtotalFob = Math.Round(item.Cantidad * item.CostoUnitarioFob, 2);

                subtotalFobTotal += subtotalFob;
                itemsPreparados.Add((item, producto, factor, subtotalFob, unidadesBase));
            }

            decimal totalCostoAlmacen = 0m;

            foreach (var entry in itemsPreparados)
            {
                decimal proporcion = subtotalFobTotal > 0 ? entry.SubtotalFob / subtotalFobTotal : 0m;
                decimal costoLogisticoItem = Math.Round(totalCostosLogisticos * proporcion, 2);
                decimal costoTotalItem = Math.Round(entry.SubtotalFob + costoLogisticoItem, 2);
                decimal costoRealUnitarioBase = entry.UnidadesBase > 0 ? Math.Round(costoTotalItem / entry.UnidadesBase, 4) : 0m;

                var detalle = new DetalleIngreso
                {
                    ProductoId = entry.Producto.Id,
                    UnidadMedidaSunatId = entry.Item.UnidadMedidaSunatId,
                    Cantidad = entry.Item.Cantidad,
                    FactorConversion = entry.Factor,
                    CostoUnitarioFob = entry.Item.CostoUnitarioFob,
                    CostoLogisticoProrrateado = costoLogisticoItem,
                    CostoRealUnitario = costoRealUnitarioBase,
                    Subtotal = costoTotalItem
                };

                ingreso.Detalles.Add(detalle);
                totalCostoAlmacen += costoTotalItem;

                // Actualizar stock físico y Costo Promedio Ponderado en Kardex
                decimal saldoAnteriorCant = entry.Producto.StockFisico;
                decimal saldoAnteriorValor = Math.Round(saldoAnteriorCant * entry.Producto.CostoPromedioPonderado, 2);

                decimal nuevoSaldoCant = saldoAnteriorCant + entry.UnidadesBase;
                decimal nuevoSaldoValor = Math.Round(saldoAnteriorValor + costoTotalItem, 2);
                decimal nuevoCpp = nuevoSaldoCant > 0 ? Math.Round(nuevoSaldoValor / nuevoSaldoCant, 4) : costoRealUnitarioBase;

                entry.Producto.StockFisico = nuevoSaldoCant;
                entry.Producto.CostoPromedioPonderado = nuevoCpp;

                _context.MovimientosKardex.Add(new MovimientoKardex
                {
                    ProductoId = entry.Producto.Id,
                    TrabajadorId = trabajadorId,
                    TipoMovimiento = "INGRESO",
                    ReferenciaDocumento = ingreso.NumeroDocumento,
                    Observacion = $"{ingreso.TipoIngreso} - {proveedor.RazonSocial}",
                    CantidadUnidadesBase = entry.UnidadesBase,
                    CostoUnitario = costoRealUnitarioBase,
                    CostoPromedioResultante = nuevoCpp,
                    SaldoCantidad = nuevoSaldoCant,
                    SaldoValorizado = nuevoSaldoValor
                });
            }

            ingreso.SubtotalFob = subtotalFobTotal;
            ingreso.TotalCostosLogisticos = totalCostosLogisticos;
            ingreso.TotalCostoPuestoAlmacen = totalCostoAlmacen;

            _context.Ingresos.Add(ingreso);
            await _context.SaveChangesAsync();
            await tx.CommitAsync();

            return ingreso;
        }

        //cálculo de Costo Promedio Ponderado en el Kardex
        public async Task<MovimientoKardex> RegistrarAjusteManualAsync(AjusteInventarioDto dto, int trabajadorId)
        {
            if (string.IsNullOrWhiteSpace(dto.Justificacion))
                throw new ArgumentException("La justificación es obligatoria para ajustes manuales o mermas.");

            if (dto.CantidadUnidadesBase == 0)
                throw new ArgumentException("La cantidad de ajuste no puede ser cero.");

            await using var tx = await _context.Database.BeginTransactionAsync();

            var producto = await _context.Productos.FindAsync(dto.ProductoId)
                ?? throw new KeyNotFoundException("Producto no encontrado.");

            decimal factor = await _productoRepo.ObtenerFactorConversionAsync(dto.ProductoId, dto.UnidadMedidaSunatId);
            decimal deltaUnidadesBase = dto.CantidadUnidadesBase * factor;

            decimal nuevoSaldoCant = producto.StockFisico + deltaUnidadesBase;
            if (nuevoSaldoCant < 0)
                throw new InvalidOperationException("El ajuste deja el stock físico en negativo.");

            decimal costoUnitario = producto.CostoPromedioPonderado;
            decimal nuevoSaldoValor = Math.Round(nuevoSaldoCant * costoUnitario, 2);

            producto.StockFisico = nuevoSaldoCant;

            var movimiento = new MovimientoKardex
            {
                ProductoId = producto.Id,
                TrabajadorId = trabajadorId,
                TipoMovimiento = "AJUSTE",
                ReferenciaDocumento = $"AJ-{DateTime.UtcNow:yyyyMMddHHmmss}",
                Observacion = dto.Justificacion.Trim(),
                CantidadUnidadesBase = deltaUnidadesBase,
                CostoUnitario = costoUnitario,
                CostoPromedioResultante = costoUnitario,
                SaldoCantidad = nuevoSaldoCant,
                SaldoValorizado = nuevoSaldoValor
            };

            _context.MovimientosKardex.Add(movimiento);
            await _context.SaveChangesAsync();
            await tx.CommitAsync();

            return movimiento;
        }
    }
}