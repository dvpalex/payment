<!--#INCLUDE FILE="checa_senha.inc" -->
<!--#INCLUDE FILE="Funcoes_Uteis.asp" -->
<% 
' Essa pagina chama a si mesma.

' Esta página só pode ser acessada se o visitante já se autenticou
' como administrador da loja
checa_senha()

' Abre conexao com banco de dados
Set Conexao = Server.CreateObject("ADODB.Connection")
Conexao.Open Application("StringConexaoODBC")

codigo_categoria = Request("codigo_categoria")

' Se foi clicado o botão alterar...
If Request("alterar") <> "" Then

    nome = Request("nome")
    desc = Request("desc")
    Conexao.Execute "UPDATE Categorias SET nome_categoria = '" & nome & "',descricao_categoria = '" & desc & "' WHERE codigo_categoria = " & codigo_categoria

end if
%>

<HTML>
<HEAD>
<TITLE>Detalhe da categoria</TITLE>
<meta http-equiv="Content-Type" content="text/html; charset=">
</HEAD>
<BODY BGCOLOR="#FFFFFF">
<%
' Se foi clicado o botão remover...
' Remove produto do banco de dados e todos os seus pedidos
If Request("remover") <> "" Then

    Conexao.Execute "DELETE FROM Categorias WHERE codigo_categoria = " & codigo_categoria
    Conexao.Execute "DELETE FROM Produtos WHERE codigo_categoria = " & codigo_categoria
    Conexao.Execute "DELETE FROM Pedidos WHERE codigo_pedido IN (SELECT codigo_pedido FROM Pedido_Item WHERE codigo_produto IN (SELECT codigo_produto FROM Produtos WHERE codigo_categoria = " & codigo_categoria & "))"
    Conexao.Execute "DELETE FROM Autorizacoes WHERE codigo_pedido IN (SELECT codigo_pedido FROM Pedido_Item WHERE codigo_produto IN (SELECT codigo_produto FROM Produtos WHERE codigo_categoria = " & codigo_categoria & "))"
    Conexao.Execute "DELETE FROM Pedido_Item WHERE codigo_produto IN (SELECT codigo_produto FROM Produtos WHERE codigo_categoria = " & codigo_categoria & ")"

%>
<h1>Confirma&ccedil;ão de remo&ccedil;ão</h1>
<p>Categoria <i><%= Request("nome") %></i> e todos os seus produtos e pedidos foram removidos do banco de dados.<br>
<hr>
<br>
<%
    Call Pe_admin_produtos()

    Conexao.Close 
    Set Conexao = Nothing

    Response.End    

end if

Set Categorias = Server.CreateObject("ADODB.Recordset")

' Le informacoes da Categoria
Categorias.Open "SELECT * FROM Categorias WHERE codigo_categoria = " & codigo_categoria, Conexao

' Checa se pedido ja foi deletado
if Categorias.Eof then 
%>
<p><b>Esta categoria já foi apagada. Por favor atualize a página de Categorias.<br>
<%
	Categorias.Close
	Response.End
end if

' A parte acima é executada quando alguns dos botões atualizar ou o remover são acionados.
%>
<h1>Informa&ccedil;ões da categoria</h1>
</p>
<h2>Dados da categoria</h2>
<p><i>Confira abaixo as informa&ccedil;ões da categoria.</i><br>
<form method="POST" name ="categoria" onsubmit="return valida_categoria()" action="altera_categoria.asp?codigo_categoria=<%= Categorias("codigo_categoria") %>" > <table border="1" width="80%">
  <tr> 
    <td width="30%"> 
           Nome:      
    </td>
    <td > 
	<align="LEFT" >
	<input type="TEXT" name="nome" value="<%= Categorias("nome_categoria") %>" size=50>
    </td>
  <tr> 
    <td width="30%"> 
           Descri&ccedil;ão:  
    </td>
    <td> 
        <align="LEFT">
        <input type="TEXT" name="desc" value="<%= Categorias("descricao_categoria") %>" size=50>
    </td>
  <tr> 
 </table>
<SCRIPT LANGUAGE="JavaScript">
<!--//
function valida_categoria() {

     var Form;
     Form = document.categoria;
     if (Form.nome.value.length == 0) {
	alert("O nome é um campo obrigatório !");
        Form.nome.focus();
        return false;
     }
     if (Form.desc.value.length == 0) {
	alert("A descrição é um campo obrigatório !");
        Form.desc.focus();
        return false;
     }
     return true;
}
function confirma_apagar() {
     var resposta;
     resposta = confirm("Tem certeza que deseja apagar essa categoria ?");
     if (resposta == true) 
	return true;
     else
	return false;
}

//-->
</SCRIPT>
<br>
<p align="CENTER"><input type="SUBMIT" name="alterar" value="Alterar Categoria" >
<input type="SUBMIT" name="remover" value="Apagar Categoria" onsubmit="return confirma_apagar()">
</form>
<p><i><b>OBS:</b> Se apagar a categoria, todos os produtos e pedidos relativos a essa categoria serão removidos.</i><br>
<hr>

<%

Categorias.Close
Conexao.Close

Set Categorias = Nothing
Set Conexao = Nothing

Call Pe_admin_produtos()

%>
</BODY>
</HTML>
