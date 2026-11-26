namespace RCCatherineApi.DTOs
{
    public class EmitirGuiaRemisionDto
    {
        public int OrdenVentaId { get; set; }
        public DateTime FechaTraslado { get; set; } = DateTime.UtcNow;
        public string PuntoPartida { get; set; } = string.Empty;
        public string PuntoLlegada { get; set; } = string.Empty;
        public string MotivoTraslado { get; set; } = "VENTA";
        public int NumeroBultos { get; set; } = 1;
        public decimal PesoBrutoTotalKg { get; set; }
        public string ModalidadTransporte { get; set; } = "PUBLICO"; // PUBLICO | PRIVADO
        public int? AgenciaTransporteId { get; set; }
        public string? PlacaVehiculo { get; set; }
        public string? LicenciaConductor { get; set; }
        public string? DniConductor { get; set; }
        public string? NombresApellidosConductor { get; set; }
    }
}