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
            if (!ValidaData(txtInicial.Text.Trim()))
                SuperPag.Helper.Ensure.IsNotNullOrEmpty(null, "Data inicial inválida");

            if (!ValidaData(txtFinal.Text.Trim()))
                SuperPag.Helper.Ensure.IsNotNullOrEmpty(null, "Data final inválida");

            GridRel.DataSource = PaymentAttemptBoleto.GetRelBoleto(Convert.ToDateTime(txtInicial.Text.Trim()), Convert.ToDateTime(txtFinal.Text.Trim()), Store.LocateStore(this.UserId).StoreId, this.txtNomeSacado.Text.Trim(), this.ddlMeioEnvio.SelectedValue, this.LimpaIPTE(this.txtIPTE.Text.Trim()), this.txtContrato.Text.Trim());
            GridRel.DataBind();
        }
        catch (Exception ex)
        {           
            GridRel.DataBind();

            SuperPag.Helper.GenericHelper.LogFile("BindRelatorio::Default.aspx.cs:: Ocorreu um erro ao gerar o relatório" + ex.Message, LogFileEntryType.Error);

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
            Convert.ToDateTime(Data);
            return true;
        }
        catch { return false; }
    }
}
