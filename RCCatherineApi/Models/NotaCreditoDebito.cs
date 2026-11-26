namespace RCCatherineApi.Models
{
    public class NotaCreditoDebito
    {
        public int Id { get; set; }
        public int ComprobanteElectronicoId { get; set; }
        public ComprobanteElectronico? ComprobanteElectronico { get; set; }

        // NOTA_CREDITO | NOTA_DEBITO
        public string TipoNota { get; set; } = "NOTA_CREDITO";
        public string Serie { get; set; } = "FC01";
        public string Correlativo { get; set; } = string.Empty;
        public DateTime FechaEmision { get; set; } = DateTime.UtcNow;

        public string CodigoMotivoSunat { get; set; } = string.Empty;
        public string Motivo { get; set; } = string.Empty;
        public decimal Monto { get; set; }
        public bool ExtornaMercaderiaKardex { get; set; }
        public string EstadoSunat { get; set; } = "PENDIENTE_ENVIO";
        public string? CodigoCdr { get; set; }
        public string? DescripcionCdr { get; set; }
    }
}