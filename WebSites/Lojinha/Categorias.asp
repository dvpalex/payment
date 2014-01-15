<%
' Pagina de categorias
Response.Buffer = true

Response.Redirect("produtos.asp")
Response.End


' Abre conexao com banco de dados. Alterar a string de conexao em global.asa
Set Conexao = Server.CreateObject("ADODB.Connection")
Conexao.Open Application("StringConexaoODBC")

' Abre tabela de produtos usando a conexao aberta acima.
Set Categorias = Server.CreateObject("ADODB.Recordset")
Categorias.Open "select * from categorias" , Conexao
%>


<!--
<HTML>
<HEAD>
<TITLE>Categorias de Produto</TITLE>
<meta http-equiv="Content-Type" content="text/html; charset=">
</HEAD>

<BODY>

<p><br><i><font face="Comic Sans MS" size="4">Veja abaixo nosso catálogo de mercadorias.&nbsp;</font></i></p>
<p><i><font face="Comic Sans MS" size="4"> Clique sobre uma categoria para levá-lo a página de produtos ou se preferir procure por um produto digitando uma palavra chave.</font></i></p>
<p>&nbsp;</p>
<table border="0" cellpadding="6">
<%
' INICIO PROCURA
' Esse codigo pode ser retirado e colocado numa pagina em separado se não se
' desejar misturar categorias com procura.
%>
<td valign="middle" align="center">
<ul>
<form method="POST" name="encontra" action="produtos.asp" onsubmit="return valida_procura()">
    <p align="left"><font face="Comic Sans MS" color="#FFFFFF">Nome/Descrição do Produto: <br>
	<input type="TEXT" name="procura" size="20" maxlength="40" >
	<input type="SUBMIT" name="submeter" value="Procurar">    
    </font>    
</form>
</ul>
<%
' FIM PROCURA
%>
<br>
<%
' Le codigo, nome da categoria, ja usando o codigo da categoria para montar o link para a pagina de produtos
' Quanto executado o codigo abaixo retorna um trecho de HTML conforme o exemplo:
'  <tr>
'    <td><a href="produtos.asp?codigo_categoria=2&nome_categoria=Brinquedos">Brinquedos</a><br>
'      Encontre aqui todos os brinquedos disponíveis na nossa loja.</td>
'  </tr>

'Categorias.MoveFirst
'While Not Categorias.EOF
%>

<ul>
  <li><p><a href="produtos.asp?codigo_categoria=<%'= Categorias("codigo_categoria") %>&nome_categoria=<%'= Categorias("nome_categoria") %>"><%'= Categorias("nome_categoria") %><br></a>
        <%'= Categorias("descricao_categoria") %></p></li>
</ul>

<%
'  Categorias.MoveNext
'Wend

'Categorias.Close
'Conexao.Close

'Set Categorias = Nothing
'Set Conexao = Nothing

%>

</table>
<p>&nbsp;</p>
<p>&nbsp;</p>
</BODY>
</HTML>
-->


<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML><HEAD><TITLE>Nova Conexão</TITLE>
<META http-equiv=Content-Type content="text/html; charset=windows-1252"><!-- frames -->
<META content="MSHTML 5.50.4807.2300" name=GENERATOR>
<SCRIPT LANGUAGE="JavaScript">
<!--//
function valida_procura() {

     var Form; 
     Form = document.encontra;

     if (Form.procura.value.length == 0) {
	alert("Campo Procura não pode ser vazio !");
        Form.procura.focus();
        return false;
     }
     if (Form.procura.value.length < 3) {
	alert("Campo procura deve conter ao menos 3 caracteres !");
        Form.procura.focus();
        return false;
     }
     return true;
}
//-->
</SCRIPT>
</HEAD>

<BODY bgColor=#ffffff leftMargin=0 topMargin=0 marginheight="0" marginwidth="0">

<table width="80%" border="0" cellpadding="0" cellspacing="0" align="center">
  <tr valign="top"> 
    <td bgcolor="#FFFFFF" colspan="4"> 
      <p>&nbsp;</p>
    </td>
  </tr>
  <tr valign="middle"> 
    <td bgcolor="#FFFFFF" colspan="4"> 
      <div align="center"> 
        <p><font size="2" face="Verdana, Arial, Helvetica, sans-serif"><b><font color="#999999" size="3" face="Arial, Helvetica, sans-serif"><font face="Verdana, Arial, Helvetica, sans-serif" size="2">Este 
          &eacute; o nosso cat&aacute;logo de categorias<br>
          </font></font></b></font><font face="Verdana, Arial, Helvetica, sans-serif" size="1">Clique 
          sobre uma categoria</font></p>
      </div>
    </td>
  </tr>
  <tr valign="top"> 
    <td bgcolor="#FFFFFF" colspan="4"> 
      <hr color=#cccccc size=1 height="1">
    </td>
  </tr>

<%
Do While not Categorias.EOF
%>
  <tr bordercolor="#FFFFFF"> 
    <td width="40%"> &nbsp;</td>
    <td valign="top"> 
      <br><font face="Verdana, Arial, Helvetica, sans-serif" size="2" color="#999999"><b>
		<a href="produtos.asp?codigo_categoria=<%= Categorias("codigo_categoria") %>&nome_categoria=<%= Categorias("nome_categoria") %>"><%= Categorias("nome_categoria") %></a>
        <br><%= Categorias("descricao_categoria") %></font>
      </td>
    <td width="25%"> &nbsp;</td>
  </tr>
<%
    Categorias.MoveNext
Loop
Categorias.Close
%>      

</table>
<table width="80%" border="0" cellpadding="0" cellspacing="0">
  <tr> 
    <td width="25%" align="left" valign="top"> 
      <p align="center">&nbsp;</p>
      </td>
    <td width="25%" valign="top">&nbsp;</td>
    <td width="25%" valign="top"> 
      <p>&nbsp;</p>
      </td>
    <td width="25%" valign="top"> 
      <p align="left">&nbsp;</p>
      </td>
  </tr>
</table>
<table width="770" border="0" align="center">
  <tr> 
    <td>
      <hr color=#cccccc size=1 height="1">
    </td>
  </tr>
  <tr> 
    <td> 
      <div align="center"><font 
      face="Verdana, Arial, Helvetica, sans-serif" size=1><font 
      color=#666666>Nova Conexão</font></font></div>
    </td>
  </tr>
</table>

</BODY></HTML>

<%
Conexao.Close

Set Categorias = Nothing
Set Conexao = Nothing
%>
