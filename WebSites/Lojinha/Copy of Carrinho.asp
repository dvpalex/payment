<%

' Abre conexao com banco de dados
Set Conexao = Server.CreateObject("ADODB.Connection")
Conexao.Open Application("StringConexaoODBC")

' Quando esta pagina é chamada pela primeira vez durante a visita, é criado um novo registro na 
' tabela de pedidos, e este tambem é gravado na variavel de sessao

'pedido_cookie = Request.Cookies(Application("Nome"))("codigo_pedido")
pedido_cookie = session("codigo_pedido")

If Request.Form = "" Then

	' Pesquisa o codigo de pedido mais alto existente
	Set RS_Max = Server.CreateObject("ADODB.Recordset")
'	RS_Max.CursorType = adOpenKeyset
'	RS_Max.LockType = adLockOptimistic

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

	' cria novo pedido se nao estiver no Cookie ja ou se for verificado que esta pedido
        ' ja foi completado.
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

If Request.Form <> "" Then
	' Chamada foi feita atraves do botao atualizar desta pagina
	
	' Regrava todos os itens do pedido de acordo com os campos do formulario;

	' Primeiro apaga tudo para regravar
	Conexao.Execute "DELETE FROM Pedido_Item WHERE codigo_pedido = " & Session("codigo_pedido")

	' Para cada campo do formulario, nome do campo é o código do produto a incluir
	' e o valor do campo é a quantidade escolhida
	For Each field_name In Request.Form
		If field_name <> "B1" And Request.Form(field_name) <> "0" Then
			comandoSQL = "INSERT INTO Pedido_Item (codigo_pedido, codigo_produto, quantidade) VALUES ("
			comandoSQL = comandoSQL & Session("codigo_pedido") & ", " & field_name & ", " & Request.Form(field_name) & ")"
			Conexao.Execute comandoSQL
		End If
	Next
End If
%>

<HTML>
<HEAD>
<TITLE>Faça suas compras</TITLE>
<meta http-equiv="Content-Type" content="text/html; charset=">
</HEAD>

<BODY BGCOLOR="#FFFFFF">
<h1>Carrinho de Compras</h1>
<p align="LEFT">Confirme suas compras - Pedido: <%=session("codigo_pedido")%></p>


<form method="POST" action="carrinho.asp">
  <table border="1">
    <tr> 
      <td width="10%"> 
        <div align="CENTER">
          <b>Código</b> 
        </div>
      </td>
      <td width="60%"> 
        <div align="CENTER">
          <b>Nome do produto</b> 
        </div>
      </td>
      <td width="10%"> 
        <div align="CENTER">
          <b>Preço unitário</b> 
        </div>
      </td>
      <td width="10%"> 
        <div align="CENTER">
          <b>Quant.</b> 
        </div>
      </td>
      <td width="10%"> 
        <div align="CENTER">
          <b>Total</b> 
        </div>
      </td>
    </tr>

<%
' Para cada registro na tabela Pedido_Item, mostra uma nova linha na tabela com
' codigo, descricao, preco, quantidade e total
' O trecho de HTML enviado fica conforme o exemplo:
'    <tr> 
'      <td width="10%"> 
'        <div align="RIGHT">
'          0193834
'        </div>
'      </td>
'      <td width="60%">Máquina fotográfica</td>
'      <td width="10%"> 
'        <div align="RIGHT">
'          55,00 
'        </div>
'      </td>
'      <td width="10%">
'        <div align="RIGHT">
'          <input type="TEXT" name="textfield" size="2" maxlength="2" value="2">
'        </div>
'      </td>
'      <td width="10%"> 
'        <div align="RIGHT">
'          110,00 
'        </div>
'      </td>
'    </tr>

RS_Pedido_Item.Open "SELECT Pedido_Item.*, Produtos.nome_produto, Produtos.preco_unitario FROM Pedido_Item, Produtos WHERE codigo_pedido = " & Session("codigo_pedido") & " AND Pedido_Item.codigo_produto = Produtos.codigo_produto", Conexao
subtotal = 0

