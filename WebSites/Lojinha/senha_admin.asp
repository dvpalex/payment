<HTML>
<HEAD>
<TITLE>Logon</TITLE>
<meta http-equiv="Content-Type" content="text/html; charset=">
</HEAD>

<BODY BGCOLOR="#333399">
<h1><font color="#FF0000" face="Comic Sans MS" size="5">Digite seu login</font></h1>
<form method="POST" action="<%= Application("URL_Admin_Seguro") %>">
<table border="0">
     <tr>
	<td>	
      <font face="Comic Sans MS" color="#FFFFFF">	
	  Senha:</font> 
	</td>	
	<td>	
      <font face="Comic Sans MS" color="#FFFFFF">	
	  <input type="PASSWORD" name="senha" size="20"><br>
      </font>
	</td>	
     </tr>
</table>
<font face="Comic Sans MS" color="#CC0000">
<br><input type="SUBMIT" name="submit" value="Entrar">  
</font>  
</form>
</BODY>
</HTML>
