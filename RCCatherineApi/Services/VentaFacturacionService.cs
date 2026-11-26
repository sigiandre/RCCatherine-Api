using Microsoft.EntityFrameworkCore;
using RCCatherineApi.Data;
using RCCatherineApi.DTOs;
using RCCatherineApi.Models;
using RCCatherineApi.Repositories;

namespace RCCatherineApi.Services
{
    public class VentaFacturacionService
    {
        private readonly AppDbContext _context;
        private readonly IProductoRepository _productoRepo;

        public VentaFacturacionService(AppDbContext context, IProductoRepository productoRepo)
        {
            _context = context;
            _productoRepo = productoRepo;
        }

        //Cubre cotizaciones
        public async Task<Cotizacion> CrearCotizacionAsync(CrearCotizacionDto dto, int vendedorId)
        {
            if (dto.Items.Count == 0)
                throw new ArgumentException("La cotización debe tener al menos un ítem.");

            var cliente = await _context.Clientes.FindAsync(dto.ClienteId)
                ?? throw new KeyNotFoundException("Cliente no encontrado.");

            int correlativo = await _context.Cotizaciones.CountAsync() + 1;

            var cotizacion = new Cotizacion
            {
                Codigo = $"COT-{correlativo:D4}",
                FechaEmision = DateTime.UtcNow,
                DiasVigencia = dto.DiasVigencia > 0 ? dto.DiasVigencia : 7,
                FechaVencimiento = DateTime.UtcNow.AddDays(dto.DiasVigencia > 0 ? dto.DiasVigencia : 7),
                Estado = "Pendiente",
                ClienteId = cliente.Id,
                DireccionEntregaId = dto.DireccionEntregaId,
                VendedorId = vendedorId,
                Observaciones = dto.Observaciones
            };

            decimal subtotalGeneral = 0m;

            foreach (var item in dto.Items)
            {
                var producto = await _context.Productos.FindAsync(item.ProductoId)
                    ?? throw new KeyNotFoundException($"Producto {item.ProductoId} no encontrado.");

                decimal factor = await _productoRepo.ObtenerFactorConversionAsync(item.ProductoId, item.UnidadMedidaSunatId);

                string lista = item.TipoListaPrecio.ToUpper();
                decimal precioUnitario = lista switch
                {
                    "MAYOR" => producto.PrecioMayor,
                    "PERSONALIZADO" => item.PrecioPersonalizado
                        ?? throw new ArgumentException("Debe indicar PrecioPersonalizado cuando el tipo es PERSONALIZADO."),
                    _ => producto.PrecioMenor
                };

                decimal subtotalLinea = Math.Round(item.Cantidad * precioUnitario, 2);
                subtotalGeneral += subtotalLinea;

                cotizacion.Detalles.Add(new DetalleCotizacion
                {
                    ProductoId = producto.Id,
                    UnidadMedidaSunatId = item.UnidadMedidaSunatId,
                    Cantidad = item.Cantidad,
                    FactorConversion = factor,
                    TipoListaPrecio = lista,
                    PrecioUnitario = precioUnitario,
                    Subtotal = subtotalLinea
                });
            }

            cotizacion.Subtotal = Math.Round(subtotalGeneral, 2);
            cotizacion.Igv = Math.Round(cotizacion.Subtotal * 0.18m, 2);
            cotizacion.Total = Math.Round(cotizacion.Subtotal + cotizacion.Igv, 2);

            _context.Cotizaciones.Add(cotizacion);
            await _context.SaveChangesAsync();

            return cotizacion;
        }

