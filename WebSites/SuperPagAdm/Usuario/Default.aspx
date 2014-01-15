<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Default.aspx.cs" Inherits="Usuario_Default" %>

<%@ Register Src="~/Usuario/ListaUser.ascx" TagName="ListaUser" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table>
        <tr>
            <td style="border-right: solid 2px #EAEAEA; width: 50%; padding-right: 10px; height: 296px;" valign="top">
                <asp:CreateUserWizard ID="CreateUserWizard" runat="server" OnCreatingUser="CreateUserWizard_CreatingUser"
                    OnCreatedUser="CreateUserWizard_CreatedUser" Width="100%" LoginCreatedUser="False" DuplicateUserNameErrorMessage="Ja existe um usuário com este nome. Por favor especifique outro nome." CreateUserButtonText="Criar Usuário" DuplicateEmailErrorMessage="Ja existe um usuário com este e-mail. Por favor especifique outro e-mail." InvalidPasswordErrorMessage="A senha deve conter no mínimo 6 caracteres.">
                    <WizardSteps>
                        <asp:CreateUserWizardStep ID="CreateUserWizardStep1" runat="server">
                            <ContentTemplate>
                                <table border="0" class="tbPainelFRM">
                                    <tr>
                                        <td align="center" colspan="2" class="tdBarraFerramentasSUM">
                                            Criar novo usuário.
                                        </td>
                                    </tr>
                                    <tr id="TRStore" runat="server" visible="false">
                                        <td align="right" class="tdLabelFRM">
                                            <asp:Label ID="StoreLabel" runat="server" AssociatedControlID="ddlStore">Loja:</asp:Label>
                                        </td>
                                        <td class="tdCampoFRM" style="width: 612px">
                                           <asp:DropDownList ID="ddlStore" DataValueField="StoreId" DataTextField="Name" runat="server" Width="200px" AppendDataBoundItems="True" AutoPostBack="True" OnSelectedIndexChanged="ddlStore_SelectedIndexChanged">
                                                <asp:ListItem Text="Selecione" Value="-1"></asp:ListItem>
                                            </asp:DropDownList>&nbsp;
                                            <asp:Label ID="lblValidaLoja" runat="server" ForeColor="Red" Text="*" Visible="False"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td align="right" class="tdLabelFRM">
                                            <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName">Nome:</asp:Label></td>
                                        <td class="tdCampoFRM" style="width: 612px">
                                            <asp:TextBox ID="UserName" runat="server" CssClass="tdCampoFRMINPUT" Width="300px"></asp:TextBox>&nbsp;
                                            <asp:Label ID="lblValidaNome" runat="server" ForeColor="Red" Text="*" Visible="False"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td align="right" class="tdLabelFRM">
                                            <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password">Senha:</asp:Label></td>
                                        <td class="tdCampoFRM" style="width: 612px">
                                            <asp:TextBox ID="Password" runat="server" TextMode="Password" CssClass="tdCampoFRMINPUT"
                                                Width="150px"></asp:TextBox>&nbsp;
                                            <asp:Label ID="lblValidaSenha" runat="server" ForeColor="Red" Text="*" Visible="False"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td align="right" class="tdLabelFRM">
                                            <asp:Label ID="ConfirmPasswordLabel" runat="server" AssociatedControlID="ConfirmPassword">Confirmar Senha:</asp:Label></td>
                                        <td class="tdCampoFRM" style="width: 612px">
                                            <asp:TextBox ID="ConfirmPassword" runat="server" TextMode="Password" CssClass="tdCampoFRMINPUT"
                                                Width="150px"></asp:TextBox>&nbsp;
                                            <asp:Label ID="lblValidaConfirmSenha" runat="server" ForeColor="Red" Text="*" Visible="False"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td align="right" class="tdLabelFRM">
                                            <asp:Label ID="EmailLabel" runat="server" AssociatedControlID="Email">E-mail:</asp:Label></td>
                                        <td class="tdCampoFRM" style="width: 612px">
                                            <asp:TextBox ID="Email" runat="server" CssClass="tdCampoFRMINPUT" Width="300px"></asp:TextBox>
                                            &nbsp;
                                            <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="Email"
                                                ErrorMessage="E-mail invalido!" ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" Font-Size="X-Small"></asp:RegularExpressionValidator></td>
                                    </tr>
                                    <tr>
                                        <td align="center" colspan="2">
                                            <asp:CompareValidator ID="PasswordCompare" runat="server" ControlToCompare="Password"
                                                ControlToValidate="ConfirmPassword" Display="Dynamic" ErrorMessage="A senha deve coincidir com a confimação da senha."
                                                ValidationGroup="CreateUserWizard1"></asp:CompareValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" colspan="2" style="color: red; height: 14px;">
                                            <asp:Literal ID="ErrorMessage" runat="server" EnableViewState="False"></asp:Literal><br />
                                            <asp:CustomValidator ID="CustomRoles" runat="server" ControlToValidate="UserName"
                                                ValidationGroup="CreateUserWizard1" ErrorMessage="CustomValidator"></asp:CustomValidator>
                                            </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </asp:CreateUserWizardStep>
                        <asp:CompleteWizardStep runat="server">
                            <ContentTemplate>
                                <table border="0" class="tbPainelFRM">
                                    <tr>
                                        <td align="center" colspan="2" class="tdBarraFerramentasSUM">
                                            Criar novo usuário.
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="height: 45px; padding-left: 5px; font-size: 11px;">
                                            <b>Sua conta foi criada com êxito.</b>&nbsp;&nbsp;&nbsp;&nbsp;<asp:Button ID="btnConcluir"
                                                runat="server" Text="Concluir" OnClick="btnConcluir_Click"></asp:Button>
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </asp:CompleteWizardStep>
                    </WizardSteps>
                </asp:CreateUserWizard>
            </td>
            <td style="vertical-align: top; padding-left: 10px; height: 296px;" valign="top" colspan="">
                <table border="0" class="tbPainelFRM">
                    <tr>
                        <td align="center" colspan="2" class="tdBarraFerramentasSUM">
                            Inserir usuário em um grupo.
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCampoFRM">
                            <asp:CheckBoxList ID="chGrupos" runat="server" RepeatLayout="Flow">
                            </asp:CheckBoxList>
                        </td>
                    </tr>
                </table>
                <asp:CustomValidator ID="CustomRoles1" runat="server" ErrorMessage="CustomValidator" ValidationGroup="CreateUserWizard1" Font-Names="Arial" Font-Size="X-Small"></asp:CustomValidator>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <uc1:ListaUser ID="ListaUser" runat="server" />
            </td>
        </tr>
    </table>
</asp:Content>
