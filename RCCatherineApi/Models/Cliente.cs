namespace RCCatherineApi.Models
{
    public class Cliente
    {
        public int Id { get; set; }

        // RUC | DNI | CE | PASAPORTE
        public string TipoDocumento { get; set; } = "DNI";
        public string NumeroDocumento { get; set; } = string.Empty;
        public string NombreRazonSocial { get; set; } = string.Empty;
        public string? DireccionFiscal { get; set; }
        public string? Telefono { get; set; }
        public string? Correo { get; set; }
        public bool Activo { get; set; } = true;

        public ICollection<DireccionCliente> Direcciones { get; set; } = new List<DireccionCliente>();
        public ICollection<Cotizacion> Cotizaciones { get; set; } = new List<Cotizacion>();
        public ICollection<OrdenVenta> OrdenesVenta { get; set; } = new List<OrdenVenta>();
    }
}