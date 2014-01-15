<%@ page language="C#" masterpagefile="~/Store/default/sp.master" autoeventwireup="true" inherits="fillconsumer, App_Web_ummozifu" %>
<%@ Register Src="Controls/FillConsumer.ascx" TagName="FillConsumer" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="tableTop" runat="Server">
    <asp:PlaceHolder ID="plhTableTop" runat="server"></asp:PlaceHolder>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="Server">
    <div id="FillConsumer">
        <uc1:FillConsumer ID="FillConsumer1" runat="server" />
    </div>
</asp:Content>
