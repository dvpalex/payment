using System;
using System.IO;
using System.Reflection;
using System.Collections;

namespace SuperPag.Framework.Data.Builder
{
	
	public class Cache
	{
		private static Hashtable hsLock = new Hashtable();

		private static Hashtable cachedDataLayers = null;
		internal static string dataLayerCachePath = null;

		static Cache() 
		{
			dataLayerCachePath = AppDomain.CurrentDomain.BaseDirectory + "\\dlCache\\";
			dataLayerCachePath = dataLayerCachePath.Replace("\\\\", "\\");
		
			if(Directory.Exists(dataLayerCachePath)) 
			{
				if ( System.Configuration.ConfigurationSettings.AppSettings [ "ProductionServer" ] != "1" )
				{
					//limpo os assemblies do cache 
					string[] files = System.IO.Directory.GetFiles(dataLayerCachePath);
					foreach(string f in files) 
					{
						System.IO.File.Delete(f);		
					}
				}
			}
		
		}

		static object LoadAssembly(string assemblyPath, Type interfaceType, string className, string connectionString) 
		{
			//Carrega o assembly e inst�ncia o objeto
			Assembly dlAssembly = Assembly.LoadFrom(assemblyPath);
			return dlAssembly.CreateInstance(interfaceType.Namespace + "." +  className, true, BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Instance, null, new object[] {connectionString}, null, null );
		}

		//TODO: voltar a classe base
		public static object Get( Type dataInterfaceType , string connectionString )
		{
			//Lock para programas multi-threading
			lock( hsLock.SyncRoot ) 
			{
				//Verifica se o objeto est� no cache de mem�ria
				if (cachedDataLayers != null && cachedDataLayers.ContainsKey(dataInterfaceType.FullName)) 
				{
					//Retorna a inst�ncia do dataLayer
					return cachedDataLayers[ dataInterfaceType.FullName ];
				} 
				else 
				{
					//Formata o nome da classe
					string className = "AutoData_" + dataInterfaceType.Name;

					//Formata o nome do assembly
					string dllAssemblyPath = dataLayerCachePath + className + ".dll";
		
					//Verifica se o objeto est� no cache de disco						
					if (!File.Exists(dllAssemblyPath)) 
					{		
						//Cria a classe e tamb�m grava no cache de disco
						Builder dlBuilder = new Builder(dllAssemblyPath, dataInterfaceType, className, dataLayerCachePath);
						dlBuilder.Build();
					}
		
					object dataLayerInstance = LoadAssembly(dllAssemblyPath, dataInterfaceType, className, connectionString);
		
					//Verifica se deve armazenar a inst�ncia na mem�ria
					if (Attribute.IsDefined(dataInterfaceType, typeof( CacheInMemory ), true)) 
					{							
						//Verifica se deve inicializar o hash
						if (cachedDataLayers == null) cachedDataLayers = new Hashtable();
							
						//Armaze a inst�ncia do DataLayer na mem�ria
						cachedDataLayers.Add(dataInterfaceType.FullName, dataLayerInstance);
					}							
		
					//Retorna a inst�ncia do dataLayer
					return dataLayerInstance;
				}
				
			}
		}
	}
}
