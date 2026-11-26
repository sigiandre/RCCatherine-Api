namespace RCCatherineApi.Models
{
    public class Ingreso
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; } = DateTime.UtcNow;
        public string NumeroDocumento { get; set; } = string.Empty;

        // IMPORTACION | COMPRA_LOCAL | AJUSTE_MANUAL
        public string TipoIngreso { get; set; } = "COMPRA_LOCAL";

        public int ProveedorId { get; set; }
        public Proveedor? Proveedor { get; set; }

        public int TrabajadorId { get; set; }
        public Trabajador? Trabajador { get; set; }

        // Costos logísticos prorrateables (FOB va en detalle; estos se prorratean)
        public decimal FleteInternacional { get; set; }
        public decimal Seguro { get; set; }
        public decimal Aranceles { get; set; }
        public decimal GastosDesaduanaje { get; set; }

        public decimal SubtotalFob { get; set; }
        public decimal TotalCostosLogisticos { get; set; }
        public decimal TotalCostoPuestoAlmacen { get; set; }
        
        // Obligatorio para ajustes manuales/mermas
        public string? Justificacion { get; set; }
        
        // REGISTRADO | ANULADO
        public string Estado { get; set; } = "REGISTRADO";

        public ICollection<DetalleIngreso> Detalles { get; set; } = new List<DetalleIngreso>();
    }
}