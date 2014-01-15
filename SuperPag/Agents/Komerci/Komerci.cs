using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using SuperPag.Helper;

namespace SuperPag.Agents.Komerci
{
    public class Komerci
    {
        public static string GeraCodVerificacao(string NumFil, string Total, string IPAddress)
        {
            try
            {
                Process p = new Process();
                ProcessStartInfo startInfo = new ProcessStartInfo();

                startInfo.Arguments = "-classpath \"" + System.Configuration.ConfigurationManager.AppSettings["KomerciJavaClasses"] + "\" CodVer " + NumFil + " " + Total + " " + IPAddress;

                startInfo.FileName = System.Configuration.ConfigurationManager.AppSettings["JavaVM"];
                startInfo.UseShellExecute = false;
                startInfo.CreateNoWindow = true;
                startInfo.RedirectStandardOutput = true;
                startInfo.RedirectStandardError = true;

                p.StartInfo = startInfo;
                p.Start();

                string codever = p.StandardOutput.ReadToEnd();
                string error = p.StandardError.ReadToEnd();

                if (error != null && error.Trim() != "")
                {
                    GenericHelper.LogFile("EasyPagObject::Komerci.cs::GeraCodVerificacao " + error.Replace("\r\n", " ") + " arguments=" + startInfo.Arguments, LogFileEntryType.Error);
                    return "";
                }

                if (String.IsNullOrEmpty(codever))
                {
                    GenericHelper.LogFile("EasyPagObject::Komerci.cs::GeraCodVerificacao codver é nulo ou vazio arguments=" + startInfo.Arguments, LogFileEntryType.Error);
                    return "";
                }
                
                return codever.Trim();
            }
            catch (Exception e)
            {
                GenericHelper.LogFile("EasyPagObject::Komerci.cs::GeraCodVerificacao " + e.Message, LogFileEntryType.Error);
                return "";
            }
        }
        
        public static bool ValidaAVS(string respavs, string acceptedavs)
        {
            if (respavs.ToUpper() == "W")
                return true;

            if (String.IsNullOrEmpty(acceptedavs))
                return false;
            
            foreach (string regra in acceptedavs.Split(",".ToCharArray()))
                if (respavs.ToUpper() == regra.ToUpper())
                    return true;

            return false;
        }
        
        public static bool BinExcecao(string bin, string listabins)
        {
            if (String.IsNullOrEmpty(listabins) || String.IsNullOrEmpty(bin) || bin.Length < 6)
                return false;

            foreach (string binex in listabins.Split(",".ToCharArray()))
                if (bin.Substring(0, 6).ToUpper() == binex.ToUpper())
                    return true;

            return false;
        }
    }
}
