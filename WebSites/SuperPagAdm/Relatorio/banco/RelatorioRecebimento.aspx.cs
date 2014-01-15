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

    private void BindComboStatus()
    {
        try
        {
            
            ddlStatus.DataSource = CnabControleEntrada.getRecuperaStatus();
            ddlStatus.DataBind();
        }
        finally
        { }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            BindComboStatus();
            BindComboBancos();
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

        GridRel.Columns[0].Visible = false;

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
            if (!ValidaData(txtInicial.Text.Trim()))
                SuperPag.Helper.Ensure.IsNotNullOrEmpty(null, "Data inicial inválida");

            if (!ValidaData(txtFinal.Text.Trim()))
                SuperPag.Helper.Ensure.IsNotNullOrEmpty(null, "Data final inválida");

            if (!ValidaIntervaloData(txtInicial.Text.Trim(), txtFinal.Text.Trim()))
            {
                SuperPag.Helper.Ensure.IsNotNullOrEmpty(null, "Data Final deve ser maior ou igual a Data Inicial");
            }

            
            // Alteração : Jair Jersey Marinho - 22/10/2008
            GridRel.DataSource = CnabControleEntrada.getRelatorioRecebimentoCnab(Convert.ToInt32(rdTipoData.SelectedValue), Convert.ToDateTime(txtInicial.Text), AlteraHoraDataFinal(Convert.ToDateTime(txtFinal.Text)), Convert.ToInt32(ddlBanco.SelectedValue.ToString()), Convert.ToInt32(ddlStatus.SelectedValue.ToString()));
            GridRel.DataBind();
        }
        catch (Exception ex)
        {           
            //GridRel.DataBind();

            SuperPag.Helper.GenericHelper.LogFile("BindRelatorio::RelatorioEnvio.aspx.cs:: Ocorreu um erro ao gerar o relatório" + ex.Message, LogFileEntryType.Error);

            CustomRelatorio.IsValid = false;
            CustomRelatorio.ErrorMessage = ex.Message;
        }

        if (GridRel.Rows.Count > 0)
        {
            btnExportar.Visible = true;
        }
        else
        {
            btnExportar.Visible = false;
        }
    }
    protected void btnBuscar_Click(object sender, EventArgs e)
    {
        GridRel.PageIndex = 0;
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

    private DateTime AlteraHoraDataFinal(DateTime DataFinal)
    {
        TimeSpan time = new TimeSpan(23, 59, 59);

        DataFinal = DataFinal.Add(time);

        return DataFinal;
    }

    protected void VerTransacoes_click(object sender, CommandEventArgs e)
    {
        Response.Redirect("../transacoes/RelatorioResumoTransacoes.aspx?arquivo=" + e.CommandArgument.ToString());
        //Response.Redirect("../transacoes/RelatorioVisaoTransacoes.aspx?arquivo=" + e.CommandArgument.ToString());
    }
}
