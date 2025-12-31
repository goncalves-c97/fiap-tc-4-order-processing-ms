namespace Core.Dtos
{
    public class LancheComIngredientesDto
    {
        public int IdLanche { get; set; }
        public string Nome { get; set; }
        public decimal Preco { get; set; }
        public List<IngredienteLancheDto> IngredientesLanche { get; set; }
    }
}
