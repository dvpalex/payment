<%@ Page Language="C#" MasterPageFile="~/Store/default/sp.master" AutoEventWireup="true" CodeFile="fillconsumer.aspx.cs" Inherits="fillconsumer" %>
<%@ Register Src="Controls/FillConsumer.ascx" TagName="FillConsumer" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="tableTop" runat="Server">
    <asp:PlaceHolder ID="plhTableTop" runat="server"></asp:PlaceHolder>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="Server">
    <div id="FillConsumer">
        <uc1:FillConsumer ID="FillConsumer1" runat="server" />
    </div>
</asp:Content>
