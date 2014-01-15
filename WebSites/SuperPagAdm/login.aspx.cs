using System;
using System.Data;
using System.Configuration; 
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

public partial class _DefaultLogin : System.Web.UI.Page 
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void LoginController_Authenticate(object sender, AuthenticateEventArgs e)
    {
        //if (LoginController.UserName.Equals("homolog") && LoginController.Password.Equals("123456"))
        //{
            e.Authenticated = Membership.ValidateUser(LoginController.UserName, LoginController.Password);
        //}
        //else
        //{
        //    //Retorna o endereço IP do Server cliente 
        //    String ServerClient = Request.UserHostAddress;

        //    if (ServerClient.Equals("201.59.132.214"))
        //        //Faz a autenticação do usuário
        //        e.Authenticated = Membership.ValidateUser(LoginController.UserName, LoginController.Password);
        //    else
        //        //Ñem faz a autenticação, nega o acesso direto
        //        e.Authenticated = false;
        //}
    }
}