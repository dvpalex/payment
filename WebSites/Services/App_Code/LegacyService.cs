using System;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using SuperPag.Data.Messages;
using SuperPag.Data;
using SuperPag.Helper;
using EasyPag.SuperPagWS.Legacy;
using System.Text;
using SuperPag;
using SuperPag.Handshake;
using System.Xml.Serialization;
using System.IO;
using System.Xml;

[WebService(Namespace = "http://www.superpag.com.br/Services")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
public class LegacyService : System.Web.Services.WebService
{
    public LegacyService()
    {
        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod(Description = "Retorna as informações do pedido a partir da chave/pedido")]
    public string ConsultaPedido(string chave, string senha, string pedido)
    {
        try
        {
            GenericHelper.LogFile("SuperPagWS::LegacyService.cs::ConsultaPedido Consulta do pedido " + pedido + " recebido para a loja " + chave, LogFileEntryType.Information);
            
            Ensure.IsNotNullOrEmpty(chave, "Chave não informada");
            Ensure.IsNotNullOrEmpty(pedido, "Código de pedido inválido");

            DStore store = DataFactory.Store().Locate(chave);
            Ensure.IsNotNull(store, "A chave {0} é inválida", chave);

            if (Ensure.IsNotNullOrEmpty(store.password) && !Ensure.IsNotNullOrEmpty(senha))
                Ensure.IsNotNullOrEmpty(null, "Senha não informada");
            
            if (Ensure.IsNotNullOrEmpty(store.password) && store.password != GenericHelper.CalculateHash(senha))
                Ensure.IsNotNull(null, "A senha para a chave {0} é inválida", chave);

            DOrder[] orders = DataFactory.Order().List(store.storeId, pedido);
            Ensure.IsNotNull(orders, "<PEDIDO/>", pedido);

            StringBuilder xml = new StringBuilder();

            foreach (DOrder order in orders)
            {
                DPaymentAttemptComplete[] arrPaymentAttempt = DataFactory.PaymentAttempt().ListComplete(order.orderId);
                if (Ensure.IsNull(arrPaymentAttempt))
                    continue;

                string ger = "", countryDelivery = "", countryBilling = "";

                PEDIDO p = new PEDIDO();
                p.CLIENTES_INTERNET = new PEDIDOCLIENTES_INTERNET();
                p.ENDERECO_ENTREGA_PEDIDO = new PEDIDOENDERECO_ENTREGA_PEDIDO();

                DConsumer consumer = DataFactory.Consumer().Locate(order.consumerId);
                if (Ensure.IsNotNull(consumer))
                {
                    PEDIDOCLIENTES_INTERNET cliente = new PEDIDOCLIENTES_INTERNET();

                    #region Dados do cliente
                    cliente.STR_CPF = (String.IsNullOrEmpty(consumer.CPF) ? (ulong)GenericHelper.Parse(consumer.CNPJ, typeof(ulong)) : (ulong)GenericHelper.Parse(consumer.CPF, typeof(ulong)));
                    cliente.INT_NUMERO_PEDIDO = order.orderId;
                    cliente.STR_NOME_CLIENTE = (String.IsNullOrEmpty(consumer.CPF) ? GenericHelper.GetCompanyName(order.orderId) : consumer.name);
                    cliente.DAT_NASCIMENTO = (consumer.birthDate == DateTime.MinValue ? "1900-01-01T00:00:00" : consumer.birthDate.ToString("s"));
                    cliente.STR_SEXO = (String.IsNullOrEmpty(consumer.ger) ? "M" : consumer.ger);
                    cliente.STR_CELULAR = consumer.celularPhone;
                    cliente.STR_CPF_RESPONSAVEL = (String.IsNullOrEmpty(consumer.responsibleCPF) ? " " : consumer.responsibleCPF);
                    cliente.STR_EMAIL = consumer.email;
                    cliente.STR_FAX = consumer.fax;
                    cliente.STR_GERA_CAPITALIZACAO = "N"; //está fixo para a IBM
                    cliente.STR_INSCRICAO = consumer.IE;
                    cliente.STR_NOME_RESPONSAVEL = consumer.responsibleName;
                    cliente.STR_ESTADO_CIVIL = (!String.IsNullOrEmpty(consumer.CPF) && String.IsNullOrEmpty(consumer.civilState) ? "C" : consumer.civilState);
                    cliente.STR_PROFISSAO = (!String.IsNullOrEmpty(consumer.CPF) && String.IsNullOrEmpty(consumer.occupation) ? "OUTROS" : consumer.occupation);
                    cliente.STR_RECEBER_EMAIL = "S"; //está fixo para a IBM
                    cliente.STR_TELEFONE = (ulong)GenericHelper.Parse(consumer.phone, typeof(ulong));
                    ger = consumer.ger;
                    #endregion

                    DConsumerAddress consumerAddressBilling = DataFactory.ConsumerAddress().Locate(consumer.consumerId, (int)AddressTypes.Billing);
                    if (Ensure.IsNotNull(consumerAddressBilling))
                    {
                        #region Endereço de cobrança
                        cliente.STR_BAIRRO = consumerAddressBilling.district;
                        cliente.STR_CEP = (uint)GenericHelper.Parse(consumerAddressBilling.cep, typeof(uint));
                        cliente.STR_CIDADE = consumerAddressBilling.city;
                        cliente.STR_COMPLEMENTO_ENDERECO = consumerAddressBilling.addressComplement;
                        cliente.STR_ENDERECO = consumerAddressBilling.address;
                        cliente.STR_ESTADO = consumerAddressBilling.state.Trim();
                        cliente.STR_LOGRADOURO = consumerAddressBilling.logradouro;
                        cliente.STR_NUMERO_ENDERECO = consumerAddressBilling.addressNumber;
                        cliente.STR_PAIS = consumerAddressBilling.country;
                        countryBilling = consumerAddressBilling.country;
                        #endregion
                    }

                    p.CLIENTES_INTERNET = cliente;

                    DConsumerAddress consumerAddressDelivery = DataFactory.ConsumerAddress().Locate(consumer.consumerId, (int)AddressTypes.Delivery);
                    if (Ensure.IsNotNull(consumerAddressDelivery))
                    {
                        PEDIDOENDERECO_ENTREGA_PEDIDO enderecoEntrega = new PEDIDOENDERECO_ENTREGA_PEDIDO();

                        #region Endereço de entrega
                        enderecoEntrega.DAT_ATUALIZACAO = order.creationDate;
                        enderecoEntrega.INT_CODIGO_USUARIO = 3; //está fixo para a IBM
                        enderecoEntrega.INT_NUMERO_PEDIDO = order.orderId;
                        enderecoEntrega.STR_BAIRRO_ENTREGA_PEDIDO = consumerAddressDelivery.district;
                        enderecoEntrega.STR_CEP_ENTREGA_PEDIDO = (uint)GenericHelper.Parse(consumerAddressDelivery.cep, typeof(uint));
                        enderecoEntrega.STR_CIDADE_ENTREGA_PEDIDO = consumerAddressDelivery.city;
                        enderecoEntrega.STR_COMPLEMENTO_ENDERECO_ENTREGA_PEDIDO = consumerAddressDelivery.addressComplement;
                        enderecoEntrega.STR_CPF = (String.IsNullOrEmpty(consumer.CPF) ? (ulong)GenericHelper.Parse(consumer.CNPJ, typeof(ulong)) : (ulong)GenericHelper.Parse(consumer.CPF, typeof(ulong)));
                        enderecoEntrega.STR_ENDERECO_ENTREGA_PEDIDO = consumerAddressDelivery.address;
                        enderecoEntrega.STR_ESTADO_ENTREGA_PEDIDO = consumerAddressDelivery.state.Trim();
                        enderecoEntrega.STR_LOGRADOURO_ENTREGA_PEDIDO = consumerAddressDelivery.logradouro;
                        enderecoEntrega.STR_NUMERO_ENDERECO_ENTREGA_PEDIDO = consumerAddressDelivery.addressNumber;
                        enderecoEntrega.STR_PAIS_ENTREGA_PEDIDO = consumerAddressDelivery.country;
                        enderecoEntrega.STR_RESPONSAVEL_PEDIDO = (String.IsNullOrEmpty(consumer.CPF) ? GenericHelper.GetCompanyName(order.orderId) : consumer.name);
                        enderecoEntrega.STR_TELEFONE_PEDIDO = (ulong)GenericHelper.Parse(consumer.phone, typeof(ulong));
                        countryDelivery = consumerAddressDelivery.country;
                        #endregion

                        p.ENDERECO_ENTREGA_PEDIDO = enderecoEntrega;
                    }
                }

                PEDIDOCLIENTES_INTERNET_PEDIDO clientePedido = new PEDIDOCLIENTES_INTERNET_PEDIDO();

                #region Dados genéricos do pedido
                clientePedido.DAT_ENTRADA_PEDIDO = order.creationDate.ToString("dd/MM/yyyy HH:mm:ss");
                clientePedido.INT_NUMERO_PEDIDO = order.orderId;
                clientePedido.STR_FLAG_PEDIDO = Helper.GetSmartPagOrderStatus(order.status);
                clientePedido.NUM_VALOR_FINAL_PEDIDO = (order.finalAmount == decimal.MinValue ? 0 : order.finalAmount);

                //já setar valores default ou não exibir valor algum para os dados a seguir,
                //específicos do meio de pagamento, prevendo os casos em que o pedido
                //não possui tentativa de pagamento
                clientePedido.INT_CODIGO_MEIO_PAGAMENTO = 0;
                clientePedido.INT_NUMERO_PARCELAS_PEDIDO = 0;
                clientePedido.MEIO_PAGTO = new PEDIDOCLIENTES_INTERNET_PEDIDOMEIO_PAGTO();
                #endregion

                #region Dados específicos do meio de pagamento
                if (Ensure.IsNotNull(arrPaymentAttempt))
                {
                    DPaymentAttempt paymentAttempt = GenericHelper.ChooseAttemptByStatus((DPaymentAttempt[])arrPaymentAttempt);

                    clientePedido.INT_CODIGO_MEIO_PAGAMENTO = (byte)Helper.GetSmartPagPaymentForm(paymentAttempt.paymentFormId);
                    clientePedido.INT_NUMERO_PARCELAS_PEDIDO = (byte)order.installmentQuantity;

                    DOrderInstallment[] installments;

                    switch ((PaymentForms)paymentAttempt.paymentFormId)
                    {
                        case PaymentForms.FinanciamentoABN:
                            #region Valores default dos campos do consumidor diferentes para o ABN
                            p.CLIENTES_INTERNET.STR_SEXO = (String.IsNullOrEmpty(ger.Trim()) ? " " : ger);
                            //p.CLIENTES_INTERNET.STR_PROFISSAO = (!String.IsNullOrEmpty(consumer.CPF) && String.IsNullOrEmpty(consumer.occupation) ? " " : consumer.occupation);
                            p.CLIENTES_INTERNET.STR_PROFISSAO = consumer.occupation;
                            p.CLIENTES_INTERNET.STR_PAIS = (String.IsNullOrEmpty(countryBilling) ? " " : countryBilling);
                            p.ENDERECO_ENTREGA_PEDIDO.STR_PAIS_ENTREGA_PEDIDO = (String.IsNullOrEmpty(countryDelivery) ? " " : countryDelivery);
                            clientePedido.STR_FLAG_PEDIDO = (int)(paymentAttempt.status == (int)PaymentAttemptStatus.Paid || paymentAttempt.status == (int)PaymentAttemptStatus.PendingPaid ? SmartPagLegacy.StatusPedido.EmAnalise : SmartPagLegacy.StatusPedido.NaoConcluido);
                            #endregion

                            clientePedido.MEIO_PAGTO.Item = new PEDIDOCLIENTES_INTERNET_PEDIDOMEIO_PAGTOMP_FINANCIAMENTO[1];
                            clientePedido.MEIO_PAGTO.Item[0] = new PEDIDOCLIENTES_INTERNET_PEDIDOMEIO_PAGTOMP_FINANCIAMENTO();
                            ((PEDIDOCLIENTES_INTERNET_PEDIDOMEIO_PAGTOMP_FINANCIAMENTO)clientePedido.MEIO_PAGTO.Item[0]).CONSULTA_ABN = new PEDIDOCLIENTES_INTERNET_PEDIDOMEIO_PAGTOMP_FINANCIAMENTOCONSULTA_ABN();

                            DPaymentAttemptABN paymentAttemptABN = DataFactory.PaymentAttemptABN().Locate(paymentAttempt.paymentAttemptId);
                            if (Ensure.IsNotNull(paymentAttemptABN))
                            {
                                #region Informações Fianciamento ABN
                                ((PEDIDOCLIENTES_INTERNET_PEDIDOMEIO_PAGTOMP_FINANCIAMENTO)clientePedido.MEIO_PAGTO.Item[0]).CONSULTA_ABN.STR_STATUS_CONTRATO = paymentAttemptABN.statusProposta;
                                ((PEDIDOCLIENTES_INTERNET_PEDIDOMEIO_PAGTOMP_FINANCIAMENTO)clientePedido.MEIO_PAGTO.Item[0]).INT_CONTROLE_ABN = (uint)(paymentAttemptABN.numControle == decimal.MinValue ? 0 : paymentAttemptABN.numControle);
                                ((PEDIDOCLIENTES_INTERNET_PEDIDOMEIO_PAGTOMP_FINANCIAMENTO)clientePedido.MEIO_PAGTO.Item[0]).STR_NUMERO_CONTRATO = (ulong)GenericHelper.Parse(paymentAttemptABN.numProposta, typeof(ulong));
                                #endregion
                            }
                            
                            break;
                        case PaymentForms.Amex2Party:
                        case PaymentForms.Amex3Party:
                            DPaymentAttemptPaymentClientVirtual paymentClient = DataFactory.PaymentAttemptPaymentClientVirtual().Locate(paymentAttempt.paymentAttemptId);
                            installments = DataFactory.OrderInstallment().List(paymentAttempt.orderId);
           
                            if (Ensure.IsNotNull(paymentClient) && Ensure.IsNotNull(installments))
                            {
                                #region Informações Amex
                                clientePedido.MEIO_PAGTO.Item = new PEDIDOCLIENTES_INTERNET_PEDIDOMEIO_PAGTOMP_CARTAO_CREDITO[1];
                                clientePedido.MEIO_PAGTO.Item[0] = new PEDIDOCLIENTES_INTERNET_PEDIDOMEIO_PAGTOMP_CARTAO_CREDITO();
                                ((PEDIDOCLIENTES_INTERNET_PEDIDOMEIO_PAGTOMP_CARTAO_CREDITO)clientePedido.MEIO_PAGTO.Item[0]).DAT_ATUALIZACAO = paymentAttempt.lastUpdate.ToString("dd/MM/yyyy hh:mm:ss");
                                ((PEDIDOCLIENTES_INTERNET_PEDIDOMEIO_PAGTOMP_CARTAO_CREDITO)clientePedido.MEIO_PAGTO.Item[0]).INT_CODIGO_OP_CARTAO = Helper.GetSmartPagCreditCardFlag(paymentAttempt.paymentFormId);
                                ((PEDIDOCLIENTES_INTERNET_PEDIDOMEIO_PAGTOMP_CARTAO_CREDITO)clientePedido.MEIO_PAGTO.Item[0]).INT_NUMERO_PARCELA_MP = installments[0].installmentNumber;
                                ((PEDIDOCLIENTES_INTERNET_PEDIDOMEIO_PAGTOMP_CARTAO_CREDITO)clientePedido.MEIO_PAGTO.Item[0]).INT_NUMERO_PEDIDO = paymentAttempt.orderId;
                                ((PEDIDOCLIENTES_INTERNET_PEDIDOMEIO_PAGTOMP_CARTAO_CREDITO)clientePedido.MEIO_PAGTO.Item[0]).NUM_VALOR_PARCELA_PEDIDO = installments[0].installmentValue;
                                ((PEDIDOCLIENTES_INTERNET_PEDIDOMEIO_PAGTOMP_CARTAO_CREDITO)clientePedido.MEIO_PAGTO.Item[0]).STR_FLAG_COBRANCA = Helper.GetSmartPagBillingStatus(paymentAttempt.status);
                                ((PEDIDOCLIENTES_INTERNET_PEDIDOMEIO_PAGTOMP_CARTAO_CREDITO)clientePedido.MEIO_PAGTO.Item[0]).STR_NOME_CLIENTE_CARTAO = "";
                                ((PEDIDOCLIENTES_INTERNET_PEDIDOMEIO_PAGTOMP_CARTAO_CREDITO)clientePedido.MEIO_PAGTO.Item[0]).STR_NUMERO_CARTAO_MP = "";
                                ((PEDIDOCLIENTES_INTERNET_PEDIDOMEIO_PAGTOMP_CARTAO_CREDITO)clientePedido.MEIO_PAGTO.Item[0]).STR_SAFENET_NUMAUTENT = "";
                                ((PEDIDOCLIENTES_INTERNET_PEDIDOMEIO_PAGTOMP_CARTAO_CREDITO)clientePedido.MEIO_PAGTO.Item[0]).STR_SAFENET_NUMAUTOR = "";
                                ((PEDIDOCLIENTES_INTERNET_PEDIDOMEIO_PAGTOMP_CARTAO_CREDITO)clientePedido.MEIO_PAGTO.Item[0]).STR_SAFENET_NUMCV = "";
                                ((PEDIDOCLIENTES_INTERNET_PEDIDOMEIO_PAGTOMP_CARTAO_CREDITO)clientePedido.MEIO_PAGTO.Item[0]).STR_VENCIMENTO_CARTAO_MP = "/";
                                ((PEDIDOCLIENTES_INTERNET_PEDIDOMEIO_PAGTOMP_CARTAO_CREDITO)clientePedido.MEIO_PAGTO.Item[0]).STR_VN_Cod_Autoriza = paymentClient.vpc_AuthorizeId;
                                ((PEDIDOCLIENTES_INTERNET_PEDIDOMEIO_PAGTOMP_CARTAO_CREDITO)clientePedido.MEIO_PAGTO.Item[0]).STR_VN_ID_Pedido = "";
                                #endregion
                            }
                            else
                            {
                                clientePedido.MEIO_PAGTO.Item = new PEDIDOCLIENTES_INTERNET_PEDIDOMEIO_PAGTOMP_CARTAO_CREDITO[1];
                                clientePedido.MEIO_PAGTO.Item[0] = new PEDIDOCLIENTES_INTERNET_PEDIDOMEIO_PAGTOMP_CARTAO_CREDITO();
                            }

                            break;
                        case PaymentForms.DinersKomerci:
                        case PaymentForms.MasterKomerci:
                        case PaymentForms.DinersKomerciInBox:
                        case PaymentForms.MasterKomerciInBox:
                            DPaymentAttemptKomerci paymentAttemptKomerci = DataFactory.PaymentAttemptKomerci().Locate(paymentAttempt.paymentAttemptId);
                            installments = DataFactory.OrderInstallment().List(paymentAttempt.orderId);
                            
                            if (Ensure.IsNotNull(paymentAttemptKomerci) && Ensure.IsNotNull(installments))
                            {
                                #region Informações Komerci
                                clientePedido.MEIO_PAGTO.Item = new PEDIDOCLIENTES_INTERNET_PEDIDOMEIO_PAGTOMP_CARTAO_CREDITO[1];
                                clientePedido.MEIO_PAGTO.Item[0] = new PEDIDOCLIENTES_INTERNET_PEDIDOMEIO_PAGTOMP_CARTAO_CREDITO();
                                ((PEDIDOCLIENTES_INTERNET_PEDIDOMEIO_PAGTOMP_CARTAO_CREDITO)clientePedido.MEIO_PAGTO.Item[0]).DAT_ATUALIZACAO = paymentAttempt.lastUpdate.ToString("dd/MM/yyyy hh:mm:ss");
                                ((PEDIDOCLIENTES_INTERNET_PEDIDOMEIO_PAGTOMP_CARTAO_CREDITO)clientePedido.MEIO_PAGTO.Item[0]).INT_CODIGO_OP_CARTAO = Helper.GetSmartPagCreditCardFlag(paymentAttempt.paymentFormId);
                                ((PEDIDOCLIENTES_INTERNET_PEDIDOMEIO_PAGTOMP_CARTAO_CREDITO)clientePedido.MEIO_PAGTO.Item[0]).INT_NUMERO_PARCELA_MP = installments[0].installmentNumber;
                                ((PEDIDOCLIENTES_INTERNET_PEDIDOMEIO_PAGTOMP_CARTAO_CREDITO)clientePedido.MEIO_PAGTO.Item[0]).INT_NUMERO_PEDIDO = paymentAttempt.orderId;
                                ((PEDIDOCLIENTES_INTERNET_PEDIDOMEIO_PAGTOMP_CARTAO_CREDITO)clientePedido.MEIO_PAGTO.Item[0]).NUM_VALOR_PARCELA_PEDIDO = installments[0].installmentValue;
                                ((PEDIDOCLIENTES_INTERNET_PEDIDOMEIO_PAGTOMP_CARTAO_CREDITO)clientePedido.MEIO_PAGTO.Item[0]).STR_FLAG_COBRANCA = Helper.GetSmartPagBillingStatus(paymentAttempt.status);
                                ((PEDIDOCLIENTES_INTERNET_PEDIDOMEIO_PAGTOMP_CARTAO_CREDITO)clientePedido.MEIO_PAGTO.Item[0]).STR_NOME_CLIENTE_CARTAO = "";
                                ((PEDIDOCLIENTES_INTERNET_PEDIDOMEIO_PAGTOMP_CARTAO_CREDITO)clientePedido.MEIO_PAGTO.Item[0]).STR_NUMERO_CARTAO_MP = paymentAttemptKomerci.nr_cartao;
                                ((PEDIDOCLIENTES_INTERNET_PEDIDOMEIO_PAGTOMP_CARTAO_CREDITO)clientePedido.MEIO_PAGTO.Item[0]).STR_SAFENET_NUMAUTENT = paymentAttemptKomerci.numautent;
                                ((PEDIDOCLIENTES_INTERNET_PEDIDOMEIO_PAGTOMP_CARTAO_CREDITO)clientePedido.MEIO_PAGTO.Item[0]).STR_SAFENET_NUMAUTOR = paymentAttemptKomerci.numautor;
                                ((PEDIDOCLIENTES_INTERNET_PEDIDOMEIO_PAGTOMP_CARTAO_CREDITO)clientePedido.MEIO_PAGTO.Item[0]).STR_SAFENET_NUMCV = paymentAttemptKomerci.numcv;
                                ((PEDIDOCLIENTES_INTERNET_PEDIDOMEIO_PAGTOMP_CARTAO_CREDITO)clientePedido.MEIO_PAGTO.Item[0]).STR_VENCIMENTO_CARTAO_MP = (String.IsNullOrEmpty(paymentAttemptKomerci.data) ? "/" : paymentAttemptKomerci.data);
                                ((PEDIDOCLIENTES_INTERNET_PEDIDOMEIO_PAGTOMP_CARTAO_CREDITO)clientePedido.MEIO_PAGTO.Item[0]).STR_VN_Cod_Autoriza = 0;
                                ((PEDIDOCLIENTES_INTERNET_PEDIDOMEIO_PAGTOMP_CARTAO_CREDITO)clientePedido.MEIO_PAGTO.Item[0]).STR_VN_ID_Pedido = "";
                                #endregion
                            }
                            else
                            {
                                clientePedido.MEIO_PAGTO.Item = new PEDIDOCLIENTES_INTERNET_PEDIDOMEIO_PAGTOMP_CARTAO_CREDITO[1];
                                clientePedido.MEIO_PAGTO.Item[0] = new PEDIDOCLIENTES_INTERNET_PEDIDOMEIO_PAGTOMP_CARTAO_CREDITO();
                            }

                            break;
                        case PaymentForms.DinersWebService:
                        case PaymentForms.MasterWebService:
                            //TODO: pegar os valores corretos na tabela PaymentAttemptKomerciWS
                            clientePedido.MEIO_PAGTO.Item = new PEDIDOCLIENTES_INTERNET_PEDIDOMEIO_PAGTOMP_FINANCIAMENTO[1];
                            clientePedido.MEIO_PAGTO.Item[0] = new PEDIDOCLIENTES_INTERNET_PEDIDOMEIO_PAGTOMP_FINANCIAMENTO();
                            break;
                        case PaymentForms.VisaSitef:
                            //TODO: pegar os valores corretos na tabela PaymentAttemptSitef ou PaymentAttemptVisaSitef
                            clientePedido.MEIO_PAGTO.Item = new PEDIDOCLIENTES_INTERNET_PEDIDOMEIO_PAGTOMP_FINANCIAMENTO[1];
                            clientePedido.MEIO_PAGTO.Item[0] = new PEDIDOCLIENTES_INTERNET_PEDIDOMEIO_PAGTOMP_FINANCIAMENTO();
                            break;
                        case PaymentForms.VisaMoset:
                        case PaymentForms.VisaMoset3:
                            //TODO: pegar os valores corretos na tabela PaymentAttemptMoset
                            clientePedido.MEIO_PAGTO.Item = new PEDIDOCLIENTES_INTERNET_PEDIDOMEIO_PAGTOMP_FINANCIAMENTO[1];
                            clientePedido.MEIO_PAGTO.Item[0] = new PEDIDOCLIENTES_INTERNET_PEDIDOMEIO_PAGTOMP_FINANCIAMENTO();
                            break;
                        case PaymentForms.VisaVBV:
                        case PaymentForms.VisaVBVInBox:
                        case PaymentForms.VisaVBV3:
                            DPaymentAttemptVBV paymentAttemptVBV = DataFactory.PaymentAttemptVBV().Locate(paymentAttempt.paymentAttemptId);
                            installments = DataFactory.OrderInstallment().List(paymentAttempt.orderId);

                            if (Ensure.IsNotNull(paymentAttemptVBV) && Ensure.IsNotNull(installments))
                            {
                                #region Informações VBV
                                clientePedido.MEIO_PAGTO.Item = new PEDIDOCLIENTES_INTERNET_PEDIDOMEIO_PAGTOMP_CARTAO_CREDITO[installments.Length];
                                clientePedido.MEIO_PAGTO.Item[0] = new PEDIDOCLIENTES_INTERNET_PEDIDOMEIO_PAGTOMP_CARTAO_CREDITO();
                                ((PEDIDOCLIENTES_INTERNET_PEDIDOMEIO_PAGTOMP_CARTAO_CREDITO)clientePedido.MEIO_PAGTO.Item[0]).DAT_ATUALIZACAO = paymentAttempt.lastUpdate.ToString("dd/MM/yyyy hh:mm:ss");
                                ((PEDIDOCLIENTES_INTERNET_PEDIDOMEIO_PAGTOMP_CARTAO_CREDITO)clientePedido.MEIO_PAGTO.Item[0]).INT_CODIGO_OP_CARTAO = Helper.GetSmartPagCreditCardFlag(paymentAttempt.paymentFormId);
                                ((PEDIDOCLIENTES_INTERNET_PEDIDOMEIO_PAGTOMP_CARTAO_CREDITO)clientePedido.MEIO_PAGTO.Item[0]).INT_NUMERO_PARCELA_MP = installments[0].installmentNumber;
                                ((PEDIDOCLIENTES_INTERNET_PEDIDOMEIO_PAGTOMP_CARTAO_CREDITO)clientePedido.MEIO_PAGTO.Item[0]).INT_NUMERO_PEDIDO = paymentAttempt.orderId;
                                ((PEDIDOCLIENTES_INTERNET_PEDIDOMEIO_PAGTOMP_CARTAO_CREDITO)clientePedido.MEIO_PAGTO.Item[0]).NUM_VALOR_PARCELA_PEDIDO = installments[0].installmentValue;
                                ((PEDIDOCLIENTES_INTERNET_PEDIDOMEIO_PAGTOMP_CARTAO_CREDITO)clientePedido.MEIO_PAGTO.Item[0]).STR_FLAG_COBRANCA = Helper.GetSmartPagBillingStatus(paymentAttempt.status);
                                ((PEDIDOCLIENTES_INTERNET_PEDIDOMEIO_PAGTOMP_CARTAO_CREDITO)clientePedido.MEIO_PAGTO.Item[0]).STR_NOME_CLIENTE_CARTAO = "";
                                ((PEDIDOCLIENTES_INTERNET_PEDIDOMEIO_PAGTOMP_CARTAO_CREDITO)clientePedido.MEIO_PAGTO.Item[0]).STR_NUMERO_CARTAO_MP = "";
                                ((PEDIDOCLIENTES_INTERNET_PEDIDOMEIO_PAGTOMP_CARTAO_CREDITO)clientePedido.MEIO_PAGTO.Item[0]).STR_SAFENET_NUMAUTENT = "";
                                ((PEDIDOCLIENTES_INTERNET_PEDIDOMEIO_PAGTOMP_CARTAO_CREDITO)clientePedido.MEIO_PAGTO.Item[0]).STR_SAFENET_NUMAUTOR = "";
                                ((PEDIDOCLIENTES_INTERNET_PEDIDOMEIO_PAGTOMP_CARTAO_CREDITO)clientePedido.MEIO_PAGTO.Item[0]).STR_SAFENET_NUMCV = "";
                                ((PEDIDOCLIENTES_INTERNET_PEDIDOMEIO_PAGTOMP_CARTAO_CREDITO)clientePedido.MEIO_PAGTO.Item[0]).STR_VENCIMENTO_CARTAO_MP = "/";
                                ((PEDIDOCLIENTES_INTERNET_PEDIDOMEIO_PAGTOMP_CARTAO_CREDITO)clientePedido.MEIO_PAGTO.Item[0]).STR_VN_Cod_Autoriza = paymentAttemptVBV.arp;
                                ((PEDIDOCLIENTES_INTERNET_PEDIDOMEIO_PAGTOMP_CARTAO_CREDITO)clientePedido.MEIO_PAGTO.Item[0]).STR_VN_ID_Pedido = paymentAttemptVBV.tid;
                                #endregion
                            }
                            else
                            {
                                clientePedido.MEIO_PAGTO.Item = new PEDIDOCLIENTES_INTERNET_PEDIDOMEIO_PAGTOMP_CARTAO_CREDITO[1];
                                clientePedido.MEIO_PAGTO.Item[0] = new PEDIDOCLIENTES_INTERNET_PEDIDOMEIO_PAGTOMP_CARTAO_CREDITO();
                            }
                            
                            break;
                    }
                }
                #endregion

                p.CLIENTES_INTERNET_PEDIDO = clientePedido;

                XmlSerializerNamespaces nmspc = new XmlSerializerNamespaces();
                nmspc.Add(String.Empty, String.Empty);
                StringWriter writer = new StringWriter();
                MyXmlTextWriter txtwriter = new MyXmlTextWriter(writer);
                txtwriter.WriteRaw(String.Empty);
                txtwriter.Formatting = Formatting.None;
                XmlSerializer serializer = new XmlSerializer(typeof(PEDIDO));
                serializer.Serialize(txtwriter, p, nmspc);

                xml.Append(txtwriter.GetXML());
            }

            if (xml.Length == 0)
                xml.Append("<PEDIDO/>");

            GenericHelper.LogFile("SuperPagWS::LegacyService.cs::ConsultaPedido loja=" + store.storeId + " pedido= " + pedido + " retorno=" + xml.ToString(), LogFileEntryType.Information);
            
            return xml.ToString();
        }
        catch (ApplicationException e)
        {
            GenericHelper.LogFile("SuperPagWS::LegacyService.cs::ConsultaPedido " + e.Message, LogFileEntryType.Warning);

            return e.Message;
        }
        catch (Exception e)
        {
            GenericHelper.LogFile("SuperPagWS::LegacyService.cs::ConsultaPedido " + e.Message, LogFileEntryType.Error);
            
            SoapException se = new SoapException("Erro interno: " + e.Message + (e.InnerException != null ? e.InnerException.Message : ""), SoapException.ClientFaultCode, Context.Request.Url.AbsoluteUri);
            throw se;
        }
    }
}
