using System;
using System.Web;
using System.Web.Security;
using System.Configuration.Provider;
using System.Collections.Specialized;
using System.Collections.Generic;
using SuperPag.Data;
using SuperPag.Data.Messages;

namespace SuperPag.Business.Membership
{
    public sealed class ControllerRoleProvider : RoleProvider
    {
        public override string ApplicationName
        {
            get { throw new NotSupportedException(); }
            set { throw new NotSupportedException(); }
        }
        public override void Initialize(string name, NameValueCollection config)
        {
            #region Default Implementation to call Base method
            if (config == null)
                throw new ArgumentNullException("config");

            if (String.IsNullOrEmpty(name))
                name = "ControllerRoleProvider";

            if (String.IsNullOrEmpty(config["description"]))
            {
                config.Remove("description");
                config.Add("description", "Controller Role Provider");
            }

            base.Initialize(name, config);
            #endregion

            // Throw an exception if unrecognized attributes remain
            if (config.Count > 0)
            {
                string attr = config.GetKey(0);
                if (!String.IsNullOrEmpty(attr))
                    throw new ProviderException
                        ("Unrecognized attribute: " + attr);
            }
        }
        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            foreach (string username in usernames)
            {
                DUsers user = DataFactory.Users().Locate(username.ToLower());
                if (user == null)
                    throw new SuperPag.Framework.FWCException("Username not found.");

                foreach (string rolename in roleNames)
                {
                    DRoles role = DataFactory.Roles().Locate(rolename.ToLower());
                    if (role == null)
                        throw new SuperPag.Framework.FWCException("Role name not found.");

                    DUsersInRoles userInRole = DataFactory.UsersInRoles().Locate(user.UserId, role.RoleId);
                    if (userInRole != null)
                        throw new SuperPag.Framework.FWCException("User is already in role.");
                    else
                    {
                        userInRole = new DUsersInRoles();
                        userInRole.UserId = user.UserId;
                        userInRole.RoleId = role.RoleId;
                        DataFactory.UsersInRoles().Insert(userInRole);
                    }
                }
            }
        }
        public override void CreateRole(string roleName)
        {
            if (roleName.IndexOf(',') > 0)
                throw new ArgumentException("Role names cannot contain commas.");

            DRoles role = DataFactory.Roles().Locate(roleName.ToLower());
            if (role != null)
                throw new SuperPag.Framework.FWCException("Role name already exists.");
            
            role = new DRoles();
            role.RoleId = Guid.NewGuid();
            role.RoleName = roleName;
            role.LoweredRoleName = roleName.ToLower();
            DataFactory.Roles().Insert(role);
        }
        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            DRoles role = DataFactory.Roles().Locate(roleName.ToLower());
            if (role == null)
                throw new SuperPag.Framework.FWCException("Role name not found.");
            DataFactory.Roles().Delete(role.RoleId);
            return true;
        }
        public override string[] GetAllRoles()
        {
            DRoles[] roles = DataFactory.Roles().List();
            List<string> roleNames = new List<string>();
            foreach (DRoles role in roles)
                roleNames.Add(role.RoleName);
            return roleNames.ToArray();
        }
        public override string[] GetRolesForUser(string username)
        {
            DUsers user = DataFactory.Users().Locate(username.ToLower());
            if (user == null)
                throw new SuperPag.Framework.FWCException("Username not found.");
            
            DUsersInRoles[] usersInRoles = DataFactory.UsersInRoles().ListByUser(user.UserId);
            List<string> roleNames = new List<string>();
            if (usersInRoles != null)
            {
                foreach (DUsersInRoles userInRole in usersInRoles)
                {
                    DRoles role = DataFactory.Roles().Locate(userInRole.RoleId);
                    roleNames.Add(role.RoleName);
                }
            }
            return roleNames.ToArray();
        }
        public override string[] GetUsersInRole(string roleName)
        {
            DRoles role = DataFactory.Roles().Locate(roleName.ToLower());
            if (role == null)
                throw new SuperPag.Framework.FWCException("Role name not found.");

            DUsersInRoles[] usersInRoles = DataFactory.UsersInRoles().ListByRole(role.RoleId);
            List<string> usernames = new List<string>();
            foreach (DUsersInRoles userInRole in usersInRoles)
            {
                DUsers user = DataFactory.Users().Locate(userInRole.UserId);
                usernames.Add(user.Username);
            }

            return usernames.ToArray();
        }
        public override bool IsUserInRole(string username, string roleName)
        {
            DUsers user = DataFactory.Users().Locate(username.ToLower());
            if (user == null)
                throw new SuperPag.Framework.FWCException("Username not found.");

            DRoles role = DataFactory.Roles().Locate(roleName.ToLower());
            if (role == null)
                throw new SuperPag.Framework.FWCException("Role name not found.");

            DUsersInRoles userInRole = DataFactory.UsersInRoles().Locate(user.UserId, role.RoleId);
            return userInRole != null;
        }
        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            foreach (string username in usernames)
            {
                DUsers user = DataFactory.Users().Locate(username.ToLower());
                if (user == null)
                    throw new SuperPag.Framework.FWCException("Username not found.");

                foreach (string rolename in roleNames)
                {
                    DRoles role = DataFactory.Roles().Locate(rolename.ToLower());
                    if (role == null)
                        throw new SuperPag.Framework.FWCException("Role name not found.");

                    DUsersInRoles userInRole = DataFactory.UsersInRoles().Locate(user.UserId, role.RoleId);
                    if (userInRole == null)
                        throw new SuperPag.Framework.FWCException("User is not in role.");
                    else
                        DataFactory.UsersInRoles().Delete(userInRole.UserId, userInRole.RoleId);
                }
            }
        }
        public override bool RoleExists(string roleName)
        {
            DRoles role = DataFactory.Roles().Locate(roleName.ToLower());
            return role != null;
        }
        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            DRoles role = DataFactory.Roles().Locate(roleName.ToLower());
            if (role == null)
                throw new SuperPag.Framework.FWCException("Role name not found.");
            
            DUsersInRoles[] usersInRole = DataFactory.UsersInRoles().ListByRoleSortByUsername(role.RoleId);
            List<string> usernames = new List<string>();
            foreach (DUsersInRoles userInRole in usersInRole)
            {
                DUsers user = DataFactory.Users().Locate(userInRole.UserId);
                usernames.Add(user.Username);
            }
            return usernames.ToArray();
        }

    }
}