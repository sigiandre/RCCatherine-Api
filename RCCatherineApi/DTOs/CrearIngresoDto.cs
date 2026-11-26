namespace RCCatherineApi.DTOs
{
    public class CrearIngresoDto
    {
        public string NumeroDocumento { get; set; } = string.Empty;
        public string TipoIngreso { get; set; } = "COMPRA_LOCAL"; // IMPORTACION | COMPRA_LOCAL
        public int ProveedorId { get; set; }

        public decimal FleteInternacional { get; set; }
        public decimal Seguro { get; set; }
        public decimal Aranceles { get; set; }
        public decimal GastosDesaduanaje { get; set; }

        public string? Justificacion { get; set; }
        public List<DetalleIngresoItemDto> Items { get; set; } = new();
    }
}