<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Default.aspx.cs" Inherits="Relatorio_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript">
       function mascara(event,campo,tipo){         
        if(event.keyCode<48 || event.keyCode>57)
           {                 
                event.returnValue=false;
           }  
        if(campo.value.length==2 || campo.value.length==5 && event.keyCode!=8){  
           if (tipo == 'tempo') {  
               campo.value+=":";  
           } 
           else if(event.keyCode!=8)
           {  
               campo.value+="/";  
           }  
          }  
       }  
    </script>

    <table class="tbPainelFRM">
        <tr>
            <td align="center" colspan="4" class="tdBarraFerramentasSUM">
                Relatório</td>
        </tr>
        <tr>
            <td class="tdLabelFRM" align="left">
                <asp:Label ID="lblInicial" runat="server">Data Inicial:</asp:Label></td>
            <td class="tdCampoFRM" style="width: 138px">
                <asp:TextBox ID="txtInicial" runat="server" MaxLength="10" onkeypress="mascara(event,this,'data')"
                    Width="100px"></asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator2"
                        runat="server" ControlToValidate="txtInicial" ErrorMessage="*"></asp:RequiredFieldValidator></td>
            <td class="tdLabelFRM">
                <asp:Label ID="lblFinal" runat="server">Data Final:</asp:Label></td>
            <td class="tdCampoFRM">
                &nbsp;<asp:TextBox ID="txtFinal" runat="server" Width="100px" MaxLength="10" onkeypress="mascara(event,this,'data')"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtFinal"
                    ErrorMessage="*"></asp:RequiredFieldValidator></td>
        </tr>
        <tr>
            <td class="tdLabelFRM" align="left">
                <asp:Label ID="lblMeioEnio" runat="server" Width="111px">Meio de Envio:</asp:Label></td>
            <td class="tdCampoFRM" colspan="3">
                <asp:DropDownList ID="ddlMeioEnvio" runat="server" Width="106px" AppendDataBoundItems="True">
                    <asp:ListItem Value="" Text="Selecione"></asp:ListItem>
                    <asp:ListItem>Correio</asp:ListItem>
                    <asp:ListItem>E-mail</asp:ListItem>
                    <asp:ListItem>Fax</asp:ListItem>
                    <asp:ListItem>Impress&#227;o</asp:ListItem>
                    <asp:ListItem>Todos</asp:ListItem>
                </asp:DropDownList></td>
        </tr>
        <tr>
            <td class="tdLabelFRM" align="left">
                <asp:Label ID="lblNomeSacado" runat="server" Width="84px">Nome Sacado:</asp:Label></td>
            <td class="tdCampoFRM" colspan="3">
                <asp:TextBox ID="txtNomeSacado" runat="server" MaxLength="10" Width="311px"></asp:TextBox></td>
        </tr>
        <tr>
            <td class="tdLabelFRM" align="left">
                <asp:Label ID="lblIPTE" runat="server">IPTE:</asp:Label></td>
            <td class="tdCampoFRM" colspan="3">
                <asp:TextBox ID="txtIPTE" runat="server" Width="311px"></asp:TextBox></td>
        </tr>
        <tr>
            <td class="tdLabelFRM" align="left">
                <asp:Label ID="lblContrato" runat="server">Contrato:</asp:Label></td>
            <td class="tdCampoFRM" colspan="3">
                <asp:TextBox ID="txtContrato" runat="server" MaxLength="10" Width="100px"></asp:TextBox></td>
        </tr>
        <tr>
            <td align="right" class="tdCampoFRM" colspan="4">
                <asp:Button ID="btnBuscar" runat="server" OnClick="btnBuscar_Click" Text="Buscar"
                    BorderStyle="Solid" BorderWidth="1px" Width="87px" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            </td>
        </tr>
        <tr>
            <td class="tdCampoFRM" colspan="4">
                <asp:CustomValidator ID="CustomRelatorio" runat="server" ErrorMessage="CustomValidator"></asp:CustomValidator></td>
        </tr>
    </table>
    <table>
        <tr>
            <td class="tdCampo" colspan="6" align="center" valign="top">                
                    <asp:GridView runat="server" ID="GridRel" GridLines="None" CellPadding="0" CellSpacing="1"
                        AllowPaging="True" AutoGenerateColumns="False" OnPageIndexChanging="GridRel_PageIndexChanging"
                        CssClass="tbPainelFRM" ShowFooter="True" Width="975px" PageSize="15" EmptyDataText="Não foi encontrado resultado para esta pesquisa.">
                        <HeaderStyle CssClass="tdBarraFerramentasSUM" HorizontalAlign="Center" BorderWidth="0px" />
                        <PagerStyle CssClass="tdCampoFRM" />
                        <RowStyle CssClass="tdCampoFRM" HorizontalAlign="Center" />
                        <FooterStyle CssClass="tdCampoFRM" HorizontalAlign="Center" />
                        <Columns>
                            <asp:BoundField DataField="paymentDate" HeaderText="Data/Hora" >
                                <HeaderStyle Width="100px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="UserName" HeaderText="Usu&#225;rio">
                                <HeaderStyle Width="70px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Sacado" HeaderText="Nome do Sacado">
                                <HeaderStyle Width="100px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="barcode" HeaderText="C&#243;d. IPTE" />
                            <asp:BoundField DataField="expirationPaymentDate" HeaderText="Vencimento" DataFormatString="{0:dd/MM/yyyy}">
                                <HeaderStyle Width="100px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="MeioEnvio" HeaderText="Meio Envio">
                                <HeaderStyle Width="80px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Status" HeaderText="Enviado">
                                <HeaderStyle Width="70px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Contrato" HeaderText="Contrato">
                                <HeaderStyle Width="80px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="ErrorMail" HeaderText="Erro" />
                        </Columns>
                        <PagerSettings FirstPageText="" LastPageText="" Mode="NumericFirstLast" NextPageText="Pr&#243;ximo"
                            PreviousPageText="Anterior" />
                        <EmptyDataRowStyle Font-Bold="False" ForeColor="Red" />
                    </asp:GridView>                
            </td>
        </tr>
        <tr>
            <td align="right" class="tdCampo" colspan="6" style="padding: 10px">
                <asp:Button ID="btnExportar" runat="server" OnClick="btnExportar_Click" Text="Exportar para excel"
                    Visible="False" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            </td>
        </tr>
    </table>
</asp:Content>
