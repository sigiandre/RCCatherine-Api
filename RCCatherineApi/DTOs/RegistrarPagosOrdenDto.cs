namespace RCCatherineApi.DTOs
{
    public class RegistrarPagosOrdenDto
    {
        public int OrdenVentaId { get; set; }
        public List<RegistrarPagoItemDto> Pagos { get; set; } = new();
    }
}