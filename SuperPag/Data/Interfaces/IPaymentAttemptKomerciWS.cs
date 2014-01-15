using System;
using SuperPag.Framework.Data.Components.AutoDataLayer.DataInterfaceAttributes;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Framework.Data.Mapping;
using SuperPag.Data.Messages;

namespace SuperPag.Data.Interfaces
{
    [DefaultDataMessage(typeof(DPaymentAttemptKomerciWS))]
    public interface IPaymentAttemptKomerciWS
    {
        [MethodType(MethodTypes.Query)]
        DPaymentAttemptKomerciWS[] List();

        [MethodType(MethodTypes.Query)]
        DPaymentAttemptKomerciWS Locate(Guid paymentAttemptId);

        [MethodType(MethodTypes.Insert)]
        void Insert(DPaymentAttemptKomerciWS dPaymentAttemptKomerciWS);

        [MethodType(MethodTypes.Update)]
        void Update(DPaymentAttemptKomerciWS dPaymentAttemptKomerciWS);

        [MethodType(MethodTypes.Delete)]
        void Delete(Guid paymentAttemptId);
    }
}
