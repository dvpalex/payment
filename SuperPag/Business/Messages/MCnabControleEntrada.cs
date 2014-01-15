using System;
using System.Collections.Generic;
using System.Text;

namespace SuperPag.Business.Messages
{
    public class MCnabControleEntrada
    {
        public MCnabControleEntrada()
        { 
        }

        public MCnabControleEntrada(int CnabControleEntradaId, int NumInstituicao, DateTime DataArquivo,
                                    int NumSeqRemessa, string NomeArquivo, int QtdeDetalhes, decimal ValorTotalDetalhes,
                                    string Ocorrencia, int Status
                                   )
        {
            this.CnabControleEntradaId = CnabControleEntradaId;
            this.NumInstituicao = NumInstituicao;
            this.DataArquivo = DataArquivo;
            this.NumSeqRemessa = NumSeqRemessa;
            this.NomeArquivo = NomeArquivo;
            this.QtdeDetalhes = QtdeDetalhes;
            this.ValorTotalDetalhes = ValorTotalDetalhes;
            this.Ocorrencia = Ocorrencia;
            this.Status = Status;
        }

        private int _CnabControleEntradaId;
        private int _NumInstituicao;
        private DateTime _DataArquivo;
        private int _NumSeqRemessa;
        private string _NomeArquivo;
        private int _QtdeDetalhes;
        private decimal _ValorTotalDetalhes;
        private string _Ocorrencia;
        private int _Status;

        public int CnabControleEntradaId
        {
            get { return _CnabControleEntradaId; }
            set { _CnabControleEntradaId = value; }
        }
        public int NumInstituicao
        {
            get { return _NumInstituicao; }
            set { _NumInstituicao = value; }
        }
        public DateTime DataArquivo
        {
            get { return _DataArquivo; }
            set { _DataArquivo = value; }
        }
        public int NumSeqRemessa
        {
            get { return _NumSeqRemessa; }
            set { _NumSeqRemessa = value; }
        }
        public string NomeArquivo
        {
            get { return _NomeArquivo; }
            set { _NomeArquivo = value; }
        }
        public int QtdeDetalhes
        {
            get { return _QtdeDetalhes; }
            set { _QtdeDetalhes = value; }
        }
        public decimal ValorTotalDetalhes
        {
            get { return _ValorTotalDetalhes; }
            set { _ValorTotalDetalhes = value; }
        }
        public string Ocorrencia
        {
            get { return _Ocorrencia; }
            set { _Ocorrencia = value; }
        }
        public int Status
        {
            get { return _Status; }
            set { _Status = value; }
        }

    }
}
