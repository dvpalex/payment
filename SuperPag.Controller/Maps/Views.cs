using System;
using SuperPag.Framework.Web.WebController;

namespace Controller.Lib.Map {
	
	/// <summary>
	/// Summary description for Views.
	/// </summary>
	public class Views{

		public static ViewInfo Home = new ViewInfo("Home", "Home", "~/Views/default.aspx");

        public static ViewInfo ChangePassword = new ViewInfo("Alterar Senha", "ChangePassword", "~/Views/changepassword.aspx");

        public static ViewInfo SearchTransaction = new ViewInfo("Pesquisar Transações", "SearchTransaction", "~/Views/transactionsearch.aspx");

        public static ViewInfo ListTransaction = new ViewInfo("Listagem de Transações", "ListTransaction", "~/Views/transactionlist.aspx");

        public static ViewInfo ShowTransaction = new ViewInfo("Detalhe da Transação", "ShowTransaction", "~/Views/transactiondetail.aspx");

        public static ViewInfo SearchOrder = new ViewInfo("Pesquisar Pedidos", "SearchOrder", "~/Views/ordersearch.aspx");

        public static ViewInfo ListOrder = new ViewInfo("Listagem de Pedidos", "ListOrder", "~/Views/orderlist.aspx");

        public static ViewInfo ListOrderDetailItem = new ViewInfo("Listagem de Pedidos com Detalhamento de Itens", "ListOrderDetailItem", "~/Views/orderdetailitemlist.aspx");

        public static ViewInfo ShowOrder = new ViewInfo("Detalhe do Pedido", "ShowOrder", "~/Views/orderdetail.aspx");

        public static ViewInfo ShowOrderTransaction = new ViewInfo("Detalhe da Transação do Pedido", "ShowOrderTransaction", "~/Views/ordertransactiondetail.aspx");

        public static ViewInfo ListPosts = new ViewInfo("Listagem de Transações", "ListPosts", "~/Views/postslist.aspx");

        public static ViewInfo Extrato = new ViewInfo("Extrato Financeiro", "Extrato", "~/Views/extrato.aspx");

        public static ViewInfo ExtratoFilter = new ViewInfo("Extrato", "ExtratoFilter", "~/Views/extratofilter.aspx");
        
        public static ViewInfo ExtratoFilterFinancial2 = new ViewInfo("Extrato", "ExtratoFilterFinancial2", "~/Views/extratofilterfinancial2.aspx");

        public static ViewInfo ExtratoFinancial = new ViewInfo("Extrato Financeiro", "ExtratoFinancial", "~/Views/extratofinancial.aspx");

        public static ViewInfo ExtratoFinancial2 = new ViewInfo("Extrato Financeiro Herbalife", "ExtratoFinancial2", "~/Views/extratofinancial2.aspx");

        public static ViewInfo ExtratoMovement = new ViewInfo("Extrato de Movimento", "ExtratoMovement", "~/Views/extratomovement.aspx");

        public static ViewInfo NavigationFilter = new ViewInfo("Navegação", "NavigationFilter", "~/Views/navigationfilter.aspx");

        public static ViewInfo NavigationList = new ViewInfo("Navegação", "NavigationList", "~/Views/navigationlist.aspx");

        public static ViewInfo RefazerBoleto = new ViewInfo("Boleto Bancário", "RefazerBoleto", "~/Views/refazerboleto.aspx");

        public static ViewInfo ReenviarBoleto = new ViewInfo("Boleto Bancário", "ReenviarBoleto", "~/Views/reenviarboleto.aspx");

        public static ViewInfo ListStores = new ViewInfo("Lojas cadastradas", "ListStores", "~/Views/Config/storelist.aspx");
        public static ViewInfo ShowStore = new ViewInfo("Configuração da loja", "ShowStore", "~/Views/Config/storedetail.aspx");
        public static ViewInfo EditStore = new ViewInfo("Cadastro de loja", "EditStore", "~/Views/Config/storeedit.aspx");

