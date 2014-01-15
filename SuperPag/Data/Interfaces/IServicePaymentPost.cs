using System;
using SuperPag.Framework.Data.Components.AutoDataLayer.DataInterfaceAttributes;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Framework.Data.Mapping;
using SuperPag.Data.Messages;

namespace SuperPag.Data.Interfaces
{
    [DefaultDataMessage(typeof(DServicePaymentPost))]
    public interface IServicePaymentPost
    {
        [MethodType(MethodTypes.Query)]
        DServicePaymentPost[] List();

        [MethodType(MethodTypes.Query)]
        DServicePaymentPost[] List(int[] postStatus);

        [MethodType(MethodTypes.QueryProc, "ProcServicePaymentPost")]
        DServicePaymentPost[] List(int storeId);

        [MethodType(MethodTypes.Query)]
        DServicePaymentPost Locate(Guid paymentAttemptId, int installmentNumber);

        [MethodType(MethodTypes.Insert)]
        void Insert(DServicePaymentPost dServicePaymentPost);

        [MethodType(MethodTypes.Update)]
        void Update(DServicePaymentPost dServicePaymentPost);

        [MethodType(MethodTypes.Delete)]
        void Delete(Guid paymentAttemptId, int installmentNumber);
    }
}
