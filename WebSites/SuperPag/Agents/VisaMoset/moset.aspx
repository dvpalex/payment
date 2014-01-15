<%@ Page Language="C#" MasterPageFile="~/Store/default/sp.master" AutoEventWireup="true" CodeFile="moset.aspx.cs" Inherits="Agents_VisaMoset_moset" %>
<asp:Content ID="Content2" ContentPlaceHolderID="tableTop" runat="Server">
    <asp:PlaceHolder ID="plhTableTop" runat="server"></asp:PlaceHolder>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="content" runat="Server">
    <div id="foot" runat="server">
        <span id="PngImage" runat="server" style="margin-top: 8px; margin-left: 4px; width: 149px;
            height: 27px"></span>
    </div>
    <div id="content" runat="server">
        <div id="divWait" style="text-align: center">
            <table width="610" border="0" cellpadding="0" cellspacing="3">
                <tr>
                    <td><img src="../../images/Visa/Tit_Pagamento.gif" width="600" height="115" border="0"></td>
                </tr>
                <tr>
                    <td align="center">		
                        <div align="center">
                        <p><font size="2"><b><font face="Verdana, Arial, Helvetica, sans-serif" color="#000099">Aguarde
                          transa&#231;&#227;o em andamento...</font></b></font></p>
                        <p><font size="1" face="Verdana, Arial, Helvetica, sans-serif">Sua transação já está sendo processada. Por favor aguarde até a transação ser finalizada. Este processo pode levar até 30 segundos.</font><font size="2" face="Verdana, Arial, Helvetica, sans-serif"><BR>&nbsp;</font><BR><br>
                        </p>
                        </div>
                    </td>
                </tr>
            </table>
       </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="extraHtml" runat="Server">
    <form id="visa" name="visa" action="result.aspx" method="POST">
    </form>
    <script type="text/javascript">
      var form = document.getElementById("visa");
      form.submit();
    </script>
</asp:Content>
