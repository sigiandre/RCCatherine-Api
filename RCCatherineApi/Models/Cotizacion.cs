namespace RCCatherineApi.Models
{
    public class Cotizacion
    {
        public int Id { get; set; }

        // Ej. COT-0001
        public string Codigo { get; set; } = string.Empty;
        public DateTime FechaEmision { get; set; } = DateTime.UtcNow;
        public DateTime FechaVencimiento { get; set; }
        public int DiasVigencia { get; set; } = 7;

        // Pendiente | Aprobada | Rechazada | Convertida
        public string Estado { get; set; } = "Pendiente";

        public int ClienteId { get; set; }
        public Cliente? Cliente { get; set; }

        public int? DireccionEntregaId { get; set; }
        public DireccionCliente? DireccionEntrega { get; set; }

        public int VendedorId { get; set; }
        public Trabajador? Vendedor { get; set; }

        public decimal Subtotal { get; set; }
        public decimal Igv { get; set; }
        public decimal Total { get; set; }
        public string? Observaciones { get; set; }

        public ICollection<DetalleCotizacion> Detalles { get; set; } = new List<DetalleCotizacion>();
        public OrdenVenta? OrdenVenta { get; set; }
    }
}