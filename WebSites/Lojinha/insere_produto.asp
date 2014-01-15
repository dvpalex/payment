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
<h2>Novo Produto Criado</h2>
<% 

' Esta secao faz o upload do arquivos para o diretório /images
'On Error Resume Next 

Set mySmartUpload = Server.CreateObject("Persits.Upload.1") 

Count = mySmartUpload.Save(Application("DiretorioRaiz") & imagem)

If Count <> 0 then

        Arquivo = mySmartUpload.Files(1).ExtractFileName

	If Err Then 
	   Response.Write("<b>Erro : </b>" & Err.description & "<br>") 
	   Set mySmartUpload = nothing      
	   Response.End
	End If 

End If

'Esta secao faz a atualização no banco de dados
Set Conexao = Server.CreateObject("ADODB.Connection")
Conexao.Open Application("StringConexaoODBC")

Set RS_Produto = Server.CreateObject("ADODB.Recordset")

' Insere produto
nome = Request.QueryString("nome")
desc = Request.QueryString("desc")
cat  = Request.QueryString("cat")
nomecat = Request.QueryString("nomecat")
' Retira virgula e repoe por ponto, para evitar problemas no UPDATE
preco = replace(Request.QueryString("preco"),",",".")
preco_display = replace(Request.QueryString("preco"),".",",")
if not IsNumeric(preco) then
	erro = true
end if
imagem = Request.QueryString("imagem")

' Le informacoes do produto
RS_Produto.CursorType = adOpenKeyset
RS_Produto.LockType = adLockOptimistic
RS_Produto.Open "SELECT Max(codigo_produto) AS novo_codigo_produto FROM Produtos " , Conexao

' Checa se existem produtos no banco de dados 
If IsNull(RS_Produto("novo_codigo_produto")) Then
	novo_codigo_produto = 1
Else 
	novo_codigo_produto = RS_Produto("novo_codigo_produto") + 1
End If 

' Insere produto
If not erro then
	Conexao.Execute "insert into Produtos(codigo_produto, codigo_categoria, nome_produto, descricao_produto, preco_unitario, url_imagem, disponivel) values(" & novo_codigo_produto & "," & cat & ",'" & nome & "','" & desc & "'," & preco & ",'" & imagem & "',1)"
end if

%>
<table border="0" width="80%">
  <tr> 
    <td width="35%"> 
           Categoria:      
    </td>
    <td > 
	<align="LEFT" >
	<b><%= nomecat %></b>
    </td>
  <tr> 
    <td width="35%"> 
           Nome:      
    </td>
    <td > 
	<align="LEFT" >
	<b><%= nome %></b>
    </td>
  <tr> 
    <td width="35%"> 
           Descrição:  
    </td>
    <td> 
        <align="LEFT">
	<b><%= desc %></b>
    </td>
  <tr> 
    <td width="35%"> 
           Preço Unitário: 
    </td>
    <td> 
        <align="LEFT">
	<b><%= FormatCurrency(preco_display) %></b>
    </td>
  <tr> 
    <td width="35%"> 
	Imagem Carregada: 
    </td>
    <td> 
	<b><%= imagem %></b>
    </td>
</table>

<%

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
