namespace RCCatherineApi.Models
{
    public class ComprobanteElectronico
    {
        public int Id { get; set; }
        public int OrdenVentaId { get; set; }
        public OrdenVenta? OrdenVenta { get; set; }

        // BOLETA | FACTURA
        public string TipoComprobante { get; set; } = "BOLETA";
        public string Serie { get; set; } = "B001";
        public string Correlativo { get; set; } = string.Empty;
        public DateTime FechaEmision { get; set; } = DateTime.UtcNow;

        public string TipoDocumentoCliente { get; set; } = "DNI";
        public string NumeroDocumentoCliente { get; set; } = string.Empty;
        public string NombreRazonSocialCliente { get; set; } = string.Empty;

        public decimal Subtotal { get; set; }
        public decimal Igv { get; set; }
        public decimal Total { get; set; }
        public decimal SaldoDeudor { get; set; }

        // Estado SUNAT / UBL / CDR
        // PENDIENTE_ENVIO | ACEPTADO | OBSERVADO | RECHAZADO | ANULADO
        public string EstadoSunat { get; set; } = "PENDIENTE_ENVIO";
        public string? NombreArchivoXml { get; set; }
        public string? CodigoCdr { get; set; }
        public string? DescripcionCdr { get; set; }

        public ICollection<NotaCreditoDebito> Notas { get; set; } = new List<NotaCreditoDebito>();
    }
}