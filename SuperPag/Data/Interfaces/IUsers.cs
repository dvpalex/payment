using System;
using SuperPag.Framework.Data.Components.AutoDataLayer.DataInterfaceAttributes;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Framework.Data.Mapping;
using SuperPag.Data.Messages;

namespace SuperPag.Data.Interfaces
{
    [DefaultDataMessage(typeof(DUsers))]
    public interface IUsers
    {
        [MethodType(MethodTypes.Query)]
        DUsers Locate(Guid UserId);

        [MethodType(MethodTypes.Query)]
        DUsers Locate(string LoweredUsername);

        [MethodType(MethodTypes.Query)]
        DUsers LocateByEmail(string LoweredEmail);

        [MethodType(MethodTypes.Query)]
        DUsers[] List();

        [MethodType(MethodTypes.Query)]
        [OrderBy(0, DUsers.Fields.Username, SortOrder.ASC)]
        DUsers[] ListSortedByUsername();

        [MethodType(MethodTypes.Query)]
        [Where(0, "Username", DUsers.Fields.Username, Filter.Like, Link.And)]
        [OrderBy(0, DUsers.Fields.Username, SortOrder.ASC)]
        DUsers[] ListLikeByUsername(string Username);

        [MethodType(MethodTypes.Query)]
        [Where(0, "Email", DUsers.Fields.Email, Filter.Like, Link.And)]
        [OrderBy(0, DUsers.Fields.Username, SortOrder.ASC)]
        DUsers[] ListLikeByEmail(string Email);

        [MethodType(MethodTypes.Query)]
        [Where(0, "date", DUsers.Fields.LastActivityDate, Filter.GreaterThan, Link.And)]
        DUsers[] ListGraterLastActivityDate(DateTime date);
        
        [MethodType(MethodTypes.Insert)]
        void Insert(DUsers dUsers);
        
        [MethodType(MethodTypes.Update)]
        void Update(DUsers dUsers);
        
        [MethodType(MethodTypes.Delete)]
        void Delete(Guid UserId);
    }
}
