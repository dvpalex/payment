using System;
using System.Collections.Generic;
using System.Text;

namespace SuperPag
{
    public enum RecurrenceStatus
    {
        Active = 1,
        Cancelled = 2
    }
    public enum ScheduleStatus
    {
        Scheduled = 1,
        Processed = 2,
        Canceled = 3
    }
    public enum InstallmentType
    {
        Merchant = 1,
        Emissor = 2
    }
    public enum LogFileEntryType
    {
        Error = 1,
        Warning = 2,
        Information = 3
    }
    public enum WorkflowOrderStatus
    {
        HandshakeFinished = 1,
        ConsumerFilled = 2,
        PaymentFormChoosed = 3,
        InstallmentChoosed = 4,
        SpecificInfoDefined = 5,
        AgentCalled = 6,
        Finished = 7,
        Error = 8
    }
    public enum OrderStatus
    {
        Unfinished = 1,
        Analysing = 2,
        Approved = 3,
        Cancelled = 4,
        Transportation = 5,
        Delivered = 6,
        Undelivered = 7,
        NotPaid = 8,
        PendingPaid = 9
    }
    public enum OrderStatusForCombo
    {
        Approved = 3,
        Cancelled = 4
    }
    public enum HandshakeType
    {
        HtmlSPag10 = 1,
        XmlSPag10 = 2,
        XmlService = 3
    }
    public enum ItemTypes
    {
        Regular = 1,
        ShippingRate = 2,
        Discount = 3,
        Extra = 4
    }
    public enum PaymentGroups
    {
        CreditCard = 1,
        BoletoBancario = 2,
        TEF = 3,
        Financiamento = 4,
        DebitCard = 5,
        DepositoIdentificado = 6
    }
    public static class PaymentGroupsWord
    {
        public const string CreditCard = "C";
    }
    public enum AddressTypes
    {
        Billing = 1,
        Delivery = 2
    }
    public enum OrderInstallmentStatus
    {
        Pending = 1,
        Paid = 2,
        NotPaid = 3,
        Canceled = 4,
        PendingPaid = 5
    }
    public enum PaymentAttemptStatus
    {
        Pending = 1,
        Paid = 2,
        NotPaid = 3,
        Canceled = 4,
        PendingPaid = 5,
        Delivered = 6,
    }
    public enum PaymentForms
    {
        VisaVBV = 1,
        MasterKomerciInBox = 2,
        DinersKomerciInBox = 4,
        BBPag = 5,
        BoletoBancoDoBrasil = 6,
        ItauShopLine = 7,
        VisaVBVInBox = 8,
        BoletoBradesco = 9,
        FinanciamentoABN = 10,
        Amex3Party = 11,
        VisaMoset = 12,
        MasterKomerci = 13,
        DinersKomerci = 14,
        Amex2Party = 15,
        BBPagCrediario = 16,
        BoletoSantos = 18,
        Doc = 19,
        ChequeEletronico = 20,
        ValePresente = 21,
        VisaSitef = 22,
        MasterSitef = 23,
        DinersSitef = 24,
        AmexSitef = 25,
        HipercardSitef = 26,
        MasterWebService = 27,
        DinersWebService = 28,
        BoletoItau = 29,
        VisaElectronVBV = 30,
        BoletoHSBC = 34,
        DepositoIdentificadoBradesco = 35,
        VisaMoset3 = 38,
        VisaElectronVBV3 = 39,
        VisaVBV3 = 40,
        PagamentoFacilBradesco = 41,
        BoletoInvestCred=42
        //Reservados (Integração SmartPag: 31, 32, 33, 36, 37)
    }
    public enum PostStatus
    {
        Sent = 2,
        Error = 3,
        Confirmed = 4,
        Canceled = 5
    }
    public enum PaymentAttemptKomerciStatus
    {
        Initial = 1,
        Post = 2,
        Capture = 3,
        End = 4
    }
    public enum PaymentAttemptKomerciWSStatus
    {
        Initial = 1,
        WaitingCapture = 2,
        Captured = 3
    }
    public enum PaymentAttemptVBVStatus
    {
        Initial = 1,
        Mpg = 2,
        WaitingCapture = 3,
        Capture = 4,
        End = 5,
        Expired = 6
    }
    public enum PaymentAttemptVBVInterfaces
    {
        Mpg = 1,
        AutoCapture = 2,
        Sonda = 3,
        CaptureJob = 4,
        Controller = 5
    }
    public enum PaymentAttemptVisaMosetStatus
    {
        Initial = 1,
        Send = 2,
        WaitingCapture = 3,
        Capture = 4,
        End = 5
    }
    public enum PaymentAttemptPaymentClientVirtualStatus
    {
        Initial = 1,
        Send = 2,
        WaitingCapture = 3,
        Capture = 4,
        End = 5
    }
    public enum PaymentAttemptItauShoplineStatus
    {
        Initial = 1,
        Post = 2,
        Capture = 3,
        End = 4
    }
    public enum PaymentAttemptBBPagStatus
    {
        Initial = 1,
        Post = 2,
        Capture = 3,
        End = 4
    }
    public enum PaymentAttemptBradescoStatus
    {
        Initial = 1,
        Post = 2,
        OrderNotification = 3,
        Confirmation = 4,
        End = 5
    }
    public enum PaymentAttemptABNStatus
    {
        Initial = 1,
        Post = 2,
        End = 3
    }
    public enum PaymentAgents
    {
        VBV = 1,
        KomerciInBox = 2,
        Boleto = 3,
        ItauShopLine = 4,
        BBPag = 5,
        VBVInBox = 6,
        FinanciamentoABN = 7,
        PaymentClientVirtual3Party = 8,
        VisaMoset = 9,
        Komerci = 10,
        PaymentClientVirtual2Party = 11,
        Bradesco = 12,
        KomerciWS = 13,
        DepositoIdentificado = 14,
        VBV3 = 15,
        VisaMoset3 = 16,
        DebitoContaCorrente = 17
    }
    public enum PostTypes
    {
        Finalization = 1,
        Payment = 2
    }
    public enum EmailTypes
    {
        ConsumerFinalization = 1,
        ConsumerPayment = 2,
        StoreFinalization = 3,
        StorePayment = 4,
        FinazationPostContingency = 5,
        PaymentPostContingency = 6
    }
    public enum Languages
    {
        PortuguesBrasil = 1,
        InglesEUA = 2,
        Espanhol = 3
    }
    public class CreditCardInformation
    {
        public string Name = "";
        public string Number = "";
        public string SecurityNumber = "";
        public DateTime ExpirationDate = DateTime.MinValue;
    }
    public enum ContaCorrenteStatus
    {
        ErroLeituraArquivo=1,
        ErroValidacaoLayout=2,
        LoteDuplicado=3,
        ErroProcessamentoSuperpag=4,
        EnviadoSuperpag=5,
        EnviadoBanco=6,
        EnviadoCSU=7,
        AguardandoPagamento=8,
        Pago=9,
        NaoPago=10
    }
    public class RedirectInformation
    {
        public string Return = "";
        public string Redirect = "";
        public string UrlRedirect = "";
    }
   

