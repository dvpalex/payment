using System;
using SuperPag.Framework.Data.Components.AutoDataLayer.DataInterfaceAttributes;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Framework.Data.Mapping;
using SuperPag.Data.Messages;


namespace SuperPag.Data.Interfaces
{
    [DefaultDataMessage(typeof(DSPLegacyPaymentGroup))]
    public interface ISPLegacyPaymentGroup
	{
        [ MethodType ( MethodTypes.Query ) ]
        DSPLegacyPaymentGroup Locate(int paymentFormGroupId,int storeId);
	}
}