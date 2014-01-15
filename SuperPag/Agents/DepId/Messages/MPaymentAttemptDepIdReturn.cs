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


namespace SuperPag.Agents.DepId.Messages
{
    [Serializable()]
    [XmlRoot("CNAB")]
    public class MPaymentAttemptDepIdReturn
    {
        public MPaymentAttemptDepIdReturn() { }

        private MPaymentAttemptDepIdReturnHeader _header;
        private MPaymentAttemptDepIdReturnDetails _details;

        [XmlElement("HEADER")]
        public MPaymentAttemptDepIdReturnHeader Header
        {
            get { return _header; }
            set { _header = value; }
        }

        [XmlElement("DETAILS")]
        public MPaymentAttemptDepIdReturnDetails Details
        {
            get { return _details; }
            set { _details = value; }
        }
    }

    [Serializable()]
    [DefaultMapping(typeof(DPaymentAttemptDepIdReturnHeader))]
    public class MPaymentAttemptDepIdReturnHeader : Message
    {
        public MPaymentAttemptDepIdReturnHeader() { }

        private int _headerId;
        private int _bankNumber;
        private int _sequencialReturnNumber;
        private DateTime _recordFileDate;
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

        [Mapping(DPaymentAttemptDepIdReturnHeader.Fields.headerId)]
        public int HeaderId
        {
            get { return _headerId; }
            set { _headerId = value; }
        }

        [Mapping(DPaymentAttemptDepIdReturnHeader.Fields.bankNumber)]
        [XmlElement("codigo_banco")]
        public int BankNumber
        {
            get { return _bankNumber; }
            set { _bankNumber = value; }
        }

        [Mapping(DPaymentAttemptDepIdReturnHeader.Fields.sequencialReturnNumber)]
        [XmlElement("num_sequencial_registro")]
        public int SequencialReturnNumber
        {
            get { return _sequencialReturnNumber; }
            set { _sequencialReturnNumber = value; }
        }

        [Mapping(DPaymentAttemptDepIdReturnHeader.Fields.recordFileDate)]
        [XmlElement("data_gravacao")]
        public DateTime RecordFileDate
        {
            get { return _recordFileDate; }
            set { _recordFileDate = value; }
        }

        [Mapping(DPaymentAttemptDepIdReturnHeader.Fields.agencyNumber)]
        [XmlElement("codigo_agencia")]
        public int AgencyNumber
        {
            get { return _agencyNumber; }
            set { _agencyNumber = value; }
        }

        [Mapping(DPaymentAttemptDepIdReturnHeader.Fields.agencyDV)]
        [XmlElement("digito_agencia")]
        public string AgencyDV
        {
            get { return _agencyDV; }
            set { _agencyDV = value; }
        }

        [Mapping(DPaymentAttemptDepIdReturnHeader.Fields.assignorNumber)]
        [XmlElement("num_conta_corrente")]
        public int AssignorNumber
        {
            get { return _assignorNumber; }
            set { _assignorNumber = value; }
        }

        [Mapping(DPaymentAttemptDepIdReturnHeader.Fields.assignorDV)]
        [XmlElement("dig_conta_corrente")]
        public string AssignorDV
        {
            get { return _assignorDV; }
            set { _assignorDV = value; }
        }

        [Mapping(DPaymentAttemptDepIdReturnHeader.Fields.companyCode)]
        [XmlElement("numero_inscricao_cliente")]
        public int CompanyCode
        {
            get { return _companyCode; }
            set { _companyCode = value; }
        }

        [Mapping(DPaymentAttemptDepIdReturnHeader.Fields.nameOfCapturedFile)]
        public string NameOfCapturedFile
        {
            get { return _nameOfCapturedFile; }
            set { _nameOfCapturedFile = value; }
        }

        [Mapping(DPaymentAttemptDepIdReturnHeader.Fields.creationDateCapturedFile)]
        public DateTime CreationDateCapturedFile
        {
            get { return _creationDateCapturedFile; }
            set { _creationDateCapturedFile = value; }
        }

        [Mapping(DPaymentAttemptDepIdReturnHeader.Fields.nameOfArquivedFile)]
        public string NameOfArquivedFile
        {
            get { return _nameOfArquivedFile; }
            set { _nameOfArquivedFile = value; }
        }

        [Mapping(DPaymentAttemptDepIdReturnHeader.Fields.processDate)]
        public DateTime ProcessDate
        {
            get { return _processDate; }
            set { _processDate = value; }
        }

        [Mapping(DPaymentAttemptDepIdReturnHeader.Fields.numberOfDetailsRecords)]
        public int NumberOfDetailsRecords
        {
            get { return _numberOfDetailsRecords; }
            set { _numberOfDetailsRecords = value; }
        }
    }

    [Serializable()]
    public class MPaymentAttemptDepIdReturnDetails
    {
        public MPaymentAttemptDepIdReturnDetails() { }

        private List<MPaymentAttemptDepIdReturnDetail> _detail;

        [XmlElement("DETAIL")]
        public List<MPaymentAttemptDepIdReturnDetail> Detail
        {
            get { return _detail; }
            set { _detail = value; }
        }

    }

