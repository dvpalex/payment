using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Specialized;
using System.Web;


namespace SuperPag.Business
{
    public class ServerHttpHtmlRequisition
    {
        private string _url;
        public string Url
        {
            get { return _url; }
            set { _url = value; }
        }
        private NameValueCollection _parameters;
        public NameValueCollection Parameters
        {
            get { return _parameters; }
            set { _parameters = value; }
        }
        private string _method;
        public string Method
        {
            get { return _method; }
            set { _method = value; }
        }
        private string _response;
        public string Response
        {
            get { return _response; }
            set { _response = value; }
        }
        private string _lastSentData;
        public string LastSentData
        {
            get { return _lastSentData; }
        }
        private bool _upperKeys;
        public bool UpperKeys
        {
            get { return _upperKeys; }
            set { _upperKeys = value; }
        }

        public ServerHttpHtmlRequisition(string url, NameValueCollection parameters, string method)
        {
            _url = url;
            _parameters = parameters;
            _method = method;
            _upperKeys = true;
        }
        public ServerHttpHtmlRequisition(string url, NameValueCollection parameters) : this(url, parameters, null) { }
        public ServerHttpHtmlRequisition(string url) : this(url, new NameValueCollection()) { }
        public ServerHttpHtmlRequisition()
        {
            _parameters = new NameValueCollection();
        }

        public bool Send()
        {
            try
            {
                if (String.IsNullOrEmpty(Url))
                {
                    Response = "A url de envio está nula ou vazia";
                    return false;
                }

                if (String.IsNullOrEmpty(Method))
                    Method = "POST";

                StringBuilder param = new StringBuilder();
                if (UpperKeys)
                {
                    foreach (string key in _parameters.AllKeys)
                        param.AppendFormat("{0}={1}&", HttpUtility.UrlEncode(key.ToUpper(), Encoding.GetEncoding("iso-8859-1")), HttpUtility.UrlEncode(_parameters[key], Encoding.GetEncoding("iso-8859-1")));
                }
                else
                {
                    foreach (string key in _parameters.AllKeys)
                        param.AppendFormat("{0}={1}&", HttpUtility.UrlEncode(key), HttpUtility.UrlEncode(_parameters[key]));
                }
                if (param.Length != 0)
                    param.Remove(param.Length - 1, 1);

                _lastSentData = param.ToString();

                System.Net.WebClient webClient = new System.Net.WebClient();
                webClient.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                
                byte[] responseHTTP = null;

                if (Method == "GET")
                {
                    webClient.QueryString = Parameters;
                    responseHTTP = webClient.UploadData(Url, new byte[] { });
                }
                else
                {
                    responseHTTP = webClient.UploadData(Url, Method, Encoding.ASCII.GetBytes(param.ToString()));
                }
                
                Response = Encoding.ASCII.GetString(responseHTTP, 0, responseHTTP.Length);
            }
            catch (Exception ex)
            {
                Response = ex.Message;
                return false;
            }
            return true;
        }
    }
}
