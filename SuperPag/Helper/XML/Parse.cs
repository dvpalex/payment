using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Schema;
using System.Xml;
using System.IO;
using System.Xml.Serialization;
using System.Reflection;
using SuperPag.Helper.Xml.Request;
using SuperPag.Helper.Xml.Response;

namespace SuperPag.Helper.Xml
{
    public class Parse
    {
        private string _msgerror = "";

        public string Error
        {
            get
            {
                return _msgerror;
            }
        }
        
        public bool Xml(string xml, string xsd, string nspace)
        {
            XmlReader reader = null;
            XmlTextReader treader = null;

            try
            {
                try
                {
                    _msgerror = "";

                    XmlSchemaSet sc = new XmlSchemaSet();
                    sc.Add(nspace, xsd);

                    XmlReaderSettings settings = new XmlReaderSettings();
                    settings.Schemas = sc;
                    settings.ValidationType = ValidationType.Schema;
                    settings.ValidationFlags |= XmlSchemaValidationFlags.ReportValidationWarnings;
                    settings.ValidationEventHandler += new ValidationEventHandler(ValidationCallBack);

                    treader = new XmlTextReader(xml, XmlNodeType.Document, null);
                    reader = XmlReader.Create(treader, settings);
                }
                catch (Exception e)
                {
                    _msgerror = e.Message;
                    return false;
                }

                try
                {
                    while (reader.Read()) { }

                    if (_msgerror != "")
                        return false;

                    return true;
                }
                catch (Exception e)
                {
                    _msgerror = e.Message;
                    return false;
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                if (treader != null)
                    treader.Close();
            }
        }

        public void ValidationCallBack(object sender, ValidationEventArgs args)
        {
            _msgerror += args.Message + "\n";
        }
    }

    public class XmlHelper
    {
        public static request GetRequestClass(string xml, out string msgerror)
        {
            try
            {
                msgerror = "";

                StringReader reader = new StringReader(xml);
                XmlSerializer serializer = new XmlSerializer(typeof(request));
                request req = (request)serializer.Deserialize(reader);

                return req;
            }
            catch (Exception ex)
            {
                msgerror = ex.Message + (ex.InnerException == null ? "" : ex.InnerException.Message);
                return null;
            }
        }

        public static string GetResponseXml(response resp)
        {
            StringWriter writer = new StringWriter();
            XmlSerializerNamespaces nmspc = new XmlSerializerNamespaces();
            nmspc.Add(String.Empty, String.Empty);
            
            MyXmlTextWriter txtwriter = new MyXmlTextWriter(ref writer);
            txtwriter.WriteRaw(String.Empty);
            txtwriter.Formatting = Formatting.None;
            XmlSerializer serializer = new XmlSerializer(typeof(response));
            serializer.Serialize(txtwriter, resp, nmspc);

            return txtwriter.GetXML();
        }

        public static object GetClass(string xml, Type t, out string msgerror)
        {
            try
            {
                msgerror = "";

                StringReader reader = new StringReader(xml);
                XmlSerializer serializer = new XmlSerializer(t);
                object o = serializer.Deserialize(reader);

                return o;
            }
            catch (Exception ex)
            {
                msgerror = ex.Message + (ex.InnerException == null ? "" : ex.InnerException.Message);
                return null;
            }
        }

        public static string GetXml(Type t, object o)
        {
            StringWriter writer = new StringWriter();
            //try
            //{
                XmlSerializerNamespaces nmspc = new XmlSerializerNamespaces();
                nmspc.Add(String.Empty, String.Empty);                
                MyXmlTextWriter txtwriter = new MyXmlTextWriter(ref writer);
                txtwriter.WriteRaw(String.Empty);
                txtwriter.Formatting = Formatting.None;
                XmlSerializer serializer = new XmlSerializer(t);
                serializer.Serialize(txtwriter, o, nmspc);
                
                return txtwriter.GetXML();
            //}
            //catch (Exception ex)
            //{ 
            //    return writer.ToString(); 
            //}            
            
        }
    }
    
    public class MyXmlTextWriter : XmlTextWriter
    {
        private StringWriter _swriter = null;

        public MyXmlTextWriter(ref StringWriter w) : base(w) 
        { 
            _swriter = w;
        }

        public override void WriteEndElement()
        {
            base.WriteFullEndElement();
        }
        public override void WriteStartAttribute(string prefix, string localName, string ns)
        {
            base.WriteStartAttribute(prefix, localName, ns);
        }
        public override void WriteEndAttribute()
        {
            base.WriteEndAttribute();
        }
        public override void WriteString(string text)
        {
            base.WriteString(text);           
        }
        public string GetXML()
        {
            if (_swriter != null)
            {               
                return _swriter.ToString();
            }
            else
                return null;
        }
    }
}
