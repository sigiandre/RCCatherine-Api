namespace RCCatherineApi.DTOs
{
    public class DetalleIngresoItemDto
    {
        public int ProductoId { get; set; }
        public int UnidadMedidaSunatId { get; set; }
        public decimal Cantidad { get; set; }
        public decimal CostoUnitarioFob { get; set; }
    }
}