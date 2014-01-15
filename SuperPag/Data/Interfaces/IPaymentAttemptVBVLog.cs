using System;
using SuperPag.Framework.Data.Components.AutoDataLayer.DataInterfaceAttributes;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Framework.Data.Mapping;
using SuperPag.Data.Messages;

namespace SuperPag.Data.Interfaces
{
	[DefaultDataMessage(typeof(DPaymentAttemptVBVLog))]
	public interface IPaymentAttemptVBVLog
	{
        [ MethodType ( MethodTypes.Query ) ]
        DPaymentAttemptVBVLog[] List();

        [MethodType(MethodTypes.Insert)]
		void Insert(DPaymentAttemptVBVLog DPaymentAttemptVBVLog);
	}
}