        //conversión de cotización aprobada a orden de venta
        public async Task<OrdenVenta> CrearOrdenVentaAsync(CrearOrdenVentaDto dto, int vendedorId)
        {
            await using var tx = await _context.Database.BeginTransactionAsync();

            Cotizacion? cotizacion = null;
            if (dto.CotizacionId.HasValue)
            {
                cotizacion = await _context.Cotizaciones
                    .Include(c => c.Detalles)
                    .FirstOrDefaultAsync(c => c.Id == dto.CotizacionId.Value)
                    ?? throw new KeyNotFoundException("Cotización no encontrada.");

                if (!string.Equals(cotizacion.Estado, "Aprobada", StringComparison.OrdinalIgnoreCase))
                    throw new InvalidOperationException("Solo se pueden convertir cotizaciones en estado 'Aprobada'.");

                if (cotizacion.OrdenVenta is not null)
                    throw new InvalidOperationException("Esta cotización ya fue convertida en una Orden de Venta.");

                cotizacion.Estado = "Convertida";
            }

            var cliente = await _context.Clientes.FindAsync(cotizacion?.ClienteId ?? dto.ClienteId)
                ?? throw new KeyNotFoundException("Cliente no encontrado.");

            int correlativo = await _context.OrdenesVenta.CountAsync() + 1;

            var orden = new OrdenVenta
            {
                NumeroOrden = $"OV-{correlativo:D4}",
                CotizacionId = cotizacion?.Id,
                ClienteId = cliente.Id,
                DireccionEntregaId = cotizacion?.DireccionEntregaId ?? dto.DireccionEntregaId,
                VendedorId = vendedorId,
                Estado = "RESERVADA"
            };

            decimal subtotalGeneral = 0m;

            var itemsOrigen = cotizacion is not null
                ? cotizacion.Detalles.Select(d => new DetalleCotizacionItemDto
                {
                    ProductoId = d.ProductoId,
                    UnidadMedidaSunatId = d.UnidadMedidaSunatId,
                    Cantidad = d.Cantidad,
                    TipoListaPrecio = d.TipoListaPrecio,
                    PrecioPersonalizado = d.PrecioUnitario
                }).ToList()
                : dto.Items;

            if (itemsOrigen.Count == 0)
                throw new ArgumentException("La orden de venta debe contener al menos un producto.");

            foreach (var item in itemsOrigen)
            {
                var producto = await _context.Productos.FindAsync(item.ProductoId)
                    ?? throw new KeyNotFoundException($"Producto {item.ProductoId} no encontrado.");

                decimal factor = await _productoRepo.ObtenerFactorConversionAsync(item.ProductoId, item.UnidadMedidaSunatId);
                decimal unidadesAreservar = item.Cantidad * factor;

                if (producto.StockDisponible < unidadesAreservar)
                    throw new InvalidOperationException($"Stock disponible insuficiente para '{producto.Nombre}'.");

                producto.StockReservado += unidadesAreservar;

                string lista = item.TipoListaPrecio.ToUpper();
                decimal precioUnitario = lista switch
                {
                    "MAYOR" => producto.PrecioMayor,
                    "PERSONALIZADO" => item.PrecioPersonalizado ?? producto.PrecioMenor,
                    _ => producto.PrecioMenor
                };

                decimal subtotalLinea = Math.Round(item.Cantidad * precioUnitario, 2);
                subtotalGeneral += subtotalLinea;

                orden.Detalles.Add(new DetalleOrdenVenta
                {
                    ProductoId = producto.Id,
                    UnidadMedidaSunatId = item.UnidadMedidaSunatId,
                    Cantidad = item.Cantidad,
                    FactorConversion = factor,
                    UnidadesBaseReservadas = unidadesAreservar,
                    PrecioUnitario = precioUnitario,
                    Subtotal = subtotalLinea
                });
            }

            orden.Subtotal = Math.Round(subtotalGeneral, 2);
            orden.Igv = Math.Round(orden.Subtotal * 0.18m, 2);
            orden.Total = Math.Round(orden.Subtotal + orden.Igv, 2);
            orden.MontoPagado = 0m;
            orden.SaldoPendiente = orden.Total;

            _context.OrdenesVenta.Add(orden);
            await _context.SaveChangesAsync();
            await tx.CommitAsync();

            return orden;
        }

