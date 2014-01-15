using System;
using System.Collections.Generic;
using System.Text;
using SuperPag.Business.Messages;
using SuperPag.Data.Messages;
using SuperPag.Data;
using SuperPag.Framework;

namespace SuperPag.Business
{
    public class HandshakeConfigurationHtml
    {
        public static MHandshakeConfigurationHtml Locate(int handshakeConfigurationId)
        {
            DHandshakeConfigurationHtml dHandshakeConfigurationHtml = DataFactory.HandshakeConfigurationHtml().Locate(handshakeConfigurationId);

            MessageMapper mapper = new MessageMapper();
            MHandshakeConfigurationHtml mHandshakeConfigurationHtml = (MHandshakeConfigurationHtml)mapper.Do(dHandshakeConfigurationHtml, typeof(MHandshakeConfigurationHtml));

            return mHandshakeConfigurationHtml;

        }

        public static void Save(MHandshakeConfigurationHtml mHandshakeConfigurationHtml)
        {
            if (Locate(mHandshakeConfigurationHtml.HandshakeConfigurationId) == null)
            {
                MessageMapper mapper = new MessageMapper();
                DHandshakeConfigurationHtml dHandshakeConfigurationHtml = (DHandshakeConfigurationHtml)mapper.Do(mHandshakeConfigurationHtml, typeof(DHandshakeConfigurationHtml));
                DataFactory.HandshakeConfigurationHtml().Insert(dHandshakeConfigurationHtml);
            }
            else
            {
                MessageMapper mapper = new MessageMapper();
                DHandshakeConfigurationHtml dHandshakeConfigurationHtml = (DHandshakeConfigurationHtml)mapper.Do(mHandshakeConfigurationHtml, typeof(DHandshakeConfigurationHtml));
                DataFactory.HandshakeConfigurationHtml().Update(dHandshakeConfigurationHtml);
            }
        }

    }
}
