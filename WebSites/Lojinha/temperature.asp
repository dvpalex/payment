<%@ Language=VBScript %>
<% Option Explicit %>
<%
  Dim objTempConv
  Set objTempConv = Server.CreateObject("Temperature.Convert")
%>

<HTML>
<BODY>

<P>&nbsp;</P>

</BODY>
</HTML>

<%
  Set objTempConv = Nothing      'Clean up!
%>

