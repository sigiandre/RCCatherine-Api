namespace RCCatherineApi.Models
{
    public class UnidadMedidaSunat
    {
        public int Id { get; set; }

        // Códigos SUNAT Catálogo N° 03: NIU, BX, PK, BG, DZN, GRO, MLL, KGM, CJ, etc.
        public string CodigoSunat { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public bool Activo { get; set; } = true;

        public ICollection<EquivalenciaProducto> Equivalencias { get; set; } = new List<EquivalenciaProducto>();
    }
}