using System;
using System.Collections.Generic;
using System.Text;

namespace SuperPag.Business.Messages
{
    public class ControleSaidaContaCorrente
    {
        private int _ControleSaidaId;
        private DateTime _DataGeracaoLote;
        private int _NumLote;
        private int _QtdeDetalhes;
        private decimal _ValorTotalDetalhes;
        private string _NomeArquivoEnviado;
        private string _NomeArquivoArmazenado;
        private string _ProtocoloRecebimento;
        private int _StatusProcessamento;

        public string ProtocoloRecebimento
        {
            get { return _ProtocoloRecebimento; }
            set { _ProtocoloRecebimento = value; }
        }        
        public int ControleSaidaId
        {
            get { return _ControleSaidaId; }
            set { _ControleSaidaId = value; }
        }
        public DateTime DataGeracaoLote
        {
            get { return _DataGeracaoLote; }
            set { _DataGeracaoLote = value; }
        }
        public int NumLote
        {
            get { return _NumLote; }
            set { _NumLote = value; }
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
        public string NomeArquivoEnviado
        {
            get { return _NomeArquivoEnviado; }
            set { _NomeArquivoEnviado = value; }
        }
        public string NomeArquivoArmazenado
        {
            get { return _NomeArquivoArmazenado; }
            set { _NomeArquivoArmazenado = value; }
        }
        public int StatusProcessamento
        {
            get { return _StatusProcessamento; }
            set { _StatusProcessamento = value; }
        }
    }
}
