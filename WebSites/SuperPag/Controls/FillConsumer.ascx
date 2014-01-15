<%@ Control Language="C#" AutoEventWireup="true" CodeFile="fillconsumer.ascx.cs" Inherits="Store_default_fillconsumer" %>

<asp:Label ID="label2" runat="server" CssClass="fillInstructions" meta:resourcekey="label2Resource1" Text="(*) indica campo de preenchimento obrigatório" style="text-align: right"/>

<div id="consumerDetails" runat="server" visible="false">
<table border="0" cellpadding="0" cellspacing="4" width="100%" >
    <tr>
        <td class="fillRowTitle" colspan="2" valign="top"><asp:Label ID="label1" runat="server" meta:resourcekey="label1Resource1" Text="Dados do Cadastro" /></td></tr>
    <tr>
        <td class="fillLabel"><asp:Label ID="label3" runat="server" meta:resourcekey="label3Resource1" Text="Pessoa" /></td><td>
        <asp:DropDownList ID="ddlPessoa" runat="server" OnSelectedIndexChanged="ddlPessoa_SelectedIndexChanged" meta:resourcekey="ddlPessoaResource1" AutoPostBack="true">
            <asp:ListItem meta:resourcekey="ListItemResource1" Value="0" />
            <asp:ListItem meta:resourcekey="ListItemResource2" Text="Jur&#237;dica" Value="1" />
            <asp:ListItem  meta:resourcekey="ListItemResource3" Text="F&#237;sica" Value="2" />
        </asp:DropDownList></td></tr>
</table>
        
<table id="tblFisica" runat="server" border="0" cellpadding="0" cellspacing="4" width="100%" visible="false">
    <tr>
        <td class="fillLabel"><asp:Label ID="label4" runat="server" meta:resourcekey="label4Resource1" Text="Nome (*)" /></td>
        <td><asp:TextBox ID="txtNome" runat="server" Columns="40" MaxLength="40" meta:resourcekey="txtNomeResource2" />
            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtNome"
                ErrorMessage="*"></asp:RequiredFieldValidator></td></tr>
    <tr>
        <td class="fillLabel"><asp:Label ID="label5" runat="server" meta:resourcekey="label5Resource1" Text="CPF (*) " /></td>
        <td><asp:TextBox ID="txtCPF" runat="server" Columns="11" MaxLength="11" meta:resourcekey="txtCPFResource2"  />
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtCPF"
                ErrorMessage="*"></asp:RequiredFieldValidator></td></tr>
    <tr>
        <td class="fillLabel"><asp:Label ID="label6" runat="server" meta:resourcekey="label6Resource1" Text="Nascimento" /></td>
        <td><asp:TextBox ID="txtNascimento" runat="server" Columns="10" MaxLength="10" meta:resourcekey="txtNascimentoResource2" /></td></tr>
    <tr>
        <td class="fillLabel"><asp:Label ID="label7" runat="server" meta:resourcekey="label7Resource1" Text="Sexo" /></td>
        <td><asp:DropDownList ID="ddlSexo" runat="server" meta:resourcekey="ddlSexoResource1">
                <asp:ListItem meta:resourcekey="ListItemResource4"/>
                <asp:ListItem Text="Masculino" Value="M" meta:resourcekey="ListItemResource5"/>
                <asp:ListItem Text="Feminino" Value="F" meta:resourcekey="ListItemResource6"/>
            </asp:DropDownList></td></tr>
    <tr>
        <td class="fillLabel"><asp:Label ID="label8" runat="server" meta:resourcekey="label8Resource1" Text="Estado Civil" /></td>
        <td><asp:DropDownList ID="ddlEstadoCivil" runat="server"><asp:ListItem meta:resourcekey="EstadoCivil1"/>
                <asp:ListItem Text="Solteiro" Value="S" meta:resourcekey="EstadoCivil2"/>
                <asp:ListItem Text="Casado" Value="C" meta:resourcekey="EstadoCivil4"/>
            </asp:DropDownList></td></tr>
    <tr>
        <td class="fillLabel"><asp:Label ID="label9" runat="server" meta:resourcekey="label9Resource1" Text="Profissão" /></td>
        <td><asp:TextBox ID="txtProfissao" runat="server" Columns="40" MaxLength="40" meta:resourcekey="txtProfissaoResource2"  /></td></tr>
</table>

<table id="tblJuridica" runat="server" border="0" cellpadding="0" cellspacing="4" width="100%" visible="false">
    <tr>
        <td class="fillLabel"><asp:Label ID="label10" runat="server" meta:resourcekey="label10Resource1" Text="Razão Social (*)" /></td>
        <td><asp:TextBox ID="txtRazaoSocial" runat="server" Columns="40" MaxLength="40" meta:resourcekey="txtRazaoSocialResource2" />
            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtRazaoSocial"
                ErrorMessage="*"></asp:RequiredFieldValidator></td></tr>
    <tr>
        <td class="fillLabel"><asp:Label ID="label11" runat="server" meta:resourcekey="label11Resource1" Text="CNPJ (*)" /></td>
        <td><asp:TextBox ID="txtCNPJ" runat="server" Columns="20" MaxLength="14" meta:resourcekey="txtCNPJResource2" /> 
            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtCNPJ"
                ErrorMessage="*"></asp:RequiredFieldValidator>&nbsp;
            <asp:Label CssClass="fillInstructions" ID="label34" runat="server" meta:resourcekey="label34Resource1">numérico 14 dígitos</asp:Label></td></tr>
    <tr>
        <td class="fillLabel"><asp:Label ID="label12" runat="server" meta:resourcekey="label12Resource1" Text="Incrição" /></td>
        <td><asp:TextBox ID="txtIE" runat="server" MaxLength="40" meta:resourcekey="txtIEResource2" /> <asp:Label CssClass="fillInstructions" ID="label35" runat="server" meta:resourcekey="label35Resource1">municipal ou estadual</asp:Label></td></tr>
    <tr>
        <td class="fillLabel">&nbsp;</td><td> <strong><asp:Label ID="label13" runat="server" meta:resourcekey="label13Resource1" Text="O nome e CPF devem ser do responsável pela empresa" /></strong></td></tr>
    <tr>
        <td class="fillLabel"><asp:Label ID="label14" runat="server" meta:resourcekey="label14Resource1" Text="Nome" /></td>
        <td><asp:TextBox ID="txtNomeResp" runat="server" Columns="40" MaxLength="40" meta:resourcekey="txtNomeRespResource1"/> <asp:Label CssClass="fillInstructions" ID="label36" runat="server" meta:resourcekey="label36Resource1">completo</asp:Label></td></tr>
    <tr>
        <td class="fillLabel"><asp:Label ID="label15" runat="server" meta:resourcekey="label15Resource1" Text="CPF" /></td>
        <td><asp:TextBox ID="txtCPFResp" runat="server" Columns="11" MaxLength="11" meta:resourcekey="txtCPFRespResource1"/> <asp:Label CssClass="fillInstructions" ID="label37" runat="server" meta:resourcekey="label37Resource1">numérico 11 dígitos</asp:Label></td></tr>
</table>
    
