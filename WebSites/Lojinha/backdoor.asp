<%@ Language=VBScript %>

<%
Response.Buffer = true

if Request("passloja") = "lojateste" or Request("passloja") = "lodjao" or _
  Request("passloja") = "lojapan" or Request("passloja") = "lojabradesco" or _
  Request("passloja") = "lojafinasa" or Request("passloja") = "lojavisa01" or _
  Request("passloja") = "lojavisa445" or Request("passloja") = "lojavisa446" or _
  Request("passloja") = "lojavisa447" or Request("passloja") = "lojavisa448" or _
  Request("passloja") = "lojavisa449" or Request("passloja") = "lojavisa451" or _
  Request("passloja") = "lojavisa452" or Request("passloja") = "lojavisa453" or _
  Request("passloja") = "lojavisa454" or Request("passloja") = "lojavisa455" or _
  Request("passloja") = "lojavisa02" or Request("passloja") = "lojavisa03" or _
  Request("passloja") = "lojavisa04" or Request("passloja") = "lojavisa05" or _
  Request("passloja") = "lojatecban01" or Request("passloja") = "lojatecban02" or _
  Request("passloja") = "lojatecban03" or Request("passloja") = "lojatecban04" or _

  Request("passloja") = "lojabradesco01" or Request("passloja") = "lojabradesco02" or _
  Request("passloja") = "lojabradesco03" or Request("passloja") = "lojabradesco04" or _
  Request("passloja") = "lojabradesco05" or Request("passloja") = "lojabradesco06" or _
  Request("passloja") = "lojabradesco07" or Request("passloja") = "lojabradesco08" or _
  Request("passloja") = "lojabradesco09" or Request("passloja") = "lojabradesco10" or _
  Request("passloja") = "lojaabn1" or Request("passloja") = "superpag" or _

  Request("passloja") = "lojatecban05" or Request("passloja") = "Remota1" then

  Set Conexao = Server.CreateObject("ADODB.Connection")
  Conexao.Open Application("StringConexaoODBC")

  Set RS_Max = Server.CreateObject("ADODB.Recordset")
  RS_Max.Open "SELECT MAX(codigo_pedido) AS max_codigo_pedido FROM Pedidos", Conexao

  If IsNull(RS_Max("max_codigo_pedido")) Then
  	novo_codigo_pedido = 1
  Else
  	novo_codigo_pedido = RS_Max("max_codigo_pedido") + 1
  End If

  Conexao.Execute "INSERT INTO Pedidos (codigo_pedido) VALUES (" & novo_codigo_pedido & ")"
  ' Grava novo codigo do pedido na sessao para que possa ser lido de outras paginas
  Session("codigo_pedido") = novo_codigo_pedido

  'Response.Write "ss " & Session("codigo_pedido")

  'Response.Write Session("codigo_pedido")
  Response.Cookies(Application("Nome"))("codigo_pedido") = novo_codigo_pedido


  'Lojinha de testes para apresentação
	If Request("passloja") = "Remota1" Then
		Session("loja") = "Lojinha Remota 1"
		Response.Cookies(Application("Nome"))("loja") = "Lojinha Remota 1"
	End If

  'Lojinha padrão com dados enviados pela lojinha
	If Request("passloja") = "lojateste" Then
		Session("loja") = "Lojinha Teste"
		Response.Cookies(Application("Nome"))("loja") = "Lojinha Teste"
	End If

  'Lojinha padrão
	If Request("passloja") = "lodjao" Then
		Session("loja") = "lodjao"
		Response.Cookies(Application("Nome"))("loja") = "lodjao"
	End If

  'Lojinha de Homologação PanAmericano
	If Request("passloja") = "lojapan" Then
		Session("loja") = "pan"
		Response.Cookies(Application("Nome"))("loja") = "pan"
	End If

  'Lojinha de Homologação Bradesco
	If Request("passloja") = "lojabradesco" Then
		Session("loja") = "Loja Bradesco"
		Response.Cookies(Application("Nome"))("loja") = "Loja Bradesco"
	End If

