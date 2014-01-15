<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeFile="reenviarboleto.aspx.cs" Inherits="Views_reenviarboleto" %>
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
				<asp:label id="lblTituloSNZ" runat="server">Reenviar boleto ao consumidor via e-mail</asp:label>
			</td>
		</tr>
		<tr>
			<td class="tdPainelError">
			    <fwc:MsgLabel id="MsgError" runat="server" Width="304px" DataTypeBind="Value" BindValueKey="SendError"></fwc:MsgLabel>
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
							<asp:label id="Label9" runat="server">E-mail do consumidor(sacado)</asp:label></td>
						<td class="tdCampoFRM" align="left" style="HEIGHT: 2px">
							<fwc:MsgTextBox id="Msgtextbox2" runat="server" MsgTextField="Order.Consumer.Email" MsgSource="MPaymentAttempt"
								InputMandatory="True" Width="304px" MaxLength="50"></fwc:MsgTextBox>
						</td>
					</tr>
				</table>
				<table class="tbBarraDeAcoesBAC">
					<tr>
						<td class="tdBarraDeAcoesBAC">
							<table>
								<tr>
									<td class="tdBarraDeAcoesBAC">
										<fwc:eventbutton id="btnEnvio" runat="server" EventName="Reenviar" text="Reenviar" CausesValidation="true" OnClick="btnReenviar_Click"></fwc:eventbutton>
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