using System;
using System.Collections.Generic;
using System.Text;
using SuperPag.Business.Messages;
using SuperPag.Data.Messages;
using SuperPag.Data;
using SuperPag.Framework;

namespace SuperPag.Business
{
    public class HandshakeConfiguration
    {
        public static MCHandshakeConfiguration List(int storeId)
        {
            MCHandshakeConfiguration mcHandshakeConfiguration = null;
            DHandshakeConfiguration[] arrDHandshakeConfiguration = DataFactory.HandshakeConfiguration().List(storeId);

            if (arrDHandshakeConfiguration != null)
            {
                MessageMapper mapper = new MessageMapper();
                mcHandshakeConfiguration = (MCHandshakeConfiguration)mapper.Do(arrDHandshakeConfiguration, typeof(MCHandshakeConfiguration));
            }
            else
                mcHandshakeConfiguration = new MCHandshakeConfiguration();

            return mcHandshakeConfiguration;

        }

        public static int SelectMaxId()
        {
            DHandshakeConfigurationMaxId dHandshakeConfigurationMaxId = DataFactory.HandshakeConfiguration().MaxId();

            return dHandshakeConfigurationMaxId.handshakeConfigurationId;
        }

        public static MHandshakeConfiguration Locate(int handshakeConfigurationId)
        {
            DHandshakeConfiguration dHandshakeConfiguration = DataFactory.HandshakeConfiguration().Locate(handshakeConfigurationId);
            MessageMapper mapper = new MessageMapper();
            MHandshakeConfiguration mHandshakeConfiguration = (MHandshakeConfiguration)mapper.Do(dHandshakeConfiguration, typeof(MHandshakeConfiguration));
            return mHandshakeConfiguration;
        }

        public static void Save(MHandshakeConfiguration mHandshakeConfiguratio)
        {
            if (mHandshakeConfiguratio.IsNew)
            {
                mHandshakeConfiguratio.CreationDate = mHandshakeConfiguratio.LastUpdate = DateTime.Now;
                MessageMapper mapper = new MessageMapper();
                DHandshakeConfiguration dHandshakeConfiguration = (DHandshakeConfiguration)mapper.Do(mHandshakeConfiguratio, typeof(DHandshakeConfiguration));
                DataFactory.HandshakeConfiguration().Insert(dHandshakeConfiguration);
            }
            else
            {
                mHandshakeConfiguratio.LastUpdate = DateTime.Now;
                MessageMapper mapper = new MessageMapper();
                DHandshakeConfiguration dHandshakeConfiguration = (DHandshakeConfiguration)mapper.Do(mHandshakeConfiguratio, typeof(DHandshakeConfiguration));
                DataFactory.HandshakeConfiguration().Update(dHandshakeConfiguration);
            }
        }
    }
}
