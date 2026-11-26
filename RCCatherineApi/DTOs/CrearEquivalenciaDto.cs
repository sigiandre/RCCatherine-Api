namespace RCCatherineApi.DTOs
{
    public class CrearEquivalenciaDto
    {
        public int ProductoId { get; set; }
        public int UnidadMedidaSunatId { get; set; }
        public decimal FactorUnidadesBase { get; set; } = 1m;
    }
}