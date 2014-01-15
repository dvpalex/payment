<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeFile="transactionsearch.aspx.cs" Inherits="Views_transactionsearch" %>
<%@ Register TagPrefix="fwc" Namespace="SuperPag.Framework.Web.WebControls" Assembly="SuperPag.Framework" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Conteudo" Runat="Server">
    <div id="SubTitulo">
        <asp:Image ID="imgHome" runat="server" SkinID="imgEmployee" />
        <asp:Label ID="lblNomeUsuario" runat="server" Text="Label"></asp:Label>    
    </div>
	<br />
	<!--Painel de Sinalizacao-->
			<table class="tbPainelSNZ" cellpadding="0" cellspacing="0">
				<tr>
					<td class="tdPainelSNZ">
						<asp:label id="lblTituloSNZ" runat="server">Transações</asp:label>
					</td>
				</tr>
				<tr>
					<td class="tdSubTituloSNZ">
						<asp:label id="Label5" runat="server">Busca de transações</asp:label>
					</td>
				</tr>
			</table>
			<table id="tbCentral">
				<tr>
					<td id="tdCentral">
						<table class="tbPainelFRM">
							<tr>
								<td class="tdBarraFerramentasSUM" colspan="4">
									<asp:label id="Label2" runat="server">Pesquisar por período</asp:label>
								</td>
							</tr>
							<tr>
								<td class="tdLabelFRM" style="HEIGHT: 2px">
									<asp:label id="Label8" runat="server">Loja</asp:label></td>
								<td class="tdCampoFRM" align="left" style="HEIGHT: 2px">
									<fwc:MsgDropDownList id="Msgdropdownlist1" runat="server" MsgSource="MTransactionSearch" MsgListSourceType="Message"
										MsgListItemsSource="MCStore" MsgListItemTextField="Name" MsgListItemValueField="StoreId" MsgSourceField="StoreId"
										MsgSelectedValueField="StoreId" Width="256px" FirstBlank="False"></fwc:MsgDropDownList></td>
								<td class="tdLabelFRM" align="left" style="HEIGHT: 2px">
									<asp:label id="Label9" runat="server">No. do pedido</asp:label></td>
								<td class="tdCampoFRM" align="left" style="HEIGHT: 2px">
									<fwc:MsgTextBox id="Msgtextbox2" runat="server" MsgTextField="StoreReferenceOrder" MsgSource="MTransactionSearch"
										InputMandatory="False" Width="304px" MaxLength="50"></fwc:MsgTextBox></td>
							</tr>
							<tr>
								<td class="tdLabelFRM">
									<asp:label id="Label15" runat="server">Data de:</asp:label></td>
								<td class="tdCampoFRM" align="left">
									<fwc:MsgTextBox id="Msgtextbox7" runat="server" MsgTextField="OrderDateFrom" MsgSource="MTransactionSearch"
										MaxLength="50" Width="171px" InputMandatory="False" InputFilter="Data" CustomFormat="Data"></fwc:MsgTextBox>
									<fwc:MsgTextBox id="MsgTextBox1" runat="server" MsgSource="MTransactionSearch" MaxLength="50" Width="55px"
										InputMandatory="False" MsgTextField="OrderTimeFrom" CustomFormat="HorarioHM" InputFilter="HorarioHM"></fwc:MsgTextBox></td>
								<td class="tdLabelFRM" align="left">
									<asp:label id="Label16" runat="server">até</asp:label></td>
								<td class="tdCampoFRM" align="left">
									<fwc:MsgTextBox id="Msgtextbox8" runat="server" MsgTextField="OrderDateTo" MsgSource="MTransactionSearch"
										InputMandatory="False" Width="209px" MaxLength="50" InputFilter="Data" CustomFormat="Data"></fwc:MsgTextBox>
									<fwc:MsgTextBox id="MsgTextBox3" runat="server" MsgSource="MTransactionSearch" MaxLength="50" Width="60px"
										InputMandatory="False" MsgTextField="OrderTimeTo" CustomFormat="HorarioHM" InputFilter="HorarioHM"></fwc:MsgTextBox></td>
							</tr>
							<tr>
								<td class="tdLabelFRM">
									<asp:label id="Label11" runat="server">Nome do consumidor</asp:label></td>
								<td class="tdCampoFRM" align="left" colspan="3">
									<fwc:MsgTextBox id="Msgtextbox10" runat="server" MsgTextField="ConsumerName" MsgSource="MTransactionSearch"
										InputMandatory="False" Width="578px" MaxLength="250" InputFilter="None" CustomFormat="None"></fwc:MsgTextBox>
								</td>
							</tr>
							<tr>
								<td class="tdLabelFRM">
									<asp:label id="Label3" runat="server">CPF</asp:label></td>
								<td class="tdCampoFRM" align="left">
									<fwc:MsgTextBox id="MsgTextBox5" runat="server" MsgSource="MTransactionSearch" MaxLength="50" Width="160px"
										InputMandatory="False" MsgTextField="Cpf" CustomFormat="CPF" InputFilter="CPF"></fwc:MsgTextBox>
								</td>
								<td class="tdLabelFRM" align="left">
									<asp:label id="Label10" runat="server">ou CNPJ</asp:label></td>
								<td class="tdCampoFRM" align="left">
									<fwc:MsgTextBox id="MsgTextBox9" runat="server" MsgSource="MTransactionSearch" MaxLength="50" Width="160px"
										InputMandatory="False" MsgTextField="Cnpj" CustomFormat="CNPJ" InputFilter="CNPJ"></fwc:MsgTextBox>
								</td>
							</tr>
							<tr>
								<td class="tdLabelFRM">
									<asp:label id="Label6" runat="server">Status de cobrança</asp:label></td>
								<td class="tdCampoFRM" align="left">
									<fwc:MsgDropDownList id="MsgDropDownList2" runat="server" MsgSource="MTransactionSearch" MsgSelectedValueField="Status"
										MsgSourceField="Status" MsgListSourceType="Enumeration" MsgListItemsSource="PaymentAttemptStatus">
									</fwc:MsgDropDownList></td>
								<td class="tdLabelFRM">
									<asp:label id="Label12" runat="server">Status do pedido</asp:label></td>
								<td class="tdCampoFRM" align="left">
									<fwc:MsgDropDownList id="MsgDropDownList4" runat="server" MsgSource="MTransactionSearch" MsgSelectedValueField="OrderStatus"
										MsgSourceField="OrderStatus" MsgListSourceType="Enumeration" MsgListItemsSource="OrderStatus">
									</fwc:MsgDropDownList></td>
	
							</tr>
							<tr>
								<td class="tdLabelFRM">
									<asp:label id="Label7" runat="server">Meio de pagamento:</asp:label></td>
								<td class="tdCampoFRM" align="left" colspan="3">
									<fwc:MsgDropDownList id="MsgDropDownList3" runat="server" MsgSource="MTransactionSearch" MsgSelectedValueField="PaymentFormId"
										MsgSourceField="PaymentFormId" MsgListItemValueField="PaymentFormId" MsgListItemTextField="Name"
										MsgListItemsSource="MCPaymentForm" MsgListSourceType="Message">
										<asp:ListItem Value="Todas">Todas</asp:ListItem>
									</fwc:MsgDropDownList></td>
							</tr>
						</table>
						<table class="tbPainelFRM">
							<tr>
								<td class="tdBarraFerramentasSUM" colspan="4">
									<asp:label id="Label4" runat="server">Pesquisar por Cód. de Controle SuperPag</asp:label>
								</td>
							</tr>
							<tr>
								<td class="tdLabelFRM">
									<asp:label id="Label1" runat="server">Código de controle:</asp:label></td>
								<td class="tdCampoFRM" align="left" colspan="3">
									<fwc:MsgTextBox id="txtOrderId" runat="server" MsgTextField="PaymentAttemptId" MsgSource="MTransactionSearch" InputMandatory="False"
										Width="304px" MaxLength="50" CustomFormat="None" InputFilter="None"></fwc:MsgTextBox></td>
							</tr>
						</table>
						<table class="tbBarraDeAcoesBAC">
							<tr>
								<td class="tdBarraDeAcoesBAC">
									<table>
										<tr>
											<td class="tdBarraDeAcoesBAC">
												<fwc:eventbutton id="btnSearch" runat="server" EventName="Search" text="Pesquisar" CausesValidation="true" OnClick="btnSearch_Click"></fwc:eventbutton>
											</td>
										</tr>
									</table>
								</td>
							</tr>
						</table>
					</td>
				</tr>
			</table>
</asp:Content>