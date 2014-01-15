<%
' *****************************************************************
' *****************************************************************
' *****************************************************************
' Esta função retorna a taxa de envio baseada na cidade e no estado.
Function Calcula_Taxa_Total(cidade, estado)

   ' Calcula taxa de envio baseada nos dados de entrega
   If estado = "SP" Then
	If cidade = "Sao Paulo" Or cidade = "São Paulo" Then
		taxa_envio = 0
	Else
		taxa_envio = 10
	End If
   Else
 	taxa_envio = 25
   End If

   ' Calcula total da compra
   subtotal = Session("subtotal")
   total = subtotal + taxa_envio

   ' Grava variaveis de sessao para acesso pelas outras paginas
   Session("taxa_envio") = taxa_envio
   Session("total") = total

End function

' *****************************************************************
' *****************************************************************
' *****************************************************************
' Checa se pedido ainda é válido.
Function Checa_Sessao_Pedido()

   If Session("codigo_pedido") = "" then
%>
<b>Este pedido expirou. Por favor retorne à <a href="<%= Application("URL_Categorias") %>">página de Catálogo de Produtos</a> para reiniciar suas compras</b>.
<%
	Response.End
   End If 

End Function

' *****************************************************************
' *****************************************************************
' *****************************************************************
' Cria Combo Box com as categorias. Possui um campo especial que especifica o que
' associar ao conteudo de cada elemento:
' 1 - Links
' 2 - Valor da categoria
Function Cria_Combo_Categoria(categoria, especial)

If especial = 1 Then
       conteudo = "mostra_produtos.asp?codigo_categoria="
Else
       conteudo = ""
End If

' Abre conexao com banco de dados
Set Conexao2 = Server.CreateObject("ADODB.Connection")
Conexao2.Open Application("StringConexaoODBC")

Set Categorias = Server.CreateObject("ADODB.Recordset")
Categorias.Open "SELECT codigo_categoria, nome_categoria FROM Categorias ORDER BY nome_categoria", Conexao2

' Checa se existem categorias no banco de dados
If Categorias.Eof Then %>
	<p><b>Não há informações sobre produtos e categorias no seu seu banco de dados.<br>
<%
	Call Pe_admin_produtos()

	Categorias.Close 
	Conexao.Close

	Set Categorias = Nothing
	Set Conexao = Nothing

	Response.End
Else

	If categoria <> "" Then
		codigo_categoria = categoria
	Else
		codigo_categoria = Categorias("codigo_categoria")
	End If

End If %>
<SELECT NAME="lista" >
<% 
While Not Categorias.EOF 
	If (Categorias("codigo_categoria") = Cint(codigo_categoria)) Then %>
	<OPTION SELECTED VALUE="<%= conteudo %><%= Categorias("codigo_categoria") %>"><%= Categorias("nome_categoria") %></OPTION>
	<% Else %> 
	<OPTION VALUE="<%= conteudo %><%= Categorias("codigo_categoria") %>"><%= Categorias("nome_categoria") %></OPTION>
	<% End If
   Categorias.MoveNext
Wend 
Categorias.Close 
Conexao2.Close %>
   </SELECT>
<%
Set Categorias = Nothing
Set Conexao2 = Nothing

End Function

' *****************************************************************
' *****************************************************************
' *****************************************************************
Sub Cria_Combo_Disponivel(disponivel)
%>
<SELECT NAME="disponivel" >
	<%
If (disponivel = "True") Then %>
        <OPTION SELECTED VALUE="True">Sim</OPTION>		
        <OPTION VALUE="False">Não</OPTION>		
<% Else %>
        <OPTION VALUE="True">Sim</OPTION>		
        <OPTION SELECTED VALUE="False">Não</OPTION>		
<% End If %>

</SELECT>
<%
End Sub


' *****************************************************************
' *****************************************************************
' *****************************************************************
' Mostra produtos na página de administração segundo forma de pagamento
Function Mostra_Pedidos(Conexao, Pagamento)

%>
<table border="1" width="90%">
  <tr>
    <td width="10%"><b>Código</b></td>
    <td width="55%"><b>Nome do Cliente</b></td>
    <td width="15%"><b>Valor</b></td>
    <td width="10%"><div align="RIGHT"><b>Atendido</b></td>
    <td width="10%"><div align="RIGHT"><b>Pago</b></td>
  </tr>
<%
   Set Pedidos = Server.CreateObject("ADODB.Recordset")
   Pedidos.Open "SELECT Pedidos.* FROM Pedidos WHERE Pedidos.forma_pagamento='" & Pagamento  & "' ORDER BY codigo_pedido", Conexao

   While Not Pedidos.EOF
%>
  <tr>
    <td width="10%"><a href="<%= Application("URL_Mostra_pedidos_seguro") %>?codigo_pedido=<%= Pedidos("codigo_pedido") %>"><%= Pedidos("codigo_pedido") %></a></td>
    <td width="55%">
      <div align="LEFT">
        <%= Pedidos("nome") %>
      </div>
    </td>
    <td width="15%">
      <div align="RIGHT">
        <%= FormatCurrency(Pedidos("total")) %>
      </div>
    </td>
    <td width="10%">
      <div align="RIGHT">
        <% if Pedidos("atendido") = false then
               response.write "Não"
           else
               response.write "Sim"
	   end if 
	%>
      </div>
    </td>
    <td width="10%">
      <div align="RIGHT">
        <% if Pedidos("pago") = false then
               response.write "Não"
           else
               response.write "Sim"
	   end if 
	%>
      </div>
    </td>
  </tr>

<%
        total = total + Pedidos("total")
	Pedidos.MoveNext
   Wend

   Pedidos.Close
   Set Pedidos = Nothing

%>
  <tr>
    <td width="10%"></td>
    <td width="55%"></td>
    <td width="15%">
      <div align="RIGHT"><b>
        <%= FormatCurrency(total) %>
      </b></div></td>
    <td width="10%"></td>
    <td width="10%"></td>
  </tr>

</table>

<%
End Function

' *****************************************************************
' *****************************************************************
' *****************************************************************
Sub Pe_admin_produtos()
%>

<p align="CENTER">
<a href="<%= Application("URL_Categorias") %>" ><i>Veja o Catálogo de Produtos</i></a>
<br>
<p align="CENTER">
<a href="<%= Application("URL_Admin_Seguro") %>"><i>Voltar a administração da loja</i></a>
<br>

<% End Sub %>
