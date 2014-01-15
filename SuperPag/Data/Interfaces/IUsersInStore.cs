using System;
using SuperPag.Framework.Data.Components.AutoDataLayer.DataInterfaceAttributes;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Framework.Data.Mapping;
using SuperPag.Data.Messages;

namespace SuperPag.Data.Interfaces
{
    [DefaultDataMessage(typeof(DUsersInStore))]
    public interface IUsersInStore
    {
        [MethodType(MethodTypes.Query)]
        DUsersInStore[] List(Guid UserId);
    }
}
