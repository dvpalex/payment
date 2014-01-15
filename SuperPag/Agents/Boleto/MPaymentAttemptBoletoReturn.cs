using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using SuperPag.Framework.Data.Components;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Data.Messages;
using SuperPag.Data;
using SuperPag.Framework;


namespace SuperPag.Agents.Boleto.Messages
{    
    [Serializable()]
    [XmlRoot("CNAB")]
    public class MPaymentAttemptBoletoReturn 
    {
        public MPaymentAttemptBoletoReturn() { }

        private MPaymentAttemptBoletoReturnHeader _header;
        private MPaymentAttemptBoletoReturnDetails _details;

        [XmlElement("HEADER")]
        public MPaymentAttemptBoletoReturnHeader Header
        {
            get { return _header; }
            set { _header = value; }
        }

        [XmlElement("DETAILS")]
        public MPaymentAttemptBoletoReturnDetails Details
        {
            get { return _details; }
            set { _details = value; }
        }
    }  
    
    [Serializable()]
    [DefaultMapping(typeof(DPaymentAttemptBoletoReturnHeader))]
    public class MPaymentAttemptBoletoReturnHeader : Message
    {
        public MPaymentAttemptBoletoReturnHeader() { }

        private int _headerId;
        private int _bankNumber;
        private int _sequencialReturnNumber;
        private DateTime _recordFileDate;
        private string _companyName;
        private int _agencyNumber;
        private string _agencyDV;
        private int _assignorNumber;
        private string _assignorDV;
        private int _companyCode;
        private string _nameOfCapturedFile;
        private DateTime _creationDateCapturedFile;
        private string _nameOfArquivedFile;
        private DateTime _processDate;
        private int _numberOfDetailsRecords;

        [Mapping(DPaymentAttemptBoletoReturnHeader.Fields.headerId)]
        public int HeaderId
        {
            get { return _headerId; }
            set { _headerId = value; }
        }

        [Mapping(DPaymentAttemptBoletoReturnHeader.Fields.bankNumber)]
        [XmlElement("codigo_banco")]
        public int BankNumber
        {
            get { return _bankNumber; }
            set { _bankNumber = value; }
        }

        [Mapping(DPaymentAttemptBoletoReturnHeader.Fields.sequencialReturnNumber)]
        [XmlElement("sequencial_retorno")]
        public int SequencialReturnNumber
        {
            get { return _sequencialReturnNumber; }
            set { _sequencialReturnNumber = value; }
        }

        [Mapping(DPaymentAttemptBoletoReturnHeader.Fields.recordFileDate)]
        [XmlElement("data_gravacao")]
        public DateTime RecordFileDate
        {
            get { return _recordFileDate; }
            set { _recordFileDate = value; }
        }

        [Mapping(DPaymentAttemptBoletoReturnHeader.Fields.companyName)]
        [XmlElement("nome_empresa")]
        public string CompanyName
        {
            get { return _companyName; }
            set { _companyName = value; }
        }

        [Mapping(DPaymentAttemptBoletoReturnHeader.Fields.agencyNumber)]
        [XmlElement("prefixo_agencia")]
        public int AgencyNumber
        {
            get { return _agencyNumber; }
            set { _agencyNumber = value; }
        }

        [Mapping(DPaymentAttemptBoletoReturnHeader.Fields.agencyDV)]
        [XmlElement("prefixo_agencia_dv")]
        public string AgencyDV
        {
            get { return _agencyDV; }
            set { _agencyDV = value; }
        }

        [Mapping(DPaymentAttemptBoletoReturnHeader.Fields.assignorNumber)]
        [XmlElement("codigo_cedente")]
        public int AssignorNumber
        {
            get { return _assignorNumber; }
            set { _assignorNumber = value; }
        }

        [Mapping(DPaymentAttemptBoletoReturnHeader.Fields.assignorDV)]
        [XmlElement("codigo_cedente_dv")]
        public string AssignorDV
        {
            get { return _assignorDV; }
            set { _assignorDV = value; }
        }

        [Mapping(DPaymentAttemptBoletoReturnHeader.Fields.companyCode)]
        [XmlElement("numero_convenente")]
        public int CompanyCode        
        {
            get { return _companyCode; }
            set { _companyCode = value; }
        }

        [Mapping(DPaymentAttemptBoletoReturnHeader.Fields.nameOfCapturedFile)]
        public string NameOfCapturedFile
        {
            get { return _nameOfCapturedFile; }
            set { _nameOfCapturedFile = value; }
        }

        [Mapping(DPaymentAttemptBoletoReturnHeader.Fields.creationDateCapturedFile)]
        public DateTime CreationDateCapturedFile
        {
            get { return _creationDateCapturedFile; }
            set { _creationDateCapturedFile = value; }
        }

