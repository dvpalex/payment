using System;
using SuperPag.Framework.Data.Components.AutoDataLayer.DataInterfaceAttributes;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Framework.Data.Mapping;
using SuperPag.Data.Messages;

namespace SuperPag.Data.Interfaces
{
    [DefaultDataMessage(typeof(DPaymentAttemptKomerci))]
    public interface IPaymentAttemptKomerci
    {
        [MethodType(MethodTypes.Query)]
        DPaymentAttemptKomerci[] List();

        [MethodType(MethodTypes.Query)]
        DPaymentAttemptKomerci Locate(Guid paymentAttemptId);

        [MethodType(MethodTypes.Query)]
        DPaymentAttemptKomerci Locate(int agentOrderReference);
        
        [MethodType(MethodTypes.Insert)]
        void Insert(DPaymentAttemptKomerci dPaymentAttemptKomerci);

        [MethodType(MethodTypes.Update)]
        void Update(DPaymentAttemptKomerci dPaymentAttemptKomerci);

        [MethodType(MethodTypes.Delete)]
        void Delete(Guid paymentAttemptId);
    }
}
