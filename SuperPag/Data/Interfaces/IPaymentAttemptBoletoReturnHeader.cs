using System;
using SuperPag.Framework.Data.Components.AutoDataLayer.DataInterfaceAttributes;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Framework.Data.Mapping;
using SuperPag.Data.Messages;

namespace SuperPag.Data.Interfaces
{
    [DefaultDataMessage(typeof(DPaymentAttemptBoletoReturnHeader))]
    public interface IPaymentAttemptBoletoReturnHeader
    {
//        [MethodType(MethodTypes.Query)]
//        DPaymentAttemptBoletoReturnHeader[] List(int storeId);
        
        [MethodType(MethodTypes.Query)]
        DPaymentAttemptBoletoReturnHeader Locate(int bankNumber, int sequencialReturnNumber, int companyCode);

        [MethodType(MethodTypes.Query)]
        DPaymentAttemptBoletoReturnHeader Locate(string nameOfCapturedFile);

        [MethodType(MethodTypes.Query)]
        DPaymentAttemptBoletoReturnHeader Locate(int headerId);

        [MethodType(MethodTypes.Insert)]
        void Insert(DPaymentAttemptBoletoReturnHeader dPaymentAttemptBoletoReturnHeader);
    }
}
