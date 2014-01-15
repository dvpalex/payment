using System;
using System.Collections.Generic;
using System.Text;
using SuperPag.Business.Messages;
using SuperPag.Data.Messages;
using SuperPag.Data;
using SuperPag.Framework;

namespace SuperPag.Business
{
    public class Consumer
    {
        public static MConsumer Locate(long consumerId)
        {
            if (consumerId.Equals(long.MinValue))
                return null;
            
            DConsumer dConsumer = DataFactory.Consumer().Locate(consumerId);

            MessageMapper mapper = new MessageMapper();
            MConsumer mConsumer = (MConsumer)mapper.Do(dConsumer, typeof(MConsumer));
            return mConsumer;

        }

    }
}
