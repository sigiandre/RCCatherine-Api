namespace RCCatherineApi.Models
{
    public class GuiaRemision
    {
        public int Id { get; set; }
        public int OrdenVentaId { get; set; }
        public OrdenVenta? OrdenVenta { get; set; }

        public string Serie { get; set; } = "T001";
        public string Correlativo { get; set; } = string.Empty;
        public DateTime FechaEmision { get; set; } = DateTime.UtcNow;
        public DateTime FechaTraslado { get; set; } = DateTime.UtcNow;

        // Datos SUNAT obligatorios para GRE
        public string PuntoPartida { get; set; } = string.Empty;
        public string PuntoLlegada { get; set; } = string.Empty;
        public string MotivoTraslado { get; set; } = "VENTA";
        public int NumeroBultos { get; set; } = 1;
        public decimal PesoBrutoTotalKg { get; set; }

        // PUBLICO | PRIVADO
        public string ModalidadTransporte { get; set; } = "PUBLICO";

        // Transporte público (Agencia de Transporte)
        public int? AgenciaTransporteId { get; set; }
        public AgenciaTransporte? AgenciaTransporte { get; set; }

        // Transporte privado (RENIEC / Vehículo)
        public string? PlacaVehiculo { get; set; }
        public string? LicenciaConductor { get; set; }
        public string? DniConductor { get; set; }
        public string? NombresApellidosConductor { get; set; }

        public string EstadoSunat { get; set; } = "PENDIENTE_ENVIO";
        public string? CodigoCdr { get; set; }
    }
}