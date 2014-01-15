using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using System.Web.Security; //Seguraça
using SuperPag.Business;
using SuperPag.Business.Messages;
using SuperPag.Business.MembershipAdm;

public partial class Usuario_Roles : System.Web.UI.Page
{

    #region PROPRIEDADES DA PÁGINA
    public String _PageIdentification
    {
        get
        {
            return "ManutencaoCadastroGrupos";
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

    //Retorna a Store Selecionada no Combo "ddlStore"
    public int SelectedStore
    {
       get { return Convert.ToInt32(ViewState["SelectedStore"]); }
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
            Response.Redirect("~/login.aspx");
    }
    #endregion

    #region FUNÇÕES DA PAGINA

        protected void Page_Load(object sender, EventArgs e)
        {
            //Verifica se o usuário tem permissão para acessar a página
            this.PermissaoAcesso();

            if (!IsPostBack)
            {
                this.BindGrupos();
                this.BindTree();

                if (Roles.IsUserInRole("ADMINISTRADORTIVIT").Equals(true))
                {
                    this.BindStore();
                    this.TRStore.Visible = true; //Habilita a <TR> onde está o combo LOJA
                }
            }
        }

        //Limpa o Formulário
        private void LimpaTela()
        {
            this.txtPapeis.Text = string.Empty;
            this.txtPapeis.Enabled = true;
            this.ddlStore.Enabled = true;
            this.btnIncluir.Text = "Inserir";
            ViewState["ItemID"] = null;
            ViewState["ItemNOME"] = null;

            //Varre TreeView e Limpa os Nós do mesmo     
            TreeNodeCollection treeview = this.TreeGrupos.Nodes;
            int i = 0;
            foreach (TreeNode tre in treeview)//Nones Pais
            {
                TreeNodeCollection treChild = treeview[i].ChildNodes;//Nones Filhos deste Pai
                for (int j = 0; j < treChild.Count; j++)
                    treChild[j].Checked = false;
                i++;
            }
    }

    private void BindGrupos()
    {
        try
        {
            RolesAdm ListRoles = new RolesAdm();
            object oAllGrupos;

            //Define o a base de dados para o DataSource do Controle "rptGrupos"
            if (SelectedStore > 0)    
                oAllGrupos = ListRoles.GetAllRoles(this.SelectedStore); 
            else
                oAllGrupos = ListRoles.GetAllRoles(this.UserLogadoStore);

            //Bind no Controle "rptGrupos"
            this.rptGrupos.DataSource = oAllGrupos;
            this.rptGrupos.DataBind();

            if (rptGrupos.Items.Count <= 0)
                throw new Exception("Nenhum Grupo de usuário foi cadastrado nesta loja!");
            }
            catch
            {
                this.CustomRole.IsValid = false;
                this.CustomRole.ErrorMessage = "Nenhum Grupo de usuário foi cadastrado nesta loja!";
            }
    }

    private void BindStore()
    {
        this.ddlStore.DataSource = Store.GetStoreAll();
        this.ddlStore.DataBind();
    }
    #endregion

    #region FUNÇÕES DO ITEM
     
    private void ItemInsert(string roleName, string LoweredRoleName, int StoreID)
    {
        #region CADASTRA O NOVO GRUPO
          RolesAdm objRoleAdm = new RolesAdm();
          objRoleAdm.CreateRole(roleName, LoweredRoleName, StoreID);
        #endregion

        #region CADASTRA AS PERMISSÕES DO GRUPO
          this.InserePermissions(roleName, StoreID);
        #endregion

        #region REFRESH DA PÁGINA
          this.BindGrupos();   //Atualiza listas de Grupos
          this.LimpaTela();    //Limpa Formulário
        #endregion
      }

        //Salva alterações do GRUPO e de suas permissões
    private void ItemSave(Guid Key, string roleName, int StoreID)
        {
            try
            {
                //Exclui as permissões atuais do Grupo
                PermissionPages.PermissionInRolesDELETE(Key);
            }
            catch
            {
                this.CustomRole.IsValid = false;
                this.CustomRole.ErrorMessage = "Um erro correu ao tentar atualizar as permissões do Grupo de usuário.";
                return;
            }

            //Insere as novas permissões para o Grupo
            IList<string[]> oListRoles = new List<string[]>();
            oListRoles = this.VarreTree(roleName, StoreID);
            PermissionPages.PermissionInRolesINSERT(oListRoles);
            this.LimpaTela(); //Limpa Formulário
        }

        //Exclui o GRUPO e suas permissões
        private void ItemDelete(string roleName, int StoreID)
        {
            try
            {
                Guid RoleId = Role.RetunRoleId(roleName, StoreID);
                RolesAdm oRolesAdm = new RolesAdm();
                oRolesAdm.DeleteRole(RoleId, StoreID);    //Deleta Grupo
                this.BindGrupos();                        //Atualiza lista de Grupos
                ViewState["ItemID"] = null;               //Limpa ViewState ID
                ViewState["ItemNOME"] = null;             //Limpa ViewState NOME
            }
            catch (System.Data.SqlClient.SqlException)
            {
                this.CustomRole.IsValid = false;
                this.CustomRole.ErrorMessage = "Este Grupo não pode ser excluído, pois há usuários dependentes a ele.";
                return;
            }
        }

        //Edita um GRUPO selecionado no Repiater
        private void ItemEdit(string roleName, int StoreID)
        { 
            this.LimpaTela();
            Guid RoleId = this.ReturnRoleId(roleName, StoreID);
            ViewState["ItemID"] = RoleId;            //Armazena chave do Grupo inserido
            ViewState["ItemNOME"] = roleName;        //Armazena o nome do Grupo inserido

            //Bind Permissões Grupo
            IList<MPermissionInRoles> ObjListItem = PermissionPages.PermissionInRolesSELECT(RoleId);
            TreeNodeCollection treeview = this.TreeGrupos.Nodes;    //Armazena os Nones Pais do TreeView
            for (int i = 0; i < ObjListItem.Count; i++)
            {
                int K = 0;
                foreach (TreeNode tre in treeview)    //Varre um None Pai de cada vez
                {
                    TreeNodeCollection treChild = treeview[K].ChildNodes;//Armazena os Nones Filhos deste Pai
                    for (int j = 0; j < treChild.Count; j++)
                    {
                        if (Convert.ToInt16(treChild[j].Value) == ObjListItem[i].PermissionPagesId)
                            treChild[j].Checked = true;   //Marca o checkbox do item encontrado
                    }
                    K++;
                }
            }
            this.txtPapeis.Text = roleName;
        }  
    #endregion
    
    #region EVENTOS DE CONTROLES
    
    protected void btnIncluir_Click(object sender, EventArgs e)
    {
       #region VALIDAÇÕES PARA CADASTRAR/SALVAR O GRUPO

        //Valida antes de Salvar (No caso do Adiministrador TIVIT) se uma loja foi selecionada no combo
        if (ViewState["ItemID"] == null)
        {
            if (this.SelectedStore <= 0 && Roles.IsUserInRole("ADMINISTRADORTIVIT").Equals(true))
            {
                this.lblValidaddlStore.Visible = true;
                return;
            }
            else
                this.lblValidaddlStore.Visible = false;
        }

        //Verifica se foi atribuida permissões a este grupo
        bool Selected = this.ValidaPagesNulas(this.TreeGrupos.Nodes);
        try
        {
        if (Selected == false)
            throw new Exception("Nenhuma permissão foi atribuída para o novo Grupo.");
        }catch
        {
            this.CustomRole.IsValid = false;
            this.CustomRole.ErrorMessage = "Nenhuma permissão foi atribuída para o novo Grupo.";
            return;
        }

        try
        {           
            //Valida duplicidade do Nome do Grupo "ADMINISTRADORTIVIT"
            if (this.txtPapeis.Text.Equals("ADMINISTRADORTIVIT") && ViewState["ItemID"] == null)
            {
                throw new Exception(String.Concat("Grupo já existe! Por favor especifique um nome diferente para o Grupo."));
            }

            //Valida duplicidade do Nome do Grupo na mesma Loja       
            IList<MRole> oRoles;

            if(this.SelectedStore >0)
                oRoles = Role.GetRoleInStore(this.SelectedStore);
            else
                oRoles = Role.GetRoleInStore(UserLogadoStore);
            
            for (int i = 0; i < oRoles.Count; i++)
            {
                if(ViewState["ItemID"] == null)
                    if (oRoles[i].RoleName.ToString() == this.txtPapeis.Text.ToUpper())
                        throw new Exception(String.Concat("Grupo já existe! Por favor especifique um nome diferente para o Grupo."));
            }
        }
        catch
        {
            this.CustomRole.IsValid = false;
            this.CustomRole.ErrorMessage = String.Concat("Grupo já existente! Por favor especifique um nome diferente para o Grupo.");
            return;
        }

       #endregion         
        
       #region CADASTRA/SALVA GRUPO

        //Se o usuário logado, for o "Administrador TIVIT" então o sistema vai cadastrar 
        //o usuário na loja selecionado no combo.
        //Se o Usuário logado for o "Administrador da loja" então o sistema vai cadastrar 
        //o usuário na mesma loja do usuário logado

        int StoreRole;
        if (SelectedStore != 0)
            StoreRole = SelectedStore;
        else
            StoreRole = this.UserLogadoStore;
         
        if (ViewState["ItemID"] == null)
            this.ItemInsert(this.txtPapeis.Text.Trim().ToUpper(), this.txtPapeis.Text.ToLower(), StoreRole);
        else
            this.ItemSave((Guid)ViewState["ItemID"], ViewState["ItemNOME"].ToString().Trim().ToUpper(), StoreRole);
       #endregion
  }

    protected void btnCancelar_Click(object sender, EventArgs e)
    {
        this.LimpaTela();
    }

    protected void ddlStore_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["SelectedStore"] = this.ddlStore.SelectedValue;
        this.BindGrupos();
    }
            
    protected void rptUsuarios_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if(e.CommandName == "excluir")
        {
            if (this.SelectedStore > 0 && Roles.IsUserInRole("ADMINISTRADORTIVIT").Equals(true))
                this.ItemDelete(((Label)e.Item.FindControl("lblRoleName")).Text, this.SelectedStore);
            else
                this.ItemDelete(((Label)e.Item.FindControl("lblRoleName")).Text, this.UserLogadoStore);
            
            this.LimpaTela();
        }

        if (e.CommandName == "editar")
        {
            int StoreRole;
            if (this.ddlStore.SelectedIndex != 0)
                StoreRole = Convert.ToInt32(this.ddlStore.SelectedValue);
            else
                StoreRole = this.UserLogadoStore; //retorna o ID da Store do usuário logado

            this.ItemEdit(((Label)e.Item.FindControl("lblRoleName")).Text, StoreRole);
            this.btnIncluir.Text = "Salvar";
            this.txtPapeis.Enabled = false;
            this.ddlStore.Enabled = false;
        }
    }
    #endregion

