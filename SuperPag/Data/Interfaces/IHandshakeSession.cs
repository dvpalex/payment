using System;
using SuperPag.Framework.Data.Components.AutoDataLayer.DataInterfaceAttributes;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Framework.Data.Mapping;
using SuperPag.Data.Messages;

namespace SuperPag.Data.Interfaces
{
    [DefaultDataMessage(typeof(DHandshakeSession))]
    public interface IHandshakeSession
    {
        [MethodType(MethodTypes.Query)]
        DHandshakeSession[] List();

        [MethodType(MethodTypes.Query)]
        [OrderBy(0, DHandshakeSession.Fields.createDate, SortOrder.DESC)]
        DHandshakeSession[] List(long orderId);

        [MethodType(MethodTypes.Query)]
        DHandshakeSession Locate(Guid handshakeSessionId);

        [MethodType(MethodTypes.Insert)]
        void Insert(DHandshakeSession dHandshakeSessionHtml);

        [MethodType(MethodTypes.Update)]
        void Update(DHandshakeSession dHandshakeSessionHtml);

        [MethodType(MethodTypes.Delete)]
        void Delete(Guid handshakeSessionId);
    }
}