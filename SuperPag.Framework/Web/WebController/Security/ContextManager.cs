//using System;
//using System.Web;

//namespace SuperPag.Framework.Web.WebController
//{
//    //TODO: Comentário
//    public class FWCContextManager
//    {
//        public static void Persist(Context context)
//        {
//            System.Web.HttpContext.Current.Session["__CONTEXT"] = context;
//        }

//        public static Context GetCurrent()
//        {
//            if( System.Web.HttpContext.Current != null &&
//                System.Web.HttpContext.Current.Session != null && 
//                System.Web.HttpContext.Current.Session["__CONTEXT"] is Context)
//            {
//                return (Context)System.Web.HttpContext.Current.Session["__CONTEXT"];
//            } 
//            else
//            {
//                return null;
//            }
//        }

//    }
//}
