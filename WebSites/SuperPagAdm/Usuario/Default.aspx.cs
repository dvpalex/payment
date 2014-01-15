using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SuperPag.Business;
using SuperPag.Business.Messages;
using SuperPag.Business.MembershipAdm;
using SuperPag.Data.Messages;
using SuperPag.Data;
using SuperPag.Framework;


public partial class Usuario_Default : System.Web.UI.Page
{
    #region PROPRIEDADES DA PÁGINA
    public String _PageIdentification
    {
        get
        {
            return "ManutencaoCadastroUsuario";
        }
    }

    //Retorna o ID do usuário logado
    public Guid UserLogadoID
    {
        get
        {
            return (Guid)Membership.GetUser(true).ProviderUserKey;
        }
    }

    //Retorna a Store do usuário logado
    public int UserLogadoStore
    {
        get
        {
            return Store.Locate(UserLogadoID);
        }
    }

    #endregion

    #region PROPRIEDADES DO CONTROLE "CreateUserWizard"

    public DropDownList MyddlStore
    {
        get
        {
            return ((DropDownList)CreateUserWizard.CreateUserStep.ContentTemplateContainer.FindControl("ddlStore"));
        }
    }

    public CustomValidator MyCustomRoles
    {
        get
        {
            return ((CustomValidator)CreateUserWizard.CreateUserStep.ContentTemplateContainer.FindControl("CustomRoles"));
        }
    }
    
    public TextBox MyUserName
    {
        get
        {
            return ((TextBox)CreateUserWizard.CreateUserStep.ContentTemplateContainer.FindControl("UserName"));
        }
    }

    public TextBox MyPassword
    {
        get
        {
            return ((TextBox)CreateUserWizard.CreateUserStep.ContentTemplateContainer.FindControl("Password"));
        }
    }

    public TextBox MyConfirmPassword
    {
        get
        {
            return ((TextBox)CreateUserWizard.CreateUserStep.ContentTemplateContainer.FindControl("ConfirmPassword"));
        }
    }
    #endregion

    #region DELEGATE END EVENTO

    //Sobrescreve o método "OnInit"
    override protected void OnInit(EventArgs e)
    {
        this.ListaUser.clicou += new ListaUser.DelegateListUser(ListaUser_clicou);
    }

    private void ListaUser_clicou(string value)
    {
        this.MyddlStore.SelectedValue = value;
        this.BindGrupos();
    }
    #endregion

