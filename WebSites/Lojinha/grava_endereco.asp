<!--#INCLUDE FILE="funcoes_uteis.asp" -->
<%
Response.Buffer = True

' Pega os dados de endereco
nome = Request("ship_to_name")
rua = Request("ship_to_street")
cidade = Request("ship_to_city")
estado = Request("ship_to_state")
cep = Request("ship_to_zip")
pais = Request("ship_to_country")
telefone = Request("ship_to_phone")
email = Request("ship_to_email")


' Pega os dados adicionais da pagina de endereco
instrucoes = Request("instrucoes") ' Exemplo de dado não suportado pela Wallet

' Calcula taxa de envio baseada nos dados de entrega
Call Calcula_Taxa_Total(cidade, estado)

' Grava informacoes no banco de dados
Set Conexao = Server.CreateObject("ADODB.Connection")
Set RS_Pedido = Server.CreateObject("ADODB.Recordset")
RS_Pedido.CursorType = adOpenKeyset
RS_Pedido.LockType = adLockOptimistic

Conexao.Open Application("StringConexaoODBC")
RS_Pedido.Open "SELECT * FROM Pedidos WHERE codigo_pedido = " & Session("codigo_pedido"), Conexao

RS_Pedido("nome") = nome
RS_Pedido("rua") = rua
RS_Pedido("cidade") = cidade
RS_Pedido("estado") = estado
RS_Pedido("cep") = cep
RS_Pedido("pais") = pais
RS_Pedido("telefone") = telefone
RS_Pedido("email") = email
RS_Pedido("taxa_envio") = Session("taxa_envio")
RS_Pedido("total") = Session("total")
If Session("forma_pagamento") = "CARTAO" Then
	RS_Pedido("cartao_encrypt") = Session("dados_cartao")
End If

' Grava instrucoes se houver
If instrucoes <> "" Then
	RS_Pedido("instrucoes") = instrucoes
End If

' Grava tambem a forma de pagamento, da variavel de sessao
RS_Pedido("forma_pagamento") = Session("forma_pagamento")

' Caso forma de pagamento seja boleto grava tambem CGC/CPF
If Session("forma_pagamento") = "BOLETO" Then
	RS_Pedido("cgccpf") = Request("cgccpf")
End If

RS_Pedido.Update

RS_Pedido.Close
Conexao.Close

Set RS_Pedido = Nothing
Set Conexao = Nothing

If Session("forma_pagamento") = "BRADESCONET" Then

	' Muda para a página de pagamento enviando dados da compra
	' Os dados enviados são obrigatórios

	valor = "R$"
	valor = valor & Trim(CStr(Fix(Session("total")))) & ","
	valor = valor & Right("00" & CInt(100 * (Session("total") - Fix(Session("total")))), 2)

	Response.Redirect "pagamento.asp?valor=" & valor & "&numOrder=" & Session("codigo_pedido") & "&numShopper=00000000000000000001&"
	Response.End

ElseIf Session("forma_pagamento") = "CARTAO" Then

	Response.Redirect Application("URL_Recibo_seguro")
	Response.End

Else

	Response.Redirect Application("URL_Recibo")
	Response.End

End If
%>
