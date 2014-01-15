using System;
using SuperPag.Framework.Data.Components.AutoDataLayer.DataInterfaceAttributes;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Framework.Data.Mapping;
using SuperPag.Data.Messages;

namespace SuperPag.Data.Interfaces
{
	[DefaultDataMessage(typeof(DPaymentForm))]
	public interface IPaymentForm
	{
        [DataRelation(typeof(DPaymentForm), "paymentFormId", typeof(DStorePaymentForm), "paymentFormId", Join.Inner)]
        [Where(0, "storeId", typeof(DStorePaymentForm), "storeId", Filter.Equal, Link.And)]
        [MethodType(MethodTypes.Query)]
        DPaymentForm[] ListFromClient(int storeId);

        [DataRelation(typeof(DPaymentForm), "paymentFormId", typeof(DStorePaymentForm), "paymentFormId", Join.Inner)]
        [Where(0, "storeId", typeof(DStorePaymentForm), "storeId", Filter.Equal, Link.And)]
        [Where(1, "name", "name", Filter.Equal, Link.And)]
        [MethodType(MethodTypes.Query)]
        DPaymentForm Locate(int storeId, string name);

        [Where(0, Block.Begin,Link.And)]
        [Where(1, "paymentFormId", "paymentFormId", Filter.NotEqual, Link.And)]
        [Where(2, Block.End, Link.And)]
        [MethodType(MethodTypes.Query)]
        DPaymentFormComplete[] ListOtherPaymentForms(int[] paymentFormId);

        [ MethodType ( MethodTypes.Query ) ]
        DPaymentFormComplete[] List();

        [ MethodType ( MethodTypes.Query ) ]
        DPaymentForm Locate(int paymentFormId);

        [ MethodType ( MethodTypes.Insert ) ]
		void Insert(DPaymentForm dPaymentForm);

		[ MethodType ( MethodTypes.Update ) ]
		void Update(DPaymentForm dPaymentForm);

		[ MethodType ( MethodTypes.Delete ) ]
		void Delete(int paymentFormId);


	}
}