using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.IO;
using SuperPag.Business;
using SuperPag.Business.Messages;
using SuperPag;
using System.Collections;

namespace RetornoDebContaCorrente
{
    class Program
    {
        public static string[] StoreId
        {
            get { return ConfigurationManager.AppSettings["StoreId"].Split(','); }
        }

        /// <summary>
        /// Diretorio onde sera disponibilizado o arquivo para CSU
        /// </summary>
        public static string Path
        {
            get { return ConfigurationManager.AppSettings["CnabDir"]; }
        }

        /// <summary>
        /// Caminho onde esta a pasta do EDI
        /// </summary>
        public static string PathContaCorrente
        {
            get { return ConfigurationManager.AppSettings["PathContaCorrente"]; }
        }

        public static string StoreIdCarrefour
        {
            get { return ConfigurationManager.AppSettings["StoreIdCarrefour"]; }
        }

        static void Main(string[] args)
        {

            MCnabControleSaida ObjMCnabControleSaida = new MCnabControleSaida();
            MCSUControleSaidaContaCorrente ObjMCSUControleSaidaContaCorrente = new MCSUControleSaidaContaCorrente();
            MPaymentAttemptContaCorrente ObjContaCorrente = new MPaymentAttemptContaCorrente();
            string ArqCSU = string.Empty;

            IList<MCnab> ListaRetHeader = new List<MCnab>();
            IList<MCnab> ListaRetDetalhe = new List<MCnab>();
            IList<MCnab> ListaRetTrailler = new List<MCnab>();
            IList<int?> ListNrDocum = new List<int?>();

            SuperPag.Helper.GenericHelper.LogFile("INICIO DE PROCESSAMENTO: Serviço de recebimento arquivo Banco.", SuperPag.LogFileEntryType.Information);

            foreach (string StrStoreId in StoreId)
            {

                foreach (string StrPath in PaymentAgentSetupDebitoContaCorrente.GetInstance().GetPath(Convert.ToInt32(StrStoreId)))
                {
                    string StrPathOutbox = PathContaCorrente + StrPath + "OUTBOX";
                    foreach (string StrArquivo in Directory.GetFiles(StrPathOutbox))
                    {
                        try
                        {
                            //valido o arquivo e retorno o numero de linhas do arquivo
                            int NumLinhas = ValidaArquivo(StrArquivo, 400);

                            //Header Retorno
                            ListaRetHeader = GetArquivo(StrArquivo, Cnab.GetCnab("400", "01", Convert.ToDateTime("8/9/2004 00:00:00"), 2, 2), null);
                            ListaRetTrailler = GetArquivo(StrArquivo, Cnab.GetCnab("400", "01", Convert.ToDateTime("8/9/2004 00:00:00"), 19, 2), NumLinhas);

                            ObjMCnabControleSaida.NumInstituicao = Convert.ToInt32(ListaRetHeader[7].Valor);
                            ObjMCnabControleSaida.DataArquivo = DateTime.Now;
                            //ObjMCnabControleSaida.NumSeqRemessa
                            ObjMCnabControleSaida.NomeArquivo = StrArquivo.Substring(StrArquivo.LastIndexOf("\\") + 1);
                            ObjMCnabControleSaida.NomeArquivoArmazenado = Guid.NewGuid().ToString() + ".cnab";
                            ObjMCnabControleSaida.QtdeDetalhes = NumLinhas - 2;
                            ObjMCnabControleSaida.Status = (int)ContaCorrenteStatus.EnviadoSuperpag;

                            ObjMCnabControleSaida.CnabControleSaidaId = CnabControleSaida.Insert(ObjMCnabControleSaida);

                            //Nome do arquivo que sera enviado a CSU
                            ArqCSU = PathContaCorrente + StrPath + "OUTBOXSUPERPAG\\" + "\\REM_" + ObjMCnabControleSaida.NumInstituicao.ToString() + "_" + DateTime.Now.ToString("dd_MM_yyyy") + "_" + ObjMCnabControleSaida.CnabControleSaidaId.ToString() + ".REM"; ;

                            for (int i = 2; i < NumLinhas; i++)
                            {
                                ListaRetDetalhe = GetArquivo(StrArquivo, Cnab.GetCnab("400", "01", Convert.ToDateTime("8/9/2004 00:00:00"), 3, 2), i);

                                ObjMCnabControleSaida.ValorTotalDetalhes += Convert.ToDecimal(ListaRetDetalhe[18].Valor.Insert(ListaRetDetalhe[18].Valor.Length - 2, ","));
                                CnabControleSaida.Update(ObjMCnabControleSaida);

                                ObjContaCorrente.Ocorrencia = ListaRetDetalhe[13].Valor;
                                ObjContaCorrente.OcorrenciaSuperpag = GetOcorrenciaSP(ListaRetDetalhe[13].Valor);
                                ObjContaCorrente.NrDocum = Convert.ToInt32(ListaRetDetalhe[5].Valor.Substring(1));
                                ObjContaCorrente.Status = GetStatus(ListaRetDetalhe[13].Valor);

                                //Atualiso a tabela de acordo com o retorno o arquivo
                                PaymentAttemptContaCorrente.GetInstance().UpdateByNrDocum(ObjContaCorrente);

                                //somente para carrefour
                                if (StrStoreId == StoreIdCarrefour && ObjContaCorrente.Status == (int)ContaCorrenteStatus.Pago)
                                {
                                    ListNrDocum.Add(ObjContaCorrente.NrDocum);
                                    ObjContaCorrente = PaymentAttemptContaCorrente.GetInstance().Locate((int)ObjContaCorrente.NrDocum);
                                    CriarArquivo(ObjContaCorrente, ObjMCnabControleSaida, ArqCSU);

                                    ObjContaCorrente.CnabControleSaidaId = ObjMCnabControleSaida.CnabControleSaidaId;
                                    ObjContaCorrente.Status = (int)ContaCorrenteStatus.Pago;
                                    PaymentAttemptContaCorrente.GetInstance().Update(ObjContaCorrente);

                                    ObjMCSUControleSaidaContaCorrente.QtdeDetalhes += 1;
                                    ObjMCSUControleSaidaContaCorrente.ValorTotalDetalhes += ObjContaCorrente.ValorAgendado;
                                }
                            }

                            //Arquiva o cnab de retorno do banco
                            string ArqArchiveCnabControleSaida = PathContaCorrente + StrPath + "Archive\\" + ObjMCnabControleSaida.NomeArquivoArmazenado;
                            //arquivo CSU para caixa de saida CSU
                            string ArqCSUPara = ConfigurationManager.AppSettings["CnabDir"] + StrArquivo.Substring(StrArquivo.LastIndexOf("\\") + 1);

                            //Arquiva o cnab de retorno do banco
                            Move(StrArquivo, ArqArchiveCnabControleSaida);
                            //move o arquivo CSU para caixa de saida CSU
                            if (File.Exists(ArqCSU))
                            {
                                Move(ArqCSU, ArqCSUPara);
                            }

                            ObjMCSUControleSaidaContaCorrente.ControleSaidaId = ObjMCnabControleSaida.CnabControleSaidaId;
                            ObjMCSUControleSaidaContaCorrente.DataGeracaoLote = DateTime.Now;
                            ObjMCSUControleSaidaContaCorrente.StatusProcessamento = (int)ContaCorrenteStatus.EnviadoCSU;
                            ObjMCSUControleSaidaContaCorrente.NomeArquivoEnviado = ArqCSUPara;
                            ObjMCSUControleSaidaContaCorrente.NomeArquivoArmazenado = Guid.NewGuid() + ".csu";
                            //ObjMCSUControleSaidaContaCorrente.NumLote                                                                                                                  
                            //ObjMCSUControleSaidaContaCorrente.ProtocoloRecebimento   

                            ObjMCSUControleSaidaContaCorrente.ControleSaidaId = CSUControleSaidaContaCorrente.Insert(ObjMCSUControleSaidaContaCorrente);

                            foreach (int? NrDocum in ListNrDocum)
                            {
                                ObjContaCorrente = PaymentAttemptContaCorrente.GetInstance().Locate((int)ObjContaCorrente.NrDocum);
                                ObjContaCorrente.ControleSaidaId = ObjMCSUControleSaidaContaCorrente.ControleSaidaId;
                                PaymentAttemptContaCorrente.GetInstance().Update(ObjContaCorrente);
                            }

                            //Arquivo o arquivo enviado para CSU
                            File.Copy(ArqCSUPara, ConfigurationManager.AppSettings["CnabDirArchive"] + ObjMCSUControleSaidaContaCorrente.NomeArquivoArmazenado);

                        }
                        catch (Exception ex)
                        {
                            #region Exception

                            if (ObjMCnabControleSaida.CnabControleSaidaId != 0)
                            {
                                try
                                {
                                    string PathError = StrArquivo.Substring(0, StrArquivo.LastIndexOf("\\") - 6) + "Error\\" + ObjMCnabControleSaida.NomeArquivoArmazenado;
                                    ObjMCnabControleSaida = CnabControleSaida.Locate(ObjMCnabControleSaida.CnabControleSaidaId);
                                    ObjMCnabControleSaida.Status = (int)ContaCorrenteStatus.ErroProcessamentoSuperpag;
                                    CnabControleSaida.Update(ObjMCnabControleSaida);

                                    if (File.Exists(StrArquivo))
                                    {
                                        //move o arquivo para pasta de erro
                                        Move(StrArquivo, PathError);
                                    }

                                    if (File.Exists(ArqCSU))
                                    {
                                        if (!File.Exists(PathError))
                                        {
                                            //move o arquivo para pasta de erro
                                            Move(ArqCSU, PathError);
                                        }
                                        else
                                        {
                                            File.Delete(ArqCSU);
                                        }
                                    }

                                    SuperPag.Helper.GenericHelper.LogFile("RetornoDebContaCorrente::Main Erro no arquivo de retorno=" + StrArquivo + " Arquivo salvo na pasta de erro=" + PathError + " Erro=" + ex.Message, LogFileEntryType.Error);
                                }
                                catch (Exception exr)
                                {
                                    SuperPag.Helper.GenericHelper.LogFile("RetornoDebContaCorrente::Main " + exr.Message, LogFileEntryType.Error);
                                }
                            }
                            #endregion
                        }
                    }
                }
            }
        }

