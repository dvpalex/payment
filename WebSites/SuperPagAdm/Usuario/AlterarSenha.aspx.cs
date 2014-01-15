using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SuperPag.Business;
using SuperPag.Business.Messages;
using SuperPag.Data.Messages;
using SuperPag.Data;
using SuperPag.Framework;

public partial class Usuario_AlterarSenha : System.Web.UI.Page
{
    #region PROPRIEDADES DA PÁGINA
    public String _PageIdentification
    {
        get
        {
            return "ManutencaoAlterarSenha";
        }
    }

    //Retorna a o ID do usuário logado
    public Guid UserLogadoID
    {
        get
        {
            return (Guid)Membership.GetUser(true).ProviderUserKey;
        }
    }

    //Retorna o Store do usuário logado
    public int UserLogadoStore
    {
        get
        {
            return Store.Locate(UserLogadoID);
        }
    }
    #endregion
    
    
    protected void Page_Load(object sender, EventArgs e)
    {
        //Verifica se o usuário tem permissão para acessar a página
        this.PermissaoAcesso();
    }

    
    #region PERMISSÃO DE ACESSO

    private void PermissaoAcesso()
    {
        //Retorna se o usuário tem permissão para acessar esta página
        //Se tiver, permite que o usuário acesse a página e 
        //habilita os recursos da mesma conforme o Perfil(Grupo) do Usuário
        bool Page = Pages.ReturnPagesInRole(_PageIdentification, this.UserLogadoStore);

        if (Page.Equals(false))
            Response.Redirect("~/login.aspx");
    }
    #endregion

    protected void btnFinalizar_Click(object sender, EventArgs e)
    {
        Response.Redirect("../Usuario/Default.aspx");
    }
}
