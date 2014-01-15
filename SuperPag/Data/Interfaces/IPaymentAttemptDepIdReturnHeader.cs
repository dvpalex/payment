using System;
using SuperPag.Framework.Data.Components.AutoDataLayer.DataInterfaceAttributes;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Framework.Data.Mapping;
using SuperPag.Data.Messages;

namespace SuperPag.Data.Interfaces
{
    [DefaultDataMessage(typeof(DPaymentAttemptDepIdReturnHeader))]
    public interface IPaymentAttemptDepIdReturnHeader
    {
        //        [MethodType(MethodTypes.Query)]
        //        DPaymentAttemptBoletoReturnHeader[] List(int storeId);

        [MethodType(MethodTypes.Query)]
        DPaymentAttemptDepIdReturnHeader Locate(int bankNumber, int sequencialReturnNumber, int companyCode);

        [MethodType(MethodTypes.Query)]
        DPaymentAttemptDepIdReturnHeader Locate(string nameOfCapturedFile);

        [MethodType(MethodTypes.Query)]
        DPaymentAttemptDepIdReturnHeader Locate(int headerId);

        [MethodType(MethodTypes.Insert)]
        void Insert(DPaymentAttemptDepIdReturnHeader dPaymentAttemptDepIdReturnHeader);
    }
}
