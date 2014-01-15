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
 
<HTML><HEAD><TITLE></TITLE></HEAD>
<BODY>
<FORM name=sendorder action=http://localhost/SuperPag/handshakehtml.aspx method=post>
<INPUT type=hidden value=<%=Request("36948FFEF212F5E4")%> name=36948FFEF212F5E4>
<INPUT type=hidden value=<%=Request("91D4C3128BF7DA7F")%> name=91D4C3128BF7DA7F>
<INPUT type=hidden value={A01769A5-F7EF-456F-879E-FCA081684AA0} name=sOrderID>
<INPUT type=hidden value={E24671BF-2AF1-4899-B0E2-1B153E9F00D2} name=sUserID>
<INPUT type=hidden value=0 name=SMP_VALE_PRESENTE><INPUT type=hidden value=3367 name=SPV>
<INPUT type=hidden value=377 name=SFRETE><INPUT type=hidden value=1 name=SITEM>
<INPUT type=hidden value=1 name=QTD_1><INPUT type=hidden value=09601 name=COD_1>
<INPUT type=hidden value="PreActive Tônico Bifásico Matificante pele Mista a Oleosa 150ml" name=DES_1>
<INPUT type=hidden value=2990 name=VAL_1>
<INPUT type=hidden value="Realizar o pagamento até a data de vencimento. Após esta data, realize um novo pedido no site ou solicite maiores informações através de nosso SAC - Serviço de Atendimento ao Cliente pelo telefone 0800-413011." name=INSTRUCAO_BOLETO>
<INPUT type=hidden value="SIDNEY LOPES DE ARAUJO" name=NOME>
<INPUT type=hidden value=03465409922 name=CPF>
<INPUT type=hidden value=marcos.sampaio.ext@tivit.com.br name=EMAIL>
<INPUT type=hidden value=4133817431 name=FONE>
<INPUT type=hidden value=M name=SEXO>
<INPUT type=hidden value=01021981 name=NASC>
<INPUT type=hidden name=PROF>
<INPUT type=hidden value=AV name=LOGRADOURO>
<INPUT type=hidden value="CDOR FRANCO" name=ENDERECO><INPUT type=hidden value=3041 name=NUMERO>
<INPUT type=hidden name=COMPLEMENTO>
<INPUT type=hidden value=GUABIROTUBA name=BAIRRO>
<INPUT type=hidden value=CURITIBA name=CIDADE>
<INPUT type=hidden value=81520000 name=CEP>
<INPUT type=hidden value=PR name=ESTADO>
<INPUT type=hidden value=BR name=PAIS>
<INPUT type=hidden value=AV name=LOGRADOURO_D>
<INPUT type=hidden value="CDOR FRANCO" name=ENDERECO_D>
<INPUT type=hidden value=3041 name=NUMERO_D>
<INPUT type=hidden name=COMPLEMENTO_D>
<INPUT type=hidden value=GUABIROTUBA name=BAIRRO_D>
<INPUT type=hidden value=CURITIBA name=CIDADE_D>
<INPUT type=hidden value=81520000 name=CEP_D>
<INPUT type=hidden value=PR name=ESTADO_D>
<INPUT type=hidden value=BR name=PAIS_D>
<INPUT type=hidden name=ENVIA_EMAIL_CLIENTE>
</FORM>
<SCRIPT>document.sendorder.submit();</SCRIPT>
</BODY></HTML>

<%End If
  %>
