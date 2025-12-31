namespace Core.Entities;

public partial class AlteracaoIngredienteCombo
{
    public int IdCombo { get; set; }

    public int IdIngrediente { get; set; }

    public int Quantidade { get; set; }

    public virtual Combo IdComboNavigation { get; set; } = null!;

    public virtual Ingrediente IdIngredienteNavigation { get; set; } = null!;
}
