namespace RCCatherineApi.DTOs
{
    public class EmitirComprobanteDto
    {
        public int OrdenVentaId { get; set; }
        public string TipoComprobante { get; set; } = "BOLETA"; // BOLETA | FACTURA
    }
}