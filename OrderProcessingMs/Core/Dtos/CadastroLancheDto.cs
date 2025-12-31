namespace Core.Dtos
{
    public class CadastroLancheDto
    {
        public string Nome { get; set; }
        public decimal Preco { get; set; }
        public List<IngredienteLancheDto> IngredientesLanche { get; set; }
    }
}
