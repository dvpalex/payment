using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SuperPag;
using SuperPag.Data;
using SuperPag.Data.Messages;

namespace SuperPag
{
    public abstract class PaymentChooserBase : ControlBase
    {
        int _storeId;
        DStorePaymentForm[] _storePaymentForm;
        string _pageName;

        public int storeId
        {
            get { return _storeId; }
            set { _storeId = value; }
        }

        public DStorePaymentForm[] StorePaymentForms
        {
            get { return _storePaymentForm; }
            set { _storePaymentForm = value; }
        }
       
        public string PageName
        {
            get { return _pageName; }
            set { _pageName = value; }
        }
    }
}