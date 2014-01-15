using System;
using System.Collections.Generic;
using System.Text;

namespace SuperPag.Business.Messages
{
    public class MCSUControleEntradaContaCorrente
    {
        public MCSUControleEntradaContaCorrente(
                                                int ControleEntradaId, DateTime DataArquivo, int NumSeqRemessa, string NomeArquivoCapturado,
                                                DateTime DataCapturaArquivo, string NomeArquivoArmazenado, int QtdeDetalhes, decimal ValorTotalDetalhes,
                                                int NumInstituicao,int StatusProcessamento
                                               )
        {
            this.ControleEntradaId = ControleEntradaId;
            this.DataArquivo = DataArquivo;
            this.NumSeqRemessa = NumSeqRemessa;
            this.NomeArquivoCapturado = NomeArquivoCapturado;
            this.DataCapturaArquivo = DataCapturaArquivo;
            this.NomeArquivoArmazenado = NomeArquivoArmazenado;
            this.QtdeDetalhes = QtdeDetalhes;
            this.ValorTotalDetalhes = ValorTotalDetalhes;
            this.NumInstituicao = NumInstituicao;
            this.StatusProcessamento = StatusProcessamento;
        }
        public MCSUControleEntradaContaCorrente()
        { 
        }

        private int _ControleEntradaId;
        private DateTime _DataArquivo;
        private int _NumSeqRemessa;
        private string _NomeArquivoCapturado;
        private DateTime _DataCapturaArquivo;
        private string _NomeArquivoArmazenado;
        private int _QtdeDetalhes;
        private decimal _ValorTotalDetalhes;
        private int _NumInstituicao;
        private int _StatusProcessamento;

        public int ControleEntradaId
        {
            get { return _ControleEntradaId; }
            set { _ControleEntradaId = value; }
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
        public string NomeArquivoCapturado
        {
            get { return _NomeArquivoCapturado; }
            set { _NomeArquivoCapturado = value; }
        }
        public DateTime DataCapturaArquivo
        {
            get { return _DataCapturaArquivo; }
            set { _DataCapturaArquivo = value; }
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
        public int NumInstituicao
        {
            get { return _NumInstituicao; }
            set { _NumInstituicao = value; }
        }

        public int StatusProcessamento
        {
            get { return _StatusProcessamento; }
            set { _StatusProcessamento = value; }
        }
    }
}
