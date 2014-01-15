using System;
using System.Collections;
using SuperPag.Framework.Web.WebController;
using SuperPag.Framework.Web;
using SuperPag.Business.Messages;
using SuperPag.Business;
using Controller.Lib;
using Controller.Lib.Commands;

namespace Controller.Lib.Views.Ev.HandshakeConfigurationHtmlEdit
{
    public class Save : BaseEvent
    {
        protected override BaseCommand OnExecute()
        {
            BaseCommand b = null;

            MHandshakeConfigurationHtml mHandshakeConfigurationHtml = (MHandshakeConfigurationHtml)this.GetMessage(typeof(MHandshakeConfigurationHtml));

            HandshakeConfigurationHtml.Save(mHandshakeConfigurationHtml);

            b = this.MakeCommand(typeof(ListHandshakeConfiguration));

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
