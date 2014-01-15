using System;
using System.Collections;
using SuperPag.Framework.Web.WebController;
using SuperPag.Business.Messages;
using SuperPag.Business;
using SuperPag.Framework;
using Controller.Lib.Util;

namespace Controller.Lib.Commands
{
    public class ListHandshakeConfiguration : BaseCommand
	{
		protected override ViewInfo OnExecute()
		{
            MCHandshakeConfiguration MCHandshakeConfiguration = HandshakeConfiguration.List(int.MinValue);

            this.AddMessage(MCHandshakeConfiguration);

            return Map.Views.ListHandshakeConfiguration;
		}
	}


}
