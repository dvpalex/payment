using System;

namespace SuperPag.Framework.Business
{

	[ AttributeUsage ( AttributeTargets.Method ) ]
	public class NoPreProcessingAttribute : Attribute
	{
	}
}
