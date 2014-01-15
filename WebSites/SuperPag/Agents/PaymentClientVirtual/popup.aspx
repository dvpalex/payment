<%@ Page Language="C#" AutoEventWireup="true" CodeFile="popup.aspx.cs" Inherits="Agents_PaymentClientVirtual_popup" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Superpag - Amex</title>
</head>
<body>
    <iframe name="frameRetorno" src="<%=queryString %>" marginheight="0" marginwidth="0" frameborder="0" scrolling="no" width="600" height="550"></iframe>
</body>
</html>
