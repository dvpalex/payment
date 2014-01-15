using System;
using System.Collections.Generic;
using System.Text;
using SuperPag.Business.Messages;
using SuperPag.Data.Messages;
using SuperPag.Data;
using SuperPag.Framework;

namespace SuperPag.Business
{
    public class ConsumerAddress
    {
        public static MConsumerAddress Locate(long consumerId, MConsumerAddress.AddressTypes addressType)
        {
            DConsumerAddress dConsumerAddress = DataFactory.ConsumerAddress().Locate(consumerId, (int)addressType);

            MessageMapper mapper = new MessageMapper();
            MConsumerAddress mConsumerAddress = (MConsumerAddress)mapper.Do(dConsumerAddress, typeof(MConsumerAddress));
            return mConsumerAddress;
        }
    }
}
