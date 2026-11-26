namespace RCCatherineApi.Models
{
    public class DetalleCotizacion
    {
        public int Id { get; set; }
        public int CotizacionId { get; set; }
        public Cotizacion? Cotizacion { get; set; }

        public int ProductoId { get; set; }
        public Producto? Producto { get; set; }

        public int UnidadMedidaSunatId { get; set; }
        public UnidadMedidaSunat? UnidadMedidaSunat { get; set; }

        public decimal Cantidad { get; set; }
        public decimal FactorConversion { get; set; } = 1m;

        // MENOR | MAYOR | PERSONALIZADO
        public string TipoListaPrecio { get; set; } = "MENOR";
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal { get; set; }
    }
}