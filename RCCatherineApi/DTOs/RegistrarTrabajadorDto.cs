namespace RCCatherineApi.DTOs
{
    public class RegistrarTrabajadorDto
    {
        public string NombreCompleto { get; set; } = string.Empty;
        public string Dni { get; set; } = string.Empty;
        public string Usuario { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Rol { get; set; } = "Vendedor";
    }
}