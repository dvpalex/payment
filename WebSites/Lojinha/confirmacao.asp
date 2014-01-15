<!--#INCLUDE FILE="encripta.asp" -->
<!--#INCLUDE FILE="funcoes_uteis.asp" -->
<%

'escolhe redirecionamento da próxima página baseado na forma de pagamento. Para cartão de crédito,
'deve sempre seguir a via segura.
If Session("forma_pagamento") = "CARTAO" Then
	URL_Grava_endereco=Application("URL_Grava_endereco_seguro")
Else
	URL_Grava_endereco=Application("URL_Grava_endereco")
End If

' Checa de Session("codigo_pedido") nao expirou
Call Checa_Sessao_Pedido()

' Cliente clicou em 'Gravar Dados'. Checa se numero do cartao de credito é valido
If Session("forma_pagamento") = "CARTAO" Then

           ' Redireciona para endereco_pedido caso ocorra algum erro com cartão
	   ' ?erro_cartao = 1 -> problema no número
	   ' ?erro_cartao = 2 -> problema na data de validade

 	   ' guarda os dados em variaveis de sessao para que eles não se percam.
 	   Session("nome") = Request("ship_to_name")
	   Session("endereco") = Request("ship_to_street") 
	   Session("cgccpf") = Request("cgccpf")
	   Session("cidade") = Request("ship_to_city")
	   Session("estado") = Request("ship_to_state")
	   Session("cep") = Request("ship_to_zip")
	   Session("instrucoes") = Request("instrucoes")
	   Session("pais") = Request("ship_to_country")
	   Session("fone") = Request("ship_to_phone")
	   Session("email") = Request("ship_to_email")
	   Session("nome_cartao") = Request("nome_cartao")
	   Session("tipo_cartao") = Request("tipo_cartao")
	   Session("mes_validade") = Request("mes_validade")
	   Session("ano_validade") = Request("ano_validade")

	   If checkcc(Request("num_cartao"), Request("tipo_cartao")) <> 0 Then
                'redireciona para endereco_pedido com variável indicando que houve erro
                'na validação do número do cartão.
		Response.Redirect Application("URL_Pedido_Seguro") & "?erro_cartao=1"
		Response.End
	   End If

	   If (Request("ano_validade") & Request("mes_validade") < Year(Now) & Month(Now)) Then

		Response.Redirect Application("URL_Pedido_Seguro") & "?erro_cartao=2"
		Response.End

	   End If
End If

' Abre conexao com banco de dados
Set Conexao = Server.CreateObject("ADODB.Connection")
Conexao.Open Application("StringConexaoODBC")

' Le informacoes do pedido
Set RS_Pedido = Server.CreateObject("ADODB.Recordset")
RS_Pedido.CursorType = adOpenKeyset
RS_Pedido.LockType = adLockOptimistic

RS_Pedido.Open "SELECT * FROM Pedidos WHERE codigo_pedido = " & Session("codigo_pedido"), Conexao
%>

<HTML>
<HEAD>
<TITLE>Confirme seu pedido</TITLE>
<meta http-equiv="Content-Type" content="text/html; charset="><style type="text/css"></style></HEAD>

<BODY BGCOLOR="#FFFFFF">
<h1>Dados de sua compra</h1>
<p><i>Estes são todos os dados da compra que está realizando. Para confirmar, clique no botão no final da página.
</i></p>

<% If Request("status") <> "" And Request("status") <> "0" Then %>
<p><i>Obs.: Houve um erro na autorização junto à instituição financeira. Contate 
  o Lojista para saber como proceder com o pagamento.</i></p>
<% End If %>

<h2>Dados para Entrega</h2>
<table border="0">
  <tr> 
    <td>Nome:</td>
    <td><b><%= Request("ship_to_name") %></b></td>
  <tr>
    <td>Endereço:</td>
    <td><b><%= Request("ship_to_street") %></b></td>
  <tr>
    <td>CEP:</td>
    <td><b><%= Request("ship_to_zip") %></b></td>
  <tr>
    <td>Cidade:</td>
    <td><b><%= Request("ship_to_city") %>, <%= Request("ship_to_state") %></b></td>
  <tr>
    <td>Prazo de entrega:     </td>
    <td><b>10 dias</b></td>
  </tr>
</table>
<h2>Dados do Pedido</h2>
<table border="0">
  <tr> 
    <td>Número do pedido:</td>
    <td><b><%= Session("codigo_pedido") %></b></td>
  <tr>
    <td>Data do pedido:</td>
    <td><b><%= now %></b></td>
  <tr>
    <td>Forma de pagamento:</td>
    <td><b><%= Session("forma_pagamento") %></b></td>
