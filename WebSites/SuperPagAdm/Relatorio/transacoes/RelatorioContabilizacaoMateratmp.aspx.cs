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

    public Guid UserId
    {
        get
        {
            return (Guid)Membership.GetUser(true).ProviderUserKey;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
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

    private void LimpaTabela()
    {
        tblResumo.Rows.Clear();
    }

    private void PopularTabela()
    {

        IList<MRelatorioContabilizacaoMatera> relcsu = PaymentAttemptContaCorrente.getRelatorioContabilizacaoMatera(0, Convert.ToDateTime(txtInicial.Text), AlteraHoraDataFinal(Convert.ToDateTime(txtInicial.Text)));
        IList<MRelatorioContabilizacaoMatera> relcnab = PaymentAttemptContaCorrente.getRelatorioContabilizacaoMatera(1, Convert.ToDateTime(txtInicial.Text), AlteraHoraDataFinal(Convert.ToDateTime(txtInicial.Text)));

        if (relcsu.Count > 0 || relcnab.Count > 0) //Verifica se tem Dados
        {
            CriaLinhasDocs(relcsu, 0, "Processadas CSU", true);
            CriaLinhasDocs(relcnab, 1, "Aceitas Banco", true);
            TotalGeral(); // Cria Total Geral

            btnExportar.Visible = true;
        }
        else
        {
            CriarLinhaSemResultado();

            btnExportar.Visible = false;
        }
    }

    private void CriaLinhasDocs(IList<MRelatorioContabilizacaoMatera> rel, int tipo, string titulo, bool calculatotal)
    {
        String[] valores = new String[9];
        ValorTotal = 0;
        decimal Valor;
        decimal SubTotalBanco = 0;
        string NumBanco = String.Empty;

        if (rel.Count > 0)
        {
            NovaLinhaTitulo(titulo);
            CriarCabecalho(tipo);

            for (Int32 i = 0; i < rel.Count; i++)
            {
                Valor = rel[i].Valor;

                if (rel[i].NumBanco.Equals(NumBanco))
                {
                    SubTotalBanco += rel[i].Valor;
                }
                else
                {
                    SubTotalBanco = rel[i].Valor;
                    NumBanco = rel[i].NumBanco;
                }

                valores[0] = rel[i].NumBanco;
                valores[1] = rel[i].NomeBanco;
                valores[2] = rel[i].ContaCorrente.ToString() ;
                valores[3] = rel[i].ContaTransit.ToString();
                valores[4] = String.Format("{0:dd/MM/yyyy}", rel[i].DataProcessamento);
                valores[5] = String.Format("{0:dd/MM/yyyy}", rel[i].DataOperacao);
                valores[6] = tipo == 0 ? "&nbsp;" : String.Format("{0:dd/MM/yyyy}", rel[i].DataCredito);
                valores[7] = String.Format("{0:C2}", Valor);
                valores[8] = tipo == 0 ? "&nbsp;" : String.Format("{0:C2}", SubTotalBanco);

                NovaLinha(valores, false, false);
                titulo = "";
                ValorTotal += Valor;

                if (calculatotal)
                {
                    ValorTotalGeral += Valor;
                }
            }

            valores[0] = "TOTAL";
            valores[1] = "&nbsp";
            valores[2] = "&nbsp";
            valores[3] = "&nbsp";
            valores[4] = "&nbsp";
            valores[5] = "&nbsp";
            valores[6] = "&nbsp";
            valores[7] = String.Format("{0:C2}", ValorTotal);
            valores[8] = "&nbsp;";
            NovaLinha(valores, true, false);
        }

    }

    private void TotalGeral()
    {
        NovaLinhaTitulo("Total do Relatorio");

        String[] valores = new String[9];

        valores[0] = "TOTAL";
        valores[1] = "&nbsp";
        valores[2] = "&nbsp";
        valores[3] = "&nbsp";
        valores[4] = "&nbsp";
        valores[5] = "&nbsp";
        valores[6] = "&nbsp";
        valores[7] = String.Format("{0:C2}", ValorTotalGeral);
        valores[8] = "&nbsp;";
        NovaLinha(valores, true, true);

    }

    private TableCell NovaColuna(String Texto, Int32 Largura)
    {
        TableCell td = new TableCell();
        td.Text = Texto;
        td.Width = new Unit(Largura, UnitType.Percentage);
        return td;
    }

    private TableCell NovaColuna(String Texto, Int32 Largura, Int32 ColumnSpan, HorizontalAlign hAlign)
    {
        TableCell td = new TableCell();
        td.Text = Texto;
        td.Width = new Unit(Largura, UnitType.Percentage);
        td.ColumnSpan = ColumnSpan;
        td.HorizontalAlign = hAlign;
        return td;
    }

    private void NovaLinhaTitulo(String titulo)
    {
        TableRow tr = new TableRow();
        tr.Style.Add("font-weight", "bold");
        tr.Cells.Add(NovaColuna(titulo, 100, 9, HorizontalAlign.Left));
        tblResumo.Rows.Add(tr);
    }

    private void NovaLinha(String[] valores, Boolean bold, Boolean red)
    {
        TableRow tr = new TableRow();
        if (bold)
            tr.Style.Add("font-weight", "bold");
        if (red)
            tr.Style.Add("color", "red");
        tr.Cells.Add(NovaColuna(valores[0].ToString(), 5));
        tr.Cells.Add(NovaColuna(valores[1].ToString(), 10));
        tr.Cells.Add(NovaColuna(valores[2].ToString(), 9));
        tr.Cells.Add(NovaColuna(valores[3].ToString(), 9));
        tr.Cells.Add(NovaColuna(valores[4].ToString(), 13));
        tr.Cells.Add(NovaColuna(valores[5].ToString(), 13));
        tr.Cells.Add(NovaColuna(valores[6].ToString(), 10));
        tr.Cells.Add(NovaColuna(valores[7].ToString(), 10));
        tr.Cells.Add(NovaColuna(valores[8].ToString(), 13));
        tr.CssClass = "tbPainelFRM";
        tblResumo.Rows.Add(tr);

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

    private void CriarCabecalho(int tipo)
    {
        TableRow tr = new TableRow();
        tr.Cells.Add(NovaColuna("Banco", 5));
        tr.Cells.Add(NovaColuna("Nome", 10));
        tr.Cells.Add(NovaColuna("Conta Banco", 11));
        tr.Cells.Add(NovaColuna("Conta Transit.", 11));
        tr.Cells.Add(NovaColuna("Dt Processamento", 15));
        tr.Cells.Add(NovaColuna("Dt Operacao", 13));
        tr.Cells.Add(NovaColuna(tipo == 0 ? "&nbsp;" : "Dt Credito", 10));
        tr.Cells.Add(NovaColuna("Valor", 10));
        tr.Cells.Add(NovaColuna(tipo == 0 ? "&nbsp;" : "SubTotal Banco", 13));
        tr.CssClass = "tdBarraFerramentasSUM";
        tblResumo.Rows.Add(tr);
    }
}
