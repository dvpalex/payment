using System;
using System.Configuration;
using System.Collections.Generic;
using System.Text;
using SuperPag.Data.Messages;
using SuperPag.Data;
using SuperPag.Helper;
using SuperPag.Agents.VBV;
using SuperPag.Agents.VBV.Messages;
using SuperPag;

namespace CapturaVBV
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                GenericHelper.LogFile("CapturaVBV::Program.cs::Main Início da sonda de captura", LogFileEntryType.Information);

                if (!String.IsNullOrEmpty(ConfigurationManager.AppSettings["TestAttempt"]))
                {
                    VBV vbv = new VBV(new Guid(ConfigurationManager.AppSettings["TestAttempt"]));
                    GenericHelper.LogFile("CapturaVBV::Program.cs::Main Job em modo teste para a paymentAttemptId: " + ConfigurationManager.AppSettings["TestAttempt"], LogFileEntryType.Information);
                    if (vbv.attemptVBV != null)
                    {
                        vbv.Capture((int)PaymentAttemptVBVInterfaces.CaptureJob);
                        GenericHelper.LogFile("CapturaVBV::Program.cs::Main Job em modo teste finalizou e deixou o status da attempt em: " + vbv.attempt.status.ToString(), LogFileEntryType.Information);
                    }
                    GenericHelper.LogFile("CapturaVBV::Program.cs::Main Fim do job em modo teste", LogFileEntryType.Information);
                    return;
                }
                
                if (String.IsNullOrEmpty(ConfigurationManager.AppSettings["StoreIds"]))
                    return;

                string[] stores = ConfigurationManager.AppSettings["StoreIds"].Split(",".ToCharArray());
                List<int> storeIds = new List<int>();
                foreach (string store in stores)
                    storeIds.Add(int.Parse(store));

                int daysBefore = String.IsNullOrEmpty(ConfigurationManager.AppSettings["DaysBefore"]) ? 1 : int.Parse(ConfigurationManager.AppSettings["DaysBefore"]);
                DateTime sinceDate = DateTime.Now.AddDays(-daysBefore);

                DPaymentAttemptVBV[] arrAttemptVBV = DataFactory.PaymentAttemptVBV().ListToCapture((int)PaymentAttemptVBVStatus.WaitingCapture, storeIds.ToArray(), sinceDate, (int)PaymentAttemptStatus.PendingPaid);
                if (Ensure.IsNull(arrAttemptVBV))
                {
                    GenericHelper.LogFile("CapturaVBV::Program.cs::Main Nenhuma tentativa de pagamento pendente de sonda de captura", LogFileEntryType.Information);
                    return;
                }
                GenericHelper.LogFile("CapturaVBV::Program.cs::Main Job em modo normal iniciando para " + arrAttemptVBV.Length.ToString() + " tentativas", LogFileEntryType.Information);

                foreach (DPaymentAttemptVBV attemptVBV in arrAttemptVBV)
                {
                    VBV vbv = new VBV(attemptVBV);
                    vbv.Capture((int)PaymentAttemptVBVInterfaces.CaptureJob);
                }

                GenericHelper.LogFile("CapturaVBV::Program.cs::Main " + arrAttemptVBV.Length + " tentativas de pagamento processadas", LogFileEntryType.Information);
            }
            catch (Exception e)
            {
                GenericHelper.LogFile("CapturaVBV::Program.cs::Main " + e.Message, LogFileEntryType.Error);
            }
        }
    }
}
