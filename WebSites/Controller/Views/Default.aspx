<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Views_Default" %>
<%@ Register TagPrefix="fwc" Namespace="SuperPag.Framework.Web.WebControls" Assembly="SuperPag.Framework" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Conteudo" Runat="Server">
    <div id="SubTitulo">
        <asp:Image ID="imgHome" runat="server" SkinID="imgEmployee" />
        <asp:Label ID="lblNomeUsuario" runat="server" Text="Label"></asp:Label>    
    </div>
	<br />
	<br />
    <!--Painel de Formulario-->
	<div>
      <table id="tblDados" cellspacing="0" cellpadding="0" style="width: 90%; text-align: center; border-width: 3px; border-color: cornflowerblue;">
		<tr>
		  <td style="vertical-align: middle">
		      <table width="95%" align="center" bgColor="lightsteelblue" border="0">
				<tr>
				    <td style="WIDTH: 167px" width="167"><font face="Tahoma" size="2"><asp:label id="lblPeriodo" runat="server">Período:</asp:label></font></td>
				    <td><font face="Tahoma" size="2">De</font></td>
				    <td><font face="Tahoma" size="2"><fwc:MsgTextBox id="txtDataInicial" runat="server" InputMandatory="True" MsgSource="MSummaryFilter" CustomFormat="DataDMY" InputFilter="DataDMY" MsgTextField="StartDate"></fwc:MsgTextBox></font></td>
				    <td><font face="Tahoma" size="2">Até</font></td>
				    <td><font face="Tahoma" size="2"><fwc:MsgTextBox id="MsgTextBox1" runat="server" InputMandatory="False" MsgSource="MSummaryFilter" CustomFormat="DataDMY" InputFilter="DataDMY" MsgTextField="FinishDate"></fwc:MsgTextBox></font></td>
				    <td><fwc:EventButton id="btnUpdate" runat="server" CausesValidation="true" text="Atualizar" EventName="Update" OnClick="btnUpdate_Click"></fwc:EventButton></td>
			    </tr>
	     	 </table>
 			 <br />
			 <table width="95%" align="center" bgColor="lightsteelblue" border="0">
				<tr>
					<td></td>
				</tr>
				<tr>
					<td class="summarioProduto" style="FONT-SIZE: x-small; FONT-FAMILY: verdana">
						<asp:datagrid id="grdSumario" runat="server" DESIGNTIMEDRAGDROP="102" ShowFooter="True" CssClass="summario"
							AutoGenerateColumns="False" OnItemDataBound="grdSumario_ItemDataBound" OnItemCommand="grdSumario_ItemCommand">
							<FooterStyle BackColor="LightBlue"></FooterStyle>
							<HeaderStyle HorizontalAlign="Right" CssClass="summarioHeader"></HeaderStyle>
							<Columns>
								<asp:BoundColumn DataField="ProductName">
									<HeaderStyle HorizontalAlign="Left" Width="300px"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right" CssClass="summarioProduto"></ItemStyle>
								</asp:BoundColumn>
								<asp:TemplateColumn>
									<ItemStyle HorizontalAlign="Right" CssClass="summarioDestacado"></ItemStyle>
									<ItemTemplate>
										<asp:LinkButton ID="LinkButton1" CommandArgument="1" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Qtde_Ok") %>'>
										</asp:LinkButton>
									</ItemTemplate>
									<FooterTemplate>
										<asp:LinkButton CommandArgument="1" runat="server" id="Linkbutton5"></asp:LinkButton>
									</FooterTemplate>
									<FooterStyle CssClass="footer"></FooterStyle>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="Valor_Ok" ReadOnly="True" DataFormatString="{0:N2}">
									<ItemStyle CssClass="summarioDestacadoValor"></ItemStyle>
									<FooterStyle CssClass="footer"></FooterStyle>
								</asp:BoundColumn>
								<asp:TemplateColumn>
									<ItemStyle HorizontalAlign="Right" CssClass="summarioDestacado2"></ItemStyle>
									<ItemTemplate>
										<asp:LinkButton ID="LinkButton2" CommandArgument="3" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Qtde_NaoOK") %>'>
										</asp:LinkButton>
									</ItemTemplate>
									<FooterTemplate>
										<asp:LinkButton CommandArgument="3" runat="server" id="Linkbutton4"></asp:LinkButton>
									</FooterTemplate>
									<FooterStyle CssClass="footer"></FooterStyle>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="Valor_NaoOK" ReadOnly="True" DataFormatString="{0:N2}">
									<ItemStyle CssClass="summarioDestacadoValor2"></ItemStyle>
									<FooterStyle CssClass="footer"></FooterStyle>
								</asp:BoundColumn>
								<asp:TemplateColumn>
									<ItemStyle HorizontalAlign="Right" CssClass="summarioDestacado"></ItemStyle>
									<ItemTemplate>
										<asp:LinkButton ID="LinkButton3" CommandArgument="5" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Qtde_PayPend") %>'>
										</asp:LinkButton>
									</ItemTemplate>
									<FooterTemplate>
										<asp:LinkButton CommandArgument="5" runat="server" id="Linkbutton3"></asp:LinkButton>
									</FooterTemplate>
									<FooterStyle CssClass="footer"></FooterStyle>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="Valor_PayPend" ReadOnly="True" DataFormatString="{0:N2}">
									<ItemStyle CssClass="summarioDestacadoValor"></ItemStyle>
									<FooterStyle CssClass="footer"></FooterStyle>
								</asp:BoundColumn>
								<asp:TemplateColumn>
									<ItemStyle HorizontalAlign="Right" CssClass="summarioDestacado2"></ItemStyle>
									<ItemTemplate>
										<asp:LinkButton ID="LinkButton6" CommandArgument="7" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Qtde_Cancel") %>'>
										</asp:LinkButton>
									</ItemTemplate>
									<FooterTemplate>
										<asp:LinkButton CommandArgument="7" runat="server" id="Linkbutton2"></asp:LinkButton>
									</FooterTemplate>
									<FooterStyle CssClass="footer"></FooterStyle>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="Valor_Cancel" ReadOnly="True" DataFormatString="{0:N2}">
									<ItemStyle CssClass="summarioDestacadoValor2"></ItemStyle>
									<FooterStyle CssClass="footer"></FooterStyle>
								</asp:BoundColumn>
								<asp:TemplateColumn>
									<ItemStyle HorizontalAlign="Right" CssClass="summarioDestacado"></ItemStyle>
									<ItemTemplate>
										<asp:LinkButton ID="LinkButton7" CommandArgument="9" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Qtde_Pend") %>'>
										</asp:LinkButton>
									</ItemTemplate>
									<FooterTemplate>
										<asp:LinkButton CommandArgument="9" runat="server" id="Linkbutton1"></asp:LinkButton>
									</FooterTemplate>
									<FooterStyle CssClass="footer"></FooterStyle>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="Valor_Pend" ReadOnly="True" DataFormatString="{0:N2}">
									<ItemStyle CssClass="summarioDestacadoValor"></ItemStyle>
									<FooterStyle CssClass="footer"></FooterStyle>
								</asp:BoundColumn>
							</Columns>
						</asp:datagrid>
					</td>
				</tr>
			</table>
			<br/>
          </td>
        </tr>
      </table>
   </div>
</asp:Content>