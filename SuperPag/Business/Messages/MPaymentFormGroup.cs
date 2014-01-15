using System;
using SuperPag.Framework.Data.Components;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Data.Messages;
using SuperPag.Data;
using SuperPag.Framework;


namespace SuperPag.Business.Messages
{
    [ DefaultMapping( typeof (DPaymentFormGroup))]
  [ Serializable() ]
	public class MPaymentFormGroup : Message
	{
        public enum PaymentGroups
        {
            CreditCard = 1,
            BoletoBancario = 2,
            TEF = 3
        }

		public MPaymentFormGroup() {}

		private int _paymentFormGroupId;
		[ Mapping ( DPaymentFormGroup.Fields.paymentFormGroupId ) ]
		public int PaymentFormGroupId
          {
		    get { return _paymentFormGroupId; }
		    set { _paymentFormGroupId = value; }
          }


		private string _name;
		[ Mapping ( DPaymentFormGroup.Fields.name ) ]
		public string Name
          {
		    get { return _name; }
		    set { _name = value; }
          }


	}

  [Serializable]
    [CollectionOf(typeof(MPaymentFormGroup))]
	public class MCPaymentFormGroup : MessageCollection
	{
	}
}