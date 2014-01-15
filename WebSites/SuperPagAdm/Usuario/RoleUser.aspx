<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="RoleUser.aspx.cs" Inherits="Usuario_Roles" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table>
        <tr>
            <td style="border-right: solid 2px #EAEAEA; width: 30%; padding-right: 10px;" valign="top">
                <table class="tbPainelFRM" style="width: 95%">
                    <tr>
                        <td class="tdBarraFerramentasSUM" colspan="2">
                            Cadastro de papeis
                        </td>
                    </tr>
                    <tr id="TRStore" runat="server" visible="false">
                        <td class="tdLabelFRM" style="height: 22px">Loja:</td>
                        <td class="tdCampoFRM" style="width: 612px; height: 22px;">
                           <asp:DropDownList ID="ddlStore" DataValueField="StoreId" DataTextField="Name" runat="server" Width="200px" AppendDataBoundItems="True" AutoPostBack="True" OnSelectedIndexChanged="ddlStore_SelectedIndexChanged">
                                <asp:ListItem Text="Selecione"></asp:ListItem>
                            </asp:DropDownList>&nbsp;
                            <asp:Label ID="lblValidaddlStore" runat="server" ForeColor="Red" Text="*" Visible="False"></asp:Label></td>
                    </tr>
                    <tr>
                        <td class="tdLabelFRM">Papeis:</td>
                        <td class="tdCampoFRM">
                            <asp:TextBox ID="txtPapeis" runat="server" MaxLength="30" Width="195px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rvPapeis" runat="server" ControlToValidate="txtPapeis" Display="Dynamic" ValidationGroup="grpInsert">*</asp:RequiredFieldValidator></td>
                    </tr>
                    <tr>
                        <td class="tdCampoFRM" colspan="2">
                            <div style="overflow: auto; height:350px;">
                                <asp:TreeView ID="TreeGrupos" runat="server" ShowCheckBoxes="Leaf" ExpandDepth="0"
                                    ExpandImageToolTip="" ShowLines="True">
                                </asp:TreeView>
                            </div>
                        </td>
                    </tr>
                </table>
                <div style="text-align: right; width: 280px;">
                    <asp:Button ID="btnIncluir" runat="server" Text="Inserir" Width="104px" Font-Bold="True"
                        OnClick="btnIncluir_Click" ValidationGroup="grpInsert" />&nbsp;
                    <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" Width="104px" Font-Bold="True"
                        OnClick="btnCancelar_Click" />
                </div>
            </td>
            <td style="vertical-align: top; padding-left: 10px;" valign="top">
                <asp:Repeater ID="rptGrupos" runat="server" EnableTheming="True" OnItemCommand="rptUsuarios_ItemCommand">
                    <HeaderTemplate>
                        <table border="0" class="tbPainelFRM" style="text-align: center; width: 85%;">
                            <tr>
                                <td class="tdBarraFerramentasSUM" align="center">
                                    Papeis
                                </td>
                                <td class="tdBarraFerramentasSUM" align="center">
                                    Editar
                                </td>
                                <td class="tdBarraFerramentasSUM" align="center">
                                    Excluir
                                </td>
                            </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td align="left" class="tdCampoFRM">
                                <asp:Label ID="lblRoleName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"RoleName") %>'></asp:Label></td>
                            <td align="Center" class="tdCampoFRM">
                                <asp:LinkButton ID="lnkEditar" runat="server" CommandName="editar">Editar</asp:LinkButton></td>
                            <td align="Center" class="tdCampoFRM">
                                <asp:ImageButton ID="imgExcluir" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"RoleName") %>'
                                    CommandName="excluir" ImageAlign="AbsMiddle" ImageUrl="~/App_Themes/default/images/excluir.gif"
                                    OnClientClick="return confirm('Deseja excluir este Grupo?');" runat="server"
                                    ToolTip="Excluir" />
                            </td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>                       
                        </table>
                    </FooterTemplate>
                </asp:Repeater> 
                <asp:CustomValidator ID="CustomRole" runat="server" ErrorMessage="CustomValidator" Font-Size="10pt"></asp:CustomValidator>                
            </td>
        </tr>
        <tr>
            <td colspan="2" valign="top">
            </td>
        </tr>
    </table>
</asp:Content>
