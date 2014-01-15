using System;
using SuperPag.Framework.Data.Components.AutoDataLayer.DataInterfaceAttributes;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Framework.Data.Mapping;
using SuperPag.Data.Messages;

namespace SuperPag.Data.Interfaces
{
    [DefaultDataMessage(typeof(DExtratoFinanceiro2))]
    public interface IExtratoFinanceiro2
	{
        [MethodType(MethodTypes.QueryProc, "ProcExtratoFinanceiro2")]
        DExtratoFinanceiro2[] List(DateTime dataInicial, DateTime dataFinal, int storeId);
	}
}
