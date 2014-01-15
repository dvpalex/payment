using System;
using System.Collections.Generic;
using System.Text;



namespace SuperPag.Business.Messages
{
    public class MStatusProcessamento
    {
        #region CONSTRUTORES

        public MStatusProcessamento(int Id, string Descricao)
        {
            this.Id = Id;
            this.Descricao = Descricao;
        }

        #endregion

        #region MEMBROS DE DADOS
        // Adiciona os Campo para o Relatorio
        public int Id
        {
            get { return this._ID; }
            set { _ID = value; }
        }

        public string Descricao
        {
            get { return this._Descricao ; }
            set { _Descricao = value; }

        }

        #endregion

        #region PROPRIEDADES
        private int _ID;
        private string _Descricao;
        #endregion
    }
}
