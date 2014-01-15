using System;
using SuperPag.Framework.Data.Components.AutoDataLayer.DataInterfaceAttributes;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Framework.Data.Mapping;
using SuperPag.Data.Messages;

namespace SuperPag.Data.Interfaces
{
    [DefaultDataMessage(typeof(DPaymentAttemptDepIdReturnChk))]
    public interface IPaymentAttemptDepIdReturnChk
    {
        [MethodType(MethodTypes.Query)]
        DPaymentAttemptDepIdReturnChk Locate(int checkId);

        [MethodType(MethodTypes.Query)]
        DPaymentAttemptDepIdReturnChk[] List(int paymentAttemptDepIdReturnId);

        [MethodType(MethodTypes.Query)]
        DPaymentAttemptDepIdReturnChk[] List(int paymentAttemptDepIdReturnId, int status);

        [MethodType(MethodTypes.Insert)]
        void Insert(DPaymentAttemptDepIdReturnChk dPaymentAttemptDepIdReturnChk);

        [MethodType(MethodTypes.Update)]
        void Update(DPaymentAttemptDepIdReturnChk dPaymentAttemptDepIdReturnChk);
    }
}
