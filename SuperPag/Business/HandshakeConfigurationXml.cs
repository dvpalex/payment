using System;
using System.Collections.Generic;
using System.Text;
using SuperPag.Business.Messages;
using SuperPag.Data.Messages;
using SuperPag.Data;
using SuperPag.Framework;

namespace SuperPag.Business
{
    public class HandshakeConfigurationXml
    {
        public static MHandshakeConfigurationXml Locate(int handshakeConfigurationId)
        {
            DHandshakeConfigurationXml dHandshakeConfigurationXml = DataFactory.HandshakeConfigurationXml().Locate(handshakeConfigurationId);

            MessageMapper mapper = new MessageMapper();
            MHandshakeConfigurationXml mHandshakeConfigurationXml = (MHandshakeConfigurationXml)mapper.Do(dHandshakeConfigurationXml, typeof(MHandshakeConfigurationXml));

            return mHandshakeConfigurationXml;

        }

        public static void Save(MHandshakeConfigurationXml mHandshakeConfigurationXml)
        {
            if (Locate(mHandshakeConfigurationXml.HandshakeConfigurationId) == null)
            {
                MessageMapper mapper = new MessageMapper();
                DHandshakeConfigurationXml dHandshakeConfigurationXml = (DHandshakeConfigurationXml)mapper.Do(mHandshakeConfigurationXml, typeof(DHandshakeConfigurationXml));
                DataFactory.HandshakeConfigurationXml().Insert(dHandshakeConfigurationXml);
            }
            else
            {
                MessageMapper mapper = new MessageMapper();
                DHandshakeConfigurationXml dHandshakeConfigurationXml = (DHandshakeConfigurationXml)mapper.Do(mHandshakeConfigurationXml, typeof(DHandshakeConfigurationXml));
                DataFactory.HandshakeConfigurationXml().Update(dHandshakeConfigurationXml);
            }
        }

    }
}
