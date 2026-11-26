namespace RCCatherineApi.DTOs
{
    public class EmitirNotaDto
    {
        public int ComprobanteElectronicoId { get; set; }
        public string TipoNota { get; set; } = "NOTA_CREDITO"; // NOTA_CREDITO | NOTA_DEBITO
        public string CodigoMotivoSunat { get; set; } = string.Empty;
        public string Motivo { get; set; } = string.Empty;
        public decimal Monto { get; set; }
        public bool ExtornaMercaderiaKardex { get; set; }
    }
}