        // RegistrarPagos
        public async Task<OrdenVenta> RegistrarPagosAsync(RegistrarPagosOrdenDto dto, int trabajadorId)
        {
            if (dto.Pagos.Count == 0)
                throw new ArgumentException("Debe registrar al menos un método de pago.");

            await using var tx = await _context.Database.BeginTransactionAsync();

            var orden = await _context.OrdenesVenta
                .Include(o => o.Pagos)
                .FirstOrDefaultAsync(o => o.Id == dto.OrdenVentaId)
                ?? throw new KeyNotFoundException("Orden de venta no encontrada.");

            decimal sumaNuevosPagos = 0m;

            foreach (var pagoDto in dto.Pagos)
            {
                if (pagoDto.Monto <= 0)
                    throw new ArgumentException("El monto de cada pago debe ser mayor a cero.");

                sumaNuevosPagos += pagoDto.Monto;

                orden.Pagos.Add(new PagoVenta
                {
                    OrdenVentaId = orden.Id,
                    MetodoPago = pagoDto.MetodoPago.ToUpper(),
                    Monto = Math.Round(pagoDto.Monto, 2),
                    NumeroOperacion = pagoDto.NumeroOperacion,
                    RegistradoPorId = trabajadorId
                });
            }

            orden.MontoPagado = Math.Round(orden.Pagos.Sum(p => p.Monto), 2);
            orden.SaldoPendiente = Math.Round(orden.Total - orden.MontoPagado, 2);

            if (orden.SaldoPendiente < 0)
                throw new InvalidOperationException("El total pagado excede el monto de la orden de venta.");

            await _context.SaveChangesAsync();
            await tx.CommitAsync();

            return orden;
        }

        //Emitir Comprobantes
        public async Task<ComprobanteElectronico> EmitirComprobanteAsync(EmitirComprobanteDto dto)
        {
            await using var tx = await _context.Database.BeginTransactionAsync();

            var orden = await _context.OrdenesVenta
                .Include(o => o.Cliente)
                .Include(o => o.ComprobanteElectronico)
                .FirstOrDefaultAsync(o => o.Id == dto.OrdenVentaId)
                ?? throw new KeyNotFoundException("Orden de venta no encontrada.");

            if (orden.ComprobanteElectronico is not null)
                throw new InvalidOperationException("La orden ya tiene un comprobante electrónico emitido.");

            var cliente = orden.Cliente!;
            string tipo = dto.TipoComprobante.ToUpper();

            if (tipo == "FACTURA")
            {
                if (!string.Equals(cliente.TipoDocumento, "RUC", StringComparison.OrdinalIgnoreCase)
                    || cliente.NumeroDocumento.Length != 11
                    || !cliente.NumeroDocumento.All(char.IsDigit))
                {
                    throw new InvalidOperationException("Para emitir FACTURA se requiere un RUC válido de 11 dígitos.");
                }
            }
            else if (tipo == "BOLETA")
            {
                var docsBoleta = new[] { "DNI", "CE", "PASAPORTE" };
                if (!docsBoleta.Contains(cliente.TipoDocumento.ToUpper()))
                    throw new InvalidOperationException("La BOLETA solo aplica para DNI, Carnet de Extranjería o Pasaporte.");
            }
            else
            {
                throw new ArgumentException("Tipo de comprobante no válido. Use BOLETA o FACTURA.");
            }

            string serie = tipo == "FACTURA" ? "F001" : "B001";
            int correlativoNum = await _context.ComprobantesElectronicos
                .CountAsync(c => c.Serie == serie) + 1;

            var comprobante = new ComprobanteElectronico
            {
                OrdenVentaId = orden.Id,
                TipoComprobante = tipo,
                Serie = serie,
                Correlativo = correlativoNum.ToString("D8"),
                TipoDocumentoCliente = cliente.TipoDocumento.ToUpper(),
                NumeroDocumentoCliente = cliente.NumeroDocumento,
                NombreRazonSocialCliente = cliente.NombreRazonSocial,
                Subtotal = orden.Subtotal,
                Igv = orden.Igv,
                Total = orden.Total,
                SaldoDeudor = orden.SaldoPendiente,
                EstadoSunat = "ACEPTADO",
                NombreArchivoXml = $"{serie}-{correlativoNum:D8}.xml",
                CodigoCdr = "0",
                DescripcionCdr = "Comprobante aceptado por SUNAT (simulación base UBL/CDR)."
            };

            if (orden.Estado == "RESERVADA")
                orden.Estado = "FACTURADA";

            _context.ComprobantesElectronicos.Add(comprobante);
            await _context.SaveChangesAsync();
            await tx.CommitAsync();

            return comprobante;
        }