    #region FUNÇÕES DA PAGINA

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.BindGrupos();
            this.PermissaoAcesso(); //Verifica se o usuário tem permissão para acessar a página 
        }        
    }

    //Recupera a chave do Usuário recentemente inserido
    private Guid ReturnUserId(string UserName)
    {
        Guid UserId = SuperPag.Business.Users.RetunUserId(UserName);
        return UserId;
    }

    protected void ddlStore_SelectedIndexChanged(object sender, EventArgs e)
    {
       this.BindGrupos();
       Session["SelectedStore"] = this.MyddlStore.SelectedValue;
       ListaUser.ValorSelecionado = this.MyddlStore.SelectedValue;       
    }
    #endregion

    #region PERMISSÃO DE ACESSO

    private void PermissaoAcesso()
    {
        //Retorna se o usuário tem permissão para acessar esta página
        //Se tiver, permite que o usuário acesse a página e 
        //habilita os recursos da mesma conforme o Perfil(Grupo) do Usuário
        bool Page = Pages.ReturnPagesInRole(_PageIdentification, this.UserLogadoStore);

        if (Page.Equals(false))
            Server.Transfer("~/login.aspx");
        else
            this.HabilitaRecursosdaPagina();
    }

    //Este método habilita Recursos conforme o Perfil(Grupo) do Usuário
    private void HabilitaRecursosdaPagina()
    {  
        bool user = Roles.IsUserInRole("ADMINISTRADORTIVIT");
    
        if (user.Equals(true))
        {   this.BindStore();
            //Habilita a <TD> onde está o combo LOJA
            ((HtmlTableRow)CreateUserWizard.CreateUserStep.ContentTemplateContainer.FindControl("TRStore")).Visible = true;            
        }
        else
            chGrupos.Items.Remove("ADMINISTRADORTIVIT");
    }
    #endregion

    #region BIND CONTROLES
    private void BindStore()
    {
        this.MyddlStore.DataSource = Store.GetStoreAll();
        this.MyddlStore.DataBind();
    }

    private void BindGrupos()
    {       
        //Objetos Uteis para o procedimento
        RolesAdm ListRoles = new RolesAdm();
        object oGrupos;

        //Define a Base de dados do controle
        if (this.MyddlStore.SelectedIndex > 0)
        {
            int SelectedStore = Convert.ToInt32(this.MyddlStore.SelectedValue);
            oGrupos = ListRoles.GetAllRoles(SelectedStore);
        }
        else
            oGrupos = ListRoles.GetAllRoles(UserLogadoStore);

        //Bind no caontrole
        try
        {
            this.chGrupos.DataTextField = "RoleName";
            this.chGrupos.DataValueField = "RoleId";
            this.chGrupos.DataSource = oGrupos;
            this.chGrupos.DataBind();

            if (chGrupos.Items.Count <= 0)
                throw new Exception("Nenhum Grupo de usuário foi cadastrado nesta loja!");
        }
        catch
        {
            this.CustomRoles1.IsValid = false;
            this.CustomRoles1.ErrorMessage = "Nenhum Grupo de usuário foi cadastrado nesta loja!";
        }
    }
    
    #endregion

    #region FUNÇÕES PARA O CONTROLE "CreateUserWizard"

    private void RequereValidator(string Label, bool Visible)
    {
        //Ficam visíveis ou invisiveis os (*)
        ((Label)CreateUserWizard.CreateUserStep.ContentTemplateContainer.FindControl(Label)).Visible = Visible;

        //É disparada a mensagem para o usuário
        this.MyCustomRoles.ErrorMessage = "Campos com (*) devem ser preenchidos.";
        this.MyCustomRoles.IsValid = false;
        return;
    }

    protected void CreateUserWizard_CreatingUser(object sender, LoginCancelEventArgs e)
    {
        #region VALIDA GRUPO(S) SELECIONADO(S)

        //Verifica antes de salvar se algum item do CheckBoxList foi selecionado

        bool Selected = false;
        for (int i = 0; i < chGrupos.Items.Count; i++)
            if (chGrupos.Items[i].Selected.Equals(true))
                Selected = true;

        if (!Selected)
        {
            this.MyCustomRoles.ErrorMessage = "Selecione um Grupo para o usuário.";
            this.MyCustomRoles.IsValid = false;
            e.Cancel = true;
            return;
        }
        #endregion

        #region VALIDA CAMPOS OBRIGATÓRIOS

        //Verifica antes de salvar se os dados do usuário foram todos preenchidos

        if (this.MyUserName.Text.Equals(String.Empty))
        {
            this.RequereValidator("lblValidaNome", true);
            e.Cancel = true;
        }
        else
            this.RequereValidator("lblValidaNome", false);

        if (this.MyPassword.Text.Equals(String.Empty))
        {
            this.RequereValidator("lblValidaSenha", true);
            e.Cancel = true;
        }
        else
            this.RequereValidator("lblValidaSenha", false);

        if (this.MyConfirmPassword.Text.Equals(String.Empty))
        {
            this.RequereValidator("lblValidaConfirmSenha", true);
            e.Cancel = true;
        }
        else
            this.RequereValidator("lblValidaConfirmSenha", false);

        if (this.MyddlStore.SelectedIndex <= 0 &&
            (Roles.IsUserInRole("ADMINISTRADORTIVIT").Equals(true)))
        {
            this.RequereValidator("lblValidaLoja", true);
            e.Cancel = true;
        }
        else
            this.RequereValidator("lblValidaLoja", false);
        #endregion
    }
    
    
    protected void CreateUserWizard_CreatedUser(object sender, EventArgs e)
    {
        #region CADASTRA GRUPOS PARA ESTE USUÁRIO
        for (int i = 0; i < chGrupos.Items.Count; i++)
        {
            if (chGrupos.Items[i].Selected.Equals(true))
            {
                //Recupera o ID do Usuário
                Guid UserId = this.ReturnUserId(this.MyUserName.Text);
                RolesAdm oRolesAdm = new RolesAdm();
                oRolesAdm.AddUsersToRoles(UserId, new Guid(this.chGrupos.Items[i].Value));
            }
        }
        chGrupos.Enabled = false;
        #endregion

        #region CADASTRA UMA LOJA PARA O USUÁRIO
        //Se o usuário logado, for o "Administrador TIVIT" então o sistema vai cadastrar 
        //o usuário na loja selecionado no combo.
        //Se o Usuário logado for o "Administrador da loja" então o sistema vai cadastrar 
        //o usuário na mesma loja do usuário logado
        
        int StoreRole;

        if (this.MyddlStore.SelectedIndex != 0)
            StoreRole = Convert.ToInt32(this.MyddlStore.SelectedValue);
        else
            StoreRole = this.UserLogadoStore; //retorna o ID da Store do usuário logado
        

        //Recupera o Usuário Recentemente Cadastrado
        MembershipUser user = Membership.GetUser(this.MyUserName.Text);

        //Vincula o Usuário Recem Cadastrado a uma loja
        Store.Save(new Guid(user.ProviderUserKey.ToString()), StoreRole);
        #endregion
    }

    protected void btnConcluir_Click(object sender, EventArgs e)
    {
        Server.Transfer("../Usuario/Default.aspx");
    }
    #endregion   
}
