<%@ page language="C#" autoeventwireup="true" inherits="error, App_Web_ummozifu" culture="auto" uiculture="auto" meta:resourcekey="PageResource1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>SuperPag - Erro</title>
</head>
<body>
    <form id="form1" runat="server">
    <div style="text-align: center;">
    <table cellpadding="0" cellspacing="0" border="0" width="452">
        <tr>
            <td colspan="2" valign="middle"><img src="/Images/x1.png" alt=""/></td>
        </tr>
        <tr style="background-color: #F6F6F6; padding-top: 20px; padding-bottom: 10px;">
            <td style="text-align: right; padding-right: 10px; width: 50px;">
                <img src="/Images/error.gif" border="0" alt=""/>
            </td>
            <td style="text-align: left; width: 402px;">
                <b>Ocorreu um erro durante o processo de pagamento!</b>
            </td>
        </tr>
        <tr style="background-color: #F6F6F6; padding-bottom: 20px;">
            <td valign="middle"></td>
            <td valign="middle" style="text-align: left;">
                <asp:Label ID="lblDescription" runat="server" />
            </td>
        </tr>
        <tr style="background-image: url('/Images/x2.png');">
            <td colspan="2"><img src="/Images/x2.png" border="0" alt=""/></td>
        </tr>
    </table>
        </div>
    </form>
</body>
</html>
