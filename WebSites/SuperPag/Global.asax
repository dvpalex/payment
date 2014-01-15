<%@ Application Language="C#" %>

<script runat="server">
    void Application_Start(object sender, EventArgs e) 
    {

    }
    
    void Application_PreRequestHandlerExecute(object sender, EventArgs e)
    {
        if (HttpContext.Current.Session != null && HttpContext.Current.Session["Language"] != null)
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(HttpContext.Current.Session["Language"].ToString());
    }
    
    void Application_End(object sender, EventArgs e) 
    {

    }
        
    void Application_Error(object sender, EventArgs e) 
    {
        Server.Transfer("~/error.aspx");
    }

    void Begin_Request(object sender, EventArgs e)
    {    
        //System.Web.HttpContext.Current.Request.ContentEncoding = Encoding.UTF8;
    }

    void End_Request(object sender, EventArgs e)
    {
    }
    
    void Session_Start(object sender, EventArgs e) 
    {

    }

    void Session_End(object sender, EventArgs e) 
    {

    }
</script>
