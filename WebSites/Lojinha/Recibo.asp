<!--#INCLUDE FILE="funcoes_uteis.asp" -->
<!--#include FILE="envia_mail.asp" -->
<%
' Checa de Session("codigo_pedido") nao expirou
Call Checa_Sessao_Pedido()

' Remove Cookie
Response.Cookies(Application("Nome"))("codigo_pedido") = ""
Response.Cookies(Application("Nome")).Expires = now
Response.Cookies(Application("Nome")).Domain = Application("URLdaLoja")

' Abre conexao com banco de dados
Set Conexao = Server.CreateObject("ADODB.Connection")
Conexao.Open Application("StringConexaoODBC")

' Le informacoes do pedido
Set RS_Pedido = Server.CreateObject("ADODB.Recordset")
RS_Pedido.CursorType = adOpenKeyset
RS_Pedido.LockType = adLockOptimistic

RS_Pedido.Open "SELECT * FROM Pedidos WHERE codigo_pedido = " & Session("codigo_pedido"), Conexao

' Atualiza data do pedido
RS_Pedido("data_pedido") = Now
Email_cliente = RS_Pedido("email")
RS_Pedido.Update
%>

<HTML>
<HEAD>
<TITLE>Confirmação de Compra</TITLE>
<meta http-equiv="Content-Type" content="text/html; charset=">
<style type="text/css">
<!--
-->
</style></HEAD>

<BODY BGCOLOR="#333399">
<h1><font color="#FF0000" face="Comic Sans MS" size="5">Confirmação de Compra </font><font color="#000066" face="Comic Sans MS" size="5"> <img border="0" src="images/final.gif" width="117" height="84"></font></h1>
<p><i><font color="#99CCFF" face="Comic Sans MS">Este é o recibo de sua compra. Por favor imprima esta página para
<%
If Session("forma_pagamento") = "BOLETO" Then
	Response.Write " pagamento na rede bancária."
Else
	Response.Write " referência."
End If
%>
Você deverá receber um e-mail com esses dados em breve também.</font>
</i></p>

<% If Request("status") <> "" And Request("status") <> "0" Then %>
<p><i><font color="#99CCFF" face="Comic Sans MS">Obs.: Houve um erro na autorização junto à instituição financeira. Contate 
  o Lojista para saber como proceder com o pagamento.</font></i></p>
<% End If %>

<h2><font color="#FF0000" face="Comic Sans MS" size="4">Dados para Entrega</font></h2>
<table border="0">
  <tr> 
    <td><font color="#FFFFFF" face="Comic Sans MS">Nome:</font></td>
    <td><b><font color="#FFFFFF" face="Comic Sans MS"><%= RS_Pedido("nome") %></font></b></td>
  <tr>
    <td><font color="#FFFFFF" face="Comic Sans MS">Endereço:</font></td>
    <td><b><font color="#FFFFFF" face="Comic Sans MS"><%= RS_Pedido("rua") %></font></b></td>
  <tr>
    <td><font color="#FFFFFF" face="Comic Sans MS">CEP:</font></td>
    <td><b><font color="#FFFFFF" face="Comic Sans MS"><%= RS_Pedido("cep") %></font></b></td>
  <tr>
    <td><font color="#FFFFFF" face="Comic Sans MS">Cidade:</font></td>
    <td><b><font color="#FFFFFF" face="Comic Sans MS"><%= RS_Pedido("cidade") %>, <%= RS_Pedido("estado") %> </font> </b></td>
  <tr>
    <td><font color="#FFFFFF" face="Comic Sans MS">Prazo de entrega:</font>     </td>
    <td><b><font color="#FFFFFF" face="Comic Sans MS">10 dias</font></b></td>
  </tr>
