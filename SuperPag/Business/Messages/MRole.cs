using System;
using System.Collections.Generic;
using System.Text;



namespace SuperPag.Business.Messages
{
    public class MRole
    {
        #region CONSTRUTORES
        
        public MRole(Guid ApplicationId,
                     Guid RoleId, 
                     String RoleName, 
                     String LoweredRoleName, 
                     String Description, 
                     int StoreId
                    )
        {
           this.ApplicationId = ApplicationId;
           this.RoleId = RoleId;
           this.RoleName = RoleName;
           this.LoweredRoleName = LoweredRoleName;
           this.Description = Description;
           this.StoreId = StoreId;
        }

        #endregion

        #region MEMBROS DE DADOS
        private Guid _ApplicationId;
            private Guid _RoleId;
            private String _RoleName;
            private String _LoweredRoleName;
            private String _Description;
            private int _StoreId;
        #endregion
        
        #region PROPRIEDADES
            public Guid ApplicationId
            {
                get {  return _ApplicationId;  }
                set {  _ApplicationId = value;  }
            }

            public Guid RoleId
            {
                get { return _RoleId; }
                set { _RoleId = value; }
            }

            public String RoleName
            {
                get { return _RoleName; }
                set { _RoleName = value; }
            }

            public String LoweredRoleName
            {
                get { return _LoweredRoleName; }
                set { _LoweredRoleName = value; }
            }

            public String Description
            {
                get { return _Description; }
                set { _Description = value; }
            }

            public int StoreId
            {
                get { return _StoreId; }
                set { _StoreId = value; }
            }
        #endregion
    }
}
