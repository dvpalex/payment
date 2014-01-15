using System;
using System.Collections.Generic;
using System.Text;
using SuperPag.Data.Messages;
using SuperPag.Data;
using SuperPag.Helper;
using SuperPag.Agents.ItauShopLine;
using SuperPag;

namespace SondaItauShopline
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                GenericHelper.LogFile("SondaItauShopline::Program.cs::Main Início da sonda", LogFileEntryType.Information);
                
                DPaymentAttemptItauShopline[] arrAttemptItau = DataFactory.PaymentAttemptItauShopline().ListForLead((int)PaymentAttemptStatus.NotPaid, (int)PaymentAttemptStatus.Pending, 2);
                if (Ensure.IsNull(arrAttemptItau))
                {
                    GenericHelper.LogFile("SondaItauShopline::Program.cs::Main nenhuma tentativa de pagamento pendente de sonda", LogFileEntryType.Information);
                    return;
                }

                Sonda.UpdateStatus(arrAttemptItau);

                GenericHelper.LogFile("SondaItauShopline::Program.cs::Main " + arrAttemptItau.Length + " tentativas de pagamento processadas", LogFileEntryType.Information);
            }
            catch (Exception e)
            {
                GenericHelper.LogFile("SondaItauShopline::Program.cs::Main " + e.Message, LogFileEntryType.Error);
            }
        }
    }
}