    public enum DepIdStatusEnum
    {
        ReturnNotFound = 0,
        AttemptNotFound = 1,
        PaymentValueOk = 2,
        LesserPaymentValue = 3,
        BiggerPaymentValue = 4
    }
    public enum BankNumber
    {
        Tecban = 0,
        Bradesco = 237,
        Itaú = 341,
        BancoDoBrasil = 1,
        Banrisul = 41,
        ItaúBank = 479,
        Unibanco = 409,
        Real = 356,
        SantanderBanespa = 8,
        NossaCaixa = 151,
        HSBC = 399,
        Citibank = 745,
        Safra = 422,
        MercantilDoBrasil = 389,
        BancoSantander = 351 ,
        Banespa = 33
    }
    // Códigos utilizados pelo SmartPag
    public static class SmartPagLegacy
    {
        // Códigos cadastrados na tabela OP_CARTAO_DE_CREDITO no campo INT_CODIGO_OP_CARTAO
        public enum CodigoOpCartao
        {
            CartãoVirtual = 0,
            AmericanExpress = 1,
            MasterCard = 2,
            Sollo = 3,
            Visa = 4,
            DinersClub = 5,
            VisaOnLine = 23,
            MasterCardOnLine = 41,
            DinersClubOnLine = 44,
            AmericanExpressOnLine = 50,
            VisaSitef = 123,
            MasterCardSitef = 141,
            DinersClubSitef = 144,
            AmericanExpressSitef = 150
        }