<table border="0" cellpadding="0" cellspacing="4" width="100%">
    <tr >
        <td class="fillLabel"><asp:Label ID="label16" runat="server" meta:resourcekey="label16Resource1" Text="Telefone" /></td>
        <td><asp:TextBox ID="txtTelefoneDDD" runat="server" Columns="2" MaxLength="2" meta:resourcekey="txtTelefoneDDDResource1"/> - <asp:TextBox ID="txtTelefone" runat="server" Columns="8" MaxLength="8" meta:resourcekey="txtTelefoneResource1"/></td></tr>
    <tr>
        <td class="fillLabel"><asp:Label ID="label17" runat="server" meta:resourcekey="label17Resource1" Text="Fax" /></td>
        <td><asp:TextBox ID="txtFaxDDD" runat="server" Columns="2" MaxLength="2" meta:resourcekey="txtFaxDDDResource1"/> - <asp:TextBox ID="txtFax" runat="server" Columns="8" MaxLength="8" meta:resourcekey="txtFaxResource1"/></td></tr>
    <tr>
        <td class="fillLabel"><asp:Label ID="label18" runat="server" meta:resourcekey="label18Resource1" Text="Email" /></td>
        <td><asp:TextBox ID="txtEmail" runat="server" Columns="40" MaxLength="40" meta:resourcekey="txtEmailResource1"/></td></tr>
</table>
</div>

