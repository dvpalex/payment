using System;
using System.Collections.Generic;
using System.Text;

namespace SuperPag.Business.Messages
{
    public class MPaymentAttemptContaCorrente
    {
        public MPaymentAttemptContaCorrente() { }
                                                           
        public MPaymentAttemptContaCorrente(
                                            Guid PaymentAttemptId, string Ocorrencia, int? NrDocum, int? CodigoLogo,
                                            string Plastico, string DigVerAg, string NumAgencia, int Status, string DigVerCont,
                                            string NumContCorrent, DateTime DataVencimento, int? ControleEntradaId, int? ControleSaidaId,
                                            decimal ValorAgendado,int? CnabControleEntradaId,int? CnabControleSaidaId,DateTime? DataProcessamento,
                                            string OcorrenciaSuperpag,int? NumInstituicao
                                           )
        {
            this.PaymentAttemptId = PaymentAttemptId;
            this.Ocorrencia = Ocorrencia;
            this.NrDocum = NrDocum;
            this.CodigoLogo = CodigoLogo;
            this.Plastico = Plastico;
            this.DigVerAg = DigVerAg;
            this.NumAgencia = NumAgencia;
            this.Status = Status;
            this.DigVerCont = DigVerCont;
            this.NumContCorrent = NumContCorrent;
            this.DataVencimento = DataVencimento;
            this.ControleEntradaId = ControleEntradaId;
            this.ControleSaidaId = ControleSaidaId;
            this.ValorAgendado = ValorAgendado;
            this.CnabControleEntradaId = CnabControleEntradaId;
            this.CnabControleSaidaId = CnabControleSaidaId;
            this.DataProcessamento = DataProcessamento;
            this.OcorrenciaSuperpag = OcorrenciaSuperpag;
            this.NumInstituicao = NumInstituicao;
            this.DescricaoStatus = "";

        }

        public MPaymentAttemptContaCorrente(
                                            Guid PaymentAttemptId, 
                                            Int32? NumInstituicao, 
                                            DateTime DataVencimento, 
                                            int Status,
                                            string DescricaoStatus,
                                            decimal ValorAgendado,
                                            DateTime? DataProcessamento,
                                            DateTime? DataPagamento,
                                            string OcorrenciaSuperpag,                                
                                            string Ocorrencia,
                                            string NumAgencia,
                                            string NumContCorrent,
                                            string DigVerCont,
                                            string Plastico
                                           )
        {
            this.PaymentAttemptId = PaymentAttemptId;
            this.Ocorrencia = Ocorrencia;
            this.NrDocum = NrDocum;
            this.CodigoLogo = CodigoLogo;
            this.Plastico = Plastico;
            this.DigVerAg = DigVerAg;
            this.NumAgencia = NumAgencia;
            this.Status = Status;
            this.DigVerCont = DigVerCont;
            this.NumContCorrent = NumContCorrent;
            this.DataVencimento = DataVencimento;
            this.ControleEntradaId = ControleEntradaId;
            this.ControleSaidaId = ControleSaidaId;
            this.ValorAgendado = ValorAgendado;
            this.DescricaoStatus = DescricaoStatus;
            this.NumInstituicao = NumInstituicao;
            this.DataProcessamento = DataProcessamento;
            this.DataPagamento = DataPagamento;
            this.OcorrenciaSuperpag = OcorrenciaSuperpag;
        }


        public MPaymentAttemptContaCorrente(
                                            Guid PaymentAttemptId,
                                            Int32? NumInstituicao,
                                            DateTime DataVencimento,
                                            int Status,
                                            string DescricaoStatus,
                                            decimal ValorAgendado,
                                            DateTime? DataProcessamento,
                                            DateTime? DataTransacao,
                                            string OcorrenciaSuperpag,
                                            string Ocorrencia,
                                            string NumAgencia,
                                            string NumContCorrent,
                                            string DigVerCont,
                                            string Plastico,
                                            DateTime? DataCredito
                                           )
        {
            this.PaymentAttemptId = PaymentAttemptId;
            this.Ocorrencia = Ocorrencia;
            this.NrDocum = NrDocum;
            this.CodigoLogo = CodigoLogo;
            this.Plastico = Plastico;
            this.DigVerAg = DigVerAg;
            this.NumAgencia = NumAgencia;
            this.Status = Status;
            this.DigVerCont = DigVerCont;
            this.NumContCorrent = NumContCorrent;
            this.DataVencimento = DataVencimento;
            this.ControleEntradaId = ControleEntradaId;
            this.ControleSaidaId = ControleSaidaId;
            this.ValorAgendado = ValorAgendado;
            this.DescricaoStatus = DescricaoStatus;
            this.NumInstituicao = NumInstituicao;
            this.DataProcessamento = DataProcessamento;
            this.DataTransacao = DataTransacao;
            this.OcorrenciaSuperpag = OcorrenciaSuperpag;
            this.DataCredito = DataCredito;
        }