        public static ViewInfo ListPaymentAgentSetup = new ViewInfo("Configurações de agentes", "ListPaymentAgentSetup", "~/Views/Config/paymentagentsetuplist.aspx");
        public static ViewInfo EditPaymentAgentSetup = new ViewInfo("Cadastro de configuração de agente de pagamento", "EditPaymentAgentSetup", "~/Views/Config/paymentagentsetupedit.aspx");

        public static ViewInfo EditPaymentAgentSetupABN = new ViewInfo("Configurações de agentes", "EditPaymentAgentSetupABN", "~/Views/Config/paymentagentsetupABNedit.aspx");
        public static ViewInfo EditPaymentAgentSetupBB = new ViewInfo("Configurações de agente BBPag", "EditPaymentAgentSetupBB", "~/Views/Config/paymentagentsetupBBedit.aspx");
        public static ViewInfo EditPaymentAgentSetupBoleto = new ViewInfo("Configurações de agente Boleto", "EditPaymentAgentSetupBoleto", "~/Views/Config/paymentagentsetupBoletoedit.aspx");
        public static ViewInfo EditPaymentAgentSetupBradesco = new ViewInfo("Configurações de agente Pagamento fácil Bradesco", "EditPaymentAgentSetupBradesco", "~/Views/Config/paymentagentsetupBradescoedit.aspx");
        public static ViewInfo EditPaymentAgentSetupItauShopLine = new ViewInfo("Configurações de agente Itaú ShopLine", "EditPaymentAgentSetupItauShopLine", "~/Views/Config/paymentagentsetupItaushoplineedit.aspx");
        public static ViewInfo EditPaymentAgentSetupKomerci = new ViewInfo("Configurações de agente Komerci", "EditPaymentAgentSetupKomerci", "~/Views/Config/paymentagentsetupkomerciedit.aspx");
        public static ViewInfo EditPaymentAgentSetupMoset = new ViewInfo("Configurações de agente Moset", "EditPaymentAgentSetupMoset", "~/Views/Config/paymentagentsetupMosetedit.aspx");
        public static ViewInfo EditPaymentAgentSetupPaymentClientVirtual = new ViewInfo("Configurações de agente Amex", "EditPaymentAgentSetupPaymentClientVirtual", "~/Views/Config/paymentagentsetuppaymentclientedit.aspx");
        public static ViewInfo EditPaymentAgentSetupVBV = new ViewInfo("Configurações de agente VBV", "EditPaymentAgentSetupVBV", "~/Views/Config/paymentagentsetupVBVedit.aspx");

        public static ViewInfo EditStorePaymentForm = new ViewInfo("Configurações de forma de pagamento para loja", "EditStorePaymentForm", "~/Views/Config/storepaymentformedit.aspx");
        public static ViewInfo ShowStorePaymentForm = new ViewInfo("Detalhes da forma de pagamento para loja", "ShowStorePaymentForm", "~/Views/Config/storepaymentformdetail.aspx");

        public static ViewInfo EditStorePaymentInstallment = new ViewInfo("Cadastro de parcelamento", "EditStorePaymentInstallment", "~/Views/Config/storepaymentinstallmentsedit.aspx");

        public static ViewInfo ListHandshakeConfiguration = new ViewInfo("Handshakes cadastrados", "ListHandshakeConfiguration", "~/Views/Config/handshakeconfigurationlist.aspx");
        public static ViewInfo EditHandshakeConfiguration = new ViewInfo("Cadastro de handshake", "EditHandshakeConfiguration", "~/Views/Config/handshakeconfigurationedit.aspx");
        public static ViewInfo EditHandshakeConfigurationHtml = new ViewInfo("Cadastro de handshake Html", "EditHandshakeConfigurationHtml", "~/Views/Config/handshakeconfigurationhtmledit.aspx");
        public static ViewInfo EditHandshakeConfigurationXml = new ViewInfo("Cadastro de handshake Xml", "EditHandshakeConfigurationXml", "~/Views/Config/handshakeconfigurationxmledit.aspx");

    }
}
