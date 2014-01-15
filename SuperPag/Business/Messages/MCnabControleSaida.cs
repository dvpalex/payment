using System;
using System.Collections.Generic;
using System.Text;

namespace SuperPag.Business.Messages
{
    public class MCnabControleSaida
    {
        public MCnabControleSaida() { }
        public MCnabControleSaida(
                                  int CnabControleSaidaId, int NumInstituicao, DateTime DataArquivo, int NumSeqRemessa,
                                  string NomeArquivo, string NomeArquivoArmazenado, int QtdeDetalhes, decimal ValorTotalDetalhes, 
                                  int Status, string Ocorrencia
                                 )
        {
            this.CnabControleSaidaId = CnabControleSaidaId;
            this.NumInstituicao = NumInstituicao;
            this.DataArquivo = DataArquivo;
            this.NumSeqRemessa = NumSeqRemessa;
            this.NomeArquivo = NomeArquivo;
            this.QtdeDetalhes = QtdeDetalhes;
            this.ValorTotalDetalhes = ValorTotalDetalhes;
            this.Status = Status;
            this.Ocorrencia = Ocorrencia;
        }

        private int _CnabControleSaidaId;
        private int _NumInstituicao;
        private DateTime _DataArquivo;
        private int _NumSeqRemessa;
        private string _NomeArquivo;
        private string _NomeArquivoArmazenado;        
        private int _QtdeDetalhes;
        private decimal _ValorTotalDetalhes;
        private int _Status;
        private string _Ocorrencia;

        public int CnabControleSaidaId
        {
            get { return _CnabControleSaidaId; }
            set { _CnabControleSaidaId = value; }
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
        public string NomeArquivoArmazenado
        {
            get { return _NomeArquivoArmazenado; }
            set { _NomeArquivoArmazenado = value; }
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
        public int Status
        {
            get { return _Status; }
            set { _Status = value; }
        }
        public string Ocorrencia
        {
            get { return _Ocorrencia; }
            set { _Ocorrencia = value; }
        }
    }
}
