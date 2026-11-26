namespace RCCatherineApi.Models
{
    public class DetalleIngreso
    {
        public int Id { get; set; }
        public int IngresoId { get; set; }
        public Ingreso? Ingreso { get; set; }

        public int ProductoId { get; set; }
        public Producto? Producto { get; set; }

        public int UnidadMedidaSunatId { get; set; }
        public UnidadMedidaSunat? UnidadMedidaSunat { get; set; }

        public decimal Cantidad { get; set; }
        public decimal FactorConversion { get; set; } = 1m;
        public decimal CostoUnitarioFob { get; set; }
        public decimal CostoLogisticoProrrateado { get; set; }
        public decimal CostoRealUnitario { get; set; }
        public decimal Subtotal { get; set; }
    }
}