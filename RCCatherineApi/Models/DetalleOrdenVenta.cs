namespace RCCatherineApi.Models
{
    public class DetalleOrdenVenta
    {
        public int Id { get; set; }
        public int OrdenVentaId { get; set; }
        public OrdenVenta? OrdenVenta { get; set; }

        public int ProductoId { get; set; }
        public Producto? Producto { get; set; }

        public int UnidadMedidaSunatId { get; set; }
        public UnidadMedidaSunat? UnidadMedidaSunat { get; set; }

        public decimal Cantidad { get; set; }
        public decimal FactorConversion { get; set; } = 1m;
        public decimal UnidadesBaseReservadas { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal { get; set; }
    }
}