    #region ROLES E PERMISSIONS

    //Recupera a chave do Grupo recentemente inserido
    private Guid ReturnRoleId(string RoleName, int StoreID)
    {
        Guid RoleId = Role.RetunRoleId(RoleName, StoreID);
        return RoleId;
    }

    //Insere permissões para o susário
    private void InserePermissions(string roleName, int StoreID)
    {
        IList<string[]> oListRoles = new List<string[]>();
        oListRoles = this.VarreTree(roleName, StoreID);
        PermissionPages.PermissionInRolesINSERT(oListRoles);
    }
    #endregion
    
    #region FUNÇÕES DO TREE
    private IList<string[]> VarreTree(string RoleName, int StoreID)
    {
        //Recupera a chave do Grupo recentemente inserido
        Guid RoleId = this.ReturnRoleId(RoleName, StoreID);

        //Varre TreeView e armazena os itens selecionados na lista      
        IList<string[]> oListRoles = new List<string[]>();
        int i = 0;

        TreeNodeCollection treeview = this.TreeGrupos.Nodes;

        foreach (TreeNode tre in treeview)//Nones Pais
        {
            TreeNodeCollection treChild = treeview[i].ChildNodes;//Nones Filhos deste Pai
            for (int j = 0; j < treChild.Count; j++)
            {
                if (treChild[j].Checked == true)
                {
                    string[] str = new string[2];
                    str[0] = treChild[j].Value;
                    str[1] = RoleId.ToString();
                    oListRoles.Add(str);
                }
            }
            i++;
        }
        return oListRoles;
    }

