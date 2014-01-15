using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SuperPag.Data.Messages;
using SuperPag.Business;
using System.IO;
using SuperPag;
using SuperPag.Business.Messages;
using System.Collections.Generic;

public partial class Relatorio_Default : System.Web.UI.Page
{
    private decimal ValorTotal;
    private decimal ValorTotalGeral;

    private Int32 QtdeTotal;
    private Int32 QtdeTotalGeral;

    public Guid UserId
    {
        get
        {
            return (Guid)Membership.GetUser(true).ProviderUserKey;
        }
    }

    private void BindComboBancos()
    {
        try
        {
            ddlBanco.DataSource = PaymentAgentSetupDebitoContaCorrente.ListadeBancos("Carrefour");
            ddlBanco.DataBind();
        }
        finally
        { }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            BindComboBancos();
        }
    }

    protected void btnExportar_Click(object sender, EventArgs e)
    {
        
        this.Bind();

        StringWriter ObjStringWriter = new StringWriter();
        HtmlTextWriter ObjHtmlTextWriter = new HtmlTextWriter(ObjStringWriter);
        HtmlForm ObjHtmlForm = new HtmlForm();

        Response.ContentType = "application/vnd.ms-excel";
        Response.AddHeader("content-disposition", "attachment;filename=Relatorio.xls");
        Response.Charset = "";

        EnableViewState = false;
        Controls.Add(ObjHtmlForm);

        ObjHtmlForm.Controls.Add(tblResumo);
        ObjHtmlForm.RenderControl(ObjHtmlTextWriter);

        Response.Write(ObjStringWriter.ToString());
        Response.End();

    }


    private void Bind()
    {
        try
        {
            LimpaTabela();

            if (!ValidaData(txtInicial.Text.Trim()))
                SuperPag.Helper.Ensure.IsNotNullOrEmpty(null, "Data inválida");

            PopularTabela();
        }
        catch (Exception ex)
        {           
            SuperPag.Helper.GenericHelper.LogFile("BindRelatorio::RelatorioEnvio.aspx.cs:: Ocorreu um erro ao gerar o relatório" + ex.Message, LogFileEntryType.Error);

            CustomRelatorio.IsValid = false;
            CustomRelatorio.ErrorMessage = ex.Message;
        }

    }
    protected void btnBuscar_Click(object sender, EventArgs e)
    {
        this.Bind();
    }

    private string LimpaIPTE(string strIPTE)
    {
        strIPTE = strIPTE.Replace(" ", string.Empty);
        strIPTE = strIPTE.Replace(".", string.Empty);
        return strIPTE;
    }

    private bool ValidaData(string Data)
    {
        try
        {
            DateTime dt = Convert.ToDateTime(Data);
            if (dt.Year < 1800)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        catch { return false; }
    }

    private DateTime AlteraHoraDataFinal(DateTime DataFinal)
    {
        TimeSpan time = new TimeSpan(23, 59, 59);

        DataFinal = DataFinal.Add(time);

        return DataFinal;
    }

    private void PopularTabela()
    {

        IList<MRelatorioRecebimentoCnab> rel = CnabControleEntrada.getRelatorioConferecia(Convert.ToDateTime(txtInicial.Text), AlteraHoraDataFinal(Convert.ToDateTime(txtInicial.Text)), 0, Convert.ToInt32(ddlBanco.SelectedValue));
        IList<MRelatorioRecebimentoCnab> relpg = CnabControleEntrada.getRelatorioConferecia(Convert.ToDateTime(txtInicial.Text), AlteraHoraDataFinal(Convert.ToDateTime(txtInicial.Text)), 1, Convert.ToInt32(ddlBanco.SelectedValue));
        IList<MRelatorioRecebimentoCnab> relrec = CnabControleEntrada.getRelatorioConferecia(Convert.ToDateTime(txtInicial.Text), AlteraHoraDataFinal(Convert.ToDateTime(txtInicial.Text)), 2, Convert.ToInt32(ddlBanco.SelectedValue));
        if (rel.Count > 0 || relpg.Count > 0 || relrec.Count > 0) //Verifica se tem Dados
        {
            CriarCabecario(); //Cria Cabecario
            /*****************************************/
            /* Impressão de Debito Facil Enviado     */
            /*****************************************/
            CriaLinhasDocs(rel, "Débito Fácil Enviado", true);
            
            /*****************************************/
            /* Impressão de Total de DOCS*/
            /*****************************************/
            
            /*****************************************/
            // CriarCabecario(); //Cria Cabecario
            /*****************************************/
            /* Impressão de Valores Pagos*/
            /*****************************************/
            CriaLinhasDocs(relpg, "Débitos Efetuados", true);
            /*****************************************/
            /* Impressão de Valores Recusados*/
            /*****************************************/
            CriaLinhasDocs(relrec, "Recusadas", true);
            /*****************************************/
            /* Impressão de Valores Pendentes*/
            /*****************************************/
            //IList<MRelatorioRecebimentoCnab> relp = CnabControleEntrada.getRelatorioConferecia(Convert.ToDateTime(txtInicial.Text), AlteraHoraDataFinal(Convert.ToDateTime(txtInicial.Text)), 8);
            //CriaLinhasDocs(relp, "RECURSADAS", true);
            /*****************************************/
            TotalGeral(); // Cria Total Geral

            btnExportar.Visible = true;
        }
        else
        {
            CriarLinhaSemResultado();

            btnExportar.Visible = false;
        }
    }
    private void CriaLinhasDocs(IList<MRelatorioRecebimentoCnab> rel,string titulo, bool calculatotal)
    {
        String[] valores = new String[3];
        QtdeTotal = 0;
        ValorTotal = 0;
        Int32 Qtde;
        decimal Valor;

        if (rel.Count == 0)
        {
            Qtde = 0;
            Valor = 0;
            valores[0] = "";
            valores[1] = Qtde.ToString();
            valores[2] = String.Format("{0:C2}", Valor);
            NovaLinha(titulo, valores, false, false);
        }
        else
        {
            for (Int32 i = 0; i < rel.Count; i++)
            {
                Qtde = rel[i].QtdeDetalhes;
                Valor = rel[i].ValorTotalDetalhes;

                valores[0] = rel[i].NumInstituicao.ToString();
                valores[1] = Qtde.ToString();
                valores[2] = String.Format("{0:C2}", Valor);

                NovaLinha(titulo, valores, false, false);
                titulo = "";
                QtdeTotal += Qtde;
                ValorTotal += Valor;

                if (calculatotal)
                {
                    QtdeTotalGeral += Qtde;
                    ValorTotalGeral += Valor;
                }
            }
        }

        valores[0] = "";
        valores[1] = QtdeTotal.ToString();
        valores[2] = String.Format("{0:C2}", ValorTotal);
        NovaLinha("Total", valores, true, false);

    }

    private void TotalGeral()
    {
        
        String[] valores = new String[3];
        valores[0] = "&nbsp;";
        valores[1] = "&nbsp;";
        valores[2] = "&nbsp;";
        NovaLinha("", valores, false, false);

        valores[0] = "";
        valores[1] = QtdeTotalGeral.ToString();
        valores[2] = String.Format("{0:C2}", ValorTotalGeral);
        NovaLinha("TOTAL GERAL", valores, true, true);

    }

    private void CriarCabecario()
    {
        TableRow tr = new TableRow();
        tr.Cells.Add(NovaColuna("&nbsp;",9));
        tr.Cells.Add(NovaColuna("Banco", 30));
        tr.Cells.Add(NovaColuna("Qtde", 30));
        tr.Cells.Add(NovaColuna("Valor", 30));
        tr.CssClass = "tdBarraFerramentasSUM";
        tblResumo.Rows.Add(tr);
    }

    private void NovaLinha(String titulo, String[] valores, Boolean bold, Boolean red)
    {
        TableRow tr = new TableRow();
        if (bold)
            tr.Style.Add("font-weight", "bold");
        if (red)
            tr.Style.Add("color", "red");
        tr.Cells.Add(NovaColuna(titulo, 15));
        tr.Cells.Add(NovaColuna(valores[0].ToString(), 14));
        tr.Cells.Add(NovaColuna(valores[1].ToString(), 30));
        tr.Cells.Add(NovaColuna(valores[2].ToString(), 30));
        tr.CssClass = "tbPainelFRM";
        tblResumo.Rows.Add(tr);

    }


    private TableCell NovaColuna(String Texto, Int32 Largura)
    {
        TableCell td = new TableCell();
        td.Text = Texto;
        td.Width = new Unit(Largura, UnitType.Percentage);
        return td;
    }

    private void LimpaTabela()
    {
        tblResumo.Rows.Clear();
    }

    private void CriarRodape() { 

    }

    private void CriarLinhaSemResultado()
    {
        TableRow tr = new TableRow();
        tr.Style.Add("font-weight", "normal");
        tr.Style.Add("color", "red");
        tr.Style.Add("text-align", "center");
        tr.Style.Add("background-color", "#ffffff");
        tr.Style.Add("height", "50px");
        tr.Cells.Add(NovaColuna("Não foi encontrado resultado para esta pesquisa.", 100));
        tr.CssClass = "tbPainelFRM";

        tblResumo.Rows.Add(tr);
    }
    protected void ddlBanco_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
}
