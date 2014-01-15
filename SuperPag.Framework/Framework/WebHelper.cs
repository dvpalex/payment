using System;
using System.Collections;
using System.Net;
using System.IO;

namespace SuperPag.Framework
{
	public class WebHelper
	{
		/// <summary>
		/// Executa o método GET em uma URL e devolve o resultado
		/// </summary>
		/// <param name="url">url</param>
		/// <param name="parameters">parametros separados por &</param>
		/// <returns></returns>
		public static string DoServerGet ( string url, string parameters )
		{
			if ( parameters[ 0 ] == '&' ) parameters.Replace( '&', '?' );
			if ( parameters[ 0 ] != '?' ) parameters.Insert ( 0, "?" );

			WebRequest request = WebRequest.Create(url);
		
			request.Method = "GET";
			
			//TODO: tratar erro
			WebResponse response = request.GetResponse();
			
			Stream stream = response.GetResponseStream();
			using ( stream )
			{
				string responseString = System.Text.Encoding.UTF8.GetString ( ReadStream ( stream ) );
				return responseString;
			}
		}

		private static byte[] ReadStream( Stream stream)
		{
			ArrayList temp = new ArrayList ( 100 );
			int byteRead = stream.ReadByte();
			while ( byteRead != -1 )
			{
				temp.Add ( (byte)byteRead );
				byteRead = stream.ReadByte();
			}
			return (byte[])temp.ToArray ( typeof ( byte ) );
		}
	}
}
