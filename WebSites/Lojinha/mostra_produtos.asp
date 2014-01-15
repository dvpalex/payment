<!--#INCLUDE FILE="Funcoes_Uteis.asp" -->
<!--#INCLUDE FILE="checa_senha.inc" -->
<% 
' Esta página só pode ser acessada se o visitante já se autenticou
' como administrador da loja
checa_senha()
%>
<HTML> 
<HEAD>
<TITLE>Administração de produtos</TITLE>
</HEAD>
<BODY BGCOLOR="#333399"> 
<%
' Abre conexao com banco de dados
Set Conexao = Server.CreateObject("ADODB.Connection")
Conexao.Open Application("StringConexaoODBC")

codigo_categoria = Request("codigo_categoria")

%>
<SCRIPT LANGUAGE="JavaScript">
<!--//
// rotina que o valor de <action> do form 'categoria', baseado no 'value' da lista. Esse
// valor é o link mostra_produtos?codigo_categoria=xx.
function escolhe_categoria() { 

   eval("document.categoria.action='" + document.categoria.lista.options[document.categoria.lista.selectedIndex].value + "'");
   return true;
}
//-->
</SCRIPT>

<h1><b><i><font color="#FF0000" size="5" face="Comic Sans MS">Relação de Produtos</font></i></b></h1>
<p><i><font color="#99CCFF" face="Comic Sans MS"><font size="4">Confira abaixo a relação dos produtos de sua loja.</font><br>
</font>
</i>
<font color="#CC0000" face="Comic Sans MS">
<br>
</font>
<font color="#FFFFFF" face="Comic Sans MS">Escolha uma categoria e clique em 'Mostrar Produtos'<br>
<br>
</font>
<form method="POST" onsubmit="return escolhe_categoria()" name="categoria" action="mostra_produtos.asp">
   <% 'Cria combo de categorias e o popula com links
    Call Cria_Combo_Categoria(codigo_categoria, 1)  %>
   <p><font color="#FFFFFF" face="Comic Sans MS"><input type="SUBMIT" name="submit" value="Mostrar Produtos"></font></p>
</form>

<table border="1">
  <tr> 
    <td width="10%"> 
      <div align="CENTER">
        <b><font color="#FFFFFF" face="Comic Sans MS">Código</font></b> 
      </div>
    </td>
    <td width="30%"> 
      <div align="CENTER">
        <b><font color="#FFFFFF" face="Comic Sans MS">Produto</font></b> 
      </div>
    </td>
    <td width="30%"> 
      <div align="CENTER">
        <b><font color="#FFFFFF" face="Comic Sans MS">Descrição</font></b> 
      </div>
    </td>
    <td width="10%"> 
      <div align="CENTER">
        <b><font color="#FFFFFF" face="Comic Sans MS">Preço Unitário</font></b> 
      </div>
    </td>
    <td width="20%"> 
      <div align="CENTER">
        <b><font color="#FFFFFF" face="Comic Sans MS">Imagem</font></b> 
      </div>
    </td>
  </tr>

<%
' Le informacoes dos produtos
Set RS_Produto = Server.CreateObject("ADODB.Recordset")
RS_Produto.Open "SELECT * FROM Produtos WHERE codigo_categoria=" & codigo_categoria, Conexao

' Checa se existem produtos no banco de dados
if RS_Produto.Eof then 
%>
<p><b><font color="#FFFFFF" face="Comic Sans MS">Não há produtos no seu seu banco de dados.<br>
<%

end if
' Mostra tabela de produtos

While Not RS_Produto.EOF
%> 
</font> 
  <tr> 
    <td width="10%"> 
        <a href="altera_produto.asp?codigo_produto=<%= RS_Produto("codigo_produto") %>"><font color="#CC0000" face="Comic Sans MS"><%= RS_Produto("codigo_produto") %></font></a> 
    </td>
    <td width="30%"> 
      <div align="LEFT">
        <font color="#CC0000" face="Comic Sans MS">
        <%= RS_Produto("nome_produto") %> 
        </font> 
      </div>
    </td>
    <td width="30%"> 
      <div align="LEFT">
        <font color="#CC0000" face="Comic Sans MS">
        <%= RS_Produto("descricao_produto") %> 
        </font> 
      </div>
    </td>
    <td width="10%"> 
      <div align="RIGHT">
        <font color="#CC0000" face="Comic Sans MS">
        <%= FormatCurrency(RS_Produto("preco_unitario")) %> 
        </font> 
      </div>
    </td>
    <td width="20%"> 
      <div align="RIGHT">
        <font color="#CC0000" face="Comic Sans MS">
        <%= RS_Produto("url_imagem") %> 
        </font> 
      </div>
    </td>
  </tr>
  <%

     RS_Produto.MoveNext
Wend

RS_Produto.Close
Conexao.Close

Set RS_Produto = Nothing
Set Conexao = Nothing

%> 
</table>
  <font color="#CC0000" face="Comic Sans MS">
<br>
  </font>
<form method="POST" action="cria_produto.asp">
  <p align="CENTER"><font color="#CC0000" face="Comic Sans MS"><input type="SUBMIT" name="criar" value="Adicionar Novo Produto">
  </font>
</form>
<hr>
<%
Call Pe_admin_produtos()
%>
</BODY> 
</HTML> 
