<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="RelatorioContabilizacaoMateratmp.aspx.cs" Inherits="Relatorio_Default" %>

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
            <td align="center" colspan="4" class="tdBarraFerramentasSUM" style="height: 14px">
                Relatório de Contabilização Matera</td>
        </tr>
        <tr>
            <td class="tdLabelFRM" align="left">
                <asp:Label ID="lblInicial" runat="server">Data:</asp:Label></td>
            <td class="tdCampoFRM" colspan="3">
                <asp:TextBox ID="txtInicial" runat="server" MaxLength="10" onkeypress="mascara(event,this,'data')"
                    Width="100px"></asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator2"
                        runat="server" ControlToValidate="txtInicial" ErrorMessage="*"></asp:RequiredFieldValidator></td>
        </tr>
        <tr>
            <td align="left" class="tdCampoFRM" colspan="4" style="height: 27px">
                <asp:Button ID="btnBuscar" runat="server" OnClick="btnBuscar_Click" Text="Buscar"
                    BorderStyle="Solid" BorderWidth="1px" Width="87px" /></td>
        </tr>
        <tr>
            <td class="tdCampoFRM" colspan="4" style="height: 21px">
                <asp:CustomValidator ID="CustomRelatorio" runat="server" ErrorMessage="CustomValidator"></asp:CustomValidator></td>
        </tr>
    </table>
    <table>
        <tr>
            <td colspan="6" align="right" valign="top">  
                
                <asp:Table ID="tblResumo" runat="server" Width="975px" CssClass="tbdivPainelFRM" CellSpacing="1">
                </asp:Table>
                          
                <%--<asp:Table ID="tblResumo" runat="server" Width="975px" CssClass="tbdivPainelFRM" CellSpacing="1">
                     <asp:TableRow ID="TableRow2" runat="server" CssClass="tbPainelFRM" Font-Size="Small" HorizontalAlign="Left">
                        <asp:TableCell ColumnSpan="10">Processadas CSU</asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow runat="server" CssClass="tdBarraFerramentasSUM" Font-Size="Small">
                        <asp:TableCell runat="server" >Banco</asp:TableCell>
                        <asp:TableCell runat="server" >Nome</asp:TableCell>
                        <asp:TableCell runat="server" >Conta Banco</asp:TableCell>
                        <asp:TableCell runat="server" >Conta Transit.</asp:TableCell>
                        <asp:TableCell ID="TableCell1" >Dt Processamento</asp:TableCell>
                        <asp:TableCell ID="TableCell2" >Dt Operação</asp:TableCell>
                        <asp:TableCell ID="TableCell13" >&nbsp;</asp:TableCell>
                        <asp:TableCell ID="TableCell4" >Valor</asp:TableCell>
                        <asp:TableCell ID="TableCell5" >&nbsp;</asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="TableRow5" runat="server" CssClass="tbPainelFRM" Font-Size="Small">
                        <asp:TableCell ID="TableCell15" runat="server" >1001</asp:TableCell>
                        <asp:TableCell ID="TableCell16" runat="server" >BANCO DO BRASIL</asp:TableCell>
                        <asp:TableCell ID="TableCell17" runat="server" >18892003612</asp:TableCell>
                        <asp:TableCell ID="TableCell18" runat="server" >18892003699</asp:TableCell>
                        <asp:TableCell ID="TableCell19" >15/09/2008</asp:TableCell>
                        <asp:TableCell ID="TableCell20" >15/09/2008</asp:TableCell>
                        <asp:TableCell ID="TableCell21" >&nbsp;</asp:TableCell>
                        <asp:TableCell ID="TableCell22" >22.348,52</asp:TableCell>
                        <asp:TableCell ID="TableCell23" >&nbsp;</asp:TableCell>
                    </asp:TableRow>              
                    <asp:TableRow ID="TableRow6" runat="server" CssClass="tbPainelFRM" Font-Size="Small">
                        <asp:TableCell ID="TableCell24" runat="server" >1001</asp:TableCell>
                        <asp:TableCell ID="TableCell25" runat="server" >BANCO DO BRASIL</asp:TableCell>
                        <asp:TableCell ID="TableCell26" runat="server" >18892003612</asp:TableCell>
                        <asp:TableCell ID="TableCell27" runat="server" >18892003699</asp:TableCell>
                        <asp:TableCell ID="TableCell28" >15/09/2008</asp:TableCell>
                        <asp:TableCell ID="TableCell29" >15/09/2008</asp:TableCell>
                        <asp:TableCell ID="TableCell30" >&nbsp;</asp:TableCell>
                        <asp:TableCell ID="TableCell31" >22.348,52</asp:TableCell>
                        <asp:TableCell ID="TableCell32" >&nbsp;</asp:TableCell>
                    </asp:TableRow> 
                    <asp:TableRow ID="TableRow7" runat="server" CssClass="tbPainelFRM" Font-Bold="True">
                        <asp:TableCell ID="TableCell33" runat="server" ColumnSpan="1">TOTAL</asp:TableCell>
                        <asp:TableCell ID="TableCell35" runat="server" ColumnSpan="6">&nbsp;</asp:TableCell>
                        <asp:TableCell ID="TableCell36" runat="server">192.168,00</asp:TableCell>
                        <asp:TableCell ID="TableCell34" runat="server">&nbsp;</asp:TableCell>
                    </asp:TableRow>
                     <asp:TableRow ID="TableRow3" runat="server" CssClass="tbPainelFRM" Font-Size="Small"  HorizontalAlign="Left">
                        <asp:TableCell ColumnSpan="10">Aceitas Banco</asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="TableRow1" runat="server" CssClass="tdBarraFerramentasSUM" Font-Size="Small">
                        <asp:TableCell ID="TableCell6" runat="server" >Banco</asp:TableCell>
                        <asp:TableCell ID="TableCell7" runat="server" >Nome</asp:TableCell>
                        <asp:TableCell ID="TableCell8" runat="server" >Conta Banco</asp:TableCell>
                        <asp:TableCell ID="TableCell9" runat="server" >Conta Transit.</asp:TableCell>
                        <asp:TableCell ID="TableCell10" >Dt Processamento</asp:TableCell>
                        <asp:TableCell ID="TableCell11" >Dt Operação</asp:TableCell>
                        <asp:TableCell ID="TableCell3" >Dt Credito</asp:TableCell>
                        <asp:TableCell ID="TableCell12" >Valor</asp:TableCell>
                        <asp:TableCell ID="TableCell14" >SubTotal Banco</asp:TableCell>                        
                    </asp:TableRow>
                    <asp:TableRow ID="TableRow8" runat="server" CssClass="tbPainelFRM" Font-Size="Small">
                        <asp:TableCell ID="TableCell37" runat="server" >1001</asp:TableCell>
                        <asp:TableCell ID="TableCell38" runat="server" >BANCO DO BRASIL</asp:TableCell>
                        <asp:TableCell ID="TableCell39" runat="server" >18892003612</asp:TableCell>
                        <asp:TableCell ID="TableCell40" runat="server" >18892003699</asp:TableCell>
                        <asp:TableCell ID="TableCell41" >15/09/2008</asp:TableCell>
                        <asp:TableCell ID="TableCell42" >15/09/2008</asp:TableCell>
                        <asp:TableCell ID="TableCell43" >15/09/2008;</asp:TableCell>
                        <asp:TableCell ID="TableCell44" >22.348,52</asp:TableCell>
                        <asp:TableCell ID="TableCell45" >22.348,52</asp:TableCell>
                    </asp:TableRow>              
                    <asp:TableRow ID="TableRow9" runat="server" CssClass="tbPainelFRM" Font-Size="Small">
                        <asp:TableCell ID="TableCell46" runat="server" >1001</asp:TableCell>
                        <asp:TableCell ID="TableCell47" runat="server" >BANCO DO BRASIL</asp:TableCell>
                        <asp:TableCell ID="TableCell48" runat="server" >18892003612</asp:TableCell>
                        <asp:TableCell ID="TableCell49" runat="server" >18892003699</asp:TableCell>
                        <asp:TableCell ID="TableCell50" >15/09/2008</asp:TableCell>
                        <asp:TableCell ID="TableCell51" >15/09/2008</asp:TableCell>
                        <asp:TableCell ID="TableCell52" >15/09/2008;</asp:TableCell>
                        <asp:TableCell ID="TableCell53" >22.348,52</asp:TableCell>
                        <asp:TableCell ID="TableCell54" >22.348,52</asp:TableCell>
                    </asp:TableRow>                                  
                    <asp:TableRow ID="TableRow10" runat="server" CssClass="tbPainelFRM" Font-Bold="True">
                        <asp:TableCell ID="TableCell55" runat="server" ColumnSpan="1">TOTAL</asp:TableCell>
                        <asp:TableCell ID="TableCell56" runat="server" ColumnSpan="6">&nbsp;</asp:TableCell>
                        <asp:TableCell ID="TableCell57" runat="server">192.168,00</asp:TableCell>
                        <asp:TableCell ID="TableCell58" runat="server">&nbsp;</asp:TableCell>
                    </asp:TableRow>
                    
                     <asp:TableRow ID="TableRow4" runat="server" CssClass="tbPainelFRM" Font-Size="Small"  HorizontalAlign="Left">
                        <asp:TableCell ColumnSpan="10">Total do Relatorio</asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="TableRow11" runat="server" CssClass="tbPainelFRM" Font-Bold="True" ForeColor="RED">
                        <asp:TableCell ID="TableCell59" runat="server" ColumnSpan="1">TOTAL</asp:TableCell>
                        <asp:TableCell ID="TableCell60" runat="server" ColumnSpan="6">&nbsp;</asp:TableCell>
                        <asp:TableCell ID="TableCell61" runat="server">192.168,00</asp:TableCell>
                        <asp:TableCell ID="TableCell62" runat="server">&nbsp;</asp:TableCell>
                    </asp:TableRow>                    
                </asp:Table>--%>
            </td>
        </tr>
        <tr>
            <td align="left" class="tdCampoFRM" colspan="6">
                <asp:Button ID="btnExportar" runat="server" OnClick="btnExportar_Click" Text="Exportar para excel"
                    Visible="False" /></td>
            </td>
        </tr>
    </table>
</asp:Content>
