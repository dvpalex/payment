<!--#INCLUDE FILE="Funcoes_Uteis.asp" -->

<%
' Pagina de produtos

' Abre conexao com banco de dados. Alterar a string de conexao em global.asa
Set Conexao = Server.CreateObject("ADODB.Connection")
Conexao.Open Application("StringConexaoODBC")

'passado por procura
produto          = Request("procura")

'passado quando se seleciona uma categoria
'codigo_categoria = Request("codigo_categoria")
'nome_categoria   = Request("nome_categoria")
codigo_categoria  = 1	

' Abre tabela de produtos usando a conexao aberta acima.
Set Produtos = Server.CreateObject("ADODB.Recordset")

If Request("procura") <> "" Then
'esta consulta utiliza o comando LIKE que procura por textos aproximados
'SELECT.... WHERE nome_produto LIKE '%eletronico%' OR descricao_produto LIKE '%eletronico%'
'Response.write "SELECT DISTINCT codigo_produto, codigo_categoria, nome_produto, descricao_produto, preco_unitario, url_imagem, disponivel FROM produtos WHERE nome_produto like '%" & produto & "%' OR descricao_produto LIKE '%" & produto & "%' AND disponivel = 0"
'Response.End
	Produtos.Open "SELECT DISTINCT codigo_produto, codigo_categoria, nome_produto, descricao_produto, preco_unitario, url_imagem, disponivel FROM produtos WHERE nome_produto like '%" & produto & "%' OR descricao_produto LIKE '%" & produto & "%' AND disponivel = 0", Conexao
Else
' quando se escolhe uma categoria
	Produtos.Open "SELECT codigo_produto, codigo_categoria, nome_produto, descricao_produto, preco_unitario, url_imagem, disponivel FROM Produtos WHERE codigo_categoria=" & codigo_categoria & " AND disponivel = 0", Conexao
End If
%>

<!--
<HTML>
<HEAD>
<TITLE>Produtos</TITLE>
<meta http-equiv="Content-Type" content="text/html; charset=">
</HEAD>

<BODY BGCOLOR="#333399" text="#CC0000">
<h1><b><i><font color="#FF0000" size="5" face="Comic Sans MS">Produtos</font><font color="#000066" size="5" face="Comic Sans MS">&nbsp;<img border="0" src="images/produtos.gif" width="323" height="51"></font></i></b></h1>
<p><i><font face="Comic Sans MS" color="#99CCFF">Veja abaixo nosso catálogo de produtos para a categoria <b><%= nome_categoria %></b>.&nbsp;&nbsp;</font></i></p>
<p><i><font face="Comic Sans MS" color="#99CCFF">Clique sobre um produto para incluir 
  no carrinho de compras.</font></i></p>
<table border="0" cellpadding="6">

<%
' Checa se existem produtos no banco de dados
'If Produtos.Eof Then 

'	If Request("procura") = "" Then
%>
<p><b><font face="Comic Sans MS" color="#FFFFFF">Não há produtos para essa categoria no banco de dados.<br>
<%
'	Else
%>
</font>
<p><font face="Comic Sans MS"><font color="#FFFFFF">Não foi encontrado nenhum produto.</font><br>
<%
'	End If
'Else

' Le codigo, nome do produto, preco e URL da imagem para escrever as linhas da tabela,
' ja usando o codigo do produto para montar o link para a pagina de carrinho de compras
' Quanto executado o codigo abaixo retorna um trecho de HTML conforme o exemplo:
'  <tr>
'    <td><img src="images/rotrphon.jpg"></td>
'    <td><a href="carrinho.asp?produto_codigo=2">Telefone vermelho: 20,00</a><br>
'      Telefone com disco rotativo de dez posições. Discagem por pulso.</td>
'  </tr>

