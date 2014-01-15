using System;
using System.Collections.Generic;
using System.Text;

namespace SuperPag.Business.Messages
{
    class MPermissionPages
    {
        #region CONSTRUTORES
        
        public MPermissionPages()
        {}

        public MPermissionPages(int PermissionPagesId, int PageId, string Description)
            {
                this.PermissionPagesId = PermissionPagesId;
                this.PageId = PageId;
                this.Description = Description;
            }

        #endregion

        #region MEMBROS DE DADOS
        
        private int _PermissionPagesId;
        private int _PageId;
        private string _Description;
        
        #endregion

        #region PROPRIEDADES
        public int PermissionPagesId
        {
            get { return _PermissionPagesId; }
            set { _PermissionPagesId = value; }
        }

        public int PageId
        {
            get { return _PageId;}
            set { _PageId = value;}
        }

        public string Description
        {
            get { return _Description; }
            set {_Description = value;}
        }
        #endregion
    }
}
