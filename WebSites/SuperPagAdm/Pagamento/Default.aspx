<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Default.aspx.cs" Inherits="Pagamento_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <head>

        <script type="text/javascript" language="javascript" src="../Jscript/ValidaTeclas.js"></script>

    </head>
    <asp:MultiView ID="MultiViewGeral" runat="server">
        <asp:View ID="ViewTelaEntrada" runat="server">
            <table class="tbPainelFRM" border="0">
                <tr>
                    <td class="tdBarraFerramentasSUM" align="center" colspan="2">
                        Informações de pagamento.
                    </td>
                </tr>
                <tr>
                    <td class="tdCampoFRM" style="padding: 5px  5px 3px;">
                        <table border="0">
                            <tr>
                                <td class="tdCampoFRM" style="width: 106px">
                                    Tipo de Pagamento
                                </td>
                                <td class="tdCampoFRM">
                                    <asp:DropDownList ID="ddlTipoPag" runat="server" Enabled="False">
                                        <asp:ListItem Value="42">Boleto Invest Cred S/A</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:MultiView ID="MultiViewPagForm" runat="server">
                            <asp:View ID="ViewPagIpte" runat="server">
                                <table width="100%">
                                    <tr>
                                        <td class="tdCampoFRM" style="padding-right: 5px; padding-left: 5px; padding-bottom: 3px;
                                            padding-top: 5px; height: 101px;">
                                            <fieldset style="width: 960px;">
                                                <legend>Dados do Documento</legend>
                                                <table border="0" width="100%">
                                                    <tr>
                                                        <td class="tdCampoFRM" style="width: 58px">
                                                            IPTE
                                                        </td>
                                                        <td class="tdCampoFRM">
                                                            <asp:TextBox ID="txtIPTE" runat="server" Width="311px"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="rvIPTE" runat="server" ControlToValidate="txtIPTE"
                                                                Display="Dynamic" ErrorMessage="Informe Número do IPTE.">*</asp:RequiredFieldValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="tdCampoFRM" style="width: 58px">
                                                            Sacado
                                                        </td>
                                                        <td class="tdCampoFRM">
                                                            <asp:TextBox ID="txtSacado" runat="server" MaxLength="50"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="rvSacado" runat="server" ControlToValidate="txtSacado"
                                                                Display="Dynamic" ErrorMessage="Informe o nome do Sacado.">*</asp:RequiredFieldValidator></td>
                                                    </tr>
                                                    <tr>
                                                        <td class="tdCampoFRM" style="width: 58px">
                                                            Contrato</td>
                                                        <td class="tdCampoFRM">
                                                            <asp:TextBox ID="txtContrato" runat="server" MaxLength="50"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ControlToValidate="txtContrato"
                                                                Display="Dynamic" ErrorMessage="Informe o contrato.">*</asp:RequiredFieldValidator></td>
                                                    </tr>
                                                </table>
                                            </fieldset>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdCampoFRM" style="padding-right: 5px; padding-left: 5px; padding-bottom: 3px;
                                            padding-top: 5px">
                                            <fieldset style="width: 960px;">
                                                <legend>Canal de Envio</legend>
                                                <table border="0">
                                                    <tr>
                                                        <td class="tdCampoFRM" style="width: 60px;" valign="middle">
                                                            <asp:RadioButtonList ID="RadioEnvio" runat="server" AutoPostBack="true" RepeatDirection="Horizontal"
                                                                Width="306px" OnSelectedIndexChanged="RadioEnvio_SelectedIndexChanged">
                                                                <asp:ListItem Value="01">E-mail</asp:ListItem>
                                                                <asp:ListItem Value="02">Fax</asp:ListItem>
                                                                <asp:ListItem Value="03">Correio</asp:ListItem>
                                                                <asp:ListItem Value="04">Impress&#227;o</asp:ListItem>
                                                            </asp:RadioButtonList>
                                                        </td>
                                                        <td align="left" class="tdCampoFRM" style="width: 60px;" valign="middle">
                                                            <asp:RequiredFieldValidator ID="rvCanalEnvio" runat="server" ControlToValidate="RadioEnvio"
                                                                Display="Dynamic" ErrorMessage="Selecione um Canala de Envio.">*</asp:RequiredFieldValidator></td>
                                                    </tr>
                                                </table>
                                            </fieldset>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdCampoFRM">
                                            <asp:MultiView ID="MultiViewEnvio" runat="server">
                                                <asp:View ID="ViewEmail" runat="server">
                                                    <fieldset style="width: 960px;">
                                                        <legend>Informe o e-mail do sacado</legend>
                                                        <table>
                                                            <tr>
                                                                <td class="tdCampoFRM" style="width: 85px">
                                                                    Para
                                                                </td>
                                                                <td class="tdCampoFRM">
                                                                    <asp:TextBox ID="txtEmail" runat="server"></asp:TextBox>
                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtEmail"
                                                                        Display="Dynamic" ErrorMessage="Informe o email do Sacado.">*</asp:RequiredFieldValidator>
                                                                    <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtEmail"
                                                                        ErrorMessage="E-mail invalido!" ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator></td>
                                                            </tr>
                                                        </table>
                                                    </fieldset>
                                                </asp:View>
                                                <asp:View ID="ViewFax" runat="server">
                                                    <fieldset style="width: 960px;">
                                                        <legend>Informe o número do Fax do sacado</legend>
                                                        <table>
                                                            <tr>
                                                                <td class="tdCampoFRM" style="width: 85px">
                                                                    Fax
                                                                </td>
                                                                <td class="tdCampoFRM">
                                                                    <asp:TextBox ID="txtDDD" runat="server" Width="20px" MaxLength="2"></asp:TextBox>
                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtDDD"
                                                                        Display="Dynamic" ErrorMessage="Informe o DDD." Width="11px">*</asp:RequiredFieldValidator>&nbsp;
                                                                    <asp:TextBox ID="txtTel" runat="server" MaxLength="8"></asp:TextBox>&nbsp;
                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtTel"
                                                                        Display="Dynamic" ErrorMessage="InformE O número do FAX.">*</asp:RequiredFieldValidator></td>
                                                            </tr>
                                                        </table>
                                                    </fieldset>
                                                </asp:View>
                                                <asp:View ID="ViewCorreio" runat="server">
                                                    <fieldset style="width: 960px; padding-right: 5px;">
                                                        <legend>Informe o endereço completo do sacado</legend>
                                                        <table>
                                                            <tr>
                                                                <td class="tdCampoFRM">
                                                                    Endereço
                                                                </td>
                                                                <td class="tdCampoFRM">
                                                                    <asp:TextBox ID="txtEnd" runat="server" Width="295px"></asp:TextBox>
                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtEnd"
                                                                        Display="Dynamic" ErrorMessage="Informe o endereço do Sacado.">*</asp:RequiredFieldValidator></td>
                                                                <td class="tdCampoFRM">
                                                                    Nº
                                                                </td>
                                                                <td class="tdCampoFRM">
                                                                    <asp:TextBox ID="txtNum" runat="server" Width="66px" MaxLength="7"></asp:TextBox>
                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="txtNum"
                                                                        Display="Dynamic" ErrorMessage="Informe o número da residência do Sacado.">*</asp:RequiredFieldValidator></td>
                                                                <td class="tdCampoFRM">
                                                                    Complemento
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtComplemento" runat="server" Width="156px"></asp:TextBox>&nbsp;
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="tdCampoFRM">
                                                                    Bairro
                                                                </td>
                                                                <td class="tdCampoFRM">
                                                                    <asp:TextBox ID="txtBairro" runat="server"></asp:TextBox>
                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtBairro"
                                                                        Display="Dynamic" ErrorMessage="Informe o bairro do Sacado.">*</asp:RequiredFieldValidator></td>
                                                                <td class="tdCampoFRM">
                                                                    CEP
                                                                </td>
                                                                <td class="tdCampoFRM">
                                                                    <asp:TextBox ID="txtCEP" runat="server" Width="130px" MaxLength="8"></asp:TextBox>
                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="txtCEP"
                                                                        Display="Dynamic" ErrorMessage="Informe o CEP do Sacado.">*</asp:RequiredFieldValidator></td>
                                                            </tr>
                                                            <tr>
                                                                <td class="tdCampoFRM">
                                                                    Estado
                                                                </td>
                                                                <td class="tdCampoFRM">
                                                                    <asp:DropDownList ID="ddlEstado" runat="server">
                                                                        <asp:ListItem></asp:ListItem>
                                                                        <asp:ListItem Value="01">AC</asp:ListItem>
                                                                        <asp:ListItem Value="02">AL</asp:ListItem>
                                                                        <asp:ListItem Value="03">AM</asp:ListItem>
                                                                        <asp:ListItem Value="04">AP</asp:ListItem>
                                                                        <asp:ListItem Value="05">BA</asp:ListItem>
                                                                        <asp:ListItem Value="06">CE</asp:ListItem>
                                                                        <asp:ListItem Value="07">DF</asp:ListItem>
                                                                        <asp:ListItem Value="08">ES</asp:ListItem>
                                                                        <asp:ListItem Value="09">GO</asp:ListItem>
                                                                        <asp:ListItem Value="10">MA</asp:ListItem>
                                                                        <asp:ListItem Value="11">MG</asp:ListItem>
                                                                        <asp:ListItem Value="12">MS</asp:ListItem>
                                                                        <asp:ListItem Value="13">MT</asp:ListItem>
                                                                        <asp:ListItem Value="14">PA</asp:ListItem>
                                                                        <asp:ListItem Value="15">PB</asp:ListItem>
                                                                        <asp:ListItem Value="16">PE</asp:ListItem>
                                                                        <asp:ListItem Value="17">PI</asp:ListItem>
                                                                        <asp:ListItem Value="18">PR</asp:ListItem>
                                                                        <asp:ListItem Value="19">RJ</asp:ListItem>
                                                                        <asp:ListItem Value="20">RN</asp:ListItem>
                                                                        <asp:ListItem Value="21">RO</asp:ListItem>
                                                                        <asp:ListItem Value="22">RR</asp:ListItem>
                                                                        <asp:ListItem Value="23">RS</asp:ListItem>
                                                                        <asp:ListItem Value="24">SC</asp:ListItem>
                                                                        <asp:ListItem Value="25">SE</asp:ListItem>
                                                                        <asp:ListItem Value="26">SP</asp:ListItem>
                                                                        <asp:ListItem Value="27">TO</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="ddlEstado"
                                                                        Display="Dynamic" ErrorMessage="Informe o estado do Sacado.">*</asp:RequiredFieldValidator></td>
                                                                <td class="tdCampoFRM">
                                                                    Cidade
                                                                </td>
                                                                <td class="tdCampoFRM">
                                                                    <asp:TextBox ID="txtCidade" runat="server" Width="130px"></asp:TextBox>
                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="txtCidade"
                                                                        Display="Dynamic" ErrorMessage="Informe a Cidade do Sacado.">*</asp:RequiredFieldValidator></td>
                                                            </tr>
                                                        </table>
                                                    </fieldset>
                                                </asp:View>
                                            </asp:MultiView>
                                        </td>
                                    </tr>
                                </table>
                            </asp:View>
                        </asp:MultiView>
                    </td>
                </tr>
                <tr>
                    <td style="padding-right: 10px; padding-left: 5px; padding-bottom: 5px; padding-top: 5px;
                        text-align: right; background-color: white;">
                        &nbsp;<asp:Button ID="btnConfirmarGeral" runat="server" Text="Confirmar" OnClick="btnConfirmarGeral_Click" />
                        <asp:Button ID="btnCancelGeral" runat="server" Text="Cancelar" OnClick="btnCancelGeral_Click" />&nbsp;&nbsp;
                    </td>
                </tr>
                <tr>
                    <td align="left" style="padding-right: 10px; padding-left: 5px; padding-bottom: 5px;
                        padding-top: 5px; background-color: white; text-align: left">
                        <asp:CustomValidator ID="CustomIPTEent" runat="server" ErrorMessage="Erro inesperado consulte o administrador."
                            Font-Size="10pt" ForeColor="OrangeRed"></asp:CustomValidator></td>
                </tr>
            </table>
        </asp:View>
        <asp:View ID="ViewTelaFinal" runat="server">
            <table class="tbPainelFRM" border="0">
                <tr>
                    <td class="tdBarraFerramentasSUM" align="center" colspan="2">
                        Confirmação.
                    </td>
                </tr>
                <tr>
                    <td class="tdCampoFRM" style="padding-right: 5px; padding-left: 5px; padding-bottom: 3px;
                        padding-top: 5px">
                        <asp:MultiView ID="MultiViewPagFinal" runat="server">
                            <asp:View ID="ViewFinalEmail" runat="server">
                                <fieldset style="width: 960px;">
                                    <legend>Confirmação dos Dados</legend>
                                    <table>
                                        <tr>
                                            <td class="tdCampoFRM" style="border: none; width: 154px;">
                                                <b>Representação Numérica</b>
                                            </td>
                                            <td colspan="3">
                                                <asp:Label ID="lblEmailIPTE" runat="server" Text="24990.10730 80705.010017 85052.05732 1 13840000001349"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdCampoFRM" style="border: none; width: 154px;">
                                                <b>Sacado</b>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblEmailSacado" runat="server" Text="Label"></asp:Label>
                                            </td>
                                            <td colspan="2">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdCampoFRM" style="border: none; width: 154px;">
                                                <b>Valor da Operação</b></td>
                                            <td>
                                                <asp:Label ID="lblEmailValOp" runat="server"></asp:Label>
                                            </td>
                                            <td class="tdCampoFRM" style="border: none; width: 98px;">
                                                <b>Data Vencimento</b>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblEmailDataVenc" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdCampoFRM" style="border: none; width: 154px;">
                                                <b>Canal de Envio</b>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblEmailCanalEnv" runat="server" Text="Label"></asp:Label>
                                            </td>
                                            <td class="tdCampoFRM" style="border: none; width: 98px;">
                                                <b>E-mail</b>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblEmailCanalEmail" runat="server" Text="Label"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </asp:View>
                            <asp:View ID="ViewFinalFax" runat="server">
                                <fieldset style="width: 960px;">
                                    <legend>Confirmação dos Dados</legend>
                                    <table>
                                        <tr>
                                            <td class="tdCampoFRM" style="border: none; width: 154px;">
                                                <b>Representação Numérica</b>
                                            </td>
                                            <td colspan="3">
                                                <asp:Label ID="lblFaxIPTE" runat="server" Text="24990.10730 80705.010017 85052.05732 1 13840000001349"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdCampoFRM" style="border: none; width: 154px;">
                                                <b>Sacado</b>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblFaxSacado" runat="server" Text="Label"></asp:Label>
                                            </td>
                                            <td colspan="2">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdCampoFRM" style="border: none; width: 154px;">
                                                <b>Valor da Operação</b>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblFaxValOp" runat="server" Text="Label"></asp:Label>
                                            </td>
                                            <td class="tdCampoFRM" style="border: none; width: 98px;">
                                                <b>Data Vencimento</b>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblFaxDataVenc" runat="server" Text="Label"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdCampoFRM" style="border: none; width: 154px;">
                                                <b>Canal de Envio</b>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblFaxCanalEnv" runat="server" Text="Label"></asp:Label>
                                            </td>
                                            <td class="tdCampoFRM" style="border: none; width: 98px;">
                                                <b>Número</b>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblFaxTel" runat="server" Text="Label"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </asp:View>
                            <asp:View ID="ViewFinalCorreio" runat="server">
                                <fieldset style="width: 960px;">
                                    <legend>Confirmação dos Dados</legend>
                                    <table>
                                        <tr>
                                            <td class="tdCampoFRM" style="border: none; width: 154px;">
                                                <b>Representação Numérica</b>
                                            </td>
                                            <td colspan="3">
                                                <asp:Label ID="lblCorreioIPTE" runat="server" Text="24990.10730 80705.010017 85052.05732 1 13840000001349"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdCampoFRM" style="border: none; width: 154px;">
                                                <b>Sacado</b>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblCorreioSacado" runat="server" Text="Label"></asp:Label>
                                            </td>
                                            <td colspan="2">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdCampoFRM" style="border: none; width: 154px;">
                                                <b>Valor da Operação</b>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblCorreioValOp" runat="server" Text="Label"></asp:Label>
                                            </td>
                                            <td class="tdCampoFRM" style="border: none; width: 98px;">
                                                <b>Data Vencimento</b>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblCorreioDataVenc" runat="server" Text="Label"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdCampoFRM" style="border: none; width: 154px;">
                                                <b>Canal de Envio</b>
                                            </td>
                                            <td colspan="3">
                                                <asp:Label ID="lblCorreioCanalEnv" runat="server" Text="Label"></asp:Label>
                                                &nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td class="tdCampoFRM" style="border-right: medium none; border-top: medium none;
                                                border-left: medium none; width: 154px; border-bottom: medium none">
                                                <strong>Endereço</strong></td>
                                            <td colspan="3">
                                                <asp:Label ID="lblCorreioEnd" runat="server" Text="Label"></asp:Label></td>
                                        </tr>
                                        <tr>
                                            <td class="tdCampoFRM" style="border-right: medium none; border-top: medium none;
                                                border-left: medium none; width: 154px; border-bottom: medium none">
                                                <strong>Bairro</strong></td>
                                            <td>
                                                <asp:Label ID="lblCorreioBairro" runat="server" Text="Label"></asp:Label></td>
                                            <td class="tdCampoFRM" style="border-right: medium none; border-top: medium none;
                                                border-left: medium none; width: 98px; border-bottom: medium none">
                                                <strong>CEP</strong></td>
                                            <td>
                                                <asp:Label ID="lblCorreioCEP" runat="server" Text="Label"></asp:Label></td>
                                        </tr>
                                        <tr>
                                            <td class="tdCampoFRM" style="border-right: medium none; border-top: medium none;
                                                border-left: medium none; width: 154px; border-bottom: medium none">
                                                <strong>Estado</strong></td>
                                            <td>
                                                <asp:Label ID="lblCorreioEstado" runat="server" Text="Label"></asp:Label></td>
                                            <td class="tdCampoFRM" style="border-right: medium none; border-top: medium none;
                                                border-left: medium none; width: 98px; border-bottom: medium none">
                                                <strong>Cidade</strong></td>
                                            <td>
                                                <asp:Label ID="lblCorreioCidade" runat="server" Text="Label"></asp:Label></td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </asp:View>
                            <asp:View ID="ViewImpressao" runat="server">
                                <fieldset style="width: 960px;">
                                    <legend>Confirmação dos Dados</legend>
                                    <table>
                                        <tr>
                                            <td class="tdCampoFRM" style="border: none; width: 154px;">
                                                <b>Representação Numérica</b>
                                            </td>
                                            <td colspan="3">
                                                <asp:Label ID="lblImpressaoIPTE" runat="server" Text="24990.10730 80705.010017 85052.05732 1 13840000001349"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdCampoFRM" style="border: none; width: 154px;">
                                                <b>Sacado</b>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblImpressaoSacado" runat="server"></asp:Label>
                                            </td>
                                            <td colspan="2">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdCampoFRM" style="border: none; width: 154px;">
                                                <b>Valor da Operação</b></td>
                                            <td>
                                                <asp:Label ID="lblImpressaoValOp" runat="server"></asp:Label>
                                            </td>
                                            <td class="tdCampoFRM" style="border: none; width: 98px;">
                                                <b>Data Vencimento</b>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblImpressaoDataVenc" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdCampoFRM" style="border: none; width: 154px;">
                                                <b>Canal de Envio</b>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblImpressaoCanalEnv" runat="server"></asp:Label>
                                            </td>
                                            <td colspan="2">
                                                <b></b>&nbsp;
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </asp:View>
                        </asp:MultiView>
                    </td>
                </tr>
                <tr>
                    <td style="padding-right: 10px; padding-left: 5px; padding-bottom: 5px; padding-top: 5px;
                        text-align: right; background-color: white;">
                        &nbsp;<asp:Button ID="btnFinalComfirm" runat="server" Text="Confirmar" OnClick="btnFinalComfirm_Click"
                            OnClientClick="document.getElementById(this.name).style.display = 'none';" />
                            
                        &nbsp;<asp:Button ID="btnFinalCancel" runat="server" Text="Cancelar" OnClick="btnFinalCancel_Click" />&nbsp;&nbsp;
                    </td>
                </tr>
                <tr>
                    <td style="padding-right: 10px; padding-left: 5px; padding-bottom: 5px; padding-top: 5px;
                        background-color: white; text-align: left">
                        <asp:CustomValidator ID="CustomIPTE" runat="server" ErrorMessage="Erro inesperado consulte o administrador."
                            ForeColor="OrangeRed" Font-Size="10pt"></asp:CustomValidator></td>
                </tr>
            </table>
        </asp:View>
    </asp:MultiView>
</asp:Content>