</table>
<h2><font color="#FF0000" face="Comic Sans MS" size="4">Dados do Pedido</font></h2>
<table border="0">
  <tr> 
    <td><font color="#FFFFFF" face="Comic Sans MS">Número do pedido:</font></td>
    <td><b><font color="#FFFFFF" face="Comic Sans MS"><%= Session("codigo_pedido") %></font></b></td>
  <tr>
    <td><font color="#FFFFFF" face="Comic Sans MS">Data do pedido:</font></td>
    <td><b><font color="#FFFFFF" face="Comic Sans MS"><%= RS_Pedido("data_pedido") %></font></b></td>
  <tr>
    <td><font color="#FFFFFF" face="Comic Sans MS">Forma de pagamento:</font></td>
    <td><b><font color="#FFFFFF" face="Comic Sans MS"><%= Session("forma_pagamento") %></font></b></td>
  </tr>
</table>
<% 
If RS_Pedido("forma_pagamento") = "BRADESCONET" Then
	Set RS_Autorizacoes = Server.CreateObject("ADODB.Recordset")
	RS_Autorizacoes.Open "SELECT * FROM Autorizacoes WHERE codigo_pedido = " & Session("codigo_pedido"), Conexao 
%>  
<table border="0">
  <tr>
    <td><font color="#FFFFFF" face="Comic Sans MS">Prazo de pagamento (dias):</font></td>
    <td><b><font color="#FFFFFF" face="Comic Sans MS"><%= RS_Autorizacoes("prazo") %></font></b></td>
  <tr>
    <td><font color="#FFFFFF" face="Comic Sans MS">Código de autorização:</font></td>
    <td><font face="Comic Sans MS" size="2" color="#FFFFFF"><% aut = RS_Autorizacoes("codigo_autorizacao") %> 
  <%= Mid(aut, 1, 64) %><br>
  <%= Mid(aut, 65, 64) %><br>
  <%= Mid(aut, 129, 64) %><br>
  <%= Mid(aut, 193, 64) %><br>
  </font></td>
<%
	RS_Autorizacoes.Close
        Set RS_Autorizacoes = Nothing
%>
  </tr>
</table>
<font color="#CC0000" face="Comic Sans MS">
<%
End If
%>

<br>
</font>
<table border="1">
  <tr> 
    <td width="10%"> 
      <div align="CENTER">
        <b><font color="#FFFFFF" face="Comic Sans MS">Código</font></b> 
      </div>
    </td>
    <td width="60%"> 
      <div align="CENTER">
        <b><font color="#FFFFFF" face="Comic Sans MS">Nome do produto</font></b> 
      </div>
    </td>
    <td width="10%"> 
      <div align="CENTER">
        <b><font color="#FFFFFF" face="Comic Sans MS">Preço unitário</font></b> 
      </div>
    </td>
    <td width="10%"> 
      <div align="CENTER">
        <b><font color="#FFFFFF" face="Comic Sans MS">Quant.</font></b> 
      </div>
    </td>
    <td width="10%"> 
      <div align="CENTER">
        <b><font color="#FFFFFF" face="Comic Sans MS">Total</font></b> 
      </div>
    </td>
  </tr>
  <%
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
        <font color="#FFFFFF" face="Comic Sans MS">
        <%= RS_Pedido_Item("codigo_produto") %> 
        </font> 
      </div>
    </td>
    <td width="60%"> 
      <div align="LEFT">
        <font color="#FFFFFF" face="Comic Sans MS">
        <%= RS_Pedido_Item("nome_produto") %> 
        </font> 
      </div>
    </td>
    <td width="10%"> 
      <div align="RIGHT">
        <font color="#FFFFFF" face="Comic Sans MS">
        <%= FormatCurrency(RS_Pedido_Item("preco_unitario")) %> 
        </font> 
      </div>
    </td>
    <td width="10%"> 
      <div align="RIGHT">
        <font color="#FFFFFF" face="Comic Sans MS">
        <%= RS_Pedido_Item("quantidade") %> 
        </font> 
      </div>
    </td>
    <td width="10%"> 
      <div align="RIGHT">
        <font color="#CC0000" face="Comic Sans MS">
        <%= FormatCurrency(RS_Pedido_Item("quantidade") * RS_Pedido_Item("preco_unitario"))%> 
        </font> 
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
        <font color="#FFFFFF" face="Comic Sans MS">
        Subtotal:</font> 
      </div>
    </td>
    <td width="10%"> 
      <div align="RIGHT">
        <font color="#CC0000" face="Comic Sans MS">
        <%= FormatCurrency(subtotal) %> 
        </font> 
      </div>
    </td>
  </tr>
  <tr> 
    <td width="10%" colspan="4">
      <div align="RIGHT">
        <font color="#FFFFFF" face="Comic Sans MS">
        Taxa de envio:</font>
      </div>
    </td>
    <td width="10%">
      <div align="RIGHT">
        <font color="#CC0000" face="Comic Sans MS">
        <%= FormatCurrency(Session("taxa_envio")) %> 
        </font> 
      </div>
