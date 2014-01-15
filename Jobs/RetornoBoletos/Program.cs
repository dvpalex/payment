using System;
using System.Configuration;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using SuperPag.Agents.Boleto.Messages;
using SuperPag.Agents.Boleto;

namespace RetornoBoletos
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {

                SuperPag.Helper.GenericHelper.LogFile("INICIO DE PROCESSAMENTO: Serviço de Retorno de Boleto", SuperPag.LogFileEntryType.Information);

                string returnCnabDir = ConfigurationManager.AppSettings["CnabDir"];
                string returnCnabHerbalife = ConfigurationManager.AppSettings["CnabHerbalife"];
                string arqCnabDir = ConfigurationManager.AppSettings["ArqCnabDir"];
                string errCnabDir = ConfigurationManager.AppSettings["ErrCnabDir"];

                if (SuperPag.Agents.Boleto.Configuration.ConfigurationSettings.AppSettings.ExtensionMaps.Count > 0)
                {
                    foreach (SuperPag.Agents.Boleto.Configuration.MExtensionMap mExtensionMap in SuperPag.Agents.Boleto.Configuration.ConfigurationSettings.AppSettings.ExtensionMaps)
                    {
                        string[] fileNames = Directory.GetFiles(returnCnabDir, mExtensionMap.Extension, SearchOption.TopDirectoryOnly);
                        Int32 Posicao;
                        if (fileNames.Length > 0)
                        {
                            foreach (string fileName in fileNames)
                            {
                                //Verifica e Move o arquivo para a pasta da Herbalife
                                Posicao = fileName.LastIndexOf(".");
                                if (fileName.Substring(Posicao-5,5) == "00006")
                                    File.Copy(fileName, returnCnabHerbalife + fileName.Replace(returnCnabDir, ""));
                                
                                StreamReader file = new StreamReader(fileName, Encoding.ASCII);
                                XmlDocument xml = null;

                                try
                                {

                                    //if (file != null) file.Close();
                                        

                                    SuperPag.Helper.GenericHelper.LogFile(string.Format("Processando Arquivo: {0}", fileName), SuperPag.LogFileEntryType.Information);
                                    xml = CnabConverter.CNAB.TransformCnabToXml(file.ReadToEnd(), mExtensionMap.CnabType);
                                    file.Close();

                                    StringReader reader = new StringReader(xml.InnerXml);
                                    //StreamReader reader = new StreamReader(@"c:\xmlcnab.xml");

                                    XmlSerializer deserializer = new XmlSerializer(typeof(MPaymentAttemptBoletoReturn));
                                    MPaymentAttemptBoletoReturn mPaymentAttemptBoletoReturn = (MPaymentAttemptBoletoReturn)deserializer.Deserialize(reader);
                                    mPaymentAttemptBoletoReturn.Header.NameOfCapturedFile = fileName.Substring(fileName.LastIndexOf("\\") + 1);
                                    mPaymentAttemptBoletoReturn.Header.CreationDateCapturedFile = File.GetCreationTime(fileName);

                                    PaymentAttemptBoletoReturn AttemptBoletoReturn = new PaymentAttemptBoletoReturn();

                                    if (AttemptBoletoReturn.HeaderExists(mPaymentAttemptBoletoReturn.Header))
                                    {
                                        throw new Exception(string.Format("O arquivo {0} cnab já foi processado!", mPaymentAttemptBoletoReturn.Header.NameOfCapturedFile));
                                    }

                                    mPaymentAttemptBoletoReturn.Header.NameOfArquivedFile = Guid.NewGuid().ToString() + ".cnab";

                                    AttemptBoletoReturn.ProcessBoletoReturn(mPaymentAttemptBoletoReturn);

                                    File.Move(fileName, arqCnabDir + mPaymentAttemptBoletoReturn.Header.NameOfArquivedFile);

                                    SuperPag.Helper.GenericHelper.LogFile(string.Format("Arquivo {0} processado com sucesso. Gravado como {1}",
                                        mPaymentAttemptBoletoReturn.Header.NameOfCapturedFile,
                                        mPaymentAttemptBoletoReturn.Header.NameOfArquivedFile), SuperPag.LogFileEntryType.Information);

                                }
                                catch (Exception ex)
                                {
                                    string message = "";
                                    if(ex.InnerException != null)
                                        message = "message=" + ex.InnerException.Message + " stack=" + ex.InnerException.StackTrace + " source=" + ex.InnerException.Source;
                                    else
                                        message = "message=" + ex.Message + " stack=" + ex.StackTrace + " source=" + ex.Source;
                                    if (xml != null)
                                        message += " xml=" + xml.InnerXml;
                                        
                                    SuperPag.Helper.GenericHelper.LogFile(message, SuperPag.LogFileEntryType.Error);
                                    
                                    if (file != null) file.Close();
                                    File.Move(fileName, errCnabDir + fileName.Substring(fileName.LastIndexOf("\\") + 1));
                                }
                            }
                        }
                        else
                        {
                            SuperPag.Helper.GenericHelper.LogFile(string.Format("Nenhum arquivo encontrada com extensão {0}", mExtensionMap.Extension), SuperPag.LogFileEntryType.Information);
                        }
                    }
                }
                else
                {
                    SuperPag.Helper.GenericHelper.LogFile("Não foi definido nenhuma extensão no arquivo de configuração para ser processada", SuperPag.LogFileEntryType.Warning);
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    SuperPag.Helper.GenericHelper.LogFile("message=" + ex.InnerException.Message + " stack=" + ex.InnerException.StackTrace + " source=" + ex.InnerException.Source, SuperPag.LogFileEntryType.Error);
                else
                    SuperPag.Helper.GenericHelper.LogFile("message=" + ex.Message + " stack=" + ex.StackTrace + " source=" + ex.Source, SuperPag.LogFileEntryType.Error);
            }
            finally
            {
                SuperPag.Helper.GenericHelper.LogFile("FIM DE PROCESSAMENTO: Serviço de Reorno de Boleto", SuperPag.LogFileEntryType.Information);
            }
        }

    }
}
