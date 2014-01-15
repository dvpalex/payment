using System;
using SuperPag.Framework.Data.Components.AutoDataLayer.DataInterfaceAttributes;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Framework.Data.Mapping;
using SuperPag.Data.Messages;

namespace SuperPag.Data.Interfaces
{
    [DefaultDataMessage(typeof(DPaymentAttemptBoletoReturn))]
    public interface IPaymentAttemptBoletoReturn
    {        
        [MethodType(MethodTypes.Query)]
        DPaymentAttemptBoletoReturn List();

        [MethodType(MethodTypes.Query)]
        DPaymentAttemptBoletoReturn Locate(string nossoNumero);
        
        [MethodType(MethodTypes.Insert)]
        void Insert(DPaymentAttemptBoletoReturn dPaymentAttemptBoletoReturn);
    }
}
