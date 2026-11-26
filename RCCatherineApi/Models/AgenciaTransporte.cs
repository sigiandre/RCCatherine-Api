namespace RCCatherineApi.Models
{
    public class AgenciaTransporte
    {
        public int Id { get; set; }
        public string Ruc { get; set; } = string.Empty;
        public string RazonSocial { get; set; } = string.Empty;
        public string DireccionAgencia { get; set; } = string.Empty;
        public string? Telefono { get; set; }
        public bool Activo { get; set; } = true;

        public ICollection<GuiaRemision> GuiasRemision { get; set; } = new List<GuiaRemision>();
    }
}