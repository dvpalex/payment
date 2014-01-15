<%@ Page language="c#" Inherits="StoreTest._Default" enableViewState="False" CodeFile="Default.aspx.cs" %>
<HTML>
	<HEAD>
		<title>Loja Teste SuperPag</title>
	</HEAD>
	<body bgcolor="#dcdcdc">
		<form id="Form1" method="post" runat="server">
			<table cellpadding="4" border="0">
			<tr>
			    <td colspan="2" align="center">
			        <b>Lojinha de Testes SuperPag</b>
			    </td>
			</tr>
			<tr>
			    <td align="right" valign="top">Tipo de Handshake:</td>
			    <td>
			        <asp:RadioButtonList id="rblHandshake" runat="server" RepeatDirection="Horizontal" Width="607px" RepeatColumns="4">
					    <asp:ListItem Value="XML" Selected="True">XML</asp:ListItem>
					    <asp:ListItem Value="HTML2">HTML 2 Passos</asp:ListItem>
					    <asp:ListItem Value="HTML1">HTML 1 Passo</asp:ListItem>
					    <asp:ListItem Value="WSREQ">Web Service Request</asp:ListItem>
                        <asp:ListItem Value="WSUP">Web Service Update</asp:ListItem>
                        <asp:ListItem Value="WSCHK">Web Service Check</asp:ListItem>
                        <asp:ListItem Value="WSCNL">Web Service Cancel</asp:ListItem>
				    </asp:RadioButtonList>
			        <asp:TextBox id="txtIncludeHandshake" runat="server" Width="600px" TextMode="MultiLine" />
			    </td>
			</tr>
			<tr>
			    <td align="right" valign="top">Chave:</td>
			    <td>
			        <asp:RadioButtonList id="rblChave" runat="server" RepeatDirection="Vertical">
					    <asp:ListItem Value="2DF5C4349AB047EFFC769546AF2E316AB8DC8E5954" Selected="True">Loja Teste</asp:ListItem>
					    <asp:ListItem Value="2DF5C4349AB047EFFC769546AF2E316AB8DC8E5955">Loja Localhost</asp:ListItem>
					    <asp:ListItem Value="0">Outra:</asp:ListItem>
				    </asp:RadioButtonList>
				    <asp:TextBox id="txtChave" runat="server" Width="600px" />
			    </td>
			</tr>
			<tr>
			    <td align="right" valign="top">Pedido loja:</td>
			    <td><asp:TextBox id="txtPedido" runat="server" Width="87px" />
                    (se vazio o no. do pedido será gerado)</td>
			<tr>
			    <td align="right" valign="top">Idioma:</td>
			    <td>
			        <asp:RadioButtonList ID="rblIdioma" runat="server" RepeatDirection="Horizontal">
			        	<asp:ListItem Value="pt-br">Português</asp:ListItem>
					    <asp:ListItem Value="en">Inglês</asp:ListItem>
					    <asp:ListItem Value="es">Espanhol</asp:ListItem>
                    </asp:RadioButtonList>			    
			    </td>
			</tr>
			<tr>
			    <td colspan="2" align="right">
			        <asp:button id="btnIniciaHandshake" runat="server" text="Inicia Handshake" onclick="btnIniciaHandshake_Click" />
			    </td>
			</tr>
			</table>
            <asp:Label ID="lblRetorno" runat="server" Height="174px" Width="717px"></asp:Label>
		</form>
	</body>
</HTML>
