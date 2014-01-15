<%@ Language=VBScript %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML><HEAD><TITLE>Nova Conexão</TITLE>
<META http-equiv=Content-Type content="text/html; charset=windows-1252"><!-- frames -->
<SCRIPT language=JavaScript>
function Valida(){
	
	if( document.formLogin.passloja.value == "" )
	{
		alert("Por favor, informe a senha de acesso.");
		document.formLogin.passloja.focus();
	}
	else
		document.formLogin.submit();
	
}
</SCRIPT>

<META content="MSHTML 5.50.4807.2300" name=GENERATOR></HEAD>

<BODY bgColor=#ffffff leftMargin=0 topMargin=0 marginheight="0" marginwidth="0" onload="document.formLogin.passloja.focus();">

<form action="login.asp" method=post name=formLogin>

<table width="780" border="0" cellpadding="2" cellspacing="0" align="center">
  <tr valign="top"> 
    <td bgcolor="#FFFFFF" colspan="3"> 
      <p><font size="2" face="Verdana, Arial, Helvetica, sans-serif"><b><font color="#999999" size="3" face="Arial, Helvetica, sans-serif">&nbsp;&nbsp;</font></b></font></p>
    </td>
  </tr>
  <tr> 
    <td width="401" rowspan="2"><img src="images/GRUPO.jpg" width="401" height="305"></td>
    <td valign="middle" colspan="2"> 
      <p style="text-align:center
	  "><font size="2" face="Verdana, Arial, Helvetica, sans-serif"><b><font color="#999999" size="3">Bem-vindo 
        &agrave; <br>
        Loja Virtual de Teste SuperPag</font></b></font></p>
      <p style="text-align:justify"><font size="1" face="Verdana, Arial, Helvetica, sans-serif"><br>
        <font size="2">Comprar nesta loja &eacute; muito f&aacute;cil!<br>
        </font></font><font face="Verdana, Arial, Helvetica, sans-serif" size="2">Conhe&ccedil;a 
        nosso cat&aacute;logo e confira nossas promo&ccedil;&otilde;es</font></p>

		<% if Request("erro")=1 then %>		      
		    <p><br><br><br><font face="Arial, Helvetica, sans-serif" color="red" size="2">
		    <b>Senha não confere! Por favor, redigite novamente.</b></font>
		<%end if%>  
        
    </td>
  </tr>

  <tr> 
    <td valign="middle" width="252"> 
      <div align="right">
      <font size="1" face="Verdana, Arial, Helvetica, sans-serif">
      <b><font face="Arial, Helvetica, sans-serif" color="#ff0000" size="5">
      <i><font size="3" color="#0099CC">Senha:</font></i>
      </font><font face="Comic Sans MS" color=#ff0000 size=5>
<input id=passloja type=password name=passloja autocomplete="off">
        </font><font size="1" face="Verdana, Arial, Helvetica, sans-serif"><b><font face="Comic Sans MS" color=#ff0000 size=5> 
        </font></b></font></b></font></div>
    </td>
    <td valign="middle" width="171" align="center"> 
      <div align="left"><font size="1" face="Verdana, Arial, Helvetica, sans-serif"><b><font size="1" face="Verdana, Arial, Helvetica, sans-serif"><b><font face="Comic Sans MS" color=#ff0000 size=5><a href="javascript:Valida();" target="main"><img src="images/botao.gif" width="54" height="26" border="0" vspace="0" align="middle"></a></font></b></font></b></font></div>
    </td>
  </tr>
</table>
<table width="80%" border="0">
  <tr> 
    <td width="13%">&nbsp;</td>
    <td width="87%">&nbsp;</td>
  </tr>
</table>
<table width="780" border="0" align="center">
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

</form>

</BODY></HTML>
