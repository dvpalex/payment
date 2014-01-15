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
using System.Xml;
using System.IO;
using System.Text;
using SuperPag.Data.Messages;
using SuperPag.Data;
using SuperPag;
using SuperPag.Helper;
using SuperPag.Agents.VBV;

public partial class Agents_VBV_processampg : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // Devemos utilizar o campo TID (que também é único) ao invés do FREE para evitar fraudes
        // e funcionar tanto no IE como no Firefox (em que o tid é enviado pela querystring)
        Ensure.IsNotNullPage(Request["TID"], "Post inválido tentando recuperar o ID de uma transação VISA");
        VBV vbv = new VBV(Request["TID"]);
        vbv.Step2();
    }
}
