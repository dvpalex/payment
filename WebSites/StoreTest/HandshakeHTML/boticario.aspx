<%@ Page Language="C#" AutoEventWireup="true" %>
<html>
<head runat="server">
    <title>POST BOTICARIO</title>
</head>
<body>
    <form id="form1" runat="server">
    </form>

    <form name="sendorder" id="sendorder" action="<%=System.Configuration.ConfigurationManager.AppSettings["urlHandshakeHTML"]%>?idioma=<%=Request["IDIOMA"]%>" method="post">
        <input type="hidden" name="5DED746B8F924F2E" value="<%=Request["5DED746B8F924F2E"]%>" />
        <input type="hidden" name="91D4C3128BF7DA7F" value="<%=Request["91D4C3128BF7DA7F"]%>" />
        <input type="hidden" value="1" name="VALIDA_KEY" />
        
        <input type="hidden" value="1000" name="SPV" />
        <input type="hidden" value="0" name="SFRETE" />
        
        <input type="hidden" value="2" name="SITEM" />
        <input type="hidden" value="1" name="QTD_1" />
        <input type="hidden" value="22P9272" name="COD_1" />
        <input type="hidden" value="Memria 1GB PC3200 400MHz DDR-SDRAM" name="DES_1" />
        <input type="hidden" value="999999999900" name="VAL_1" />
        
        <input type="hidden" value="1" name="QTD_2" />
        <input type="hidden" value="DISCOUNT" name="COD_2" />
        <input type="hidden" value="Cart Discount" name="DES_2" />
        <input type="hidden" value="-999999999800" name="VAL_2" />
        
        <input type="hidden" value="SIDNEY LOPES DE ARAUJO" name="NOME" />
        <input type="hidden" value="03465409922" name="CPF" />
        <input type="hidden" value="marcos.sampaio.ext@tivit.com.br" name="EMAIL" />
        <input type="hidden" value="4133817431" name="FONE" />
        <input type="hidden" value="M" name="SEXO" />
        <input type="hidden" value="01021981" name="NASC" />
        <input type="hidden" name="PROF" />
        <input type="hidden" value="AV" name="LOGRADOURO" />
        <input type="hidden" value="CDOR FRANCO" name="ENDERECO" />
        <input type="hidden" value="3041" name="NUMERO" />
        <input type="hidden" name="COMPLEMENTO" />
        <input type="hidden" value="GUABIROTUBA" name="BAIRRO" />
        <input type="hidden" value="CURITIBA" name="CIDADE" />
        <input type="hidden" value="81520000" name="CEP" />
        <input type="hidden" value="PR" name="ESTADO" />
        <input type="hidden" value="US" name="PAIS" />
        <input type="hidden" value="AV" name="LOGRADOURO_D" />
        <input type="hidden" value="CDOR FRANCO" name="ENDERECO_D" />
        <input type="hidden" value="3041" name="NUMERO_D" />
        <input type="hidden" name="COMPLEMENTO_D" />
        <input type="hidden" value="GUABIROTUBA" name="BAIRRO_D" />
        <input type="hidden" value="CURITIBA" name="CIDADE_D" />
        <input type="hidden" value="81520000" name="CEP_D" />
        <input type="hidden" value="PR" name="ESTADO_D" />
        <input type="hidden" value="BR" name="PAIS_D" />

        <input type="hidden" value="Realizar o pagamento até a data de vencimento. Após esta data, realize um novo pedido no site ou solicite maiores informações através de nosso SAC - Serviço de Atendimento ao Cliente pelo telefone 0800-413011." name="INSTRUCAO_BOLETO" />

        <input type="hidden" name="SIMULACAO" value="0" />
        
        <input type="hidden" name="PPAGAMENTO" value="" />
        <input type="hidden" name="BANDEIRA" value="" />
        
        <input type="hidden" name="VAL_ENTRADA_ABN" value="" />
        <input type="hidden" name="VAL_PARCELA_ABN" value="100" />
        <input type="hidden" name="DAT_VENCIMENTO1_ABN" value="29/09/2006" />
        <input type="hidden" name="QtdParcelas_ABN" value="4" />
        
        <input type="hidden" name="ENVIA_EMAIL_CLIENTE" value="0" />
        <input type="hidden" name="SHOW_TELA_FINALIZACAO" value="1" />
        <input type="hidden" name="TIPO_ACAO" value="1,2,3,8,9,10" />
    </form>
    <script type="text/javascript">
        document.sendorder.submit();
    </script>
</body>
</html>
