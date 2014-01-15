using System;
using System.Reflection;
using System.Reflection.Emit;

namespace SuperPag.Framework.Business
{
	internal class BusinessObjectFactory
	{
		private static MethodInfo BUSINESS_OBJECT_PREPROCESSING =
			typeof(BusinessObject).GetMethod("PreProcessing", BindingFlags.Instance | BindingFlags.Public);

		private static MethodInfo BUSINESS_OBJECT_POSPROCESSING =
			typeof(BusinessObject).GetMethod("PosProcessing", BindingFlags.Instance | BindingFlags.Public);

		private static MethodInfo BUSINESS_OBJECT_PROCESSINGERROR =
			typeof(BusinessObject).GetMethod("ProcessingError", BindingFlags.Instance | BindingFlags.Public);

		protected static AppDomain _appDomain;
		protected static ModuleBuilder _modBuilder;
		static AssemblyBuilder _asmBuilder;

		static BusinessObjectFactory()
		{
			_appDomain = System.Threading.Thread.GetDomain();

			AssemblyName asm = new AssemblyName();
			asm.Name = "BusinessObjectAssembly";

			_asmBuilder = 
				_appDomain.DefineDynamicAssembly( asm, AssemblyBuilderAccess.Run );

			_modBuilder = _asmBuilder.DefineDynamicModule( "BusinessObjectClass" );
		}

		//Constroi um novo tipo, injetando codigo que garante contexto transcional
		//e efetua o pré e pos processamento em cada método
		public static object Build( Type classType )
		{
			string typeName = String.Format("{0}.{1}_RuntimeProxyObject", classType.Namespace, classType.Name);

			Type serviceType = _modBuilder.GetType(typeName);

			//Se o tipo já existe, crio uma nova instância
			if(serviceType != null) 
			{
				return _asmBuilder.CreateInstance(serviceType.FullName, false, BindingFlags.CreateInstance, null, null, null, new object[] {});
			} 
			else 
			{
				//Garante que o tipo não é criado duas vezes no mesmo app Domain
				lock( _appDomain ) 
				{
					//refaço a primeira condição devido ao lock
					if(_modBuilder.GetType(typeName) != null) 
					{
						return _asmBuilder.CreateInstance( serviceType.FullName, false, BindingFlags.CreateInstance,  null, null, null, new object[] {});
					}

					//Defino o tipo
					TypeBuilder tBuilder;
					tBuilder = _modBuilder.DefineType(
						typeName,
						TypeAttributes.Public,
						classType);
					
					//Defino os construtores
					DefineConstructor(classType, tBuilder);

					//Defino os metodos
					DefineMethods(classType, tBuilder);
					
					//Crio o Tipo
					Type newType = tBuilder.CreateType();

					//Crio a instancia
					return _asmBuilder.CreateInstance(newType.FullName, false, BindingFlags.CreateInstance, null, null, null, new object[] {});
				}
			}				
		}

		//Define o construtor para a classe, passando o Contexto
		private static void DefineConstructor(Type classType, TypeBuilder tBuilder) 
		{
			ConstructorBuilder ctor = tBuilder.DefineConstructor(
				MethodAttributes.Public,
				CallingConventions.Standard,
				null);

			ConstructorInfo ctorClassType = typeof(BusinessObject).GetConstructor( BindingFlags.NonPublic | BindingFlags.Instance , null, Type.EmptyTypes, null );

			ILGenerator ilCtor = ctor.GetILGenerator();
			ilCtor.Emit(OpCodes.Ldarg_0);
			ilCtor.Emit(OpCodes.Call, ctorClassType);
			ilCtor.Emit(OpCodes.Ret);
		}

		//Injeta o codigo em todos os métodos do tipo
		private static void DefineMethods(Type classType, TypeBuilder tBuilder) 
		{
			//Obtenho os metodos
			MethodInfo[] methods = 
				classType.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);