'Lojinha de Homologação Finasa
	If Request("passloja") = "lojafinasa" Then
		Session("loja") = "Loja Finasa"
		Response.Cookies(Application("Nome"))("loja") = "Loja Finasa"
	End If

'Lojinha de Homologação Visa 01
	If Request("passloja") = "lojavisa01" Then
		Session("loja") = "Loja Visa 01"
		Response.Cookies(Application("Nome"))("loja") = "Loja Visa 01"
	End If

'Lojinha de Homologação Visa 02
	If Request("passloja") = "lojavisa02" Then
		Session("loja") = "Loja Visa 02"
		Response.Cookies(Application("Nome"))("loja") = "Loja Visa 02"
	End If

'Lojinha de Homologação Visa 03
	If Request("passloja") = "lojavisa03" Then
		Session("loja") = "Loja Visa 03"
		Response.Cookies(Application("Nome"))("loja") = "Loja Visa 03"
	End If

'Lojinha de Homologação Visa 04
	If Request("passloja") = "lojavisa04" Then
		Session("loja") = "Loja Visa 04"
		Response.Cookies(Application("Nome"))("loja") = "Loja Visa 04"
	End If

'Lojinha de Homologação Visa 05
	If Request("passloja") = "lojavisa05" Then
		Session("loja") = "Loja Visa 05"
		Response.Cookies(Application("Nome"))("loja") = "Loja Visa 05"
	End If

'Lojinha de Homologação Visa 445
	If Request("passloja") = "lojavisa445" Then
		Session("loja") = "Loja Visa 445"
		Response.Cookies(Application("Nome"))("loja") = "Loja Visa 445"
	End If

'Lojinha de Homologação Visa 446
	If Request("passloja") = "lojavisa446" Then
		Session("loja") = "Loja Visa 446"
		Response.Cookies(Application("Nome"))("loja") = "Loja Visa 446"
	End If

'Lojinha de Homologação Visa 447
	If Request("passloja") = "lojavisa447" Then
		Session("loja") = "Loja Visa 447"
		Response.Cookies(Application("Nome"))("loja") = "Loja Visa 447"
	End If

'Lojinha de Homologação Visa 448
	If Request("passloja") = "lojavisa448" Then
		Session("loja") = "Loja Visa 448"
		Response.Cookies(Application("Nome"))("loja") = "Loja Visa 448"
	End If

'Lojinha de Homologação Visa 449
	If Request("passloja") = "lojavisa449" Then
		Session("loja") = "Loja Visa 449"
		Response.Cookies(Application("Nome"))("loja") = "Loja Visa 449"
	End If

'Lojinha de Homologação Visa 451
	If Request("passloja") = "lojavisa451" Then
		Session("loja") = "Loja Visa 451"
		Response.Cookies(Application("Nome"))("loja") = "Loja Visa 451"
	End If

'Lojinha de Homologação Visa 452
	If Request("passloja") = "lojavisa452" Then
		Session("loja") = "Loja Visa 452"
		Response.Cookies(Application("Nome"))("loja") = "Loja Visa 452"
	End If

'Lojinha de Homologação Visa 453
	If Request("passloja") = "lojavisa453" Then
		Session("loja") = "Loja Visa 453"
		Response.Cookies(Application("Nome"))("loja") = "Loja Visa 453"
	End If

'Lojinha de Homologação Visa 454
	If Request("passloja") = "lojavisa454" Then
		Session("loja") = "Loja Visa 454"
		Response.Cookies(Application("Nome"))("loja") = "Loja Visa 454"
	End If

'Lojinha de Homologação Visa 455
	If Request("passloja") = "lojavisa455" Then
		Session("loja") = "Loja Visa 455"
		Response.Cookies(Application("Nome"))("loja") = "Loja Visa 455"
	End If

'Lojinha de Homologação TecBan 01
	If Request("passloja") = "lojatecban01" Then
		Session("loja") = "Loja TecBan 01"
		Response.Cookies(Application("Nome"))("loja") = "Loja TecBan 01"
	End If