        [Mapping(DPaymentAttemptBoletoReturnHeader.Fields.nameOfArquivedFile)]
        public string NameOfArquivedFile
        {
            get { return _nameOfArquivedFile; }
            set { _nameOfArquivedFile = value; }
        }

        [Mapping(DPaymentAttemptBoletoReturnHeader.Fields.processDate)]
        public DateTime ProcessDate
        {
            get { return _processDate; }
            set { _processDate = value; }
        }

        [Mapping(DPaymentAttemptBoletoReturnHeader.Fields.numberOfDetailsRecords)]
        public int NumberOfDetailsRecords
        {
            get { return _numberOfDetailsRecords; }
            set { _numberOfDetailsRecords = value; }
        }


        
    }

    [Serializable()]
    public class MPaymentAttemptBoletoReturnDetails 
    {
        public MPaymentAttemptBoletoReturnDetails() { }

        private List<MPaymentAttemptBoletoReturnDetail> _detail;

        [XmlElement("DETAIL")]
        public List<MPaymentAttemptBoletoReturnDetail> Detail
        {
            get { return _detail; }
            set { _detail = value; }
        }

    }

    [Serializable()]
    [DefaultMapping(typeof(DPaymentAttemptBoletoReturn))]
    public class MPaymentAttemptBoletoReturnDetail : Message
    {
        public MPaymentAttemptBoletoReturnDetail() { }

        public enum BoletoReturnStatusEnum 
        {
            ProcessOk = 1,
            AttemptNotFound = 2,
            DuplicatePayment = 3,
            PaymentValueIncomplatible = 4
        }

        private int _paymentAttemptBoletoReturnId;
        private Guid _paymentAttemptId;
        private int _nossoNumero;
        private string _nossoNumeroDV;
        private int _comando;
        private int _naturezaRecebimento;
        private DateTime _dataLiquidacao;
        private decimal _valorTitulo;
        private int _codigoBancoRecebedor;
        private int _prefixoAgenciaRecebedora;
        private string _digitoAgenciaRecebedora;
        private DateTime _dataCredito;
        private decimal _valorTarifa;
        private decimal _outrasDespesas;
        private decimal _valorAbatimento;
        private decimal _valorDescontoConcedido;
        private decimal _valorRecebido;
        private decimal _jurosMora;
        private decimal _outrosRecebimentos;
        private decimal _valorCreditoConta;
        private int _indicativoCreditoDebito;
        private int _sequencialRegistro;
        private int _headerBoletoId;
        private DateTime _creationDate;
        private BoletoReturnStatusEnum _boletoReturnStatus;

