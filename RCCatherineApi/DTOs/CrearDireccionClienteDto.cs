namespace RCCatherineApi.DTOs
{
    public class CrearDireccionClienteDto
    {
        public string Etiqueta { get; set; } = "Sucursal";
        public string Direccion { get; set; } = string.Empty;
        public string? Ubigeo { get; set; }
        public string? Referencia { get; set; }
        public bool EsDireccionFiscal { get; set; }
    }
}