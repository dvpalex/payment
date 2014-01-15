<%@ Application Language="C#" %>

<script runat="server">

    void Application_Start(object sender, EventArgs e) 
    {

    }
    
    void Application_End(object sender, EventArgs e) 
    {

    }
        
    void Application_Error(object sender, EventArgs e) 
    {
        Exception ex = Server.GetLastError();
        if (HttpContext.Current != null)
        {
            HttpContext.Current.Items.Add("_error", ex);
        }
        Server.Transfer("~/error.aspx");
    }

    void Session_Start(object sender, EventArgs e) 
    {

    }

    void Session_End(object sender, EventArgs e) 
    {

    }

    protected void Application_AcquireRequestState(Object sender, EventArgs e)
    {
        if (HttpContext.Current.Request.Path.ToLower().EndsWith(".aspx"))
        {
            if (HttpContext.Current.User == null && !HttpContext.Current.Request.Path.ToLower().EndsWith("/login.aspx"))
            {
                Response.Redirect("~/login.aspx");
            }
        }
    }

    protected void Application_AuthenticateRequest(Object sender, EventArgs e)
    {
        if ((HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath != "~/login.aspx") &&
        (HttpContext.Current.User == null))
        {
            Response.Redirect("~/login.aspx");
        }
    }

       
</script>
