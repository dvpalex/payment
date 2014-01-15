using System;
using System.Collections;
using SuperPag.Framework.Web.WebController;
using SuperPag.Framework.Web;
using SuperPag.Business.Messages;
using SuperPag.Business;
using Controller.Lib;
using Controller.Lib.Commands;

namespace Controller.Lib.Views.Ev.PaymentAgentSetupEdit
{
    public class Save : BaseEvent
    {
        protected override BaseCommand OnExecute()
        {
            BaseCommand b = null;

            MPaymentAgentSetup mPaymentAgentSetup = (MPaymentAgentSetup)this.GetMessage(typeof(MPaymentAgentSetup));

            PaymentAgentSetup.Save(mPaymentAgentSetup);

            switch (mPaymentAgentSetup.PaymentAgentId)
            {
                case (int)SuperPag.PaymentAgents.BBPag:
                    b = this.MakeCommand(typeof(EditPaymentAgentSetupBB));
                    break;
                case (int)SuperPag.PaymentAgents.VBV:
                case (int)SuperPag.PaymentAgents.VBVInBox:
                    b = this.MakeCommand(typeof(EditPaymentAgentSetupVBV));
                    break;
                case (int)SuperPag.PaymentAgents.Boleto:
                    b = this.MakeCommand(typeof(EditPaymentAgentSetupBoleto));
                    break;
                case (int)SuperPag.PaymentAgents.ItauShopLine:
                    b = this.MakeCommand(typeof(EditPaymentAgentSetupItaushopline));
                    break;
                case (int)SuperPag.PaymentAgents.FinanciamentoABN:
                    b = this.MakeCommand(typeof(EditPaymentAgentSetupABN));
                    break;
                case (int)SuperPag.PaymentAgents.PaymentClientVirtual3Party:
                case (int)SuperPag.PaymentAgents.PaymentClientVirtual2Party:
                    b = this.MakeCommand(typeof(EditPaymentAgentSetupPaymentclientvirtual));
                    break;
                case (int)SuperPag.PaymentAgents.VisaMoset:
                    b = this.MakeCommand(typeof(EditPaymentAgentSetupMoset));
                    break;
                case (int)SuperPag.PaymentAgents.Komerci:
                case (int)SuperPag.PaymentAgents.KomerciInBox:
                    b = this.MakeCommand(typeof(EditPaymentAgentSetupKomerci));
                    break;
                case (int)SuperPag.PaymentAgents.Bradesco:
                    b = this.MakeCommand(typeof(EditPaymentAgentSetupBradesco));
                    break;
                default:
                    throw new Exception("Não há tela implementada para este tipo de agente de pagamento");
            }
            

            b.Parameters["PaymentAgentSetupId"] = mPaymentAgentSetup.PaymentAgentSetupId;

            return b;
        }
    }

    public class Delete : BaseEvent
    {
        protected override BaseCommand OnExecute()
        {
            MPaymentAgentSetup mPaymentAgentSetup = (MPaymentAgentSetup)this.GetMessage(typeof(MPaymentAgentSetup));

            PaymentAgentSetup.Delete(mPaymentAgentSetup.PaymentAgentSetupId);
            
            BaseCommand b = this.LastView();

            return b;
        }
    }

    public class Cancel : BaseEvent
    {
        protected override BaseCommand OnExecute()
        {
            BaseCommand b = null;

            b = this.LastView();

            return b;
        }
    }
}