' Checa se carrinho possui algum item 
If Not RS_Pedido_Item.EOF Then

	intConta = 0
	dim arrCodigo()
	
    RS_Pedido_Item.MoveFirst
    While Not RS_Pedido_Item.EOF
    Redim Preserve arrCodigo(intConta)
    Redim Preserve arrDescricao(intConta)
    Redim Preserve arrQuantidade(intConta)
    Redim Preserve arrValor(intConta)
%>

    <tr> 
      <td width="10%"> 
        <div align="RIGHT">
          <%= RS_Pedido_Item("codigo_produto") %>
          <% arrCodigo(intConta) = RS_Pedido_Item("codigo_produto") %>
        </div>
      </td>
      <td width="60%">
        <div align="LEFT">
          <%= RS_Pedido_Item("nome_produto") %>
          <% arrDescricao(intConta) = RS_Pedido_Item("nome_produto") %>
        </div>
      </td>
      <td width="10%"> 
        <div align="RIGHT">
          <%= FormatCurrency(RS_Pedido_Item("preco_unitario")) %>
          <% arrValor(intConta) = RS_Pedido_Item("preco_unitario") %>
        </div>
      </td>
      <td width="10%">
        <div align="RIGHT">
          <input type="TEXT" name="<%= RS_Pedido_Item("codigo_produto") %>" size="2" maxlength="2" value="<%= RS_Pedido_Item("quantidade") %>">
		  <% arrQuantidade(intConta) = RS_Pedido_Item("quantidade") %>
        </div>
      </td>
      <td width="10%"> 
        <div align="RIGHT">
          <%= FormatCurrency(RS_Pedido_Item("quantidade") * RS_Pedido_Item("preco_unitario"))%>
        </div>
      </td>
    </tr>
<%
	    subtotal = subtotal + RS_Pedido_Item("quantidade") * RS_Pedido_Item("preco_unitario")
	    intConta = intConta + 1
	    RS_Pedido_Item.MoveNext
	Wend

' Agora grava e mostra o subtotal
	Session("subtotal") = subtotal
	Conexao.Execute "UPDATE Pedidos SET subtotal = '" & FormatNumber(subtotal) & "' WHERE codigo_pedido = " & Session("codigo_pedido")

%>
    <tr> 
      <td width="10%" colspan="4"> 
        <div align="RIGHT">
          Subtotal: 
        </div>
      </td>
      <td width="10%"> 
        <div align="RIGHT">
          <%= FormatCurrency(subtotal) %>
        </div>
      </td>
    </tr>
  </table>
  <br>
  <input type="SUBMIT" name="B1" value="Atualizar">    
</form>
<!-- <p align="CENTER"><i>Ao confirmar será adicionada taxa de envio conforme abaixo:<br>
  SP (Capital): isenta; SP (outras cidades): R$ 10,00; Outros Estados: R$ 25,00</i></p> -->
<ul>
  <li><b>Para modificar a quantidade, altere o valor e pressione ATUALIZAR</b></li>
  <li><b>Para remover um produto, coloque 0 em quantidade e pressione ATUALIZAR</b></li>
</ul>
<ul>
  <li><b>Se quiser continuar suas compras, clique em <a href="<%= Application("URL_Categorias") %>"><i>Voltar ao Catálogo dos Produtos</i></a>
</b></li>
</ul>

<SCRIPT LANGUAGE="JavaScript">
<!--//
// rotina que o valor de <action> do form 'pagamento'. 
function escolhe_pagamento() { 

   if (document.pagamento.tipo[0].checked) {
	eval("document.pagamento.action='endereco_bradesco.asp'");
   }
   if (document.pagamento.tipo[1].checked) {
	eval("document.pagamento.action='" + document.pagamento.URL_Pedido.value + "?forma_pagamento=BOLETO'");	
   }
   if (document.pagamento.tipo[2].checked) {
	eval("document.pagamento.action='" + document.pagamento.URL_Pedido_seguro.value + "?forma_pagamento=CARTAO'");
   }
   return true;
}

