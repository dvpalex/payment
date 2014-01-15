<%@ Page Language="C#" AutoEventWireup="true" CodeFile="notifica.aspx.cs" Inherits="Agents_BBPag_notifica" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>BBPag</title>
    <script language="Javascript" type="text/javascript">
    function Post()
    {
        window.opener.document.formPagamento.tpPagamento.value = "<%=Request["tpPagamento"]%>"
        window.opener.document.formPagamento.refTran.value = "<%=Request["refTran"]%>"
        window.opener.document.formPagamento.idConv.value = "<%=Request["idConv"]%>"

        window.opener.document.formPagamento.submit();
        self.close();
    }
    </script>
</head>
<body>
	<script language="JavaScript" type="text/javascript">
           Post();
	</script>
	<form id="form1" runat="server">
    <div>
    
    </div>
    </form>
</body>
</html>
