using System;
using SuperPag.Framework.Data.Components.AutoDataLayer.DataInterfaceAttributes;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Framework.Data.Mapping;
using SuperPag.Data.Messages;

namespace SuperPag.Data.Interfaces
{
	[DefaultDataMessage(typeof(DStorePaymentForm))]
	public interface IStorePaymentForm
	{

        [ MethodType ( MethodTypes.Query ) ]
        DStorePaymentForm[] List();

        [MethodType(MethodTypes.Query)]
        DStorePaymentForm[] List(int storeId);

        [MethodType(MethodTypes.Query)]
        DStorePaymentForm[] List(int storeId, bool isActive);

        [MethodType(MethodTypes.Query)]
        DStorePaymentForm[] ListCombo(int storeId, bool showInCombo);

        [MethodType(MethodTypes.Query)]
        DStorePaymentFormComplete[] ListCompleteByStore(int storeId);

        [ MethodType ( MethodTypes.Query ) ]
        DStorePaymentForm Locate(int storeId, int paymentFormId);

        [ MethodType ( MethodTypes.Insert ) ]
		void Insert(DStorePaymentForm dStorePaymentForm);

		[ MethodType ( MethodTypes.Update ) ]
		void Update(DStorePaymentForm dStorePaymentForm);

	    [ MethodType ( MethodTypes.Delete ) ]
        void Delete(int storeId, int paymentFormId);
	}
}