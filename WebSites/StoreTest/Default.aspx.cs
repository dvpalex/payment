using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using SuperPag.Helper;
using System.IO;
using System.Xml;
using System.Globalization;

namespace StoreTest
{
	public partial class _Default : System.Web.UI.Page
	{
		protected void Page_Load(object sender, System.EventArgs e)
		{
            if (!Page.IsPostBack)
            {
                if(Session["includeHandshakeStoreTest"] != null)
                    txtIncludeHandshake.Text = Session["includeHandshakeStoreTest"].ToString();
                if (Session["handshakeTypeStoreTest"] != null)
                    rblHandshake.SelectedValue = Session["handshakeTypeStoreTest"].ToString();
            }
		}

        protected void btnIniciaHandshake_Click(object sender, System.EventArgs e)
		{
            lblRetorno.Text = "";

            Session["handshakeTypeStoreTest"] = rblHandshake.SelectedValue;
            Session["includeHandshakeStoreTest"] = txtIncludeHandshake.Text;
            
            string pedido = (String.IsNullOrEmpty(txtPedido.Text.Trim()) ? System.DateTime.Now.Ticks.ToString() : txtPedido.Text.Trim());

            switch (rblHandshake.SelectedValue)
            {
                case "XML":
                    
                    if (txtIncludeHandshake.Text.StartsWith("<?xml"))
                        Application["xmlDoc"] = txtIncludeHandshake.Text;
                    else
                        Application["xmlName"] = String.IsNullOrEmpty(txtIncludeHandshake.Text) ? "handshake.xml" : txtIncludeHandshake.Text;

                    ClientHttpRequisition postXml = new ClientHttpRequisition();
                    postXml.Url = System.Configuration.ConfigurationManager.AppSettings["urlHandshakeXML"];
                    postXml.Target = System.Configuration.ConfigurationManager.AppSettings["target"];
                    postXml.Parameters.Add("5DED746B8F924F2E", rblChave.Text == "0" ? txtChave.Text : rblChave.Text);
                    postXml.Parameters.Add("91D4C3128BF7DA7F", pedido);
                    postXml.Parameters.Add("IDIOMA", rblIdioma.Text);
                    postXml.Send();
                    break;
                case "HTML2":
                    ClientHttpRequisition postHtml = new ClientHttpRequisition();
                    postHtml.Url = System.Configuration.ConfigurationManager.AppSettings["urlHandshakeHTML"];
                    postHtml.Target = System.Configuration.ConfigurationManager.AppSettings["target"];
                    postHtml.Parameters.Add("5DED746B8F924F2E", rblChave.Text == "0" ? txtChave.Text : rblChave.Text);
                    postHtml.Parameters.Add("91D4C3128BF7DA7F", pedido);
                    postHtml.Parameters.Add("IDIOMA", rblIdioma.Text);
                    postHtml.Send();
                    break;
                case "HTML1":
                    Response.Redirect(String.Format("HandshakeHTML/{0}?5DED746B8F924F2E={1}&91D4C3128BF7DA7F={2}&IDIOMA={3}", (!String.IsNullOrEmpty(txtIncludeHandshake.Text) ? txtIncludeHandshake.Text : "default.aspx"), (rblChave.Text == "0" ? txtChave.Text : rblChave.Text), pedido, rblIdioma.Text));
                    break;

                case "WSREQ":
                    try
                    {
                        string xmlName = String.IsNullOrEmpty(txtIncludeHandshake.Text) ? "default.xml" : txtIncludeHandshake.Text;
                        
                        string xml;
                        try
                        {
                            XmlDocument doc = new XmlDocument();
                            doc.LoadXml(xmlName);
                            xml = doc.OuterXml;
                        }
                        catch
                        {
                            xml = File.ReadAllText(Server.MapPath("./WebService/" + xmlName));
                        }

                        string key = "2DF5C4349AB047EFFC769546AF2E316AB8DC8E5954";
                        string pass = "";

                        if (!String.IsNullOrEmpty(txtChave.Text))
                        {
                            string[] info = txtChave.Text.Split(";".ToCharArray());

                            if (info.Length > 0)
                                if (!String.IsNullOrEmpty(info[0]))
                                    key = info[0];

                            if (info.Length > 1)
                                if (!String.IsNullOrEmpty(info[1]))
                                    pass = info[1];
                        }                        
                        localhost.Payment payment = new localhost.Payment();
                        string response = payment.SendOrder(key, pass, xml);
                        
                        lblRetorno.Text = HttpUtility.HtmlEncode(response.Replace("><", "> <"));
                        lblRetorno.ForeColor = Color.Black;
                    }
                    catch (Exception ex)
                    {
                        lblRetorno.Text = ex.Message;
                        lblRetorno.ForeColor = Color.Red;
                    }
                    break;
                
                case "WSUP":
                    try
                    {
                        string xmlName = String.IsNullOrEmpty(txtIncludeHandshake.Text) ? "default.xml" : txtIncludeHandshake.Text;

                        string xml;
                        try
                        {
                            XmlDocument doc = new XmlDocument();
                            doc.LoadXml(xmlName);
                            xml = doc.OuterXml;
                        }
                        catch
                        {
                            xml = File.ReadAllText(Server.MapPath("./WebService/" + xmlName));
                        }

                        string key = "2DF5C4349AB047EFFC769546AF2E316AB8DC8E5954";
                        string pass = "";

                        if (!String.IsNullOrEmpty(txtChave.Text))
                        {
                            string[] info = txtChave.Text.Split(";".ToCharArray());

                            if (info.Length > 0)
                                if (!String.IsNullOrEmpty(info[0]))
                                    key = info[0];

                            if (info.Length > 1)
                                if (!String.IsNullOrEmpty(info[1]))
                                    pass = info[1];
                        }
                        
                        localhost.Payment payment = new localhost.Payment();
                        string response = payment.UpdateOrder(key, pass, xml);
                        
                        lblRetorno.Text = HttpUtility.HtmlEncode(response.Replace("><", "> <"));
                        lblRetorno.ForeColor = Color.Black;
                    }
                    catch (Exception ex)
                    {
                        lblRetorno.Text = ex.Message;
                        lblRetorno.ForeColor = Color.Red;
                    }
                    break;

                case "WSCHK":
                    try
                    {
                        string key = "2DF5C4349AB047EFFC769546AF2E316AB8DC8E5954";
                        string pass = "";
                        int orderid = 0;
                        DateTime dateFrom = DateTime.MinValue, dateTo = DateTime.MinValue;
                        bool withDate = false;

                        if (!String.IsNullOrEmpty(txtIncludeHandshake.Text))
                        {
                            string[] info = txtIncludeHandshake.Text.Split(";".ToCharArray());

                            if (info.Length > 2)
                            {
                                orderid = int.Parse(info[0]);
                                dateFrom = DateTime.ParseExact(info[1], "yyyy-MM-dd", new System.Globalization.DateTimeFormatInfo(), DateTimeStyles.None);
                                dateTo = DateTime.ParseExact(info[2], "yyyy-MM-dd", new System.Globalization.DateTimeFormatInfo(), DateTimeStyles.None);
                                withDate = true;
                            }
                            else
                            {
                                orderid = int.Parse(txtIncludeHandshake.Text);
                            }
                        }
                        
                        if (!String.IsNullOrEmpty(txtChave.Text))
                        {
                            string[] info = txtChave.Text.Split(";".ToCharArray());

                            if(info.Length > 0)
                                if (!String.IsNullOrEmpty(info[0]))
                                    key = info[0];

                            if (info.Length > 1)
                                if(!String.IsNullOrEmpty(info[1]))
                                    pass = info[1];
                        }

                        localhost.Payment payment = new localhost.Payment();
                        string response = "";
                        if(withDate)
                            response = payment.CheckRecurrence(key, pass, orderid, dateFrom, dateTo);
                        else
                            response = payment.CheckOrder(key, pass, orderid);

                        lblRetorno.Text = HttpUtility.HtmlEncode(response.Replace("><", "> <"));
                        lblRetorno.ForeColor = Color.Black;
                    }
                    catch (Exception ex)
                    {
                        lblRetorno.Text = ex.Message;
                        lblRetorno.ForeColor = Color.Red;
                    }
                    break;

                case "WSCNL":
                    try
                    {
                        localhost.Payment payment = new localhost.Payment();
                        string response = payment.CancelOrder("2DF5C4349AB047EFFC769546AF2E316AB8DC8E5954", "", int.Parse(txtIncludeHandshake.Text));

                        lblRetorno.Text = HttpUtility.HtmlEncode(response.Replace("><", "> <"));
                        lblRetorno.ForeColor = Color.Black;
                    }
                    catch (Exception ex)
                    {
                        lblRetorno.Text = ex.Message;
                        lblRetorno.ForeColor = Color.Red;
                    }
                    break;
            }
        }
    }
}
