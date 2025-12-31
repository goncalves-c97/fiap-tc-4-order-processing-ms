using System.Runtime.Serialization;

namespace Core.Enums
{
    public enum CategoriaProdutoEnum
    {
        [EnumMember(Value = "Acompanhamentos")]
        Acompanhamento = 1,
        [EnumMember(Value = "Bebidas")]
        Bebida = 2,
        [EnumMember(Value = "Lanches")]
        Lanche = 3,
        [EnumMember(Value = "Sobremesas")]
        Sobremesa = 4,
        [EnumMember(Value = "Ingredientes")]
        Ingrediente = 5
    }
}
