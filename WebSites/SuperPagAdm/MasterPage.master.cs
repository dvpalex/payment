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
using System.Collections.Generic;

public partial class MasterPage : System.Web.UI.MasterPage
{

    #region PROPRIEDADES DA PÁGINA
   
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
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.BindMenu();
        }
    }

    private void BindMenu()
    {
        //RECUPERA OS GRUPOS DO USUÁRIO LOGADO
        string[] oGruposUsu = Roles.GetRolesForUser();

        //Variáveis auxiliares para evitar duplicidade de menus e submenus.
        List<string> str = new List<string>();
        List<string> strfilho = new List<string>();

        for (int i = 0; i < oGruposUsu.Length; i++)
        {
            //Recupera a chave do Grupo
            Guid RoleId = Role.RetunRoleId(oGruposUsu[i], UserLogadoStore);

            //CRIA A LISTA DOS ITENS DO MENU PAI
            IList<MPages> ObjListItem = new List<MPages>();

            if (oGruposUsu[i] == "Administrador")
                ObjListItem = Pages.GetMenu(-1);
            else
                ObjListItem = Pages.GetMenu(-1, RoleId);

            //for (int j = 0; j < ObjListItem.Count; j++)
            for (int j = 0; j < 2; j++)
            {
                //Verifica antes de criar o menu PAI se o mesmo já não foi criado por um outro grupo
                if (!str.Contains(ObjListItem[j].PageId.ToString()))
                {
                    str.Add(ObjListItem[j].PageId.ToString());
                    MenuSgr.Items.Add(new MenuItem(("&nbsp;" + ObjListItem[j].PageName + "&nbsp;|"), ObjListItem[j].PageId.ToString()));
                }
            }
            
            foreach (MenuItem Item in MenuSgr.Items)
            {
               CriaSubItens(Item,oGruposUsu[i], RoleId);
            }
        }
    }



    private void CriaSubItens(MenuItem Item, string oGruposUsu, Guid RoleId)
    {
        List<string> strfilho = new List<string>();
        IList<MPages> ObjListItem = new List<MPages>();
        IList<MPages> ObjList = new List<MPages>();
        if (oGruposUsu == "Administrador")
            ObjList = Pages.GetMenu(Convert.ToInt32(Item.Value));
        else
            ObjList = Pages.GetMenu(Convert.ToInt32(Item.Value), RoleId);

        for (int j = 0; j < ObjList.Count; j++)
        {
            //Verifica antes de criar o menu FILHO se o mesmo já não foi criado por um outro grupo
            if (!strfilho.Contains(ObjList[j].PageId.ToString()))
            {
                strfilho.Add(ObjList[j].PageId.ToString());
                MenuItem ItemS = new MenuItem(ObjList[j].PageName, ObjList[j].PageId.ToString(), string.Empty, ObjList[j].PagePath);
                CriaSubItens(ItemS, oGruposUsu, RoleId);
                Item.ChildItems.Add(ItemS);
                ItemS = null;
            }
            
        }
        
    }
}