'   Produtos.MoveFirst
'   While Not Produtos.EOF
%>
</font></b>
  <tr>
    <td><font face="Comic Sans MS"><img src="<%= Produtos("url_imagem") %>"></font></td> 
    <td>
      <p><font face="Comic Sans MS"><a href="carrinho.asp?codigo_produto=<%=produtos("codigo_produto")%>"><%=Produtos("nome_produto")%></a>: <%=FormatCurrency(produtos("preco_unitario"))%><br><%=produtos("descricao_produto")%></font></p>
    </td>
  </tr>

<%
'     Produtos.MoveNext
'   Wend

'End If 

'Produtos.Close
'Conexao.Close

'Set Produtos = Nothing
'Set Conexao = Nothing

%>

</table>
<p align="CENTER">&nbsp;</p>
<p align="CENTER">
<a href="<%= Application("URL_Categorias") %>" ><i><font face="Comic Sans MS">Voltar as categorias de produtos</font></i></a>
<br>
<p align="CENTER">&nbsp;</p>
</BODY>
</HTML>
-->

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML><HEAD><TITLE>Nova Conexão</TITLE>
<META http-equiv=Content-Type content="text/html; charset=windows-1252"><!-- frames -->
<SCRIPT language=JavaScript>
	function MM_goToURL2(url) 
	{ 
		if (url == '1')
			{	
				self.location  = document.frmMain.select.value;
			}
		if (url == '2')
			{
				self.location  = "/main.asp?SessionId=&grafico=&grupo=" + document.frmMain.grupo.value ;
			}
	}
	function SubmitNewsletter()
	{
		if(document.FrmNewsletter.strNome.value == '')
		{
			window.alert('Preencha o nome para o cadastro.');
			window.FrmNewsletter.strNome.focus();
			return false;
		}
		if (document.FrmNewsletter.strMail.value == '')
		{
			window.alert('Preencha o e-mail para o cadastro.');
			window.FrmNewsletter.strMail.focus();
			return false;
		}
		window.FrmNewsletter.action = 'insere_news.asp'
		window.FrmNewsletter.submit();
	
	}
</SCRIPT>

<META content="MSHTML 5.50.4807.2300" name=GENERATOR></HEAD>

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
          &eacute; o nosso cat&aacute;logo de mercadorias - <%=request("nome_categoria")%><br>
          </font></font></b></font>
          </p>
      </div>
    </td>
  </tr>

<%
iLoop = 0
Produtos.MoveFirst
While Not Produtos.EOF
%>
<%if iLoop Mod 2 = 0 then%>
  <tr valign="top"> 
    <td bgcolor="#FFFFFF" colspan="4"> 
      <hr color=#cccccc size=1 height="1">
    </td>
  </tr>
  <tr bordercolor="#FFFFFF"> 
<%end if%>
    <td width="25%" valign="top"> 
      <div align="center"><font face="Verdana, Arial, Helvetica, sans-serif"><b><img src="<%= Produtos("url_imagem") %>" align="middle"></b></font></div>
    </td>
    <td width="25%" valign="top"> 
      <p><font size="2" face="Verdana, Arial, Helvetica, sans-serif" color="#999999">
      <b><%=Produtos("nome_produto")%></b></font><font size="1" face="Verdana, Arial, Helvetica, sans-serif"><br>
        <br><%=produtos("descricao_produto")%>
        <br><br>R<%=FormatCurrency(produtos("preco_unitario"))%></font></p>
      <p><a href="carrinho.asp?codigo_produto=<%=produtos("codigo_produto")%>"><img src="images/comprar.gif" width="80" height="26" border=0></a><font size="1" face="Verdana, Arial, Helvetica, sans-serif"> 
        </font></p>
    </td>
<%if iLoop Mod 2 <> 0 then%>
  </tr>  
<%end if%>
<%
	iLoop = iLoop + 1
  Produtos.MoveNext
Wend
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
Produtos.Close
Conexao.Close

Set Produtos = Nothing
Set Conexao = Nothing
%>
