namespace RCCatherineApi.DTOs
{
    public class CrearCotizacionDto
    {
        public int ClienteId { get; set; }
        public int? DireccionEntregaId { get; set; }
        public int DiasVigencia { get; set; } = 7;
        public string? Observaciones { get; set; }
        public List<DetalleCotizacionItemDto> Items { get; set; } = new();
    }
}