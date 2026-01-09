namespace Core.Dtos
{
    public class FullComboDto
    {
        public int? IdLanche { get; set; }
        public string? NomeLanche { get; set; }
        public int? IdAcompanhamento { get; set; }
        public string? NomeAcompanhamento { get; set; }
        public int? IdBebida { get; set; }
        public string NomeBebida { get; set; }
        public int? IdSobremesa { get; set; }
        public string NomeSobremesa { get; set; }
        public List<IngredienteLancheDto>? AlteracoesIngrediente { get; set; }
        public bool HasAlteracoesIngrediente { get => AlteracoesIngrediente != null && AlteracoesIngrediente.Any(); }
    }
}
