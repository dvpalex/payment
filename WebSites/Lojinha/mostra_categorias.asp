<!--#INCLUDE FILE="Funcoes_Uteis.asp" -->
<HTML> 
<HEAD>
<TITLE>Administração de categorias</TITLE>
</HEAD>
<BODY BGCOLOR="#333399"> 

<h1><b><i><font face="Comic Sans MS" size="5" color="#FF0000">Relação de Categorias</font></i></b></h1>
<p><i><font size="4" color="#99CCFF" face="Comic Sans MS">Confira abaixo a relação das categorias de sua loja.</font><font face="Comic Sans MS" color="#CC0000"><br>
</font>
</i>
<table border="1">
  <tr> 
    <td width="10%"> 
      <div align="CENTER">
        <b><font face="Comic Sans MS" color="#FFFFFF">Código</font></b> 
      </div>
    </td>
    <td width="35%"> 
      <div align="CENTER">
        <b><font face="Comic Sans MS" color="#FFFFFF">Categoria</font></b> 
      </div>
    </td>
    <td width="65%"> 
      <div align="CENTER">
        <b><font face="Comic Sans MS" color="#FFFFFF">Descrição</font></b> 
      </div>
    </td>
  </tr>

<%
' Abre conexao com banco de dados
Set Conexao = Server.CreateObject("ADODB.Connection")
Conexao.Open Application("StringConexaoODBC")

' Le informacoes das Categorias 
Set Categorias = Server.CreateObject("ADODB.Recordset")
Categorias.Open "SELECT * FROM Categorias", Conexao

' Checa se existem Categorias no banco de dados
if Categorias.Eof then 
%>
<p><b><font face="Comic Sans MS" color="#FFFFFF"><font size="3">Não há categorias no seu seu banco de dados.</font><br>
<%

end if
' Mostra tabela de categorias 

While Not categorias.EOF
%> 
</font> 
  <tr> 
    <td width="10%"> 
        <a href="altera_categoria.asp?codigo_categoria=<%= Categorias("codigo_categoria") %>"><font face="Comic Sans MS" color="#FFFFFF"><%= Categorias ("codigo_categoria") %></font></a> 
    </td>
    <td width="35%"> 
      <div align="LEFT">
        <font face="Comic Sans MS" color="#FFFFFF">
        <%= Categorias("nome_categoria") %> 
        </font> 
      </div>
    </td>
    <td width="65%"> 
      <div align="LEFT">
        <font face="Comic Sans MS" color="#FFFFFF">
        <%= Categorias("descricao_categoria") %> 
        </font> 
      </div>
    </td>
  </tr>
  <%

     Categorias.MoveNext
Wend

Categorias.Close
Conexao.Close

Set Categorias = Nothing
Set Conexao = Nothing

%> 
</table>
  <font face="Comic Sans MS" color="#CC0000">
<br>
  </font>
<form method="POST" action="cria_categoria.asp">
  <p align="CENTER"><font face="Comic Sans MS" color="#CC0000"><input type="SUBMIT" name="criar" value="Adicionar Nova Categoria">
  </font>
</form>
<hr>
<%
Call Pe_admin_produtos()
%>
</BODY> 
</HTML> 
