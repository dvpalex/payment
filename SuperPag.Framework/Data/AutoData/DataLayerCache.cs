using System;
using System.IO;
using System.Reflection;
using System.Collections;


namespace SuperPag.Framework.Data.Components.AutoDataLayer {
	public class DataLayerCache {
		
		private static Hashtable hsLock = new Hashtable();

		private static Hashtable cachedDataLayers = null;
		internal static string dataLayerCachePath = null;

		static DataLayerCache() {
            dataLayerCachePath = System.Configuration.ConfigurationSettings.AppSettings["dlCache"];
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

		static object LoadAssembly(string assemblyPath, Type interfaceType, string className, string connectionString) {
			//Carrega o assembly e inst�ncia o objeto
			Assembly dlAssembly = Assembly.LoadFrom(assemblyPath);
			return dlAssembly.CreateInstance(interfaceType.Namespace + "." +  className, true, BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Instance, null, new object[] {connectionString}, null, null );
		}

		public static object GetCachedDataLayer(Type dataInterfaceType, string connectionString) {

			//Lock para programas multi-threading
			lock( hsLock.SyncRoot ) 
			{
				try 
				{
					//Verifica se o objeto est� no cache de mem�ria
					if (cachedDataLayers != null && cachedDataLayers.ContainsKey(dataInterfaceType.FullName)) 
					{
						//Retorna a inst�ncia do dataLayer
						return cachedDataLayers[dataInterfaceType.FullName];
					} 
					else 
					{
				
						//Formata o nome da classe
#if SQL
                        string className = dataInterfaceType.Name.Substring(1) + "SqlServer";
#elif ORACLE
                        string className = dataInterfaceType.Name.Substring(1) + "Oracle";
#else
                        string className = dataInterfaceType.Name.Substring(1) + "SqlServer";
#endif
                        //Formata o nome do assembly
						string dllAssemblyPath = dataLayerCachePath + className + ".dll";

						//Verifica se o objeto est� no cache de disco
				
						if (!File.Exists(dllAssemblyPath)) 
						{

							//Cria a classe e tamb�m grava no cache de disco
							DataLayerBuilder dlBuilder = new DataLayerBuilder(dllAssemblyPath, dataInterfaceType, className, dataLayerCachePath);
							dlBuilder.Build();
						}

						object dataLayerInstance = LoadAssembly(dllAssemblyPath, dataInterfaceType, className, connectionString);

						//Verifica se deve armazenar a inst�ncia na mem�ria
						if (Attribute.IsDefined(dataInterfaceType, typeof(DataInterfaceAttributes.PersistInMemoryAttribute), true)) 
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
				catch (AutoDataLayerBuildException ex) 
				{
					throw ex;
				} 
				catch (Exception ex) 
				{
					throw new AutoDataLayerBuildException("Erro inexperado: " + ex.Message, ex);
				}
			}
		}
	}
}