    [Serializable()]
    [DefaultMapping(typeof(DPaymentAttemptDepIdReturn))]
    public class MPaymentAttemptDepIdReturnDetail : Message
    {
        public MPaymentAttemptDepIdReturnDetail() { }

        private int _paymentAttemptDepIdReturnId;
        private Guid _paymentAttemptId;
        private DateTime _data_deposito;
        private int _ag_acolhedora;
        private int _digito_agencia;
        private string _remetente_deposito;
        private decimal _valor_deposito_dinheiro;
        private decimal _valor_deposito_cheque;
        private decimal _valor_total_deposito;
       // private int _digitoAgenciaRecebedora;
        private int _numero_documento;
        private int _cod_canal_distribuicao;
        private int _num_sequencia_arquivo;
        private int _headerDepIdentId;
        private DateTime _creationDate;
        private int _status;
        private MPaymentAttemptDepIdReturnChks _checks;

        [Mapping(DPaymentAttemptDepIdReturn.Fields.paymentAttemptDepIdReturnId)]
        public int PaymentAttemptDepIdReturnId
        {
            get { return _paymentAttemptDepIdReturnId; }
            set { _paymentAttemptDepIdReturnId = value; }
        }

        [Mapping(DPaymentAttemptDepIdReturn.Fields.paymentAttemptId)]
        public Guid PaymentAttemptId
        {
            get { return _paymentAttemptId; }
            set { _paymentAttemptId = value; }
        }

        [Mapping(DPaymentAttemptDepIdReturn.Fields.data_deposito)]
        [XmlElement("data_deposito")]
        public DateTime Data_deposito
        {
            get { return _data_deposito; }
            set { _data_deposito = value; }
        }

        [Mapping(DPaymentAttemptDepIdReturn.Fields.ag_acolhedora)]
        [XmlElement("ag_acolhedora")]
        public int Ag_acolhedora
        {
            get { return _ag_acolhedora; }
            set { _ag_acolhedora = value; }
        }

        [Mapping(DPaymentAttemptDepIdReturn.Fields.digito_agencia)]
        [XmlElement("dig_ag_acolhedora")]
        public int Digito_agencia
        {
            get { return _digito_agencia; }
            set { _digito_agencia = value; }
        }

        [Mapping(DPaymentAttemptDepIdReturn.Fields.remetente_deposito)]
        [XmlElement("remetente")]
        public string Remetente_deposito
        {
            get { return _remetente_deposito; }
            set { _remetente_deposito = value; }
        }

        [Mapping(DPaymentAttemptDepIdReturn.Fields.valor_deposito_dinheiro)]
        [XmlElement("vlr_total_dinheiro")]
        public decimal Valor_deposito_dinheiro
        {
            get { return _valor_deposito_dinheiro; }
            set { _valor_deposito_dinheiro = value; }
        }

        [Mapping(DPaymentAttemptDepIdReturn.Fields.valor_deposito_cheque)]
        [XmlElement("vlr_total_cheque")]
        public decimal Valor_deposito_cheque
        {
            get { return _valor_deposito_cheque; }
            set { _valor_deposito_cheque = value; }
        }

        [Mapping(DPaymentAttemptDepIdReturn.Fields.valor_total_deposito)]
        [XmlElement("vlr_total_deposito")]
        public decimal Valor_total_deposito
        {
            get { return _valor_total_deposito; }
            set { _valor_total_deposito = value; }
        }

        //[Mapping(DPaymentAttemptDepIdReturn.Fields.digitoAgenciaRecebedora)]
        //[XmlElement("digitoAgenciaRecebedora")]
        //public int DigitoAgenciaRecebedora
        //{
        //    get { return _digitoAgenciaRecebedora; }
        //    set { _digitoAgenciaRecebedora = value; }
        //}

        [Mapping(DPaymentAttemptDepIdReturn.Fields.numero_documento)]
        [XmlElement("num_documento")]
        public int Numero_documento
        {
            get { return _numero_documento; }
            set { _numero_documento = value; }
        }

        [Mapping(DPaymentAttemptDepIdReturn.Fields.cod_canal_distribuicao)]
        [XmlElement("cod_canal_distribuicao")]
        public int Cod_canal_distribuicao
        {
            get { return _cod_canal_distribuicao; }
            set { _cod_canal_distribuicao = value; }
        }

        [Mapping(DPaymentAttemptDepIdReturn.Fields.num_sequencia_arquivo)]
        [XmlElement("numero_sequencial")]
        public int Num_sequencia_arquivo
        {
            get { return _num_sequencia_arquivo; }
            set { _num_sequencia_arquivo = value; }
        }

        [Mapping(DPaymentAttemptDepIdReturn.Fields.headerDepIdentId)]
        public int HeaderDepIdentId
        {
            get { return _headerDepIdentId; }
            set { _headerDepIdentId = value; }
        }

        [Mapping(DPaymentAttemptDepIdReturn.Fields.creationDate)]
        public DateTime CreationDate
        {
            get { return _creationDate; }
            set { _creationDate = value; }
        }

