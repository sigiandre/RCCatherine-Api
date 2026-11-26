namespace RCCatherineApi.Models
{
    public class EquivalenciaProducto
    {
        public int Id { get; set; }
        
        public int ProductoId { get; set; }
        public Producto? Producto { get; set; }
        
        public int UnidadMedidaSunatId { get; set; }
        public UnidadMedidaSunat? UnidadMedidaSunat { get; set; }

        public decimal FactorUnidadesBase { get; set; } = 1m;
        public bool Activo { get; set; } = true;
    }
}