using System;
using SuperPag.Framework.Data.Components;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Data.Messages;
using SuperPag.Data;
using SuperPag.Framework;


namespace SuperPag.Business.Messages
{
    [ DefaultMapping( typeof (DOrderItem))]
  [ Serializable() ]
	public class MOrderItem : Message
	{
        public enum ItemTypes {Regular = 1, ShippingRate = 2, Discount = 3 }

		public MOrderItem() {}

		private long _orderItemId;
		[ Mapping ( DOrderItem.Fields.orderItemId ) ]
		public long OrderItemId
          {
		    get { return _orderItemId; }
		    set { _orderItemId = value; }
          }

        private ItemTypes _itemType;
		[ Mapping ( DOrderItem.Fields.itemType ) ]
        public ItemTypes ItemType
          {
		    get { return _itemType; }
		    set { _itemType = value; }
          }


		private int _itemNumber;
		[ Mapping ( DOrderItem.Fields.itemNumber ) ]
		public int ItemNumber
          {
		    get { return _itemNumber; }
		    set { _itemNumber = value; }
          }


		private string _itemCode;
		[ Mapping ( DOrderItem.Fields.itemCode ) ]
		public string ItemCode
          {
		    get { return _itemCode; }
		    set { _itemCode = value; }
          }


		private string _itemDescription;
		[ Mapping ( DOrderItem.Fields.itemDescription ) ]
		public string ItemDescription
          {
		    get { return _itemDescription; }
		    set { _itemDescription = value; }
          }


		private int _itemQuantity;
		[ Mapping ( DOrderItem.Fields.itemQuantity ) ]
		public int ItemQuantity
          {
		    get { return _itemQuantity; }
		    set { _itemQuantity = value; }
          }


		private decimal _itemValue;
		[ Mapping ( DOrderItem.Fields.itemValue ) ]
		public decimal ItemValue
          {
		    get { return _itemValue; }
		    set { _itemValue = value; }
          }


	}

  [Serializable]
    [CollectionOf(typeof(MOrderItem))]
	public class MCOrderItem : MessageCollection
	{
	}
}