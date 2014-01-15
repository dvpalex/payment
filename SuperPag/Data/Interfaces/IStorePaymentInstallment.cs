using System;
using SuperPag.Framework.Data.Components.AutoDataLayer.DataInterfaceAttributes;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Framework.Data.Mapping;
using SuperPag.Data.Messages;

namespace SuperPag.Data.Interfaces
{
	[DefaultDataMessage(typeof(DStorePaymentInstallment))]
	public interface IStorePaymentInstallment
	{

        [ MethodType ( MethodTypes.Query ) ]
        DStorePaymentInstallment[] List();

        [MethodType(MethodTypes.Query)]
        [Where(0, "storeId", DStorePaymentInstallment.Fields.storeId, Filter.Equal, Link.And)]
        [Where(1, "paymentFormId", DStorePaymentInstallment.Fields.paymentFormId, Filter.Equal, Link.And)]
        [Where(2, "totalAmount", DStorePaymentInstallment.Fields.minValue, Filter.LessOrEqual, Link.And)]
        [Where(3, "totalAmount", DStorePaymentInstallment.Fields.maxValue, Filter.GreaterOrEqual, Link.And)]
        DStorePaymentInstallment[] List(int storeId, int paymentFormId, decimal totalAmount);

        [MethodType(MethodTypes.Query)]
        DStorePaymentInstallment[] List(int storeId, int paymentFormId);

        [MethodType(MethodTypes.Query)]
        DStorePaymentInstallment Locate(int storeId, int paymentFormId, int installmentNumber);

        [MethodType(MethodTypes.Query)]
        DStorePaymentInstallment Locate(byte installmentNumber);

        [ MethodType ( MethodTypes.Insert ) ]
		void Insert(DStorePaymentInstallment dStorePaymentInstallment);

		[ MethodType ( MethodTypes.Update ) ]
		void Update(DStorePaymentInstallment dStorePaymentInstallment);

        [MethodType(MethodTypes.Delete)]
        void Delete(int storeId, int paymentFormId, int installmentNumber);

	}
}