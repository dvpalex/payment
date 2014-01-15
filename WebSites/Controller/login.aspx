<%@ Page Language="C#" EnableTheming="true" AutoEventWireup="true" CodeFile="login.aspx.cs" Inherits="Views_login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
    <head id="Head1" runat="server">
        <title>SUPERPAG - Controller</title>
        <meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1"/>
    </head>
<body>
<form id="form1" runat="server" >
  <div id="login">
     <table id="loginbox" cellpadding="0" cellspacing="0">
      <tr>
        <td colspan="3"><asp:Image ID="Image1" SkinID="login_01" runat="server" /></td>
      </tr>
      <tr>
        <td><asp:Image ID="Image2" SkinID="login_02" runat="server"/></td>
        <td class="fundologin" >
            <asp:Login CssClass="textlogin" ID="Login1" runat="server" FailureText="Acesso negado" LoginButtonText="Login" PasswordLabelText="Senha:" UserNameLabelText="Usuário:" Height="118px" Font-Names="Arial, Helvetica, sans-serif" Font-Size="10pt" ForeColor="White" TitleText="" DestinationPageUrl="ShowHome.do" DisplayRememberMe="False">
            </asp:Login>   
        </td>
        <td><asp:Image ID="Image3" SkinID="login_04" runat="server"/></td>
      </tr>
      <tr>
        <td colspan="3" style="height: 72px"><asp:Image ID="Image4" SkinID="login_05" runat="server" /></td>
      </tr>
    </table>                  
  </div>
</form>
</body>
</html>
