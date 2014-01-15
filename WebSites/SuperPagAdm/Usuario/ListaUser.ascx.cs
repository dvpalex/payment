using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Web.Security;//MemberShip e Roles
using System.Web.Profile;
using SuperPag.Business.MembershipAdm;
using SuperPag.Business;
using SuperPag.Business.Messages;


public partial class ListaUser : System.Web.UI.UserControl
{
    public delegate void DelegateListUser(string value); //crio o delegate
    public event DelegateListUser clicou; //e o evento tipado para esse delegate 

    #region PROPRIEDADES DA PÁGINA

    public string ValorSelecionado
    {
        get { return this.ddlStore.SelectedValue; }
        set { this.ddlStore.SelectedValue = value; }
    }

    //Retorna a o ID do usuário logado
    public Guid UserLogadoID
    {
        get  {  return (Guid)Membership.GetUser(true).ProviderUserKey;   }
    }

    //Retorna a Store do usuário logado
    public int UserLogadoStore
    {
        get  {  return Store.Locate(UserLogadoID); }
    }

    //Retorna a Store Selecionada no Combo "ddlStore"
    public int SelectedStore
    {
        get { return Convert.ToInt32(Session["SelectedStore"]); }
    }
    
    public int CurrentPage
    {
        get
        {
            object o = this.ViewState["_CurrentPageUsuarios"];
            if (o == null || (int)o < 0)
                return 0;
            else
                return (int)o;
        }

        set  { this.ViewState["_CurrentPageUsuarios"] = value;  }
    }

    string _Letra;
    string Letra
    {
        set { _Letra = value; }
        get { return _Letra;  }
    }

    #endregion

    #region FUNÇÕES DA PÁGINA

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.BindFiltroLetras();

            //Habilita a <TD> onde está o combo LOJA
            if (Roles.IsUserInRole("ADMINISTRADORTIVIT").Equals(true))
            {
                this.BindStore();
                this.TDStore.Visible = true;
                this.TDStore1.Visible = true;
            }
        }
    }

    private void BindStore()
    {
        this.ddlStore.DataSource = Store.GetStoreAll();
        this.ddlStore.DataBind();
    }

    private void BindFiltroLetras()
    {
        DataSet dsXML = new DataSet();
        dsXML.ReadXml(Request.MapPath("XML/Letras.xml"));

        this.DataListLetras.DataSource = dsXML;
        this.DataListLetras.DataBind();
    }

    protected void SelectUsu(object sender, System.EventArgs e)
    {
        Letra = ((Button)sender).CommandArgument;
        this.ItemsGet(Letra);
    }

    private void ItemsGet(String Letra)
    {       
        PagedDataSource PGDS = new PagedDataSource();   //Instaciamos a Classe PagedDataSource que é a responsavel pela Paginação
        PGDS.AllowPaging = true;                        //Dizemos que ela esta habilitada para ser paginada
        PGDS.PageSize = 10;                              //Estabelece o limite de linhas por página
        PGDS.CurrentPageIndex = CurrentPage;
        ViewState["Letra"] = Letra;
        //Estabelece o DataSource do objeto Paginação        
        int StoreId;

        if (SelectedStore >0)
            StoreId = SelectedStore;
        else
            StoreId = this.UserLogadoStore;


        if (Letra == "TODOS")
        {
            //Busca TODOS os usuários da mesma loja do uisuário logado
            MemberShipProviderAdm ListUser = new MemberShipProviderAdm();
            PGDS.DataSource = ListUser.GetAllUsers(StoreId);
        }
        else
        {
            MemberShipProviderAdm ListUser = new MemberShipProviderAdm();
            PGDS.DataSource = ListUser.FindUsersByName(Letra, StoreId);
        }
       
        //Bind de usuários na página
        this.rptUsuarios.DataSource = PGDS;
        this.rptUsuarios.DataBind();
        this.PanelPagnacao.Visible = true;

        //Informa a página corrente
        this.lblCurrentPage.Text = "Pagina: " + (CurrentPage + 1).ToString() + " de <b>" + PGDS.PageCount.ToString() + "</b>";

        //Habilita controles de paginação conforme lista de usuários retornada
        if (PGDS.PageCount == 1 && CurrentPage + 1 == 1)
            this.lblCurrentPage.Visible = false;
        else
            this.lblCurrentPage.Visible = true;

        this.btnPrev.Enabled = !PGDS.IsFirstPage;
        this.btnNext.Enabled = !PGDS.IsLastPage;
    }

    public void btnPrev_Click(object sender, EventArgs e)
    {
        CurrentPage--;  //Volta 1 página
        Letra = ViewState["Letra"].ToString();
        ItemsGet(Letra);
    }

    public void btnNext_Click(object sender, EventArgs e)
    { 
        CurrentPage++;  //Vai para a próxima página
        Letra = ViewState["Letra"].ToString();
        ItemsGet(Letra);
    }

    protected void ddlStore_SelectedIndexChanged(object sender, EventArgs e)
    {
        Session["SelectedStore"] = this.ddlStore.SelectedValue;
        this.clicou(this.ddlStore.SelectedValue);
        this.LimpaPesquisa(); //Limpa o resultado da pesquisa anterior para executar uma nova
    }

    private void LimpaPesquisa()
    {
        this.CurrentPage = 0;
        this.lblCurrentPage.Text = String.Empty;
        this.rptUsuarios.DataSource = null;
        this.rptUsuarios.DataBind();
        this.PanelPagnacao.Visible = false;
    }
    #endregion

    #region EVENTOS DE CONTROLES

    protected void rptUsuarios_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName == "editar")
        {
            Session["NomeUsu"] = ((Label)e.Item.FindControl("lblNome")).Text;
            Response.Redirect("~/Usuario/EditUser.aspx");
        }

        if (e.CommandName == "excluir")
        {
            //Exclui logicamente um usuário
            String UserName = ((Label)e.Item.FindControl("lblNome")).Text;
            MembershipUser user = Membership.GetUser(UserName);

            //Atualização do usuário no banco
            user.IsApproved = false;
            Membership.UpdateUser(user);
        }
    }

    protected void rptUsuarios_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
           if (ddlFiltro.SelectedValue == "-1" || Convert.ToBoolean(ddlFiltro.SelectedValue) == ((MembershipUser)e.Item.DataItem).IsApproved)
            {
                ((CheckBox)e.Item.FindControl("ChStatus")).Checked = ((MembershipUser)e.Item.DataItem).IsApproved;
                ((CheckBox)e.Item.FindControl("ChStatus")).Visible = true;
                ((Label)e.Item.FindControl("lblNome")).Text = ((MembershipUser)e.Item.DataItem).UserName;
                ((Label)e.Item.FindControl("lblNome")).Visible = true;
                ((LinkButton)e.Item.FindControl("lbEditar")).Visible = true;
            }
    }
    protected void ChStatus_CheckedChanged(object sender, EventArgs e)
    {
        rptUsuarios.ItemCommand += new RepeaterCommandEventHandler(rptUsuarios_ItemCommand);
    }

    #endregion


}
