<%@ page language="C#" autoeventwireup="true" inherits="_Default, App_Web_7cw0kyry" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.1//EN" "http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Login</title>
    <link href="App_Themes/default/geral.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server" style="vertical-align: middle;">        
        <div id="Login">
            <div id="LoginTop">
            </div>
            <div id="Loginleft">
            </div>
            <div id="Loginright">
            </div>
            <div id="Loginconteudo">
                <asp:Login CssClass="textlogin" ID="LoginController" runat="server" FailureText="Acesso negado"
                    LoginButtonText="Login" PasswordLabelText="Senha:" UserNameLabelText="Usuário:"
                    Height="118px" Font-Names="Arial, Helvetica, sans-serif" Font-Size="10pt" ForeColor="White"
                    TitleText="" DestinationPageUrl="~/Pagamento/Default.aspx" DisplayRememberMe="False" OnAuthenticate="LoginController_Authenticate">
                </asp:Login>
            </div>
            <div id="Loginbottom">
                </div>
        </div>
    </form>
</body>
</html>
