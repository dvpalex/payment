using System;
using System.Collections.Generic;
using System.Text;



namespace SuperPag.Business.Messages
{
    public class MStatusSuperPag 
    {
        #region CONSTRUTORES

        public MStatusSuperPag(
                                    Int32 IdStatus, String NmStatus,
                                    String DsStatus, Int32 IdWorkflowWeiseIT
                            
                                  )
        {
            this.IdStatus = IdStatus;
            this.NmStatus = NmStatus;
            this.DsStatus = DsStatus;
            this.IdWorkflowWeiseIT = IdWorkflowWeiseIT;
        }

        #endregion


        #region PROPRIEDADES
        private Int32 _IdStatus;
        private String _NmStatus;
        private String _DsStatus;
        private Int32 _IdWorkflowWeiseIT;
        #endregion

        #region MEMBROS DE DADOS

        public Int32 IdStatus
        {
            set {_IdStatus = value; }
            get {return _IdStatus; }
        }

        public String NmStatus
        {
            set{_NmStatus = value;}
            get {return _NmStatus; }
        }

        public String DsStatus
        {
            set { _DsStatus = value; }
            get { return _DsStatus; }
        }

        public Int32 IdWorkflowWeiseIT
        {
            get { return _IdWorkflowWeiseIT; }
            set { _IdWorkflowWeiseIT = value; }
        }


        #endregion

    }
}
