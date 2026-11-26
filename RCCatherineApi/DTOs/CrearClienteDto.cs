namespace RCCatherineApi.DTOs
{
    public class CrearClienteDto
    {
        public string TipoDocumento { get; set; } = "DNI"; // RUC | DNI | CE | PASAPORTE
        public string NumeroDocumento { get; set; } = string.Empty;
        public string NombreRazonSocial { get; set; } = string.Empty;
        public string? DireccionFiscal { get; set; }
        public string? Telefono { get; set; }
        public string? Correo { get; set; }
        public List<CrearDireccionClienteDto> DireccionesAdicionales { get; set; } = new();
    }
}