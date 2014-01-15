using System;
using System.Reflection;

namespace SuperPag.Framework.Helper
{
	public class ResourcesHelper 
	{
		public static string FindResourcePath(Assembly assembly, string searchPattern) 
		{
			string[] arrayOfResources = assembly.GetManifestResourceNames();

			foreach (string resource in arrayOfResources) 
			{
				if (resource.ToUpper().IndexOf("." + searchPattern.ToUpper()) != -1) 
				{
					return resource;
				}
			}

			return null;
		}

		public static string ResolvePrivateAssembly(string assemblyFile) 
		{
			string assemblyPath = assemblyFile.ToLower();
			if (!assemblyPath.EndsWith(".dll"))
			{
				assemblyPath += ".dll";
			}

			if (!System.IO.Path.IsPathRooted(assemblyPath)) 
			{
				if (AppDomain.CurrentDomain.RelativeSearchPath != null) assemblyPath = System.IO.Path.Combine(AppDomain.CurrentDomain.RelativeSearchPath, assemblyPath);
				assemblyPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, assemblyPath);
			}

			return assemblyPath;
		}
	}
}
