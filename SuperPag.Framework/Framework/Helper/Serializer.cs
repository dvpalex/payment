using System;
using System.Xml;
using System.Xml.Serialization;

namespace SuperPag.Framework.Helper
{
	
	public class Serializer
	{
		const string __namespace = null; //"http://e-financial/schemas";

		public static object ToObject( Type instanceType, XmlNode xmlinstance )
		{
			XmlSerializer serializer = new XmlSerializer( instanceType, __namespace );
			
			return serializer.Deserialize( new XmlNodeReader( xmlinstance ) );
		}

		public static XmlDocument ToDocument( object instance )
		{
			XmlSerializer serializer = new XmlSerializer( instance.GetType() );
			System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();
			
			XmlSerializerNamespaces nmspc = new XmlSerializerNamespaces();
			nmspc.Add( "", __namespace );
			
			serializer.Serialize( memoryStream, instance, nmspc );
			
			System.IO.StreamReader reader = new System.IO.StreamReader( memoryStream );
			
			memoryStream.Position = 0;
			
			string msg = reader.ReadToEnd();
			
			reader.Close();
			memoryStream.Close();
			
			XmlDocument xmldoc = new XmlDocument();
			xmldoc.LoadXml( msg );
			
			return xmldoc;
		}
	}     
}