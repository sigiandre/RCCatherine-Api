using Microsoft.EntityFrameworkCore;
using RCCatherineApi.Data;
using RCCatherineApi.Models;

namespace RCCatherineApi.Repositories
{
    public class ProductoRepository : Repository<Producto>, IProductoRepository
    {
        public ProductoRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Producto>> GetCatalogoCompletoAsync() =>
        await _dbSet
            .AsNoTracking()
            .Include(p => p.Categoria)
            .Include(p => p.UnidadBase)
            .Include(p => p.Equivalencias)
                .ThenInclude(e => e.UnidadMedidaSunat)
            .Where(p => p.Activo)
            .OrderBy(p => p.Nombre)
            .ToListAsync();

        public async Task<Producto?> GetDetalleProductoAsync(int id) =>
            await _dbSet
                .Include(p => p.Categoria)
                .Include(p => p.UnidadBase)
                .Include(p => p.Equivalencias)
                    .ThenInclude(e => e.UnidadMedidaSunat)
                .FirstOrDefaultAsync(p => p.Id == id);

        public async Task<IEnumerable<Producto>> GetAlertasStockMinimoAsync() =>
            await _dbSet
                .AsNoTracking()
                .Include(p => p.Categoria)
                .Include(p => p.UnidadBase)
                .Where(p => p.Activo && (p.StockFisico - p.StockReservado) <= p.StockMinimo)
                .ToListAsync();

        public async Task<IEnumerable<MovimientoKardex>> GetKardexPorProductoAsync(int productoId) =>
            await _context.MovimientosKardex
                .AsNoTracking()
                .Where(m => m.ProductoId == productoId)
                .OrderByDescending(m => m.Fecha)
                .ThenByDescending(m => m.Id)
                .ToListAsync();

        public async Task<decimal> ObtenerFactorConversionAsync(int productoId, int unidadMedidaSunatId)
        {
            var producto = await _dbSet.AsNoTracking().FirstOrDefaultAsync(p => p.Id == productoId)
                ?? throw new KeyNotFoundException($"Producto {productoId} no encontrado.");

            if (producto.UnidadBaseId == unidadMedidaSunatId)
                return 1m;

            var eq = await _context.EquivalenciasProducto
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.ProductoId == productoId
                                      && e.UnidadMedidaSunatId == unidadMedidaSunatId
                                      && e.Activo);

            if (eq is null)
                throw new InvalidOperationException("No existe equivalencia configurada para la unidad seleccionada en este producto.");

            return eq.FactorUnidadesBase;
        }
    }
}