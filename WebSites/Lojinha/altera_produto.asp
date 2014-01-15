<!--#INCLUDE FILE="checa_senha.inc" -->
<!--#INCLUDE FILE="Funcoes_Uteis.asp" -->
<% 
' Essa pagina chama a si mesma.

' Esta página só pode ser acessada se o visitante já se autenticou
' como administrador da loja
checa_senha()
%>
<%
' Abre conexao com banco de dados
Set Conexao = Server.CreateObject("ADODB.Connection")
Conexao.Open Application("StringConexaoODBC")

' Se foi clicado o botão alterar...
If Request.Form("alterar") <> "" Then

    cat  = Request.Form("lista")
    nome = Request.Form("nome")
    desc = Request.Form("desc")
' Retira virgula e repoe por ponto, para evitar problemas no UPDATE
    preco = replace(Request.Form("preco"),",",".")
    disponivel = Request.Form("disponivel")
    Conexao.Execute "UPDATE Produtos SET codigo_categoria = " & cat & ",nome_produto = '" & nome & "',descricao_produto = '" & desc & "',preco_unitario = " & preco & ",disponivel = " & disponivel & " WHERE codigo_produto = " & Request.QueryString("codigo_produto")

end if
%>

<HTML>
<HEAD>
<TITLE>Detalhe do produto</TITLE>
<meta http-equiv="Content-Type" content="text/html; charset=">
</HEAD>
<BODY BGCOLOR="#FFFFFF">
<%
' Se foi clicado o botão remover...
' Remove produto do banco de dados e todos os seus pedidos
If Request.form("remover") <> "" Then

    Conexao.Execute "DELETE FROM Produtos WHERE codigo_produto = " & Request.QueryString("codigo_produto")
    Conexao.Execute "DELETE FROM Pedidos WHERE codigo_pedido IN (SELECT codigo_pedido FROM Pedido_Item WHERE codigo_produto = " & Request.QueryString("codigo_produto") & ")"
    Conexao.Execute "DELETE FROM Autorizacoes WHERE codigo_pedido IN (SELECT codigo_pedido FROM Pedido_Item WHERE codigo_produto = " & Request.QueryString("codigo_produto") & ")"
    Conexao.Execute "DELETE FROM Pedido_Item WHERE codigo_produto = " & Request.QueryString("codigo_produto")

%>
<h1>Confirma&ccedil;ão de remo&ccedil;ão</h1>
<p>Produto <i><%= Request.Form("nome") %></i> e todos os seus pedidos foram removidos do banco de dados.<br>
<hr>
<br>
<%
    Call Pe_admin_produtos()

    Conexao.Close 
    Set Conexao = Nothing

    Response.End    

end if

Set RS_Produto = Server.CreateObject("ADODB.Recordset")

' Le informacoes do produto
RS_Produto.Open "SELECT * FROM Produtos WHERE codigo_produto = " & Request.QueryString("codigo_produto"), Conexao

' Checa se pedido ja foi deletado
if RS_Produto.Eof then 
%>
<p><b>Este produto já foi apagado. Por favor atualize a página de Produtos.<br>
<%
	RS_Produto.Close
	Response.End
end if

' A parte acima é executada quando alguns dos botões atualizar ou o remover são acionados.
%>
<h1>Informa&ccedil;ões do Produto</h1>
</p>
<h2>Dados do produto</h2>
<p><i>Confira abaixo as informa&ccedil;ões do produto.</i><br>
<form method="POST" name ="produto" onsubmit="return valida_produto()" action="altera_produto.asp?codigo_produto=<%= RS_Produto("codigo_produto") %>" >
 <table border="1" width="80%">
  <tr> 
    <td width="30%"> 
           Categoria:      
    </td>
    <td > 
	<align="LEFT" >
	<% Call Cria_Combo_Categoria(RS_Produto("codigo_categoria"), 2) %>
    </td>
  <tr> 
    <td width="30%"> 
           Nome:      
    </td>
    <td > 
	<align="LEFT" >
	<input type="TEXT" name="nome" value="<%= RS_Produto("nome_produto") %>" size=50>
    </td>
  <tr> 
    <td width="30%"> 
           Descri&ccedil;ão:  
    </td>
    <td> 
        <align="LEFT">
        <input type="TEXT" name="desc" value="<%= RS_Produto("descricao_produto") %>" size=50>
    </td>
  <tr> 
    <td width="30%"> 
           Pre&ccedil;o Unitário: 
    </td>
    <td> 
        <align="LEFT">
	R$<input type="TEXT" name="preco" value="<%= FormatNumber(RS_Produto("preco_unitario"),2) %>" size=10>
    </td>
  <tr> 
    <td width="30%"> 
           Disponivel: 
    </td>
    <td> 
        <align="LEFT">
	<% Call Cria_Combo_Disponivel(RS_Produto("disponivel")) %> 
    </td>
  <tr> 
 </table>