        // Códigos cadastrados na tabela MEIO_PAGAMENTO no campo INT_CODIGO_MEIO_PAGAMENTO
        public enum CodigoMeioPagamento
        {
            BoletoBancario = 1, //Banco Santos
            CartaoDeCredito = 2,
            Especie = 3,
            DepositoIdentificado = 4,
            Cheque = 5,
            Doc = 6,
            FinanciamentoChequePre = 7,
            ChequeEletrônicoCom = 9,
            FinanciamentoPanAmericano = 10,
            FinanciamentoFinasa = 12,
            DebitoEmConta = 13,
            PagamentoFacilBradesco = 14,
            BoletoBradescoSPS = 15,
            ItauShopLine = 18,
            BoletoItau = 19,
            F2B = 20,
            BBPag = 21,
            FinanciamentoABNAMROBank = 23,
            ValePresente = 24,
            BoletoBancarioFisico = 27,
            FinanciamentoVirtualCred = 31,
            BoletoBradesco = 32,
            BoletoBancoDoBrasil = 33,
            BoletoBradescoFisico = 36,
            BBPagCrediario = 37
        }

        public enum StatusPedido
        {
            NaoConcluido = 0,
            Recebido = 1,
            EmAnalise = 2,
            AceitoOuEmProcessamento = 3,
            Cancelado = 4,
            EmTransito = 5,
            Entregue = 6,
            NaoEntregue = 7,
            RetirarCheque = 8,
            ChequesRetirados = 9,
            ChequesNaoRetirados = 10,
            DevolverMercadoria = 11
        }        
        public enum StatusCobranca
        {
            NaoConcluido = 0,
            NaoPago = 1,
            Pago = 2,
            ARestituir = 3,
            Restituido = 4
        }
    }

    #region Código dos Meios de Pagamento no Smartpag
    //INT_CODIGO_OP_CARTAO
    //0     Cartão Virtual
    //1	    American Express
    //2	    MasterCard
    //3	    Sollo
    //4	    Visa
    //5	    Diners Club
    //23	Visa (On Line)
    //41	MasterCard (On Line)
    //44	Diners Club (On Line)
    //50	American Express (On Line)
    //123	Visa (Sitef)
    //141	MasterCard (Sitef)
    //144	Diners Club (Sitef)
    //150	American Express (Sitef)

    //INT_CODIGO_MEIO_PAGAMENTO
    //1	    Boleto Bancário //Banco Santos
    //2	    Cartão de Crédito
    //3	    Espécie
    //4	    Depósito Identificado
    //5	    Cheque
    //6	    Doc
    //7	    Financiamento Cheque-Pré
    //9	    Cheque Eletrônico.com
    //10	Financiamento PanAmericano
    //12	Financ. Finasa
    //13	Débito em Conta
    //14	Pagamento Fácil Bradesco
    //15	Boleto Bradesco (SPS)
    //18	Itaú ShopLine
    //19	Boleto Itaú
    //20	F2B
    //21	BB PAG
    //23	Financ. ABN-AMRO Bank
    //24	Vale Presente
    //27	Boleto Bancário Físico
    //31	Financiamento VirtualCred
    //32	Boleto Bancário //Bradesco
    //33	Boleto Banco do Brasil
    //36	Boleto Bradesco Fisico

    //INT_CODIGO_STATUS_PEDIDO tabela STATUS_PEDIDO
    //0	    Não Concluído
    //1	    Recebido
    //2	    Em Análise
    //3	    Aceito ou em proc.
    //4	    Cancelado
    //5	    Em Trânsito
    //6	    Entregue
    //7	    Não Entregue
    //8	    Retirar Cheque
    //9	    Cheques Retirados
    //10	Cheques Não Retirados
    //11	Devolver Mercadoria

    //INT_CODIGO_COBRANCA tabela STATUS_COBRANCA
    //1	    Não Pago
    //2	    Pago
    //3	    A Restituir
    //4	    Restituido

    #endregion

    #region Enum Tela de pagamento

    public enum MultiViewGeral
    {
        Entrada =0,
        Confirmacao=1
    }
    public enum MultiViewPagForm
    {
        FormaPagamentoIPTE =0
    }
    public enum MultiViewEnvio
    {
        Email=0,
        Fax=1,
        Correio=2
    }
    public enum MultiViewPagFinal
    {
        FinalEmail=0,
        FinalFax=1,
        FinalCorreio=2,
        FinalImpressao=3
    }

    # endregion
}

