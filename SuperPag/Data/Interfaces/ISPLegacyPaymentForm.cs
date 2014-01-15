using System;
using SuperPag.Framework.Data.Components.AutoDataLayer.DataInterfaceAttributes;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Framework.Data.Mapping;
using SuperPag.Data.Messages;


namespace SuperPag.Data.Interfaces
{
	[DefaultDataMessage(typeof(DSPLegacyPaymentForm))]
	public interface ISPLegacyPaymentForm
	{

        [ MethodType ( MethodTypes.Query ) ]
        DSPLegacyPaymentForm[] List();

        [ MethodType ( MethodTypes.Query ) ]
        DSPLegacyPaymentForm Locate(int paymentFormId, int storeId);

        [ MethodType ( MethodTypes.Insert ) ]
		void Insert(DSPLegacyPaymentForm dSPLegacyPaymentForm);

		[ MethodType ( MethodTypes.Update ) ]
		void Update(DSPLegacyPaymentForm dSPLegacyPaymentForm);

		[ MethodType ( MethodTypes.Delete ) ]
		void Delete(int paymentFormId);


	}
}