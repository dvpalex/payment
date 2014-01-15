<%@ Page Language="C#" %>

<html>
<head id="Head1" runat="server">
    <title>POST HTML 1 passo</title>
</head>
<body>
    <form id="form1" runat="server">
    </form>

    <form name="sendorder" id="sendorder" action="<%=System.Configuration.ConfigurationManager.AppSettings["urlHandshakeHTML"]%>?idioma=<%=Request["IDIOMA"]%>" method="post" target="<%=System.Configuration.ConfigurationManager.AppSettings["target"]%>">
                  
        <input type="hidden" name="5DED746B8F924F2E" value="<%=Request["5DED746B8F924F2E"]%>" />
        
        <input type="hidden" name="91D4C3128BF7DA7F" value="<%=Request["91D4C3128BF7DA7F"]%>" />
        <input type="hidden" value="1" name="VALIDA_KEY" />
        <input type="hidden" name="bandeira" value="" />
        <input type="hidden" name="pqtdparcelas" value="1" />
        <input type="hidden" name="ppagamento" value="" />
        <input type="hidden" name="state" value="smartpagthankyou" />
        <input type="hidden" name="spv" value="200" />
        <input type="hidden" name="sfrete" value="0" />
        <input type="hidden" name="qtd_1" value="3" />
        <input type="hidden" name="cod_1" value="9645Q1P00" />
        <input type="hidden" name="des_1" value="ThinkCentre M55e" />
        <input type="hidden" name="val_1" value="339069" />
        <input type="hidden" name="qtd_2" value="1" />
        <input type="hidden" name="cod_2" value="DISCOUNT" />
        <input type="hidden" name="des_2" value="Cart Discount" />
        <input type="hidden" name="val_2" value="-807507" />
        <input type="hidden" name="sitem" value="2" />
        <input type="hidden" name="show_tela_finalizacao" value="0" />
        <input type="hidden" name="envia_email_cliente" value="0" />
        <input type="hidden" name="frame50" value="0" />
        <input type="hidden" name="tipo_acao" value="1,2,3,8,9,10" />
        <input type="hidden" name="nome" value="Carlos Lima" />
        <input type="hidden" name="tipo_pessoa" value="PJ" />
        <input type="hidden" name="cpf" value="" />
        <input type="hidden" name="endereco" value="" />
        <input type="hidden" name="complemento" value="" />
        <input type="hidden" name="bairro" value="" />
        <input type="hidden" name="cidade" value="" />
        <input type="hidden" name="cep" value="" />
        <input type="hidden" name="estado" value="RJ" />
        <input type="hidden" name="endereco_d" value="" />
        <input type="hidden" name="complemento_d" value="" />
        <input type="hidden" name="bairro_d" value="" />
        <input type="hidden" name="cidade_d" value="" />
        <input type="hidden" name="cep_d" value="" />
        <input type="hidden" name="estado_d" value="RJ" />
        <input type="hidden" name="fone" value="2125379943" />
        <input type="hidden" name="fax" value="2125379943" />
        <input type="hidden" name="nasc" value="" />
        <input type="hidden" name="email" value="carloslima@caminhoa.com.br" />
        <input type="hidden" name="cgc" value="39137443000326" />
        <input type="hidden" name="razao_pj" value="Farmacia Caminhoa Homeopatia Ltda" />
        <input type="hidden" name="inscricao_pj" value="ISENTO" />
        <input type="hidden" name="nome_resp_pj" value="Carlos Lima" />
        <input type="hidden" name="cpf_resp_pj" value="" />  
        <input type="hidden" name="v" value="15" />
        <input type="hidden" name="q" value="" />          
        <input type="hidden" name="lang" value="pt" /> 
        <input type="hidden" name="cc" value="br" /> 
           
    </form>
    <script type="text/javascript">
        document.sendorder.submit();
    </script>
</body>
</html>
