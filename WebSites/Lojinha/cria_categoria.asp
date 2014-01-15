<!--#INCLUDE FILE="checa_senha.inc" -->
<% 
' Esta página só pode ser acessada se o visitante já se autenticou
' como administrador da loja
checa_senha()
%>

<HTML>
<HEAD>
<TITLE>Criar nova Categoria</TITLE>
<meta http-equiv="Content-Type" content="text/html; charset=">
</HEAD>
<BODY BGCOLOR="#FFFFFF">
<h1>Informações da Categoria</h1>
</p>
<h2>Dados da Categoria</h2>
<p><i>Entre as novas informações da categoria.</i><br>
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
           Descrição:  
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

//-->
</SCRIPT>
<hr>
<br>
<p align="CENTER">
<a href="<%= Application("URL_Mostra_produtos") %>"><i>Voltar a administração de produtos</i></a>
</BODY>
</HTML>
