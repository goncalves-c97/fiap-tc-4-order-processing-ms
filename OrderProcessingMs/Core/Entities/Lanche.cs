using System;
using System.Collections.Generic;

namespace Core.Entities;

public partial class Lanche
{
    public int IdLanche { get; set; }

    public string Nome { get; set; } = null!;

    public decimal Preco { get; set; }

    public virtual ICollection<Combo> Combos { get; set; } = [];

    public virtual ICollection<IngredientesLanche> IngredientesLanche { get; set; } = [];
}