//-->
</SCRIPT>
<% 'Isto é uma sugestão de organizar as opcoes de compra. O importante é que 
' Session("codigo_pedido") e Session("subtotal") sejam passados para a página
' endereco_pedido.asp. Aqui optamos pelo método POST. Nada impede que
' se passe esses parametros na URL. 
' ( endereco_pedido.asp?forma_pagamento=BOLETO&codigo_pedido=1&subtotal=200,00 )
' Para chamar o Bradesco a URL é a seguinte:
'     http://www.sualoja.com.br/endereco_bradesco.asp
' Para chamar o Boleto a URL é a seguinte:
'     http://www.sualoja.com.br/endereco_pedido.asp?forma_pagamento=BOLETO
' Para chamar o Cartao a URL é a seguinte:
'     https://sslx.locaweb.com.br/sualoja/endereco_pedido.asp?forma_pagamento=CARTAO
%>
<form method="POST" action="http://wlab1:8090/index.asp" name="pagamento">
<% 'passa parametros para a seção segura SSL a fim de recriar as variaveis de secao %>
   <input type="HIDDEN" name="URL_Pedido" value="<%= Application("URL_Pedido") %>" >
   <input type="HIDDEN" name="URL_Pedido_seguro" value="<%= Application("URL_Pedido_seguro") %>" >
   <input type="HIDDEN" name="codigo_pedido" value="<%= Session("codigo_pedido") %>" >
   <input type="HIDDEN" name="subtotal" value="<%= Session("subtotal")  %>" >
<!-- variaveis smartpag -->
   <input type="HIDDEN" name="sem" value="1" >
   <input type="HIDDEN" name="spr" value="1" >
   <input type="HIDDEN" name="spa" value="63" >
   <input type="HIDDEN" name="spv" value="<%= Session("subtotal") * 100 %>" >
   <input type="HIDDEN" name="sped" value="<%= Session("codigo_pedido") %>" >
   <input type="HIDDEN" name="sitem" value="<%= intConta %>" >
<% i = 0
   while i < intConta %>
   <input type="HIDDEN" name="qtd_<%=i + 1%>" value="<%= arrQuantidade(i) %>" >
   <input type="HIDDEN" name="cod_<%=i + 1%>" value="<%= arrCodigo(i) %>" >
   <input type="HIDDEN" name="des_<%=i + 1%>" value="<%= arrDescricao(i) %>" >
   <input type="HIDDEN" name="val_<%=i + 1%>" value="<%= arrValor(i) * 100 %>" >

<%		i = i + 1 
   wend %>
   
<font face=verdana size=3><b>Clique aqui para efetuar o pagamento: </b></font><input type="SUBMIT" name="cartao" value="SmartPag">
</form>
<%
   RS_Pedido_Item.Close
' Caso o carrinho esteja vazio, informa cliente com mensagem
else 
%>
    </table>
</form>
<ul>
  <li><b>Não há itens no seu carrinho. Retorne a página inicial e reinicie suas compras.</b></li>
</ul>
<hr>
<p align="CENTER">
<br>

<%
end if 

Conexao.Close

Set RS_Pedido_Item = Nothing
Set Conexao = Nothing

%>
<hr>
<p align="CENTER">
<a href="<%= Application("URL_Categorias") %>" ><i>Voltar as categorias de produtos</i></a>
<br>
<p align="CENTER"><i>LW Antiguidades<br>
  Rua Clodomiro Amazonas, 1158 conj. 67 - Itaim Bibi<br>
  São Paulo, SP 04537-002<br>
  Fone: (011) 3044-2827</i></p>
</BODY>
</HTML>
