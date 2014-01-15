using System;
using System.Collections.Generic;
using System.Text;
using SuperPag.Data.Messages;
using SuperPag.Data;
using SuperPag.Helper;
using SuperPag.Agents.BB;
using SuperPag;

namespace SondaComercioEletronicoBB
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                GenericHelper.LogFile("SondaComercioEletronicoBB::Program.cs::Main Início da sonda", LogFileEntryType.Information);

                DPaymentAttemptBB[] attemptsBB = DataFactory.PaymentAttemptBB().ListForLead((int)PaymentAttemptStatus.NotPaid, (int)PaymentAttemptStatus.Pending, 2);
                if (Ensure.IsNull(attemptsBB))
                {
                    GenericHelper.LogFile("SondaComercioEletronicoBB::Program.cs::Main nenhuma tentativa de pagamento pendente de sonda", LogFileEntryType.Information);
                    return;
                }

                Sonda.UpdateStatus(attemptsBB);

                GenericHelper.LogFile("SondaComercioEletronicoBB::Program.cs::Main " + attemptsBB.Length + " tentativas de pagamento processadas", LogFileEntryType.Information);
            }
            catch (Exception e)
            {
                GenericHelper.LogFile("SondaComercioEletronicoBB::Program.cs::Main " + e.Message, LogFileEntryType.Error);
            }
        }
    }
}
