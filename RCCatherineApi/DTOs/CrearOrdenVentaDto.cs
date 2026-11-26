namespace RCCatherineApi.DTOs
{
    public class CrearOrdenVentaDto
    {
        public int? CotizacionId { get; set; }
        public int ClienteId { get; set; }
        public int? DireccionEntregaId { get; set; }
        public List<DetalleCotizacionItemDto> Items { get; set; } = new();
    }
}