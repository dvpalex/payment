using System;
using System.Collections;
using SuperPag.Framework.Web.WebController;
using SuperPag.Business.Messages;
using SuperPag.Business;
using SuperPag.Framework;
using Controller.Lib.Util;

namespace Controller.Lib.Commands
{
    public class StorePaymentFormEdit : BaseCommand
	{
		protected override ViewInfo OnExecute()
		{
            MStorePaymentForm mStorePaymentForm = (MStorePaymentForm)this.Parameters["StorePaymentForm"];
           
            MPaymentForm mPaymentForm = PaymentForm.Locate(mStorePaymentForm.PaymentFormId);

            //Adiciona as configurações de agentes disponíveis de acordo com a forma de pagamento escolhida
            MCPaymentAgentSetup mcPaymentAgentSetup = null;
            if (mPaymentForm != null)
                mcPaymentAgentSetup = PaymentAgentSetup.ListNameWithId(mPaymentForm.PaymentAgentId);
            this.AddMessage(mcPaymentAgentSetup);
            //Adiciona as formas de pagamentos disponiveis para lista no combo
            this.AddMessage(PaymentForm.ListNotUsePaymentForm(mStorePaymentForm.StoreId));

            this.AddMessage(mStorePaymentForm);
            
            return Map.Views.EditStorePaymentForm;
		}
	}

    public class InsertStorePaymentForm : BaseCommand
    {
        protected override ViewInfo OnExecute()
        {
            MStorePaymentForm mStorePaymentForm;

            if (this.Parameters["StoreId"] != null)
            {
                int storeId = (int)this.Parameters["StoreId"];
                mStorePaymentForm = new MStorePaymentForm();
                mStorePaymentForm.StoreId = storeId;
                mStorePaymentForm.IsInsert = true;
            }
            else
            {
                mStorePaymentForm = (MStorePaymentForm)this.Parameters["StorePaymentForm"];
            }
           
            MPaymentForm mPaymentForm = PaymentForm.Locate(mStorePaymentForm.PaymentFormId);
            
            //Adiciona as configurações de agentes disponíveis de acordo com a forma de pagamento escolhida
            MCPaymentAgentSetup mcPaymentAgentSetup = null;
            if (mPaymentForm != null)
                mcPaymentAgentSetup = PaymentAgentSetup.ListNameWithId(mPaymentForm.PaymentAgentId);
            this.AddMessage(mcPaymentAgentSetup);
            //Adiciona as formas de pagamentos disponiveis para lista no combo
            this.AddMessage(PaymentForm.ListNotUsePaymentForm(mStorePaymentForm.StoreId));

            this.AddMessage(mStorePaymentForm);
            
            return Map.Views.EditStorePaymentForm;
        }
    }

}
