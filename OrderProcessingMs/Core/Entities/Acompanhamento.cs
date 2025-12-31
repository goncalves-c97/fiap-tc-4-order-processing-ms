namespace Core.Entities;

public partial class Acompanhamento
{
    public int IdAcompanhamento { get; set; }

    public string Nome { get; set; } = null!;

    public decimal Preco { get; set; }

    public virtual ICollection<Combo> Combos { get; set; } = [];
}
