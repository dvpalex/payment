using System;
using SuperPag.Framework.Data.Components.AutoDataLayer.DataInterfaceAttributes;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Framework.Data.Mapping;
using SuperPag.Data.Messages;

namespace SuperPag.Data.Interfaces
{
    [DefaultDataMessage(typeof(DUsersInRoles))]
    public interface IUsersInRoles
    {
        [MethodType(MethodTypes.Query)]
        DUsersInRoles Locate(Guid UserId, Guid RoleId);

        [MethodType(MethodTypes.Query)]
        DUsersInRoles[] ListByUser(Guid UserId);

        [MethodType(MethodTypes.Query)]
        DUsersInRoles[] ListByRole(Guid RoleId);

        [MethodType(MethodTypes.Query)]
        [DataRelation(typeof(DUsersInRoles), DUsersInRoles.Fields.UserId, typeof(DUsers), DUsers.Fields.UserId, Join.Inner)]
        [Where(0, "RoleId", DUsersInRoles.Fields.RoleId, Filter.Equal, Link.And)]
        [OrderBy(0,typeof(DUsers), DUsers.Fields.Username, SortOrder.ASC)]
        DUsersInRoles[] ListByRoleSortByUsername(Guid RoleId);

        [MethodType(MethodTypes.Insert)]
        void Insert(DUsersInRoles dUsersInRoles);
               
        [MethodType(MethodTypes.Delete)]
        void Delete(Guid UserId, Guid RoleId);
    }
}
