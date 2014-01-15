using System;
using System.Collections.Specialized;
using System.Text;
using System.Web;
using System.IO;

namespace SuperPag.Helper
{
    public class ClientHttpRequisition
    {
        private string _url;
        public string Url
        {
            get { return _url; }
            set { _url = value; }
        }
        private string _formName;
        public string FormName
        {
            get { return _formName; }
            set { _formName = value; }
        }
        private string _method;
        public string Method
        {
            get { return _method; }
            set { _method = value; }
        }
        private string _target;
        public string Target
        {
            get { return _target; }
            set { _target = value; }
        }
        private NameValueCollection _parameters;
        public NameValueCollection Parameters
        {
            get { return _parameters; }
            set { _parameters = value; }
        }
        private string _script;
        public string Script
        {
            get { return _script; }
            set { _script = value; }
        }
        private string _lastSentData;
        public string LastSentData
        {
            get { return _lastSentData; }
            set { _lastSentData = value; }
        }
        private bool _endResponse = true;
        public bool EndResponse
        {
            get { return _endResponse; }
            set { _endResponse = value; }
        }
        private string _errorMessage;
        public string ErrorMessage
        {
            get { return _errorMessage; }
            set { _errorMessage = value; }
        }
        private string _enconding = null;
        public string Encoding
        {
            get { return _enconding; }
            set { _enconding = value; }
        }

        public ClientHttpRequisition(string url, NameValueCollection parameters, string method, string target, string formName)
        {
            _url = url;
            _parameters = parameters;
            _formName = formName;
            _method = method;
            _target = target;
        }
        public ClientHttpRequisition(string url, NameValueCollection parameters, string method) : this(url, parameters, method, null, null) { }
        public ClientHttpRequisition(string url) : this(url, new NameValueCollection(), null) { }
        public ClientHttpRequisition()
        {
            _parameters = new NameValueCollection();
        }

        public string GetFormString()
        {
            if (HttpContext.Current == null)
                return "";
            if (String.IsNullOrEmpty(Url))
                return "";
            if (String.IsNullOrEmpty(FormName))
                FormName = "ClientHttpRequisitionForm1";

            StringBuilder response = new StringBuilder();
            response.Append("<form");
            response.Append(" name=\"" + FormName + "\" id=\"" + FormName + "\"");
            response.Append(" action=\"" + Url + "\"");
            if (!String.IsNullOrEmpty(Method))
                response.Append(" method=\"" + Method + "\"");
            if (!String.IsNullOrEmpty(Target))
                response.Append(" target=\"" + Target + "\"");
            response.Append(">\n");

            if (Parameters != null)
            {
                foreach (string key in Parameters.Keys)
                {
                    if (Parameters[key] != null)
                        response.Append("<input type=\"hidden\" name=\"" + key + "\" value=\"" + Parameters[key] + "\">\n");
                }
            }
            response.Append("</form>");
            if (!String.IsNullOrEmpty(Script))
                response.Append("<script language=\"JavaScript\">" + Script + "</script>\n");

            return response.ToString(); ;
        }

        public bool Send()
        {
            if (HttpContext.Current == null)
            {
                ErrorMessage = "O contexto http está inválido";
                return false;
            }

            if (String.IsNullOrEmpty(Url))
            {
                ErrorMessage = "A url de envio está nula ou vazia";
                return false;
            }

            if (String.IsNullOrEmpty(FormName))
                FormName = "ClientHttpRequisitionForm1";

            StringBuilder response = new StringBuilder();
            response.Append("<html><head></head><body>\n");
            response.Append("<form");
            response.Append(" name=\"" + FormName + "\" id=\"" + FormName + "\"");
            response.Append(" action=\"" + Url + "\"");
            if (!String.IsNullOrEmpty(Method))
                response.Append(" method=\"" + Method + "\"");
            if (!String.IsNullOrEmpty(Target))
                response.Append(" target=\"" + Target + "\"");
            response.Append(">\n");

            if (Parameters != null)
            {
                foreach (string key in Parameters.Keys)
                {
                    if (Parameters[key] != null)
                        response.Append("<input type=\"hidden\" name=\"" + key + "\" value=\"" + Parameters[key] + "\">\n");
                }
            }

            response.Append("</form>\n");
            response.Append("<script language=\"JavaScript\">document." + FormName + ".submit();</script>\n");
            if (!String.IsNullOrEmpty(Script))
                response.Append("<script language=\"JavaScript\">" + Script + "</script>\n");
            response.Append("</body></html>\n");

            _lastSentData = response.ToString();

            HttpContext.Current.Response.Clear();
            if (!String.IsNullOrEmpty(_enconding))
                HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding(_enconding);
            HttpContext.Current.Response.Write(response.ToString());
            if(_endResponse)
                HttpContext.Current.Response.End();

            return true;
        }
    }
}
