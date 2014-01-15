using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using SuperPag;
using SuperPag.Helper;
using SuperPag.Data;
using SuperPag.Data.Messages;

[WebService(Namespace = "http://www.superpag.com.br/Services")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
public class CheckStatus : System.Web.Services.WebService
{
    public CheckStatus()
    {
    }

    [WebMethod]
    public int CheckAttemptStatus(string storeKey, string storePassword, string COD_CONTROLE)
    {
        Ensure.IsNotNullOrEmpty(storeKey, "Invalid StoreKey");
        Ensure.IsNotNullOrEmpty(storePassword, "Invalid storePassword");
        Ensure.IsNotNullOrEmpty(COD_CONTROLE, "Invalid COD_CONTROLE");
        
        DStore dStore = DataFactory.Store().Locate(storeKey);
        Ensure.IsNotNull(dStore, "Invalid storeKey");

        if (Ensure.IsNotNullOrEmpty(dStore.password) && dStore.password != GenericHelper.CalculateHash(storePassword))
            Ensure.IsNotNull(null, "Invalid storePassword");

        DPaymentAttempt attempt = DataFactory.PaymentAttempt().Locate(new Guid(COD_CONTROLE));
        Ensure.IsNotNull(dStore, "Invalid COD_CONTROLE");

        return attempt.status;
    }

    [WebMethod]
    public int CheckOrderStatus(string storeKey, string storePassword, string storeOrderNumber)
    {
        Ensure.IsNotNullOrEmpty(storeKey, "Invalid StoreKey");
        Ensure.IsNotNullOrEmpty(storePassword, "Invalid storePassword");
        Ensure.IsNotNullOrEmpty(storeOrderNumber, "Invalid storeOrderNumber");

        DStore dStore = DataFactory.Store().Locate(storeKey);
        Ensure.IsNotNull(dStore, "Invalid storeKey");

        if (Ensure.IsNotNullOrEmpty(dStore.password) && dStore.password != storePassword)
            Ensure.IsNotNull(null, "Invalid storePassword");

        DOrder[] orders = DataFactory.Order().List(dStore.storeId, storeOrderNumber);
        Ensure.IsNotNull(orders, "Invalid storeOrderNumber");
        Ensure.IsNotNull(orders[0], "Invalid storeOrderNumber");

        DPaymentAttempt[] attempts = DataFactory.PaymentAttempt().ListSortedByDate(orders[0].orderId);
        Ensure.IsNotNull(attempts, "No payment attempts to this storeOrderNumber");
        Ensure.IsNotNull(attempts[0], "No payment attempts to this storeOrderNumber");
        return attempts[0].status;
    }
}
