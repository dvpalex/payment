<%@ page language="C#" masterpagefile="~/Store/default/sp.master" autoeventwireup="true" inherits="payment, App_Web_ummozifu" %>
<asp:Content ID="Content1" ContentPlaceHolderID="tableTop" runat="Server">
    <asp:PlaceHolder ID="plhTableTop" runat="server"></asp:PlaceHolder>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="Server">
    <asp:DropDownList ID="ddlPayments" runat="server" CssClass="ddlPayments"  AutoPostBack="True" OnSelectedIndexChanged="ddlPayments_SelectedIndexChanged" ></asp:DropDownList>
    <asp:PlaceHolder ID="plhPaymentGroupInstructions" runat="server"></asp:PlaceHolder>
    <table width="100%">
    <tr>
    <asp:Repeater ID="rptCreditCards"  runat="server" OnItemCommand="rptCreditCards_ItemCommand">
    <ItemTemplate>
          <td style="width: 20px">
		    <asp:ImageButton CausesValidation=false AlternateText='<%# GetUTF8String(((SuperPag.Data.Messages.DPaymentForm)Container.DataItem).name) %>' id="ImageButton1" CommandArgument='<%# ((SuperPag.Data.Messages.DPaymentForm)Container.DataItem).paymentFormId.ToString() %>' runat="server" ImageUrl='<%# GetImageUrl( ((SuperPag.Data.Messages.DPaymentForm)Container.DataItem).paymentFormId.ToString() ) %>' ></asp:ImageButton>
		  </td>  
    </ItemTemplate>
    </asp:Repeater>
    </tr>
    </table>
    <asp:PlaceHolder ID="plhPaymentInstructions" runat="server"></asp:PlaceHolder>
    <asp:GridView ID="gvInstallments" runat="server" Width="100%" AutoGenerateColumns="False" BorderWidth="2px"	BorderStyle="Solid" BorderColor="White" OnRowCreated="gvInstallments_RowCreated" >
        <HeaderStyle CssClass="dataheader2" />
        <RowStyle CssClass="datatxt" HorizontalAlign="Center" />
        <Columns>
            <asp:TemplateField ShowHeader="False" >
                <ItemTemplate>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="InstallmentString" HeaderText="Parcela(s)" meta:resourcekey="BoundFieldResource1" />
            <asp:BoundField DataField="InstallmentValue" HeaderText="Valor Parcela" meta:resourcekey="BoundFieldResource2" />
            <asp:BoundField DataField="FinalAmount" HeaderText="Valor Total" meta:resourcekey="BoundFieldResource3" />
            <asp:BoundField DataField="InterestPercentage" HeaderText="Juros (% a.m.)" meta:resourcekey="BoundFieldResource4" />
        </Columns>
    </asp:GridView>
    <asp:Panel ID="pnlInputCreditCard" runat="server" Visible="false">
        <table border="0" cellpadding="2" cellspacing="1" width="100%">            
        <tr>
            <td>
                <asp:Label CssClass="instructions" ID="lblCardMessage" runat="server" Text="(*) Informar os três últimos dígitos do número em negrito no verso do cartão próximo à tarja de assinatura."></asp:Label>
            </td>
        </tr>	  
        </table>
        <table border="0" cellpadding="2" cellspacing="1" width="100%">            
          <tr>
            <td class="dataheader" style="width: 208px; text-align:left; height: 17px;">Nº Cartão de Crédito</td>
            <td class="dataheader" style="width: 138px; text-align:left; height: 17px;">
                <asp:Label ID="lblCVV" runat="server" CssClass="instructions" Text="CVV(*)"></asp:Label></td>
          </tr>      
          <tr bgcolor="#e7e3e7">
            <td style="width: 208px">
              <asp:TextBox CssClass="formTag" ID="txtCardNumber" MaxLength="16" runat="server" Width="228px"></asp:TextBox>&nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtCardNumber" ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator><asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtCardNumber" Display="Dynamic" ErrorMessage="*" ValidationExpression="\d{16}|\d{15}"></asp:RegularExpressionValidator></td>
            <td style="width: 138px">      
                <asp:TextBox CssClass="formTag" ID="txtCardSecurity" MaxLength="4" runat="server" Width="55px"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtCardSecurity"
                    ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator><asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtCardSecurity"
                    ErrorMessage="*" ValidationExpression="\d{4}|\d{3}" Display="Dynamic" Width="1px"></asp:RegularExpressionValidator></td>
          </tr>
          <tr>
            <td class="dataheader" style="text-align:left; height: 18px;">Nome do Titular</td>
            <td style="width: 138px"></td>
          </tr>
          <tr>
            <td colspan="2">
               <asp:TextBox CssClass="formTag" ID="txtCardName" MaxLength="25" runat="server" Width="375px"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtCardName"
                    ErrorMessage="*"></asp:RequiredFieldValidator>
            </td>
          </tr>
          <tr>
             <td colspan="2" class="dataheader" style="width: 208px">Data de Validade</td>
          </tr>
          <tr>
            <td colspan="2" align="left" bgcolor="#e7e3e7" >
                <asp:DropDownList ID="ddlCardValidationMonth" runat="server">
                    <asp:ListItem Value="0">---</asp:ListItem>
                    <asp:ListItem Value="01">01</asp:ListItem>
		            <asp:ListItem  Value="02" >02</asp:ListItem>
		            <asp:ListItem  Value="03" >03</asp:ListItem>
		            <asp:ListItem  Value="04" >04</asp:ListItem>
		            <asp:ListItem  Value="05" >05</asp:ListItem>
		            <asp:ListItem  Value="06" >06</asp:ListItem>
		            <asp:ListItem  Value="07" >07</asp:ListItem>
		            <asp:ListItem  Value="08" >08</asp:ListItem>
		            <asp:ListItem  Value="09" >09</asp:ListItem>
		            <asp:ListItem  Value="10" >10</asp:ListItem>
		            <asp:ListItem  Value="11" >11</asp:ListItem>
		            <asp:ListItem  Value="12" >12</asp:ListItem>
                </asp:DropDownList>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator3"
                        runat="server" ControlToValidate="ddlCardValidationMonth" Display="Dynamic" ErrorMessage="*"
                        ValidationExpression="\d{2}"></asp:RegularExpressionValidator>
                <asp:DropDownList ID="ddlCardValidationYear" runat="server">
		            <asp:ListItem Value="0">-------</asp:ListItem>
		            <asp:ListItem Value="2006" >2006</asp:ListItem>
		            <asp:ListItem Value="2007" >2007</asp:ListItem>
		            <asp:ListItem Value="2008" >2008</asp:ListItem>
		            <asp:ListItem Value="2009" >2009</asp:ListItem>
		            <asp:ListItem Value="2010" >2010</asp:ListItem>
		            <asp:ListItem Value="2011" >2011</asp:ListItem>
		            <asp:ListItem Value="2012" >2012</asp:ListItem>
		            <asp:ListItem Value="2013" >2013</asp:ListItem>
		            <asp:ListItem Value="2014" >2014</asp:ListItem>
		            <asp:ListItem Value="2015" >2015</asp:ListItem>			
                </asp:DropDownList>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" ControlToValidate="ddlCardValidationYear"
                    Display="Dynamic" ErrorMessage="*" ValidationExpression="\d{4}"></asp:RegularExpressionValidator>
              <font size="1"><font face='Verdana, Arial, San-Serif' size='1' color='gray'>mês / ano</font></font>
            </td>
          </tr>
        </table>    
    </asp:Panel>
    <div class="buttonbtnNext"><asp:ImageButton id="btnNext" onclick="btnNext_Click" runat="server" ImageUrl="~/Store/default/images/continuar.gif" ></asp:ImageButton></div>
    <br />
    <asp:Label id="lblHomolog" runat="server" CssClass="instructions"></asp:Label>
    <br />
    <div class="buttonlnkReturn"><asp:HyperLink id="lnkReturn" runat="server" ImageUrl="~/Store/default/images/retornarloja.gif" NavigateUrl="#" Visible="false" Target="_parent">Retornar à Loja</asp:HyperLink></div>    
</asp:Content>
