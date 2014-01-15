using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using SuperPag;
using SuperPag.Helper.Xml.Request;
using SuperPag.Helper.Xml.Response;
using SuperPag.Handshake.Service;
using SuperPag.Helper.Xml;
using SuperPag.Data;
using SuperPag.Data.Messages;
using System.Configuration;
using SuperPag.Business;
using System.IO;
using SuperPag.Business.Messages;

namespace RecebimentoArquivo
{
    class Program
    {

        public static int StoreId
        {
            get { return int.Parse(ConfigurationManager.AppSettings["StoreId"]); }
        }

        static void Main(string[] args)
        {
            string Path = ConfigurationManager.AppSettings["CnabDir"];
            int ControleEntradaId = 0;
            MCSUControleEntradaContaCorrente ObjEntradaContaCorrente = new MCSUControleEntradaContaCorrente();

            SuperPag.Helper.GenericHelper.LogFile("INICIO DE PROCESSAMENTO: Serviço de recebimento arquivo CSU.", SuperPag.LogFileEntryType.Information);

            foreach (string fileNames in Directory.GetFiles(Path))
            {
                try
                {
                    //valido o arquivo e retorno o numero de linhas do arquivo
                    int NumLinhas = ValidaArquivo(fileNames, 150);

                    IList<MCnab> ListaRet = new List<MCnab>();
                  
                    //recupero o valor do HEADER DE LOTE
                    ListaRet = GetArquivo(fileNames, Cnab.GetCnab("150", "CSU", Convert.ToDateTime("11/9/2008"), 2,1), null);

                    //Console.WriteLine("Qtd. Lista :: " + ListaRet.Count);
                    //Console.ReadLine();

                    //TODO: Verificar como sera localisado o numero do banco no arquivo ou atravez do diretorio
                    if (!CSUControleEntradaContaCorrente.SelectLote(Convert.ToInt32(ListaRet[5].Valor)))
                    {
                        throw new Exception("Lote duplicado(seguencial repetido) verificar na pasta de erro. Arquivo=" + fileNames);
                    }

                    //data da geração do arquivo pela csu
                    ObjEntradaContaCorrente.DataArquivo = FormatDate(ListaRet[3].Valor);
                    ObjEntradaContaCorrente.DataCapturaArquivo = DateTime.Now;
                    ObjEntradaContaCorrente.NomeArquivoArmazenado = Guid.NewGuid() + ".csu";
                    ObjEntradaContaCorrente.NomeArquivoCapturado = fileNames.Substring(fileNames.LastIndexOf('\\'));
                    //ObjEntradaContaCorrente.NumInstituicao = Convert.ToInt32(ListaRet[0].Valor);
                    ObjEntradaContaCorrente.NumSeqRemessa = Convert.ToInt32(ListaRet[5].Valor);

                    //recupero o valor do REGISTRO TRAILER DE LOTE
                    ListaRet = GetArquivo(fileNames, Cnab.GetCnab("150", "CSU", Convert.ToDateTime("11/9/2008"), 18,1), NumLinhas);

                    ObjEntradaContaCorrente.QtdeDetalhes = Convert.ToInt32(ListaRet[3].Valor);
                    //ObjEntradaContaCorrente.ValorTotalDetalhes = Convert.ToDecimal(ListaRet[5].Valor.Insert(ListaRet[5].Valor.Length - 2, ","));
                    //ObjEntradaContaCorrente.NumInstituicao = Convert.ToInt32(ListaRet[1].Valor);

                    String flagdeb = String.Empty;
                    String Valor = String.Empty;

                    for (int i = 2; i < NumLinhas; i++)
                    {

                        //recupero o valor do REGISTRO DE DETALHE
                        ListaRet = GetArquivo(fileNames, Cnab.GetCnab("150", "CSU", Convert.ToDateTime("11/9/2008"), 3, 1), i);

                        // Recupera o Valor a ser Pago
                        flagdeb = ListaRet[18].Valor;
                        if (flagdeb == "")
                            Valor = String.Empty;
                        else if (flagdeb == "T")
                            Valor = ListaRet[7].Valor;
                        else
                            Valor = ListaRet[9].Valor;

                        ObjEntradaContaCorrente.ValorTotalDetalhes += Convert.ToDecimal(Valor.Insert(Valor.Length - 2, ","));
                    }
                    
                    ControleEntradaId = CSUControleEntradaContaCorrente.Insert(ObjEntradaContaCorrente);
                    flagdeb = String.Empty;
                    Valor = String.Empty;
                    for (int i = 2; i < NumLinhas; i++)
                    {
                        
                        //recupero o valor do REGISTRO DE DETALHE
                        ListaRet = GetArquivo(fileNames, Cnab.GetCnab("150", "CSU", Convert.ToDateTime("11/9/2008"), 3,1), i);
                        
                        // Recupera o Valor a ser Pago
                        flagdeb = ListaRet[18].Valor;
                        if (flagdeb == "")
                            Valor = String.Empty;
                        else if (flagdeb == "T")
                            Valor = ListaRet[7].Valor;
                        else
                            Valor = ListaRet[9].Valor;


                        // Gera XML
                        string instituicaoDepara =  getDeparaInstituicao(Convert.ToInt32(ListaRet[17].Valor));

                        
                        string xml = InsertXML(Valor, instituicaoDepara, "0", ListaRet[4].Valor, "0", ListaRet[3].Valor, FormatDate(ListaRet[11].Valor), ListaRet[1].Valor, ListaRet[2].Valor);
                        //Gera o arquivo padrao 400
                        MPaymentAttemptContaCorrente ObjContaCorrente = PaymentAttemptContaCorrente.GetInstance().Locate(Salvar(xml));
                        ObjContaCorrente.ControleEntradaId = ControleEntradaId;
                        ObjContaCorrente.NumInstituicao = Convert.ToInt32(ListaRet[17].Valor);
                        PaymentAttemptContaCorrente.GetInstance().Update(ObjContaCorrente);
                    }
                    string ArqArchive = ConfigurationManager.AppSettings["CnabDirArchive"] + ObjEntradaContaCorrente.NomeArquivoArmazenado;

                    //guardo o arquivo
                    StreamWriter ObjWriter = File.CreateText(ArqArchive);
                    ObjWriter.Write(File.ReadAllText(fileNames));
                    ObjWriter.Close();

                    File.Delete(fileNames);

                    //move todos os arquivos para a pasta imbox
                    MoveArq(StoreId, ControleEntradaId);
                }
                catch (Exception ex)
                {
                    SuperPag.Helper.GenericHelper.LogFile("RecebimentoArquivo::Main " + ex.Message + " - " + ex.StackTrace, LogFileEntryType.Error);


                    //TODO:Workflow
                    #region Exception
                    try
                    {
                        string PathError = ConfigurationManager.AppSettings["CnabDirError"];
                        string NomeArq;
                        if (ControleEntradaId == 0)
                        {
                            NomeArq = Guid.NewGuid().ToString() + ".csu";

                        }
                        else
                        {
                            NomeArq = ObjEntradaContaCorrente.NomeArquivoArmazenado;
                        }

                        StreamWriter ObjWriter = File.CreateText(PathError + NomeArq);
                        ObjWriter.Write(File.ReadAllText(fileNames));
                        ObjWriter.Close();

                        File.Delete(fileNames);

                        SuperPag.Helper.GenericHelper.LogFile("RecebimentoArquivo::Main Arquivo Enviado pela CSU=" + fileNames + " Arquivo salvo na pasta de erro=" + NomeArq + " Erro=" + ex.Message, LogFileEntryType.Error);
                    #endregion
                    }
                    catch (Exception exr)
                    {

                        SuperPag.Helper.GenericHelper.LogFile("RecebimentoArquivo::Main " + exr.Message + " - " + exr.StackTrace, LogFileEntryType.Error);
                    }
                }
            }
        }

