<%@ Language=VBScript %>

<HTML>
<HEAD>
<META NAME="GENERATOR" Content="Microsoft Visual Studio 6.0">
</HEAD>
<BODY>

<P>&nbsp;</P>

<%
	for each item in Request.Form
		Response.Write( item & "=" & Request(item) & "<br>" & vbCrLf)
	next
%>

</BODY>
</HTML>