        //Emitir Guia Remision
        public async Task<GuiaRemision> EmitirGuiaRemisionAsync(EmitirGuiaRemisionDto dto, int almacenistaId)
        {
            await using var tx = await _context.Database.BeginTransactionAsync();

            var orden = await _context.OrdenesVenta
                .Include(o => o.Detalles)
                .Include(o => o.GuiaRemision)
                .FirstOrDefaultAsync(o => o.Id == dto.OrdenVentaId)
                ?? throw new KeyNotFoundException("Orden de venta no encontrada.");

            if (orden.GuiaRemision is not null)
                throw new InvalidOperationException("Ya se emitió una Guía de Remisión para esta orden.");

            string modalidad = dto.ModalidadTransporte.ToUpper();
            if (modalidad == "PUBLICO")
            {
                if (!dto.AgenciaTransporteId.HasValue)
                    throw new ArgumentException("Para transporte público debe seleccionar una agencia de transporte.");

                var agencia = await _context.AgenciasTransporte.FindAsync(dto.AgenciaTransporteId.Value)
                    ?? throw new KeyNotFoundException("Agencia de transporte no encontrada.");
            }
            else if (modalidad == "PRIVADO")
            {
                if (string.IsNullOrWhiteSpace(dto.PlacaVehiculo)
                    || string.IsNullOrWhiteSpace(dto.LicenciaConductor)
                    || string.IsNullOrWhiteSpace(dto.DniConductor)
                    || string.IsNullOrWhiteSpace(dto.NombresApellidosConductor))
                {
                    throw new ArgumentException("Para transporte privado se requieren placa, licencia, DNI y nombres del conductor.");
                }
            }
            else
            {
                throw new ArgumentException("Modalidad de transporte inválida. Use PUBLICO o PRIVADO.");
            }

            int correlativoNum = await _context.GuiasRemision.CountAsync() + 1;

            var guia = new GuiaRemision
            {
                OrdenVentaId = orden.Id,
                Serie = "T001",
                Correlativo = correlativoNum.ToString("D8"),
                FechaTraslado = dto.FechaTraslado,
                PuntoPartida = dto.PuntoPartida,
                PuntoLlegada = dto.PuntoLlegada,
                MotivoTraslado = dto.MotivoTraslado,
                NumeroBultos = dto.NumeroBultos,
                PesoBrutoTotalKg = dto.PesoBrutoTotalKg,
                ModalidadTransporte = modalidad,
                AgenciaTransporteId = modalidad == "PUBLICO" ? dto.AgenciaTransporteId : null,
                PlacaVehiculo = modalidad == "PRIVADO" ? dto.PlacaVehiculo : null,
                LicenciaConductor = modalidad == "PRIVADO" ? dto.LicenciaConductor : null,
                DniConductor = modalidad == "PRIVADO" ? dto.DniConductor : null,
                NombresApellidosConductor = modalidad == "PRIVADO" ? dto.NombresApellidosConductor : null,
                EstadoSunat = "ACEPTADO",
                CodigoCdr = "0"
            };

            // Descontar definitivamente el stock físico y liberar reserva + registrar salida en Kardex
            foreach (var det in orden.Detalles)
            {
                var producto = await _context.Productos.FindAsync(det.ProductoId)
                    ?? throw new KeyNotFoundException($"Producto {det.ProductoId} no encontrado.");

                decimal unidadesSalida = det.UnidadesBaseReservadas;
                if (producto.StockFisico < unidadesSalida)
                    throw new InvalidOperationException($"Stock físico insuficiente para despachar '{producto.Nombre}'.");

                producto.StockReservado = Math.Max(0m, producto.StockReservado - unidadesSalida);
                producto.StockFisico -= unidadesSalida;

                decimal costoSalida = producto.CostoPromedioPonderado;
                decimal saldoValor = Math.Round(producto.StockFisico * costoSalida, 2);

                _context.MovimientosKardex.Add(new MovimientoKardex
                {
                    ProductoId = producto.Id,
                    TrabajadorId = almacenistaId,
                    TipoMovimiento = "SALIDA",
                    ReferenciaDocumento = $"{guia.Serie}-{guia.Correlativo}",
                    Observacion = $"Despacho Orden {orden.NumeroOrden}",
                    CantidadUnidadesBase = -unidadesSalida,
                    CostoUnitario = costoSalida,
                    CostoPromedioResultante = costoSalida,
                    SaldoCantidad = producto.StockFisico,
                    SaldoValorizado = saldoValor
                });
            }

            orden.Estado = "DESPACHADA";
            _context.GuiasRemision.Add(guia);

            await _context.SaveChangesAsync();
            await tx.CommitAsync();

            return guia;
        }
    }
}