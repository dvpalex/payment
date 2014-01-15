using System;

namespace SuperPag.Framework.Data.Mapping
{
	[AttributeUsage(AttributeTargets.Parameter)]
	public class UsedForBind : Attribute 
	{
		Type _dataMessageType;
		
		public Type DataMessageType 
		{
			get
			{
				return this._dataMessageType;
			}
		}

		public UsedForBind( Type dataMessageType )
		{
			this._dataMessageType = dataMessageType;
		}
	}


	[ AttributeUsage( AttributeTargets.Method ) ]
	public class BindingAllowed : Attribute
	{
		Type _dataMessageType;
		
		public Type DataMessageType 
		{
			get
			{
				return this._dataMessageType;
			}
		}

		public BindingAllowed( Type dataMessageType )
		{
			this._dataMessageType = dataMessageType;
		}
	}
	
	[ AttributeUsage( AttributeTargets.Field ) ]
	public class Bindable : Attribute
	{
		Type _methodType;
		
		public Type MethodType 
		{
			get
			{
				return this._methodType;
			}
		}
		
		public Bindable( Type methodType )
		{
			this._methodType = methodType;
		}
	}
}
