namespace Core.Entities;

public partial class Bebida
{
    public int IdBebida { get; set; }

    public string Nome { get; set; } = null!;

    public decimal Preco { get; set; }

    public virtual ICollection<Combo> Combos { get; set; } = new List<Combo>();
}