<SCRIPT LANGUAGE="JavaScript">
<!--//
function valida_produto() {

     var Form;
     Form = document.produto;
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
     if (Form.preco.value.length == 0) {
	alert("O preco é um campo obrigatório !");
        Form.preco.focus();
        return false;
     } else {
        precoStr = strReplaceAll(Form.preco.value, ',', '.');
	if (isNaN(precoStr)) {
		alert("O preco deve ser numérico !");
	        Form.preco.focus();
	        return false;
	}
     }
     return true;
}
// esta funcao repoe caracteres. Usado para repor virgulas por pontos no preco.
function strReplaceAll ( theSource, toFind, replaceWith ) {
     if (null == theSource ) return "";
 
     li_pos = theSource.indexOf( toFind );
 
     while (li_pos != -1)
     {
        if (li_pos < theSource.length -1 )
	   theSource = theSource.substring(0, li_pos ) + replaceWith +
	              theSource.substring(li_pos+1, theSource.length);
        else
  	   theSource = theSource.substring(0, li_pos );
 
	   li_pos = theSource.indexOf( toFind, li_pos + replaceWith.length ); 
     }
     return theSource;
}
//-->
</SCRIPT>
<br>
<p align="CENTER"><input type="SUBMIT" name="alterar" value="Alterar Produto">
<input type="SUBMIT" name="remover" value="Apagar Produto" >
</form>
<p><i><b>OBS:</b> Se apagar o produto, todos os pedidos relativos a esse produto serão removidos.</i><br>
<hr>
<br>
<h2>Dados da Imagem do Produto</h2>
<p><i>Confira abaixo as informa&ccedil;ões da imagem associada ao seu produto.</i><br>
<form method="POST" action="<%= Application("URL_Carrega_Imagem") %>" name="image" enctype="multipart/form-data" onsubmit="return upload()" >

 <table border="0" width="80%">
  <tr> 
    <td width="30%"> 
	Caminho no servidor: 
    </td>
    <td> 
	<input type="HIDDEN" name="codigo" value="<%= Request.QueryString("codigo_produto") %>" >
	<input type="TEXT" name="imagem" value="<%= RS_Produto("url_imagem") %>" value="/images/" size=30><font size=2><i>(Geralmente /images/...)</i>
    </td>
  <tr>
    <td width="30%"><align="TOP">
	Entre nova imagem: 
    </td>
    <td> 
	<input type="FILE" name="FILE1" size=30><BR>
  <tr>
    <td width="30%"> 
    </td>
    <td> 
	<i><font size=2>Obs: Se o botão "Browse..." não aparecer, seu Browser não suporta transferência de arquivos.</i>
    </td>
  <tr>
</table>
<p align="CENTER"><input type=submit VALUE="Alterar dados da imagem"> 
<SCRIPT LANGUAGE="JavaScript">
<!--//
function upload() { 
    
   var Form;
   Form = document.image;
   if (Form.imagem.value.length == 0) {
	alert("O campo 'Caminho no servidor' é obrigatório !");
        Form.imagem.focus();
        return false;
   }
   // pequena 'mágica' que muda o valor <action> do form acima. Isto é necessário por que 
   // a proxima pagina - carrega_imagem.asp - não sabe interpretar o valor na variavel Request.Form, apenas
   // em Request.QueryString.
   eval("document.image.action='http://loja2.locaweb.com.br/carrega_imagem.asp?atualiza_produto=Y&codigo_produto="+ document.image.codigo.value + "&imagem="+ document.image.imagem.value + "'");
   return true;
}

//-->
</SCRIPT>
</form>
<hr>
<br>
<%
RS_Produto.Close
Conexao.Close

Set RS_Produto = Nothing
Set Conexao = Nothing

Call Pe_admin_produtos()

%>
</BODY>
</HTML>
