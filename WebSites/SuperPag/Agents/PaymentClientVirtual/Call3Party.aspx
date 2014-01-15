<%@ Page Language="C#" MasterPageFile="~/Store/default/sp.master" AutoEventWireup="true" CodeFile="Call3Party.aspx.cs" Inherits="Agents_PaymentClientVirtual_Call3Party" %>
<asp:Content ID="Content2" ContentPlaceHolderID="tableTop" runat="Server">
    <asp:PlaceHolder ID="plhTableTop" runat="server"></asp:PlaceHolder>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="content" runat="Server">
    <script type="text/javascript" language='JavaScript'>
		window.name="SuperPag";
		
		var amexPopUp;
		var interval;
		
		function openAmexPopUp()
		{
			if(navigator.appName.indexOf("Netscape") != -1)
			    amexPopUp = window.open('<%= queryString %>', "PaymentClient","top=0,toolbar=0,location=0,directories=0,status=1,menubar=0,scrollbars=1,resizable=0,width=650,height=600"); //screenX=0,screenY=0,left=0,top=0
			else
			    amexPopUp = window.open('<%= queryString %>', "PaymentClient","top=0,toolbar=0,location=0,directories=0,status=1,menubar=0,scrollbars=1,resizable=1,width=650,height=600"); //screenX=0,screenY=0,left=0,top=0

			divStart.style.display = "none";
			divWait.style.display = "";		
			interval = window.setInterval ("closedPopUp();", 1000 );
		}
		
		function closedPopUp()
		{	
			if(amexPopUp.closed)
			{
			    document.formAmexReturn.submit();
				window.clearTimeout(interval);
				return;
			}
		}
		
		function focusPopUp()
		{
			if ( amexPopUp != undefined )
			{
				try { amexPopUp.focus(); } catch (e) {}
			}
		}
		function focusPopUpClick()
		{
			if ( amexPopUp != undefined )
			{
				try { amexPopUp.focus(); } catch (e) {}
			}
		}
	    function CheckPopUp()
		{
			var win;
			
			try
			{
				win=window.open("about:blank","_blank","toolbar=0,location=0,directories=0,status=1,menubar=0,scrollbars=0,resizable=0,screenX=0,screenY=0,left=2048,top=2048,width=50,height=50");
				win.close();
				openAmexPopUp();
			}
			catch(e)
			{
				document.getElementById("divStart").style.display="";
			}
		}
    </script>

    <div id="background" runat="server">
    </div>
    <div id="head" runat="server">
    </div>
    <div id="foot" runat="server">
        <span id="PngImage" runat="server" style="margin-top: 8px; margin-left: 4px; width: 149px;
            height: 27px"></span>
    </div>
    <div id="content" runat="server">
        <div id="divStart" style="display: none; text-align: center">
            <h2><font face="Verdana, Arial, Helvetica, sans-serif" color="#000099">Anti-PopUp detectado</font></h2>
            <p>
            <font size="1" face="Verdana, Arial, Helvetica, sans-serif">Para iniciar o processo, voce deve estar com seu anti-popup desligado.</font></p>
            <p>
            <a href="javascript:openAmexPopUp();"><font size="1" face="Verdana, Arial, Helvetica, sans-serif">clique aqui para iniciar</font></a></p>
        </div>
        <div id="divWait" style="display: none; text-align: center">
            <table width="610" border="0" cellpadding="0" cellspacing="3">
                <tr>
                    <td><img src="../../images/Amex/Tit_Pagamento.gif" width="600" height="115" border="0"></td>
                </tr>
                <tr>
                    <td align="center">		
                        <div align="center">
                        <p><font size="2"><b><font face="Verdana, Arial, Helvetica, sans-serif" color="#000099">Aguarde
                          transa&#231;&#227;o em andamento...</font></b></font></p>
                        <p><font size="1" face="Verdana, Arial, Helvetica, sans-serif">Em alguns segundos, dever&#225; aparecer o pop-up que ir&#225; efetuar o seu pagamento. Se esta tela n&#227;o aparecer, favor voltar para a tela de sele&#231;&#227;o de meios de pagamento e desabilitar o seu bloqueador de pop-ups.</font><font size="2" face="Verdana, Arial, Helvetica, sans-serif"><BR>&nbsp;</font><BR><br>
                        </p>
                        </div>
                    </td>
                </tr>
            </table>
       </div>
    </div>
    <script type="text/javascript" language='JavaScript'>
        CheckPopUp();
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="extraHtml" runat="Server">
<form name="formAmexReturn" id="formAmexReturn" action="check.aspx" method="GET">
    <input type="hidden" name="id" value="<%= Session["PaymentAttemptId"].ToString() %>">
</form>
</asp:Content>