<table id="tblEnderecoCobranca" runat="server" border="0" cellpadding="0" cellspacing="4" width="100%" visible="false">
    <tr><td class="fillRowTitle" colspan="2" valign="top"><asp:Label ID="label19" runat="server" meta:resourcekey="label19Resource1" Text="Endereço de Cobrança" /></td></tr>
    <tr><td class="fillLabel"><asp:Label ID="label20" runat="server" meta:resourcekey="label20Resource1" Text="Endereço (*)" /></td><td><asp:DropDownList ID="ddlLogradouro" runat="server" meta:resourcekey="ddlLogradouroResource1">
            <asp:ListItem meta:resourcekey="ListItemResource7"></asp:ListItem>
            <asp:ListItem meta:resourcekey="ListItemResource8" Text="Rua"></asp:ListItem>
            <asp:ListItem meta:resourcekey="ListItemResource9" Text="Avenida"></asp:ListItem>
            <asp:ListItem meta:resourcekey="ListItemResource10" Text="Travessa"></asp:ListItem>
            <asp:ListItem meta:resourcekey="ListItemResource11" Text="Pra&#231;a"></asp:ListItem>
            <asp:ListItem meta:resourcekey="ListItemResource12" Text="Alameda"></asp:ListItem>
            <asp:ListItem meta:resourcekey="ListItemResource13" Text="Quadra"></asp:ListItem>
            <asp:ListItem meta:resourcekey="ListItemResource14" Text="Bloco"></asp:ListItem>
            <asp:ListItem meta:resourcekey="ListItemResource15" Text="Outro"></asp:ListItem>
        </asp:DropDownList></td></tr>
    <tr>
        <td class="fillLabel"></td>
        <td class="fillLabel"><asp:TextBox ID="txtEndereco" runat="server" Columns="40" MaxLength="130" /> 
            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtEndereco"
                ErrorMessage="*"></asp:RequiredFieldValidator>
            Nº <asp:TextBox ID="txtNumero" runat="server" Columns="7" MaxLength="7" /> 
            <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ControlToValidate="txtNumero"
                ErrorMessage="*"></asp:RequiredFieldValidator>
        </td></tr>
    <tr>
        <td class="fillLabel"><asp:Label ID="label21" runat="server" meta:resourcekey="label21Resource1" Text="Complemento" /></td>
        <td><asp:TextBox ID="txtComplemento" runat="server" Columns="40" MaxLength="40" meta:resourcekey="txtComplementoResource1"/></td></tr>
    <tr>
        <td class="fillLabel"><asp:Label ID="label22" runat="server" meta:resourcekey="label22Resource1" Text="Bairro" /></td>
        <td><asp:TextBox ID="txtBairro" runat="server" Columns="40" MaxLength="40" meta:resourcekey="txtBairroResource1"/></td></tr>
    <tr>
        <td class="fillLabel"><asp:Label ID="label23" runat="server" meta:resourcekey="label23Resource1" Text="Cidade (*)" /></td>
        <td><asp:TextBox ID="txtCidade" runat="server" Columns="40" MaxLength="40" meta:resourcekey="txtCidadeResource1"/> 
            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtCidade"
                ErrorMessage="*"></asp:RequiredFieldValidator>
        </td></tr>
    <tr>
        <td class="fillLabel"><asp:Label ID="label24" runat="server" meta:resourcekey="label24Resource1" Text="CEP (*)" /></td>
        <td><asp:TextBox ID="txtCEP" runat="server" MaxLength="5" Columns="5" meta:resourcekey="txtCEPResource1"/> 
            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="txtCEP"
                ErrorMessage="*"></asp:RequiredFieldValidator>
            - <asp:TextBox ID="txtCEP2" runat="server" Columns="3" MaxLength="3" meta:resourcekey="txtCEP2Resource1"/> 
            <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="txtCEP2"
                ErrorMessage="*"></asp:RequiredFieldValidator>
            <asp:Label ID="Label38" Text="(5 - 3 dígitos)" CssClass="fillInstructions" runat="server"/></td></tr>
    <tr>
        <td class="fillLabel"><asp:Label ID="label25" runat="server" meta:resourcekey="label25Resource1" Text="Estado (*)" /></td>
        <td><asp:TextBox ID="txtEstado" runat="server" Columns="40" MaxLength="40" meta:resourcekey="txtEstadoResource1"/>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server" ControlToValidate="txtEstado"
                ErrorMessage="*"></asp:RequiredFieldValidator></td></tr>
    <tr>
        <td class="fillLabel"><asp:Label ID="label26" runat="server" meta:resourcekey="label26Resource1" Text="País" /></td>
        <td>
            <asp:DropDownList ID="ddlPais" runat="server">
                <asp:ListItem Value=""></asp:ListItem>
                <asp:ListItem Value="AC">ACORES E MADEIRA</asp:ListItem>
                <asp:ListItem Value="AF">AFEGANISTAO</asp:ListItem>
                <asp:ListItem Value="ZA">AFRICA DO SUL</asp:ListItem>
                <asp:ListItem Value="AL">ALBANIA</asp:ListItem>
                <asp:ListItem Value="DE">ALEMANHA</asp:ListItem>
                <asp:ListItem Value="AD">ANDORRA</asp:ListItem>
                <asp:ListItem Value="AO">ANGOLA</asp:ListItem>
                <asp:ListItem Value="AI">ANGUILA (TERRIT BRITANICO)</asp:ListItem>
                <asp:ListItem Value="AG">ANTIGUA E BARBUDA</asp:ListItem>
                <asp:ListItem Value="AN">ANTILHAS HOLANDESAS</asp:ListItem>
                <asp:ListItem Value="SA">ARABIA SAUDITA</asp:ListItem>
                <asp:ListItem Value="DZ">ARGELIA</asp:ListItem>
                <asp:ListItem Value="AR">ARGENTINA</asp:ListItem>
                <asp:ListItem Value="AM">ARMENIA</asp:ListItem>
                <asp:ListItem Value="AW">ARUBA (TER DA HOLANDA)</asp:ListItem>
                <asp:ListItem Value="SS">ASCENCAO</asp:ListItem>
                <asp:ListItem Value="AU">AUSTRALIA</asp:ListItem>
                <asp:ListItem Value="AT">AUSTRIA</asp:ListItem>
                <asp:ListItem Value="AZ">AZERBAIJÃO</asp:ListItem>
                <asp:ListItem Value="BS">BAHAMAS</asp:ListItem>
                <asp:ListItem Value="BH">BAHREIN</asp:ListItem>
                <asp:ListItem Value="BD">BANGLADESH</asp:ListItem>
                <asp:ListItem Value="BB">BARBADOS</asp:ListItem>
                <asp:ListItem Value="BY">BELARUS</asp:ListItem>
                <asp:ListItem Value="BE">BELGICA</asp:ListItem>
                <asp:ListItem Value="BZ">BELIZE</asp:ListItem>
                <asp:ListItem Value="BJ">BENIN</asp:ListItem>
                <asp:ListItem Value="BM">BERMUDAS</asp:ListItem>
                <asp:ListItem Value="BO">BOLIVIA</asp:ListItem>
                <asp:ListItem Value="BA">BOSNIA-HERZEGOVINA</asp:ListItem>
                <asp:ListItem Value="BW">BOTSUANA</asp:ListItem>
                <asp:ListItem Value="BR">BRASIL</asp:ListItem>
                <asp:ListItem Value="BN">BRUNEI</asp:ListItem>
                <asp:ListItem Value="BG">BULGARIA</asp:ListItem>
                <asp:ListItem Value="BF">BURKINA FASO</asp:ListItem>
                <asp:ListItem Value="BI">BURUNDI</asp:ListItem>
                <asp:ListItem Value="BT">BUTAO</asp:ListItem>
                <asp:ListItem Value="CV">CABO VERDE</asp:ListItem>
                <asp:ListItem Value="CM">CAMAROES</asp:ListItem>
                <asp:ListItem Value="KH">CAMBODJA</asp:ListItem>
                <asp:ListItem Value="CA">CANADA</asp:ListItem>
                <asp:ListItem Value="QA">CATAR</asp:ListItem>
                <asp:ListItem Value="KY">CAYMAN</asp:ListItem>
                <asp:ListItem Value="KZ">CAZAQUISTÃO</asp:ListItem>
                <asp:ListItem Value="CF">CENTRO AFRICANA (REPUBLICA)</asp:ListItem>
                <asp:ListItem Value="TD">CHADE</asp:ListItem>
                <asp:ListItem Value="CL">CHILE</asp:ListItem>
                <asp:ListItem Value="CN">CHINA</asp:ListItem>
                <asp:ListItem Value="CY">CHIPRE</asp:ListItem>
                <asp:ListItem Value="SG">CINGAPURA</asp:ListItem>
                <asp:ListItem Value="CC">COCOS-ILHAS(TERR DA AUSTRALIA)</asp:ListItem>
                <asp:ListItem Value="CO">COLOMBIA</asp:ListItem>
                <asp:ListItem Value="KM">COMORES</asp:ListItem>
                <asp:ListItem Value="KP">COREIA DO NORTE</asp:ListItem>
                <asp:ListItem Value="KR">COREIA DO SUL</asp:ListItem>
                <asp:ListItem Value="CI">COSTA DO MARFIM</asp:ListItem>
                <asp:ListItem Value="CR">COSTA RICA</asp:ListItem>
                <asp:ListItem Value="HR">CROACIA</asp:ListItem>
                <asp:ListItem Value="CU">CUBA</asp:ListItem>
                <asp:ListItem Value="DK">DINAMARCA</asp:ListItem>
                <asp:ListItem Value="DJ">DJIBUTI</asp:ListItem>
                <asp:ListItem Value="DM">DOMINICA</asp:ListItem>
                <asp:ListItem Value="DO">DOMINICANA</asp:ListItem>
                <asp:ListItem Value="EG">EGITO</asp:ListItem>
                <asp:ListItem Value="SV">EL SALVADOR</asp:ListItem>
                <asp:ListItem Value="AE">EMIRADOS ARABES UNIDOS</asp:ListItem>
                <asp:ListItem Value="EC">EQUADOR</asp:ListItem>
                <asp:ListItem Value="ER">ERITREIA</asp:ListItem>
                <asp:ListItem Value="SK">ESLOVAQUIA</asp:ListItem>
                <asp:ListItem Value="SI">ESLOVENIA</asp:ListItem>
                <asp:ListItem Value="ES">ESPANHA</asp:ListItem>
                <asp:ListItem Value="US">ESTADOS UNIDOS</asp:ListItem>
                <asp:ListItem Value="EE">ESTONIA</asp:ListItem>
                <asp:ListItem Value="ET">ETIOPIA</asp:ListItem>
                <asp:ListItem Value="FK">FALKLAND (TER BRITANICO)</asp:ListItem>
                <asp:ListItem Value="FO">FAROE</asp:ListItem>
                <asp:ListItem Value="FJ">FIJI</asp:ListItem>
                <asp:ListItem Value="PH">FILIPINAS</asp:ListItem>
                <asp:ListItem Value="FI">FINLANDIA</asp:ListItem>
                <asp:ListItem Value="FR">FRANÇA</asp:ListItem>
                <asp:ListItem Value="GA">GABAO</asp:ListItem>
                <asp:ListItem Value="GM">GAMBIA</asp:ListItem>
                <asp:ListItem Value="GH">GANA</asp:ListItem>
                <asp:ListItem Value="GE">GEORGIA</asp:ListItem>
                <asp:ListItem Value="GS">GEORGIA E SANDWICH SUL (GB)</asp:ListItem>
                <asp:ListItem Value="GI">GIBRALTAR (TER BRITANICO)</asp:ListItem>
                <asp:ListItem Value="GB">GRA-BRETANHA</asp:ListItem>
                <asp:ListItem Value="GD">GRANADA</asp:ListItem>
                <asp:ListItem Value="GR">GRECIA</asp:ListItem>
                <asp:ListItem Value="GL">GROENLANDIA(TERR DA DINAMARCA)</asp:ListItem>
                <asp:ListItem Value="GP">GUADALUPE (TERRIT FRANCES)</asp:ListItem>
                <asp:ListItem Value="GU">GUAM (TERR AMERICANO)</asp:ListItem>
                <asp:ListItem Value="GT">GUATEMALA</asp:ListItem>
                <asp:ListItem Value="GY">GUIANA</asp:ListItem>
                <asp:ListItem Value="GF">GUIANA FRANCESA</asp:ListItem>
                <asp:ListItem Value="GN">GUINE</asp:ListItem>
                <asp:ListItem Value="GW">GUINE BISSAU</asp:ListItem>
                <asp:ListItem Value="GQ">GUINE EQUATORIAL</asp:ListItem>
                <asp:ListItem Value="HT">HAITI</asp:ListItem>
                <asp:ListItem Value="HN">HONDURAS</asp:ListItem>
                <asp:ListItem Value="HK">HONG KONG</asp:ListItem>
                <asp:ListItem Value="HU">HUNGRIA</asp:ListItem>
                <asp:ListItem Value="YE">IEMEM</asp:ListItem>
                <asp:ListItem Value="CT">ILHAS CANARIAS E TENERIFE</asp:ListItem>
                <asp:ListItem Value="CX">ILHAS CHRISTMAS</asp:ListItem>
                <asp:ListItem Value="CK">ILHAS COOK</asp:ListItem>
                <asp:ListItem Value="MP">ILHAS MARIANAS</asp:ListItem>
                <asp:ListItem Value="MH">ILHAS MARSHALL</asp:ListItem>
                <asp:ListItem Value="MI">ILHAS MIDWAY (TER. AMERICANO)</asp:ListItem>
                <asp:ListItem Value="VI">ILHAS VIRGENS AMERICANAS</asp:ListItem>
                <asp:ListItem Value="WK">ILHAS WAKE</asp:ListItem>
                <asp:ListItem Value="IN">INDIA</asp:ListItem>
                <asp:ListItem Value="ID">INDONESIA</asp:ListItem>
                <asp:ListItem Value="IR">IRA</asp:ListItem>
                <asp:ListItem Value="IQ">IRAQUE</asp:ListItem>
                <asp:ListItem Value="IE">IRLANDA</asp:ListItem>
                <asp:ListItem Value="IS">ISLANDIA</asp:ListItem>
                <asp:ListItem Value="IL">ISRAEL</asp:ListItem>
                <asp:ListItem Value="IT">ITALIA</asp:ListItem>
                <asp:ListItem Value="YU">IUGOSLAVIA</asp:ListItem>
                <asp:ListItem Value="JM">JAMAICA</asp:ListItem>
                <asp:ListItem Value="JP">JAPAO</asp:ListItem>
                <asp:ListItem Value="JO">JORDANIA</asp:ListItem>
                <asp:ListItem Value="KI">KIRIBATI</asp:ListItem>
                <asp:ListItem Value="KW">KUWAIT</asp:ListItem>
                <asp:ListItem Value="LA">LAOS</asp:ListItem>
                <asp:ListItem Value="LS">LESOTO</asp:ListItem>
                <asp:ListItem Value="LV">LETONIA</asp:ListItem>
                <asp:ListItem Value="LB">LIBANO</asp:ListItem>
                <asp:ListItem Value="LR">LIBERIA</asp:ListItem>
                <asp:ListItem Value="LY">LIBIA</asp:ListItem>
                <asp:ListItem Value="LI">LIECHTENSTEIN</asp:ListItem>
                <asp:ListItem Value="LT">LITUANIA</asp:ListItem>
                <asp:ListItem Value="LU">LUXEMBURGO</asp:ListItem>
                <asp:ListItem Value="MO">MACAU</asp:ListItem>
                <asp:ListItem Value="MK">MACEDONIA</asp:ListItem>
                <asp:ListItem Value="MG">MADAGASCAR</asp:ListItem>
                <asp:ListItem Value="MY">MALÁSIA</asp:ListItem>
                <asp:ListItem Value="MW">MALAVI</asp:ListItem>
                <asp:ListItem Value="MV">MALDIVAS</asp:ListItem>
                <asp:ListItem Value="ML">MALI</asp:ListItem>
                <asp:ListItem Value="MT">MALTA</asp:ListItem>
                <asp:ListItem Value="MA">MARROCOS</asp:ListItem>
                <asp:ListItem Value="MQ">MARTINICA</asp:ListItem>
                <asp:ListItem Value="MU">MAURICIO</asp:ListItem>
                <asp:ListItem Value="MR">MAURITANIA</asp:ListItem>
                <asp:ListItem Value="YT">MAYOTTE</asp:ListItem>
                <asp:ListItem Value="MX">MEXICO</asp:ListItem>
                <asp:ListItem Value="MM">MIANMAR</asp:ListItem>
                <asp:ListItem Value="FM">MICRONESIA</asp:ListItem>
                <asp:ListItem Value="MZ">MOCAMBIQUE</asp:ListItem>
                <asp:ListItem Value="MD">MOLDAVIA</asp:ListItem>
                <asp:ListItem Value="MC">MONACO</asp:ListItem>
                <asp:ListItem Value="MN">MONGOLIA</asp:ListItem>
                <asp:ListItem Value="MS">MONTSERRAT</asp:ListItem>
                <asp:ListItem Value="NA">NAMIBIA</asp:ListItem>
                <asp:ListItem Value="NR">NAURU</asp:ListItem>
                <asp:ListItem Value="NP">NEPAL</asp:ListItem>
                <asp:ListItem Value="NI">NICARAGUA</asp:ListItem>
                <asp:ListItem Value="NE">NIGER</asp:ListItem>
                <asp:ListItem Value="NG">NIGERIA</asp:ListItem>
                <asp:ListItem Value="NU">NIUE</asp:ListItem>
                <asp:ListItem Value="NF">NORFOLK</asp:ListItem>
                <asp:ListItem Value="NO">NORUEGA</asp:ListItem>
                <asp:ListItem Value="NC">NOVA CALEDONIA</asp:ListItem>
                <asp:ListItem Value="NZ">NOVA ZELANDIA</asp:ListItem>
                <asp:ListItem Value="OM">OMA</asp:ListItem>
                <asp:ListItem Value="NL">PAISES BAIXOS</asp:ListItem>
                <asp:ListItem Value="PW">PALAU</asp:ListItem>
                <asp:ListItem Value="PA">PANAMA</asp:ListItem>
                <asp:ListItem Value="PG">PAPUA-NOVA GUINE</asp:ListItem>
                <asp:ListItem Value="PK">PAQUISTAO</asp:ListItem>
                <asp:ListItem Value="PY">PARAGUAI</asp:ListItem>
                <asp:ListItem Value="PE">PERU</asp:ListItem>
                <asp:ListItem Value="PN">PITCAIRN</asp:ListItem>
                <asp:ListItem Value="PF">POLINESIA FRANCESA</asp:ListItem>
                <asp:ListItem Value="PL">POLONIA</asp:ListItem>
                <asp:ListItem Value="PR">PORTO RICO</asp:ListItem>
                <asp:ListItem Value="PT">PORTUGAL</asp:ListItem>
                <asp:ListItem Value="KE">QUENIA</asp:ListItem>
                <asp:ListItem Value="KG">QUIRGUISTAO</asp:ListItem>
                <asp:ListItem Value="CD">REP. DEMOC. DO CONGO</asp:ListItem>
                <asp:ListItem Value="CG">REP. POPULAR DO CONGO</asp:ListItem>
                <asp:ListItem Value="RE">REUNIÃO</asp:ListItem>
                <asp:ListItem Value="RO">ROMENIA</asp:ListItem>
                <asp:ListItem Value="RW">RUANDA</asp:ListItem>
                <asp:ListItem Value="RU">RUSSIA</asp:ListItem>
                <asp:ListItem Value="PM">SAINT PIERRE E MIQUELON</asp:ListItem>
                <asp:ListItem Value="SB">SALOMAO</asp:ListItem>
                <asp:ListItem Value="WS">SAMOA</asp:ListItem>
                <asp:ListItem Value="AS">SAMOA AMERICANA</asp:ListItem>
                <asp:ListItem Value="SM">SAN MARINO</asp:ListItem>
                <asp:ListItem Value="SH">SANTA HELENA</asp:ListItem>
                <asp:ListItem Value="LC">SANTA LUCIA</asp:ListItem>
                <asp:ListItem Value="KN">SAO CRISTOVAO E NEVIS</asp:ListItem>
                <asp:ListItem Value="ST">SAO TOME E PRINCIPE</asp:ListItem>
                <asp:ListItem Value="VC">SAO VICENTE E GRANADINAS</asp:ListItem>
                <asp:ListItem Value="SN">SENEGAL</asp:ListItem>
                <asp:ListItem Value="SL">SERRA LEOA</asp:ListItem>
                <asp:ListItem Value="SC">SEYCHELES</asp:ListItem>
                <asp:ListItem Value="SY">SIRIA</asp:ListItem>
                <asp:ListItem Value="SO">SOMALIA</asp:ListItem>
                <asp:ListItem Value="LK">SRI LANKA</asp:ListItem>
                <asp:ListItem Value="SZ">SUAZILANDIA</asp:ListItem>
                <asp:ListItem Value="SD">SUDAO</asp:ListItem>
                <asp:ListItem Value="SE">SUECIA</asp:ListItem>
                <asp:ListItem Value="CH">SUÍÇA</asp:ListItem>
                <asp:ListItem Value="SR">SURINAME</asp:ListItem>
                <asp:ListItem Value="TJ">TADJIQUISTAO</asp:ListItem>
                <asp:ListItem Value="TH">TAILANDIA</asp:ListItem>
                <asp:ListItem Value="TW">TAIWAN</asp:ListItem>
                <asp:ListItem Value="TZ">TANZANIA</asp:ListItem>
                <asp:ListItem Value="CZ">TCHECA</asp:ListItem>
                <asp:ListItem Value="TP">TIMOR ORIENTAL</asp:ListItem>
                <asp:ListItem Value="TG">TOGO</asp:ListItem>
                <asp:ListItem Value="TO">TONGA</asp:ListItem>
                <asp:ListItem Value="TT">TRINIDAD E TOBAGO</asp:ListItem>
                <asp:ListItem Value="TI">TRISTAO DA CUNHA</asp:ListItem>
                <asp:ListItem Value="TN">TUNISIA</asp:ListItem>
                <asp:ListItem Value="TC">TURCKS E CAICOS</asp:ListItem>
                <asp:ListItem Value="TM">TURCOMENISTAO</asp:ListItem>
                <asp:ListItem Value="TR">TURQUIA</asp:ListItem>
                <asp:ListItem Value="TV">TUVALU</asp:ListItem>
                <asp:ListItem Value="UA">UCRANIA</asp:ListItem>
                <asp:ListItem Value="UG">UGANDA</asp:ListItem>
                <asp:ListItem Value="UY">URUGUAI</asp:ListItem>
                <asp:ListItem Value="UZ">UZBEQUISTAO</asp:ListItem>
                <asp:ListItem Value="VU">VANUATU</asp:ListItem>
                <asp:ListItem Value="VA">VATICANO</asp:ListItem>
                <asp:ListItem Value="VE">VENEZUELA</asp:ListItem>
                <asp:ListItem Value="VN">VIETNA</asp:ListItem>
                <asp:ListItem Value="VG">VIRGENS BRITÂNICAS</asp:ListItem>
                <asp:ListItem Value="WF">WALLIS E FUTUNA</asp:ListItem>
                <asp:ListItem Value="ZM">ZAMBIA</asp:ListItem>
                <asp:ListItem Value="ZW">ZIMBABUE</asp:ListItem>
            </asp:DropDownList></td></tr>
