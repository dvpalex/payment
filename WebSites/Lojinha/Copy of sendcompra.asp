<%@ Language=VBScript %>



<%
  dim bSendPost
  
  bSendPost = false

  if Request("91D4C3128BF7DA7F") <> "" and Request("36948FFEF212F5E4") <> "" then
    bSendPost = true
  end if  

  if bSendPost then
    'Abre conexao com banco de dados
    Set Conexao = Server.CreateObject("ADODB.Connection")

    Conexao.Open Application("StringConexaoODBC")

    ' Quando esta pagina é chamada pela primeira vez durante a visita, é criado um novo registro na 
    ' tabela de pedidos, e este tambem é gravado na variavel de sessao
    'pedido_cookie = Request.Cookies(Application("Nome"))("codigo_pedido")
    'pedido_cookie = session("codigo_pedido")
    pedido_cookie = Request("91D4C3128BF7DA7F") 

    If Request.Form = "" Then
      ' Pesquisa o codigo de pedido mais alto existente
      Set RS_Max = Server.CreateObject("ADODB.Recordset")
      '  RS_Max.CursorType = adOpenKeyset
      '  RS_Max.LockType = adLockOptimistic

      ' checa se pedido ja foi concluido, se nao insere na tabela.
      ' Isto é necessario por medidas de seguranca. O Cookie é um arquivo texto facilmente
      ' modificado e alguem pode tentar usar um pedido que ja existe e foi concluido e assim
      ' iria estraga-lo.
      If pedido_cookie <> "" Then
        RS_Max.Open "SELECT codigo_pedido, forma_pagamento FROM Pedidos WHERE codigo_pedido=" & pedido_cookie, Conexao
        If RS_Max.EOF Then
          ' Nao achou. Tem que criar pedido
          pedido_cookie = ""
        Else
          ' Ja que pedido nao foi completado podemos usa-lo.
          If IsNull(RS_Max("forma_pagamento")) Then
            Session("codigo_pedido") = pedido_cookie
            ' Pedido ja foi usado.
          Else
            pedido_cookie = ""
          End If
        End If
        RS_Max.Close    
      End If

      'cria novo pedido se nao estiver no Cookie ja ou se for verificado que esta pedido
      'ja foi completado.
      If pedido_cookie = "" Then
        RS_Max.Open "SELECT MAX(codigo_pedido) AS max_codigo_pedido FROM Pedidos", Conexao

        ' Acrescenta 1 e grava novo codigo de pedido
        If IsNull(RS_Max("max_codigo_pedido")) Then
          novo_codigo_pedido = 1
        Else
          novo_codigo_pedido = RS_Max("max_codigo_pedido") + 1
        End If
        novo_codigo_pedido = session("codigo_pedido") ' ### inseri

        Conexao.Execute "INSERT INTO Pedidos (codigo_pedido) VALUES (" & novo_codigo_pedido & ")"
        ' Grava novo codigo do pedido na sessao para que possa ser lido de outras paginas
        'Session("codigo_pedido") = novo_codigo_pedido ### tirei
        '  Grava novo codigo do pedido em um cookie.
        Response.Cookies(Application("Nome"))("codigo_pedido") = novo_codigo_pedido
        Response.Cookies(Application("Nome")).Expires = now+10
        Response.Cookies(Application("Nome")).Domain = Application("URLdaLoja")
        RS_Max.Close
      End If

      Set RS_Max = Nothing
    End If

    ' Esta pagina pode ser chamada de dois diferentes locais:
    '
    ' Da pagina de produtos, quando o visitante clica em um produto:
    ' Nesse caso, deve ser lida a variavel codigo_produto, que e enviada junto com o request da pagina
    ' pois os links da pagina de produto sao do tipo carrinho.asp?codigo_produto=xxx
    ' 
    ' Da propria pagina carrinho.asp quando o visitante resolve mudar a quantidade de um dos itens.
    ' Nesse caso a quantidade alterada devera estar na variavel de POST

    Set RS_Pedido_Item = Server.CreateObject("ADODB.Recordset")

    If Request.QueryString("codigo_produto") <> "" Then
      ' Chamada foi feita clicando em um item na pagina de produtos

      ' Se ja nao esta no pedido, item é incluido na tabela Pedido_Item
      RS_Pedido_Item.Open "SELECT * FROM Pedido_Item WHERE codigo_pedido = " & Session("codigo_pedido") & " AND codigo_produto = " & Request.QueryString("codigo_produto"), Conexao
      
      If RS_Pedido_Item.EOF Then
        ' Nao achou. Portanto inclui
        Conexao.Execute "INSERT INTO Pedido_Item (codigo_pedido, codigo_produto, quantidade) VALUES (" & Session("codigo_pedido") & ", " & Request.QueryString("codigo_produto") & ", 1)"
      End If
      num_itens_total = 1
      RS_Pedido_Item.Close
    End If

    If Request.Form <> "" And Request("codERRO")="" And Request("desERRO")="" Then
      ' Chamada foi feita atraves do botao atualizar desta pagina
      
      ' Regrava todos os itens do pedido de acordo com os campos do formulario;

      ' Primeiro apaga tudo para regravar
      Conexao.Execute "DELETE FROM Pedido_Item WHERE codigo_pedido = " & Session("codigo_pedido")

      ' Para cada campo do formulario, nome do campo é o código do produto a incluir
      ' e o valor do campo é a quantidade escolhida
      For Each field_name In Request.Form
        comandoSQL = NULL
        
        If field_name <> "B1" And Request.Form(field_name) <> "0" Then

          comandoSQL = "INSERT INTO Pedido_Item (codigo_pedido, codigo_produto, quantidade) VALUES ("
          comandoSQL = comandoSQL & Session("codigo_pedido") & ", " & field_name & ", " & Request.Form(field_name) & ")"

          Conexao.Execute comandoSQL
        End If
      Next
    End If
    %>
    <!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
    <html>
      <head><title>Send Compra - Smart Pag</title></head>
      <body onLoad="javascript:document.formPost.submit();">


	<form action="http://homolog.superpag.com.br/handshakehtml.aspx" method="POST" name="formPost">



          <!-- ID TRANSACAO -->
          <input type="hidden" name="36948FFEF212F5E4" value="<%=Request("36948FFEF212F5E4")%>">

		  <!-- Parametro para setar o Frame em 50% -->
           <!--<input type="hidden" name="frame50" value="1">-->
           
          <!-- Parametro para não exibir a tela de finalizacao do Smartpag -->
          <input type="hidden" name="SHOW_TELA_FINALIZACAO" value="1"> 



          <!-- Parametros Novos para o e-Cobranca -->
          
          <!-- Quantidade de Recorrencias -->
          <!--<input type="hidden" name="COB_QUANTIDADE" value="0">-->
          
          <!-- Tipo de liguidacao da 1ª parcela (1-Smartpag/ 2- eCobOffLine/ 3 - eCobOnline) -->
          <!--<input type="hidden" name="COB_LIQ_1PAR" value="2">-->
          
          <!-- Data da cobranca da 1ª parcela -->
          <!-- <input type="hidden" name="COB_DATA_BASE_AGENDAMENTO" value="20040315"> -->
          
          <!-- Tipo de Recorrencia (Mensal, quinzenal, etc) -->
          <!--<input type="hidden" name="COB_RECORRENCIA" value="1">-->


         <!--<input type="hidden" name="URLPOSTLOJA" value="http://homolog.smartpag.com.br/teste_post.asp">-->
	 <input type="hidden" name="LINKBOTAO6" value="http://www.e-financial.com.br">

	<input type="hidden" name="INSTRUCAO_BOLETO" value="Instrução do boleto">
	<input type="hidden" name="DATA_BOLETO" value="20060520">
