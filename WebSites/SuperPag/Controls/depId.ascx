<%@ Control Language="C#" AutoEventWireup="true" CodeFile="depId.ascx.cs" Inherits="Agents_DepId_depId" %>
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
<table cellspacing="4" cellpadding="0" width="100%" border="0" style="border: solid 1px">
	<tr align="left" colspan="4">
	    <b>Dados para o depósito</b>
	</tr>
	<tr>
		<td align="left" colspan="4">
		
			<table id="Table1" cellspacing="1" cellpadding="0" width="100%" border="0">
				<tr>
					<td align="center" style="height: 12px" class="dataheader"><b><asp:Label ID="label1" runat="server">Banco</asp:Label></b></td>
					<td align="center" style="height: 12px" class="dataheader"><b><asp:Label ID="label8" runat="server">Agência</asp:Label></b></td>
					<td align="right" width="15%" style="height: 12px" class="dataheader"><b><asp:Label ID="label9" runat="server">Conta</asp:Label></b></td>
				</tr>
                <tr class="databg">
			        <td align="center" style="padding: 4px 4px 4px 4px;text-decoration:underline;"><big><b><asp:Label ID="lblBanco" runat="server"/></b></big></td>
			        <td align="center" style="padding: 4px 4px 4px 4px;text-decoration:underline;"><big><b><asp:Label ID="lblAgencia" runat="server"/></b></big></td>
			        <td align="right" style="padding: 4px 4px 4px 4px;text-decoration:underline;"><big><b><asp:Label ID="lblConta" runat="server"/></b></big></td>
		        </tr>
			</table>
			
			<br />		
		
			<table id="Table5" cellspacing="1" cellpadding="0" width="100%" border="0">
				<tr>
					<td align="center" style="height: 12px" class="dataheader"><b><asp:Label ID="label2" runat="server">Parcela</asp:Label></b></td>
					<td align="center" style="height: 12px" class="dataheader"><b><asp:Label ID="label3" runat="server">N&#250;mero de Identificação para o Depósito</asp:Label></b></td>
					<td align="right" width="15%" style="height: 12px" class="dataheader"><b><asp:Label ID="label4" runat="server">Valor</asp:Label></b></td>
				</tr>
				
                <asp:Repeater ID="rptDepIdInfo" runat="server" >
                    <ItemTemplate>
                        <tr class="databg">
					        <td align="center"><%# ((SuperPag.InfoDepId)Container.DataItem).InstallmentNumber.ToString()%></td>
					        <td align="center" style="white-space: nowrap; text-decoration:underline; padding: 4px 4px 4px 4px;" class="linkboleto">
                                <big><b><%# ((SuperPag.InfoDepId)Container.DataItem).IdNumber %></b></big>
					        </td>
					        <td align="right">
                                <%# ((SuperPag.InfoDepId)Container.DataItem).InstallmentValue.ToString("C", SuperPag.Helper.GenericHelper.GetNumberFormatBrasil())%>
                            </td>
				        </tr>
                    </ItemTemplate>
                    <AlternatingItemTemplate>
                        <tr class="databg2">
					        <td align="center"><%# ((SuperPag.InfoDepId)Container.DataItem).InstallmentNumber.ToString()%></td>
					        <td align="center" style="white-space: nowrap; text-decoration:underline; padding: 4px 4px 4px 4px;" class="linkboleto">
                                <big><b><%# ((SuperPag.InfoDepId)Container.DataItem).IdNumber %></b></big>
					        </td>
					        <td align="right">
                                <%# ((SuperPag.InfoDepId)Container.DataItem).InstallmentValue.ToString("C", SuperPag.Helper.GenericHelper.GetNumberFormatBrasil()) %>
                            </td>
				        </tr>
                    </AlternatingItemTemplate>
                </asp:Repeater>

			</table>
		</td>
	</tr>
	<tr>
		<td style="TEXT-ALIGN: justify" colspan="4" class="ajudatxt"><asp:Label ID="label5" runat="server" meta:resourcekey="label5Resource1">Para sua maior comodidade enviamos a(s) 
			identificação(ões) para o(s) depósito(s) para o email cadastrado no site.</asp:Label></td>
	</tr>
</table>
<br />
<table cellspacing="4" cellpadding="0" width="100%" border="0">
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
				<!--<option value="479">&nbsp;Bank Boston</option>-->
				<option value="1">&nbsp;Banco do Brasil</option>
				<option value="104">&nbsp;Caixa Econ&#244;mica Federal</option>
				<option value="477">&nbsp;Citibank</option>
				<option value="399">&nbsp;HSBC</option>
				<option value="341">&nbsp;Ita&#250;</option>
				<option value="275">&nbsp;Real / ABN</option>
				<option value="422">&nbsp;Safra</option>
				<!--<option value="8">&nbsp;Santander</option>-->
				<option value="33">&nbsp;Santander / Banespa</option>
				<option value="347">&nbsp;Sudameris</option>
				<option value="409">&nbsp;Unibanco</option>
			</select>
			<br/>
			<asp:Label ID="label6" runat="server" meta:resourcekey="label6Resource1">Escolha o banco onde voc&#234; &#233; correntista para efetuar a(s) transferência(s).</asp:Label>
		</td>
	</tr>
</table>