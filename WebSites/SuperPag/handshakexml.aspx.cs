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
using SuperPag.Helper;

public partial class handshakexml : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.AddHeader("p3p", "CP=\"IDC DSP COR ADM DEVi TAIi PSA PSD IVAi IVDi CONi HIS OUR IND CNT\"");

        string sessionCode = Request["36948FFEF212F5E4"];
        string storeKey = Request["5DED746B8F924F2E"];
        string storeReferenceOrder = Request["91D4C3128BF7DA7F"];
        string receiveTransResponse = Request["STS_RECEIVE_TRANS"];
        string keyValidation = Request["VALIDA_KEY"];
        string idioma = Request["IDIOMA"];

        Session.Clear();

        if (!String.IsNullOrEmpty(idioma))
            Session["Language"] = idioma;

        SuperPag.Handshake.Xml.Handshake xmlHandshakeHelper = new SuperPag.Handshake.Xml.Handshake();

        xmlHandshakeHelper.Step1(storeKey, storeReferenceOrder);
    }
}