<!--	<input type="hidden" name="DATA_BOLETO" value="19000101"> -->
	
	<input type="hidden" name="email" value="tecnologoasmartpag@e-financial.com.br">
	

<!-- TESTE IBM 
	<input type="hidden" name="TIPO_PESSOA" value="PF">
	<input type="hidden" name="TIPO_ACAO" value="1,2,3,8,9,10">
	<INPUT TYPE="HIDDEN" NAME="NOME" VALUE="NOME DO CARA">
	<INPUT TYPE="HIDDEN" NAME="CPF" VALUE="00000000191">
	<INPUT TYPE="HIDDEN" NAME="FONE" VALUE="1138189442">
	<INPUT TYPE="HIDDEN" NAME="ESTADO" VALUE="SP">
-->

		


          <%
          RS_Pedido_Item.Open "SELECT Pedido_Item.*, Produtos.nome_produto, Produtos.preco_unitario FROM Pedido_Item, Produtos WHERE codigo_pedido = " & Session("codigo_pedido") & " AND Pedido_Item.codigo_produto = Produtos.codigo_produto", Conexao
          subtotal = 0

          'Response.Write "intconta:" & Session("codigo_pedido") & "<br>"

          ' Checa se carrinho possui algum item 
          If Not RS_Pedido_Item.EOF Then
            intConta = 0
            valped = 0

            RS_Pedido_Item.MoveFirst
            While Not RS_Pedido_Item.EOF
              %>
              <input type="hidden" name="Qtd_<%=intconta + 1%>" value="<%=RS_Pedido_Item("quantidade")%>">
              <input type="hidden" name="Cod_<%=intconta + 1%>" value="<%=RS_Pedido_Item("codigo_produto")%>">
              <input type="hidden" name="Des_<%=intconta + 1%>" value="<%=RS_Pedido_Item("nome_produto")%>">
              <input type="hidden" name="Val_<%=intconta + 1%>" value="<%=cdbl(RS_Pedido_Item("preco_unitario")) * 100%>">
              <%
              If Session("loja") = "Lojinha Teste" or Session("loja") = "Loja Bradesco" or _
                Session("loja") = "Loja Finasa" or Session("loja") = "Loja Visa 01" or _
                Session("loja") = "Loja Visa 02" or Session("loja") = "Loja Visa 03" or _
                Session("loja") = "Loja Visa 04" or Session("loja") = "Loja Visa 05" or _
                Session("loja") = "Loja Visa 445" or Session("loja") = "Loja Visa 446" or _
                Session("loja") = "Loja Visa 447" or Session("loja") = "Loja Visa 448" or _
                Session("loja") = "Loja Visa 449" or Session("loja") = "Loja Visa 451" or _
                Session("loja") = "Loja Visa 452" or Session("loja") = "Loja Visa 453" or _
                Session("loja") = "Loja Visa 454" or Session("loja") = "Loja Visa 455" or _
                Session("loja") = "Loja TecBan 01" or Session("loja") = "Loja TecBan 02" or _

                Session("loja") = "Loja bradesco 01" or Session("loja") = "Loja bradesco 02" or _
                Session("loja") = "Loja bradesco 03" or Session("loja") = "Loja bradesco 04" or _
                Session("loja") = "Loja bradesco 05" or Session("loja") = "Loja bradesco 06" or _
                Session("loja") = "Loja bradesco 07" or Session("loja") = "Loja bradesco 08" or _
                Session("loja") = "Loja bradesco 09" or Session("loja") = "Loja bradesco 10" or _

                Session("loja") = "Loja TecBan 03" or Session("loja") = "Loja TecBan 04"Then