</table>

<table id="tblEnderecoEntrega" runat="server" border="0" cellpadding="0" cellspacing="4" width="100%" visible="false">
    <tr><td class="fillRowTitle" colspan="2" valign="top"><asp:Label ID="label27" runat="server" meta:resourcekey="label27Resource1" Text="Endereço de Entrega" />
        <asp:PlaceHolder ID="plhEnderecoEntregaInstruction" runat="server"></asp:PlaceHolder>
    </td><td align="right"></td></tr>
    <tr><td class="fillLabel">Endereço (*)</td><td><asp:DropDownList ID="ddlLogradouroE" runat="server" meta:resourcekey="ddlLogradouroEResource1">
            <asp:ListItem meta:resourcekey="ListItemResource16"></asp:ListItem>
            <asp:ListItem meta:resourcekey="ListItemResource17" Text="Rua"></asp:ListItem>
            <asp:ListItem meta:resourcekey="ListItemResource18" Text="Avenida"></asp:ListItem>
            <asp:ListItem meta:resourcekey="ListItemResource19" Text="Travessa"></asp:ListItem>
            <asp:ListItem meta:resourcekey="ListItemResource20" Text="Pra&#231;a"></asp:ListItem>
            <asp:ListItem meta:resourcekey="ListItemResource21" Text="Alameda"></asp:ListItem>
            <asp:ListItem meta:resourcekey="ListItemResource22" Text="Quadra"></asp:ListItem>
            <asp:ListItem meta:resourcekey="ListItemResource23" Text="Bloco"></asp:ListItem>
            <asp:ListItem meta:resourcekey="ListItemResource24" Text="Outro"></asp:ListItem>
        </asp:DropDownList></td></tr>
    <tr>
        <td class="fillLabel"></td>
        <td class="fillLabel"><asp:TextBox ID="txtEnderecoE" runat="server" Columns="40" MaxLength="130" meta:resourcekey="txtEnderecoEResource1"/> 
            <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="txtEnderecoE"
                ErrorMessage="*"></asp:RequiredFieldValidator>
            Nº <asp:TextBox ID="txtNumeroE" runat="server" Columns="7" MaxLength="7" meta:resourcekey="txtNumeroEResource1"/> 
            <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ControlToValidate="txtNumeroE"
                ErrorMessage="*"></asp:RequiredFieldValidator>
        </td></tr>
    <tr>
        <td class="fillLabel"><asp:Label ID="label28" runat="server" meta:resourcekey="label28Resource1" Text="Complemento" /></td>
        <td><asp:TextBox ID="txtComplementoE" runat="server" Columns="40" MaxLength="40" meta:resourcekey="txtComplementoEResource1"/></td></tr>
    <tr>
        <td class="fillLabel"><asp:Label ID="label29" runat="server" meta:resourcekey="label29Resource1" Text="Bairro" /></td>
        <td><asp:TextBox ID="txtBairroE" runat="server" Columns="40" MaxLength="40" meta:resourcekey="txtBairroEResource1"/></td></tr>
    <tr>
        <td class="fillLabel"><asp:Label ID="label30" runat="server" meta:resourcekey="label30Resource1" Text="Cidade (*)" /></td>
        <td><asp:TextBox ID="txtCidadeE" runat="server" Columns="40" MaxLength="40" meta:resourcekey="txtCidadeEResource1"/> 
            <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ControlToValidate="txtCidadeE"
                ErrorMessage="*"></asp:RequiredFieldValidator>
        </td></tr>
    <tr>
        <td class="fillLabel"><asp:Label ID="label31" runat="server" meta:resourcekey="label31Resource1" Text="CEP (*)" /></td>
        <td><asp:TextBox ID="txtCEPE" runat="server" MaxLength="5" Columns="5" meta:resourcekey="txtCEPEResource1"/> 
            <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ControlToValidate="txtCEPE"
                ErrorMessage="*"></asp:RequiredFieldValidator>
            - <asp:TextBox ID="txtCEPE2" runat="server" Columns="3" MaxLength="3" meta:resourcekey="txtCEPE2Resource1"/> 
            <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ControlToValidate="txtCEPE2"
                ErrorMessage="*"></asp:RequiredFieldValidator>
            <asp:Label ID="Label39" Text="(5 - 3 dígitos)" CssClass="fillInstructions" runat="server"/></td></tr>
    <tr>
        <td class="fillLabel"><asp:Label ID="label32" runat="server" meta:resourcekey="label32Resource1" Text="Estado (*)" /></td>
        <td><asp:TextBox ID="txtEstadoE" runat="server" Columns="40" MaxLength="40" meta:resourcekey="txtEstadoEResource1"/>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ControlToValidate="txtEstadoE"
                ErrorMessage="*"></asp:RequiredFieldValidator></td></tr>
    <tr>
        <td class="fillLabel"><asp:Label ID="label33" runat="server" meta:resourcekey="label33Resource1" Text="País" /></td>
        <td>
            <asp:DropDownList ID="ddlPaisE" runat="server">
                <asp:ListItem Value=""></asp:ListItem>
				<asp:ListItem Value="AC">ACORES E MADEIRA</asp:ListItem>
				<asp:ListItem Value="AF">AFEGANISTAO</asp:ListItem>
				<asp:ListItem Value="ZA">AFRICA DO SUL</asp:ListItem>
				<asp:ListItem Value="AL">ALBANIA</asp:ListItem>
				<asp:ListItem Value="DE">ALEMANHA</asp:ListItem>
				<asp:ListItem Value="AD">ANDORRA</asp:ListItem>
				<asp:ListItem Value="AO">ANGOLA</asp:ListItem>
				<asp:ListItem Value="AI">ANGUILA (TERRIT BRITANICO)</asp:ListItem>
				<asp:ListItem Value="AG">ANTIGUA E BARBUDA</asp:ListItem>
				<asp:ListItem Value="AN">ANTILHAS HOLANDESAS</asp:ListItem>
				<asp:ListItem Value="SA">ARABIA SAUDITA</asp:ListItem>
				<asp:ListItem Value="DZ">ARGELIA</asp:ListItem>
				<asp:ListItem Value="AR">ARGENTINA</asp:ListItem>
				<asp:ListItem Value="AM">ARMENIA</asp:ListItem>
				<asp:ListItem Value="AW">ARUBA (TER DA HOLANDA)</asp:ListItem>
				<asp:ListItem Value="SS">ASCENCAO</asp:ListItem>
				<asp:ListItem Value="AU">AUSTRALIA</asp:ListItem>
				<asp:ListItem Value="AT">AUSTRIA</asp:ListItem>
				<asp:ListItem Value="AZ">AZERBAIJÃO</asp:ListItem>
				<asp:ListItem Value="BS">BAHAMAS</asp:ListItem>
				<asp:ListItem Value="BH">BAHREIN</asp:ListItem>
				<asp:ListItem Value="BD">BANGLADESH</asp:ListItem>
				<asp:ListItem Value="BB">BARBADOS</asp:ListItem>
				<asp:ListItem Value="BY">BELARUS</asp:ListItem>
				<asp:ListItem Value="BE">BELGICA</asp:ListItem>
				<asp:ListItem Value="BZ">BELIZE</asp:ListItem>
				<asp:ListItem Value="BJ">BENIN</asp:ListItem>
				<asp:ListItem Value="BM">BERMUDAS</asp:ListItem>
				<asp:ListItem Value="BO">BOLIVIA</asp:ListItem>
				<asp:ListItem Value="BA">BOSNIA-HERZEGOVINA</asp:ListItem>
				<asp:ListItem Value="BW">BOTSUANA</asp:ListItem>
				<asp:ListItem Value="BR">BRASIL</asp:ListItem>
				<asp:ListItem Value="BN">BRUNEI</asp:ListItem>
				<asp:ListItem Value="BG">BULGARIA</asp:ListItem>
				<asp:ListItem Value="BF">BURKINA FASO</asp:ListItem>
				<asp:ListItem Value="BI">BURUNDI</asp:ListItem>
				<asp:ListItem Value="BT">BUTAO</asp:ListItem>
				<asp:ListItem Value="CV">CABO VERDE</asp:ListItem>
				<asp:ListItem Value="CM">CAMAROES</asp:ListItem>
				<asp:ListItem Value="KH">CAMBODJA</asp:ListItem>
				<asp:ListItem Value="CA">CANADA</asp:ListItem>
				<asp:ListItem Value="QA">CATAR</asp:ListItem>
				<asp:ListItem Value="KY">CAYMAN</asp:ListItem>
				<asp:ListItem Value="KZ">CAZAQUISTÃO</asp:ListItem>
				<asp:ListItem Value="CF">CENTRO AFRICANA (REPUBLICA)</asp:ListItem>
				<asp:ListItem Value="TD">CHADE</asp:ListItem>
				<asp:ListItem Value="CL">CHILE</asp:ListItem>
				<asp:ListItem Value="CN">CHINA</asp:ListItem>
				<asp:ListItem Value="CY">CHIPRE</asp:ListItem>
				<asp:ListItem Value="SG">CINGAPURA</asp:ListItem>
				<asp:ListItem Value="CC">COCOS-ILHAS(TERR DA AUSTRALIA)</asp:ListItem>
				<asp:ListItem Value="CO">COLOMBIA</asp:ListItem>
				<asp:ListItem Value="KM">COMORES</asp:ListItem>
				<asp:ListItem Value="KP">COREIA DO NORTE</asp:ListItem>
				<asp:ListItem Value="KR">COREIA DO SUL</asp:ListItem>
				<asp:ListItem Value="CI">COSTA DO MARFIM</asp:ListItem>
				<asp:ListItem Value="CR">COSTA RICA</asp:ListItem>
				<asp:ListItem Value="HR">CROACIA</asp:ListItem>
				<asp:ListItem Value="CU">CUBA</asp:ListItem>
				<asp:ListItem Value="DK">DINAMARCA</asp:ListItem>
				<asp:ListItem Value="DJ">DJIBUTI</asp:ListItem>
				<asp:ListItem Value="DM">DOMINICA</asp:ListItem>
				<asp:ListItem Value="DO">DOMINICANA</asp:ListItem>
				<asp:ListItem Value="EG">EGITO</asp:ListItem>
				<asp:ListItem Value="SV">EL SALVADOR</asp:ListItem>
				<asp:ListItem Value="AE">EMIRADOS ARABES UNIDOS</asp:ListItem>
				<asp:ListItem Value="EC">EQUADOR</asp:ListItem>
				<asp:ListItem Value="ER">ERITREIA</asp:ListItem>
				<asp:ListItem Value="SK">ESLOVAQUIA</asp:ListItem>
				<asp:ListItem Value="SI">ESLOVENIA</asp:ListItem>
				<asp:ListItem Value="ES">ESPANHA</asp:ListItem>
				<asp:ListItem Value="US">ESTADOS UNIDOS</asp:ListItem>
				<asp:ListItem Value="EE">ESTONIA</asp:ListItem>
				<asp:ListItem Value="ET">ETIOPIA</asp:ListItem>
				<asp:ListItem Value="FK">FALKLAND (TER BRITANICO)</asp:ListItem>
				<asp:ListItem Value="FO">FAROE</asp:ListItem>
				<asp:ListItem Value="FJ">FIJI</asp:ListItem>
				<asp:ListItem Value="PH">FILIPINAS</asp:ListItem>
				<asp:ListItem Value="FI">FINLANDIA</asp:ListItem>
				<asp:ListItem Value="FR">FRANÇA</asp:ListItem>
				<asp:ListItem Value="GA">GABAO</asp:ListItem>
				<asp:ListItem Value="GM">GAMBIA</asp:ListItem>
				<asp:ListItem Value="GH">GANA</asp:ListItem>
				<asp:ListItem Value="GE">GEORGIA</asp:ListItem>
				<asp:ListItem Value="GS">GEORGIA E SANDWICH SUL (GB)</asp:ListItem>
				<asp:ListItem Value="GI">GIBRALTAR (TER BRITANICO)</asp:ListItem>
				<asp:ListItem Value="GB">GRA-BRETANHA</asp:ListItem>
				<asp:ListItem Value="GD">GRANADA</asp:ListItem>
				<asp:ListItem Value="GR">GRECIA</asp:ListItem>
				<asp:ListItem Value="GL">GROENLANDIA(TERR DA DINAMARCA)</asp:ListItem>
				<asp:ListItem Value="GP">GUADALUPE (TERRIT FRANCES)</asp:ListItem>
				<asp:ListItem Value="GU">GUAM (TERR AMERICANO)</asp:ListItem>
				<asp:ListItem Value="GT">GUATEMALA</asp:ListItem>
				<asp:ListItem Value="GY">GUIANA</asp:ListItem>
				<asp:ListItem Value="GF">GUIANA FRANCESA</asp:ListItem>
				<asp:ListItem Value="GN">GUINE</asp:ListItem>
				<asp:ListItem Value="GW">GUINE BISSAU</asp:ListItem>
				<asp:ListItem Value="GQ">GUINE EQUATORIAL</asp:ListItem>
				<asp:ListItem Value="HT">HAITI</asp:ListItem>
				<asp:ListItem Value="HN">HONDURAS</asp:ListItem>
				<asp:ListItem Value="HK">HONG KONG</asp:ListItem>
				<asp:ListItem Value="HU">HUNGRIA</asp:ListItem>
				<asp:ListItem Value="YE">IEMEM</asp:ListItem>
				<asp:ListItem Value="CT">ILHAS CANARIAS E TENERIFE</asp:ListItem>
				<asp:ListItem Value="CX">ILHAS CHRISTMAS</asp:ListItem>
				<asp:ListItem Value="CK">ILHAS COOK</asp:ListItem>
				<asp:ListItem Value="MP">ILHAS MARIANAS</asp:ListItem>
				<asp:ListItem Value="MH">ILHAS MARSHALL</asp:ListItem>
				<asp:ListItem Value="MI">ILHAS MIDWAY (TER. AMERICANO)</asp:ListItem>
				<asp:ListItem Value="VI">ILHAS VIRGENS AMERICANAS</asp:ListItem>
				<asp:ListItem Value="WK">ILHAS WAKE</asp:ListItem>
				<asp:ListItem Value="IN">INDIA</asp:ListItem>
				<asp:ListItem Value="ID">INDONESIA</asp:ListItem>
				<asp:ListItem Value="IR">IRA</asp:ListItem>
				<asp:ListItem Value="IQ">IRAQUE</asp:ListItem>
				<asp:ListItem Value="IE">IRLANDA</asp:ListItem>
				<asp:ListItem Value="IS">ISLANDIA</asp:ListItem>
				<asp:ListItem Value="IL">ISRAEL</asp:ListItem>
				<asp:ListItem Value="IT">ITALIA</asp:ListItem>
				<asp:ListItem Value="YU">IUGOSLAVIA</asp:ListItem>
				<asp:ListItem Value="JM">JAMAICA</asp:ListItem>
				<asp:ListItem Value="JP">JAPAO</asp:ListItem>
				<asp:ListItem Value="JO">JORDANIA</asp:ListItem>
				<asp:ListItem Value="KI">KIRIBATI</asp:ListItem>
				<asp:ListItem Value="KW">KUWAIT</asp:ListItem>
				<asp:ListItem Value="LA">LAOS</asp:ListItem>
				<asp:ListItem Value="LS">LESOTO</asp:ListItem>
				<asp:ListItem Value="LV">LETONIA</asp:ListItem>
				<asp:ListItem Value="LB">LIBANO</asp:ListItem>
				<asp:ListItem Value="LR">LIBERIA</asp:ListItem>
				<asp:ListItem Value="LY">LIBIA</asp:ListItem>
				<asp:ListItem Value="LI">LIECHTENSTEIN</asp:ListItem>
				<asp:ListItem Value="LT">LITUANIA</asp:ListItem>
				<asp:ListItem Value="LU">LUXEMBURGO</asp:ListItem>
				<asp:ListItem Value="MO">MACAU</asp:ListItem>
				<asp:ListItem Value="MK">MACEDONIA</asp:ListItem>
				<asp:ListItem Value="MG">MADAGASCAR</asp:ListItem>
				<asp:ListItem Value="MY">MALÁSIA</asp:ListItem>
				<asp:ListItem Value="MW">MALAVI</asp:ListItem>
				<asp:ListItem Value="MV">MALDIVAS</asp:ListItem>
				<asp:ListItem Value="ML">MALI</asp:ListItem>
				<asp:ListItem Value="MT">MALTA</asp:ListItem>
				<asp:ListItem Value="MA">MARROCOS</asp:ListItem>
				<asp:ListItem Value="MQ">MARTINICA</asp:ListItem>
				<asp:ListItem Value="MU">MAURICIO</asp:ListItem>
				<asp:ListItem Value="MR">MAURITANIA</asp:ListItem>
				<asp:ListItem Value="YT">MAYOTTE</asp:ListItem>
				<asp:ListItem Value="MX">MEXICO</asp:ListItem>
				<asp:ListItem Value="MM">MIANMAR</asp:ListItem>
				<asp:ListItem Value="FM">MICRONESIA</asp:ListItem>
				<asp:ListItem Value="MZ">MOCAMBIQUE</asp:ListItem>
				<asp:ListItem Value="MD">MOLDAVIA</asp:ListItem>
				<asp:ListItem Value="MC">MONACO</asp:ListItem>
				<asp:ListItem Value="MN">MONGOLIA</asp:ListItem>
				<asp:ListItem Value="MS">MONTSERRAT</asp:ListItem>
				<asp:ListItem Value="NA">NAMIBIA</asp:ListItem>
				<asp:ListItem Value="NR">NAURU</asp:ListItem>
				<asp:ListItem Value="NP">NEPAL</asp:ListItem>
				<asp:ListItem Value="NI">NICARAGUA</asp:ListItem>
				<asp:ListItem Value="NE">NIGER</asp:ListItem>
				<asp:ListItem Value="NG">NIGERIA</asp:ListItem>
				<asp:ListItem Value="NU">NIUE</asp:ListItem>
				<asp:ListItem Value="NF">NORFOLK</asp:ListItem>
				<asp:ListItem Value="NO">NORUEGA</asp:ListItem>
				<asp:ListItem Value="NC">NOVA CALEDONIA</asp:ListItem>
				<asp:ListItem Value="NZ">NOVA ZELANDIA</asp:ListItem>
				<asp:ListItem Value="OM">OMA</asp:ListItem>
				<asp:ListItem Value="NL">PAISES BAIXOS</asp:ListItem>
				<asp:ListItem Value="PW">PALAU</asp:ListItem>
				<asp:ListItem Value="PA">PANAMA</asp:ListItem>
				<asp:ListItem Value="PG">PAPUA-NOVA GUINE</asp:ListItem>
				<asp:ListItem Value="PK">PAQUISTAO</asp:ListItem>
				<asp:ListItem Value="PY">PARAGUAI</asp:ListItem>
				<asp:ListItem Value="PE">PERU</asp:ListItem>
				<asp:ListItem Value="PN">PITCAIRN</asp:ListItem>
				<asp:ListItem Value="PF">POLINESIA FRANCESA</asp:ListItem>
				<asp:ListItem Value="PL">POLONIA</asp:ListItem>
				<asp:ListItem Value="PR">PORTO RICO</asp:ListItem>
				<asp:ListItem Value="PT">PORTUGAL</asp:ListItem>
				<asp:ListItem Value="KE">QUENIA</asp:ListItem>
				<asp:ListItem Value="KG">QUIRGUISTAO</asp:ListItem>
				<asp:ListItem Value="CD">REP. DEMOC. DO CONGO</asp:ListItem>
				<asp:ListItem Value="CG">REP. POPULAR DO CONGO</asp:ListItem>
				<asp:ListItem Value="RE">REUNIÃO</asp:ListItem>
				<asp:ListItem Value="RO">ROMENIA</asp:ListItem>
				<asp:ListItem Value="RW">RUANDA</asp:ListItem>
				<asp:ListItem Value="RU">RUSSIA</asp:ListItem>
				<asp:ListItem Value="PM">SAINT PIERRE E MIQUELON</asp:ListItem>
				<asp:ListItem Value="SB">SALOMAO</asp:ListItem>
				<asp:ListItem Value="WS">SAMOA</asp:ListItem>
				<asp:ListItem Value="AS">SAMOA AMERICANA</asp:ListItem>
				<asp:ListItem Value="SM">SAN MARINO</asp:ListItem>
				<asp:ListItem Value="SH">SANTA HELENA</asp:ListItem>
				<asp:ListItem Value="LC">SANTA LUCIA</asp:ListItem>
				<asp:ListItem Value="KN">SAO CRISTOVAO E NEVIS</asp:ListItem>
				<asp:ListItem Value="ST">SAO TOME E PRINCIPE</asp:ListItem>
				<asp:ListItem Value="VC">SAO VICENTE E GRANADINAS</asp:ListItem>
				<asp:ListItem Value="SN">SENEGAL</asp:ListItem>
				<asp:ListItem Value="SL">SERRA LEOA</asp:ListItem>
				<asp:ListItem Value="SC">SEYCHELES</asp:ListItem>
				<asp:ListItem Value="SY">SIRIA</asp:ListItem>
				<asp:ListItem Value="SO">SOMALIA</asp:ListItem>
				<asp:ListItem Value="LK">SRI LANKA</asp:ListItem>
				<asp:ListItem Value="SZ">SUAZILANDIA</asp:ListItem>
				<asp:ListItem Value="SD">SUDAO</asp:ListItem>
				<asp:ListItem Value="SE">SUECIA</asp:ListItem>
				<asp:ListItem Value="CH">SUÍÇA</asp:ListItem>
				<asp:ListItem Value="SR">SURINAME</asp:ListItem>
				<asp:ListItem Value="TJ">TADJIQUISTAO</asp:ListItem>
				<asp:ListItem Value="TH">TAILANDIA</asp:ListItem>
				<asp:ListItem Value="TW">TAIWAN</asp:ListItem>
				<asp:ListItem Value="TZ">TANZANIA</asp:ListItem>
				<asp:ListItem Value="CZ">TCHECA</asp:ListItem>
				<asp:ListItem Value="TP">TIMOR ORIENTAL</asp:ListItem>
				<asp:ListItem Value="TG">TOGO</asp:ListItem>
				<asp:ListItem Value="TO">TONGA</asp:ListItem>
				<asp:ListItem Value="TT">TRINIDAD E TOBAGO</asp:ListItem>
				<asp:ListItem Value="TI">TRISTAO DA CUNHA</asp:ListItem>
				<asp:ListItem Value="TN">TUNISIA</asp:ListItem>
				<asp:ListItem Value="TC">TURCKS E CAICOS</asp:ListItem>
				<asp:ListItem Value="TM">TURCOMENISTAO</asp:ListItem>
				<asp:ListItem Value="TR">TURQUIA</asp:ListItem>
				<asp:ListItem Value="TV">TUVALU</asp:ListItem>
				<asp:ListItem Value="UA">UCRANIA</asp:ListItem>
				<asp:ListItem Value="UG">UGANDA</asp:ListItem>
				<asp:ListItem Value="UY">URUGUAI</asp:ListItem>
				<asp:ListItem Value="UZ">UZBEQUISTAO</asp:ListItem>
				<asp:ListItem Value="VU">VANUATU</asp:ListItem>
				<asp:ListItem Value="VA">VATICANO</asp:ListItem>
				<asp:ListItem Value="VE">VENEZUELA</asp:ListItem>
				<asp:ListItem Value="VN">VIETNA</asp:ListItem>
				<asp:ListItem Value="VG">VIRGENS BRITÂNICAS</asp:ListItem>
				<asp:ListItem Value="WF">WALLIS E FUTUNA</asp:ListItem>
				<asp:ListItem Value="ZM">ZAMBIA</asp:ListItem>
				<asp:ListItem Value="ZW">ZIMBABUE</asp:ListItem>
            </asp:DropDownList></td></tr>
</table>

<div class="buttonbtnNext">
<asp:ImageButton id="btnNext" onclick="btnNext_Click" runat="server" ImageUrl="~/Store/default/images/continuar.gif" meta:resourcekey="btnNextResource1"></asp:ImageButton>
</div>

<br />
<br />

<div class="buttonlnkReturn"><asp:HyperLink id="lnkReturn" runat="server" ImageUrl="~/Store/default/images/retornarloja.gif" NavigateUrl="#" Visible="false" Target="_parent">Retornar à Loja</asp:HyperLink></div>