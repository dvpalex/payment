using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using SuperPag.Handshake;
using System.Configuration;
using System.Configuration.Assemblies;
using SuperPag;

namespace EnviaArqCorreio
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string strbody, subject;

                strbody = "Em anexo o arquivo para processamento. Diariamente estes arquivos serão encaminhados pelo sistema SUPERPAG e processados pela Marpress";
                subject = "Boleto PontoCred " + DateTime.Now.ToString() + " - Tivit";

                string[] strMail = new string[5];

                strMail[0] = ConfigurationSettings.AppSettings["EmailDestino1"].ToString();
                strMail[1] = ConfigurationSettings.AppSettings["EmailDestino2"].ToString();
                strMail[2] = ConfigurationSettings.AppSettings["EmailDestino3"].ToString();
                strMail[3] = ConfigurationSettings.AppSettings["EmailDestino4"].ToString();
                strMail[4] = ConfigurationSettings.AppSettings["EmailDestino5"].ToString();

                foreach (string str in Directory.GetFiles(ConfigurationSettings.AppSettings["PathCorreioIn"].ToString()))
                {
                    if (Helper.SendBoletoEmail(strMail[0].ToString(), str, strbody, subject, strMail))
                    {
                        bool falg = false;
                        while (falg == false)
                        {
                            try
                            {
                                Directory.Move(str, (ConfigurationSettings.AppSettings["PathCorreioOut"].ToString() + str.Substring(str.LastIndexOf("\\")).Replace("\\", string.Empty)));
                                falg = true;
                            }
                            catch { }
                        }

                        SuperPag.Helper.GenericHelper.LogFile("EnviaCorreio:: Job EnviaArqCorreio enviou arquivo com sucesso! " + DateTime.Now.ToString(), LogFileEntryType.Information);
                    }
                    else
                    {
                        try
                        {
                            Helper.SendBoletoEmail(strMail[0], string.Empty, "Erro ao enviar o arquivo ao carreio!!", "Erro superpag - Envio de arquivo ao correio", strMail);
                        }
                        catch { }

                        SuperPag.Helper.GenericHelper.LogFile("EnviaCorreio:: O job não enviou o arquivo " + DateTime.Now.ToString(), LogFileEntryType.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                SuperPag.Helper.GenericHelper.LogFile("EnviaCorreio:: O job não enviou o arquivo " + DateTime.Now.ToString() + " " + ex.ToString(), LogFileEntryType.Error);
            }
        }
    }
}
