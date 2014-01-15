using System;
using SuperPag.Framework.Data.Components.AutoDataLayer.DataInterfaceAttributes;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Framework.Data.Mapping;
using SuperPag.Data.Messages;

namespace SuperPag.Data.Interfaces
{
    [DefaultDataMessage(typeof(DServicesConfiguration))]
    public interface IServicesConfiguration
    {
        [MethodType(MethodTypes.Query)]
        DServicesConfiguration[] List();

        [MethodType(MethodTypes.Query)]
        DServicesConfiguration Locate(int storeId);
    }
}
