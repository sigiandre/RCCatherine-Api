namespace RCCatherineApi.Models
{
    public class DireccionCliente
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public Cliente? Cliente { get; set; }

        // Direccion Fiscal | Almacén Principal | Sucursal 1
        public string Etiqueta { get; set; } = "Principal";
        public string Direccion { get; set; } = string.Empty;
        public string? Ubigeo { get; set; }
        public string? Referencia { get; set; }
        public bool EsDireccionFiscal { get; set; }
        public bool Activo { get; set; } = true;
    }
}