namespace RCCatherineApi.DTOs
{
    public class RegistrarPagoItemDto
    {
        public string MetodoPago { get; set; } = "EFECTIVO"; // EFECTIVO | YAPE | PLIN | TRANSFERENCIA | TARJETA
        public decimal Monto { get; set; }
        public string? NumeroOperacion { get; set; }
    }
}