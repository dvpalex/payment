using System;
using System.Configuration;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using SuperPag.Agents.DepId;
using SuperPag.Agents.DepId.Messages;

namespace RetornoDepIdentificado
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {

                SuperPag.Helper.GenericHelper.LogFile("INICIO DE PROCESSAMENTO: Serviço de Retorno de Depósito Identificado", SuperPag.LogFileEntryType.Information);
                string returnCnabDir = ConfigurationManager.AppSettings["CnabDir"];
                string arqCnabDir = ConfigurationManager.AppSettings["ArqCnabDir"];
                string errCnabDir = ConfigurationManager.AppSettings["ErrCnabDir"];
                if (SuperPag.Agents.DepId.Configuration.ConfigurationSettings.AppSettings.ExtensionMaps.Count > 0)
                {
                    foreach (SuperPag.Agents.DepId.Configuration.MExtensionMap mExtensionMap in SuperPag.Agents.DepId.Configuration.ConfigurationSettings.AppSettings.ExtensionMaps)
                    {
                        string[] fileNames = Directory.GetFiles(returnCnabDir, mExtensionMap.Extension, SearchOption.TopDirectoryOnly);

                        if (fileNames.Length > 0)
                        {
                            foreach (string fileName in fileNames)
                            {
                                try
                                {
                                    SuperPag.Helper.GenericHelper.LogFile(string.Format("Processando Arquivo: {0}", fileName), SuperPag.LogFileEntryType.Information);
                                    StreamReader file = new StreamReader(fileName, Encoding.ASCII);
                                    
                                    XmlDocument xml = CnabConverter.CNAB.TransformCnabToXml(file.ReadToEnd(), mExtensionMap.CnabType);
                                    //XmlDocument xml = Test.GetTestXml();
                                                                        
                                    file.Close();

                                    StringReader reader = new StringReader(xml.InnerXml);
                                    //StreamReader reader = new StreamReader(@"c:\xmlcnab.xml");

                                    XmlSerializer deserializer = new XmlSerializer(typeof(MPaymentAttemptDepIdReturn));
                                    MPaymentAttemptDepIdReturn mPaymentAttemptDepIdReturn = (MPaymentAttemptDepIdReturn)deserializer.Deserialize(reader);
                                    mPaymentAttemptDepIdReturn.Header.NameOfCapturedFile = fileName.Substring(fileName.LastIndexOf("\\") + 1);
                                    mPaymentAttemptDepIdReturn.Header.CreationDateCapturedFile = File.GetCreationTime(fileName);

                                    PaymentAttemptDepIdReturn AttemptDepositoIdentReturn = new PaymentAttemptDepIdReturn();

                                    if (AttemptDepositoIdentReturn.HeaderExists(mPaymentAttemptDepIdReturn.Header))
                                    {
                                        throw new Exception(string.Format("O arquivo {0} cnab já foi processado!", mPaymentAttemptDepIdReturn.Header.NameOfCapturedFile));
                                    }

                                    mPaymentAttemptDepIdReturn.Header.NameOfArquivedFile = Guid.NewGuid().ToString() + ".cnab";

                                    AttemptDepositoIdentReturn.ProcessDepIdReturn(mPaymentAttemptDepIdReturn);

                                    File.Move(fileName, arqCnabDir + mPaymentAttemptDepIdReturn.Header.NameOfArquivedFile);


                                    SuperPag.Helper.GenericHelper.LogFile(string.Format("Arquivo {0} processado com sucesso. Gravado como {1}",
                                        mPaymentAttemptDepIdReturn.Header.NameOfCapturedFile,
                                        mPaymentAttemptDepIdReturn.Header.NameOfArquivedFile), SuperPag.LogFileEntryType.Information);

                                }
                                catch (Exception ex)
                                {
                                    SuperPag.Helper.GenericHelper.LogFile(ex.Message, SuperPag.LogFileEntryType.Error);

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
                Console.WriteLine(ex.Message);
            }
            finally
            {
                SuperPag.Helper.GenericHelper.LogFile("FIM DE PROCESSAMENTO: Serviço de Retorno de Depósito Identificado", SuperPag.LogFileEntryType.Information);
            }
        }
    }
}
