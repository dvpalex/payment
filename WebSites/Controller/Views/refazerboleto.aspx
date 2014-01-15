<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeFile="refazerboleto.aspx.cs" Inherits="Views_refazerboleto" %>
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
				<asp:label id="lblTituloSNZ" runat="server">Emissão de nova via do boleto</asp:label>
			</td>
		</tr>
	</table>
	<table id="tbCentral">
		<tr>
			<td id="tdCentral">
				<table class="tbPainelFRM">
					<tr>
						<td class="tdBarraFerramentasSUM" colspan="4">
							<asp:label id="Label2" runat="server">Dados do consumidor</asp:label>
						</td>
					</tr>
					<tr>
						<td class="tdLabelFRM" style="HEIGHT: 2px">
							<asp:label id="Label8" runat="server">Nome do consumidor</asp:label>
					    </td>
						<td class="tdCampoFRM" align="left" style="HEIGHT: 2px">
                        	<fwc:MsgLabel id="Msgtextbox1" runat="server" MsgTextField="Order.Consumer.Name" MsgSource="MPaymentAttempt"
								Width="304px" ></fwc:MsgLabel>
                        </td>
						<td class="tdLabelFRM" align="left" style="HEIGHT: 2px">
							<asp:label id="Label9" runat="server">Data Vencimento</asp:label></td>
						<td class="tdCampoFRM" align="left" style="HEIGHT: 2px">
							<fwc:MsgTextBox id="Msgtextbox2" runat="server" MsgTextField="ExpirationPaymentDate" MsgSource="MPaymentAttemptBoleto"
								CustomFormat="DataDMY" InputFilter="Data" InputMandatory="True" Width="209px"></fwc:MsgTextBox>
						</td>
					</tr>
					<tr>
						<td class="tdLabelFRM" style="HEIGHT: 2px">
							<asp:label id="Label3" runat="server">Valor do Boleto</asp:label>
					    </td>
						<td class="tdCampoFRM" align="left" style="HEIGHT: 2px" colspan="3">
                        	<fwc:MsgTextBox id="Msgtextbox4" runat="server" MsgTextField="Price" MsgSource="MPaymentAttempt"
								CustomFormat="NumDecimal_2casa" InputFilter="NumDecimal_2casa" InputMandatory="True" Width="210px"></fwc:MsgTextBox>
                        </td>
					</tr>
					<tr>
						<td class="tdLabelFRM" style="HEIGHT: 2px">
							<asp:label id="Label1" runat="server">Instruções do Boleto</asp:label>
					    </td>
						<td class="tdCampoFRM" align="left" style="HEIGHT: 2px" colspan="3">
                        	<fwc:MsgTextBox id="Msgtextbox3" runat="server" MsgTextField="Instructions" MsgSource="MPaymentAttemptBoleto"
								InputMandatory="True" Width="700px" MaxLength="1000"></fwc:MsgTextBox>
                        </td>
					</tr>
				</table>
				<table class="tbBarraDeAcoesBAC">
					<tr>
						<td class="tdBarraDeAcoesBAC">
							<table>
								<tr>
									<td class="tdBarraDeAcoesBAC" style="height: 17px">
										<fwc:eventbutton id="btnEnvio" runat="server" EventName="Reenviar" text="Emitir" CausesValidation="true" OnClick="btnReenviar_Click"></fwc:eventbutton>
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