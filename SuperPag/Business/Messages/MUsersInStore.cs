using System;
using SuperPag.Framework.Data.Components;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Data.Messages;
using SuperPag.Data;
using SuperPag.Framework;

namespace SuperPag.Business.Messages
{
    [DefaultMapping(typeof(DUsersInStore))]
    [Serializable()]
    public class MUsersInStore : Message
    {
        public MUsersInStore() { }

        private Guid _UserId;
        [Mapping(DUsersInStore.Fields.UserId)]
        public Guid UserId
        {
            get { return _UserId; }
            set { _UserId = value; }
        }

        private int _storeId;
        [Mapping(DUsersInStore.Fields.storeId)]
        public int StoreId
        {
            get { return _storeId; }
            set { _storeId = value; }
        }
    }
    
    [Serializable]
    [CollectionOf(typeof(MUsersInStore))]
    public class MCUsersInStore : MessageCollection
    {
    }
}