			//Para cada metodo
			foreach(MethodInfo method in methods) 
			{
				if(method.IsVirtual) 
				{
					string methodName = method.Name;

					//Obtenho os parametros do metodo
					Type[] paramsType = GetMethodParameters(method);
						
					//Defino um novo metodo body
					MethodBuilder mBuilder = tBuilder.DefineMethod(methodName + "_Impl", 
						MethodAttributes.Virtual | MethodAttributes.HideBySig ,
						method.ReturnType, paramsType);

					//Obtenho o IL Generator
					ILGenerator il = mBuilder.GetILGenerator();

					//Declaro variaveis locais
					EmitLocalDeclarationVariables(method, il);

					//Label para o fim do método
					Label endMethod = il.DefineLabel();

					// try
					il.BeginExceptionBlock();

					//Emito o código que faz a chamada ao pre-processamento
					EmitPreProcessingCall(method, il, paramsType, endMethod );

					//Faço as chamadas no metodo body para o correspondente na classe base
					EmitCallToOverridableMethod(method, il, paramsType);

					//Label para o fim do método
					il.MarkLabel(endMethod);

					//Catch
					il.BeginCatchBlock(typeof(Exception));
					il.Emit(OpCodes.Stloc_1);
					il.Emit(OpCodes.Ldarg_0);
					il.Emit(OpCodes.Ldloc_1);
					il.Emit(OpCodes.Call, BUSINESS_OBJECT_PROCESSINGERROR);
					//TODO: if
					il.Emit(OpCodes.Ldloc_1);
					il.Emit(OpCodes.Throw);

					//Finally
					il.BeginFinallyBlock();

					//Emito o código que faz a chamada ao pos-processamento
					EmitPosProcessingCall(method, il, paramsType);
			
					//End Try
					il.Emit(OpCodes.Endfinally);
					il.EndExceptionBlock();

					EmitMethodReturn(method, il);

					//Faço o override
					tBuilder.DefineMethodOverride(mBuilder, method);
				} 
				else 
				{	
					if ( ! Attribute.IsDefined ( method, typeof ( NoPreProcessingAttribute ) ) )
					{
						throw new Exception(string.Format("Os métodos públicos do BusinessObject não marcados com o atributo NoPreProcessing precisam ser virtuais. Nome do método: [{0}]", method.Name));
					}
				}
			}
		}

		//Obtem o array de tipos de parametros do método
		private static Type[] GetMethodParameters(MethodInfo method) 
		{
			ParameterInfo[] paramsInfo = method.GetParameters();

			Type[] paramsType = new Type[paramsInfo.Length];
			for(int i = 0, len = paramsInfo.Length; i < len; i++) 
			{
				paramsType[i] = paramsInfo[i].ParameterType;
			}
			return paramsType;
		}

		//Emite o codigo IL que gera a chamada para o método que está sendo feito o override
		private static void EmitCallToOverridableMethod(MethodInfo method, ILGenerator ilGen, Type[] paramsType) 
		{
			ilGen.Emit(OpCodes.Nop);
			ilGen.Emit(OpCodes.Ldarg_0);

			for(int i = 0, len = paramsType.Length; i < len; i++) 
			{
				ilGen.Emit(OpCodes.Ldarg_S, (byte)(i + 1));
			}
																		
			ilGen.Emit(OpCodes.Call, method);
			if(method.ReturnType != typeof(void)) 
			{
				ilGen.Emit(OpCodes.Stloc_0);
			}			
		}

		//Emite as declarações para as variaveis locais
		private static void EmitLocalDeclarationVariables(MethodInfo method, ILGenerator ilGen) 
		{			
			//variável que guarda o retorno do método
			if(method.ReturnType == typeof(void)) 
			{
				ilGen.DeclareLocal(typeof(object)); } 
			else 
			{
				ilGen.DeclareLocal(method.ReturnType); }					
			
			//variável que guarda a exception usada no throw
			ilGen.DeclareLocal(typeof(Exception));
			
			//variável que guarda o array de parametros para o pre e pos processamento
			ilGen.DeclareLocal(typeof(object[]));

			//variável que guarda o array de tipos para GetMethod do reflection
			ilGen.DeclareLocal(typeof(Type[]));

			//variavel que guarda o MethodInfo do método que será chamado na base
			ilGen.DeclareLocal(typeof(MethodInfo));
		}

		//Emite o codigo que retorna o valor resultante da chamada do metodo base
		private static void EmitMethodReturn(MethodInfo method, ILGenerator ilGen) 
		{
			ilGen.Emit(OpCodes.Ldloc_0);
			if(method.ReturnType == typeof(void)) 
			{ 
				ilGen.Emit(OpCodes.Pop);
			} 
			ilGen.Emit(OpCodes.Ret);
		}