        public MPaymentAttemptContaCorrente(
                                            Guid PaymentAttemptId,
                                            Int32? NumInstituicao,
                                            DateTime DataVencimento,
                                            int Status,
                                            string DescricaoStatus,
                                            decimal ValorAgendado,
                                            DateTime? DataProcessamento,
                                            DateTime? DataTransacao,
                                            DateTime? DataPagamento,
                                            string OcorrenciaSuperpag,
                                            string Ocorrencia,
                                            string NumAgencia,
                                            string NumContCorrent,
                                            string DigVerCont,
                                            string Plastico
                                           )
        {
            this.PaymentAttemptId = PaymentAttemptId;
            this.Ocorrencia = Ocorrencia;
            this.NrDocum = NrDocum;
            this.CodigoLogo = CodigoLogo;
            this.Plastico = Plastico;
            this.DigVerAg = DigVerAg;
            this.NumAgencia = NumAgencia;
            this.Status = Status;
            this.DigVerCont = DigVerCont;
            this.NumContCorrent = NumContCorrent;
            this.DataVencimento = DataVencimento;
            this.ControleEntradaId = ControleEntradaId;
            this.ControleSaidaId = ControleSaidaId;
            this.ValorAgendado = ValorAgendado;
            this.DescricaoStatus = DescricaoStatus;
            this.NumInstituicao = NumInstituicao;
            this.DataProcessamento = DataProcessamento;
            this.DataPagamento = DataPagamento;
            this.DataTransacao = DataTransacao;
            this.OcorrenciaSuperpag = OcorrenciaSuperpag;
        }

        private Guid _PaymentAttemptId;
        private string _Ocorrencia;
        private int? _NrDocum;
        private int? _CodigoLogo;
        private string _Plastico;
        private string _DigVerAg;
        private string _NumAgencia;
        private int _Status;
        private string _DigVerCont;
        private string _NumContCorrent;
        private DateTime _DataVencimento;
        private int? _ControleEntradaId;
        private DateTime? _DataPagamento;
        private int? _ControleSaidaId;
        private decimal _ValorAgendado;
        private string _DescricaoStatus;
        private int? _NumInstituicao;
        private string _OcorrenciaSuperpag;
        private int? _CnabControleEntradaId;       
        private int? _CnabControleSaidaId;       
        private DateTime? _DataProcessamento;
        private DateTime? _DataTransacao;
        private DateTime? _DataCredito;

        public Guid PaymentAttemptId
        {
            get { return _PaymentAttemptId; }
            set { _PaymentAttemptId = value; }
        }
        /// <summary>
        /// Ocorrencia de retorno do banco
        /// </summary>
        /// 
        public int? NumInstituicao
        {
            get { return _NumInstituicao; }
            set { _NumInstituicao = value; }
        }
        public string DescricaoStatus
        {
            get { return _DescricaoStatus; }
            set { _DescricaoStatus = value; }
        }

        public string Ocorrencia
        {
            get { return _Ocorrencia; }
            set { _Ocorrencia = value; }
        }
        public int? NrDocum
        {
            get { return _NrDocum; }
            set { _NrDocum = value; }
        }
        public int? CodigoLogo
        {
            get { return _CodigoLogo; }
            set { _CodigoLogo = value; }
        }
        public string Plastico
        {
            get { return _Plastico; }
            set { _Plastico = value; }
        }
        public string DigVerAg
        {
            get { return _DigVerAg; }
            set { _DigVerAg = value; }
        }
        public string NumAgencia
        {
            get { return _NumAgencia; }
            set { _NumAgencia = value; }
        }
        public int Status
        {
            get { return _Status; }
            set { _Status = value; }
        }
        public string DigVerCont
        {
            get { return _DigVerCont; }
            set { _DigVerCont = value; }
        }
        public string NumContCorrent
        {
            get { return _NumContCorrent; }
            set { _NumContCorrent = value; }
        }
        public DateTime DataVencimento
        {
            get { return _DataVencimento; }
            set { _DataVencimento = value; }
        }
        public int? ControleEntradaId
        {
            get { return _ControleEntradaId; }
            set { _ControleEntradaId = value; }
        }
        public DateTime? DataPagamento
        {
            get { return _DataPagamento; }
            set { _DataPagamento = value; }
        }
        public DateTime? DataCredito
        {
            get { return _DataCredito; }
            set { _DataCredito = value; }
        }

        public int? ControleSaidaId
        {
            get { return _ControleSaidaId; }
            set { _ControleSaidaId = value; }
        }
        public decimal ValorAgendado
        {
            get { return _ValorAgendado; }
            set { _ValorAgendado = value; }
        }
        public string OcorrenciaSuperpag
        {
            get { return _OcorrenciaSuperpag; }
            set { _OcorrenciaSuperpag = value; }
        }
        public int? CnabControleEntradaId
        {
            get { return _CnabControleEntradaId; }
            set { _CnabControleEntradaId = value; }
        }
        public int? CnabControleSaidaId
        {
            get { return _CnabControleSaidaId; }
            set { _CnabControleSaidaId = value; }
        }
        public DateTime? DataProcessamento
        {
            get { return _DataProcessamento; }
            set { _DataProcessamento = value; }
        }
        public DateTime? DataTransacao
        {
            get { return _DataTransacao; }
            set { _DataTransacao = value; }
        }
    }
}
