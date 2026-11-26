namespace RCCatherineApi.Models
{
    public class Producto
    {
        public int Id { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }

        public int CategoriaId { get; set; }
        public Categoria? Categoria { get; set; }

        // Unidad base del producto (por ejemplo NIU)
        public int UnidadBaseId { get; set; }
        public UnidadMedidaSunat? UnidadBase { get; set; }

        // Listas de precios en catálogo (modificables solo por Administrador)
        public decimal PrecioMenor { get; set; }
        public decimal PrecioMayor { get; set; }

        // Costo promedio ponderado actualizado por Kardex
        public decimal CostoPromedioPonderado { get; set; }

        // Stock físico real vs. reservado por órdenes de venta vs. alerta de stock mínimo
        public decimal StockFisico { get; set; }
        public decimal StockReservado { get; set; }
        public decimal StockMinimo { get; set; }

        // Stock disponible calculado: StockFisico - StockReservado
        public decimal StockDisponible => StockFisico - StockReservado;

        public bool Activo { get; set; } = true;

        public ICollection<EquivalenciaProducto> Equivalencias { get; set; } = new List<EquivalenciaProducto>();
        public ICollection<MovimientoKardex> MovimientosKardex { get; set; } = new List<MovimientoKardex>();
    }
}