using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Web.Security; //Seguraça
using SuperPag.Business;
using SuperPag.Business.Messages;
using SuperPag.Data.Messages;
using SuperPag.Data;
using SuperPag.Framework;
using SuperPag.Business.MembershipAdm;


public partial class Usuario_EditUser : System.Web.UI.Page
{
    #region PROPRIEDADES DA PÁGINA
    public String _PageIdentification
    {
        get
        {
            return "ManutencaoEditarUsuario"; // PageId da tabela Pages do BD
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

    //Retorna a Store do usuário logado
    public int UserLogadoStore
    {
        get
        {
            return Store.Locate(UserLogadoID);
        }
    }

    
    //Retorna a o ID do usuário Editado
    public Guid UserEditID
    {
        get
        {
            return SuperPag.Business.Users.RetunUserId(Convert.ToString(Session["NomeUsu"]));
        }
    }

    //Retorna a Store do usuário Editado
    public int UserEditStore
    {
        get
        {
            return Store.Locate(UserEditID);
        }
    }

    #endregion

    #region DELEGATE END EVENTO

    //Sobrescreve o método "OnInit"
    override protected void OnInit(EventArgs e)
    {
        this.ListaUser1.clicou += new ListaUser.DelegateListUser(ListaUser_clicou);
    }

    private void ListaUser_clicou(string value)
    {
        //this.MyddlStore.SelectedValue = value;
        //this.BindGrupos();
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
    #endregion

    #region FUNÇÕES DA PÁGINA

    //Este método habilita Recursos conforme o Perfil(Grupo) do Usuário
    private void HabilitaRecursosdaPagina()
    {
        if (Roles.IsUserInRole("ADMINISTRADORTIVIT").Equals(false))
           chGrupos.Items.Remove("ADMINISTRADORTIVIT");
    }

    private void LimpaTela()
    { 
        this.txtDescricao.Text = String.Empty;
        this.txtEmail.Text = String.Empty;
        this.txtIdentificacao.Text = String.Empty;
        this.chStatus.Checked = false;
        this.chGrupos.ClearSelection();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.BindUsuario();
            this.BindGrupos();
            this.PermissaoAcesso(); //Verifica se o usuário tem permissão para acessar a página
        }
    }

    private void BindUsuario()
    {
        //Cria um objeto Usuario
        String UserName = Convert.ToString(Session["NomeUsu"]);
        MembershipUser user = Membership.GetUser(UserName);

        string Key = Convert.ToString(user.ProviderUserKey);
        this.chStatus.Checked = user.IsApproved;
        this.txtEmail.Text = user.Email;
        this.txtIdentificacao.Text = user.UserName;
        this.txtDescricao.Text = user.Comment;
    }

    private void BindGrupos()
    {   
        RolesAdm ListRoles = new RolesAdm();
        //object oGrupos = ListRoles.GetAllRoles(UserLogadoStore);
        object oGrupos = ListRoles.GetAllRoles(UserEditStore);

        this.chGrupos.DataTextField = "RoleName";
        this.chGrupos.DataValueField = "RoleId";
        this.chGrupos.DataSource = oGrupos;
        this.chGrupos.DataBind();


        //Checa os itens/grupos a quem o usuário pertence
        string[] oGruposUsu = Roles.GetRolesForUser(Session["NomeUsu"].ToString());

        foreach (string item in oGruposUsu)
            this.chGrupos.Items.FindByText(item).Selected = true;
    }

    private void Save()
    {
        String UserName = this.txtIdentificacao.Text;
        MembershipUser user = Membership.GetUser(UserName);

        //Atualização do usuário no banco
        user.Comment = this.txtDescricao.Text.Trim();
        user.Email = this.txtEmail.Text.Trim();
        user.IsApproved = this.chStatus.Checked;
        Membership.UpdateUser(user);

        //Atualização do(s) Grupo(s) do usuário no banco
        string strUser = user.UserName;

        //Deleta os Grupos do usuário para cadastrar os novos
        Role.UserInRolesDELETE(new Guid(user.ProviderUserKey.ToString()));

        for (int i = 0; i < chGrupos.Items.Count; i++)
        {
            if (chGrupos.Items[i].Selected == true)
            {
                RolesAdm oRolesAdm = new RolesAdm();
                oRolesAdm.AddUsersToRoles(this.UserEditID, new Guid(this.chGrupos.Items[i].Value));
            }
        }

        this.LimpaTela();
    }
    #endregion

    #region EVENTOS DOS BOTÕES DA PÁGINA

    protected void btnSalvar_Click(object sender, EventArgs e)
    {
        //VALIDA ANTES DE SALVAR SE ALGUM GRUPO FOI SELECIONADO
        bool Nulo;
        Nulo = ValidaGrupoNulo();
        if (Nulo == true)
        {
            this.lblResultado.Visible = false;
            this.Save();
        }
        else
        {
            this.lblResultado.Text = "Escolha um Grupo para o usuário.";
            this.lblResultado.Visible = true;
            return;
        }
    }

    protected void btnCancelar_Click(object sender, EventArgs e)
    {
        Server.Transfer("~/Usuario/Default.aspx");
    }
    #endregion

    #region VALIDAÇÕES
    
    //VALIDA SE PELO MENOS UM GRUPO FOI SELECIONADO
    //Se retornar "false" então não há grupos selecionados
    private bool ValidaGrupoNulo() 
    {
        bool Grupo = false;

        for (int i = 0; i < chGrupos.Items.Count; i++)
        {
            if (chGrupos.Items[i].Selected == true)
            {
                Grupo = true;
                return Grupo;
            }
            else
            {
                Grupo = false;
            }
        }
        return Grupo;
    }
    #endregion
}
