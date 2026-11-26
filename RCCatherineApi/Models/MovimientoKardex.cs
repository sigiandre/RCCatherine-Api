namespace RCCatherineApi.Models
{
    public class MovimientoKardex
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; } = DateTime.UtcNow;

        public int ProductoId { get; set; }
        public Producto? Producto { get; set; }

        public int TrabajadorId { get; set; }
        public Trabajador? Trabajador { get; set; }

        // INGRESO | SALIDA | AJUSTE | DEVOLUCION
        public string TipoMovimiento { get; set; } = "INGRESO";
        public string ReferenciaDocumento { get; set; } = string.Empty;
        public string? Observacion { get; set; }

        // Positivo ingreso/devolución, negativo salida
        public decimal CantidadUnidadesBase { get; set; }
        public decimal CostoUnitario { get; set; }
        public decimal CostoPromedioResultante { get; set; }
        public decimal SaldoCantidad { get; set; }
        public decimal SaldoValorizado { get; set; }
    }
}