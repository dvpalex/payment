<%@ control language="C#" autoeventwireup="true" inherits="ListaUser, App_Web_toouxgw9" %>
<table class="tbPainelFRM">
    <tr>
        <td align="center" colspan="2" class="tdBarraFerramentasSUM">
            Usuário por ordem alfabética.
        </td>
    </tr>
    <tr>
        <td class="tdCampoFRM">
            <table>
                <tr>
                    <td id="TDStore" runat="server" visible="false" class="tdLabelFRM">Loja:</td>
                    <td id="TDStore1" runat="server" visible="false">
                        <asp:DropDownList ID="ddlStore" DataValueField="StoreId" DataTextField="Name" runat="server" Width="200px" AppendDataBoundItems="True" AutoPostBack="True" OnSelectedIndexChanged="ddlStore_SelectedIndexChanged">
                                 <asp:ListItem Text="Selecione" Value="-1"></asp:ListItem>
                        </asp:DropDownList>&nbsp;
                        <asp:Label ID="lblValidaLoja" runat="server" ForeColor="Red" Text="*" Visible="False"></asp:Label>
                    </td>
                    <td class="tdLabelFRM">Status:</td>
                    <td>
                        <asp:DropDownList ID="ddlFiltro" runat="server" Width="90px">
                            <asp:ListItem Value="true" Selected="True">Ativo</asp:ListItem>
                            <asp:ListItem Value="false">Inativo</asp:ListItem>
                            <asp:ListItem Value="-1">Todos</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:DataList ID="DataListLetras" runat="server" RepeatDirection="Horizontal">
                            <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                            <ItemTemplate>
                                <div align="center">
                                    <asp:Button ID="btnLetra"  runat="server" BackColor="#ffffff" BorderColor="#E0E0E0"
                                        BorderStyle="Solid" CausesValidation="false" Style="vertical-align: middle;"
                                        OnClick="SelectUsu" Text='<%# DataBinder.Eval(Container.DataItem,"Campo") %>'
                                        CommandArgument='<%# DataBinder.Eval(Container.DataItem,"Campo") %>' />
                                </div>
                            </ItemTemplate>
                        </asp:DataList>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td class="tdCampoFRM">
            <asp:Repeater ID="rptUsuarios" runat="server" OnItemCommand="rptUsuarios_ItemCommand"
                OnItemDataBound="rptUsuarios_ItemDataBound">
                <HeaderTemplate>
                    <table class="tbPainelFRM">
                        <tr>
                            <td class="tdBarraFerramentasSUM" style="padding-left: 15px;">
                                Ativo
                            </td>
                            <td class="tdBarraFerramentasSUM">
                                Nome do usuário
                            </td>
                            <td class="tdBarraFerramentasSUM">
                                Editar usuário
                            </td>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td class="tdCampoFRM" style="padding-left: 15px;">
                            <asp:CheckBox ID="ChStatus" AutoPostBack="true" Enabled="false" runat="server" Visible="false"
                                OnCheckedChanged="ChStatus_CheckedChanged" /></td>
                        <td class="tdCampoFRM">
                            <asp:Label ID="lblNome" runat="server" Visible="false"></asp:Label></td>
                        <td class="tdCampoFRM">
                            <asp:LinkButton ID="lbEditar" runat="server" CommandName="editar" Visible="false">Editar</asp:LinkButton>
                        </td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </table>
                </FooterTemplate>
            </asp:Repeater>
            <asp:Panel ID="PanelPagnacao" runat="server" Visible="False" Width="288px">
            <asp:Button ID="btnPrev" runat="server" OnClick="btnPrev_Click" Text="Anterior" Width="90px" Enabled="False" />
            <asp:Label ID="lblCurrentPage" runat="server" Font-Names="Verdana" Font-Size="8pt" ForeColor="#A9A9A9"></asp:Label>
            <asp:Button ID="btnNext" runat="server" OnClick="btnNext_Click" Text="Próximo" Width="90px" Enabled="False" /></asp:Panel>
    </tr>
</table>
