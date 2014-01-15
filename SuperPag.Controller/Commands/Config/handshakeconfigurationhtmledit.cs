using System;
using System.Collections;
using SuperPag.Framework.Web.WebController;
using SuperPag.Business.Messages;
using SuperPag.Business;
using SuperPag.Framework;
using Controller.Lib.Util;

namespace Controller.Lib.Commands
{
    public class EditHandshakeConfigurationHtml : BaseCommand
	{
		protected override ViewInfo OnExecute()
		{
            int handshakeConfigurationId = (int)this.Parameters["HandshakeConfigurationId"];

            MHandshakeConfigurationHtml mHandshakeConfigurationHtml = HandshakeConfigurationHtml.Locate(handshakeConfigurationId);

            if (mHandshakeConfigurationHtml == null)
            {
                mHandshakeConfigurationHtml = new MHandshakeConfigurationHtml();
                mHandshakeConfigurationHtml.HandshakeConfigurationId = handshakeConfigurationId;
            }

            this.AddMessage(mHandshakeConfigurationHtml);

            return Map.Views.EditHandshakeConfigurationHtml;
		}
	}


}
