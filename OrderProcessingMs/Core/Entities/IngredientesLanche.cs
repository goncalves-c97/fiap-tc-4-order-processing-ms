using System;
using System.Collections.Generic;

namespace Core.Entities;

public partial class IngredientesLanche
{
    public int IdLanche { get; set; }

    public int IdIngrediente { get; set; }

    public int Quantidade { get; set; }

    public virtual Ingrediente IdIngredienteNavigation { get; set; } = null!;

    public virtual Lanche IdLancheNavigation { get; set; } = null!;
}
