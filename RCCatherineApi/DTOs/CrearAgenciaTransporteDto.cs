namespace RCCatherineApi.DTOs
{
    public class CrearAgenciaTransporteDto
    {
        public string Ruc { get; set; } = string.Empty;
        public string RazonSocial { get; set; } = string.Empty;
        public string DireccionAgencia { get; set; } = string.Empty;
        public string? Telefono { get; set; }
    }
}