using System;
using System.Collections;
using System.Text;
using System.Web;

namespace StoreTest
{
	/// <summary>
	/// Summary description for HtmlFormPost.
	/// </summary>
	public class HtmlFormPost
	{
		private StringBuilder response;

		public HtmlFormPost(string name, string action, HtmlFormPostField[] fields)
		{
			response = new StringBuilder();

			response.Append("<html><head></head><body>\n");

			response.Append("<form name=\"" + name + "\" id=\"" + name + "\" action=\"" + action + "\" method=\"post\">\n");
			for (int i = 0; i < fields.Length; i++)
			{
				response.Append("<input type=\"hidden\" name=\"" + fields[i].Name + "\" value=\"" + fields[i].Value + "\">\n");
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
			}
		}

	}

	public struct HtmlFormPostField
	{
		private string name;
		private string currentValue;

		public string Name
		{
			get { return name; }
			set { name = value; }
		}

		public string Value
		{
			get { return currentValue; }
			set { currentValue = value; }
		}
	}
}
