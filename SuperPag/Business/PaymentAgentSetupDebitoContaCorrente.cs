using System;
using System.Collections.Generic;
using System.Text;
using SuperPag.Data.Messages;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;
using System.Data.Common;
using SuperPag.Business.Messages;

namespace SuperPag.Business
{
    public class PaymentAgentSetupDebitoContaCorrente
    {
        Database db = DatabaseFactory.CreateDatabase("fastpag");
        #region Shared/Static Methods


        static PaymentAgentSetupDebitoContaCorrente ObjPaymentAgentSetupDebitoContaCorrente = null;

        static PaymentAgentSetupDebitoContaCorrente()
        {
            CreateInstance();
        }

        private static void CreateInstance()
        {
            ObjPaymentAgentSetupDebitoContaCorrente = new PaymentAgentSetupDebitoContaCorrente();
        }

        public static PaymentAgentSetupDebitoContaCorrente GetInstance()
        {
            return ObjPaymentAgentSetupDebitoContaCorrente;
        }

        #endregion

        public MPaymentAgentSetupDebitoContaCorrente Locate(int OrderId)
        {
            try
            {

                DbCommand dbCommand = db.GetStoredProcCommand("ProcPaymentAgentContaCorrente");
                db.AddInParameter(dbCommand, "@OrderId", DbType.Int32, OrderId);
                MPaymentAgentSetupDebitoContaCorrente ObjAgentSetup = null;

                using (IDataReader dr = db.ExecuteReader(dbCommand))
                {
                    if (dr.Read())
                    {
                        ObjAgentSetup = new MPaymentAgentSetupDebitoContaCorrente(dr.GetInt32(dr.GetOrdinal("PaymentAgentSetupId")),
                                                                                  dr.GetString(dr.GetOrdinal("Path")),
                                                                                  (SuperPag.BankNumber)Convert.ToInt32(dr.GetString(dr.GetOrdinal("NumBanco"))),
                                                                                  dr.GetString(dr.GetOrdinal("Layout")),
                                                                                  dr.GetString(dr.GetOrdinal("Versao")),
                                                                                  dr.GetDateTime(dr.GetOrdinal("Data")),
                                                                                  dr.GetString(dr.GetOrdinal("CodConvenio")),
                                                                                  dr.GetString(dr.GetOrdinal("NEmpresa")),
                                                                                  dr.GetString(dr.GetOrdinal("NBanco")),
                                                                                  dr.IsDBNull(dr.GetOrdinal("NumSeq")) ? 0 : dr.GetInt32(dr.GetOrdinal("NumSeq")),
                                                                                  dr.IsDBNull(dr.GetOrdinal("Carteira")) ? string.Empty : dr.GetString(dr.GetOrdinal("Carteira")),
                                                                                  dr.IsDBNull(dr.GetOrdinal("Agencia")) ? string.Empty : dr.GetString(dr.GetOrdinal("Agencia")),
                                                                                  dr.IsDBNull(dr.GetOrdinal("ContaCorrente")) ? string.Empty : dr.GetString(dr.GetOrdinal("ContaCorrente"))
                                                                                 );

                    }
                }
                return ObjAgentSetup;
            }
            catch
            {

                return null;
            }

        }
        public IList<string> GetPath(int StoreId)
        {
            try
            {
                DbCommand dbCommand = db.GetStoredProcCommand("Proc_SelectPathPaymentAgentSetupDebitoContaCorrente");
                db.AddInParameter(dbCommand, "@storeId", DbType.Int32, StoreId);
                IList<string> ObjPath = new List<string>();

                using (IDataReader dr = db.ExecuteReader(dbCommand))
                {
                    while (dr.Read())
                    {
                        ObjPath.Add(dr.GetString(dr.GetOrdinal("Path")));
                    }
                }
                return ObjPath;
            }
            catch
            {

                return null;
            }
        }


        public static IList<MPaymentAgentSetupDebitoContaCorrente> ListadeBancos(String Cliente)
        {
            try
            {
                Database db = DatabaseFactory.CreateDatabase("fastpag");
                IList<MPaymentAgentSetupDebitoContaCorrente> ObjLista = new List<MPaymentAgentSetupDebitoContaCorrente>();
                DbCommand dbCommand = db.GetStoredProcCommand("Proc_Bancos");
                db.AddInParameter(dbCommand, "@Cliente", DbType.String, Cliente);

                using (IDataReader dr = db.ExecuteReader(dbCommand))
                {
                    if (dr!=null)
                    {
                        ObjLista.Add(new MPaymentAgentSetupDebitoContaCorrente(0, "Todos"));
                        while (dr.Read())
                        {
                            ObjLista.Add(new MPaymentAgentSetupDebitoContaCorrente( 
                                                                                     Convert.ToInt32(dr.GetString(dr.GetOrdinal("numBanco"))),
                                                                                     dr.GetString(dr.GetOrdinal("NBanco"))
                                                                                    ));

                        }
                    }
                }
                return  ObjLista;;
            }
            catch
            {

                return null;
            }

        }

        public static IList<MStatusSuperPag> getRecuperaStatus()
        {
            //Cria uma lista de Objetos da classe Entidade
            IList<MStatusSuperPag> ObjLista = new List<MStatusSuperPag>();

            Database db = DatabaseFactory.CreateDatabase("fastpag");
            DbCommand dbCommand = db.GetStoredProcCommand("Proc_GetStatusPerObject");
            db.AddInParameter(dbCommand, "@Object", DbType.String, "PaymentAgentSetupDebitoContaCorrente");

            //    //Popula dr
            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {
                if (dr != null)
                {
                    while (dr.Read())
                    {
                        //Popula ObjLista
                        ObjLista.Add(new MStatusSuperPag
                                                (
                                                        Convert.ToInt32(dr["IdStatus"].ToString()),
                                                        dr["NmStatus"].ToString(),
                                                        dr["DsStatus"].ToString(),
                                                        Convert.ToInt32(dr["IdWorkflowWeiseIT"].ToString())


                                                )
                                   );
                    }
                }
            }
            return ObjLista;

        }

        public static IList<MOcorrencias> getRecuperaOcorrencias()
        {
            //Cria uma lista de Objetos da classe Entidade
            IList<MOcorrencias> ObjLista = new List<MOcorrencias>();

            Database db = DatabaseFactory.CreateDatabase("fastpag");
            DbCommand dbCommand = db.GetStoredProcCommand("Proc_GetStatusPerOcorrencias");

            //    //Popula dr
            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {
                if (dr != null)
                {
                    ObjLista.Add(new MOcorrencias("", "Todas"));
                    while (dr.Read())
                    {
                        //Popula ObjLista
                        ObjLista.Add(new MOcorrencias
                                                (
                                                        dr["IdOcorrencia"].ToString(),
                                                        dr["DscOcorrencia"].ToString()


                                                )
                                   );
                    }
                }
            }
            return ObjLista;

        }


    }
}
