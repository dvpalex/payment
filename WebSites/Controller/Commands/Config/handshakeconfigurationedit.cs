using System;
using System.Collections;
using SuperPag.Framework.Web.WebController;
using SuperPag.Business.Messages;
using SuperPag.Business;
using SuperPag.Framework;
using Controller.Lib.Util;

namespace Controller.Lib.Commands
{
    public class EditHandshakeConfiguration : BaseCommand
	{
		protected override ViewInfo OnExecute()
		{
            MHandshakeConfiguration mHandshakeConfiguration = (MHandshakeConfiguration)this.Parameters["HandshakeConfiguration"];
            MCStore mcStore = Store.List(null);

            this.AddMessage(mHandshakeConfiguration);
            this.AddMessage(mcStore);
            this.AddEnumeration(new EnumListBuilder(typeof(MHandshakeConfiguration.HandshakeTypeEnum)));

            return Map.Views.EditHandshakeConfiguration;
		}
	}

    public class InsertHandshakeConfiguration : BaseCommand
    {
        protected override ViewInfo OnExecute()
        {
            MHandshakeConfiguration mHandshakeConfiguration = new MHandshakeConfiguration();
            mHandshakeConfiguration.IsNew = true;
            mHandshakeConfiguration.HandshakeConfigurationId = HandshakeConfiguration.SelectMaxId() + 1;

            MCStore mcStore = Store.List(null);

            this.AddMessage(mHandshakeConfiguration);
            this.AddMessage(mcStore);
            this.AddEnumeration(new EnumListBuilder(typeof(MHandshakeConfiguration.HandshakeTypeEnum)));

            return Map.Views.EditHandshakeConfiguration;
        }
    }


}
