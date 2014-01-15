<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeFile="transactiondetail.aspx.cs" Inherits="Views_transactiondetail" %>
<%@ Register TagPrefix="fwc" Namespace="SuperPag.Framework.Web.WebControls" Assembly="SuperPag.Framework" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Conteudo" Runat="Server">
    <div id="SubTitulo">
        <asp:Image ID="imgHome" runat="server" SkinID="imgEmployee" />
        <asp:Label ID="lblNomeUsuario" runat="server" Text="Label"></asp:Label>    
    </div>
	<br />
	<!--Painel de Sumario-->
	<table class="tbBarraDeAcoesBAC">
		<tr>
			<td class="tdBarraDeAcoesBAC"><fwc:EventButton id="btnGoBack" runat="server" EventName="GoBack" text="Voltar" CausesValidation="false" OnClick="btnGoBack_Click"></fwc:EventButton></td>
		</tr>
	</table>

	<table class="tbBarraFerramentasSUM">
		<tr>
			<td class="tdBarraFerramentasSUM"><asp:Label id="lblTituloSumario_1" runat="server">Informa��es do Pedido</asp:Label></td>
		</tr>
	</table>
	<table class="tbPainelSUM">
		<tr>
			<td class="tdLabelSUM"><asp:Label id="Label2" runat="server">C�digo Pedido Loja</asp:Label></td>
			<td class="tdValorSUM" colspan="2"><fwc:MsgLabel id="Msglabel17" runat="server" MsgSource="MPaymentAttempt" MsgTextField="Order.StoreReferenceOrder"></fwc:MsgLabel></td>
		</tr>
		<tr>
			<td class="tdLabelSUM"><asp:Label id="Label5" runat="server">Data pedido</asp:Label></td>
			<td class="tdValorSUM" colspan="2"><fwc:MsgLabel id="Msglabel5" runat="server" MsgSource="MPaymentAttempt" MsgTextField="StartTime"></fwc:MsgLabel></td>
		</tr>
		<tr>
			<td class="tdLabelSUM">
                <asp:Label ID="Label1" runat="server">Qtde parcelas</asp:Label></td>
			<td class="tdValorSUM" colspan="2"><fwc:MsgLabel id="Msglabel9" runat="server" MsgSource="MPaymentAttempt" MsgTextField="Order.InstallmentQuantity"></fwc:MsgLabel></td>
		</tr>
		<tr>
			<td class="tdLabelSUM"><asp:Label id="Label14" runat="server">Valor total</asp:Label></td>
			<td class="tdValorSUM" colspan="2"><fwc:MsgLabel id="Msglabel13" runat="server" CustomFormat="Valor" MsgSource="MPaymentAttempt" MsgTextField="Order.TotalAmount"></fwc:MsgLabel></td>
		</tr>
		<tr>
			<td class="tdLabelSUM"><asp:Label id="Label3" runat="server">Valor final</asp:Label></td>
			<td class="tdValorSUM" colspan="2"><fwc:MsgLabel id="Msglabel1" runat="server" CustomFormat="Valor" MsgSource="MPaymentAttempt" MsgTextField="Order.FinalAmount"></fwc:MsgLabel></td>
		</tr>
		<tr>
			<td class="tdLabelSUM"><asp:Label id="Label7" runat="server">Status</asp:Label></td>
			<td class="tdValorSUM" style="border-right-style: none; width: 89px;"><fwc:MsgLabel id="Msglabel2" runat="server" MsgSource="MPaymentAttempt" MsgTextField="Order.Status"></fwc:MsgLabel></td>
            <td class="tdValorSUM" style="border-left-style: none"><asp:Label id="Label58" runat="server" Visible="False">Alterar para:</asp:Label>
                &nbsp; &nbsp;&nbsp; <fwc:MsgDropDownList id="Msgdropdownlist1" name="StatusPedido" runat="server" MsgListItemsSource="OrderStatusForCombo" MsgSource="MPaymentAttempt" MsgSourceField="Order.Status" Visible="False" InputMandatory="True" FirstBlank="False" Width="77px" MsgSelectedValueField="Order.Status"></fwc:MsgDropDownList>
                &nbsp; &nbsp;
                <fwc:EventButton ID="btnAlterar" runat="server" EventName="Alterar"
                    OnClick="btnAlterar_Click" Text="Aterar" Width="72px" Visible="False" />
                <asp:Label ID="Label48" runat="server" Visible="False">Usu�rio:</asp:Label>
                <fwc:MsgLabel ID="MsgLabel47" runat="server" MsgSource="MPaymentAttempt" MsgTextField="Order.StatusChangeUserName"
                    Visible="False"></fwc:MsgLabel>&nbsp;&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                &nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;<asp:Label ID="Label49" runat="server" Visible="False">Data:</asp:Label>
                <fwc:MsgLabel ID="MsgLabel48" runat="server" MsgSource="MPaymentAttempt" MsgTextField="Order.StatusChangeDate"
                    Visible="False"></fwc:MsgLabel></td>
		</tr>
        <tr id="Motivo" runat="server" style="display: block;">
            <td class="tdValorSUM" style="border-right-style: none"></td>
            <td class="tdValorSUM" style="border-right-style: none; border-left-style: none; width: 89px;"></td>
            <td class="tdValorSUM" style="border-left-style: none"><asp:Label id="Label59" runat="server" Visible="False">Motivo do Cancelamento</asp:Label><br />
                <fwc:MsgTextBox ID="TextBox1" runat="server" Height="56px" TextMode="MultiLine" Visible="False"
                    Width="223px" InputMandatory="False" MsgSource="MPaymentAttempt" MsgTextField="Order.CancelDescription"></fwc:MsgTextBox></td>
        </tr>
        <tr>
            <td class="tdValorSUM" style="border-bottom-style: solid; border-right-style: none;"></td>
            <td class="tdValorSUM" style="border-bottom-style: solid; border-left-style: none; width: 89px;"></td>
            <td class="tdValorSUM"></td>
        </tr>
	</table>
	<!--Painel de Registros de Funcionarios-->
	<table class="tbBarraFerramentasSUM">
		<tr>
			<td class="tdBarraFerramentasSUM"><asp:Label id="Label9" runat="server">Itens Pedido</asp:Label></td>
		</tr>
	</table>
    <fwc:MsgDataGrid id="grdActionPlane" ShowFooter="False" runat="server" MsgSource="MPaymentAttempt" MsgSourceField="Order.Itens">
		<Columns>
			<fwc:MessageColumn CustomFormat="None" MaxLength="0" id="emcItemType" Field="ItemType" CustomTotalFormat="None" Totalizar="False" HeaderText="Tipo" HelpAction=""></fwc:MessageColumn>
			<fwc:MessageColumn CustomFormat="None" MaxLength="0" id="emcItemNumber" Field="ItemNumber" CustomTotalFormat="None" Totalizar="False" HeaderText="N�mero" HelpAction=""></fwc:MessageColumn>
			<fwc:MessageColumn CustomFormat="None" MaxLength="0" id="emcItemCode" Field="ItemCode" CustomTotalFormat="None" Totalizar="False" HeaderText="C�digo" HelpAction=""></fwc:MessageColumn>
			<fwc:MessageColumn CustomFormat="None" MaxLength="0" id="emcItemDescription" Field="ItemDescription" CustomTotalFormat="None" Totalizar="False" HeaderText="Descri��o" HelpAction=""></fwc:MessageColumn>
			<fwc:MessageColumn CustomFormat="None" MaxLength="0" id="emcItemQuantity" Field="ItemQuantity" CustomTotalFormat="None" Totalizar="False" HeaderText="Quantidade" HelpAction=""></fwc:MessageColumn>
			<fwc:MessageColumn CustomFormat="None" MaxLength="0" id="rmcItemValue" Field="ItemValue" CustomTotalFormat="None" Totalizar="False" HeaderText="Valor unit�rio" HelpAction=""></fwc:MessageColumn>
		</Columns>
    </fwc:MsgDataGrid>
    <br />
	<!--Painel de Sumario-->
	<asp:Panel ID="pnlConsumer" runat="server">
	    <table class="tbBarraFerramentasSUM">
		    <tr>
			    <td class="tdBarraFerramentasSUM"><asp:Label id="Label6" runat="server">Informa��es do Consumidor</asp:Label></td>
		    </tr>
	    </table>
        <asp:Panel ID="pnlPessoaFisica" runat="server" Width="100%">
 	      <table class="tbPainelSUM">
		    <tr>
			    <td class="tdLabelSUM"><asp:Label id="Label13" runat="server">Nome</asp:Label></td>
			    <td class="tdValorSUM" colspan="3"><fwc:MsgLabel id="Msglabel10" runat="server" MsgSource="MPaymentAttempt" MsgTextField="Order.Consumer.Name"></fwc:MsgLabel></td>
		    </tr>
		    <tr>
			    <td class="tdLabelSUM"><asp:Label id="Label39" runat="server">E-mail</asp:Label></td>
			    <td class="tdValorSUM" colspan="3"><fwc:MsgLabel id="Msglabel38" runat="server" MsgSource="MPaymentAttempt" MsgTextField="Order.Consumer.Email"></fwc:MsgLabel></td>
		    </tr>
		    <tr>
			    <td class="tdLabelSUM"><asp:Label id="Label8" runat="server">CPF</asp:Label></td>
			    <td class="tdValorSUM"><fwc:MsgLabel id="Msglabel6" runat="server" MsgSource="MPaymentAttempt" MsgTextField="Order.Consumer.CPF" CustomFormat="CPF"></fwc:MsgLabel>&nbsp;
                    (<fwc:MsgLabel ID="MsgLabel53" runat="server" CustomFormat="Texto" Font-Size="X-Small"
                        MsgSource="MPaymentAttempt" MsgTextField="Order.Consumer.CPF"></fwc:MsgLabel>)</td>
			    <td class="tdLabelSUM"><asp:Label id="Label32" runat="server">RG</asp:Label></td>
			    <td class="tdValorSUM"><fwc:MsgLabel id="Msglabel31" runat="server" MsgSource="MPaymentAttempt" MsgTextField="Order.Consumer.RG" CustomFormat="None"></fwc:MsgLabel></td>
		    </tr>
		    <tr>
			    <td class="tdLabelSUM"><asp:Label id="Label33" runat="server">Data Nascimento</asp:Label></td>
			    <td class="tdValorSUM"><fwc:MsgLabel id="Msglabel32" runat="server" MsgSource="MPaymentAttempt" MsgTextField="Order.Consumer.BirthDate" CustomFormat="DataDMY"></fwc:MsgLabel></td>
			    <td class="tdLabelSUM"><asp:Label id="Label34" runat="server">Sexo</asp:Label></td>
			    <td class="tdValorSUM"><fwc:MsgLabel id="Msglabel33" runat="server" MsgSource="MPaymentAttempt" MsgTextField="Order.Consumer.Ger"></fwc:MsgLabel></td>
		    </tr>
		    <tr>
			    <td class="tdLabelSUM"><asp:Label id="Label35" runat="server">Estado C�vil</asp:Label></td>
			    <td class="tdValorSUM"><fwc:MsgLabel id="Msglabel34" runat="server" MsgSource="MPaymentAttempt" MsgTextField="Order.Consumer.CivilState" CustomFormat="None"></fwc:MsgLabel></td>
			    <td class="tdLabelSUM"><asp:Label id="Label36" runat="server">Ocupa��o</asp:Label></td>
			    <td class="tdValorSUM"><fwc:MsgLabel id="Msglabel35" runat="server" MsgSource="MPaymentAttempt" MsgTextField="Order.Consumer.Occupation" CustomFormat="None"></fwc:MsgLabel></td>
		    </tr>
		    <tr>
			    <td class="tdLabelSUM"><asp:Label id="Label37" runat="server">Telefone</asp:Label></td>
			    <td class="tdValorSUM"><fwc:MsgLabel id="Msglabel36" runat="server" MsgSource="MPaymentAttempt" MsgTextField="Order.Consumer.Phone" CustomFormat="None"></fwc:MsgLabel></td>
			    <td class="tdLabelSUM"><asp:Label id="Label38" runat="server">Celular</asp:Label></td>
			    <td class="tdValorSUM"><fwc:MsgLabel id="Msglabel37" runat="server" MsgSource="MPaymentAttempt" MsgTextField="Order.Consumer.CelularPhone" CustomFormat="None"></fwc:MsgLabel></td>
		    </tr>
		    <tr>
			    <td class="tdLabelSUM"><asp:Label id="Label40" runat="server">Telefone Comercial</asp:Label></td>
			    <td class="tdValorSUM"><fwc:MsgLabel id="Msglabel39" runat="server" MsgSource="MPaymentAttempt" MsgTextField="Order.Consumer.CommercialPhone" CustomFormat="None"></fwc:MsgLabel></td>
			    <td class="tdLabelSUM"><asp:Label id="Label41" runat="server">Fax</asp:Label></td>
			    <td class="tdValorSUM"><fwc:MsgLabel id="Msglabel40" runat="server" MsgSource="MPaymentAttempt" MsgTextField="Order.Consumer.Fax" CustomFormat="None"></fwc:MsgLabel></td>
		    </tr>
	      </table>
        </asp:Panel>
        <asp:Panel ID="pnlPessoaJuridica" runat="server" Width="100%">
 	      <table class="tbPainelSUM">
		    <tr>
			    <td class="tdLabelSUM"><asp:Label id="Label42" runat="server">Empresa</asp:Label></td>
			    <td class="tdValorSUM" colspan="3"><fwc:MsgLabel id="Msglabel41" runat="server" MsgSource="MPaymentAttempt" MsgTextField="Order.Consumer.Name"></fwc:MsgLabel></td>
		    </tr>
		    <tr>
			    <td class="tdLabelSUM"><asp:Label id="Label43" runat="server">E-mail</asp:Label></td>
			    <td class="tdValorSUM" colspan="3"><fwc:MsgLabel id="Msglabel42" runat="server" MsgSource="MPaymentAttempt" MsgTextField="Order.Consumer.Email"></fwc:MsgLabel></td>
		    </tr>
		    <tr>
			    <td class="tdLabelSUM"><asp:Label id="Label44" runat="server">CNPJ</asp:Label></td>
			    <td class="tdValorSUM"><fwc:MsgLabel id="Msglabel43" runat="server" MsgSource="MPaymentAttempt" MsgTextField="Order.Consumer.CNPJ" CustomFormat="CNPJ"></fwc:MsgLabel></td>
			    <td class="tdLabelSUM"><asp:Label id="Label45" runat="server">IE</asp:Label></td>
			    <td class="tdValorSUM"><fwc:MsgLabel id="Msglabel44" runat="server" MsgSource="MPaymentAttempt" MsgTextField="Order.Consumer.IE" CustomFormat="None"></fwc:MsgLabel></td>
		    </tr>
		    <tr>
			    <td class="tdLabelSUM"><asp:Label id="Label52" runat="server">Telefone Comercial</asp:Label></td>
			    <td class="tdValorSUM"><fwc:MsgLabel id="Msglabel51" runat="server" MsgSource="MPaymentAttempt" MsgTextField="Order.Consumer.CommercialPhone" CustomFormat="None"></fwc:MsgLabel></td>
			    <td class="tdLabelSUM"><asp:Label id="Label53" runat="server">Fax</asp:Label></td>
			    <td class="tdValorSUM"><fwc:MsgLabel id="Msglabel52" runat="server" MsgSource="MPaymentAttempt" MsgTextField="Order.Consumer.Fax" CustomFormat="None"></fwc:MsgLabel></td>
		    </tr>
		    <tr>
			    <td class="tdLabelSUM"><asp:Label id="Label46" runat="server">Nome respons�vel</asp:Label></td>
			    <td class="tdValorSUM"><fwc:MsgLabel id="Msglabel45" runat="server" MsgSource="MPaymentAttempt" MsgTextField="Order.Consumer.ResponsibleName" CustomFormat="None"></fwc:MsgLabel></td>
			    <td class="tdLabelSUM"><asp:Label id="Label47" runat="server">CPF respons�vel</asp:Label></td>
			    <td class="tdValorSUM"><fwc:MsgLabel id="Msglabel46" runat="server" MsgSource="MPaymentAttempt" MsgTextField="Order.Consumer.ResponsibleCPF" CustomFormat="CPF"></fwc:MsgLabel></td>
		    </tr>
		    <tr>
			    <td class="tdLabelSUM"><asp:Label id="Label50" runat="server">Telefone</asp:Label></td>
			    <td class="tdValorSUM"><fwc:MsgLabel id="Msglabel49" runat="server" MsgSource="MPaymentAttempt" MsgTextField="Order.Consumer.Phone" CustomFormat="None"></fwc:MsgLabel></td>
			    <td class="tdLabelSUM"><asp:Label id="Label51" runat="server">Celular</asp:Label></td>
			    <td class="tdValorSUM"><fwc:MsgLabel id="Msglabel50" runat="server" MsgSource="MPaymentAttempt" MsgTextField="Order.Consumer.CelularPhone" CustomFormat="None"></fwc:MsgLabel></td>
		    </tr>
	      </table>
        </asp:Panel>
        <asp:Panel ID="pnlBillingAddress" runat="server" Visible="false">
	        <table class="tbBarraFerramentasSUM">
		        <tr>
			        <td class="tdBarraFerramentasSUM"><asp:Label id="Label10" runat="server">Endere�o de cobran�a</asp:Label></td>
		        </tr>
	        </table>
            <table class="tbPainelSUM">
		        <tr>
			        <td class="tdLabelSUM"><asp:Label id="Label15" runat="server">Endere�o</asp:Label></td>
			        <td class="tdValorSUM" colspan="3">
			           <fwc:MsgLabel id="Msglabel3" runat="server" MsgSource="MConsumerAddress" MsgTextField="Logradouro" MsgSourceKey="1"></fwc:MsgLabel>&nbsp;
			           <fwc:MsgLabel id="Msglabel14" runat="server" MsgSource="MConsumerAddress" MsgTextField="Address" MsgSourceKey="1"></fwc:MsgLabel>, 
			           <fwc:MsgLabel id="Msglabel15" runat="server" MsgSource="MConsumerAddress" MsgTextField="AddressNumber" MsgSourceKey="1"></fwc:MsgLabel> 
  			        </td>
		        </tr>
		        <tr>
			        <td class="tdLabelSUM"><asp:Label id="Label16" runat="server">Complemento</asp:Label></td>
			        <td class="tdValorSUM"><fwc:MsgLabel id="Msglabel4" runat="server" MsgSource="MConsumerAddress" MsgTextField="AddressComplement" MsgSourceKey="1"></fwc:MsgLabel></td>
			        <td class="tdLabelSUM"><asp:Label id="Label22" runat="server">CEP</asp:Label></td>
			        <td class="tdValorSUM"><fwc:MsgLabel id="Msglabel19" runat="server" MsgSource="MConsumerAddress" MsgTextField="Cep" MsgSourceKey="1"></fwc:MsgLabel></td>
		        </tr>
		        <tr>
			        <td class="tdLabelSUM"><asp:Label id="Label20" runat="server">Bairro</asp:Label></td>
			        <td class="tdValorSUM"><fwc:MsgLabel id="Msglabel16" runat="server" MsgSource="MConsumerAddress" MsgTextField="District" MsgSourceKey="1"></fwc:MsgLabel></td>
			        <td class="tdLabelSUM"><asp:Label id="Label21" runat="server">Cidade</asp:Label></td>
			        <td class="tdValorSUM"><fwc:MsgLabel id="Msglabel18" runat="server" MsgSource="MConsumerAddress" MsgTextField="City" MsgSourceKey="1"></fwc:MsgLabel></td>
		        </tr>
		        <tr>
			        <td class="tdLabelSUM"><asp:Label id="Label23" runat="server">Estado</asp:Label></td>
			        <td class="tdValorSUM"><fwc:MsgLabel id="Msglabel20" runat="server" MsgSource="MConsumerAddress" MsgTextField="State" MsgSourceKey="1"></fwc:MsgLabel></td>
			        <td class="tdLabelSUM"><asp:Label id="Label24" runat="server">Pa�s</asp:Label></td>
			        <td class="tdValorSUM"><fwc:MsgLabel id="Msglabel21" runat="server" MsgSource="MConsumerAddress" MsgTextField="Country" MsgSourceKey="1"></fwc:MsgLabel></td>
		        </tr>
	          </table>
	      </asp:Panel>
  	    
  	    <asp:Panel ID="pnlDeliveryAddress" runat="server" Visible="false">
  	    <table class="tbBarraFerramentasSUM">
		    <tr>
			    <td class="tdBarraFerramentasSUM"><asp:Label id="Label17" runat="server">Endere�o de entrega</asp:Label></td>
		    </tr>
	    </table>
        <table class="tbPainelSUM" style="width: 100%">
		    <tr>
			    <td class="tdLabelSUM"><asp:Label id="Label18" runat="server">Endere�o</asp:Label></td>
			    <td class="tdValorSUM" colspan="3">
			       <fwc:MsgLabel id="Msglabel11" runat="server" MsgSource="MConsumerAddress" MsgTextField="Logradouro" MsgSourceKey="2"></fwc:MsgLabel>&nbsp;
			       <fwc:MsgLabel id="Msglabel12" runat="server" MsgSource="MConsumerAddress" MsgTextField="Address" MsgSourceKey="2"></fwc:MsgLabel>, 
			       <fwc:MsgLabel id="Msglabel22" runat="server" MsgSource="MConsumerAddress" MsgTextField="AddressNumber" MsgSourceKey="2"></fwc:MsgLabel> 
  			    </td>
		    </tr>
		    <tr>
			    <td class="tdLabelSUM"><asp:Label id="Label19" runat="server">Complemento</asp:Label></td>
			    <td class="tdValorSUM"><fwc:MsgLabel id="Msglabel24" runat="server" MsgSource="MConsumerAddress" MsgTextField="AddressComplement" MsgSourceKey="2"></fwc:MsgLabel></td>
			    <td class="tdLabelSUM"><asp:Label id="Label25" runat="server">CEP</asp:Label></td>
			    <td class="tdValorSUM"><fwc:MsgLabel id="Msglabel25" runat="server" MsgSource="MConsumerAddress" MsgTextField="Cep" MsgSourceKey="2"></fwc:MsgLabel></td>
		    </tr>
		    <tr>
			    <td class="tdLabelSUM"><asp:Label id="Label26" runat="server">Bairro</asp:Label></td>
			    <td class="tdValorSUM"><fwc:MsgLabel id="Msglabel27" runat="server" MsgSource="MConsumerAddress" MsgTextField="District" MsgSourceKey="2"></fwc:MsgLabel></td>
			    <td class="tdLabelSUM"><asp:Label id="Label27" runat="server">Cidade</asp:Label></td>
			    <td class="tdValorSUM"><fwc:MsgLabel id="Msglabel28" runat="server" MsgSource="MConsumerAddress" MsgTextField="City" MsgSourceKey="2"></fwc:MsgLabel></td>
		    </tr>
		    <tr>
			    <td class="tdLabelSUM"><asp:Label id="Label28" runat="server">Estado</asp:Label></td>
			    <td class="tdValorSUM"><fwc:MsgLabel id="Msglabel29" runat="server" MsgSource="MConsumerAddress" MsgTextField="State" MsgSourceKey="2"></fwc:MsgLabel></td>
			    <td class="tdLabelSUM"><asp:Label id="Label31" runat="server">Pa�s</asp:Label></td>
			    <td class="tdValorSUM"><fwc:MsgLabel id="Msglabel30" runat="server" MsgSource="MConsumerAddress" MsgTextField="Country" MsgSourceKey="2"></fwc:MsgLabel></td>
		    </tr>
        </table>
        </asp:Panel>
    </asp:Panel>
  	<!--Painel de Sumario-->
	<table class="tbBarraFerramentasSUM">
		<tr>
			<td class="tdBarraFerramentasSUM"><asp:Label id="Label4" runat="server"> Informa��es do Meio de Pagamento</asp:Label></td>
			<td class="tdBarraFerramentasAcaoSUM" style="height: 20px"><asp:Button id="lnkPostConfimacao" runat="server" OnClick="lnkPostConfimacao_Click" Text="Enviar Post Finaliza��o" /></td>
			<td class="tdBarraFerramentasAcaoSUM" style="height: 20px"><asp:Button id="lnkPostPagamento" runat="server" OnClick="lnkPostPagamento_Click" Text="Enviar Post Pagamento" /></td>
		</tr>
	</table>
	<table class="tbPainelSUM">
		<tr>
			<td class="tdLabelSUM"><asp:Label id="Label11" runat="server">Forma de pagamento</asp:Label></td>
			<td class="tdValorSUM" colspan="4"><fwc:MsgLabel id="Msglabel7" runat="server" MsgSource="MPaymentAttempt" MsgTextField="PaymentForm.Name"></fwc:MsgLabel></td>
		</tr>
		<tr>
			<td class="tdLabelSUM"><asp:Label id="Label12" runat="server">C�digo de identifica��o da transa��o</asp:Label></td>
			<td class="tdValorSUM" colspan="4"><fwc:MsgLabel id="Msglabel8" runat="server" MsgSource="MPaymentAttempt" MsgTextField="PaymentAttemptId"></fwc:MsgLabel></td>
		</tr>
		<tr>
			<td class="tdLabelSUM"><asp:Label id="Label29" runat="server">Status</asp:Label></td>
			<td class="tdValorSUM" colspan="4"><fwc:MsgLabel id="Msglabel23" runat="server" MsgSource="MPaymentAttempt" MsgTextField="Status"></fwc:MsgLabel></td>
		</tr>
		<tr>
			<td class="tdLabelSUM"><asp:Label id="Label30" runat="server">Mensagem operadora</asp:Label></td>
			<td class="tdValorSUM" colspan="4"><fwc:MsgLabel id="Msglabel26" runat="server" MsgSource="MPaymentAttempt" MsgTextField="ReturnMessage"></fwc:MsgLabel></td>
		</tr>
	</table>
    <asp:PlaceHolder ID="phTransactionDetail" runat="server"></asp:PlaceHolder>
</asp:Content>
