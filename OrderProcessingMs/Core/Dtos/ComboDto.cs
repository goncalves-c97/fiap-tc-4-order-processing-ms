namespace Core.Dtos
{
    public class ComboDto
    {
        public int? IdLanche { get; set; }
        public int? IdAcompanhamento { get; set; }
        public int? IdBebida { get; set; }
        public int? IdSobremesa { get; set; }
        public List<IngredienteLancheDto>? AlteracoesIngrediente { get; set; }
        public bool HasAlteracoesIngrediente { get => AlteracoesIngrediente != null && AlteracoesIngrediente.Count != 0; }
    }
}
