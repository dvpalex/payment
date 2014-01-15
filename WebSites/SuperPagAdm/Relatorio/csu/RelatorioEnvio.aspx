<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="RelatorioEnvio.aspx.cs" Inherits="Relatorio_Default" %>

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
       
       function abreLog(logFile) {
            //window.open(logFile,logFile,"status=1");
            window.open(logFile , '', "toolbar=no,location=no,directories=no,status=yes, menubar=no,scrollbars=yes,resizable=yes,copyhistory =yes,width=580,height=580,top=50,left=100");
       }
    </script>

    <table class="tbPainelFRM">
        <tr>
            <td align="center" colspan="4" class="tdBarraFerramentasSUM">
                Relatório de Envio CSU</td>
        </tr>
            <tr class="tdCampoFRM">
            <td align="left" class="tdCampoFRM">
                <asp:Label ID="lblPesquisar" runat="server" Text="Pesquisar por:"></asp:Label></td>
                <td colspan=2><asp:RadioButtonList ID="rdTipoData" runat="server" Height="19px" RepeatDirection="Horizontal"
                        Width="248px">
                        <asp:ListItem Selected="True" Value="0">Data da Transa&#231;&#227;o</asp:ListItem>
                        <asp:ListItem Value="1">Data do Arquivo</asp:ListItem>
                    </asp:RadioButtonList></td><td></td>
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
            <td align="left" class="tdCampoFRM" colspan="4">
                <asp:Button ID="btnBuscar" runat="server" OnClick="btnBuscar_Click" Text="Buscar"
                    BorderStyle="Solid" BorderWidth="1px" Width="87px" /></td>
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
                        CssClass="tbPainelFRM" ShowFooter="True" Width="975px" EmptyDataText="Não foi encontrado resultado para esta pesquisa.">
                        <HeaderStyle CssClass="tdBarraFerramentasSUM" HorizontalAlign="Center" BorderWidth="0px" />
                        <RowStyle HorizontalAlign="Center" BackColor="#F5F5F7" />
                        <FooterStyle CssClass="tdCampoFRM" HorizontalAlign="Center" />
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgLogCSU" runat="server" CommandName="LogCSU" ImageUrl="~/App_Themes/default/images/filenew.gif" OnCommand="VerTransacoes_click" CommandArgument='<%# Eval("NomeArquivoEnviado") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgDetais" runat="server" CommandName="VisualizarTransacoes"
                                        ImageUrl="~/App_Themes/default/images/details.gif" OnClientClick="abreLog('logs/log_arquivo.txt');" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="DataArquivo" HeaderText="Data da Transação" DataFormatString="{0:dd/MM/yyyy}" >
                            </asp:BoundField>
                            <asp:BoundField DataField="DataGeracaoLote" HeaderText="Data do Arquivo" DataFormatString="{0:dd/MM/yyyy}" >
                            </asp:BoundField>
                            <asp:BoundField DataField="NomeArquivoEnviado" HeaderText="Nome do Arquivo">
                            </asp:BoundField>
                            <asp:BoundField DataField="QtdeDetalhes" HeaderText="Qtde de Transa&#231;&#245;es">
                            </asp:BoundField>
                            <asp:BoundField DataField="ValorTotalDetalhes" HeaderText="Valor Total das Transa&#231;&#245;es" DataFormatString="{0:C2}" />
                            <asp:BoundField DataField="DescricaoStauts" HeaderText="Status">
                            </asp:BoundField>
                        </Columns>
                        <PagerSettings FirstPageText="" LastPageText="" Mode="NumericFirstLast" NextPageText="Pr&#243;ximo"
                            PreviousPageText="Anterior" />
                        <EmptyDataRowStyle Font-Bold="False" ForeColor="Red" />
                    </asp:GridView>                
            </td>
        </tr>
        <tr>
            <td align="left" class="tdCampoFRM" colspan="6">
                <asp:Button ID="btnExportar" runat="server" OnClick="btnExportar_Click" Text="Exportar para excel"
                    Visible="False" /></td>
        </tr>
    </table>
</asp:Content>
