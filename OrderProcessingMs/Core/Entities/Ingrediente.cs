using System;
using System.Collections.Generic;

namespace Core.Entities;

public partial class Ingrediente
{
    public int IdIngrediente { get; set; }

    public string Nome { get; set; } = null!;

    public decimal? PrecoAdicional { get; set; }
}
