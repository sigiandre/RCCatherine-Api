namespace RCCatherineApi.Models
{
    public class Trabajador
    {
        public int Id { get; set; }
        public string NombreCompleto { get; set; } = string.Empty;
        public string Dni { get; set; } = string.Empty;
        public string Usuario { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;

        //Roles: Vendedor | Almacenero | Administrador
        public string Rol { get; set; } = "Vendedor";
        public bool Activo { get; set; } = true;

        public ICollection<Cotizacion> Cotizaciones { get; set; } = new List<Cotizacion>();
        public ICollection<OrdenVenta> OrdenesVenta { get; set; } = new List<OrdenVenta>();
        public ICollection<Ingreso> Ingresos { get; set; } = new List<Ingreso>();
        public ICollection<MovimientoKardex> MovimientosKardex { get; set; } = new List<MovimientoKardex>();
    }
}