using System;
using System.Collections;
using SuperPag.Framework.Web.WebController;
using SuperPag.Business.Messages;
using SuperPag.Business;
using SuperPag.Framework;
using Controller.Lib.Util;

namespace Controller.Lib.Commands
{
    public class EditHandshakeConfigurationXml : BaseCommand
    {
        protected override ViewInfo OnExecute()
        {
            int handshakeConfigurationId = (int)this.Parameters["HandshakeConfigurationId"];

            MHandshakeConfigurationXml mHandshakeConfigurationXml = HandshakeConfigurationXml.Locate(handshakeConfigurationId);

            if (mHandshakeConfigurationXml == null)
            {
                mHandshakeConfigurationXml = new MHandshakeConfigurationXml();
                mHandshakeConfigurationXml.HandshakeConfigurationId = handshakeConfigurationId;
            }

            this.AddMessage(mHandshakeConfigurationXml);

            return Map.Views.EditHandshakeConfigurationXml;
        }
    }


}
