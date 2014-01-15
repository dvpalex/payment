<HTML>
<HEAD>
<TITLE>Categoria criada</TITLE>
<meta http-equiv="Content-Type" content="text/html; charset=">
</HEAD>
<BODY>
<h2>Novo Categoria Criada</h2>
<% 

'Esta secao faz a atualização no banco de dados
Set Conexao = Server.CreateObject("ADODB.Connection")
Conexao.Open Application("StringConexaoODBC")

Set Categorias = Server.CreateObject("ADODB.Recordset")

' Insere produto
nome = Request("nome")
desc = Request("desc")

' Le informacoes da categoria
Categorias.CursorType = adOpenKeyset
Categorias.LockType = adLockOptimistic
Categorias.Open "SELECT MAX(codigo_categoria) AS novo_codigo_categoria FROM Categorias" , Conexao

' Checa se existem categorias no banco de dados 
If IsNull(Categorias("novo_codigo_categoria")) Then
	novo_codigo_categoria = 1
Else 
	novo_codigo_categoria = Categorias("novo_codigo_categoria") + 1
End If 

' Insere categoria
If not erro then
	Conexao.Execute "INSERT INTO Categorias(codigo_categoria, nome_categoria, descricao_categoria) VALUES(" & novo_codigo_categoria & ",'" & nome & "','" & desc & "')"
end if

%>
<table border="0" width="80%">
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
</table>

<%

Categorias.Close
Conexao.Close

Set Categorias = Nothing
Set Conexao = Nothing

%> 
<br>
<hr>
<p align="CENTER">
<a href="<%= Application("URL_Mostra_produtos") %>"><i>Voltar a administração de produtos</i></a>
<br>
<p align="CENTER">
<a href="<%= Application("URL_Categorias") %>"><i>Veja as categorias de produto</i></a>
</HTML>
</BODY>
