using System;

namespace SuperPag.Framework
{
	[ AttributeUsage ( AttributeTargets.Class , AllowMultiple = false  ) ]
	public class DefaultMappingAttribute : Attribute 
	{
		public Type Type;

		public DefaultMappingAttribute ( Type type )
		{
			this.Type = type; 
		}
	}


	[ AttributeUsage ( AttributeTargets.Property , AllowMultiple = true ) ]
	public class MappingAttribute : Attribute 
	{
		public string Name;
		public Type Type;

		public MappingAttribute ( string name )
		{
			this.Name = name;			
		}

		public MappingAttribute ( string name , Type type )
		{
			this.Name = name;
			this.Type = type; 
		}

		public override bool IsDefaultAttribute()
		{
			return Type == null ;
		}
	}


}
