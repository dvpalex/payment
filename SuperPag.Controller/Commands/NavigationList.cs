using System;
using System.Collections;
using SuperPag.Framework.Web.WebController;
using SuperPag.Business.Messages;
using SuperPag.Business;
using SuperPag.Data.Messages;
using SuperPag.Data;
using SuperPag.Framework;
using SuperPag;

namespace Controller.Lib.Commands
{
    public class NavigationList : BaseCommand
    {
        protected override ViewInfo OnExecute()
        {
            MNavigationSearch mNavigationSearch = (MNavigationSearch)this.Parameters["NavigationSearch"];

            MCNavigationResult results = new MCNavigationResult();
            DWorkflowOrderStatus_Order[] woc = DataFactory.WorkflowOrderStatus().List(mNavigationSearch.CPF.Replace(".", "").Replace("-", "").Replace("/", ""), mNavigationSearch.InitialDate, mNavigationSearch.FinalDate.AddHours(23).AddMinutes(59).AddSeconds(59));
            if (woc == null)
            {
                this.AddMessage(results);
                return Map.Views.NavigationList;
            }
            
            foreach (DWorkflowOrderStatus_Order wo in woc)
            {
                string descricao = "";
                switch(wo.status)
                {
                    case (int)WorkflowOrderStatus.HandshakeFinished:
                        descricao = "Finalizou Handshake";
                        break;
                    case (int)WorkflowOrderStatus.ConsumerFilled:
                        descricao = "Completou dados de Consumidor";
                        break;
                    case (int)WorkflowOrderStatus.PaymentFormChoosed:
                        descricao = "Escolheu meio de pagamento: ";
                        if(!String.IsNullOrEmpty(wo.text))
                        {
                            int num = 0;
                            if (int.TryParse(wo.text, out num))
                            {
                                DPaymentForm pf = DataFactory.PaymentForm().Locate(num);
                                Ensure.IsNotNull(pf);
                                descricao += pf.name;
                            }
                        }
                        break;
                    case (int)WorkflowOrderStatus.InstallmentChoosed:
                        descricao = "Escolheu quantidade de parcelas: ";
                        if (!String.IsNullOrEmpty(wo.text))
                        {
                            string[] fields = wo.text.Split(new char[] { ',' });
                            if (fields != null && fields.Length > 1)
                            {
                                int num = 0;
                                if (int.TryParse(fields[fields.Length - 1], out num))
                                    descricao += num;
                            }
                        }
                        break;
                    case (int)WorkflowOrderStatus.SpecificInfoDefined:
                        descricao = "Definiu informações específicas do meio de pagamento";
                        break;
                    case (int)WorkflowOrderStatus.AgentCalled:
                        descricao = "Iniciou o processo de pagamento: ";
                        if(!String.IsNullOrEmpty(wo.text))
                        {
                            string []fields = wo.text.Split(new char[] { ',' });
                            if (fields != null && fields.Length > 1)
                            {
                                int num = 0;
                                if (int.TryParse(fields[fields.Length - 1], out num))
                                {
                                    DPaymentAgent pa = DataFactory.PaymentAgent().Locate(num);
                                    Ensure.IsNotNull(pa);
                                    descricao += pa.name;
                                }
                            }
                        }
                        break;
                    case (int)WorkflowOrderStatus.Finished:
                        descricao = "Pedido finalizado";
                        if(!String.IsNullOrEmpty(wo.text))
                            descricao += " (" + wo.text + ")";
                        break;
                    case (int)WorkflowOrderStatus.Error:
                        descricao = "Erro ocorrido: " + wo.text;
                        break;
                }
                results.Add(new MNavigationResult(mNavigationSearch.CPF, wo.dOrder.storeReferenceOrder, descricao, wo.creationDate));
            }

            this.AddMessage(results);
            return Map.Views.NavigationList;
        }
    }

    [Serializable]
    public class MNavigationResult : Message
    {
        private string cpf;
        private string storeReferenceOrder;
        private string description;
        private DateTime date;

        public DateTime Date
        {
            get { return date; }
            set { date = value; }
        }
        public string Description
        {
            get { return description; }
            set { description = value; }
        }
        public string StoreReferenceOrder
        {
            get { return storeReferenceOrder; }
            set { storeReferenceOrder = value; }
        }
        public string CPF
        {
            get { return cpf; }
            set { cpf = value; }
        }

        public MNavigationResult(string _cpf, string _storeReferenceOrder, string _description, DateTime _date)
        {
            cpf = _cpf;
            storeReferenceOrder = _storeReferenceOrder;
            description = _description;
            date = _date;
        }
	
    }

    [Serializable]
    [CollectionOf(typeof(MNavigationResult))]
    public class MCNavigationResult : MessageCollection
    {
    }

}

