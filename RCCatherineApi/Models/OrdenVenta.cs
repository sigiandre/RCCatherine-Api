namespace RCCatherineApi.Models
{
    public class OrdenVenta
    {
        public int Id { get; set; }
        public string NumeroOrden { get; set; } = string.Empty; // Ej. OV-0001
        public DateTime Fecha { get; set; } = DateTime.UtcNow;

        public int? CotizacionId { get; set; }
        public Cotizacion? Cotizacion { get; set; }

        public int ClienteId { get; set; }
        public Cliente? Cliente { get; set; }

        public int? DireccionEntregaId { get; set; }
        public DireccionCliente? DireccionEntrega { get; set; }

        public int VendedorId { get; set; }
        public Trabajador? Vendedor { get; set; }

        // RESERVADA | FACTURADA | DESPACHADA | ANULADA
        public string Estado { get; set; } = "RESERVADA";

        public decimal Subtotal { get; set; }
        public decimal Igv { get; set; }
        public decimal Total { get; set; }
        public decimal MontoPagado { get; set; }
        public decimal SaldoPendiente { get; set; }

        // Tracking o clave de recojo en agencia de transporte
        public string? NumeroGuiaAgencia { get; set; }

        public ICollection<DetalleOrdenVenta> Detalles { get; set; } = new List<DetalleOrdenVenta>();
        public ICollection<PagoVenta> Pagos { get; set; } = new List<PagoVenta>();
        public ComprobanteElectronico? ComprobanteElectronico { get; set; }
        public GuiaRemision? GuiaRemision { get; set; }
    }
}