using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace StoreTest
{
	public partial class getXmlError : System.Web.UI.Page
	{
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			string msg = Request.QueryString["sp_message"];

			if( msg != null )
			{
				string [] msgs = msg.Split( '|' );
				if( msgs != null && msgs.Length > 0 )
				{
					lblMensagem.Text = msgs[0] + "<br><br>";
					for( int i=1; i<msgs.Length; i++ )
						lblMensagem.Text += ( msgs[i] != "" ? "<li>" + msgs[i] + "<br>" : "" );
				}
			}
		}

	}

}
