<%@ Language=VBScript %>
<HTML>
<HEAD>
<META NAME="GENERATOR" Content="Microsoft Visual Studio 6.0">
</HEAD>
<BODY>
<%
dim tst1
set tst1 = server.CreateObject("CDONTS.NewMail")
tst1.Subject = "Teste Email"
tst1.To = "fabio.tavares@e-financial.com.br"
tst1.From = "fabio teste"
tst1.Body = "este foi um teste - espero que funcione"
tst1.Send "fabio teste", "fabio.tavares@santosseg.com.br", "teste email", "este eh apenas um teste"
%>

<P>TESTE EMAIL</P>

</BODY>
</HTML>
