<%@ page language="C#" masterpagefile="~/Store/default/sp.master" autoeventwireup="true" inherits="finalization, App_Web_ummozifu" %>

<asp:Content ID="Content2" ContentPlaceHolderID="tableTop" runat="Server">
    <asp:PlaceHolder ID="plhTableTop" runat="server"></asp:PlaceHolder>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="content" runat="Server">
    <div class="ajudatxt">
        <asp:Label ID="lblClientName" CssClass="ajudatxt" runat="server" />,<br />
        <br />
        <%= Server.HtmlEncode(InstrucaoFinalizacao) %>
    </div>
    <br />
    <br />
    <table cellspacing="1" cellpadding="2" width="100%" border="0">
        <tr>
            <td colspan="4" class="datatxt2">
                <b>
                    <asp:Label ID="Label1" runat="server" meta:resourcekey="Label1Resource1">N&#250;mero do Pedido:</asp:Label></b>
                <asp:Label ID="lblPedido" runat="server" meta:resourcekey="lblPedidoResource1"></asp:Label></td>
        </tr>
        <tr>
            <td colspan="4" class="datatxt2">
                <b>
                    <asp:Label ID="Label2" runat="server" meta:resourcekey="Label2Resource1">Forma de Pagamento:</asp:Label></b>
                <asp:Label ID="lblPaymentForm" runat="server" meta:resourcekey="lblPaymentFormResource1"></asp:Label></td>
        </tr>
        <tr>
            <td align="center" class="dataheader">
                <b>
                    <asp:Label ID="Label3" runat="server" meta:resourcekey="Label3Resource1">Qtde. de Parcelas</asp:Label></b></td>
            <td align="center" class="dataheader">
                <b>
                    <asp:Label ID="Label4" runat="server" meta:resourcekey="Label4Resource1">Valor da Parcela</asp:Label></b></td>
        </tr>
        <tr class="databg">
            <td align="center" style="height: 16px">
                <asp:Label ID="lblInstallmentQuantity" runat="server"></asp:Label></td>
            <td align="center" style="height: 16px">
                <asp:Label ID="lblInstalmentValue" runat="server"></asp:Label></td>
        </tr>
    </table>
    <br />
    <br />
    <asp:PlaceHolder ID="plhFinalization" runat="server"></asp:PlaceHolder>
    <br />
    <br />
    <table cellspacing="1" cellpadding="2" width="100%" border="0">
        <tr class="dataheader">
            <td valign="top">
                <b>
                    <asp:Label ID="Label5" runat="server" meta:resourcekey="Label5Resource1">C&#243;digo do Item</asp:Label></b></td>
            <td valign="top">
                <b>
                    <asp:Label ID="Label6" runat="server" meta:resourcekey="Label6Resource1">Descri&#231;&#227;o do Item</asp:Label></b></td>
            <td valign="top">
                <b>
                    <asp:Label ID="Label7" runat="server" meta:resourcekey="Label7Resource1">Qtde.</asp:Label></b></td>
            <td valign="top" align="right">
                <b>
                    <asp:Label ID="Label8" runat="server" meta:resourcekey="Label8Resource1">Valor Unit&#225;rio</asp:Label></b></td>
            <td valign="top" align="right">
                <b>
                    <asp:Label ID="Label9" runat="server" meta:resourcekey="Label9Resource1">Total Item</asp:Label></b></td>
        </tr>
        <asp:Repeater ID="rptItens" runat="server">
            <ItemTemplate>
                <tr class="databg">
                    <td>
                        <%# DataBinder.Eval(Container.DataItem, "ItemCode") %>
                    </td>
                    <td>
                        <%# DataBinder.Eval(Container.DataItem, "ItemDescription") %>
                    </td>
                    <td>
                        <%# DataBinder.Eval(Container.DataItem, "ItemQuantity") %>
                    </td>
                    <td align="right">
                        <%# DataBinder.Eval(Container.DataItem, "ItemValue", "{0:C}") %>
                    </td>
                    <td align="right">
                        <%# ((decimal)DataBinder.Eval(Container.DataItem, "ItemTotal")).ToString("C", SuperPag.Helper.GenericHelper.GetNumberFormatBrasil()) %>
                    </td>
                </tr>
            </ItemTemplate>
            <AlternatingItemTemplate>
                <tr class="databg2">
                    <td>
                        <%# DataBinder.Eval(Container.DataItem, "ItemCode") %>
                    </td>
                    <td>
                        <%# DataBinder.Eval(Container.DataItem, "ItemDescription") %>
                    </td>
                    <td>
                        <%# DataBinder.Eval(Container.DataItem, "ItemQuantity") %>
                    </td>
                    <td align="right">
                        <%# DataBinder.Eval(Container.DataItem, "ItemValue", "{0:C}") %>
                    </td>
                    <td align="right">
                        <%# ((decimal)DataBinder.Eval(Container.DataItem, "ItemTotal")).ToString("C", SuperPag.Helper.GenericHelper.GetNumberFormatBrasil()) %>
                    </td>
                </tr>
            </AlternatingItemTemplate>
        </asp:Repeater>
        <tr>
            <td style="height: 16px">
                &nbsp;</td>
            <td style="height: 16px">
                &nbsp;</td>
            <td style="height: 16px">
                &nbsp;</td>
            <td align="right" style="height: 16px" class="dataheader">
                <b>
                    <asp:Label ID="Label10" runat="server" meta:resourcekey="Label10Resource1">Subtotal</asp:Label></b></td>
            <td align="right" style="height: 16px" class="databg2">
                <asp:Label ID="lblSubTotal" runat="server"></asp:Label></td>
        </tr>
        <tr>
            <td style="height: 20px">
                &nbsp;</td>
            <td style="height: 20px">
            </td>
            <td style="height: 20px">
                &nbsp;</td>
            <td align="right" style="height: 20px" class="dataheader">
                <b>
                    <asp:Label ID="Label11" runat="server" meta:resourcekey="Label11Resource1">Frete</asp:Label></b></td>
            <td align="right" style="height: 20px" class="databg2">
                <asp:Label ID="lblFrete" runat="server"></asp:Label></td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td align="right" class="dataheader">
                <b>
                    <asp:Label ID="Label12" runat="server" meta:resourcekey="Label12Resource1">Total</asp:Label></b></td>
            <td align="right" class="databg2">
                <asp:Label ID="lblTotal" runat="server"></asp:Label></td>
        </tr>
    </table>
    <br />
    <div class="buttonlnkEnd">        
        <asp:HyperLink ID="lnkEnd" runat="server" ImageUrl="~/Store/default/images/finalizar.gif"
            NavigateUrl="#" Target="_parent">Finalizar Compra</asp:HyperLink>        
    </div>
</asp:Content>
