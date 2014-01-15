using System;
using System.Xml;
using System.Web;
using System.Text;
using System.Threading;
using System.Collections.Generic;
using System.Collections.Specialized;
using SuperPag.Data;
using SuperPag.Data.Messages;
using SuperPag.Data.Interfaces;
using SuperPag.Helper;
using System.Configuration;

namespace SuperPag.Handshake.Xml
{
    public class Helper
    {
        public string CreateXmlForResponse(string urlRedirect)
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlNode root = xmlDoc.CreateElement("root");
            XmlNode queryStringNode = xmlDoc.CreateElement("queryString");
            GenericHelper.AppendNodeForCollection(xmlDoc, queryStringNode, HttpContext.Current.Request.QueryString);
            root.AppendChild(queryStringNode);
            xmlDoc.AppendChild(root);

            return xmlDoc.InnerXml;
        }
        public string CreateXmlForRequest()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlNode rootNode = xmlDoc.CreateElement("root");
            XmlNode formNode = xmlDoc.CreateElement("form");
            XmlNode queryStringNode = xmlDoc.CreateElement("queryString");

            GenericHelper.AppendNodeForCollection(xmlDoc, formNode, HttpContext.Current.Request.Form);
            GenericHelper.AppendNodeForCollection(xmlDoc, queryStringNode, HttpContext.Current.Request.QueryString);
            rootNode.AppendChild(formNode);
            rootNode.AppendChild(queryStringNode);
            xmlDoc.AppendChild(rootNode);