        [Mapping(DPaymentAttemptDepIdReturn.Fields.status)]
        public SuperPag.DepIdStatusEnum Status
        {
            get { return (SuperPag.DepIdStatusEnum)_status; }
            set { _status = (int)value; }
        }

        [XmlElement("DETAILS")]
        public MPaymentAttemptDepIdReturnChks Checks
        {
            get { return _checks; }
            set { _checks = value; }
        }
    }

    [Serializable()]
    public class MPaymentAttemptDepIdReturnChks
    {
        public MPaymentAttemptDepIdReturnChks() { }

        private List<MPaymentAttemptDepIdReturnChk> _checks;
        [XmlElement("DETAIL")]
        public List<MPaymentAttemptDepIdReturnChk> Checks
        {
            get { return _checks; }
            set { _checks = value; }
        }
    }

    [Serializable()]
    [DefaultMapping(typeof(DPaymentAttemptDepIdReturnChk))]
    public class MPaymentAttemptDepIdReturnChk : Message
    {
        public enum DepositStatusEnum
        {
            NaoConfirmado = 0,
            Confirmado = 1
        }

        public MPaymentAttemptDepIdReturnChk() { }

        private int _paymentAttemptDepIdReturnId;
        [Mapping(DPaymentAttemptDepIdReturnChk.Fields.paymentAttemptDepIdReturnId)]
        public int PaymentAttemptDepIdReturnChk
        {
            get { return _paymentAttemptDepIdReturnId; }
            set { _paymentAttemptDepIdReturnId = value; }
        }

        private int _checkId;
        [Mapping(DPaymentAttemptDepIdReturnChk.Fields.checkId)]
        public int CheckId
        {
            get { return _checkId; }
            set { _checkId = value; }
        }

        private DateTime _data_deposito_cheque;
        [Mapping(DPaymentAttemptDepIdReturnChk.Fields.data_deposito_cheque)]
        [XmlElement("data_deposito_cheque")]
        public DateTime Data_deposito_cheque
        {
            get { return _data_deposito_cheque; }
            set { _data_deposito_cheque = value; }
        }

        private int _ag_acolhedora_cheque;
        [Mapping(DPaymentAttemptDepIdReturnChk.Fields.ag_acolhedora_cheque)]
        [XmlElement("ag_acolhedora_cheque")]
        public int Ag_acolhedora_cheque
        {
            get { return _ag_acolhedora_cheque; }
            set { _ag_acolhedora_cheque = value; }
        }

        private int _dig_acolhedora_cheque;
        [Mapping(DPaymentAttemptDepIdReturnChk.Fields.dig_acolhedora_cheque)]
        [XmlElement("dig_ag_acolhedora_cheque")]
        public int Dig_acolhedora_cheque
        {
            get { return _dig_acolhedora_cheque; }
            set { _dig_acolhedora_cheque = value; }
        }

        private decimal _vlr_deposito_total;
        [Mapping(DPaymentAttemptDepIdReturnChk.Fields.vlr_deposito_total)]
        [XmlElement("vlr_dep_total")]
        public decimal Vlr_deposito_total
        {
            get { return _vlr_deposito_total; }
            set { _vlr_deposito_total = value; }
        }

        private decimal _vlr_cheque;
        [Mapping(DPaymentAttemptDepIdReturnChk.Fields.vlr_cheque)]
        [XmlElement("vlr_cheque")]
        public decimal Vlr_cheque
        {
            get { return _vlr_cheque; }
            set { _vlr_cheque = value; }
        }

        private int _cod_banco;
        [Mapping(DPaymentAttemptDepIdReturnChk.Fields.cod_banco)]
        [XmlElement("cod_banco_compensacao")]
        public int Cod_banco
        {
            get { return _cod_banco; }
            set { _cod_banco = value; }
        }

        private int _cod_agencia_cheque;
        [Mapping(DPaymentAttemptDepIdReturnChk.Fields.cod_agencia_cheque)]
        [XmlElement("cod_agencia_cheque")]
        public int Cod_agencia_cheque
        {
            get { return _cod_agencia_cheque; }
            set { _cod_agencia_cheque = value; }
        }

        private string _numero_cheque;
        [Mapping(DPaymentAttemptDepIdReturnChk.Fields.numero_cheque)]
        [XmlElement("num_cheque")]
        public string Numero_cheque
        {
            get { return _numero_cheque; }
            set { _numero_cheque = value; }
        }

        private int _sequencia_arquivo;
        [Mapping(DPaymentAttemptDepIdReturnChk.Fields.sequencia_arquivo)]
        [XmlElement("numero_sequencial_arquivo")]
        public int Sequencia_arquivo
        {
            get { return _sequencia_arquivo; }
            set { _sequencia_arquivo = value; }
        }

        private DateTime _creationDate;
        [Mapping(DPaymentAttemptDepIdReturnChk.Fields.creationDate)]
        public DateTime CreationDate
        {
            get { return _creationDate; }
            set { _creationDate = value; }
        }

        private int _status;
        [Mapping(DPaymentAttemptDepIdReturnChk.Fields.status)]
        public int Status
        {
            get { return _status; }
            set { _status = value; }
        }
    }
}