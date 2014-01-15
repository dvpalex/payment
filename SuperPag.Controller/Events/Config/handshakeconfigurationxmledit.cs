using System;
using System.Collections;
using SuperPag.Framework.Web.WebController;
using SuperPag.Framework.Web;
using SuperPag.Business.Messages;
using SuperPag.Business;
using Controller.Lib;
using Controller.Lib.Commands;

namespace Controller.Lib.Views.Ev.HandshakeConfigurationXmlEdit
{
    public class Save : BaseEvent
    {
        protected override BaseCommand OnExecute()
        {
            BaseCommand b = null;

            MHandshakeConfigurationXml mHandshakeConfigurationXml = (MHandshakeConfigurationXml)this.GetMessage(typeof(MHandshakeConfigurationXml));

            HandshakeConfigurationXml.Save(mHandshakeConfigurationXml);

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