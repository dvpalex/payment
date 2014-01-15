<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeFile="navigationfilter.aspx.cs" Inherits="Views_navigationfilter" %>
<%@ Register TagPrefix="fwc" Namespace="SuperPag.Framework.Web.WebControls" Assembly="SuperPag.Framework" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Conteudo" Runat="Server">
<div id="SubTitulo">
        <asp:Image ID="imgHome" runat="server" SkinID="imgEmployee" />
        <asp:Label ID="lblNomeUsuario" runat="server" Text="Label"></asp:Label>    
    </div>
	<br />
	<!--Painel de Sinalizacao-->
			<table id="tbCentral">
				<tr>
					<td id="tdCentral">
						<table class="tbPainelFRM">
							<TR>
								<TD class="tdBarraFerramentasSUM" colspan="4">
									<asp:label id="Label2" runat="server">Pesquisar por período</asp:label>
								</TD>
							</TR>
                            <tr>
                                <td class="tdLabelFRM">
                                    Código do Cliente:<br />
                                    (CPF ou CNPJ)</td>
                                <td align="left" class="tdCampoFRM">
                                    <fwc:MsgTextBox ID="txtCodCliente" runat="server" CustomFormat="Texto" InputFilter="Texto"
                                        InputMandatory="True" MaxLength="50" MsgSource="MNavigationSearch" MsgTextField="CPF"
                                        Width="171px"></fwc:MsgTextBox></td>
                                <td align="left" class="tdLabelFRM">
                                </td>
                                <td align="left" class="tdCampoFRM">
                                </td>
                            </tr>
							<TR>
								<TD class="tdLabelFRM">
									<asp:label id="Label15" runat="server">Data pedido de:</asp:label></TD>
								<TD class="tdCampoFRM" align="left">
                                    &nbsp;<fwc:MsgTextBox ID="txtStartDate" runat="server" CustomFormat="Data" InputFilter="Data"
                                        InputMandatory="True" MaxLength="50" MsgSource="MNavigationSearch" MsgTextField="InitialDate"
                                        Width="171px"></fwc:MsgTextBox></TD>
								<TD class="tdLabelFRM" align="left">
									<asp:label id="Label16" runat="server">até</asp:label></TD>
								<TD class="tdCampoFRM" align="left">
                                    &nbsp;<fwc:MsgTextBox ID="txtEndDate" runat="server" CustomFormat="Data" InputFilter="Data"
                                        InputMandatory="True" MaxLength="50" MsgSource="MNavigationSearch" MsgTextField="FinalDate"
                                        Width="171px"></fwc:MsgTextBox></TD>
							</TR>
						</table>
						<table class="tbBarraDeAcoesBAC">
							<tr>
								<td class="tdBarraDeAcoesBAC" style="height: 33px">
									<table>
										<tr>
											<td class="tdBarraDeAcoesBAC" style="height: 17px">
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

