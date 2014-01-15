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

public partial class Relatorio_Default : System.Web.UI.Page
{
    public Guid UserId
    {
        get
        {
            return (Guid)Membership.GetUser(true).ProviderUserKey;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            String arquivo = Request.QueryString.Get("arquivo");
            String numeroInstituicao = Request.QueryString.Get("numeroInstituicao");
            if (arquivo != null && numeroInstituicao == null)
            {
                txtArquivo.Value = arquivo;
                txtNumeroInstituicao.Value = null;
                this.Bind();
            }
            else if (arquivo != null && numeroInstituicao != null)
            {
                txtArquivo.Value = arquivo;
                txtNumeroInstituicao.Value = numeroInstituicao;
                this.Bind();
            }
        }
    }

    protected void btnExportar_Click(object sender, EventArgs e)
    {
        GridRel.AllowPaging = false;
        this.Bind();

        StringWriter ObjStringWriter = new StringWriter();
        HtmlTextWriter ObjHtmlTextWriter = new HtmlTextWriter(ObjStringWriter);
        HtmlForm ObjHtmlForm = new HtmlForm();

        Response.ContentType = "application/vnd.ms-excel";
        Response.AddHeader("content-disposition", "attachment;filename=Relatorio.xls");
        Response.Charset = "";

        EnableViewState = false;
        Controls.Add(ObjHtmlForm);

        ObjHtmlForm.Controls.Add(GridRel);
        ObjHtmlForm.RenderControl(ObjHtmlTextWriter);

        Response.Write(ObjStringWriter.ToString());
        Response.End();

    }

    protected void GridRel_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridRel.PageIndex = e.NewPageIndex;
        this.Bind();
    }

    private void Bind()
    {
        try
        {
            if (txtArquivo.Value == String.Empty)
            {
                if (txtInicial.Text != "" || txtFinal.Text != "")
                {
                    if (!ValidaData(txtInicial.Text.Trim()))
                        SuperPag.Helper.Ensure.IsNotNullOrEmpty(null, "Data inicial inv lida");
                    if (!ValidaData(txtFinal.Text.Trim()))
                        SuperPag.Helper.Ensure.IsNotNullOrEmpty(null, "Data final inv lida");
                    if (!ValidaIntervaloData(txtInicial.Text.Trim(), txtFinal.Text.Trim()))
                    {
                        SuperPag.Helper.Ensure.IsNotNullOrEmpty(null, "Data Final deve ser maior ou igual a Data Inicial");
                    }
                }
                GridRel.DataSource = PaymentAttemptContaCorrente.getRelatorioAnalitico(Convert.ToInt32(rdTipoData.SelectedValue), Convert.ToDateTime(txtInicial.Text), AlteraHoraDataFinal(Convert.ToDateTime(txtFinal.Text)));
                GridRel.DataBind();
                if (GridRel.Rows.Count > 0)
                {
                    btnExportar.Visible = true;
                }
                else
                {
                    btnExportar.Visible = false;
                }
            }
            else if (txtArquivo.Value != String.Empty && txtNumeroInstituicao.Value == String.Empty)
            {
                GridRel.DataSource = PaymentAttemptContaCorrente.getRelatorioAnalitico(txtArquivo.Value);
                GridRel.DataBind();
            }
            else if (txtArquivo.Value != String.Empty && txtNumeroInstituicao.Value != String.Empty)
            {
                GridRel.DataSource = PaymentAttemptContaCorrente.getRelatorioAnalitico(txtArquivo.Value, Convert.ToInt32(txtNumeroInstituicao.Value));
                GridRel.DataBind();
            }
        }
        catch (Exception ex)
        {
            GridRel.DataBind();
            SuperPag.Helper.GenericHelper.LogFile("BindRelatorio::RelatorioEnvio.aspx.cs:: Ocorreu um erro ao gerar o relat¢rio" + ex.Message, LogFileEntryType.Error);
            CustomRelatorio.IsValid = false;
            CustomRelatorio.ErrorMessage = ex.Message;
        }

    }


    private DateTime validadata(String value, Boolean datafinal)
    {
        if (value != "")
        {
            if(datafinal)
                return Convert.ToDateTime(value).AddDays(1);
            else
                return Convert.ToDateTime(value);
        }
        else
            return new DateTime();
    }

    protected void btnBuscar_Click(object sender, EventArgs e)
    {
        GridRel.PageIndex = 0;
        txtArquivo.Value = String.Empty;
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

    private bool ValidaIntervaloData(string DataInicial, string DataFinal)
    {
        try
        {
            DateTime dtIni = Convert.ToDateTime(DataInicial);
            DateTime dtFim = Convert.ToDateTime(DataFinal);

            if (dtIni.CompareTo(dtFim) <= 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        catch { return false; }
    }
}