        private static int ValidaArquivo(string Path, int TamanhoArquivo)
        {
            StreamReader ObjStreamReaderCont = new StreamReader(Path);
            int cont = 0;
            while (ObjStreamReaderCont.Peek() > 0)
            {
                cont++;
                string strLine = ObjStreamReaderCont.ReadLine();
                if (strLine.Length != TamanhoArquivo)
                {
                    throw new Exception("Arquivo invalido não esta no formato " + TamanhoArquivo + " posições erro na linha" + cont.ToString());
                }
            }
            return cont;
        }
        private static IList<MCnab> GetArquivo(string Path, IList<MCnab> Lista, int? Linha)
        {
            StreamReader ObjStreamReaderCont = new StreamReader(Path);
            IList<MCnab> ListaRet = new List<MCnab>();

            if (Linha != null)
            {
                int cont = 0;
                while (ObjStreamReaderCont.Peek() > 0)
                {
                    cont++;
                    string strLine = ObjStreamReaderCont.ReadLine();
                    //Pego a mesma linha passada por parametro
                    if (cont == Linha)
                    {
                        foreach (MCnab ObjMCnab in Lista)
                        {
                            ObjMCnab.Valor = strLine.Substring((ObjMCnab.Inicio - 1), ObjMCnab.Tamanho);
                            ListaRet.Add(ObjMCnab);
                        }
                        if (ObjStreamReaderCont != null) ObjStreamReaderCont.Close();
                        return ListaRet;
                    }
                }
            }
            else
            {
                if (ObjStreamReaderCont.Peek() > 0)
                {
                    string strLine = ObjStreamReaderCont.ReadLine();
                    foreach (MCnab ObjMCnab in Lista)
                    {
                        ObjMCnab.Valor = strLine.Substring((ObjMCnab.Inicio - 1), ObjMCnab.Tamanho);
                        ListaRet.Add(ObjMCnab);
                    }
                }
            }
            if (ObjStreamReaderCont != null) ObjStreamReaderCont.Close();
            return ListaRet;
        }
        private static string GetOcorrenciaSP(string Ocorrencia)
        {
            switch (Ocorrencia)
            {
                case "00":
                    return "00";
                case "BD":
                    return "BD";
                default:
                    return "99";
            }
        }
        private static int GetStatus(string Ocorrencia)
        {
            switch (Ocorrencia)
            {
                case "00":
                    return (int)ContaCorrenteStatus.Pago;
                case "BD":
                    return (int)ContaCorrenteStatus.AguardandoPagamento;
                default:
                    return (int)ContaCorrenteStatus.NaoPago;
            }
        }
        private static void Move(string PathDe, string PathPrara)
        {
            StreamWriter ObjWriter = File.CreateText(PathPrara);
            ObjWriter.Write(File.ReadAllText(PathDe));
            ObjWriter.Close();

            File.Delete(PathDe);
        }
        private static void CriarArquivo(MPaymentAttemptContaCorrente ObjContaCorrente, MCnabControleSaida ObjMCnabControleSaida, String PathContaCorrente)
        {
            IList<MCnab> Lista;
            StreamWriter ObjStreamWriter;
            ObjStreamWriter = File.CreateText(PathContaCorrente);
            try
            {
                #region Parametros Header
                Lista = Cnab.GetCnab("200", "CSU", Convert.ToDateTime("11/9/2008 00:00:00"), 1, 1);

                Lista[0].Valor = "0";
                Lista[1].Valor = "015";
                Lista[2].Valor = "0";
                Lista[3].Valor = "0";
                Lista[4].Valor = "M";
                Lista[5].Valor = ""; //perguntar Nome da intituição financeira         
                Lista[6].Valor = string.Empty;

                ObjStreamWriter.WriteLine(MCnab.GetLine(Lista));
                #endregion

                #region Header de Lote
                Lista = Cnab.GetCnab("200", "CSU", Convert.ToDateTime("11/9/2008 00:00:00"), 2, 1);

                Lista[0].Valor = "1";
                Lista[1].Valor = "015";
                Lista[2].Valor = "0";
                Lista[3].Valor = "";//Ver como sera o numero do sequencial 
                Lista[4].Valor = DateTime.Now.ToString("ddMMyyyy");
                Lista[5].Valor = "0";//Perguntar Código das transaçõescontidas neste lote.
                Lista[6].Valor = "0";//
                Lista[7].Valor = string.Empty;
                Lista[8].Valor = string.Empty;
                Lista[9].Valor = "0";
                Lista[10].Valor = "0";
                Lista[11].Valor = string.Empty;

                ObjStreamWriter.WriteLine(MCnab.GetLine(Lista));
                #endregion

                #region Registro de transação
                Lista = Cnab.GetCnab("200", "CSU", Convert.ToDateTime("11/9/2008 00:00:00"), 3, 1);

                Lista[0].Valor = "2";
                Lista[1].Valor = "015";
                Lista[2].Valor = "0";
                Lista[3].Valor = "";//sequencial
                Lista[4].Valor = ObjContaCorrente.Plastico;
                Lista[5].Valor = "0";
                Lista[6].Valor = ObjContaCorrente.DataVencimento.ToString("ddMMyyyy");
                Lista[7].Valor = ObjContaCorrente.ValorAgendado.ToString().Replace(",", string.Empty);
                Lista[8].Valor = string.Empty;
                Lista[9].Valor = ObjContaCorrente.Ocorrencia;
                Lista[10].Valor = "0";
                Lista[11].Valor = "0";
                Lista[12].Valor = "0";
                Lista[13].Valor = "0";
                Lista[14].Valor = "0";
                Lista[15].Valor = "0";
                Lista[16].Valor = "0";
                Lista[17].Valor = "4";
                Lista[18].Valor = "4";
                Lista[19].Valor = "4";
                Lista[20].Valor = "0";
                Lista[21].Valor = string.Empty;
                Lista[22].Valor = ObjContaCorrente.DataVencimento.ToString("ddMMyyyy"); ;
                Lista[23].Valor = string.Empty;
                Lista[24].Valor = "0";
                Lista[25].Valor = "0";
                Lista[26].Valor = "0";
                Lista[27].Valor = string.Empty;
                Lista[28].Valor = "N";
                Lista[29].Valor = string.Empty;

                ObjStreamWriter.WriteLine(MCnab.GetLine(Lista));
                #endregion

                #region Trailler de Lote
                Lista = Cnab.GetCnab("200", "CSU", Convert.ToDateTime("11/9/2008 00:00:00"), 18, 1);

                Lista[0].Valor = "3";
                Lista[1].Valor = "015";
                Lista[2].Valor = "0";
                Lista[3].Valor = ObjMCnabControleSaida.QtdeDetalhes.ToString();
                Lista[4].Valor = ObjMCnabControleSaida.ValorTotalDetalhes.ToString().Replace(",", string.Empty);
                Lista[6].Valor = string.Empty;

                ObjStreamWriter.WriteLine(MCnab.GetLine(Lista));
                #endregion

                #region parametros Trailler

                Lista = Cnab.GetCnab("200", "CSU", Convert.ToDateTime("11/9/2008 00:00:00"), 19, 1);

                Lista[0].Valor = "9";
                Lista[1].Valor = "015";
                Lista[2].Valor = "0";
                Lista[3].Valor = "9";
                Lista[4].Valor = "1";
                Lista[5].Valor = ObjMCnabControleSaida.ValorTotalDetalhes.ToString().Replace(",", string.Empty);
                Lista[6].Valor = string.Empty;

                ObjStreamWriter.WriteLine(MCnab.GetLine(Lista));

                #endregion

                if (ObjStreamWriter != null) ObjStreamWriter.Close();
            }
            catch (Exception)
            {
                if (ObjStreamWriter != null) ObjStreamWriter.Close();
            }
        }
    }
}
