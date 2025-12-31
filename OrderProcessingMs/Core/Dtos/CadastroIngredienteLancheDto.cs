namespace Core.Dtos
{
    public class CadastroIngredienteLancheDto 
    {
        public int IdLanche { get; set; }
        public int IdIngrediente { get; set; }
        public int Quantidade { get; set; }

        public CadastroIngredienteLancheDto(int idLanche, int idIngrediente, int quantidade)
        {
            IdLanche = idLanche;
            IdIngrediente = idIngrediente;
            Quantidade = quantidade;
        }
    }
}
