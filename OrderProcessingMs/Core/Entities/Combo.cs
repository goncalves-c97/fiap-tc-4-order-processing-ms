using Core.Entities;
using System;
using System.Collections.Generic;

namespace Core.Entities;

public partial class Combo
{
    public int IdCombo { get; set; }

    public int? IdLanche { get; set; }

    public int? IdAcompanhamento { get; set; }

    public int? IdBebida { get; set; }

    public int? IdSobremesa { get; set; }

    public virtual Acompanhamento? IdAcompanhamentoNavigation { get; set; }

    public virtual Bebida? IdBebidaNavigation { get; set; }

    public virtual Lanche? IdLancheNavigation { get; set; }

    public virtual Sobremesa? IdSobremesaNavigation { get; set; }

    public virtual ICollection<AlteracaoIngredienteCombo> AlteracoesIngredientes { get; set; } = [];
}
