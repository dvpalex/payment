using System;
using System.Collections.Generic;
using System.Text;



namespace SuperPag.Business.Messages
{
    public class MOcorrencias 
    {
        #region CONSTRUTORES

        public MOcorrencias(
                                    String IdOcorrencia, String DscOcorrencia
                            
                                  )
        {
            this.IdOcorrencia = IdOcorrencia;
            this.DscOcorrencia = DscOcorrencia;

        }

        #endregion


        #region PROPRIEDADES
        private String _IdOcorrencia;
        private String _DscOcorrencia;
        #endregion

        #region MEMBROS DE DADOS

        public String IdOcorrencia
        {
            set { _IdOcorrencia = value; }
            get { return _IdOcorrencia; }
        }

        public String DscOcorrencia
        {
            set { _DscOcorrencia = value; }
            get { return _DscOcorrencia; }
        }
        #endregion

    }
}
