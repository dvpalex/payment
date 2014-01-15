using System;
using SuperPag.Framework.Data.Components.AutoDataLayer.DataInterfaceAttributes;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Framework.Data.Mapping;
using SuperPag.Data.Messages;

namespace SuperPag.Data.Interfaces
{
    [DefaultDataMessage(typeof(DPaymentSummary))]
	public interface IPaymentSummary
	{
        [MethodType(MethodTypes.QueryProc, "ProcPaymentSummary")]
        DPaymentSummary[] List(int storeId, DateTime DataInicial, DateTime DataFinal);
	}
}
