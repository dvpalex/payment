<!--#INCLUDE FILE="encripta.asp" -->
<!--#INCLUDE FILE="checa_senha.inc" -->
<% 
' Esta página só pode ser acessada se o visitante já se autenticou
' como administrador da loja
checa_senha()
%>
<%
' Abre conexao com banco de dados
Set Conexao = Server.CreateObject("ADODB.Connection")
Conexao.Open Application("StringConexaoODBC")

' Le informacoes dos pedidos 
Set RS_Pedido = Server.CreateObject("ADODB.Recordset")
RS_Pedido.Open "SELECT * FROM Pedidos WHERE codigo_pedido = " & Request.QueryString("codigo_pedido") , Conexao

' Checa se pedido ja foi deletado
if RS_Pedido.Eof then 
%>
<p><b>Este pedido já foi apagado. Por favor refresque a página de Pedidos.<br>
<p align="CENTER">
<a href="admin.asp">Voltar a administração da loja</a>
<%
	RS_Pedido.Close
        Conexao.Close

        Set RS_Pedido = nothing
        Set Conexao = nothing

	Response.End
end if

' Le informacoes de autorizacao
Set RS_Autorizacoes = Server.CreateObject("ADODB.Recordset")
RS_Autorizacoes.Open "SELECT * FROM Autorizacoes WHERE codigo_pedido = " & Request.QueryString("codigo_pedido"), Conexao

' Verifica caso pedido seja autorizado e nao capturado para permitir captura
permite_captura = False
If Not RS_Autorizacoes.EOF Then
	if RS_Autorizacoes("status_autorizacao") = 0 Then
		If IsNull(RS_Autorizacoes("status_captura")) Or RS_Autorizacoes("status_captura") <> 1 Then
			permite_captura = True
		End If
	End If
End If
%>

<HTML>
<HEAD>
<TITLE>Detalhe do pedido <%= Request.QueryString("codigo_pedido")%></TITLE>
<meta http-equiv="Content-Type" content="text/html; charset=">
</HEAD>

<BODY BGCOLOR="#FFFFFF">
<h1>Informações do Pedido</h1>
<p><i>Confira abaixo as informações do pedido.<br>
<%
' Avisa sobre captura caso trate-se de pedido ja autorizado mas nao capturado
If permite_captura Then
%>
Atenção: apenas pressione o botão 
  "Capturar" quando estiver certo de que o pedido será atendido. Isto fará com 
  que seja agendada a transferência de fundos do Cliente para o Lojista.
<% End If %>
<%
' Instrui sobre como verificar boleto
If RS_Pedido("forma_pagamento") = "BOLETO" Then
%>
Este pedido foi feito através de boleto bancário. Para saber se foi pago, verifique junto a seu banco se o documento com número idêntico ao número do pedido foi pago.
<% End If %>

</i></p>
<h2>Dados para Entrega</h2>
<table border="0">
  <tr> 
    <td>Nome:</td>
    <td><b><%= RS_Pedido("nome") %></b></td>
<%
If RS_Pedido("forma_pagamento") = "BOLETO" Then
%>
  <tr> 
    <td>CPF/CGC:</td>
    <td><b><%= RS_Pedido("cgccpf") %></b></td>
<%
End If
%>
  <tr>
    <td>Endereço:</td>
    <td><b><%= RS_Pedido("rua") %></b></td>
  <tr>
    <td>CEP:</td>
    <td><b><%= RS_Pedido("cep") %></b></td>
  <tr>
    <td>Cidade:</td>
    <td><b><%= RS_Pedido("cidade") %>, <%= RS_Pedido("estado") %> </b></td>
  <tr>
    <td>Telefone:</td>
    <td><b><%= RS_Pedido("telefone") %></b></td>
  <tr>
    <td>E-mail:</td>
    <td><b><%= RS_Pedido("email") %></b></td>
  <tr>
    <td>Prazo de entrega:</td>
    <td><b>10 dias</b></td>
  </tr>
</table>

<h2>Dados do Pedido</h2>
<table border="0">
  <tr> 
    <td>Número do pedido/conciliação do boleto:</td>
    <td><b><%= Request.QueryString("codigo_pedido") %></b></td>
  <tr>
    <td>Data do pedido:</td>
    <td><b><%= RS_Pedido("data_pedido") %></b></td>
  <tr>
    <td>Forma de pagamento:</td>
    <td><b><%= RS_Pedido("forma_pagamento") %></b></td>
