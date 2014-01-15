using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace Controller.Lib.Util
{
    public static class ControllerContext
    {
        public static int StoreId
        {
            get { return int.Parse(HttpContext.Current.Session["storeId"].ToString()); }
            set { HttpContext.Current.Session["storeId"] = value; }
        }
        
        public static string UserName
        {
            get { return HttpContext.Current.Session["userName"].ToString(); }
            set { HttpContext.Current.Session["userName"] = value; }
        }
    }
}