'Lojinha de Homologação TecBan 02
	If Request("passloja") = "lojatecban02" Then
		Session("loja") = "Loja TecBan 02"
		Response.Cookies(Application("Nome"))("loja") = "Loja TecBan 02"
	End If

'Lojinha de Homologação TecBan 03
	If Request("passloja") = "lojatecban03" Then
		Session("loja") = "Loja TecBan 03"
		Response.Cookies(Application("Nome"))("loja") = "Loja TecBan 03"
	End If

'Lojinha de Homologação TecBan 04
	If Request("passloja") = "lojatecban04" Then
		Session("loja") = "Loja TecBan 04"
		Response.Cookies(Application("Nome"))("loja") = "Loja TecBan 04"
	End If

'Lojinha de Homologação TecBan 05
	If Request("passloja") = "lojatecban05" Then
		Session("loja") = "Loja TecBan 05"
		Response.Cookies(Application("Nome"))("loja") = "Loja TecBan 05"
	End If

'Lojinha de Homologação Bradesco 01
	If Request("passloja") = "lojabradesco01" Then
		Session("loja") = "Loja bradesco 01"
		Response.Cookies(Application("Nome"))("loja") = "Loja bradesco 01"
	End If

'Lojinha de Homologação Bradesco 02
	If Request("passloja") = "lojabradesco02" Then
		Session("loja") = "Loja bradesco 02"
		Response.Cookies(Application("Nome"))("loja") = "Loja bradesco 02"
	End If

'Lojinha de Homologação Bradesco 03
	If Request("passloja") = "lojabradesco03" Then
		Session("loja") = "Loja bradesco 03"
		Response.Cookies(Application("Nome"))("loja") = "Loja bradesco 03"
	End If

'Lojinha de Homologação Bradesco 04
	If Request("passloja") = "lojabradesco04" Then
		Session("loja") = "Loja bradesco 04"
		Response.Cookies(Application("Nome"))("loja") = "Loja bradesco 04"
	End If

'Lojinha de Homologação Bradesco 05
	If Request("passloja") = "lojabradesco05" Then
		Session("loja") = "Loja bradesco 05"
		Response.Cookies(Application("Nome"))("loja") = "Loja bradesco 05"
	End If

'Lojinha de Homologação Bradesco 06
	If Request("passloja") = "lojabradesco06" Then
		Session("loja") = "Loja bradesco 06"
		Response.Cookies(Application("Nome"))("loja") = "Loja bradesco 06"
	End If

'Lojinha de Homologação Bradesco 07
	If Request("passloja") = "lojabradesco07" Then
		Session("loja") = "Loja bradesco 07"
		Response.Cookies(Application("Nome"))("loja") = "Loja bradesco 07"
	End If

'Lojinha de Homologação Bradesco 08
	If Request("passloja") = "lojabradesco08" Then
		Session("loja") = "Loja bradesco 08"
		Response.Cookies(Application("Nome"))("loja") = "Loja bradesco 08"
	End If

'Lojinha de Homologação Bradesco 09
	If Request("passloja") = "lojabradesco09" Then
		Session("loja") = "Loja bradesco 09"
		Response.Cookies(Application("Nome"))("loja") = "Loja bradesco 09"
	End If

'Lojinha de Homologação Bradesco 10
	If Request("passloja") = "lojabradesco10" Then
		Session("loja") = "Loja bradesco 10"
		Response.Cookies(Application("Nome"))("loja") = "Loja bradesco 10"
	End If

'Lojinha SuperPag
	If Request("passloja") = "superpag" Then
		Session("loja") = "Loja SuperPag"
		Response.Cookies(Application("Nome"))("loja") = "Loja SuperPag"
	End If

	If Request("passloja") = "lojaabn1" Then
		Session("loja") = "Loja ABN 1"
		Response.Cookies(Application("Nome"))("loja") = "Loja ABN 1"
	End If


	Response.Cookies(Application("Nome")).Expires = now+10
	Response.Cookies(Application("Nome")).Domain = Application("URLdaLoja")
	Response.Redirect("carrinho.asp?codigo_produto=2")

else

	Response.Redirect("main.asp?erro=1")
	
end if

Response.End() 
%>
