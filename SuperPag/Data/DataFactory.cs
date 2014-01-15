using System;
using System.Configuration;
using System.Web.Configuration;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Data.Interfaces;

namespace SuperPag.Data
{
    public class DataFactory
    {

        //private static System.Configuration.Configuration config = WebConfigurationManager.OpenWebConfiguration(System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath);
        //public static readonly string strCon = config.ConnectionStrings.ConnectionStrings["fastpag"].ConnectionString;
        public static readonly string strCon = ConfigurationManager.ConnectionStrings["fastpag"].ConnectionString;

        public static IBilling Billing()
        {
            return (IBilling)DataLayerCache.GetCachedDataLayer(typeof(IBilling), strCon);
        }
        public static IBillingSchedule BillingSchedule()
        {
            return (IBillingSchedule)DataLayerCache.GetCachedDataLayer(typeof(IBillingSchedule), strCon);
        }
        public static IRecurrence Recurrence()
        {
            return (IRecurrence)DataLayerCache.GetCachedDataLayer(typeof(IRecurrence), strCon);
        }
        public static ISchedule Schedule()
        {
            return (ISchedule)DataLayerCache.GetCachedDataLayer(typeof(ISchedule), strCon);
        }
        public static IConsumer Consumer()
        {
            return (IConsumer)DataLayerCache.GetCachedDataLayer(typeof(IConsumer), strCon);
        }
        public static IConsumerAddress ConsumerAddress()
        {
            return (IConsumerAddress)DataLayerCache.GetCachedDataLayer(typeof(IConsumerAddress), strCon);
        }
        public static IHandshakeConfiguration HandshakeConfiguration()
        {
            return (IHandshakeConfiguration)DataLayerCache.GetCachedDataLayer(typeof(IHandshakeConfiguration), strCon);
        }
        public static IHandshakeConfigurationHtml HandshakeConfigurationHtml()
        {
            return (IHandshakeConfigurationHtml)DataLayerCache.GetCachedDataLayer(typeof(IHandshakeConfigurationHtml), strCon);
        }
        public static IHandshakeConfigurationXml HandshakeConfigurationXml()
        {
            return (IHandshakeConfigurationXml)DataLayerCache.GetCachedDataLayer(typeof(IHandshakeConfigurationXml), strCon);
        }
        public static IHandshakeSessionLog HandshakeSessionLog()
        {
            return (IHandshakeSessionLog)DataLayerCache.GetCachedDataLayer(typeof(IHandshakeSessionLog), strCon);
        }
        public static IHandshakeSession HandshakeSession()
        {
            return (IHandshakeSession)DataLayerCache.GetCachedDataLayer(typeof(IHandshakeSession), strCon);
        }
        public static IOrder Order()
        {
            return (IOrder)DataLayerCache.GetCachedDataLayer(typeof(IOrder), strCon);
        }
        public static IOrderCreditCard OrderCreditCard()
        {
            return (IOrderCreditCard)DataLayerCache.GetCachedDataLayer(typeof(IOrderCreditCard), strCon);
        }
        public static IOrderInstallment OrderInstallment()
        {
            return (IOrderInstallment)DataLayerCache.GetCachedDataLayer(typeof(IOrderInstallment), strCon);
        }
        public static IOrderItem OrderItem()
        {
            return (IOrderItem)DataLayerCache.GetCachedDataLayer(typeof(IOrderItem), strCon);
        }
        public static IPaymentAgent PaymentAgent()
        {
            return (IPaymentAgent)DataLayerCache.GetCachedDataLayer(typeof(IPaymentAgent), strCon);
        }
        public static IPaymentAgentSetup PaymentAgentSetup()
        {
            return (IPaymentAgentSetup)DataLayerCache.GetCachedDataLayer(typeof(IPaymentAgentSetup), strCon);
        }
        public static IPaymentSummary PaymentSummary()
        {
            return (IPaymentSummary)DataLayerCache.GetCachedDataLayer(typeof(IPaymentSummary), strCon);
        }
        public static IPaymentAgentSetupBoleto PaymentAgentSetupBoleto()
        {
            return (IPaymentAgentSetupBoleto)DataLayerCache.GetCachedDataLayer(typeof(IPaymentAgentSetupBoleto), strCon);
        }
        public static IPaymentAgentSetupDepId PaymentAgentSetupDepId()
        {
            return (IPaymentAgentSetupDepId)DataLayerCache.GetCachedDataLayer(typeof(IPaymentAgentSetupDepId), strCon);
        }
        public static IPaymentAgentSetupABN PaymentAgentSetupABN()
        {
            return (IPaymentAgentSetupABN)DataLayerCache.GetCachedDataLayer(typeof(IPaymentAgentSetupABN), strCon);
        }
        public static IPaymentAgentSetupBB PaymentAgentSetupBB()
        {
            return (IPaymentAgentSetupBB)DataLayerCache.GetCachedDataLayer(typeof(IPaymentAgentSetupBB), strCon);
        }
        public static IPaymentAgentSetupBradesco PaymentAgentSetupBradesco()
        {
            return (IPaymentAgentSetupBradesco)DataLayerCache.GetCachedDataLayer(typeof(IPaymentAgentSetupBradesco), strCon);
        }
        public static IPaymentAgentSetupItauShopline PaymentAgentSetupItauShopline()
        {
            return (IPaymentAgentSetupItauShopline)DataLayerCache.GetCachedDataLayer(typeof(IPaymentAgentSetupItauShopline), strCon);
        }
        public static IPaymentAgentSetupKomerci PaymentAgentSetupKomerci()
        {
            return (IPaymentAgentSetupKomerci)DataLayerCache.GetCachedDataLayer(typeof(IPaymentAgentSetupKomerci), strCon);
        }
        public static IPaymentAgentSetupVBV PaymentAgentSetupVBV()
        {
            return (IPaymentAgentSetupVBV)DataLayerCache.GetCachedDataLayer(typeof(IPaymentAgentSetupVBV), strCon);
        }
        public static IPaymentAgentSetupMoset PaymentAgentSetupMoset()
        {
            return (IPaymentAgentSetupMoset)DataLayerCache.GetCachedDataLayer(typeof(IPaymentAgentSetupMoset), strCon);
        }
        public static IPaymentAgentSetupPaymentClientVirtual PaymentAgentSetupPaymentClientVirtual()
        {
            return (IPaymentAgentSetupPaymentClientVirtual)DataLayerCache.GetCachedDataLayer(typeof(IPaymentAgentSetupPaymentClientVirtual), strCon);
        }
        public static IPaymentAttempt PaymentAttempt()
        {
            return (IPaymentAttempt)DataLayerCache.GetCachedDataLayer(typeof(IPaymentAttempt), strCon);
        }
        public static IPaymentAttemptABN PaymentAttemptABN()
        {
            return (IPaymentAttemptABN)DataLayerCache.GetCachedDataLayer(typeof(IPaymentAttemptABN), strCon);
        }
        public static IPaymentAttemptBB PaymentAttemptBB()
        {
            return (IPaymentAttemptBB)DataLayerCache.GetCachedDataLayer(typeof(IPaymentAttemptBB), strCon);
        }
        public static IPaymentAttemptDepId PaymentAttemptDepId()
        {
            return (IPaymentAttemptDepId)DataLayerCache.GetCachedDataLayer(typeof(IPaymentAttemptDepId), strCon);
        }
        public static IPaymentAttemptDepIdReturnHeader PaymentAttemptDepIdReturnHeader()
        {
            return (IPaymentAttemptDepIdReturnHeader)DataLayerCache.GetCachedDataLayer(typeof(IPaymentAttemptDepIdReturnHeader), strCon);
        }
        public static IPaymentAttemptDepIdReturn PaymentAttemptDepIdReturn()
        {
            return (IPaymentAttemptDepIdReturn)DataLayerCache.GetCachedDataLayer(typeof(IPaymentAttemptDepIdReturn), strCon);
        }
        public static IPaymentAttemptDepIdReturnChk PaymentAttemptDepIdReturnChk()
        {
            return (IPaymentAttemptDepIdReturnChk)DataLayerCache.GetCachedDataLayer(typeof(IPaymentAttemptDepIdReturnChk), strCon);
        }
        public static IPaymentAttemptBoleto PaymentAttemptBoleto()
        {
            return (IPaymentAttemptBoleto)DataLayerCache.GetCachedDataLayer(typeof(IPaymentAttemptBoleto), strCon);
        }
        public static IPaymentAttemptBoletoReturn PaymentAttemptBoletoReturn()
        {
            return (IPaymentAttemptBoletoReturn)DataLayerCache.GetCachedDataLayer(typeof(IPaymentAttemptBoletoReturn), strCon);
        }
        public static IPaymentAttemptBoletoReturnHeader PaymentAttemptBoletoReturnHeader()
        {
            return (IPaymentAttemptBoletoReturnHeader)DataLayerCache.GetCachedDataLayer(typeof(IPaymentAttemptBoletoReturnHeader), strCon);
        }
        public static IPaymentAttemptBradesco PaymentAttemptBradesco()
        {
            return (IPaymentAttemptBradesco)DataLayerCache.GetCachedDataLayer(typeof(IPaymentAttemptBradesco), strCon);
        }
        public static IPaymentAttemptItauShopline PaymentAttemptItauShopline()
        {
            return (IPaymentAttemptItauShopline)DataLayerCache.GetCachedDataLayer(typeof(IPaymentAttemptItauShopline), strCon);
        }
        public static IPaymentAttemptKomerci PaymentAttemptKomerci()
        {
            return (IPaymentAttemptKomerci)DataLayerCache.GetCachedDataLayer(typeof(IPaymentAttemptKomerci), strCon);
        }
        public static IPaymentAttemptKomerciWS PaymentAttemptKomerciWS()
        {
            return (IPaymentAttemptKomerciWS)DataLayerCache.GetCachedDataLayer(typeof(IPaymentAttemptKomerciWS), strCon);
        }
        public static IPaymentAttemptVBV PaymentAttemptVBV()
        {
            return (IPaymentAttemptVBV)DataLayerCache.GetCachedDataLayer(typeof(IPaymentAttemptVBV), strCon);
        }
        public static IPaymentAttemptVBVLog PaymentAttemptVBVLog()
        {
            return (IPaymentAttemptVBVLog)DataLayerCache.GetCachedDataLayer(typeof(IPaymentAttemptVBVLog), strCon);
        }
        public static IPaymentAttemptMoset PaymentAttemptMoset()
        {
            return (IPaymentAttemptMoset)DataLayerCache.GetCachedDataLayer(typeof(IPaymentAttemptMoset), strCon);
        }
        public static IPaymentAttemptPaymentClientVirtual PaymentAttemptPaymentClientVirtual()
        {
            return (IPaymentAttemptPaymentClientVirtual)DataLayerCache.GetCachedDataLayer(typeof(IPaymentAttemptPaymentClientVirtual), strCon);
        }
        public static IPaymentForm PaymentForm()
        {
            return (IPaymentForm)DataLayerCache.GetCachedDataLayer(typeof(IPaymentForm), strCon);
        }
        public static IPaymentFormGroup PaymentFormGroup()
        {
            return (IPaymentFormGroup)DataLayerCache.GetCachedDataLayer(typeof(IPaymentFormGroup), strCon);
        }
        public static IServiceEmailPaymentForm ServiceEmailPaymentForm()
        {
            return (IServiceEmailPaymentForm)DataLayerCache.GetCachedDataLayer(typeof(IServiceEmailPaymentForm), strCon);
        }
        public static IServiceFinalizationPost ServiceFinalizationPost()
        {
            return (IServiceFinalizationPost)DataLayerCache.GetCachedDataLayer(typeof(IServiceFinalizationPost), strCon);
        }
        public static IServicePaymentPost ServicePaymentPost()
        {
            return (IServicePaymentPost)DataLayerCache.GetCachedDataLayer(typeof(IServicePaymentPost), strCon);
        }
        public static IServicesConfiguration ServicesConfiguration()
        {
            return (IServicesConfiguration)DataLayerCache.GetCachedDataLayer(typeof(IServicesConfiguration), strCon);
        }
        public static ISPLegacyPaymentForm SPLegacyPaymentForm()
        {
            return (ISPLegacyPaymentForm)DataLayerCache.GetCachedDataLayer(typeof(ISPLegacyPaymentForm), strCon);
        }
        public static ISPLegacyPaymentGroup SPLegacyPaymentGroup()
        {
            return (ISPLegacyPaymentGroup)DataLayerCache.GetCachedDataLayer(typeof(ISPLegacyPaymentGroup), strCon);
        }
        public static ISPLegacyStore SPLegacyStore()
        {
            return (ISPLegacyStore)DataLayerCache.GetCachedDataLayer(typeof(ISPLegacyStore), strCon);
        }
        public static IStore Store()
        {
            return (IStore)DataLayerCache.GetCachedDataLayer(typeof(IStore), strCon);
        }
        public static IStorePaymentForm StorePaymentForm()
        {
            return (IStorePaymentForm)DataLayerCache.GetCachedDataLayer(typeof(IStorePaymentForm), strCon);
        }
        public static IStorePaymentInstallment StorePaymentInstallment()
        {
            return (IStorePaymentInstallment)DataLayerCache.GetCachedDataLayer(typeof(IStorePaymentInstallment), strCon);
        }
        public static IUsers Users()
        {
            return (IUsers)DataLayerCache.GetCachedDataLayer(typeof(IUsers), strCon);
        }
        public static IRoles Roles()
        {
            return (IRoles)DataLayerCache.GetCachedDataLayer(typeof(IRoles), strCon);
        }
        public static IUsersInStore UsersInStore()
        {
            return (IUsersInStore)DataLayerCache.GetCachedDataLayer(typeof(IUsersInStore), strCon);
        }
        public static IUsersInRoles UsersInRoles()
        {
            return (IUsersInRoles)DataLayerCache.GetCachedDataLayer(typeof(IUsersInRoles), strCon);
        }
        public static IWorkflowOrderStatus WorkflowOrderStatus()
        {
            return (IWorkflowOrderStatus)DataLayerCache.GetCachedDataLayer(typeof(IWorkflowOrderStatus), strCon);
        }
        public static IExtratoFinanceiro2 ExtratoFinanceiro2()
        {
            return (IExtratoFinanceiro2)DataLayerCache.GetCachedDataLayer(typeof(IExtratoFinanceiro2), strCon);
        }
    }
}
