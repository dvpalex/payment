using System;
using SuperPag.Framework.Data.Components.AutoDataLayer.DataInterfaceAttributes;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Framework.Data.Mapping;
using SuperPag.Data.Messages;

namespace SuperPag.Data.Interfaces
{
	[DefaultDataMessage(typeof(DPaymentFormGroup))]
	public interface IPaymentFormGroup
	{

        [ MethodType ( MethodTypes.Query ) ]
        DPaymentFormGroup[] List();

        [ MethodType ( MethodTypes.Query ) ]
        DPaymentFormGroup Locate(byte paymentFormGroupId);

        [ MethodType ( MethodTypes.Insert ) ]
		void Insert(DPaymentFormGroup dPaymentFormGroup);

		[ MethodType ( MethodTypes.Update ) ]
		void Update(DPaymentFormGroup dPaymentFormGroup);

		[ MethodType ( MethodTypes.Delete ) ]
		void Delete(byte paymentFormGroupId);


	}
}