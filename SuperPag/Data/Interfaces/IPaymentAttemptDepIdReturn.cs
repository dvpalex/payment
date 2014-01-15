using System;
using SuperPag.Framework.Data.Components.AutoDataLayer.DataInterfaceAttributes;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Framework.Data.Mapping;
using SuperPag.Data.Messages;

namespace SuperPag.Data.Interfaces
{
    [DefaultDataMessage(typeof(DPaymentAttemptDepIdReturn))]
    public interface IPaymentAttemptDepIdReturn
    {
        [MethodType(MethodTypes.Query)]
        DPaymentAttemptDepIdReturn[] List(string remetente_deposito);

        [MethodType(MethodTypes.Query)]
        DPaymentAttemptDepIdReturn Locate(int paymentAttemptDepIdReturnId);

        [MethodType(MethodTypes.Insert)]
        void Insert(DPaymentAttemptDepIdReturn dPaymentAttemptDepIdReturn);
    }
}
