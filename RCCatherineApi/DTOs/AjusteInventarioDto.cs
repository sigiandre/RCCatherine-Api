namespace RCCatherineApi.DTOs
{
    public class AjusteInventarioDto
    {
        public int ProductoId { get; set; }
        public int UnidadMedidaSunatId { get; set; }
        public decimal CantidadUnidadesBase { get; set; } // Positivo sobrante, negativo merma/daño
        public string Justificacion { get; set; } = string.Empty;
    }
}