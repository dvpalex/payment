using System;
using System.Collections.Generic;
using System.Text;

namespace SuperPag.Business.Messages
{
    public class MOcorrencia
    {
        private int _Codigo;        
        private string _CodigoSP;        
        private string _Descricao;
        
        public int Codigo
        {
            get { return _Codigo; }
            set { _Codigo = value; }
        }
        public string CodigoSP
        {
            get { return _CodigoSP; }
            set { _CodigoSP = value; }
        }
        public string Descricao
        {
            get { return _Descricao; }
            set { _Descricao = value; }
        }
    }
}
