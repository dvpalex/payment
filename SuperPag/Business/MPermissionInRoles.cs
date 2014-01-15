using System;
using System.Collections.Generic;
using System.Text;

namespace SuperPag.Business
{
    public class MPermissionInRoles
    {
         #region CONSTRUTORES
        
        public MPermissionInRoles()
        {}

        public MPermissionInRoles(int PermissionPagesId, int RoleId)
            {
                this.PermissionPagesId = PermissionPagesId;
                this.RoleId = RoleId;
            }

        public MPermissionInRoles(int PermissionPagesId)
        {
            this.PermissionPagesId = PermissionPagesId;
        }

        #endregion

        #region MEMBROS DE DADOS
        
        private int _PermissionPagesId;
        private int _RoleId;
                
        #endregion

        
        #region PROPRIEDADES
        public int PermissionPagesId
        {
            get { return _PermissionPagesId; }
            set { _PermissionPagesId = value; }
        }

        public int RoleId
        {
            get { return _RoleId;}
            set { _RoleId = value;}
        }

        #endregion
    }
}