		//Emite a chamada para o metodo que faz o pre-processamento, na base service
		private static void EmitPreProcessingCall(MethodInfo method, ILGenerator ilGen, Type[] paramsType, Label endMethodLabel ) 
		{
			ilGen.Emit(OpCodes.Ldarg_0);
			ilGen.Emit(OpCodes.Ldarg_0);

			//cria um array para passar para o method "GetMethod" do reflection e salva
			//na variavel "3"
			ilGen.Emit(OpCodes.Ldc_I4, paramsType.Length );
			ilGen.Emit(OpCodes.Newarr, typeof(Type));
			ilGen.Emit(OpCodes.Stloc_3);
			
			//para cada parametro adiciona o type no array
			ilGen.Emit(OpCodes.Ldloc_3);
			if(paramsType.Length > 0) 
			{
				for(int i = 0; i < paramsType.Length; i++) 
				{
					ilGen.Emit(OpCodes.Ldc_I4, i);
					ilGen.Emit(OpCodes.Ldtoken, paramsType [ i ] );								        
					MethodInfo m = typeof ( Type ).GetMethod ( "GetTypeFromHandle" );
					ilGen.Emit(OpCodes.Call, m );
					ilGen.Emit(OpCodes.Stelem_Ref);
					ilGen.Emit(OpCodes.Ldloc_3);
				}
			}
			
			//Chama o método "GetMethod", passando o nome e um array de tipos 
			ilGen.Emit(OpCodes.Ldarg_0);
			ilGen.Emit(OpCodes.Ldtoken, method.DeclaringType );
			ilGen.Emit(OpCodes.Call, typeof ( Type ).GetMethod ( "GetTypeFromHandle" ) );
			ilGen.Emit(OpCodes.Ldstr, method.Name );
			ilGen.Emit(OpCodes.Ldloc_3);
			ilGen.Emit(OpCodes.Call, typeof( Type ).GetMethod("GetMethod", new Type[] { typeof ( string ), typeof ( System.Type[] ) } ) );

			//Guarda o resultado do "GetMethod" na variavel "4"
			ilGen.Emit(OpCodes.Stloc_S, (byte)4 ) ;

			//Chama o método de preprocessamento, passando: 
			//1. MethodInfo para o method base
			//2. Array de parametros
			ilGen.Emit(OpCodes.Ldc_I4, paramsType.Length);
			ilGen.Emit(OpCodes.Newarr, typeof(object));
			ilGen.Emit(OpCodes.Stloc_2);

			ilGen.Emit(OpCodes.Ldarg_0);
			ilGen.Emit(OpCodes.Ldloc_S, (byte)4 ) ;		
			ilGen.Emit(OpCodes.Ldloc_2);

			if(paramsType.Length > 0) 
			{
				for(int i = 0; i < paramsType.Length; i++) 
				{
					ilGen.Emit(OpCodes.Ldc_I4, i);
					ilGen.Emit(OpCodes.Ldarg, i + 1);
					if (paramsType[i].IsValueType) 
					{
						ilGen.Emit(OpCodes.Box, paramsType[i]);
					}				        
					ilGen.Emit(OpCodes.Stelem_Ref);
					ilGen.Emit(OpCodes.Ldloc_2);
				}
			}
			ilGen.Emit(OpCodes.Call, BUSINESS_OBJECT_PREPROCESSING);
			ilGen.Emit(OpCodes.Brfalse, endMethodLabel);
		}

		//Emite a chamada para o metodo que faz o pre-processamento, na base service
		private static void EmitPosProcessingCall(MethodInfo method, ILGenerator ilGen, Type[] paramsType) 
		{
			ilGen.Emit(OpCodes.Ldarg_0);
			//load no method info
			ilGen.Emit(OpCodes.Ldloc_S, (byte)4 );
			//load na variavel de retorno
			ilGen.Emit(OpCodes.Ldloc_0);
			if(method.ReturnType.IsValueType) 
			{
				ilGen.Emit(OpCodes.Box, method.ReturnType);
			}
			ilGen.Emit(OpCodes.Ldloc_2);
			ilGen.Emit(OpCodes.Callvirt, BUSINESS_OBJECT_POSPROCESSING);
			if(method.ReturnType.IsValueType) 
			{
				ilGen.Emit(OpCodes.Unbox, method.ReturnType);
				ilGen.Emit(OpCodes.Ldobj, method.ReturnType);
			} 
			else 
			{
				ilGen.Emit(OpCodes.Castclass, method.ReturnType);
			}
			ilGen.Emit(OpCodes.Stloc_0);
		}
	}
}