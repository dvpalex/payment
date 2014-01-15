<%@ Control Language="C#" AutoEventWireup="true" CodeFile="boleto.ascx.cs" Inherits="Agents_BoletoBB_boleto" %>
<script type="text/javascript" language="javascript">
function OpenInternetBanking(obj)
{
    var url = "";
    switch(obj.value)
    {
    	case '25':
			url = "http://www.bancoalfa.com.br/compartilhado/frm_clientealfa.asp?sAcesso=login";
			break;
		case '62':
			url = "http://www.banco1.com.br";
			break;
		case '33':
			url = "http://www.santanderbanespa.com.br";
			break;
		case '702':
			url = "http://www.bancosantos.com.br";
			break;
		case '291':
			url = "http://www.bcn.com.br";
			break;
		case '237':
			url = "http://www.bradesco.com.br";
			break;
		case '351':
			url = "http://www.santander.com.br";
			break;
		case '479':
			url = "http://www.bankboston.com.br";
			break;
		case '744':
			url = "http://www.bankboston.com.br";
			break;
		case '1':
			url = "http://www.bancobrasil.com.br";
			break;
		case '104':
			url = "http://www.cef.gov.br";
			break;
		case '477':
			url = "http://www.citibank.com.br";
			break;
		case '745':
			url = "http://www.citibank.com.br";
			break;
		case '399':
			url = "http://www.hsbc.com.br";
			break;
		case '168':
			url = "http://www.hsbc.com.br";
			break;
		case '750':
			url = "http://www.hsbc.com.br";
			break;
		case '341':
			url = "http://www.itau.com.br";
			break;
		case '275':
			url = "http://www.real.com.br";
			break;
		case '356':
			url = "http://www.real.com.br";
			break;
		case '422':
			url = "http://www.safra.com.br";
			break;
		case '8':
			url = "http://www.santander.com.br";
			break;
		case '353':
			url = "http://www.santander.com.br";
			break;
		case '502':
			url = "http://www.santander.com.br";
			break;
		case '424':
			url = "http://www.santander.com.br";
			break;
		case '409':
			url = "http://www.unibanco.com.br";
			break;
		case '230':
			url = "https://wwws.bandeirantes.com.br";
			break;
		case '347':
			url = "http://www.sudameris.com.br";
			break;
    }
	window.open(url, "Boleto","toolbar=0,location=0,directories=0,status=1,menubar=1,scrollbars=1,resizable=1,screenX=0,screenY=0,left=0,top=0,width=700,height=800");
}
function printBoleto(url)
{
    window.open(url, "Boleto","toolbar=0,location=0,directories=0,status=1,menubar=1,scrollbars=1,resizable=1,screenX=0,screenY=0,left=0,top=0,width=700,height=800");
}    
</script>
<table cellspacing="4" cellpadding="0" width="100%" border="0">
	<tr>
		<td align="left" colspan="4" class="datatxt2" style="height: 19px"><b><asp:Label ID="label1" runat="server" meta:resourcekey="label1Resource1">PARA IMPRIMIR O BOLETO CLIQUE NO N&#218;MERO ABAIXO</asp:Label></b></td>
	</tr>
	<tr>
		<td align="left" colspan="4">
			<table id="Table5" cellspacing="1" cellpadding="0" width="100%" border="0">
				<tr>
					<td align="center" style="height: 12px" class="dataheader"><b><asp:Label ID="label2" runat="server" meta:resourcekey="label2Resource1">Parcela</asp:Label></b></td>
					<td align="center" style="height: 12px" class="dataheader"><b><asp:Label ID="label3" runat="server" meta:resourcekey="label3Resource1">N&#250;mero do Boleto</asp:Label></b></td>
					<td align="right" width="15%" style="height: 12px" class="dataheader"><b><asp:Label ID="label4" runat="server" meta:resourcekey="label4Resource1">Valor</asp:Label></b></td>
				</tr>
				
                <asp:Repeater ID="rptBoletoInfo" runat="server" >
                    <ItemTemplate>
                        <tr class="databg">
					        <td align="center"><%# ((SuperPag.InfoBoleto)Container.DataItem).InstallmentNumber.ToString() %></td>
					        <td align="center" style="cursor:pointer; white-space: nowrap; text-decoration:underline;" class="linkboleto" onclick="printBoleto('Agents/Boleto/showboleto.aspx?id=<%# ((SuperPag.InfoBoleto)Container.DataItem).PaymentAttemptId.ToString() %>');">
                                <asp:Image ID="imgImpressora" runat="server" ImageUrl="~/Images/impressora.gif" AlternateText="Clique aqui para imprimir este boleto" Width="20px" Height="18px" meta:resourcekey="imgImpressoraResource2" />
                                <%# ((SuperPag.InfoBoleto)Container.DataItem).Billing %>
					        </td>
					        <td align="right">
                                <%# ((SuperPag.InfoBoleto)Container.DataItem).InstallmentValue.ToString("C", SuperPag.Helper.GenericHelper.GetNumberFormatBrasil()) %>
                            </td>
				        </tr>
                    </ItemTemplate>
                    <AlternatingItemTemplate>
                        <tr class="databg2">
					        <td align="center"><%# ((SuperPag.InfoBoleto)Container.DataItem).InstallmentNumber.ToString() %></td>
					        <td align="center" style="cursor:pointer; white-space: nowrap; text-decoration:underline;" class="linkboleto" onclick="printBoleto('Agents/Boleto/showboleto.aspx?id=<%# ((SuperPag.InfoBoleto)Container.DataItem).PaymentAttemptId.ToString() %>');">
                                <asp:Image ID="imgImpressora" runat="server" ImageUrl="~/Images/impressora.gif" AlternateText="Clique aqui para imprimir este boleto" Width="20px" Height="18px" meta:resourcekey="imgImpressoraResource1" />
                                <%# ((SuperPag.InfoBoleto)Container.DataItem).Billing %>
					        </td>
					        <td align="right">
                                <%# ((SuperPag.InfoBoleto)Container.DataItem).InstallmentValue.ToString("C", SuperPag.Helper.GenericHelper.GetNumberFormatBrasil()) %>
                            </td>
				        </tr>
                    </AlternatingItemTemplate>
                </asp:Repeater>

			</table>
		</td>
	</tr>
	<tr>
		<td style="TEXT-ALIGN: justify" colspan="4" class="ajudatxt"><asp:Label ID="label5" runat="server" meta:resourcekey="label5Resource1">Para sua maior comodidade enviamos o(s) 
			link(s) do(s) boleto(s) para o email cadastrado no site.</asp:Label></td>
	</tr>
	<tr>
		<td colspan="4">&nbsp;</td>
	</tr>
	<tr>
		<td style="TEXT-ALIGN: justify" colspan="4" class="datatxt2"><b><asp:Label ID="label7" runat="server" meta:resourcekey="label7Resource1">Internet Banking:</asp:Label></b>
			<select style="WIDTH: 210px; HEIGHT: 18px" onchange="javascript:OpenInternetBanking(this);">
				<option value="ESCOLHA">&nbsp;</option>
				<!--<option value="62">&nbsp;Banco 1</option>-->
				<!--<option value="25">&nbsp;Banco Alfa</option>-->
				<!--<option value="230">&nbsp;Bandeirantes</option>-->
				<!--<option value="291">&nbsp;BCN</option>-->
				<option value="237">&nbsp;Bradesco</option>
				<!--<option value="351">&nbsp;Bozano / Meridional</option>-->
				<option value="479">&nbsp;Bank Boston</option>
				<option value="1">&nbsp;Banco do Brasil</option>
				<option value="104">&nbsp;Caixa Econ&#244;mica Federal</option>
				<option value="477">&nbsp;Citibank</option>
				<option value="399">&nbsp;HSBC</option>
				<option value="341">&nbsp;Ita&#250;</option>
				<option value="275">&nbsp;Real / ABN</option>
				<option value="422">&nbsp;Safra</option>
				<option value="33">&nbsp;Santander / Banespa</option>
				<!--<option value="8">&nbsp;Santander</option>-->
				<option value="347">&nbsp;Sudameris</option>
				<option value="409">&nbsp;Unibanco</option>
			</select>
			<br/>
			<asp:Label ID="label6" runat="server" meta:resourcekey="label6Resource1">Escolha o banco onde voc&#234; &#233; correntista para efetuar o pagamento do(s) boleto(s).</asp:Label>
		</td>
	</tr>
</table>