<% If Session("forma_pagamento") = "CARTAO" Then %>
  <tr>
    <td>Número do cartão:</td>
    <td><b><%= Left(Request("num_cartao"), Len(Request("num_cartao")) - 4) & "XXXX" %></b></td>
<% End If %>
  </tr>
</table>
<br>
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

' Calcula taxa de envio baseada nos dados para entrega
Call Calcula_Taxa_Total(Request("ship_to_city"), Request("ship_to_state"))

' Para cada registro na tabela Pedido_Item, mostra uma nova linha na tabela com
' codigo, descricao, preco, quantidade e total
Set RS_Pedido_Item = Server.CreateObject("ADODB.Recordset")
RS_Pedido_Item.Open "SELECT Pedido_Item.*, Produtos.nome_produto, Produtos.preco_unitario FROM Pedido_Item, Produtos WHERE codigo_pedido = " & Session("codigo_pedido") & " AND Pedido_Item.codigo_produto = Produtos.codigo_produto", Conexao
RS_Pedido_Item.MoveFirst
subtotal = 0

While Not RS_Pedido_Item.EOF
%> 
  <tr> 
    <td width="10%"> 
      <div align="RIGHT">
        <%= RS_Pedido_Item("codigo_produto") %> 
      </div>
    </td>
    <td width="60%"> 
      <div align="LEFT">
        <%= RS_Pedido_Item("nome_produto") %> 
      </div>
    </td>
    <td width="10%"> 
      <div align="RIGHT">
        <%= FormatCurrency(RS_Pedido_Item("preco_unitario")) %> 
      </div>
    </td>
    <td width="10%"> 
      <div align="RIGHT">
        <%= RS_Pedido_Item("quantidade") %> 
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
    RS_Pedido_Item.MoveNext
Wend

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
  <tr> 
    <td width="10%" colspan="4">
      <div align="RIGHT">
        Taxa de envio:
      </div>
    </td>
    <td width="10%">
      <div align="RIGHT">
        <%= FormatCurrency(Session("taxa_envio")) %> 
      </div>
</td>
  </tr>
  <tr>
    <td width="10%" colspan="4">
      <div align="RIGHT">
        Total:
      </div>
    </td>
    <td width="10%">
      <div align="RIGHT">
        <%= FormatCurrency(Session("total")) %> 
      </div>
</td>
  </tr>
</table>
<br>
<hr>
<!-- chama grava_endereco.asp que atualiza informações no banco de dados -->
<form method="POST" action=<%= URL_Grava_Endereco %>>
  <input type="HIDDEN" name="ship_to_name" value="<%= Request("ship_to_name") %>">
  <input type="HIDDEN" name="cgccpf" value="<%= Request("cgccpf") %>">
  <input type="HIDDEN" name="ship_to_street" value="<%= Request("ship_to_street") %>">
  <input type="HIDDEN" name="ship_to_city" value="<%= Request("ship_to_city") %>">
  <input type="HIDDEN" name="ship_to_state" value="<%= Request("ship_to_state") %>">
  <input type="HIDDEN" name="ship_to_zip" value="<%= Request("ship_to_zip") %>">
  <input type="HIDDEN" name="ship_to_country" value="<%= Request("ship_to_country") %>">
  <input type="HIDDEN" name="ship_to_phone" value="<%= Request("ship_to_phone") %>">
  <input type="HIDDEN" name="ship_to_email" value="<%= Request("ship_to_email") %>">
  <input type="HIDDEN" name="instrucoes" value="<%= Request("instrucoes") %>">
  <p align="CENTER"><input type="SUBMIT" name="submit" value="Confirmar Pedido"><p>
  <hr><i>
</form>
<%
'Encripta numero do cartao de credito
If Session("forma_pagamento") = "CARTAO" Then 
	Data = Request("nome_cartao") & "|" & UCase(Request("tipo_cartao")) & "|" & Request("num_cartao") & "|" & Request("mes_validade") & "/" & Request("ano_validade")
        Call CriptografaDados(Data, CartaoEncriptado)
	Session("dados_cartao") = CartaoEncriptado
End If 

RS_Pedido_Item.Close
RS_Pedido.Close
Conexao.Close

Set RS_Pedido_Item = Nothing
Set RS_Pedido = Nothing
Set Conexao = Nothing

%>
<p align="CENTER"><i>Para corrigir suas informações clique no botão 'Voltar' do seu navegador</i></a></p>
</BODY>
</HTML>
