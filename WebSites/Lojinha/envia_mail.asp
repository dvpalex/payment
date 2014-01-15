<%
' Este arquivo inclui uma subrotina que envia uma mensagem de confirmacao para 
' o clinte de sua compra, bem como o link do boleto, caso ele deseje imprimi-lo 
' novamente

Sub Envia_Email(Email_cliente, URLBoleto, nome, rua, complemento)

Set Conexao = Server.CreateObject("ADODB.Connection")
Conexao.Open Application("StringConexaoODBC")

Response.Write "Enviando e-mail..."

message = "Prezado cliente," & Chr(13) & Chr(10) & Chr(13) & Chr(10)

message = message & "Esta é uma confirmação da compra que realizou na " & Application("NomeLoja") & "." & Chr(13) & Chr(10) & Chr(13) & Chr(10)

Set RS_Pedido_Item = Server.CreateObject("ADODB.Recordset")
RS_Pedido_Item.Open "SELECT Pedido_Item.*, Produtos.nome_produto, Produtos.preco_unitario FROM Pedido_Item, Produtos WHERE codigo_pedido = " & Session("codigo_pedido") & " AND Pedido_Item.codigo_produto = Produtos.codigo_produto", Conexao
RS_Pedido_Item.MoveFirst
subtotal = 0

message = message & "O pedido de número #" & Session("codigo_pedido") & ": " & Chr(13) & Chr(10) & Chr(13) & Chr(10) &_
     "Produto                   Preco Unitário     Quantidade   Total" & Chr(13) & Chr(10) &_
     "===============================================================" & Chr(13) & Chr(10) 

While Not RS_Pedido_Item.EOF

        Total = FormatCurrency(RS_Pedido_Item("quantidade") * RS_Pedido_Item("preco_unitario"))
	message = message &_ 
            Left(RS_Pedido_Item("nome_produto") & Space(29), 29) &_
            Right(Space(11) & FormatCurrency(RS_Pedido_Item("preco_unitario")), 11) &_
            Right(Space(10) & RS_Pedido_Item("quantidade"),10) &_
            Right(Space(13) & Total, 13) & Chr(13) & Chr(10)
        subtotal = subtotal + Total
	RS_Pedido_Item.MoveNext
Wend

message = message & "---------------------------------------------------------------" & Chr(13) & Chr(10)

message = message & Space(39) & "Subtotal     " & Right(Space(11) & FormatCurrency(subtotal),11) & Chr(13) & Chr(10) &_
    Space(39) & "Taxa de envio" & Right(Space(11) & FormatCurrency(Session("taxa_envio")),11) & Chr(13) & Chr(10) &_
    Space(39) & "------------------------" & Chr(13) & Chr(10) &_
    Space(39) & "Total        " & Right(Space(11) & FormatCurrency(Session("total")),11) & Chr(13) & Chr(10) 

message = message & Chr(13) & Chr(10) & "Método de pagamento escolhido: " & Session("forma_pagamento") & Chr(13) & Chr(10)

'Escreve endereco
message = message & Chr(13) & Chr(10) & "Endereço de entrega:" & Chr(13) & Chr(10) &_
	nome & Chr(13) & Chr(10) &_
	rua & Chr(13) & Chr(10) &_
	complemento & Chr(13) & Chr(10) & Chr(13) & Chr(10)

if Session("forma_pagamento") = "BOLETO" then 
    message = message &_
       "Caso deseje imprimir ou checar seu boleto novamente, por favor clique " &_
       "no seguinte link: " & URLBoleto & Chr(13) & Chr(10) & Chr(13) & Chr(10)
end if 

message = message & "A " & Application("NomeLoja") & " agradece a preferência. Esperamos atendê-lo novamente no futuro."
	
Set Mailer = Server.CreateObject("SMTPsvg.Mailer")
Mailer.FromName   = Application("NomeLoja")
Mailer.FromAddress= Application("MailLoja")
Mailer.RemoteHost = Application("ServidorSMTP")
Mailer.AddRecipient "", Email_cliente
' copia para a loja
Mailer.AddCC        Application("NomeLoja") , Application("MailLoja")
Mailer.Subject    = "Confirmação de compra - Loja Virtual Locaweb"
Mailer.BodyText   = message
if not Mailer.SendMail then  
	Response.Write "Problema com envio de e-mail. Erro: " & Mailer.Response
else
        Response.Write "E-mail de confirmação enviado"
end if

RS_Pedido_Item.Close
Conexao.Close

Set RS_Pedido_Item = Nothing
Set Conexao = Nothing
Set Mailer = Nothing

End Sub
%>
