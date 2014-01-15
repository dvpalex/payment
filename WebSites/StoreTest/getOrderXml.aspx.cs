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
using System.Xml;
using System.IO;
using System.Xml.Serialization;

namespace StoreTest
{
	public partial class getOrderXml : System.Web.UI.Page
	{
		private string _result;

		protected void Page_Load(object sender, System.EventArgs e)
		{
			FileStream file = null;
            string xmlDoc = "";

			try
			{
				string authId = Request.QueryString["36948FFEF212F5E4"];
				string orderId = Request.QueryString["91D4C3128BF7DA7F"];
                string xmlName = String.IsNullOrEmpty(Application["xmlName"].ToString()) ? "handshake.xml" : Application["xmlName"].ToString();

                if (Application["xmlDoc"] != null)
                {
                    xmlDoc = Application["xmlDoc"].ToString();
                }
                else
                {
                    file = new FileStream(Server.MapPath("./HandshakeXML/" + xmlName), FileMode.Open);
                    XmlSerializer deserializer = new XmlSerializer(typeof(MPaymentRequest));
                    MPaymentRequest m = (MPaymentRequest)deserializer.Deserialize(file);
                    m.NumeroPedido = orderId;
                    m.SetupLoja.Urls.PostFinal = String.Format("{0}://{1}{2}orderConfirmXml.aspx", (Request.ServerVariables["HTTPS"] == "off" ? "http" : "https"), Request.ServerVariables["SERVER_NAME"], Request.ServerVariables["PATH_INFO"].Replace("getOrderXml.aspx", ""));

                    XmlSerializerNamespaces nmspc = new XmlSerializerNamespaces();
                    nmspc.Add(String.Empty, String.Empty);
                    StringWriter swriter = new StringWriter();
                    XmlTextWriter txtwriter = new XmlTextWriter(swriter);
                    txtwriter.WriteRaw(String.Empty);
                    txtwriter.Formatting = Formatting.Indented;
                    XmlSerializer serializer = new XmlSerializer(typeof(MPaymentRequest));
                    serializer.Serialize(txtwriter, m, nmspc);
                    xmlDoc = swriter.ToString();
                }

				Response.Clear();
				Response.ContentType = "text/xml";
				this._result = xmlDoc;
			}
			catch( Exception ex )
			{
				Response.Write(ex.Message);
				Response.End();
			}
			finally
			{
				if( file != null )
					file.Close();
			}
		}

		protected override void Render(HtmlTextWriter writer)
		{
			if( _result != null )
				writer.Write( _result );
		}

	}
}
