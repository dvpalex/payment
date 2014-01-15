using System;
using System.Collections;
using SuperPag.Framework.Web.WebController;
using SuperPag.Framework.Web;
using SuperPag.Business.Messages;
using SuperPag.Business;
using Controller.Lib;
using Controller.Lib.Commands;

namespace Controller.Lib.Views.Ev.HandshakeConfigurationEdit
{
    public class Save : BaseEvent
    {
        protected override BaseCommand OnExecute()
        {
            BaseCommand b = null;

            MHandshakeConfiguration mHandshakeConfiguration = (MHandshakeConfiguration)this.GetMessage(typeof(MHandshakeConfiguration));

            HandshakeConfiguration.Save(mHandshakeConfiguration);

            if (mHandshakeConfiguration.HandshakeType == MHandshakeConfiguration.HandshakeTypeEnum.Html)
                b = this.MakeCommand(typeof(EditHandshakeConfigurationHtml));
            else
                b = this.MakeCommand(typeof(EditHandshakeConfigurationXml));

            b.Parameters["HandshakeConfigurationId"] = mHandshakeConfiguration.HandshakeConfigurationId;

            return b;
        }
    }

    public class Cancel : BaseEvent
    {
        protected override BaseCommand OnExecute()
        {
            BaseCommand b = null;

            b = this.MakeCommand(typeof(ListHandshakeConfiguration));
            return b;
        }
    }
}
