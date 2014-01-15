<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" inherits="Usuario_EditUser, App_Web_xj8aoxn2" %>
<%@ Register src="~/Usuario/ListaUser.ascx" tagname="ListaUser" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table>
        <tr>
            <td align="center" colspan="2" style="padding-left: 10px; vertical-align: top">
            </td>
        </tr>
        <tr>
            <td style="border-right: solid 2px #EAEAEA; width: 50%; padding-right: 10px;">
                <table border="0" class="tbPainelFRM">
                    <tr>
                        <td align="center" colspan="2" class="tdBarraFerramentasSUM">
                            Alterar usuário.
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabelFRM" style="height: 28px">
                            Identificação do Usuário:</td>
                        <td class="tdCampoFRM" style="height: 28px">
                            <asp:TextBox ID="txtIdentificacao" runat="server" ValidationGroup="grpUsuario" Enabled="False"></asp:TextBox>
                            </td>
                    </tr>
                    <tr>
                        <td class="tdLabelFRM">
                            Endereço de email</td>
                        <td class="tdCampoFRM">
                            <asp:TextBox ID="txtEmail" runat="server" ValidationGroup="grpUsuario"></asp:TextBox>&nbsp;
                            <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtEmail"
                                ErrorMessage="E-mail invalido!" ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                ValidationGroup="grpUsuario"></asp:RegularExpressionValidator></td>
                    </tr>
                    <tr>
                        <td class="tdLabelFRM" style="height: 29px">
                            Descrição:
                        </td>
                        <td class="tdCampoFRM" style="height: 29px">
                            <asp:TextBox ID="txtDescricao" runat="server" ValidationGroup="grpUsuario"></asp:TextBox>
                            </td>
                    </tr>
                    <tr>
                        <td class="tdLabelFRM">
                            Usuário ativo</td>
                        <td class="tdCampoFRM">
                            <asp:CheckBox ID="chStatus" runat="server" ValidationGroup="grpUsuario"/></td>
                    </tr>
                    <tr>
                        <td align="right" class="tdCampoFRM" colspan="2">
                            <asp:CustomValidator ID="CustomValidator1" runat="server" Font-Size="10pt" ForeColor="OrangeRed"></asp:CustomValidator>
                            <asp:Button ID="btnSalvar" runat="server"  ValidationGroup="grpUsuario" Text="Salvar" OnClick="btnSalvar_Click" Width="90px" />
                            <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" OnClick="btnCancelar_Click" Width="90px" />&nbsp;</td>
                    </tr>
                </table>
            </td>
            <td style="vertical-align: top; padding-left: 10px;">
                <table border="0" class="tbPainelFRM">
                    <tr>
                        <td align="center" colspan="2" class="tdBarraFerramentasSUM">
                            Inserir usuário em um grupo.
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCampoFRM">
                            <asp:CheckBoxList ID="chGrupos" runat="server" RepeatLayout="Flow">
                                <%--<asp:ListItem>User</asp:ListItem>
                                <asp:ListItem>Administrador</asp:ListItem>--%>
                            </asp:CheckBoxList>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <table>
    <tr align="center">
        <td>
            <asp:Label ID="lblResultado" runat="server" Visible="False" ForeColor="Red"></asp:Label>
        </td>
    </tr>
    </table>
    <table style="width: 975px">
    <tr>
       <td colspan="2" align="center" style="height: 288px">
           <uc1:ListaUser ID="ListaUser1" runat="server" />
       </td>
        </tr>
    </table>
    
</asp:Content>
