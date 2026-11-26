namespace RCCatherineApi.Models
{
    public class PagoVenta
    {
        public int Id { get; set; }
        public int OrdenVentaId { get; set; }
        public OrdenVenta? OrdenVenta { get; set; }

        public DateTime FechaPago { get; set; } = DateTime.UtcNow;

        // EFECTIVO | YAPE | PLIN | TRANSFERENCIA | TARJETA
        public string MetodoPago { get; set; } = "EFECTIVO";
        public decimal Monto { get; set; }
        public string? NumeroOperacion { get; set; }
        public int RegistradoPorId { get; set; }
        public Trabajador? RegistradoPor { get; set; }
    }
}