using System;
using System.Text;
using System.Collections;
using System.Web;


namespace StoreTest
{
	public class HtmlFormPostClient
	{
		private StringBuilder response;

		public HtmlFormPostClient(string name, string action, Hashtable fields)
		{
			response = new StringBuilder();

			response.Append("<html><head></head><body>\n");

			response.Append("<form name=\"" + name + "\" id=\"" + name + "\" action=\"" + action + "\" method=\"post\">\n");
			
			foreach(string key in fields.Keys)
			{
				if (fields[key] != null)
				{
					response.Append("<input type=\"hidden\" name=\"" + key + "\" value=\"" + fields[key].ToString() + "\">\n");
				}
			}
			
			response.Append("</form>");
			response.Append("<script language=\"JavaScript\">document." + name + ".submit();</script>\n");
			response.Append("</body></html>\n");
		}

		public override string ToString()
		{
			return response.ToString();
		}

		public void Submit()
		{
			if (HttpContext.Current != null)
			{
				HttpContext.Current.Response.Clear();
				HttpContext.Current.Response.Write(response.ToString());
				HttpContext.Current.Response.End();
			}
		}

	}
}
