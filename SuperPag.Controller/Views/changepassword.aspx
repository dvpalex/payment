<%@ Page Language="C#" EnableTheming="true" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeFile="changepassword.aspx.cs" Inherits="Views_USER_changepassword" %>
<%@ Register TagPrefix="fwc" Namespace="SuperPag.Framework.Web.WebControls" Assembly="SuperPag.Framework" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Conteudo" Runat="Server">
    <div id="SubTitulo">
        <asp:Image ID="imgHome" runat="server" SkinID="imgEmployee" />&nbsp;
        <asp:LoginName ID="LoginName1" runat="server" />
	</div>

	<!--Painel de Formulario-->
    <asp:ChangePassword ID="ChangePassword1" runat="server" CancelButtonText="Cancelar"
        ChangePasswordButtonText="Alterar" ChangePasswordFailureText="Senha inválida"
        ChangePasswordTitleText="" ConfirmNewPasswordLabelText="Confirme a senha" ConfirmPasswordCompareErrorMessage="As senhas não conferem"
        ConfirmPasswordRequiredErrorMessage="Confirmação de senha requerida" NewPasswordLabelText="Nova senha"
        NewPasswordRegularExpressionErrorMessage="Entre com uma senha diferente da atual"
        NewPasswordRequiredErrorMessage="Nova senha requerida" PasswordLabelText="Senha atual"
        PasswordRequiredErrorMessage="Senha atual requerida" SuccessText="Senha alterada com sucesso"
        SuccessTitleText="Alteração de senha completa" ContinueButtonText="Continuar" ContinueDestinationPageUrl="~/ShowHome.do" >
    </asp:ChangePassword>
</asp:Content>

