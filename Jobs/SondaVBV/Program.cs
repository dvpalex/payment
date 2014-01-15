using System;
using System.Collections.Generic;
using System.Text;
using SuperPag.Data.Messages;
using SuperPag.Data;
using SuperPag.Helper;
using SuperPag;
using SuperPag.Agents.VBV;

namespace SondaVBV
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                GenericHelper.LogFile("SondaVBV::Program.cs::Main Início da sonda", LogFileEntryType.Information);

                DPaymentAttemptVBV[] arrAttemptVBV = DataFactory.PaymentAttemptVBV().ListForLead(new int[] { (int)PaymentAttemptStatus.Pending, (int)PaymentAttemptStatus.PendingPaid }, 2, DateTime.Now.AddMinutes(-20));
                if (Ensure.IsNull(arrAttemptVBV))
                {
                    GenericHelper.LogFile("SondaVBV::Program.cs::Main nenhuma tentativa de pagamento pendente de sonda", LogFileEntryType.Information);
                    return;
                }

                VBV.CheckStatus(arrAttemptVBV);

                GenericHelper.LogFile("SondaVBV::Program.cs::Main " + arrAttemptVBV.Length + " tentativas de pagamento processadas", LogFileEntryType.Information);
            }
            catch (Exception e)
            {
                GenericHelper.LogFile("SondaVBV::Program.cs::Main " + e.Message, LogFileEntryType.Error);
            }
        }
    }
}