        private static string GetFormId(string NumBanco)
        {
            return PaymentForm.SelectPaymentFormId(NumBanco, StoreId);
        }

        /// <summary>
        /// Move o arquivo da pasta inboxsuperpag para a pasta inbox de cada banco
        /// </summary>
        /// <param name="StoreId"></param>
        private static void MoveArq(int StoreId, int CSUControleEntradaId)
        {
            IList<MCnab> ListaRet = new List<MCnab>();
            IList<MCnab> ListaHeader = new List<MCnab>();

            foreach (string str in PaymentAgentSetupDebitoContaCorrente.GetInstance().GetPath(StoreId))
            {
                string PathDe = ConfigurationManager.AppSettings["PathContaCorrente"] + "\\" + str + "INBOXSUPERPAG";
                string PathPrara = ConfigurationManager.AppSettings["PathContaCorrente"] + "\\" + str + "INBOX";
                int cont = 0;

                foreach (string StrPathPrara in Directory.GetFiles(PathPrara))
                {
                    cont++;
                }

                foreach (string StrPathDe in Directory.GetFiles(PathDe))
                {
                    MCnabControleEntrada ObjMCnabControleEntrada = new MCnabControleEntrada();
                    ObjMCnabControleEntrada.DataArquivo = DateTime.Now;

                    string Arq = StrPathDe.Substring(StrPathDe.LastIndexOf("\\") + 1);
                    Arq = Arq.Substring(0, Arq.LastIndexOf("_"));
                    Arq += "_" + cont + ".REM";
                    ObjMCnabControleEntrada.NomeArquivo = PathPrara + "\\" + Arq;
                    Move(StrPathDe, ObjMCnabControleEntrada.NomeArquivo);

                    //valido o arquivo e retorno o numero de linhas do arquivo
                    int NumLinhas = ValidaArquivo(PathPrara + "\\" + Arq, 400);

                    for (int i = 2; i < NumLinhas; i++)
                    {
                        //recupero o valor do REGISTRO DE DETALHE
                        ListaRet = GetArquivo(PathPrara + "\\" + Arq, Cnab.GetCnab("400", "01", Convert.ToDateTime("8/9/2004 00:00:00"), 3,1), i);
                        ListaHeader = GetArquivo(PathPrara + "\\" + Arq, Cnab.GetCnab("400", "01", Convert.ToDateTime("8/9/2004 00:00:00"), 1,1), 1);

                        ObjMCnabControleEntrada.NumInstituicao = Convert.ToInt32(ListaRet[11].Valor);
                        ObjMCnabControleEntrada.NumSeqRemessa = Convert.ToInt32(ListaHeader[12].Valor);
                        ObjMCnabControleEntrada.QtdeDetalhes += 1;
                        ObjMCnabControleEntrada.ValorTotalDetalhes += Convert.ToDecimal(ListaRet[25].Valor.Insert(ListaRet[25].Valor.Length - 2, ","));

                    }

                    //TODO:Colocar o status de acordo com a tabela
                    ObjMCnabControleEntrada.Status = (int)ContaCorrenteStatus.EnviadoBanco;
                    int CnabControleEntradaId = CnabControleEntrada.Insert(ObjMCnabControleEntrada);
                    PaymentAttemptContaCorrente.GetInstance().UpdateByCnabControleEntradaId(CnabControleEntradaId, CSUControleEntradaId);
                }
            }
        }
        private static void Move(string PathDe, string PathPrara)
        {
            StreamWriter ObjWriter = File.CreateText(PathPrara);
            ObjWriter.Write(File.ReadAllText(PathDe));
            ObjWriter.Close();

            File.Delete(PathDe);
        }
        private static DateTime FormatDate(string Data)
        {
            Data = Data.Insert(2, "/").Insert(5, "/");
            return Convert.ToDateTime(Data);
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

        /// <summary>alido
        /// Salva as informações nas tabelas do superpag
        /// </summary>
        private static Guid Salvar(string xml)
        {
            try
            {
                XmlDocument ObjxmlRet = new XmlDocument();

                
                DStore store = DataFactory.Store().Locate(Convert.ToInt32(ConfigurationManager.AppSettings["StoreId"].ToString()));

                request req;
                string msgerror = "";
                if ((req = (request)XmlHelper.GetClass(xml, typeof(request), out msgerror)) == null)
                    Ensure.IsNotNull(null, msgerror);

                Request requestHelper = new Request();

                response resp = requestHelper.ProcessRequest(store, req);

                string s = XmlHelper.GetXml(typeof(response), resp);

                ObjxmlRet.LoadXml(s);

                Guid PaymentAttemptId = new Guid(ObjxmlRet.InnerText);

                SuperPag.Helper.GenericHelper.LogFile("RecebimentoArquivo::Salvar " + s, LogFileEntryType.Information);

                return PaymentAttemptId;
            }
            catch (Exception ex)
            {
                SuperPag.Helper.GenericHelper.LogFile("RecebimentoArquivo::Salvar Problema no processamento do superpag" + ex.Message, LogFileEntryType.Error);
                return new Guid(string.Empty);
            }
        }
        private static string InsertXML(string Valor, string PaymentForm, string DigVerAg, string NumContCorrent,
                                string DigVerCont, string NumAgencia, DateTime Vencimento, string CodigoLogo,
                                string Plastico
                              )
        {
            try
            {
                XmlDocument Objxml = new XmlDocument();

                //Caminho do XML 
                Objxml.Load(ConfigurationManager.AppSettings["PathXml"]);

                Objxml.ChildNodes[0].ChildNodes[0].ChildNodes[0].Attributes["total"].Value = Valor;

                Objxml.ChildNodes[0].ChildNodes[0].ChildNodes[0].ChildNodes[2].ChildNodes[0].Attributes["amount"].Value = Valor;
                Objxml.ChildNodes[0].ChildNodes[0].ChildNodes[0].ChildNodes[2].ChildNodes[0].Attributes["form"].Value = PaymentForm;

                //ContaCorrenteInformationCSU
                Objxml.ChildNodes[0].ChildNodes[0].ChildNodes[0].ChildNodes[2].ChildNodes[0].ChildNodes[0].ChildNodes[0].ChildNodes[0].InnerText = DigVerAg;
                Objxml.ChildNodes[0].ChildNodes[0].ChildNodes[0].ChildNodes[2].ChildNodes[0].ChildNodes[0].ChildNodes[0].ChildNodes[1].InnerText = NumContCorrent;
                Objxml.ChildNodes[0].ChildNodes[0].ChildNodes[0].ChildNodes[2].ChildNodes[0].ChildNodes[0].ChildNodes[0].ChildNodes[2].InnerText = DigVerCont;
                Objxml.ChildNodes[0].ChildNodes[0].ChildNodes[0].ChildNodes[2].ChildNodes[0].ChildNodes[0].ChildNodes[0].ChildNodes[3].InnerText = NumAgencia;
                Objxml.ChildNodes[0].ChildNodes[0].ChildNodes[0].ChildNodes[2].ChildNodes[0].ChildNodes[0].ChildNodes[0].ChildNodes[4].InnerText = Vencimento.ToString("yyyy-MM-dd");
                Objxml.ChildNodes[0].ChildNodes[0].ChildNodes[0].ChildNodes[2].ChildNodes[0].ChildNodes[0].ChildNodes[0].ChildNodes[5].InnerText = CodigoLogo;
                Objxml.ChildNodes[0].ChildNodes[0].ChildNodes[0].ChildNodes[2].ChildNodes[0].ChildNodes[0].ChildNodes[0].ChildNodes[6].InnerText = Plastico;

                return Objxml.InnerXml;
            }
            catch (Exception ex)
            {
                //TODO:Workflow
                SuperPag.Helper.GenericHelper.LogFile("RecebimentoArquivo:InsertXML Problema na geração do XML do superpag" + ex.Message, LogFileEntryType.Error);
                return string.Empty;
            }

            

        }

        private static string getDeparaInstituicao(Int32 Valor)
        {

            switch (Valor)
            {
                case 33:
                    return "45";
                case 341:
                    return "44";
                case 237:
                    return "43";
                case 409:
                    return "51";
                case 41:
                    return "47";
                case 1:
                    return "48";
                case 356:
                    return "52";
                case 151:
                    return "548";
                case 399:
                    return "55";
                case 745:
                    return "56";
                case 422:
                    return "57";
                case 389:
                    return "58";
                case 479:
                    return "59";    
                default:
                    return "";
            }
        
        }

    }
}
