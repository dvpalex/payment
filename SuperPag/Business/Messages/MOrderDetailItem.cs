using System;
using SuperPag.Framework.Data.Components;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Data.Messages;
using SuperPag.Data;
using SuperPag.Framework;
using SuperPag;


namespace SuperPag.Business.Messages
{
    [DefaultMapping(typeof(DOrder))]
    [Serializable()]
    public class MOrderDetailItem : MOrder
    {
        private string _itemsDesc;
        public string ItemsDesc
        {
            get { return _itemsDesc; }
            set { _itemsDesc = value; }
        }
    }

    [Serializable]
    [CollectionOf(typeof(MOrderDetailItem))]
    public class MCOrderDetailItem : MessageCollection
    {
    }
}
