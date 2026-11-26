namespace RCCatherineApi.DTOs
{
    public class DetalleCotizacionItemDto
    {
        public int ProductoId { get; set; }
        public int UnidadMedidaSunatId { get; set; }
        public decimal Cantidad { get; set; }
        public string TipoListaPrecio { get; set; } = "MENOR"; // MENOR | MAYOR | PERSONALIZADO
        public decimal? PrecioPersonalizado { get; set; }
    }
}