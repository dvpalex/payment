using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace SuperPag
{
    [Serializable]
    public abstract class ControlInfo
    {
        private int storeId;
        private string path;

        public string Path
        {
            get { return path; }
            set { path = value; }
        }
        public int StoreId
        {
            get { return storeId; }
            set { storeId = value; }
        }
    }
}