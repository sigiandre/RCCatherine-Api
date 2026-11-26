namespace RCCatherineApi.Models
{
    public class Proveedor
    {
        public int Id { get; set; }
        public string Ruc { get; set; } = string.Empty;
        public string RazonSocial { get; set; } = string.Empty;
        public string? Direccion { get; set; }
        public string? Telefono { get; set; }
        public string? Contacto { get; set; }
        public bool EsInternacional { get; set; }
        public bool Activo { get; set; }

        public ICollection<Ingreso> Ingresos { get; set; } = new List<Ingreso>();
    }
}