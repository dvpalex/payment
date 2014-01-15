using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Configuration;
using SuperPag.Agents.ContaCorrente;
using SuperPag;
using SuperPag.Helper;
using SuperPag.Business;
using SuperPag.Business.Messages;
using SuperPag.Data;
using SuperPag.Data.Messages;

namespace EnviaContaCorrente
{
    class Program
    {
        static void Main(string[] args)
        {
            VerificaRetorno();
            EnviaArquivo();
        }
        private static void VerificaRetorno()
        {
            try
            {
                foreach (string strFile in Directory.GetFiles(ConfigurationManager.AppSettings["CaminhoRetorno"]))
                {
                    StreamReader ObjStreamReader = new StreamReader(strFile);
                    string str = "";

                    //Obitem o numero do retorno do banco
                    string linhatexto = ObjStreamReader.ReadLine();
                    while (linhatexto != null)
                    {
                        linhatexto = ObjStreamReader.ReadLine();
                        if (linhatexto.Substring(230, 10).Trim() != string.Empty)
                        {
                            AtualizaStaus(linhatexto.Substring(230, 10).Trim(), new Guid(linhatexto.Substring(177, 40).Trim()));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        private static void AtualizaStaus(string retornoCC, Guid PaymentAttemptId)
        {
            try
            {
                DPaymentAttempt ObjDPaymentAttempt = DataFactory.PaymentAttempt().Locate(PaymentAttemptId);
                
                ObjDPaymentAttempt.status = (int)PaymentAttemptStatus.Paid;
                DataFactory.PaymentAttempt().Update(ObjDPaymentAttempt);
                MPaymentAttemptContaCorrente ObjContaCorrente = PaymentAttemptContaCorrente.GetInstance().Locate(PaymentAttemptId);
                ObjContaCorrente.Ocorrencia = retornoCC;
                PaymentAttemptContaCorrente.GetInstance().Update(ObjContaCorrente);

            }
            catch (Exception ex)
            {
                throw;
            }
        }
        private static void EnviaArquivo()
        {
            try
            {
                foreach (string Dir in Directory.GetDirectories(ConfigurationManager.AppSettings["CaminhoAtual"]))
                {
                    foreach (string strFile in Directory.GetFiles(Dir))
                    {
                        //pega sempre o arquivo do dia anterior para enviar a van 
                        if (File.GetCreationTime(strFile).Date < DateTime.Now.Date)
                        {
                            StreamReader ObjStreamReader = new StreamReader(strFile);
                            int registro = 0;

                            //Obitem o numero de linhas do arquivo
                            string linhatexto = ObjStreamReader.ReadLine();
                            while (linhatexto != null)
                            {

                                linhatexto = ObjStreamReader.ReadLine();

                                registro = registro + 1;
                            }
                            ObjStreamReader.Close();

                            //cria a dimenção da string para criar o arquivo
                            StringBuilder ObjStringBuilder = ContaCorrente.DimencionaString();

                            StreamWriter ObjStreamWriter = new StreamWriter(strFile, true);

                            ObjStreamWriter.WriteLine(ContaCorrente.MontaTrailer((int)((registro - 1) / 2), ObjStringBuilder));
                            ObjStreamWriter.Close();
                            //Directory.GetParent(strFile);
                            string CaminhoEnviados = ConfigurationManager.AppSettings["CaminhoEnviados"] + "\\" + Directory.GetParent(strFile).Name;
                            string CaminhoVan = ConfigurationManager.AppSettings["CaminhoVan"] + "\\" + Directory.GetParent(strFile).Name;

                            if (!Directory.Exists(CaminhoEnviados))
                            {
                                Directory.CreateDirectory(CaminhoEnviados);
                            }

                            if (!Directory.Exists(CaminhoVan))
                            {
                                Directory.CreateDirectory(CaminhoVan);
                            }

                            File.Copy(strFile, CaminhoEnviados + "\\" + strFile.Substring((strFile.LastIndexOf('\\') + 1)));
                            File.Move(strFile, CaminhoVan + "\\" + strFile.Substring((strFile.LastIndexOf('\\') + 1)));

                            SuperPag.Helper.GenericHelper.LogFile("EnviaContaCorrente:Program.cs:::Arquivo enviado com sucesso" + strFile, LogFileEntryType.Information);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                SuperPag.Helper.GenericHelper.LogFile("EnviaContaCorrente::Program.cs::Main Arquivo não enviado" + e.Message, LogFileEntryType.Error);
            }
        }
    }
}
