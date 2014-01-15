<%@ page language="C#" masterpagefile="~/Store/default/sp.master" autoeventwireup="true" inherits="Agents_BBPag_naoaprovada, App_Web_qt-wfbg0" %>
<asp:Content ID="Content2" ContentPlaceHolderID="tableTop" runat="Server">
    <asp:PlaceHolder ID="plhTableTop" runat="server"></asp:PlaceHolder>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="content" runat="Server">
<span class="datatxt2">
<b><asp:Label ID="Label1" runat="server" meta:resourcekey="Label1Resource1">Sua transação não pode ser realizada.</asp:Label></b><br />
</span>
<div class="ajudatxt">
<b><asp:Label ID="Label2" runat="server" meta:resourcekey="Label2Resource1">Motivo:</asp:Label></b> <asp:Label ID="lblCode" runat="server" meta:resourcekey="lblCodeResource1"></asp:Label><br />
<asp:Label ID="lblMessage" runat="server" meta:resourcekey="lblMessageResource1"></asp:Label><br /><br />
</div>
<div class="datatxt2">
<b><asp:Label ID="Label3" runat="server" meta:resourcekey="Label3Resource1">Identifica&#231;&#227;o da Transa&#231;&#227;o: </asp:Label></b> <asp:Label ID="lblTid" runat="server" meta:resourcekey="lblTidResource1"></asp:Label>
</div>
<br />
<asp:HyperLink id="lnkEnd" runat="server" NavigateUrl="~/payment.aspx" meta:resourcekey="lnkEndResource1">&gt;&gt; Clique aqui para efetuar uma nova tentativa de pagamento</asp:HyperLink>
</asp:Content>
