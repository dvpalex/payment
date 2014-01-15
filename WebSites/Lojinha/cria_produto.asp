<!--#INCLUDE FILE="checa_senha.inc" -->
<!--#INCLUDE FILE="Funcoes_uteis.asp" -->
<% 
' Esta p�gina s� pode ser acessada se o visitante j� se autenticou
' como administrador da loja
checa_senha()

categoria = Request("codigo_categoria") 

%>

<HTML>
<HEAD>
<TITLE>Criar novo produto</TITLE>
<meta http-equiv="Content-Type" content="text/html; charset=">
</HEAD>
<BODY BGCOLOR="#FFFFFF">
<h1>Informa��es do Produto</h1>
</p>
<h2>Dados do produto</h2>
<p><i>Entre as novas informa��es do produto.</i><br>
<form method="POST" name ="produto" enctype="multipart/form-data" onsubmit="return valida_produto()" action="<%= Application("URL_Insere_Produto") %>">
 <table border="1" width="80%">
  <tr> 
    <td width="30%"> 
           Categoria:      
    </td>
    <td> 
	<align="LEFT">
	<% Call Cria_Combo_Categoria(categoria, 2) %>
    </td>
  <tr> 
    <td width="30%"> 
           Nome:      
    </td>
    <td > 
	<align="LEFT" >
	<input type="TEXT" name="nome" size=50>
    </td>
  <tr> 
    <td width="30%"> 
           Descri��o:  
    </td>
    <td> 
        <align="LEFT">
        <input type="TEXT" name="desc" size=50>
    </td>
  <tr> 
    <td width="30%"> 
           Pre�o Unit�rio: 
    </td>
    <td> 
        <align="LEFT">
	R$ 
	<%if not erro then %>
        	<input type="TEXT" name="preco" size=10>
        <% else %>
		<input type="TEXT" name="preco" value="<%= preco %>" size=10>
		<b>Pre�o deve ser num�rico</b>
	<% end if %>
    </td>
  <tr> 
    <td width="30%"> 
	Caminho no servidor e no Banco de Dados: 
    </td>
    <td> 
	<input type="TEXT" name="imagem" value="/images/" size=30><i><font size=2><i>(Geralmente /images/...)</i>
    </td>
  <tr>
    <td width="30%"><align="TOP">
	Entre nova imagem: 
    </td>
    <td> 
	<input type="FILE" name="file1" size=30><BR>
    </td>
  <tr>
    <td width="30%"> 
    </td>
    <td> 
	<i><font size=2>Obs: Se o bot�o "Browse..." n�o aparecer, seu Browser n�o suporta transfer�ncia de arquivos.</i>
    </td>
  <tr>

 </table>

<br>
<p align="CENTER"><input type="SUBMIT" name="alterar" value="Inserir Produto" >
</form>
<SCRIPT LANGUAGE="JavaScript">
<!--//
function valida_produto() { 
    
     Form = document.produto;
     if (Form.nome.value.length == 0) {
	alert("O nome � um campo obrigat�rio !");
        Form.nome.focus();
        return false;
     }
     if (Form.desc.value.length == 0) {
	alert("A descri��o � um campo obrigat�rio !");
        Form.desc.focus();
        return false;
     }
     if (Form.preco.value.length == 0) {
	alert("O preco � um campo obrigat�rio !");
        Form.preco.focus();
        return false;
     } else {
        precoStr = strReplaceAll(Form.preco.value, ',', '.');
	if (isNaN(precoStr)) {
		alert("O preco deve ser num�rico !");
	        Form.preco.focus();
	        return false;
	}
     }
     if (Form.imagem.value.length == 0) {
	alert("O campo 'Caminho no servidor' � obrigat�rio !");
        Form.imagem.focus();
        return false;
     }
     if (Form.file1.value.length == 0) {
	alert("O campo Imagem � obrigat�rio !");
        Form.file1.focus();
        return false;
     }
   // pequena 'm�gica' que muda o valor <action> do form acima. Isto � necess�rio por que 
   // a proxima pagina - carrega_imagem.asp - n�o sabe interpretar o valor na variavel Request.Form, apenas
   // em Request.QueryString.
   eval("Form.action='http://loja2.locaweb.com.br/insere_produto.asp?insere_produto=Y&imagem="+ Form.imagem.value + "&nome="+ Form.nome.value + "&desc="+ Form.desc.value + "&preco="+ Form.preco.value + "&cat="+ Form.lista.options[Form.lista.selectedIndex].value + "&nomecat="+ Form.lista.options[Form.lista.selectedIndex].text + "'");
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
<hr>
<br>
<%
Call Pe_admin_produtos()
%>
</BODY>
</HTML>
