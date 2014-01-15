using System;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Configuration;
using System.Reflection;
using SuperPag.Helper;
using SuperPag.Helper.Xml.Request;
using SuperPag.Helper.Xml.Response;
using SuperPag.Helper.Xml.Update;
using SuperPag.Data.Messages;
using SuperPag.Data;
using SuperPag;
using SuperPag.Helper.Xml;
using SuperPag.Handshake.Service;

/// <summary>
/// Summary description for Payment
/// </summary>
[WebService(Namespace = "http://www.superpag.com.br/Services")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
public class Payment : System.Web.Services.WebService
{
    const string RESPONSE = "<response><order id=\"\" reference=\"PT001\" status=\"1\"><payments><payment status=\"1\" form=\"2\"><installments><installment number=\"1\" status=\"1\"><tid></tid></installment><installment number=\"2\" status=\"1\"><tid></tid></installment></installments></payment></payments></order></response>";
    
    public Payment()
    {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    public string GetTicket(string storeKey, string password, string XML)
    {
        return Guid.NewGuid().ToString();
    }

    [WebMethod]
    public string SendOrder(string storeKey, string password, string xml)
    {
        try
        {
            Ensure.IsNotNullOrEmpty(storeKey, "Chave da loja não informada");
            Ensure.IsNotNullOrEmpty(xml, "Xml de request não informado");

            DStore store = DataFactory.Store().Locate(storeKey);
            Ensure.IsNotNull(store, "A chave {0} é inválida", storeKey);

            if (Ensure.IsNotNullOrEmpty(store.password) && Ensure.IsNull(password))
                Ensure.IsNotNullOrEmpty(null, "Senha não informada");

            if (Ensure.IsNotNullOrEmpty(store.password) && store.password != GenericHelper.CalculateHash(password))
                Ensure.IsNotNull(null, "A senha para a chave {0} está incorreta", storeKey);

            GenericHelper.LogFile("SuperPagWS::Payment.cs::SendOrder storeId=" + store.storeId + " request xml=" + xml, LogFileEntryType.Information);
            
            string requestxsd = "";
            if (ConfigurationManager.AppSettings != null && ConfigurationManager.AppSettings["RequestXsd"] != null)
                requestxsd = ConfigurationManager.AppSettings["RequestXsdFile"];
            else
                requestxsd = HttpContext.Current.Server.MapPath(".\\Xsd\\Request.xsd");

            Parse xmlparse = new Parse();
            if (!xmlparse.Xml(xml, requestxsd, null))
                Ensure.IsNotNull(null, xmlparse.Error);

            request req;
            string msgerror = "";
            if ((req = (request)XmlHelper.GetClass(xml, typeof(request), out msgerror)) == null)
                Ensure.IsNotNull(null, msgerror);

            Request requestHelper = new Request();

            if (SuperPag.Business.Order.GetDuplicateOrder(req.orders[0].reference,store.storeId))
                Ensure.IsNotNullOrEmpty(null, "Pedido duplicado.");

            response resp = requestHelper.ProcessRequest(store, req);
            
            
            string s = XmlHelper.GetXml(typeof(response), resp);

            GenericHelper.LogFile("SuperPagWS::Payment.cs::SendOrder storeId=" + store.storeId + " response xml=" + s, LogFileEntryType.Information);

            return s;
        }
        catch (ApplicationException e)
        {
            GenericHelper.LogFile("SuperPagWS::Payment.cs::SendOrder " + e.Message, LogFileEntryType.Warning);

            return e.Message;
        }
        catch (Exception e)
        {
            GenericHelper.LogFile("SuperPagWS::Payment.cs::SendOrder " + e.Message, LogFileEntryType.Error);

            SoapException se = new SoapException("Erro interno: " + e.Message + (e.InnerException != null ? e.InnerException.Message : ""), SoapException.ClientFaultCode, Context.Request.Url.AbsoluteUri);
            throw se;
        }
    }
    
    [WebMethod]
    public string CheckOrder(string storeKey, string password, int orderId)
    {
        try
        {
            Ensure.IsNotNullOrEmpty(storeKey, "Chave da loja não informada");

            DStore store = DataFactory.Store().Locate(storeKey);
            Ensure.IsNotNull(store, "A chave {0} é inválida", storeKey);

            if (Ensure.IsNotNullOrEmpty(store.password) && Ensure.IsNull(password))
                Ensure.IsNotNullOrEmpty(null, "Senha não informada");

            if (Ensure.IsNotNullOrEmpty(store.password) && store.password != GenericHelper.CalculateHash(password))
                Ensure.IsNotNull(null, "A senha para a chave {0} está incorreta", storeKey);

            GenericHelper.LogFile("SuperPagWS::Payment.cs::CheckOrder storeId=" + store.storeId + " orderId=" + orderId.ToString(), LogFileEntryType.Information);

            Check checkHelper = new Check();
            response resp = checkHelper.ProcessCheck(store, orderId);

            string s = XmlHelper.GetXml(typeof(response), resp);

            GenericHelper.LogFile("SuperPagWS::Payment.cs::CheckOrder storeId=" + store.storeId + " response xml=" + s, LogFileEntryType.Information);

            return s;
        }
        catch (ApplicationException e)
        {
            GenericHelper.LogFile("SuperPagWS::Payment.cs::CheckOrder " + e.Message, LogFileEntryType.Warning);

            return e.Message;
        }
        catch (Exception e)
        {
            GenericHelper.LogFile("SuperPagWS::Payment.cs::CheckOrder " + e.Message, LogFileEntryType.Error);

            SoapException se = new SoapException("Erro interno: " + e.Message + (e.InnerException != null ? e.InnerException.Message : ""), SoapException.ClientFaultCode, Context.Request.Url.AbsoluteUri);
            throw se;
        }
    }

    [WebMethod]
    public string CheckRecurrence(string storeKey, string password, int orderId, DateTime dateFrom, DateTime dateTo)
    {
        try
        {
            Ensure.IsNotNullOrEmpty(storeKey, "Chave da loja não informada");

            DStore store = DataFactory.Store().Locate(storeKey);
            Ensure.IsNotNull(store, "A chave {0} é inválida", storeKey);

            if (Ensure.IsNotNullOrEmpty(store.password) && Ensure.IsNull(password))
                Ensure.IsNotNullOrEmpty(null, "Senha não informada");

            if (Ensure.IsNotNullOrEmpty(store.password) && store.password != GenericHelper.CalculateHash(password))
                Ensure.IsNotNull(null, "A senha para a chave {0} está incorreta", storeKey);

            GenericHelper.LogFile("SuperPagWS::Payment.cs::CheckRecurrence storeId=" + store.storeId + " orderId=" + orderId.ToString() + " dateFrom=" + dateFrom.ToString() + " dateTo=" + dateTo.ToString(), LogFileEntryType.Information);

            Check checkHelper = new Check();
            response resp = checkHelper.ProcessCheckRecurrence(store, orderId, dateFrom, dateTo);

            string s = XmlHelper.GetXml(typeof(response), resp);

            GenericHelper.LogFile("SuperPagWS::Payment.cs::CheckRecurrence storeId=" + store.storeId + " response xml=" + s, LogFileEntryType.Information);

            return s;
        }
        catch (ApplicationException e)
        {
            GenericHelper.LogFile("SuperPagWS::Payment.cs::CheckRecurrence " + e.Message, LogFileEntryType.Warning);

            return e.Message;
        }
        catch (Exception e)
        {
            GenericHelper.LogFile("SuperPagWS::Payment.cs::CheckRecurrence " + e.Message, LogFileEntryType.Error);

            SoapException se = new SoapException("Erro interno: " + e.Message + (e.InnerException != null ? e.InnerException.Message : ""), SoapException.ClientFaultCode, Context.Request.Url.AbsoluteUri);
            throw se;
        }
    }

    //[WebMethod]
    //public string ReprocessTransaction(string storeKey, string password, int orderId, DateTime date)
    //{
    //    try
    //    {
    //        Ensure.IsNotNullOrEmpty(storeKey, "Chave da loja não informada");

    //        DStore store = DataFactory.Store().Locate(storeKey);
    //        Ensure.IsNotNull(store, "A chave {0} é inválida", storeKey);

    //        if (Ensure.IsNotNullOrEmpty(store.password) && Ensure.IsNull(password))
    //            Ensure.IsNotNullOrEmpty(null, "Senha não informada");

    //        if (Ensure.IsNotNullOrEmpty(store.password) && store.password != GenericHelper.CalculateHash(password))
    //            Ensure.IsNotNull(null, "A senha para a chave {0} está incorreta", storeKey);

    //        GenericHelper.LogFile("SuperPagWS::Payment.cs::ReprocessTransaction storeId=" + store.storeId + " orderId=" + orderId.ToString() + " date=" + date.ToString(), LogFileEntryType.Information);

    //        Check checkHelper = new Check();
    //        response resp = checkHelper.ProcessCheckRecurrence(store, orderId, dateFrom, dateTo);

    //        string s = XmlHelper.GetXml(typeof(response), resp);

    //        GenericHelper.LogFile("SuperPagWS::Payment.cs::CheckRecurrence storeId=" + store.storeId + " response xml=" + s, LogFileEntryType.Information);

    //        return s;
    //    }
    //    catch (ApplicationException e)
    //    {
    //        GenericHelper.LogFile("SuperPagWS::Payment.cs::CheckRecurrence " + e.Message, LogFileEntryType.Warning);

    //        return e.Message;
    //    }
    //    catch (Exception e)
    //    {
    //        GenericHelper.LogFile("SuperPagWS::Payment.cs::CheckRecurrence " + e.Message, LogFileEntryType.Error);

    //        SoapException se = new SoapException("Erro interno: " + e.Message + (e.InnerException != null ? e.InnerException.Message : ""), SoapException.ClientFaultCode, Context.Request.Url.AbsoluteUri);
    //        throw se;
    //    }
    //}

    [WebMethod]
    public string CancelOrder(string storeKey, string password, int orderId)
    {
        try
        {
            Ensure.IsNotNullOrEmpty(storeKey, "Chave da loja não informada");

            DStore store = DataFactory.Store().Locate(storeKey);
            Ensure.IsNotNull(store, "A chave {0} é inválida", storeKey);

            if (Ensure.IsNotNullOrEmpty(store.password) && Ensure.IsNull(password))
                Ensure.IsNotNullOrEmpty(null, "Senha não informada");

            if (Ensure.IsNotNullOrEmpty(store.password) && store.password != GenericHelper.CalculateHash(password))
                Ensure.IsNotNull(null, "A senha para a chave {0} está incorreta", storeKey);

            GenericHelper.LogFile("SuperPagWS::Payment.cs::CancelOrder storeId=" + store.storeId + " orderId=" + orderId.ToString(), LogFileEntryType.Information);

            Cancel cancelHelper = new Cancel();
            response resp = cancelHelper.ProcessCancel(store, orderId);

            string s = XmlHelper.GetXml(typeof(response), resp);

            GenericHelper.LogFile("SuperPagWS::Payment.cs::CancelOrder storeId=" + store.storeId + " response xml=" + s, LogFileEntryType.Information);

            return s;
        }
        catch (ApplicationException e)
        {
            GenericHelper.LogFile("SuperPagWS::Payment.cs::CancelOrder " + e.Message, LogFileEntryType.Warning);

            return e.Message;
        }
        catch (Exception e)
        {
            GenericHelper.LogFile("SuperPagWS::Payment.cs::CancelOrder " + e.Message, LogFileEntryType.Error);

            SoapException se = new SoapException("Erro interno: " + e.Message + (e.InnerException != null ? e.InnerException.Message : ""), SoapException.ClientFaultCode, Context.Request.Url.AbsoluteUri);
            throw se;
        }
    }

    [WebMethod]
    public string UpdateOrder(string storeKey, string password, string updatedXML)
    {
        try
        {
            Ensure.IsNotNullOrEmpty(storeKey, "Chave da loja não informada");
            Ensure.IsNotNullOrEmpty(updatedXML, "Xml de update não informado");

            DStore store = DataFactory.Store().Locate(storeKey);
            Ensure.IsNotNull(store, "A chave {0} é inválida", storeKey);

            if (Ensure.IsNotNullOrEmpty(store.password) && Ensure.IsNull(password))
                Ensure.IsNotNullOrEmpty(null, "Senha não informada");

            if (Ensure.IsNotNullOrEmpty(store.password) && store.password != GenericHelper.CalculateHash(password))
                Ensure.IsNotNull(null, "A senha para a chave {0} está incorreta", storeKey);

            GenericHelper.LogFile("SuperPagWS::Payment.cs::UpdateOrder storeId=" + store.storeId + " update xml=" + updatedXML, LogFileEntryType.Information);

            string updatedxsd = "";
            if (ConfigurationManager.AppSettings != null && ConfigurationManager.AppSettings["UpdateXsd"] != null)
                updatedxsd = ConfigurationManager.AppSettings["UpdateXsdFile"];
            else
                updatedxsd = HttpContext.Current.Server.MapPath(".\\Xsd\\Update.xsd");

            Parse xmlparse = new Parse();
            if (!xmlparse.Xml(updatedXML, updatedxsd, null))
                Ensure.IsNotNull(null, xmlparse.Error);

            update up;
            string msgerror = "";
            if ((up = (update)XmlHelper.GetClass(updatedXML, typeof(update), out msgerror)) == null)
                Ensure.IsNotNull(null, msgerror);

            Update updateHelper = new Update();
            response resp = updateHelper.ProcessUpdate(store, up);

            string s = XmlHelper.GetXml(typeof(response), resp);

            GenericHelper.LogFile("SuperPagWS::Payment.cs::UpdateOrder storeId=" + store.storeId + " response xml=" + s, LogFileEntryType.Information);

            return s;
        }
        catch (ApplicationException e)
        {
            GenericHelper.LogFile("SuperPagWS::Payment.cs::UpdateOrder " + e.Message, LogFileEntryType.Warning);

            return e.Message;
        }
        catch (Exception e)
        {
            GenericHelper.LogFile("SuperPagWS::Payment.cs::UpdateOrder " + e.Message, LogFileEntryType.Error);

            SoapException se = new SoapException("Erro interno: " + e.Message + (e.InnerException != null ? e.InnerException.Message : ""), SoapException.ClientFaultCode, Context.Request.Url.AbsoluteUri);
            throw se;
        }
    }
}
