<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="AlterarSenha.aspx.cs" Inherits="Usuario_AlterarSenha" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ChangePassword ID="ChangePassword1" Width="100%" runat="server">
        <SuccessTemplate>
            <table border="0" class="tbPainelFRM">
                <tr>
                    <td align="center" class="tdBarraFerramentasSUM" colspan="2" style="height: 14px">
                        Alterar senha.</td>
                </tr>
                <tr>
                    <td style="height: 45px; padding-left: 5px; font-size: 11px;">
                        <b>Sua senha foi alterada com êxito.</b> &nbsp; &nbsp; &nbsp;&nbsp; &nbsp;
                        <asp:Button ID="btnFinalizar" runat="server" CausesValidation="False" CommandName="Continue"
                            Text="Finalizar" OnClick="btnFinalizar_Click" /></td>
                </tr>
                <tr>
                    <td align="right" colspan="2">
                        &nbsp;
                    </td>
                </tr>
            </table>
        </SuccessTemplate>
        <ChangePasswordTemplate>
            <table border="0" class="tbPainelFRM" style="width: 400px;" align="center">
                <tr>
                    <td align="center" colspan="2" class="tdBarraFerramentasSUM">
                        Alterar senha.</td>
                </tr>
                <tr>
                    <td align="right" style="width: 562px" class="tdLabelFRM">
                        &nbsp;<asp:Label ID="CurrentPasswordLabel" runat="server" AssociatedControlID="CurrentPassword">Senha:</asp:Label></td>
                    <td class="tdCampoFRM" align="left">
                        <asp:TextBox ID="CurrentPassword" runat="server" TextMode="Password"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="CurrentPasswordRequired" runat="server" ControlToValidate="CurrentPassword"
                            ErrorMessage="Digite a sua senha atual." ToolTip="Digite a senha atual." ValidationGroup="ChangePassword1">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td align="right" style="width: 562px" class="tdLabelFRM">
                        <asp:Label ID="NewPasswordLabel" runat="server" AssociatedControlID="NewPassword">Nova senha:</asp:Label></td>
                    <td class="tdCampoFRM" align="left">
                        <asp:TextBox ID="NewPassword" runat="server" TextMode="Password"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="NewPasswordRequired" runat="server" ControlToValidate="NewPassword"
                            ErrorMessage="Digite a nova senha" ToolTip="Digite a nova senha." ValidationGroup="ChangePassword1">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td align="right" style="width: 562px" class="tdLabelFRM">
                        <asp:Label ID="ConfirmNewPasswordLabel" runat="server" AssociatedControlID="ConfirmNewPassword">Confirmação da nova senha:</asp:Label></td>
                    <td class="tdCampoFRM" align="left">
                        <asp:TextBox ID="ConfirmNewPassword" runat="server" TextMode="Password"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="ConfirmNewPasswordRequired" runat="server" ControlToValidate="ConfirmNewPassword"
                            ErrorMessage="Confirme a nova senha." ToolTip="Confirme a nova senha." ValidationGroup="ChangePassword1">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td align="center" colspan="2">
                        <asp:CompareValidator ID="NewPasswordCompare" runat="server" ControlToCompare="NewPassword"
                            ControlToValidate="ConfirmNewPassword" Display="Dynamic" ErrorMessage="A nova senha deve coincidir com confimação da senha."
                            ValidationGroup="ChangePassword1"></asp:CompareValidator>
                    </td>
                </tr>
                <tr>
                    <td align="center" colspan="2" style="color: red">
                        <asp:Literal ID="FailureText" runat="server" EnableViewState="False"></asp:Literal>
                    </td>
                </tr>
                <tr>
                    <td class="tdCampoFRM">
                    </td>
                    <td class="tdCampoFRM" align="right">
                        <asp:Button ID="ChangePasswordPushButton" runat="server" CommandName="ChangePassword"
                            Text="Alterar senha" Width="90px" />&nbsp;
                        <asp:Button ID="CancelPushButton" runat="server" CausesValidation="False" CommandName="Cancel"
                            Text="Cancelar" Width="90px" />&nbsp;
                    </td>
                </tr>
            </table>
        </ChangePasswordTemplate>
    </asp:ChangePassword>
</asp:Content>