    private void BindTree()
    {
        this.TreeGrupos.ShowCheckBoxes = TreeNodeTypes.All; //Habilita o checkbox para o Treeview

        //Carregando o 1º nivel(pais) do TreeView
        IDataReader dr = PermissionPages.GetTree(-1);

        if (dr != null)
        {
            while (dr.Read())
            {
                TreeNode masterNode = new TreeNode();
                masterNode.ShowCheckBox = false;
                masterNode.Expand();
                masterNode.Value = dr["PageId"].ToString();
                masterNode.Text = dr["PageName"].ToString();
                this.TreeGrupos.Nodes.Add(masterNode);

                //Carregando o 2º nivel (filhos)
                IDataReader drFilhos = PermissionPages.GetTree(Convert.ToInt32(masterNode.Value));

                if (drFilhos != null)
                {
                    while (drFilhos.Read())
                    {
                        TreeNode ChildNode = new TreeNode();
                        ChildNode.Value = drFilhos["PermissionPagesId"].ToString();
                        ChildNode.Text = drFilhos["Description"].ToString();
                        // ChildNode.ValuePath = drFilhos["PagePath"].ToString();
                        masterNode.ChildNodes.Add(ChildNode);
                    }
                }
            }
        }
    }
    #endregion

    #region VALIDAÇÕES

    //VALIDA SE FOI SELECIONADA PERMISSÕES PARA ACESSO DE PAGINAS PARA O GRUPO
    private bool ValidaPagesNulas(TreeNodeCollection treeview)
    {
        bool Selected = false;
        int i = 0;

        //Vai varrendo um "None Pai" de cada vez
        foreach (TreeNode tre in treeview)
        {
            TreeNodeCollection treChild = treeview[i].ChildNodes;
            for (int j = 0; j < treChild.Count; j++)
            {
                if (treChild[j].Checked == true)
                {
                    Selected = true;
                    return Selected;
                }
            }
            i++;
        }
        return Selected;
    }
    #endregion
}