' or _                Session("loja") = "Loja TecBan 05" or lcase(Session("loja")) = "lodjao" 
                %>
                <% if intConta = 0 then %>
                <input type="hidden" name="cpf" value="12312312387">
                <input type="hidden" name="nome" value="Teste E-Financial">
                <input type="hidden" name="nasc" value="01011970">
                <input type="hidden" name="fone" value="1132696101">
                <input type="hidden" name="sexo" value="M">
                
                <input type="hidden" name="logradouro" value="avenida">
                <input type="hidden" name="endereco" value="paulista">
                <input type="hidden" name="numero" value="1842">
                <input type="hidden" name="bairro" value="jardim paulistano">
                <input type="hidden" name="cep" value="01310200">
                <input type="hidden" name="cidade" value="São Paulo">
                <input type="hidden" name="estado" value="SP">

                <input type="hidden" name="nome_d" value="Teste e-Financial">
                <input type="hidden" name="logradouro_d" value="avenida">
                <input type="hidden" name="endereco_d" value="paulista">
                <input type="hidden" name="numero_d" value="1842">
                <input type="hidden" name="cep_d" value="01310200">
                <input type="hidden" name="cidade_d" value="São Paulo">
                <input type="hidden" name="estado_d" value="SP">
                <% end if %>
                <%
              End If

              valped = valped + cdbl(RS_Pedido_Item("quantidade")) * cdbl(RS_Pedido_Item("preco_unitario")) * 100
              'Response.Write valped & "<br>"

              intconta = intconta + 1
              'Response.Write "intconta:" & intconta & "<br>"

              RS_Pedido_Item.MoveNext
            Wend
          end if
          valped = valped + 0 + 100
          %>
          <!-- INFORMAÇÕES DO PEDIDO -->
          <input type="hidden" name="91D4C3128BF7DA7F" value="<%=Request("91D4C3128BF7DA7F")%>">
          <input type="hidden" name="spv" value=<%=valped%>>
          <input type="hidden" name="sitem" value="<%=intconta%>">

          <!-- nao obrigatorio -->
          <input type="hidden" name="sfrete" value="100">
          
          <%
          'Homologação Bradesco
            If Session("loja") = "Loja Bradesco" or _
                Session("loja") = "Loja bradesco 01" or Session("loja") = "Loja bradesco 02" or _
                Session("loja") = "Loja bradesco 03" or Session("loja") = "Loja bradesco 04" or _
                Session("loja") = "Loja bradesco 05" or Session("loja") = "Loja bradesco 06" or _
                Session("loja") = "Loja bradesco 07" or Session("loja") = "Loja bradesco 08" or _
                Session("loja") = "Loja bradesco 09" or Session("loja") = "Loja bradesco 10" Then
              %>
              <input type="hidden" name="pPagamento" value="PAGAMENTO FACIL BRADESCO">
              <%
            End If

          'Homologação Finasa
            If Session("loja") = "Loja Finasa" Then
              %>
              <input type="hidden" name="pPagamento" value="FINASA">
		          <input type="hidden" name="TAXA" value="39000">
		          <input type="hidden" name="TAC" value="1000">
		          <input type="hidden" name="PQTDPARCELAS" value="4">
          		<input type="hidden" name="VAL_PARCELA" value="<%=valped/4%>">
              <%
            End If

          'Homologação ABN
            If Session("loja") = "Loja ABN 1" Then
              %>
              <%
                 qtd_parcelas_abn = 12
                 data_vencto = date()+30
                 data_vencto = Right("00"&day(data_vencto),2)&"/"&Right("00"&month(data_vencto),2)&"/"&year(data_vencto)
				 vlr_entrada = 10000%>
        <input type="hidden" name="pPagamento" value="23">
		<input type="hidden" name="QTDPARCELAS_ABN" value="<%=qtd_parcelas_abn%>">
		<input type="hidden" name="VAL_ENTRADA_ABN" value="<%=vlr_entrada%>">
		<input type="hidden" name="DAT_VENCIMENTO1_ABN" value="<%=data_vencto%>">
        <%vlr_parc_abn = FormatNumber((Cdbl(Left(valped-vlr_entrada,Len(valped-vlr_entrada)-2)&"."&Right(valped-vlr_entrada,2))/qtd_parcelas_abn)+50,2)%>
		<%vlr_parc_abn = Replace(Replace(vlr_parc_abn,".",""),",","")%>
        <input type="hidden" name="VAL_PARCELA_ABN" value="<%=vlr_parc_abn%>">

              <%
            End If

          'Homologação Visa
            If Session("loja") = "Loja Visa 01" or Session("loja") = "Loja Visa 02" or _
               Session("loja") = "Loja Visa 03" or Session("loja") = "Loja Visa 04" or _
               Session("loja") = "Loja Visa 445" or Session("loja") = "Loja Visa 446" or _
               Session("loja") = "Loja Visa 447" or Session("loja") = "Loja Visa 448" or _
               Session("loja") = "Loja Visa 449" or Session("loja") = "Loja Visa 451" or _
               Session("loja") = "Loja Visa 452" or Session("loja") = "Loja Visa 453" or _
               Session("loja") = "Loja Visa 454" or Session("loja") = "Loja Visa 455" or _
               Session("loja") = "Loja Visa 05" Then
              %>
              <input type="hidden" name="pPagamento" value="CARTAO DE CREDITO">
              <%
            End If

          'Homologação TecBan
            If Session("loja") = "Loja TecBan 01" or Session("loja") = "Loja TecBan 02" or _
               Session("loja") = "Loja TecBan 03" or Session("loja") = "Loja TecBan 04" or _
               Session("loja") = "Loja TecBan 05" Then
              %>
              <input type="hidden" name="pPagamento" value="CHEQUE ELETRONICO">
              <%
            End If
            %>
        </form>
      </body>
    </html>
    <%
  end if
  %>
