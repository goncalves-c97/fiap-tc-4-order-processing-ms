using System;
using System.Collections.Generic;

namespace Core.Entities;

public partial class ComboPedido
{
    public int IdCombo { get; set; }

    public int IdPedido { get; set; }

    public virtual Combo IdComboNavigation { get; set; } = null!;
}
