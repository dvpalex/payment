using System;
using System.IO;
using System.Net;
using System.Collections.Specialized;
using System.Text;
using System.Web;

namespace SuperPag.Helper
{
    public class ServerHttpRequisition
    {
        private string _url;
        public string Url
        {
            get { return _url; }
            set { _url = value; }
        }
        private string _data;
        public string Data
        {
            get { return _data; }
            set { _data = value; }
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
        private string _content;
        public string Content
        {
            get { return _content; }
            set { _content = value; }
        }

        public ServerHttpRequisition(string url, string data, string method)
        {
            _url = url;
            _data = data;
            _method = method;
        }
        public ServerHttpRequisition(string url, string data) : this(url, data, null) { }
        public ServerHttpRequisition(string url) : this(url, null) { }
        public ServerHttpRequisition()
        {
        }

        public bool Send()
        {
            if (String.IsNullOrEmpty(Url))
            {
                Response = "A url de envio está nula ou vazia";
                return false;
            }

            if (String.IsNullOrEmpty(Method))
                Method = "POST";
            if (String.IsNullOrEmpty(Data))
                Method = "GET";

            System.Net.WebClient webClient = new System.Net.WebClient();
            webClient.Headers.Add("Content-Type", (String.IsNullOrEmpty(_content) ? "application/x-www-form-urlencoded" : _content));
            try
            {
                byte[] responseHTTP = null;

                if (Method == "GET")
                {
                    responseHTTP = webClient.DownloadData(Url);
                }
                else
                {
                    responseHTTP = webClient.UploadData(Url, Method, Encoding.ASCII.GetBytes(Data));
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
