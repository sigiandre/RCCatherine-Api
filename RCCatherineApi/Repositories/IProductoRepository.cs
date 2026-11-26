using RCCatherineApi.Models;

namespace RCCatherineApi.Repositories
{
    public interface IProductoRepository : IRepository<Producto>
    {
        Task<IEnumerable<Producto>> GetCatalogoCompletoAsync();
        Task<Producto?> GetDetalleProductoAsync(int id);
        Task<IEnumerable<Producto>> GetAlertasStockMinimoAsync();
        Task<IEnumerable<MovimientoKardex>> GetKardexPorProductoAsync(int productoId);
        Task<decimal> ObtenerFactorConversionAsync(int productoId, int unidadMedidaSunatId);
    }
}