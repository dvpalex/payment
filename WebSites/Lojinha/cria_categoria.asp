<!--#INCLUDE FILE="checa_senha.inc" -->
<% 
' Esta p�gina s� pode ser acessada se o visitante j� se autenticou
' como administrador da loja
checa_senha()
%>

<HTML>
<HEAD>
<TITLE>Criar nova Categoria</TITLE>
<meta http-equiv="Content-Type" content="text/html; charset=">
</HEAD>
<BODY BGCOLOR="#FFFFFF">
<h1>Informa��es da Categoria</h1>
</p>
<h2>Dados da Categoria</h2>
<p><i>Entre as novas informa��es da categoria.</i><br>
<form method="POST" name ="categoria" onsubmit="return valida_categoria()" action="insere_categoria.asp">
 <table border="1" width="80%">
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
 </table>

<br>
<p align="CENTER"><input type="SUBMIT" name="inserir" value="Inserir Categoria" >
</form>
<SCRIPT LANGUAGE="JavaScript">
<!--//
function valida_categoria() { 
    
     Form = document.categoria;
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
   return true;
}

//-->
</SCRIPT>
<hr>
<br>
<p align="CENTER">
<a href="<%= Application("URL_Mostra_produtos") %>"><i>Voltar a administra��o de produtos</i></a>
</BODY>
</HTML>
