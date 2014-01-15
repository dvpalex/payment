using System;
using SuperPag.Framework.Data.Components.AutoDataLayer.DataInterfaceAttributes;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Framework.Data.Mapping;
using SuperPag.Data.Messages;

namespace SuperPag.Data.Interfaces
{
    [DefaultDataMessage(typeof(DRoles))]
    public interface IRoles
    {
        [MethodType(MethodTypes.Query)]
        DRoles Locate(Guid RoleId);

        [MethodType(MethodTypes.Query)]
        DRoles Locate(string LoweredRoleName);

        [MethodType(MethodTypes.Query)]
        DRoles[] List();

        [MethodType(MethodTypes.Insert)]
        void Insert(DRoles dRoles);
        
        [MethodType(MethodTypes.Update)]
        void Update(DRoles dRoles);
        
        [MethodType(MethodTypes.Delete)]
        void Delete(Guid RoleId);
    }
}
