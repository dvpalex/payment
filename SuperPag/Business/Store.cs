using System;
using System.Collections.Generic;
using System.Text;
using SuperPag.Business.Messages;
using SuperPag.Data.Messages;
using SuperPag.Data;
using SuperPag.Framework;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;

namespace SuperPag.Business
{
    public class Store
    {
        public Store()
        {
        }

        public static MCStore List(int[] storeId)
        {
            MCStore mcStore = null;
            DStore[] arrDStore = DataFactory.Store().List(storeId);

            if (arrDStore != null)
            {
                MessageMapper mapper = new MessageMapper();
                mcStore = (MCStore)mapper.Do(arrDStore, typeof(MCStore));
            }
            else
                mcStore = new MCStore();

            return mcStore;

        }

        public static MStore Locate(int storeId)
        {
            DStore dStore = DataFactory.Store().Locate(storeId);

            MessageMapper mapper = new MessageMapper();
            MStore mStore = (MStore)mapper.Do(dStore, typeof(MStore));

            mStore.StorePaymentForms = StorePaymentForm.ListByStore(storeId);
            mStore.HandshakeConfiguration = HandshakeConfiguration.Locate(mStore.HandshakeConfigurationId);
            return mStore;
        }

        public static MStore LocateStore(Guid UserId)
        {
            Database db = DatabaseFactory.CreateDatabase("fastpag");
            DbCommand dbCommand = db.GetStoredProcCommand("Proc_LocateStore");
            db.AddInParameter(dbCommand, "@UserId", DbType.Guid, UserId);

            MStore ObjMStore = null;

            //Popula dr com o return do banco
            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {
                if (dr.Read())
                {
                    ObjMStore = new MStore(dr.GetInt32(dr.GetOrdinal("storeId")), dr.GetString(dr.GetOrdinal("name")), dr.GetString(dr.GetOrdinal("storeKey")));
                }
            }
            return ObjMStore;
        }

        public static DStore GetStore(string StoreKey)
        {
            Database db = DatabaseFactory.CreateDatabase("fastpag");
            DbCommand dbCommand = db.GetStoredProcCommand("Proc_GetStore");
            db.AddInParameter(dbCommand, "@StoreKey", DbType.String, StoreKey);

            DStore ObjDStore = new DStore();

            //Popula dr com o return do banco
            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {
                if (dr.Read())
                {

                    ObjDStore.storeId = dr.GetInt32(dr.GetOrdinal("storeId"));
                    ObjDStore.name = dr.GetString(dr.GetOrdinal("name"));
                    if (!dr.IsDBNull(dr.GetOrdinal("urlSite")))
                        ObjDStore.urlSite = dr.GetString(dr.GetOrdinal("urlSite"));
                    ObjDStore.storeKey = dr.GetString(dr.GetOrdinal("storeKey"));
                    if (!dr.IsDBNull(dr.GetOrdinal("password")))
                        ObjDStore.password = dr.GetString(dr.GetOrdinal("password"));
                    if (!dr.IsDBNull(dr.GetOrdinal("mailSenderEmail")))
                        ObjDStore.mailSenderEmail = dr.GetString(dr.GetOrdinal("mailSenderEmail"));
                    if (!dr.IsDBNull(dr.GetOrdinal("handshakeConfigurationId")))
                        ObjDStore.handshakeConfigurationId = dr.GetInt32(dr.GetOrdinal("handshakeConfigurationId"));
                    if (!dr.IsDBNull(dr.GetOrdinal("creationDate")))
                        ObjDStore.creationDate = dr.GetDateTime(dr.GetOrdinal("creationDate"));
                    if (!dr.IsDBNull(dr.GetOrdinal("lastUpdate")))
                        ObjDStore.lastUpdate = dr.GetDateTime(dr.GetOrdinal("lastUpdate"));
                }
            }
            return ObjDStore;
        }

        public static int SelectMaxId()
        {
            DStoreMaxId dStoreMaxId = DataFactory.Store().MaxId();
            return dStoreMaxId.storeId;
        }

        public static void Save(MStore mStore)
        {
            if (mStore.IsNew)
            {
                mStore.CreationDate = mStore.LastUpdate = DateTime.Now;
                MessageMapper mapper = new MessageMapper();
                DStore dStore = (DStore)mapper.Do(mStore, typeof(DStore));
                DataFactory.Store().Insert(dStore);
            }
            else
            {
                mStore.LastUpdate = DateTime.Now;
                MessageMapper mapper = new MessageMapper();
                DStore dStore = (DStore)mapper.Do(mStore, typeof(DStore));
                DataFactory.Store().Update(dStore);
            }
        }

        public static IList<MStore> GetStoreAll()
        {
            Database db = DatabaseFactory.CreateDatabase("fastpag");
            DbCommand dbCommand = db.GetStoredProcCommand("Proc_GetStoreAll");
            IList<MStore> ObjLista = new List<MStore>();

            //Popula dr com o return do banco
            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {
                while (dr.Read())
                    //Cria uma instância de "MStore", vai variando as propriedades desta instância conforme dados do dr
                    //vai add os itens no ObjLista
                    ObjLista.Add(new MStore(dr.GetInt32(dr.GetOrdinal("storeId")), dr.GetString(dr.GetOrdinal("name"))));
            }
            return ObjLista;
        }


        public static void Save(Guid UserId, int storeId)
        {
            Database db = DatabaseFactory.CreateDatabase("fastpag");
            DbCommand dbCommand = db.GetStoredProcCommand("Proc_UsersInStoreINSERT");
            db.AddInParameter(dbCommand, "@UserId", DbType.Guid, UserId);
            db.AddInParameter(dbCommand, "@storeId", DbType.Int32, storeId);
            db.ExecuteNonQuery(dbCommand);
        }

        //Este método retorna o ID da Store do usuário logado atualmente no sistema.
        public static int Locate(Guid UserId)
        {
            Database db = DatabaseFactory.CreateDatabase("fastpag");
            DbCommand dbCommand = db.GetStoredProcCommand("Proc_StoreSELECT");
            db.AddInParameter(dbCommand, "@UserId", DbType.Guid, UserId);

            int Key = (int)db.ExecuteScalar(dbCommand);

            return Key;
        }
    }
}
