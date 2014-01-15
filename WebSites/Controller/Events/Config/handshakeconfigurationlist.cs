using System;
using System.Collections;
using SuperPag.Framework.Web.WebController;
using SuperPag.Framework.Web;
using SuperPag.Business.Messages;
using SuperPag.Business;
using Controller.Lib;
using Controller.Lib.Commands;

namespace Controller.Lib.Views.Ev.HandshakeConfigurationList
{
    public class HandshakeConfigurationEdit : BaseEvent
    {
        protected override BaseCommand OnExecute()
        {
            BaseCommand b = null;

            MHandshakeConfiguration mHandshakeConfiguration = (MHandshakeConfiguration)this.GetMessage(typeof(MHandshakeConfiguration));
            
            b = this.MakeCommand(typeof(EditHandshakeConfiguration));
            b.Parameters["HandshakeConfiguration"] = mHandshakeConfiguration;

            return b;
        }
    }

    public class HandshakeConfigurationInsert : BaseEvent
    {
        protected override BaseCommand OnExecute()
        {
            BaseCommand b = null;

            b = this.MakeCommand(typeof(InsertHandshakeConfiguration));

            return b;
        }
    }

}
