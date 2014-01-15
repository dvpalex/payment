using System;
using SuperPag.Framework.Data.Components.AutoDataLayer.DataInterfaceAttributes;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Framework.Data.Mapping;
using SuperPag.Data.Messages;

namespace SuperPag.Data.Interfaces
{
    [DefaultDataMessage(typeof(DServiceEmailPaymentForm))]
    public interface IServiceEmailPaymentForm
    {
        [MethodType(MethodTypes.Query)]
        DServiceEmailPaymentForm[] List();

        [MethodType(MethodTypes.Query)]
        DServiceEmailPaymentForm Locate(int storeId, int emailType, int paymentFormId, string idioma);

        [MethodType(MethodTypes.Insert)]
        void Insert(DServiceEmailPaymentForm dServiceEmailPaymentForm);

        [MethodType(MethodTypes.Update)]
        void Update(DServiceEmailPaymentForm dServiceEmailPaymentForm);

        [MethodType(MethodTypes.Delete)]
        void Delete(int storeId, int emailType, int paymentFormId, string idioma);
    }
}
