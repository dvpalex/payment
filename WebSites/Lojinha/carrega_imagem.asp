<!--#INCLUDE FILE="checa_senha.inc" -->
<!--#INCLUDE FILE="Funcoes_uteis.asp" -->
<% 

'Força autentificação com login e senha do domínio NT
'isto é necessário por que somente o dono do domínio pode fazer
'o upload de imagem
Response.Buffer = True
Response.Clear
If Request.ServerVariables("LOGON_USER")="" Then 
	Response.Status = "401 Not Authorized"
	Response.AddHeader "WWW-Authenticate","basic"
        Response.Buffer = False
	Response.End
End If

%>
<HTML>
<HEAD>
<TITLE>Atualização da imagem</TITLE>
<meta http-equiv="Content-Type" content="text/html; charset=">
</HEAD>
<BODY>
<h2>Atualização do produto</h2>
<% 

' Esta secao faz o upload do arquivos para o diretório /images
'On Error Resume Next 

Set mySmartUpload = Server.CreateObject("Persits.Upload.1") 

' diretório em que a imagem será salva
Count = mySmartUpload.Save(Application("DiretorioRaiz") & imagem)

' algum arquivo foi feito o Upload
If Count <> 0 then

        Arquivo = mySmartUpload.Files(1).ExtractFileName

	If Err Then 
	   Response.Write("<b>Erro : </b>" & Err.description & "<br>") 
	   Set mySmartUpload = nothing      
	   Response.End
	End If 
        image = Request.QueryString("imagem")

%>
<p>A seguinte imagem foi transferida para sua loja:  <b><%= image %></b>
<br>
<%

End If

'Esta secao faz a atualização no banco de dados
Set Conexao = Server.CreateObject("ADODB.Connection")
Conexao.Open Application("StringConexaoODBC")

Set RS_Produto = Server.CreateObject("ADODB.Recordset")

' Le informacoes do produto
RS_Produto.Open "SELECT nome_produto FROM Produtos WHERE codigo_produto = " & Request.QueryString("codigo_produto"), Conexao

' Checa se pedido ja foi deletado
if RS_Produto.Eof then 
%>
<br>
<p><b>Este produto já foi apagado. Por favor refresque a página de Produtos.<br>
<p align="CENTER">
<a href="<%= Application("URL_Mostra_produtos") %>">Voltar a administração de produtos</a>
<%
	RS_Produto.Close
        Conexao.Close

        Set RS_Produto = Nothing
        Set Conexao = Nothing

	Response.End
End If

if Request.QueryString("atualiza_produto") = "Y" then

    image = Request.QueryString("imagem")
    Conexao.Execute "UPDATE Produtos SET url_imagem = '" & image & "' WHERE codigo_produto = " & Request.QueryString("codigo_produto")
%>
<br>
<p>O caminho virtual que o produto <i><u><%= RS_Produto("nome_produto") %></i></u> aponta agora é :  <b><%= image %></b>
<br>
<%

end if

RS_Produto.Close
Conexao.Close

Set RS_Produto = Nothing
Set Conexao = Nothing

Set mySmartUpload = nothing
%>
<br>
<hr>
<%
Call Pe_admin_produtos()
%> 
</HTML>
</BODY>