<% If RS_Pedido("forma_pagamento") = "CARTAO" Then 

    On Error Resume Next

    Call DescriptografaDados(RS_Pedido("cartao_encrypt"), Session("senha"), Dados_Cartao)    

    comp = Len(Dados_Cartao)
    pos = Instr(Dados_Cartao, "|")
    nome_cartao = Mid(Dados_Cartao, 1, pos-1)
    pos2 = Instr(Mid(Dados_Cartao, pos+1, comp), "|")
    tipo_cartao = Mid(Dados_Cartao, pos+1, pos2-1)
    pos2 = pos+pos2
    pos = Instr(Mid(Dados_Cartao, pos2+1, comp), "|")
    num_cartao = Mid(Dados_Cartao, pos2+1, pos-1)
    data_validade = Right(Dados_Cartao, comp-pos-pos2)
%>
  <tr> 
    <td>Tipo do cartão:</td>
    <td><b><%= tipo_cartao %></b></td>
  <tr> 
    <td>Nome no cartão:</td>
    <td><b><%= nome_cartao %></b></td>
  <tr> 
    <td>Número do cartão:</td>
    <td><b><%= num_cartao %></b></td>
  <tr>
    <td>Data de validade:</td>
    <td><b><%= data_validade %></b></td>
<% End If %>
  <tr>
    <td>Pedido já foi atendido:</td>
    <td><b><% if RS_Pedido("atendido") = false then
	       response.write "Não"
           else
               response.write "Sim"
	   end if  %></b></td>
  <tr>
    <td>Pedido já foi pago:</td>
    <td><b><% if RS_Pedido("pago") = false then
	       response.write "Não"
           else
               response.write "Sim"
	   end if  %></b></td>
  <tr>
    <td>Instrucoes de entrega:</td>
    <td><b><%= RS_Pedido("instrucoes") %></b></td>
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
'          2
'        </div>
'      </td>
'      <td width="10%"> 
'        <div align="RIGHT">
'          110,00 
'        </div>
'      </td>
'    </tr>

Set RS_Pedido_Item = Server.CreateObject("ADODB.Recordset")
RS_Pedido_Item.Open "SELECT Pedido_Item.*, Produtos.nome_produto, Produtos.preco_unitario FROM Pedido_Item, Produtos WHERE codigo_pedido = " & Request.QueryString("codigo_pedido") & " AND Pedido_Item.codigo_produto = Produtos.codigo_produto", Conexao
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
        <%= FormatCurrency(RS_Pedido("subtotal")) %> 
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
        <%= FormatCurrency(RS_Pedido("taxa_envio")) %> 
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
        <%= FormatCurrency(RS_Pedido("total")) %> 
      </div>
</td>
  </tr>
</table>
<%
' Formata campo de valor para ser submetido ao script de captura no formato R$99,99
valor = "R$"
valor = valor & Trim(CStr(Fix(RS_Pedido("total")))) & ","
valor = valor & Right("00" & CInt(100 * (RS_Pedido("total") - Fix(RS_Pedido("total")))), 2)


%> <%
' Mostra botao de captura
If permite_captura Then
%> 
<form method="POST" action="processa_captura.asp">
  <input type="HIDDEN" name="codigo_pedido" value="<%= Request.QueryString("codigo_pedido") %>">
  <input type="HIDDEN" name="valor" value="<%= valor %>">
  <p align="CENTER"><input type="SUBMIT" name="submit" value="Capturar">
  <hr><i>
</form>
<% 
End If 

' Essa parte mostra os botões 'Atender Pedido', 'Pedido Pago' e 'Remover Pedido'
' todos eles chamam altera_pedido.asp que irá atualizar a tabela de Pedidos.
%>
<form method="POST" action="altera_pedido.asp">
  <input type="HIDDEN" name="codigo_pedido" value="<%= Request.QueryString("codigo_pedido") %>">
  <p align="CENTER"><input type="SUBMIT" name="deletar" value="Remover Pedido">
<% 
if RS_Pedido("atendido") = False then %>
  <align="CENTER"><input type="SUBMIT" name="atender" value="Marcar como Atendido">
<% else %>
  <align="CENTER"><input type="SUBMIT" name="atender" value="Marcar como não atendido">
<% end if 
if RS_Pedido("pago") = False then %>
  <align="CENTER"><input type="SUBMIT" name="pagar" value="Marcar como Pago">
<% else %>
  <align="CENTER"><input type="SUBMIT" name="pagar" value="Marcar como não Pago">
<% end if %>
</form>
<br>
<p align="CENTER">
<a href="<%= Application("URL_Admin_Seguro") %>"><i>Voltar a administração da loja</i></a>
<br><br>
<hr>
<p align="CENTER"><i>LW Antiguidades<br>
  Rua Ângelo Bada, 257 - Interlagos<br>
  São Paulo, SP 04776-014<br>
  Fone: (011) 5666-949</i>5</p>
<% 

RS_Autorizacoes.Close
RS_Pedido_Item.Close
RS_Pedido.Close
Conexao.Close

Set RS_Autorizacoes = Nothing
Set RS_Pedido_Item = Nothing
Set RS_Pedido = Nothing
Set Conexao = Nothing

%>
</BODY>
</HTML>