        [Mapping(DPaymentAttemptBoletoReturn.Fields.paymentAttemptBoletoReturnId)]
        public int PaymentAttemptBoletoReturnId
        {
            get { return _paymentAttemptBoletoReturnId; }
            set { _paymentAttemptBoletoReturnId = value; }
        }
        [Mapping(DPaymentAttemptBoletoReturn.Fields.paymentAttemptId)]
        public Guid PaymentAttemptId
        {
            get { return _paymentAttemptId; }
            set { _paymentAttemptId = value; }
        }
        [Mapping(DPaymentAttemptBoletoReturn.Fields.nossoNumero)]
        [XmlElement("nosso_numero")]
        public int NossoNumero
        {
            get { return _nossoNumero; }
            set { _nossoNumero = value; }
        }
        [Mapping(DPaymentAttemptBoletoReturn.Fields.nossoNumeroDV)]
        [XmlElement("nosso_numero_dv")]
        public string NossoNumeroDV
        {
            get { return _nossoNumeroDV; }
            set { _nossoNumeroDV = value; }
        }
        [Mapping(DPaymentAttemptBoletoReturn.Fields.comando)]
        [XmlElement("comando")]
        public int Comando
        {
            get { return _comando; }
            set { _comando = value; }
        }
        [Mapping(DPaymentAttemptBoletoReturn.Fields.naturezaRecebimento)]
        [XmlElement("natureza_recebimento")]
        public int NaturezaRecebimento
        {
            get { return _naturezaRecebimento; }
            set { _naturezaRecebimento = value; }
        }
        [Mapping(DPaymentAttemptBoletoReturn.Fields.dataLiquidacao)]
        [XmlElement("data_liquidacao")]
        public DateTime DataLiquidacao
        {
            get { return _dataLiquidacao; }
            set { _dataLiquidacao = value; }
        }
        [Mapping(DPaymentAttemptBoletoReturn.Fields.valorTitulo)]
        [XmlElement("valor_titulo")]
        public decimal ValorTitulo
        {
            get { return _valorTitulo; }
            set { _valorTitulo = value; }
        }
        [Mapping(DPaymentAttemptBoletoReturn.Fields.codigoBancoRecebedor)]
        [XmlElement("codigo_banco_recebedor")]
        public int CodigoBancoRecebedor
        {
            get { return _codigoBancoRecebedor; }
            set { _codigoBancoRecebedor = value; }
        }
        [Mapping(DPaymentAttemptBoletoReturn.Fields.prefixoAgenciaRecebedora)]
        [XmlElement("prefixo_agencia_recebedora")]
        public int PrefixoAgenciaRecebedora
        {
            get { return _prefixoAgenciaRecebedora; }
            set { _prefixoAgenciaRecebedora = value; }
        }
        [Mapping(DPaymentAttemptBoletoReturn.Fields.digitoAgenciaRecebedora)]
        [XmlElement("prefixo_agencia_recebedora_dv")]
        public string DigitoAgenciaRecebedora
        {
            get { return _digitoAgenciaRecebedora; }
            set { _digitoAgenciaRecebedora = value; }
        }
        [Mapping(DPaymentAttemptBoletoReturn.Fields.dataCredito)]
        [XmlElement("data_credito")]
        public DateTime DataCredito
        {
            get { return _dataCredito; }
            set { _dataCredito = value; }
        }
        [Mapping(DPaymentAttemptBoletoReturn.Fields.valorTarifa)]
        [XmlElement("valor_tarifa")]
        public decimal ValorTarifa
        {
            get { return _valorTarifa; }
            set { _valorTarifa = value; }
        }
        [Mapping(DPaymentAttemptBoletoReturn.Fields.outrasDespesas)]
        [XmlElement("outras_despesas")]
        public decimal OutrasDespesas
        {
            get { return _outrasDespesas; }
            set { _outrasDespesas = value; }
        }
        [Mapping(DPaymentAttemptBoletoReturn.Fields.valorAbatimento)]
        [XmlElement("valor_abatimento")]
        public decimal ValorAbatimento
        {
            get { return _valorAbatimento; }
            set { _valorAbatimento = value; }
        }
        [Mapping(DPaymentAttemptBoletoReturn.Fields.valorDescontoConcedido)]
        [XmlElement("valor_desconto_concedido")]
        public decimal ValorDescontoConcedido
        {
            get { return _valorDescontoConcedido; }
            set { _valorDescontoConcedido = value; }
        }
        [Mapping(DPaymentAttemptBoletoReturn.Fields.valorRecebido)]
        [XmlElement("valor_recebido")]
        public decimal ValorRecebido
        {
            get { return _valorRecebido; }
            set { _valorRecebido = value; }
        }
        [Mapping(DPaymentAttemptBoletoReturn.Fields.jurosMora)]
        [XmlElement("juros_mora")]
        public decimal JurosMora
        {
            get { return _jurosMora; }
            set { _jurosMora = value; }
        }
        [Mapping(DPaymentAttemptBoletoReturn.Fields.outrosRecebimentos)]
        [XmlElement("outros_recebimentos")]
        public decimal OutrosRecebimentos
        {
            get { return _outrosRecebimentos; }
            set { _outrosRecebimentos = value; }
        }
        [Mapping(DPaymentAttemptBoletoReturn.Fields.valorCreditoConta)]
        [XmlElement("valor_lancamento")]
        public decimal ValorCreditoConta
        {
            get { return _valorCreditoConta; }
            set { _valorCreditoConta = value; }
        }
        [Mapping(DPaymentAttemptBoletoReturn.Fields.indicativoCreditoDebito)]
        [XmlElement("credito_debito")]
        public int IndicativoCreditoDebito
        {
            get { return _indicativoCreditoDebito; }
            set { _indicativoCreditoDebito = value; }
        }        
        [Mapping(DPaymentAttemptBoletoReturn.Fields.sequencialRegistro)]
        [XmlElement("sequencial_registro")]
        public int SequencialRegistro
        {
            get { return _sequencialRegistro; }
            set { _sequencialRegistro = value; }
        }        
        [Mapping(DPaymentAttemptBoletoReturn.Fields.headerBoletoId)]
        public int HeaderBoletoId
        {
            get { return _headerBoletoId; }
            set { _headerBoletoId = value; }
        }
        [Mapping(DPaymentAttemptBoletoReturn.Fields.creationDate)]
        public DateTime CreationDate
        {
            get { return _creationDate; }
            set { _creationDate = value; }
        }
        [Mapping(DPaymentAttemptBoletoReturn.Fields.status)]
        public BoletoReturnStatusEnum BoletoReturnStatus
        {
            get { return _boletoReturnStatus; }
            set { _boletoReturnStatus = value; }
        }

    }
}