</td>
  </tr>
  <tr>
    <td width="10%" colspan="4">
      <div align="RIGHT">
        <font color="#FFFFFF" face="Comic Sans MS">
        Total:</font>
      </div>
    </td>
    <td width="10%">
      <div align="RIGHT">
        <font color="#CC0000" face="Comic Sans MS">
        <%= FormatCurrency(Session("total")) %> 
        </font> 
      </div>
</td>
  </tr>
</table>
<font color="#CC0000" face="Comic Sans MS">
  <br>
</font>
<hr>
<% 
'Manda e-mail para cliente confirmando a compra e enviando link caso ele queira
'imprimir novamente o boleto.

complemento = RS_Pedido("cep") & " " & RS_Pedido("cidade") & " " & RS_Pedido("estado")
nome = RS_Pedido("nome")
rua = RS_Pedido("rua")
cgccpf = RS_Pedido("cgccpf") 

RS_Pedido_Item.Close
RS_Pedido.Close
Conexao.Close

' se o pagamento foi feito via boleto, inclui um link para o cliente possa imprimir
' o boleto. Vale notar que no envio do e-mail o mesmo link tb será enviado.

If Session("forma_pagamento") = "BOLETO" Then 

   valor = Session("total")
   vencimento = Formatdatetime(DateAdd("d", Application("DiasdeVencimento"), Now), 2)
   URLBoleto = "http://www1.locaweb.com.br/scripts/ficha.comp?valor=" & FormatNumber(valor, 2) &_
    "&banco=" & Application("banco") & "&agencia=" & Application("agencia") &_
    "&codigo_cedente=" & Application("codigo_cedente") & "&numdoc=" &_
    Session("codigo_pedido") & "&conta="& Application("conta") & "&sacado=" &_
    Server.URLEncode(nome) & "&cgccpfsac=" & cgccpf &_
    "&carteira=" & Application("carteira") & "&vencto=" & vencimento

%>
<SCRIPT LANGUAGE="Javascript">
<!--//
function JanelaNova() {
window.open('<%= URLBoleto %>','NOME','width=600,height=400,menubar,scrollbars,resizable','NOME','width=600,height=400,menubar,scrollbars,resizable');
}
//-->
</SCRIPT>
                          <p align="left"><b><font color="#FF0000" face="Comic Sans MS" size="4">Instru&ccedil;&otilde;es 
                            para Pagamento Inicial</font></b></p>
                          <p align="left"><font color="#99CCFF" face="Comic Sans MS">Caso 
                            possua uma impressora a laser ou jato de tinta, <b><a href="javascript:JanelaNova()">clique 
                            aqui</a></b> para exibir um boleto banc&aacute;rio 
                            que poder&aacute; ser impresso e pago em qualquer 
                            banco.</font></p>
<font color="#CC0000" face="Comic Sans MS">
<%
End if 

'Envia e-mail para cliente com dados da compra. Essa rotina se encontra em 
'envia_mail.asp
Call Envia_Email(Email_cliente, URLBoleto, nome, rua, complemento)

Set RS_Pedido_Item = Nothing
Set RS_Pedido = Nothing
Set Conexao = Nothing

%>
<br>
</font>
<p align="CENTER">
<a href="<%= Application("URL_Categorias") %>"><i>
<font color="#FFFFFF" face="Comic Sans MS">
Clique aqui para fazer mais compras
</font>
</i></a>
<font color="#FFFFFF" face="Comic Sans MS">
<br>
</font>
</BODY>
</HTML>
