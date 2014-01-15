using System;
using SuperPag.Framework.Data.Components;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Data.Messages;
using SuperPag.Data;
using SuperPag.Framework;

namespace SuperPag.Agents.DepId.Messages
{
    [Serializable]
    public class MDeposit : Message
    {
      
        
        private int _checkId;
        public int CheckId
        {
            get { return _checkId; }
            set { _checkId = value; }
        }
        
        private string _tipo;
        public string Tipo
        {
            get { return _tipo; }
            set { _tipo = value; }
        }

        private decimal _valor;
        public decimal Valor
        {
            get { return _valor; }
            set { _valor = value; }
        }

        private string _status;
        public string Status
        {
            get { return _status; }
            set { _status = value; }
        }

        private string _data;
        public string Data
        {
            get { return _data; }
            set { _data = value; }
        }

        private int _numDocto;
        public int NumDocto
        {
            get { return _numDocto; }
            set { _numDocto = value; }
        }

        private int _agencia;
        public int Agencia
        {
            get { return _agencia; }
            set { _agencia = value; }
        }

        private string _cod_Depositante;
        public string Cod_Depositante
        {
            get { return _cod_Depositante; }
            set { _cod_Depositante = value; }
        }

        private string _num_Cheque;
        public string Num_Cheque
        {
            get { return _num_Cheque; }
            set { _num_Cheque = value; }
        }
    }
}