            return xmlDoc.InnerXml;
        }

        public void ParseXml(XmlDocument xmlDoc)
        {
            //cria a order
            long orderId = CreateOrder(xmlDoc);

            //atualizo a HandshakeSession
            Guid handshakeSessionId = new Guid(HttpContext.Current.Session["handshakeSessionId"].ToString());
            DHandshakeSession handshakeSession = DataFactory.HandshakeSession().Locate(handshakeSessionId);
            Ensure.IsNotNullPage(handshakeSession, "Sessão de pagamento inválida.");
            handshakeSession.orderId = orderId;
            DataFactory.HandshakeSession().Update(handshakeSession);

            //cria os itens da order
            CreateOrderItens(xmlDoc, orderId);

            //cria o consumer
            long consumerId = CreateConsumer(xmlDoc, orderId);

            //cria o address
            CreateAddress(xmlDoc, consumerId);

            //verifica o idioma
            if (HttpContext.Current.Session["Language"] == null)
            {
                if (xmlDoc.SelectSingleNode("/pedido/parametros_opcionais/idioma") != null &&
                    xmlDoc.SelectSingleNode("/pedido/parametros_opcionais/idioma").InnerText != "")
                {
                    string language = xmlDoc.SelectSingleNode("/pedido/parametros_opcionais/idioma").InnerText;
                    HttpContext.Current.Session["Language"] = language;
                    System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(language);
                }
            }

            //verifica se é simulacao
            string simulacao = GenericHelper.GetSingleNodeString(xmlDoc, "/pedido/parametros_opcionais/simulacao");
            if (simulacao.Trim() == "1")
                HttpContext.Current.Session["isSimulation"] = true;

            string formapagto = GenericHelper.GetSingleNodeString(xmlDoc, "/pedido/parametros_opcionais/forma_pagto");
            string bandeira = GenericHelper.GetSingleNodeString(xmlDoc, "/pedido/parametros_opcionais/bandeira");
            string pqtdparcelas = GenericHelper.GetSingleNodeString(xmlDoc, "/pedido/parametros_opcionais/pqtdparcelas");

            GenericHelper.SetCanChoosePaymentFormSession(HttpContext.Current, true);

            int handshakePaymentFormId, handshakeCardFlag;
            if (int.TryParse(formapagto, out handshakePaymentFormId))
            {
                if (handshakePaymentFormId == (int)SmartPagLegacy.CodigoMeioPagamento.CartaoDeCredito)
                {
                    if (int.TryParse(bandeira, out handshakeCardFlag) && SuperPag.Handshake.Helper.SmartPagToSuperpagPaymentForm(handshakePaymentFormId, handshakeCardFlag) != int.MinValue)
                        GenericHelper.SetHandshakePaymentFormSession(HttpContext.Current, SuperPag.Handshake.Helper.SmartPagToSuperpagPaymentForm(handshakePaymentFormId, handshakeCardFlag).ToString());
                    else
                        GenericHelper.SetHandshakePaymentFormSession(HttpContext.Current, PaymentGroupsWord.CreditCard);

                    GenericHelper.SetCanChoosePaymentFormSession(HttpContext.Current, false);
                }
                else
                {
                    int paymentFormId = SuperPag.Handshake.Helper.SmartPagToSuperpagPaymentForm(handshakePaymentFormId, int.MinValue);
                    if (paymentFormId != int.MinValue)
                    {
                        GenericHelper.SetHandshakePaymentFormSession(HttpContext.Current, paymentFormId.ToString());
                        GenericHelper.SetCanChoosePaymentFormSession(HttpContext.Current, false);
                    }
                }
            }

            int handshakeInstallmentNumber;
            if (int.TryParse(pqtdparcelas, out handshakeInstallmentNumber))
            {
                GenericHelper.SetHandshakeInstallmentNumber(HttpContext.Current, handshakeInstallmentNumber);
            }

            HttpContext.Current.Session["orderId"] = orderId;
            HttpContext.Current.Session["consumerId"] = consumerId;

            #region Preencho a Order de acordo com o XML
            //    if (xmlDoc.SelectSingleNode("/pedido/setup_loja/urls").HasChildNodes)
            //    {
            //        mWebSession.UrlFinalization = GetNodeTextString( "/pedido/setup_loja/urls/post_final" );
            //        mWebSession.UrlReturn = GetNodeTextString( "/pedido/setup_loja/urls/link_retorno" );
            //    }

            //    if (xmlDoc.SelectSingleNode("/pedido/parametros_opcionais").HasChildNodes)
            //    {
            //        #region Define Meio de Pagamento / Parcelamento
            //        bool hasPaymentForm = false;
            //        switch ( GetNodeTextInt( "/pedido/parametros_opcionais/forma_pagto" ) )
            //        {
            //            //Cartao de credito
            //            case 2:
            //                #region Analisar Bandeira
            //                switch ( GetNodeTextInt( "/pedido/parametros_opcionais/bandeira" ) )
            //                {
            //                    //Visa VBV
            //                    case 23:
            //                        mWebSession.LastPaymentFormId = 9;
            //                        hasPaymentForm = true;
            //                        break;
            //                    //Mastercard Komerci
            //                    case 41:
            //                        break;
            //                    //Dinners Komerci
            //                    case 44:
            //                        break;
            //                    //Amex WebPOS
            //                    case 50:
            //                        break;
            //                    //Visa SITEF
            //                    case 123:
            //                        mWebSession.LastPaymentFormId = 2;
            //                        hasPaymentForm = true;
            //                        break;
            //                    //Mastercard SITEF
            //                    case 141:
            //                        mWebSession.LastPaymentFormId = 3;
            //                        hasPaymentForm = true;
            //                        break;
            //                    //Dinners SITEF
            //                    case 144:
            //                        mWebSession.LastPaymentFormId = 4;
            //                        hasPaymentForm = true;
            //                        break;
            //                    //Amex SITEF
            //                    case 150:
            //                        mWebSession.LastPaymentFormId = 6;
            //                        hasPaymentForm = true;
            //                        break;
            //                    //Hipercard SITEF
            //                    case 151:
            //                        mWebSession.LastPaymentFormId = 5;
            //                        hasPaymentForm = true;
            //                        break;
            //                }
            //                #endregion
            //                break;
            //            //Debito em conta (COB)
            //            case 13:
            //                break;
            //            //Pagamento Facil Bradesco
            //            case 14:
            //                mWebSession.LastPaymentFormId = 1;
            //                hasPaymentForm = true;
            //                break;
            //            //Itaú ShopLine
            //            case 18:
            //                mWebSession.LastPaymentFormId = 7;
            //                hasPaymentForm = true;
            //                break;
            //            //BB Pag
            //            case 21:
            //                mWebSession.LastPaymentFormId = 8;
            //                hasPaymentForm = true;
            //                break;
            //            //Real Pag
            //            case 34:
            //                break;
            //            //Visa Electron
            //            case 35:
            //                mWebSession.LastPaymentFormId = 10;
            //                hasPaymentForm = true;
            //                break;
            //            //Boleto Bradesco
            //            case 32:
            //                break;
            //        }

            //        //TODO: Logica para nao passar pela escolha de meio de pagamento
            //        if (hasPaymentForm)
            //        {
            //            short InstallmentNumber = (short)GetNodeTextInt( "/pedido/parametros_opcionais/num_parcela" );
            //            if ( InstallmentNumber != 0 )
            //            {
            //                bool canUseInstallmentNumber = false;
            //                #region Verifica se existe parcela para o determinado valor e meio de pagamento

            //                #endregion

            //                if ( canUseInstallmentNumber )
            //                {
            //                    WebSessionWorkflow.WebSessionWorkFlow.Instance().DefinePaymentInstallment( mWebSession.WebSessionId , (byte)InstallmentNumber );
            //                }
            //            }
            //            else
            //            {
            //                byte MaxInstallment = (byte)GetNodeTextInt( "/pedido/parametros_opcionais/pqtdparcelas" );
            //                if ( MaxInstallment != 0 )
            //                {
            //                    #region Define Parcelamento Máximo
            //                    //TODO: Definir variavel para controlar parcelamento maximo mostrado
            //                    #endregion
            //                }
            //            }
            //        }
            //        #endregion
            #endregion
        }

        public static void CreateAddress(XmlDocument xmlDoc, long consumerId)
        {
            DConsumerAddress dConsumerAddress;
            if (xmlDoc.SelectSingleNode("/pedido/dados_cliente/endereco_cobranca") != null && xmlDoc.SelectSingleNode("/pedido/dados_cliente/endereco_cobranca").HasChildNodes)
            {
                //cria o endereco de cobranca
                dConsumerAddress = new DConsumerAddress();
                dConsumerAddress.logradouro = GenericHelper.GetSingleNodeString(xmlDoc, "/pedido/dados_cliente/endereco_cobranca/logradouro_ec");
                dConsumerAddress.address = GenericHelper.GetSingleNodeString(xmlDoc, "/pedido/dados_cliente/endereco_cobranca/endereco_ec");
                dConsumerAddress.addressNumber = GenericHelper.GetSingleNodeString(xmlDoc, "/pedido/dados_cliente/endereco_cobranca/numero_ec");
                dConsumerAddress.addressType = (int)AddressTypes.Billing;
                dConsumerAddress.addressComplement = GenericHelper.GetSingleNodeString(xmlDoc, "/pedido/dados_cliente/endereco_cobranca/complemento_ec");
                dConsumerAddress.district = GenericHelper.GetSingleNodeString(xmlDoc, "/pedido/dados_cliente/endereco_cobranca/bairro_ec");
                dConsumerAddress.city = GenericHelper.GetSingleNodeString(xmlDoc, "/pedido/dados_cliente/endereco_cobranca/cidade_ec");
                dConsumerAddress.cep = GenericHelper.GetSingleNodeString(xmlDoc, "/pedido/dados_cliente/endereco_cobranca/cep_ec");
                dConsumerAddress.state = GenericHelper.GetSingleNodeString(xmlDoc, "/pedido/dados_cliente/endereco_cobranca/estado_ec");
                dConsumerAddress.country = GenericHelper.GetSingleNodeString(xmlDoc, "/pedido/dados_cliente/endereco_cobranca/pais_ec");
                dConsumerAddress.consumerAddressId = long.MinValue;
                dConsumerAddress.consumerId = consumerId;
                //insere o endereco de cobranca
                DataFactory.ConsumerAddress().Insert(dConsumerAddress);
            }

            if (xmlDoc.SelectSingleNode("/pedido/dados_cliente/endereco_entrega") != null && xmlDoc.SelectSingleNode("/pedido/dados_cliente/endereco_entrega").HasChildNodes)
            {
                //cria o endereco de entrega
                dConsumerAddress = new DConsumerAddress();
                dConsumerAddress.logradouro = GenericHelper.GetSingleNodeString(xmlDoc, "/pedido/dados_cliente/endereco_entrega/logradouro_ee");
                dConsumerAddress.address = GenericHelper.GetSingleNodeString(xmlDoc, "/pedido/dados_cliente/endereco_entrega/endereco_ee");
                dConsumerAddress.addressNumber = GenericHelper.GetSingleNodeString(xmlDoc, "/pedido/dados_cliente/endereco_entrega/numero_ee");
                dConsumerAddress.addressType = (int)AddressTypes.Delivery;
                dConsumerAddress.addressComplement = GenericHelper.GetSingleNodeString(xmlDoc, "/pedido/dados_cliente/endereco_entrega/complemento_ee");
                dConsumerAddress.district = GenericHelper.GetSingleNodeString(xmlDoc, "/pedido/dados_cliente/endereco_entrega/bairro_ee");
                dConsumerAddress.city = GenericHelper.GetSingleNodeString(xmlDoc, "/pedido/dados_cliente/endereco_entrega/cidade_ee");
                dConsumerAddress.cep = GenericHelper.GetSingleNodeString(xmlDoc, "/pedido/dados_cliente/endereco_entrega/cep_ee");
                dConsumerAddress.state = GenericHelper.GetSingleNodeString(xmlDoc, "/pedido/dados_cliente/endereco_entrega/estado_ee");
                dConsumerAddress.country = GenericHelper.GetSingleNodeString(xmlDoc, "/pedido/dados_cliente/endereco_entrega/pais_ee");
                dConsumerAddress.consumerAddressId = long.MinValue;
                dConsumerAddress.consumerId = consumerId;
                //insere o endereco de entrega
                DataFactory.ConsumerAddress().Insert(dConsumerAddress);
            }
        }
        public static long CreateConsumer(XmlDocument xmlDoc, long orderId)
        {
            DConsumer dConsumer = new DConsumer();
            if (xmlDoc.SelectSingleNode("/pedido/dados_cliente/pessoa_fisica") != null && xmlDoc.SelectSingleNode("/pedido/dados_cliente/pessoa_fisica").HasChildNodes)
            {
                //Pessoa Fisica
                dConsumer.name = GenericHelper.GetSingleNodeString(xmlDoc, "/pedido/dados_cliente/pessoa_fisica/nome_pf");
                dConsumer.CPF = GenericHelper.GetSingleNodeString(xmlDoc, "/pedido/dados_cliente/pessoa_fisica/cpf_pf");
                dConsumer.birthDate = GenericHelper.ParseDateddMMyyyy(GenericHelper.GetSingleNodeString(xmlDoc, "/pedido/dados_cliente/pessoa_fisica/data_nascimento_pf"));
                dConsumer.ger = GenericHelper.GetSingleNodeString(xmlDoc, "/pedido/dados_cliente/pessoa_fisica/sexo_pf");
                dConsumer.civilState = GenericHelper.GetSingleNodeString(xmlDoc, "/pedido/dados_cliente/pessoa_fisica/estado_civil_pf");
                dConsumer.occupation = GenericHelper.GetSingleNodeString(xmlDoc, "/pedido/dados_cliente/pessoa_fisica/profissao_pf");
                dConsumer.email = GenericHelper.GetSingleNodeString(xmlDoc, "/pedido/dados_cliente/pessoa_fisica/email_pf");
                dConsumer.phone = GenericHelper.GetSingleNodeString(xmlDoc, "/pedido/dados_cliente/pessoa_fisica/telefone_pf");
                dConsumer.fax = GenericHelper.GetSingleNodeString(xmlDoc, "/pedido/dados_cliente/pessoa_fisica/fax_pf");
            }
            else if (xmlDoc.SelectSingleNode("/pedido/dados_cliente/pessoa_juridica") != null && xmlDoc.SelectSingleNode("/pedido/dados_cliente/pessoa_juridica").HasChildNodes)
            {
                //Pessoa Juridica
                dConsumer.name = GenericHelper.GetSingleNodeString(xmlDoc, "/pedido/dados_cliente/pessoa_juridica/razao_social_pj");
                dConsumer.CNPJ = GenericHelper.GetSingleNodeString(xmlDoc, "/pedido/dados_cliente/pessoa_juridica/cgc_pj");
                dConsumer.email = GenericHelper.GetSingleNodeString(xmlDoc, "/pedido/dados_cliente/pessoa_juridica/email_pj");
                dConsumer.phone = GenericHelper.GetSingleNodeString(xmlDoc, "/pedido/dados_cliente/pessoa_juridica/telefone_pj");
                dConsumer.fax = GenericHelper.GetSingleNodeString(xmlDoc, "/pedido/dados_cliente/pessoa_juridica/fax_pj");
                dConsumer.IE = GenericHelper.GetSingleNodeString(xmlDoc, "/pedido/dados_cliente/pessoa_juridica/inscricao_estadual_pj");
                dConsumer.responsibleCPF = GenericHelper.GetSingleNodeString(xmlDoc, "/pedido/dados_cliente/pessoa_juridica/cpf_responsavel_pj");
                dConsumer.responsibleName = GenericHelper.GetSingleNodeString(xmlDoc, "/pedido/dados_cliente/pessoa_juridica/nome_responsavel_pj");
            }

            //insere o consumidor
            DataFactory.Consumer().Insert(dConsumer);

            //vincula consumidor com a order
            DataFactory.Order().Update(dConsumer.consumerId, orderId);

            return dConsumer.consumerId;
        }
        public void CreateOrderItens(XmlDocument xmlDoc, long orderId)
        {
            //defino variavel para itemNumber
            int count = 0;

            if (xmlDoc.SelectSingleNode("/pedido/valor_frete_pedido") != null &&
                xmlDoc.SelectSingleNode("/pedido/valor_frete_pedido").InnerText != "")
            {
                //Insiro o item do frete
                DOrderItem orderItem = new DOrderItem();
                orderItem.itemCode = "ShippingRate";
                orderItem.itemDescription = "Frete";
                orderItem.itemQuantity = 1;
                orderItem.itemValue = GenericHelper.ParseDecimal(xmlDoc.SelectSingleNode("/pedido/valor_frete_pedido").InnerText);
                orderItem.itemNumber = count;
                orderItem.itemType = (int)ItemTypes.ShippingRate;
                orderItem.orderItemId = long.MinValue;
                orderItem.orderId = orderId;
                DataFactory.OrderItem().Insert(orderItem);
            }

            //Insiro outros items
            foreach (XmlNode node in xmlDoc.SelectNodes("/pedido/itens/item"))
            {
                DOrderItem orderItem = new DOrderItem();

                orderItem.itemCode = node.SelectSingleNode("codigo_item") != null ? GenericHelper.ParseString(node.SelectSingleNode("codigo_item").InnerText) : "";
                orderItem.itemDescription = node.SelectSingleNode("descricao_item") != null ? GenericHelper.ParseString(node.SelectSingleNode("descricao_item").InnerText) : "";
                orderItem.itemQuantity = node.SelectSingleNode("quantidade_item") != null ? GenericHelper.ParseInt(node.SelectSingleNode("quantidade_item").InnerText) : int.MinValue;
                orderItem.itemValue = node.SelectSingleNode("valor_unitario_item") != null ? GenericHelper.ParseDecimal(node.SelectSingleNode("valor_unitario_item").InnerText) : int.MinValue;
                orderItem.itemNumber = count++;
                orderItem.orderItemId = long.MinValue;
                orderItem.orderId = orderId;

                if (orderItem.itemValue < 0)
                    orderItem.itemType = (int)ItemTypes.Discount;
                else
                    orderItem.itemType = (int)ItemTypes.Regular;

                DataFactory.OrderItem().Insert(orderItem);
            }
        }
        public long CreateOrder(XmlDocument xmlDoc)
        {
            DOrder dOrder = new DOrder();
            dOrder.orderId = long.MinValue;
            dOrder.storeId = int.Parse(HttpContext.Current.Session["storeId"].ToString());
            dOrder.consumerId = long.MinValue;
            dOrder.storeReferenceOrder = GenericHelper.GetSingleNodeString(xmlDoc, "/pedido/numero_pedido");
            dOrder.totalAmount = GenericHelper.ParseDecimal(GenericHelper.GetSingleNodeString(xmlDoc, "/pedido/valor_total_pedido"));
            dOrder.finalAmount = decimal.MinValue;
            dOrder.installmentQuantity = int.MinValue;
            dOrder.creationDate = DateTime.Now;
            dOrder.lastUpdateDate = DateTime.Now;
            dOrder.status = (int)OrderStatus.Unfinished;
            DataFactory.Order().Insert(dOrder);

            return dOrder.orderId;
        }
    }

    public class FinalizationPost
    {
        public bool Result;
        public string Error;
        public bool ShowFinalization = true;
        public bool IsOffLine = false;

        DOrder _dOrder;
        DOrderInstallment[] _dOrderInstallment;
        DHandshakeSessionLog[] _hsLogs;
        DConsumer _dConsumer;
        DPaymentForm _dPaymentForm;
        DConsumerAddress _dBillingAddress;
        DConsumerAddress _dDeliveryAddress;
        DOrderItem[] _dOrderItemArr;
        decimal _shippingRate;
        DPaymentAttempt _dAttempt;
        DHandshakeConfiguration _dHandshakeConfiguration;
        DHandshakeConfigurationXml _dHandshakeConfigurationXml;

        public FinalizationPost(Guid paymentAttemptId)
        {
            try
            {
                //obtem a attempt
                DPaymentAttempt dPaymentAttempt = DataFactory.PaymentAttempt().Locate(paymentAttemptId);
                Ensure.IsNotNull(dPaymentAttempt, "Não há tentativas de pagamento para o pedido");
                _dAttempt = dPaymentAttempt;

                //localiza o pedido
                _dOrder = DataFactory.Order().Locate(dPaymentAttempt.orderId);
                Ensure.IsNotNull(_dOrder, "Pedido {0} não encontrado", dPaymentAttempt.orderId);

                //pego a HandshakeSession
                DHandshakeSession[] handshakeSessions = DataFactory.HandshakeSession().List(_dOrder.orderId);
                Ensure.IsNotNull(handshakeSessions, "Sessão de pagamento inválida");

                //pego os logs com os dados do pedido enviado
                DHandshakeSessionLog[] hsLogs = DataFactory.HandshakeSessionLog().List(handshakeSessions[0].handshakeSessionId);
                _hsLogs = hsLogs;

                //localiza a primeira parcelas do pedido
                _dOrderInstallment = DataFactory.OrderInstallment().List(_dOrder.orderId);
                Ensure.IsNotNull(_dOrderInstallment, "Parcelas do pedido {0} não encontradas", _dOrder.orderId);

                //localiza o consumidor
                _dConsumer = DataFactory.Consumer().Locate(_dOrder.consumerId);
                Ensure.IsNotNull(_dConsumer, "Cliente {0} não encontrado", _dConsumer);

                //localiza o forma de pagamento
                _dPaymentForm = DataFactory.PaymentForm().Locate(dPaymentAttempt.paymentFormId);
                Ensure.IsNotNull(_dPaymentForm, "Forma de pagamento {0} não encontrada", dPaymentAttempt.paymentFormId);

                //localiza configuracoes do handshake xml
                _dHandshakeConfiguration = DataFactory.HandshakeConfiguration().Locate(_dOrder.storeId, (int)HandshakeType.XmlSPag10);
                Ensure.IsNotNull(_dHandshakeConfiguration, "Configuração de handshake para a loja {0}  não encontrada.", _dOrder.storeId);
                _dHandshakeConfigurationXml = DataFactory.HandshakeConfigurationXml().Locate(_dHandshakeConfiguration.handshakeConfigurationId);
                Ensure.IsNotNull(_dHandshakeConfigurationXml, "Configuração de handshake xml para a loja {0}  não encontrada.", _dOrder.storeId);

                //localiza o endereco de cobranca
                IConsumerAddress iConsumerAddress = DataFactory.ConsumerAddress();
                _dBillingAddress = iConsumerAddress.Locate(_dConsumer.consumerId, (int)AddressTypes.Billing);
                Ensure.IsNotNull(_dBillingAddress, "Endereco de cobrança do cliente {0} não encontrado", _dConsumer.consumerId);

                //localiza o endereco de entrega
                _dDeliveryAddress = iConsumerAddress.Locate(_dConsumer.consumerId, (int)AddressTypes.Delivery);
                Ensure.IsNotNull(_dDeliveryAddress, "Endereco de entrega do cliente {0} não encontrado", _dConsumer.consumerId);

                //localiza os itens do carrinho de compras
                int[] itensTypes = new int[2];
                itensTypes[0] = (int)ItemTypes.Regular;
                itensTypes[1] = (int)ItemTypes.Discount;
                _dOrderItemArr = DataFactory.OrderItem().List(_dOrder.orderId, itensTypes);
                Ensure.IsNotNull(_dOrderItemArr, "Itens do pedido {0} não encontrados", _dOrder.orderId);

                //calcular o frete total
                DOrderItem[] shippingItens = DataFactory.OrderItem().List(_dOrder.orderId, (int)ItemTypes.ShippingRate);
                _shippingRate = 0;
                if (shippingItens != null)
                {
                    foreach (DOrderItem shippingItem in shippingItens)
                        _shippingRate += shippingItem.itemValue;
                }
            }
            catch (Exception e)
            {
                GenericHelper.LogFile("EasyPagObject::HelperXml.cs::FinalizationPost.FinalizationPost(Guid) " + e.Message, LogFileEntryType.Error);
                throw;
            }
        }

        public void Send()
        {
            try
            {
                if (_dHandshakeConfiguration.finalizationHtml)
                {
                    SuperPag.Handshake.Html.FinalizationPost posthtml = new SuperPag.Handshake.Html.FinalizationPost(_dAttempt.paymentAttemptId, true);
                    posthtml.IsOffLine = IsOffLine;
                    if (ShowFinalization)
                        posthtml.Send();
                    else
                        posthtml.SendClient();

                    this.Result = posthtml.Result;
                    this.Error = posthtml.Error;

                    return;
                }

                string xmlData = "", url = "", urlHandshake = "", urlHandshakeConfiguration = "";
                foreach (DHandshakeSessionLog hsLog in _hsLogs)
                {
                    if (hsLog.step == 3)
                        xmlData = hsLog.xmlData;
                }

                urlHandshake = (IsOffLine ? "" : GenericHelper.GetSingleNodeString(xmlData, "//post_final"));
                urlHandshakeConfiguration = (IsOffLine ? _dHandshakeConfigurationXml.urlFinalizationOffline : _dHandshakeConfigurationXml.urlFinalization);

                if (String.IsNullOrEmpty(urlHandshake) && String.IsNullOrEmpty(urlHandshakeConfiguration))
                {
                    GenericHelper.LogFile("EasyPagObject::HelperXml.cs::FinalizationPost.Send A loja " + _dOrder.storeId.ToString() + " não possue url para o post de finalização " + (IsOffLine ? " offline " : "") + "configurada, o post não será enviado", LogFileEntryType.Information);

                    Result = true;
                    Error = "";
                    return;
                }

                if (!String.IsNullOrEmpty(urlHandshake))
                    url = urlHandshake;
                else if (!String.IsNullOrEmpty(urlHandshakeConfiguration))
                    url = urlHandshakeConfiguration;

                //pega o XML da handshakeSession
                XmlDocument xml = new XmlDocument();
                XmlDocument xmlHandshake = new XmlDocument();
                xmlHandshake.LoadXml(xmlData);

                DPaymentForm paymentForm = DataFactory.PaymentForm().Locate(_dAttempt.paymentFormId);
                Ensure.IsNotNullPage(paymentForm, "Meio de pagamento inválido");

                //adiciona parametros de finalizacao
                NameValueCollection aditParams = new NameValueCollection();
                NameValueCollection aditParamsFim = new NameValueCollection();
                NameValueCollection aditPFParams = new NameValueCollection();
                NameValueCollection aditParamsConsumer = new NameValueCollection();
                NameValueCollection aditParamsBillingAdd = new NameValueCollection();
                NameValueCollection aditParamsDeliveryAdd = new NameValueCollection();
                NameValueCollection aditPOParams = new NameValueCollection();

                aditParams.Add("numero_pedido", _dOrder.storeReferenceOrder);
                aditParams.Add("forma_pagto", System.Web.HttpUtility.HtmlEncode(_dPaymentForm.name));
                aditParams.Add("valor_parcial_pedido", GenericHelper.ParseString(_dOrder.totalAmount));
                aditParams.Add("valor_final_pedido", GenericHelper.ParseString(_dOrder.finalAmount));
                aditParams.Add("numero_parcelas_pedido", _dOrder.installmentQuantity.ToString());
                aditParams.Add("valor_parcela", GenericHelper.ParseString(_dOrderInstallment[0].installmentValue));
                aditParams.Add("taxa_juros_desconto", GenericHelper.ParseString(_dOrderInstallment[0].interestPercentage));
                aditParams.Add("codigo_meio_pagamento", SuperPag.Handshake.Helper.GetSmartPagPaymentForm(paymentForm.paymentFormId).ToString());
                aditParams.Add("data_trans", _dOrder.creationDate.ToString("yyyyMMdd"));
                aditParams.Add("hora_trans", _dOrder.creationDate.ToString("hhmmss"));

                aditParamsFim.Add("codigo_retorno", "0");
                aditParamsFim.Add("descricao_retorno", "SUPERPAG OK");

                //se pessoa juridica
                if (_dConsumer.CNPJ != null && _dConsumer.CNPJ != string.Empty)
                {
                    aditParamsConsumer.Add("tipo_pessoa", "J");
                    aditParamsConsumer.Add("cgc_pj", _dConsumer.CNPJ);
                    aditParamsConsumer.Add("razao_social_pj", _dConsumer.name);
                    aditParamsConsumer.Add("email_pj", _dConsumer.email);
                    aditParamsConsumer.Add("telefone_pj", _dConsumer.phone);
                    aditParamsConsumer.Add("fax_pj", _dConsumer.fax);
                    aditParamsConsumer.Add("inscricao_estadual_pj", _dConsumer.IE);
                    aditParamsConsumer.Add("cpf_responsavel_pj", _dConsumer.responsibleCPF);
                    aditParamsConsumer.Add("nome_responsavel_pj", _dConsumer.responsibleName);
                }
                else
                {
                    aditParamsConsumer.Add("tipo_pessoa", "F");
                    aditParamsConsumer.Add("cpf_pf", _dConsumer.CPF);
                    aditParamsConsumer.Add("nome_pf", _dConsumer.name);
                    aditParamsConsumer.Add("data_nascimento_pf", (_dConsumer.birthDate != DateTime.MinValue ? _dConsumer.birthDate.ToString("yyyyMMdd") : ""));
                    aditParamsConsumer.Add("sexo_pf", _dConsumer.ger);
                    aditParamsConsumer.Add("estado_civil_pf", _dConsumer.civilState);
                    aditParamsConsumer.Add("profissao_pf", _dConsumer.occupation);
                    aditParamsConsumer.Add("email_pf", _dConsumer.email);
                    aditParamsConsumer.Add("telefone_pf", _dConsumer.phone);
                    aditParamsConsumer.Add("fax_pf", _dConsumer.fax);
                }

                aditParamsBillingAdd.Add("logradouro_ec", _dBillingAddress.logradouro);
                aditParamsBillingAdd.Add("endereco_ec", _dBillingAddress.address);
                aditParamsBillingAdd.Add("numero_ec", _dBillingAddress.addressNumber);
                aditParamsBillingAdd.Add("complemento_ec", _dBillingAddress.addressComplement);
                aditParamsBillingAdd.Add("bairro_ec", _dBillingAddress.district);
                aditParamsBillingAdd.Add("cidade_ec", _dBillingAddress.city);
                aditParamsBillingAdd.Add("cep_ec", _dBillingAddress.cep);
                aditParamsBillingAdd.Add("estado_ec", _dBillingAddress.state.Trim());
                aditParamsBillingAdd.Add("pais_ec", _dBillingAddress.country);

                aditParamsDeliveryAdd.Add("nome_responsavel_ee", _dConsumer.name);
                aditParamsDeliveryAdd.Add("telefone_responsavel_ee", _dConsumer.phone);
                aditParamsDeliveryAdd.Add("logradouro_ee", _dDeliveryAddress.logradouro);
                aditParamsDeliveryAdd.Add("endereco_ee", _dDeliveryAddress.address);
                aditParamsDeliveryAdd.Add("numero_ee", _dDeliveryAddress.addressNumber);
                aditParamsDeliveryAdd.Add("complemento_ee", _dDeliveryAddress.addressComplement);
                aditParamsDeliveryAdd.Add("bairro_ee", _dDeliveryAddress.district);
                aditParamsDeliveryAdd.Add("cidade_ee", _dDeliveryAddress.city);
                aditParamsDeliveryAdd.Add("cep_ee", _dDeliveryAddress.cep);
                aditParamsDeliveryAdd.Add("estado_ee", _dDeliveryAddress.state.Trim());
                aditParamsDeliveryAdd.Add("pais_ee", _dDeliveryAddress.country);

                switch (paymentForm.paymentAgentId)
                {
                    case (int)PaymentAgents.Boleto:
                        foreach (DOrderInstallment installment in _dOrderInstallment)
                        {
                            string serverUrl = "";
                            if (HttpContext.Current != null)
                            {
                                string http = (HttpContext.Current.Request.ServerVariables["HTTPS"] == "off" ? "http" : "https");
                                string server = HttpContext.Current.Request.ServerVariables["SERVER_NAME"];
                                serverUrl = String.Format("{0}://{1}", http, server);
                            }
                            else if (ConfigurationManager.AppSettings != null && ConfigurationManager.AppSettings["ServerUrl"] != null)
                                serverUrl = ConfigurationManager.AppSettings["ServerUrl"];

                            DPaymentAttempt attempt = DataFactory.PaymentAttempt().Locate(installment.orderId, installment.installmentNumber);
                            DPaymentAttemptBoleto boleto = DataFactory.PaymentAttemptBoleto().Locate(attempt.paymentAttemptId);
                            aditPFParams.Set(String.Format("oct{0}", installment.installmentNumber), boleto.oct);
                            aditPFParams.Add(String.Format("urlboleto{0}", installment.installmentNumber), String.Format("{0}/Agents/Boleto/showboleto.aspx?id={1}", serverUrl, attempt.paymentAttemptId));
                        }
                        break;
                    case (int)PaymentAgents.VBV:
                    case (int)PaymentAgents.VBVInBox:
                    case (int)PaymentAgents.VBV3:
                        DPaymentAttemptVBV vbv = DataFactory.PaymentAttemptVBV().Locate(_dAttempt.paymentAttemptId);
                        aditPFParams.Add("bandeira", SuperPag.Handshake.Helper.GetSmartPagCreditCardFlag(paymentForm.paymentFormId).ToString());
                        aditPFParams.Add("id_trans", vbv.tid);
                        aditPFParams.Add("num_autorizacao", vbv.arp.ToString());
                        break;
                    case (int)PaymentAgents.VisaMoset:
                    case (int)PaymentAgents.VisaMoset3:
                        DPaymentAttemptMoset moset = DataFactory.PaymentAttemptMoset().Locate(_dAttempt.paymentAttemptId);
                        aditPFParams.Add("bandeira", SuperPag.Handshake.Helper.GetSmartPagCreditCardFlag(paymentForm.paymentFormId).ToString());
                        aditPFParams.Add("id_trans", moset.tid);
                        break;
                    case (int)PaymentAgents.Komerci:
                    case (int)PaymentAgents.KomerciInBox:
                        DPaymentAttemptKomerci komerci = DataFactory.PaymentAttemptKomerci().Locate(_dAttempt.paymentAttemptId);
                        aditPFParams.Add("bandeira", SuperPag.Handshake.Helper.GetSmartPagCreditCardFlag(_dPaymentForm.paymentFormId).ToString());
                        aditPFParams.Add("num_autorizacao", komerci.numautor);
                        aditPFParams.Add("comprov_venda", komerci.numcv);
                        aditPFParams.Add("id_trans", komerci.numcv);
                        aditPFParams.Add("nsu_trans", komerci.numautent);
                        break;
                    case (int)PaymentAgents.KomerciWS:
                        DPaymentAttemptKomerciWS komerciws = DataFactory.PaymentAttemptKomerciWS().Locate(_dAttempt.paymentAttemptId);
                        aditPFParams.Add("bandeira", SuperPag.Handshake.Helper.GetSmartPagCreditCardFlag(_dPaymentForm.paymentFormId).ToString());
                        aditPFParams.Add("num_autorizacao", komerciws.numautor);
                        aditPFParams.Add("comprov_venda", komerciws.numcv);
                        aditPFParams.Add("id_trans", komerciws.numcv);
                        aditPFParams.Add("nsu_trans", komerciws.numautent);
                        break;
                    case (int)PaymentAgents.PaymentClientVirtual2Party:
                    case (int)PaymentAgents.PaymentClientVirtual3Party:
                        DPaymentAttemptPaymentClientVirtual payclient = DataFactory.PaymentAttemptPaymentClientVirtual().Locate(_dAttempt.paymentAttemptId);
                        aditPFParams.Add("bandeira", SuperPag.Handshake.Helper.GetSmartPagCreditCardFlag(paymentForm.paymentFormId).ToString());
                        aditPFParams.Add("id_trans", payclient.vpc_TransactionNo.ToString());
                        aditPFParams.Add("num_autorizacao", payclient.vpc_AuthorizeId.ToString());
                        break;
                    case (int)PaymentAgents.ItauShopLine:
                        DPaymentAttemptItauShopline itauShopline = DataFactory.PaymentAttemptItauShopline().Locate(_dAttempt.paymentAttemptId);
                        aditPFParams.Add("num_autorizacao", (_dAttempt.status == (int)PaymentAttemptStatus.Paid ? "00" : "01"));
                        aditPFParams.Add("pedido_shopline", itauShopline.agentOrderReference.ToString());
                        break;
                    case (int)PaymentAgents.BBPag:
                        DPaymentAttemptBB bbpag = DataFactory.PaymentAttemptBB().Locate(_dAttempt.paymentAttemptId);
                        aditPFParams.Add("num_autorizacao", bbpag.agentOrderReference.ToString());
                        break;
                    case (int)PaymentAgents.FinanciamentoABN:
                        break;
                }

                //Incluo os parametros opcionais
                //inicialmente, tudo o que vier dentro do 'controle_loja'

                #region Modelos para saber o que passar nos Posts de Finalizacao
                ////se bandeira visa ou amex
                //if (paymentForm.paymentFormId == 1 || paymentForm.paymentFormId == 8)
                //{
                //    //post.Parameters.Add("id_trans", agentParams..a); //DPaymentAttemptVBV.tid
                //    //post.Parameters.Add("num_autorizacao", _dOrder.a); //DPaymentAttemptVBV.authent 
                //}
                //else if (GenericHelper.IsAmex(formId))
                //{
                //    //post.Parameters.Add("id_trans", dPaymentAttemptAmex.a);
                //    //post.Parameters.Add("num_autorizacao", dPaymentAttemptAmex.a);
                //}
                //else if (GenericHelper.IsMastercard(formId) || GenericHelper.IsDiners(formId))
                //{
                //    //<input type='hidden' name='num_autorizacao' value=”061662”>
                //    //<input type='hidden' name='comprov_venda' value=”3427654”>
                //    //<input type='hidden' name='nsu_trans' value=”427708”>
                //}
                //else if (GenericHelper.IsBillingPaper(formId)) //se é boleto
                //{
                //    //<!— se pagamento com Boleto Bancário
                //    //<input type='hidden' name='URLBOLETOx' value=''> onde X representa o número da parcela
                //}
                //else if (GenericHelper.IsItauShopLine(formId))
                //{
                //    //<!— se pagamento com Itaú Shopline
                //    //<input type='hidden' name='NUM_AUTORIZACAO' value=”64420536315583322003”>
                //}
                ////else if ( GenericHelper.
                ////<!— se pagamento com Comércio Eletrônico Banco do Brasil
                ////<input type='hidden' name='NUM_AUTORIZACAO' value=”64420536315583322003”>

                ////<!— se pagamento com Freedom2Buy (F2B)
                ////<input type='hidden' name='NUM_F2B_TRANSACTION' value=”64420536315583322003”>

                ////TODO: bradesco, etc

                ////TODO: rodolfo
                ////    <!— se a loja virtual utilizar o congelamento de pedido SmartPag
                ////    <input type='hidden' name='NOME_CLIENTE_CARTAO' value=”Teste e-Financial”>
                ////    <input type='hidden' name='NUMERO_CARTAO' value=”1231231231231231”>
                ////    <!— se o processamento do cartão de crédito é feito pelo lojista (POS)
                ////    <input type='hidden' name='NUMERO_CARTAO' value=”1231231231231231”>
                ////<!— se pagamento com DOC
                ////<input type='hidden' name='BancoDOC' value='”123”>
                ////<input type='hidden' name='AgenciaDOC' value=”1234”>
                ////<input type='hidden' name='ContaDOC' value=”12345678”>

                ////TODO: IF DE OK 
                //TODO:
                //<!— se pagamento com Financiamento ABN
                //<input type='hidden' name='NUMPROPOSTA' value=”15097”>
                //<input type='hidden' name='NUMCONTROLEABN' value=”0004198672”>
                //<input type='hidden' name='BSTATUS' value=”AN”>
                //<input type='hidden' name='STRDESC' value=”Proposta em análise.”>
                #endregion

                GenericHelper.AppendNodeForCollection(xml, "pedido", aditParams);
                XmlNode node = xml.ImportNode(xmlHandshake.SelectSingleNode("/pedido/itens"), true);
                if (node != null) xml.FirstChild.AppendChild(node);
                GenericHelper.AppendNodeForCollection(xml, "dados_cliente", aditParamsConsumer);
                GenericHelper.AppendNodeForCollection(xml, "endereco_cobranca", aditParamsBillingAdd);
                GenericHelper.AppendNodeForCollection(xml, "endereco_entrega", aditParamsDeliveryAdd);
                GenericHelper.AppendNodeForCollection(xml, "dados_pagamento", aditPFParams);

                if (xmlHandshake.SelectSingleNode("/pedido/controle_loja") != null)
                {
                    node = xml.ImportNode(xmlHandshake.SelectSingleNode("/pedido/controle_loja"), true);
                    if (node != null)
                    {
                        XmlElement test = GenericHelper.CopyElementToName((XmlElement)node, "parametros_opcionais");
                        xml.FirstChild.AppendChild(test);
                    }
                }
                GenericHelper.AppendNodeForCollection(xml, xml.SelectSingleNode("/pedido"), aditParamsFim);

                ServerHttpRequisition post = new ServerHttpRequisition();
                post.Method = "POST";
                post.Url = url;
                post.Content = "text/xml";
                post.Data = xml.InnerXml;

                int status;

                bool sent = post.Send();

                if (sent && !ShowFinalization)
                {
                    RedirectInformation redirectinfo = GenericHelper.GetRedirectXml(post.Response.Trim());
                    if (redirectinfo.Return.ToLower() == "ok")
                    {
                        status = (int)PostStatus.Confirmed;
                        GenericHelper.LogFile("EasyPagObject::HelperXml.cs::FinalizationPost.Send storeId=" + _dOrder.storeId.ToString() + " orderId=" + _dOrder.orderId.ToString() + " url=" + post.Url + " enviado=" + post.Data + " recebido=" + post.Response, LogFileEntryType.Information);
                    }
                    else
                    {
                        status = (int)PostStatus.Sent;
                        GenericHelper.LogFile("EasyPagObject::HelperXml.cs::FinalizationPost.Send storeId=" + _dOrder.storeId.ToString() + " orderId=" + _dOrder.orderId.ToString() + " url=" + post.Url + " enviado=" + post.Data + " recebido=" + post.Response, LogFileEntryType.Warning);
                    }

                    if (redirectinfo.Redirect.ToLower() == "ok" && !String.IsNullOrEmpty(redirectinfo.UrlRedirect))
                        GenericHelper.RedirectWindow(redirectinfo.UrlRedirect, false);
                }
                else if (sent && post.Response.Trim().ToLower().Contains("<retorno>ok</retorno>"))
                {
                    status = (int)PostStatus.Confirmed;
                    GenericHelper.LogFile("EasyPagObject::HelperXml.cs::FinalizationPost.Send storeId=" + _dOrder.storeId.ToString() + " orderId=" + _dOrder.orderId.ToString() + " url=" + post.Url + " enviado=" + post.Data + " recebido=" + post.Response.Trim(), LogFileEntryType.Information);
                }
                else if (sent && !post.Response.Trim().ToLower().Contains("<retorno>ok</retorno>"))
                {
                    status = (int)PostStatus.Sent;
                    GenericHelper.LogFile("EasyPagObject::HelperXml.cs::FinalizationPost.Send storeId=" + _dOrder.storeId.ToString() + " orderId=" + _dOrder.orderId.ToString() + " url=" + post.Url + " enviado=" + post.Data + " recebido=" + post.Response.Trim(), LogFileEntryType.Warning);
                }
                else
                {
                    status = (int)PostStatus.Error;
                    GenericHelper.LogFile("EasyPagObject::HelperXml.cs::FinalizationPost.Send storeId=" + _dOrder.storeId.ToString() + " orderId=" + _dOrder.orderId.ToString() + " url=" + post.Url + " enviado=" + post.Data + " msg erro=" + post.Response.Trim(), LogFileEntryType.Error);
                }

                if (status == (int)PostStatus.Confirmed && _dAttempt.status == (int)PaymentAttemptStatus.Paid && !_dHandshakeConfiguration.autoPaymentConfirm)
                {
                    SuperPag.Handshake.Xml.Handshake hand = new SuperPag.Handshake.Xml.Handshake();
                    hand.SendPaymentPost(_dAttempt.paymentAttemptId);
                }

                DServiceFinalizationPost finalizationPost = DataFactory.ServiceFinalizationPost().Locate(_dAttempt.paymentAttemptId);

                if (finalizationPost != null)
                {
                    finalizationPost.postStatus = status;
                    finalizationPost.postRetries += 1;
                    finalizationPost.lastUpdate = DateTime.Now;
                    DataFactory.ServiceFinalizationPost().Update(finalizationPost);
                }
                else
                {
                    DServiceFinalizationPost dServiceFinalizationPost = new DServiceFinalizationPost();
                    dServiceFinalizationPost.paymentAttemptId = _dAttempt.paymentAttemptId;
                    dServiceFinalizationPost.postStatus = status;
                    dServiceFinalizationPost.postRetries = 1;
                    dServiceFinalizationPost.lastUpdate = DateTime.Now;
                    DataFactory.ServiceFinalizationPost().Insert(dServiceFinalizationPost);
                }

                Result = true;
                Error = "";
            }
            catch (Exception e)
            {
                Result = false;
                Error = e.Message;
                GenericHelper.LogFile("EasyPagObject::HelperXml.cs::FinalizationPost.Send storeId=" + _dOrder.storeId.ToString() + " orderId=" + _dOrder.orderId.ToString() + " msg erro=" + e.Message, LogFileEntryType.Error);
            }
        }
    }

    public class PaymentPost
    {
        public bool Result;
        public string Error;
        public bool IsOffLine = false;

        DOrder _dOrder;
        DOrderInstallment[] _dOrderInstallment;
        decimal _shippingRate;
        DPaymentAttempt _dAttempt;
        DHandshakeConfiguration _dHandshakeConfiguration;
        DHandshakeConfigurationXml _dHandshakeConfigurationXml;
        int _installmentNumber;

        public PaymentPost(Guid paymentAttemptId, int installmentNumber)
        {
            try
            {
                //obtem a attempt
                DPaymentAttempt dPaymentAttempt = DataFactory.PaymentAttempt().Locate(paymentAttemptId);
                Ensure.IsNotNull(dPaymentAttempt, "Não há tentativas de pagamento para o pedido");
                _dAttempt = dPaymentAttempt;

                //localiza o pedido
                _dOrder = DataFactory.Order().Locate(dPaymentAttempt.orderId);
                Ensure.IsNotNull(_dOrder, "Pedido {0} não encontrado", dPaymentAttempt.orderId);

                //localiza configuracoes do handshake xml
                _dHandshakeConfiguration = DataFactory.HandshakeConfiguration().Locate(_dOrder.storeId, (int)HandshakeType.XmlSPag10);
                Ensure.IsNotNull(_dHandshakeConfiguration, "Configuração de handshake para a loja {0}  não encontrada.", _dOrder.storeId);
                _dHandshakeConfigurationXml = DataFactory.HandshakeConfigurationXml().Locate(_dHandshakeConfiguration.handshakeConfigurationId);
                Ensure.IsNotNull(_dHandshakeConfigurationXml, "Configuração de handshake xml para a loja {0} não encontrada.", _dOrder.storeId);

                _installmentNumber = installmentNumber;
                if (_dAttempt.installmentNumber != int.MinValue && installmentNumber != int.MinValue && _dAttempt.installmentNumber != installmentNumber)
                    Ensure.IsNotNull(null, "A parcela {0} é inválida para o envio do post.", installmentNumber);

                if (_dAttempt.installmentNumber != int.MinValue || installmentNumber != int.MinValue)
                {
                    installmentNumber = (_dAttempt.installmentNumber != int.MinValue ? _dAttempt.installmentNumber : installmentNumber);
                    DOrderInstallment installment = DataFactory.OrderInstallment().Locate(_dOrder.orderId, installmentNumber);
                    Ensure.IsNotNull(installment, "A Parcela {0} do pedido {1} não foi encontrada", installmentNumber, _dOrder.orderId);
                    _dOrderInstallment = new DOrderInstallment[1];
                    _dOrderInstallment[0] = installment;
                }
                else
                {
                    //enviar post para todas as parcelas
                    _dOrderInstallment = DataFactory.OrderInstallment().List(_dOrder.orderId);
                    Ensure.IsNotNull(_dOrderInstallment, "Parcelas do pedido {0} não encontradas", _dOrder.orderId);
                }

                //calcular o frete total
                DOrderItem[] shippingItens = DataFactory.OrderItem().List(_dOrder.orderId, (int)ItemTypes.ShippingRate);
                _shippingRate = 0;
                if (shippingItens != null)
                {
                    foreach (DOrderItem shippingItem in shippingItens)
                        _shippingRate += shippingItem.itemValue;
                }
            }
            catch (Exception e)
            {
                GenericHelper.LogFile("EasyPagObject::HelperXml.cs::PaymentPost.PaymentPost(Guid, int) " + e.Message, LogFileEntryType.Error);
                throw;
            }
        }

        public void Send()
        {
            #region Exemplo de XML
            //<?xml version="1.0" encoding="ISO-8859-1"?>
            //<POSTPAGTO>
            //     <PEDIDO>40866</PEDIDO>
            //     <NUM_PARCELA>1</NUM_PARCELA>
            //     <FORMA_PAGTO>2</FORMA_PAGTO>
            //     <COD_CONTROLE>5760644</COD_CONTROLE>
            //     <VAL_PARCIAL_PEDIDO>12140</VAL_PARCIAL_PEDIDO>
            //     <VAL_FINAL_PEDIDO>12140</VAL_FINAL_PEDIDO>
            //     <SFRETE>3500</SFRETE>
            //</POSTPAGTO>
            #endregion            

            try
            {
                if (_dHandshakeConfiguration.paymentHtml)
                {
                    SuperPag.Handshake.Html.PaymentPost payment = new SuperPag.Handshake.Html.PaymentPost(_dAttempt.paymentAttemptId, _installmentNumber, true);
                    payment.IsOffLine = IsOffLine;
                    payment.Send();

                    this.Result = payment.Result;
                    this.Error = payment.Error;

                    return;
                }

                string urlHandshakeConfiguration = (IsOffLine ? _dHandshakeConfigurationXml.urlPaymentConfirmationOffline : _dHandshakeConfigurationXml.urlPaymentConfirmation);

                if (_dAttempt.status != (int)PaymentAttemptStatus.Paid)
                    Ensure.IsNotNull(null, "O pedido {0} não está pago", _dAttempt.paymentAttemptId);

                if (String.IsNullOrEmpty(urlHandshakeConfiguration))
                {
                    GenericHelper.LogFile("EasyPagObject::HelperXml.cs::PaymentPost.Send storeId=" + _dOrder.storeId.ToString() + " A loja  não possue url para o post de pagamento " + (IsOffLine ? "offline " : "") + "configurada, o post não será enviado", LogFileEntryType.Information);

                    Result = true;
                    Error = "";
                    return;
                }

                ServerHttpRequisition post = new ServerHttpRequisition();
                post.Url = urlHandshakeConfiguration;
                post.Method = "POST";
                post.Content = "text/xml";

                string xml = "<POSTPAGTO><PEDIDO/><NUM_PARCELA/><FORMA_PAGTO/><COD_CONTROLE/><VAL_PARCELA/><VAL_PARCIAL_PEDIDO/><VAL_FINAL_PEDIDO/><SFRETE/></POSTPAGTO>";

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xml);

                xmlDoc.InsertBefore(xmlDoc.CreateXmlDeclaration("1.0", "ISO-8859-1", null), xmlDoc.FirstChild);

                xmlDoc.SelectSingleNode("//PEDIDO").InnerText = _dOrder.storeReferenceOrder;
                xmlDoc.SelectSingleNode("//FORMA_PAGTO").InnerText = SuperPag.Handshake.Helper.GetSmartPagPaymentForm(_dAttempt.paymentFormId).ToString();
                xmlDoc.SelectSingleNode("//COD_CONTROLE").InnerText = _dAttempt.paymentAttemptId.ToString();
                xmlDoc.SelectSingleNode("//VAL_PARCIAL_PEDIDO").InnerText = _dOrder.totalAmount.ToString();
                xmlDoc.SelectSingleNode("//VAL_FINAL_PEDIDO").InnerText = _dOrder.finalAmount.ToString();
                xmlDoc.SelectSingleNode("//SFRETE").InnerText = _shippingRate.ToString();

                foreach (DOrderInstallment installment in _dOrderInstallment)
                {
                    xmlDoc.SelectSingleNode("//NUM_PARCELA").InnerText = installment.installmentNumber.ToString();
                    xmlDoc.SelectSingleNode("//VAL_PARCELA").InnerText = installment.installmentValue.ToString();

                    post.Data = xmlDoc.OuterXml;

                    int status;

                    bool sent = post.Send();

                    if (sent && post.Response.ToLower().Contains("<retorno>ok</retorno>"))
                    {
                        status = (int)PostStatus.Confirmed;
                        GenericHelper.LogFile("EasyPagObject::HelperXml.cs::PaymentPost.Send storeId=" + _dOrder.storeId.ToString() + " orderId=" + _dOrder.orderId.ToString() + " installmentNumber=" + installment.installmentNumber.ToString() + " url=" + post.Url + " enviado=" + post.Data + " recebido=" + post.Response, LogFileEntryType.Information);
                    }
                    else if (sent && !post.Response.ToLower().Contains("<retorno>ok</retorno>"))
                    {
                        status = (int)PostStatus.Sent;
                        GenericHelper.LogFile("EasyPagObject::HelperXml.cs::PaymentPost.Send storeId=" + _dOrder.storeId.ToString() + " orderId=" + _dOrder.orderId.ToString() + " installmentNumber=" + installment.installmentNumber.ToString() + " url=" + post.Url + " enviado=" + post.Data + " recebido=" + post.Response, LogFileEntryType.Warning);
                    }
                    else
                    {
                        status = (int)PostStatus.Error;
                        GenericHelper.LogFile("EasyPagObject::HelperXml.cs::PaymentPost.Send storeId=" + _dOrder.storeId.ToString() + " orderId=" + _dOrder.orderId.ToString() + " installmentNumber=" + installment.installmentNumber.ToString() + " url=" + post.Url + " enviado=" + post.Data + " msg erro=" + post.Response, LogFileEntryType.Error);

                    }

                    DServicePaymentPost paymentPost = DataFactory.ServicePaymentPost().Locate(_dAttempt.paymentAttemptId, installment.installmentNumber);

                    if (paymentPost != null)
                    {
                        paymentPost.postStatus = status;
                        paymentPost.postRetries += 1;
                        paymentPost.lastUpdate = DateTime.Now;
                        DataFactory.ServicePaymentPost().Update(paymentPost);
                    }
                    else
                    {
                        DServicePaymentPost dServicePaymentPost = new DServicePaymentPost();
                        dServicePaymentPost.paymentAttemptId = _dAttempt.paymentAttemptId;
                        dServicePaymentPost.installmentNumber = installment.installmentNumber;
                        dServicePaymentPost.postStatus = status;
                        dServicePaymentPost.postRetries = 1;
                        dServicePaymentPost.lastUpdate = DateTime.Now;
                        DataFactory.ServicePaymentPost().Insert(dServicePaymentPost);
                    }
                }

                Result = true;
                Error = "";
            }
            catch (Exception e)
            {       
                Result = false;
                Error = e.Message;
                GenericHelper.LogFile("EasyPagObject::HelperXml.cs::PaymentPost.Send storeId=" + _dOrder.storeId.ToString() + " orderId=" + _dOrder.orderId.ToString() + " msg erro=" + e.Message , LogFileEntryType.Error);
            }
        }
    }

    public class FinalizationErrorPost
    {
        public bool Result;
        public string Error;
        public bool IsOffLine = false;

        DOrder _dOrder;
        DOrderInstallment[] _dOrderInstallment;
        DHandshakeSessionLog[] _hsLogs;
        DConsumer _dConsumer;
        DPaymentForm _dPaymentForm;
        DConsumerAddress _dBillingAddress;
        DConsumerAddress _dDeliveryAddress;
        DOrderItem[] _dOrderItemArr;
        decimal _shippingRate;
        DPaymentAttempt _dAttempt;
        DHandshakeConfigurationXml _dHandshakeConfigurationXml;

        public FinalizationErrorPost(Guid paymentAttemptId)
        {
            try
            {
                //obtem a attempt
                DPaymentAttempt dPaymentAttempt = DataFactory.PaymentAttempt().Locate(paymentAttemptId);
                Ensure.IsNotNull(dPaymentAttempt, "Não há tentativas de pagamento para o pedido");
                _dAttempt = dPaymentAttempt;

                //localiza o pedido
                _dOrder = DataFactory.Order().Locate(dPaymentAttempt.orderId);
                Ensure.IsNotNull(_dOrder, "Pedido {0} não encontrado", dPaymentAttempt.orderId);

                //pego a HandshakeSession
                DHandshakeSession[] handshakeSessions = DataFactory.HandshakeSession().List(_dOrder.orderId);
                Ensure.IsNotNull(handshakeSessions, "Sessão de pagamento inválida");

                //pego os logs com os dados do pedido enviado
                DHandshakeSessionLog[] hsLogs = DataFactory.HandshakeSessionLog().List(handshakeSessions[0].handshakeSessionId);
                _hsLogs = hsLogs;

                //localiza a primeira parcelas do pedido
                _dOrderInstallment = DataFactory.OrderInstallment().List(_dOrder.orderId);
                Ensure.IsNotNull(_dOrderInstallment, "Parcelas do pedido {0} não encontradas", _dOrder.orderId);

                //localiza o consumidor
                _dConsumer = DataFactory.Consumer().Locate(_dOrder.consumerId);
                Ensure.IsNotNull(_dConsumer, "Cliente {0} não encontrado", _dConsumer);

                //localiza o forma de pagamento
                _dPaymentForm = DataFactory.PaymentForm().Locate(dPaymentAttempt.paymentFormId);
                Ensure.IsNotNull(_dPaymentForm, "Forma de pagamento {0} não encontrada", dPaymentAttempt.paymentFormId);

                //localiza configuracoes do handshake xml
                DHandshakeConfiguration dHandshakeConfiguration = DataFactory.HandshakeConfiguration().Locate(_dOrder.storeId);
                Ensure.IsNotNull(dHandshakeConfiguration, "Configuração de handshake para a loja {0}  não encontrada.", _dOrder.storeId);
                _dHandshakeConfigurationXml = DataFactory.HandshakeConfigurationXml().Locate(dHandshakeConfiguration.handshakeConfigurationId);
                Ensure.IsNotNull(_dHandshakeConfigurationXml, "Configuração de handshake xml para a loja {0}  não encontrada.", _dOrder.storeId);

                //localiza o endereco de cobranca
                IConsumerAddress iConsumerAddress = DataFactory.ConsumerAddress();
                _dBillingAddress = iConsumerAddress.Locate(_dConsumer.consumerId, (int)AddressTypes.Billing);
                Ensure.IsNotNull(_dBillingAddress, "Endereco de cobrança do cliente {0} não encontrado", _dConsumer.consumerId);

                //localiza o endereco de entrega
                _dDeliveryAddress = iConsumerAddress.Locate(_dConsumer.consumerId, (int)AddressTypes.Delivery);
                Ensure.IsNotNull(_dDeliveryAddress, "Endereco de entrega do cliente {0} não encontrado", _dConsumer.consumerId);

                //localiza os itens do carrinho de compras
                int[] itensTypes = new int[2];
                itensTypes[0] = (int)ItemTypes.Regular;
                itensTypes[1] = (int)ItemTypes.Discount;
                _dOrderItemArr = DataFactory.OrderItem().List(_dOrder.orderId, itensTypes);
                Ensure.IsNotNull(_dOrderItemArr, "Itens do pedido {0} não encontrados", _dOrder.orderId);

                //calcular o frete total
                DOrderItem[] shippingItens = DataFactory.OrderItem().List(_dOrder.orderId, (int)ItemTypes.ShippingRate);
                _shippingRate = 0;
                if (shippingItens != null)
                {
                    foreach (DOrderItem shippingItem in shippingItens)
                        _shippingRate += shippingItem.itemValue;
                }
            }
            catch (Exception e)
            {
                GenericHelper.LogFile("EasyPagObject::HelperXml.cs::FinalizationErrorPost.FinalizationErrorPost(Guid) " + e.Message, LogFileEntryType.Error);
                throw;
            }
        }

        public void Send()
        {
            try
            {
                string xmlData = "", url = "", urlHandshake = "", urlHandshakeConfiguration = "";
                foreach (DHandshakeSessionLog hsLog in _hsLogs)
                {
                    if (hsLog.step == 3)
                        xmlData = hsLog.xmlData;
                }

                urlHandshake = (IsOffLine ? "" : GenericHelper.GetSingleNodeString(xmlData, "//post_final"));
                urlHandshakeConfiguration = (IsOffLine ? _dHandshakeConfigurationXml.urlFinalizationOffline : _dHandshakeConfigurationXml.urlFinalization);

                if (String.IsNullOrEmpty(urlHandshake) && String.IsNullOrEmpty(urlHandshakeConfiguration))
                {
                    GenericHelper.LogFile("EasyPagObject::HelperXml.cs::FinalizationErrorPost.Send A loja " + _dOrder.storeId.ToString() + " não possue url para o post de finalização " + (IsOffLine ? " offline " : "") + "configurada, o post não será enviado", LogFileEntryType.Information);

                    Result = true;
                    Error = "";
                    return;
                }

                if (!String.IsNullOrEmpty(urlHandshake))
                    url = urlHandshake;
                else if (!String.IsNullOrEmpty(urlHandshakeConfiguration))
                    url = urlHandshakeConfiguration;

                //pega o XML da handshakeSession
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(HttpContext.Current.Session["xmlHandshake"].ToString());

                //adiciona parametros de finalizacao
                NameValueCollection aditParams = new NameValueCollection();
                NameValueCollection aditPFParams = new NameValueCollection();
                aditParams.Add("FORMA_PAGTO", System.Web.HttpUtility.HtmlEncode(_dPaymentForm.name));
                aditParams.Add("VAL_PARCIAL_PEDIDO", GenericHelper.ParseString(_dOrder.totalAmount));
                aditParams.Add("VAL_JUROS_DESCONTO", GenericHelper.ParseString(_dOrderInstallment[0].interestPercentage));
                aditParams.Add("VAL_FINAL_PEDIDO", GenericHelper.ParseString(_dOrder.finalAmount));
                aditParams.Add("QTD_PARCELAS", _dOrder.installmentQuantity.ToString());
                aditParams.Add("VAL_PARCELA", GenericHelper.ParseString(_dOrderInstallment[0].installmentValue));
                aditParams.Add("CODE_RET", "0");
                aditParams.Add("DES_RET", "SUPER PAG OK");
                aditParams.Add("data_trans", _dOrder.creationDate.ToString("yyyyMMdd"));
                aditParams.Add("hora_trans", _dOrder.creationDate.ToShortTimeString());
                aditParams.Add("COD_CONTROLE", _dOrder.orderId.ToString());

                DPaymentForm paymentForm = DataFactory.PaymentForm().Locate(_dAttempt.paymentFormId);
                Ensure.IsNotNullPage(paymentForm, "Meio de pagamento inválido");

                DPaymentAttempt paymentAttempt = DataFactory.PaymentAttempt().Locate(new Guid(HttpContext.Current.Session["paymentAttemptId"].ToString()));
                Ensure.IsNotNullPage(paymentAttempt, "Tentativa de pagamento inválida");

                if (paymentForm.paymentFormGroupId == (int)PaymentGroups.CreditCard)
                {
                    aditParams.Add("CODIGO_MEIO_PAGAMENTO", "2");
                    aditParams.Add("BANDEIRA", paymentForm.paymentFormId.ToString());

                    if (paymentForm.paymentFormId == (int)PaymentForms.VisaVBV ||
                        paymentForm.paymentFormId == (int)PaymentForms.VisaVBVInBox ||
                        paymentForm.paymentFormId == (int)PaymentForms.VisaVBV3 ||
                        paymentForm.paymentFormId == (int)PaymentForms.VisaSitef)
                    {
                        DPaymentAttemptVBV vbv = DataFactory.PaymentAttemptVBV().Locate(new Guid(HttpContext.Current.Session["paymentAttemptId"].ToString()));
                        aditPFParams.Add("ID_TRANS", vbv.tid);
                        aditPFParams.Add("NUM_AUTORIZACAO", vbv.arp.ToString());
                    }
                }
                else
                    aditParams.Add("CODIGO_MEIO_PAGAMENTO", paymentForm.paymentFormId.ToString());

                if (paymentForm.paymentFormId == (int)PaymentForms.BoletoBancoDoBrasil)
                {
                    string serverUrl = "";
                    if (HttpContext.Current != null)
                    {
                        string http = (HttpContext.Current.Request.ServerVariables["HTTPS"] == "off" ? "http" : "https");
                        string server = HttpContext.Current.Request.ServerVariables["SERVER_NAME"];
                        serverUrl = String.Format("{0}://{1}", http, server);
                    }
                    else if (ConfigurationManager.AppSettings != null && ConfigurationManager.AppSettings["ServerUrl"] != null)
                        serverUrl = ConfigurationManager.AppSettings["ServerUrl"];

                    aditPFParams.Add("urlboleto", String.Format("{0}/Agents/BoletoBB/showboleto.aspx?id={1}", serverUrl, _dAttempt.paymentAttemptId));
                }

                GenericHelper.AppendNodeForCollection(xmlDoc, xmlDoc.SelectSingleNode("/pedido"), aditParams);
                GenericHelper.AppendNodeForCollection(xmlDoc, xmlDoc.CreateElement("dados_pagamento"), aditPFParams);

                ServerHttpRequisition post = new ServerHttpRequisition();
                post.Method = "POST";
                post.Url = url;
                post.Content = "text/xml";
                post.Data = xmlDoc.InnerXml;

                bool sent = post.Send();
                int status;
                if (sent && post.Response == "<retorno>ok</retorno>")
                {
                    status = (int)PostStatus.Confirmed;
                    GenericHelper.LogFile("EasyPagObject::HelperXml.cs::FinalizationErrorPost.Send storeId=" + _dOrder.storeId.ToString() + " orderId=" + _dOrder.orderId.ToString() + " url=" + post.Url + " enviado=" + post.Data + " recebido=" + post.Response, LogFileEntryType.Information);
                }
                else if (sent && post.Response != "<retorno>ok</retorno>")
                {
                    status = (int)PostStatus.Sent;
                    GenericHelper.LogFile("EasyPagObject::HelperXml.cs::FinalizationErrorPost.Send storeId=" + _dOrder.storeId.ToString() + " orderId=" + _dOrder.orderId.ToString() + " url=" + post.Url + " enviado=" + post.Data + " recebido=" + post.Response, LogFileEntryType.Warning);
                }
                else
                {
                    status = (int)PostStatus.Error;
                    GenericHelper.LogFile("EasyPagObject::HelperXml.cs::FinalizationErrorPost.Send storeId=" + _dOrder.storeId.ToString() + " orderId=" + _dOrder.orderId.ToString() + " url=" + post.Url + " enviado=" + post.Data + " msg erro=" + post.Response, LogFileEntryType.Error);
                }

                Result = true;
                Error = "";
            }
            catch (Exception e)
            {
                Result = false;
                Error = e.Message;
                GenericHelper.LogFile("EasyPagObject::HelperXml.cs::FinalizationErrorPost.Send storeId=" + _dOrder.storeId.ToString() + " orderId=" + _dOrder.orderId.ToString() + " msg erro=" + e.Message, LogFileEntryType.Error);
            }
        }
    }
}
