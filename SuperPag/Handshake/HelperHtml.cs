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

namespace SuperPag.Handshake.Html
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

        public string CreateXmlForRequest(NameValueCollection col)
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlNode rootNode = xmlDoc.CreateElement("root");
            XmlNode formNode = xmlDoc.CreateElement("form");
            XmlNode queryStringNode = xmlDoc.CreateElement("queryString");

            GenericHelper.AppendNodeForCollection(xmlDoc, formNode, col);
            GenericHelper.AppendNodeForCollection(xmlDoc, queryStringNode, HttpContext.Current.Request.QueryString);
            rootNode.AppendChild(formNode);
            rootNode.AppendChild(queryStringNode);
            xmlDoc.AppendChild(rootNode);

            return xmlDoc.InnerXml;
        }

        public void ParseHtml(XmlDocument xmlDoc)
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

            //cria o item de frete
            CreateShippingRateItem(xmlDoc, orderId);

            //cria o consumer
            long consumerId = CreateConsumer(xmlDoc, orderId);

            //cria o address
            CreateAddress(xmlDoc, consumerId);

            //verifica se é simulacao
            string simulacao = GenericHelper.GetSingleNodeString(xmlDoc, "/root/form/simulacao");
            if (simulacao.Trim() == "1")
                HttpContext.Current.Session["isSimulation"] = true;

            string ppagamento = GenericHelper.GetSingleNodeString(xmlDoc, "/root/form/ppagamento");
            string bandeira = GenericHelper.GetSingleNodeString(xmlDoc, "/root/form/bandeira");
            string pqtdparcelas = GenericHelper.GetSingleNodeString(xmlDoc, "/root/form/pqtdparcelas");

            GenericHelper.SetCanChoosePaymentFormSession(HttpContext.Current, true);

            int handshakePaymentFormId, handshakeCardFlag;
            if (int.TryParse(ppagamento, out handshakePaymentFormId))
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
        }

        public static void CreateAddress(XmlDocument xmlDoc, long consumerId)
        {
            //cria o endereco de cobranca
            DConsumerAddress dConsumerAddress = new DConsumerAddress();
            dConsumerAddress.address = GenericHelper.GetSingleNodeString(xmlDoc,"/root/form/endereco");
            dConsumerAddress.addressComplement = GenericHelper.GetSingleNodeString(xmlDoc,"/root/form/complemento");
            dConsumerAddress.addressNumber = GenericHelper.GetSingleNodeString(xmlDoc,"/root/form/numero");
            dConsumerAddress.addressType = (int)AddressTypes.Billing;
            dConsumerAddress.cep = GenericHelper.GetSingleNodeString(xmlDoc,"/root/form/cep");
            dConsumerAddress.city = GenericHelper.GetSingleNodeString(xmlDoc,"/root/form/cidade");
            dConsumerAddress.consumerAddressId = long.MinValue;
            dConsumerAddress.consumerId = consumerId;
            dConsumerAddress.country = GenericHelper.GetSingleNodeString(xmlDoc,"/root/form/pais");
            dConsumerAddress.district = GenericHelper.GetSingleNodeString(xmlDoc, "/root/form/bairro");
            dConsumerAddress.logradouro = GenericHelper.GetSingleNodeString(xmlDoc,"/root/form/logradouro");
            dConsumerAddress.state = GenericHelper.GetSingleNodeString(xmlDoc,"/root/form/estado");

            //insere o endereco de cobranca
            DataFactory.ConsumerAddress().Insert(dConsumerAddress);

            //cria o endereco de entrega
            dConsumerAddress = new DConsumerAddress();
            dConsumerAddress.address = GenericHelper.GetSingleNodeString(xmlDoc,"/root/form/endereco_d");
            dConsumerAddress.addressComplement = GenericHelper.GetSingleNodeString(xmlDoc,"/root/form/complemento_d");
            dConsumerAddress.addressNumber = GenericHelper.GetSingleNodeString(xmlDoc,"/root/form/numero_d");
            dConsumerAddress.addressType = (int)AddressTypes.Delivery;
            dConsumerAddress.cep = GenericHelper.GetSingleNodeString(xmlDoc,"/root/form/cep_d");
            dConsumerAddress.city = GenericHelper.GetSingleNodeString(xmlDoc,"/root/form/cidade_d");
            dConsumerAddress.consumerAddressId = long.MinValue;
            dConsumerAddress.consumerId = consumerId;
            dConsumerAddress.country = GenericHelper.GetSingleNodeString(xmlDoc,"/root/form/pais_d");
            dConsumerAddress.district = GenericHelper.GetSingleNodeString(xmlDoc, "/root/form/bairro_d");
            dConsumerAddress.logradouro = GenericHelper.GetSingleNodeString(xmlDoc,"/root/form/logradouro_d");
            dConsumerAddress.state = GenericHelper.GetSingleNodeString(xmlDoc,"/root/form/estado_d");

            //insere o endereco de entrega
            DataFactory.ConsumerAddress().Insert(dConsumerAddress);
        }

        public static long CreateConsumer(XmlDocument xmlDoc, long orderId)
        {
            DConsumer dConsumer = new DConsumer();
            dConsumer.birthDate = GenericHelper.ParseDateddMMyyyy(GenericHelper.GetSingleNodeString(xmlDoc, "/root/form/nasc" ));
            dConsumer.celularPhone = "";
            dConsumer.civilState = GenericHelper.GetSingleNodeString(xmlDoc, "/root/form/estcivil" );
            dConsumer.CNPJ = GenericHelper.GetSingleNodeString(xmlDoc,"/root/form/cgc");
            dConsumer.commercialPhone = "";
            dConsumer.consumerId = long.MinValue;
            dConsumer.CPF = GenericHelper.GetSingleNodeString(xmlDoc,"/root/form/cpf");
            dConsumer.email = GenericHelper.GetSingleNodeString(xmlDoc,"/root/form/email");
            dConsumer.fax = GenericHelper.GetSingleNodeString(xmlDoc,"/root/form/fax");
            dConsumer.ger = GenericHelper.GetSingleNodeString(xmlDoc, "/root/form/sexo");
            dConsumer.IE = GenericHelper.GetSingleNodeString(xmlDoc, "/root/form/inscricao_pj");
            dConsumer.name = GenericHelper.GetSingleNodeString(xmlDoc,"/root/form/nome");
            if (dConsumer.name == "" || dConsumer.name == null)
                dConsumer.name = GenericHelper.GetSingleNodeString(xmlDoc,"/root/form/razao_pj");
            dConsumer.occupation = GenericHelper.GetSingleNodeString(xmlDoc,"/root/form/prof");
            dConsumer.phone = GenericHelper.GetSingleNodeString(xmlDoc,"/root/form/fone");
            dConsumer.responsibleCPF = GenericHelper.GetSingleNodeString(xmlDoc, "/root/form/cpf_resp_pj");
            dConsumer.responsibleName = GenericHelper.GetSingleNodeString(xmlDoc, "/root/form/nome_resp_pj");
            dConsumer.RG = "";
            dConsumer.TruncateStringFields();

            //insere o consumidor
            DataFactory.Consumer().Insert(dConsumer);

            //vincula consumidor com a order
            DataFactory.Order().Update(dConsumer.consumerId, orderId);

            return dConsumer.consumerId;
        }

        public static void CreateShippingRateItem(XmlDocument xmlDoc, long orderId)
        {
            if (GenericHelper.GetSingleNodeString(xmlDoc,"root/form/sfrete") != null &&
                GenericHelper.GetSingleNodeString(xmlDoc,"root/form/sfrete") != "")
            {
                decimal freteValue = GenericHelper.ParseDecimal(GenericHelper.GetSingleNodeString(xmlDoc,"root/form/sfrete"));
                DOrderItem dOrderItem = new DOrderItem();
                dOrderItem.itemCode = "";
                dOrderItem.itemDescription = "";
                dOrderItem.itemNumber = 0;
                dOrderItem.itemQuantity = 1;
                dOrderItem.itemValue = freteValue;
                dOrderItem.itemType = (int)ItemTypes.ShippingRate;
                dOrderItem.orderId = orderId;
                dOrderItem.orderItemId = long.MinValue;
                DataFactory.OrderItem().Insert(dOrderItem);
            }
        }

        public void CreateOrderItens(XmlDocument xmlDoc, long orderId)
        {
            //obtem os itens do pedido        
            //extrai a lista de nos que representam itens de pedido
            List<XmlNode> itemList = ExtractItensNode(xmlDoc);

            //inicializa uma lista de pedidos
            List<DOrderItem> dOrderItemList = new List<DOrderItem>();

            //para cada nó de pedido
            foreach (XmlNode item in itemList)
            {
                //obtem o numero do item
                int number = int.Parse(item.Name.Substring(4));

                bool founded = false;

                //seta a propriedade dentro do array
                foreach (DOrderItem dOrderItem in dOrderItemList)
                {
                    if (dOrderItem.itemNumber == number)
                    {
                        SetItemProperty(item, dOrderItem);
                        founded = true;
                        break;
                    }
                }

                if (!founded)
                {
                    DOrderItem dOrderItem = new DOrderItem();
                    dOrderItem.itemNumber = number;
                    dOrderItem.orderId = orderId;
                    dOrderItem.orderItemId = int.MinValue;
                    SetItemProperty(item, dOrderItem);
                    dOrderItem.itemType = (int)ItemTypes.Regular;
                    dOrderItemList.Add(dOrderItem);
                }
            }

            foreach (DOrderItem dOrderItem in dOrderItemList)
            {
                if(dOrderItem.itemValue < 0)
                    dOrderItem.itemType = (int)ItemTypes.Discount;

                DataFactory.OrderItem().Insert(dOrderItem);
            }
        }

        public long CreateOrder(XmlDocument xmlDoc)
        {
            DOrder dOrder = new DOrder();
            dOrder.orderId = long.MinValue;
            dOrder.storeId = int.Parse(HttpContext.Current.Session["storeId"].ToString());
            dOrder.consumerId = long.MinValue;
            dOrder.storeReferenceOrder = HttpContext.Current.Session["storeReferenceOrder"].ToString();
            dOrder.totalAmount = GenericHelper.ParseDecimal(GenericHelper.GetSingleNodeString(xmlDoc, "root/form/spv"));
            dOrder.finalAmount = decimal.MinValue;
            dOrder.installmentQuantity = int.MinValue;
            dOrder.creationDate = DateTime.Now;
            dOrder.lastUpdateDate = DateTime.Now;
            dOrder.status = (int)OrderStatus.Unfinished;

            DataFactory.Order().Insert(dOrder);

            return dOrder.orderId;
        }

        public void SetItemProperty(XmlNode node, DOrderItem item)
        {
            if (node == null) return;

            string startNodeName = node.Name.Substring(0, 4);

            switch (startNodeName)
            {
                case "qtd_":
                    item.itemQuantity = GenericHelper.ParseInt(node.InnerText);
                    break;
                case "cod_":
                    item.itemCode = GenericHelper.ParseString(node.InnerText);
                    break;
                case "des_":
                    item.itemDescription = GenericHelper.ParseString(node.InnerText);
                    break;
                case "val_":
                    item.itemValue = GenericHelper.ParseDecimal(node.InnerText);
                    break;
            }
        }

        public List<XmlNode> ExtractItensNode(XmlDocument xmlDoc)
        {
            List<XmlNode> itensList = new List<XmlNode>();
            XmlNodeList nodes = xmlDoc.SelectNodes("/root/form/*");
            foreach (XmlNode node in nodes)
            {
                string nodeName = node.Name;

                if (nodeName.Length >= 4 )
                {
                    //TODO: alteracao para editora abril q envia um parametro chamado cod_site
                    // e nao deve ser tratado como item
                    string startNodeName = nodeName.Substring(0, 4).ToUpper();
                    string endNodeName = node.Name.Substring(4, node.Name.Length - 4);
                    int itemNumber = 0;

                    if ((startNodeName == "QTD_" ||
                        startNodeName == "COD_" ||
                        startNodeName == "DES_" ||
                        startNodeName == "VAL_") &&
                        Int32.TryParse(endNodeName, out itemNumber))
                    {
                        itensList.Add(node);
                    }
                }
            }
            return itensList;
        }
    }

    public class FinalizationPost
    {
        public bool Result;
        public string Error;
        public bool ConfigurationIsXml = false;
        public bool IsOffLine = false;

        DOrder _dOrder;
        DStore _dStore;
        DOrderInstallment[] _dOrderInstallment;
        DHandshakeSessionLog[] _hsLogs;
        DConsumer _dConsumer;
        DPaymentForm _dPaymentForm;
        DConsumerAddress _dBillingAddress;
        DConsumerAddress _dDeliveryAddress;
        DOrderItem[] _dOrderItemArr;
        decimal _shippingRate;
        DPaymentAttempt _dAttempt;
        DHandshakeConfigurationHtml _dHandshakeConfigurationHtml;
        DHandshakeConfigurationXml _dHandshakeConfigurationXml;
        Helper helper;

        public FinalizationPost(Guid paymentAttemptId, bool confIsXml)
        {
            ConfigurationIsXml = confIsXml;
            LoadData(paymentAttemptId);
        }

        public FinalizationPost(Guid paymentAttemptId)
        {
            LoadData(paymentAttemptId);
        }

        private void LoadData(Guid paymentAttemptId)
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

                //localiza o pedido
                _dStore = DataFactory.Store().Locate(_dOrder.storeId);
                Ensure.IsNotNull(_dStore, "Loja {0} não encontrada", _dOrder.storeId);

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

                if (ConfigurationIsXml)
                {
                    //localiza configuracoes do handshake xml
                    DHandshakeConfiguration dHandshakeConfiguration = DataFactory.HandshakeConfiguration().Locate(_dOrder.storeId, (int)HandshakeType.XmlSPag10);
                    Ensure.IsNotNull(dHandshakeConfiguration, "Configuração de handshake para a loja {0}  não encontrada.", _dOrder.storeId);
                    _dHandshakeConfigurationXml = DataFactory.HandshakeConfigurationXml().Locate(dHandshakeConfiguration.handshakeConfigurationId);
                    Ensure.IsNotNull(_dHandshakeConfigurationXml, "Configuração de handshake xml para a loja {0} não encontrada.", _dOrder.storeId);
                }
                else
                {
                    //localiza configuracoes do handshake html
                    DHandshakeConfiguration dHandshakeConfiguration = DataFactory.HandshakeConfiguration().Locate(_dOrder.storeId, (int)HandshakeType.HtmlSPag10);
                    Ensure.IsNotNull(dHandshakeConfiguration, "Configuração de handshake para a loja {0}  não encontrada.", _dOrder.storeId);
                    _dHandshakeConfigurationHtml = DataFactory.HandshakeConfigurationHtml().Locate(dHandshakeConfiguration.handshakeConfigurationId);
                    Ensure.IsNotNull(_dHandshakeConfigurationHtml, "Configuração de handshake html para a loja {0} não encontrada.", _dOrder.storeId);
                }

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
                GenericHelper.LogFile("EasyPagObject::HelperHtml.cs::FinalizationPost.FinalizationPost(Guid) " + e.Message, LogFileEntryType.Error);
                throw;
            }
        }

        public void Send()
        {
            try
            {
                //procuro os dados de envio do pedido para pegar parametros enviados
                string xmlData = "", url = "", urlHandshake = "", urlHandshakeConfiguration = "";
                foreach (DHandshakeSessionLog hsLog in _hsLogs)
                {
                    if (hsLog.step == 3)
                        xmlData = hsLog.xmlData;
                }

                urlHandshake = (IsOffLine ? "" : GenericHelper.GetSingleNodeString(xmlData, (ConfigurationIsXml ? "//post_final" : "//urlpostloja")));
                urlHandshakeConfiguration = (ConfigurationIsXml ? (IsOffLine ? _dHandshakeConfigurationXml.urlFinalizationOffline : _dHandshakeConfigurationXml.urlFinalization) : (IsOffLine ? _dHandshakeConfigurationHtml.urlFinalizationOffline : _dHandshakeConfigurationHtml.urlFinalization));

                if (String.IsNullOrEmpty(urlHandshake) && String.IsNullOrEmpty(urlHandshakeConfiguration))
                {
                    GenericHelper.LogFile("EasyPagObject::HelperHtml.cs::FinalizationPost.Send A loja " + _dOrder.storeId.ToString() + " não possue url para o post de finalização configurada, o post não será enviado", LogFileEntryType.Information);

                    Result = true;
                    Error = "";
                    return;
                }

                if (!String.IsNullOrEmpty(urlHandshake))
                    url = urlHandshake;
                else if (!String.IsNullOrEmpty(urlHandshakeConfiguration))
                    url = urlHandshakeConfiguration;

                helper = new Helper();
                ServerHttpHtmlRequisition post = new ServerHttpHtmlRequisition();
                post.Method = "POST";
                post.Url = url;

                post.Parameters.Add("nome", _dConsumer.name);
                //se pessoa juridica
                if (_dConsumer.CNPJ != null && _dConsumer.CNPJ != string.Empty)
                {
                    post.Parameters.Add("cnpj", _dConsumer.CNPJ);
                    post.Parameters.Add("razao_pj", _dConsumer.phone);
                }
                else
                {
                    post.Parameters.Add("cpf", _dConsumer.CPF);
                    post.Parameters.Add("fone", _dConsumer.phone);
                }
                post.Parameters.Add("email", _dConsumer.email);

                //endereco de cobranca
                post.Parameters.Add("logradouro", _dBillingAddress.logradouro);
                post.Parameters.Add("endereco", _dBillingAddress.address);
                post.Parameters.Add("numero", _dBillingAddress.addressNumber);
                post.Parameters.Add("complemento", _dBillingAddress.addressComplement);
                post.Parameters.Add("bairro", _dBillingAddress.district);
                post.Parameters.Add("cidade", _dBillingAddress.city);
                post.Parameters.Add("cep", _dBillingAddress.cep);
                post.Parameters.Add("estado", _dBillingAddress.state.Trim());
                post.Parameters.Add("pais", _dBillingAddress.country);

                //endereco de entrega
                post.Parameters.Add("logradouro_d", _dDeliveryAddress.logradouro);
                post.Parameters.Add("endereco_d", _dDeliveryAddress.address);
                post.Parameters.Add("numero_d", _dDeliveryAddress.addressNumber);
                post.Parameters.Add("complemento_d", _dDeliveryAddress.addressComplement);
                post.Parameters.Add("bairro_d", _dDeliveryAddress.district);
                post.Parameters.Add("cidade_d", _dDeliveryAddress.city);
                post.Parameters.Add("cep_d", _dDeliveryAddress.cep);
                post.Parameters.Add("estado_d", _dDeliveryAddress.state.Trim());
                post.Parameters.Add("pais_d", _dDeliveryAddress.country);

                //carinho de compras
                post.Parameters.Add("SITEM", _dOrderItemArr.Length.ToString());
                int i = 1;
                foreach (DOrderItem dOrderItem in _dOrderItemArr)
                {
                    post.Parameters.Add(string.Format("COD_{0}", i), dOrderItem.itemCode.ToString());
                    post.Parameters.Add(string.Format("DES_{0}", i), HttpUtility.HtmlEncode(dOrderItem.itemDescription));
                    post.Parameters.Add(string.Format("QTD_{0}", i), dOrderItem.itemQuantity.ToString());
                    post.Parameters.Add(string.Format("VAL_{0}", i), GenericHelper.ParseString(dOrderItem.itemValue));
                    post.Parameters.Add(string.Format("TOTAL_ITEM_{0}", i), GenericHelper.ParseString(dOrderItem.itemValue * dOrderItem.itemQuantity));
                    i++;
                }

                //valores e forma de pagamento do pedido
                post.Parameters.Add("91D4C3128BF7DA7F", _dOrder.storeReferenceOrder);
                post.Parameters.Add("FORMA_PAGTO", SuperPag.Handshake.Helper.GetSmartPagPaymentFormName(_dPaymentForm.paymentFormId));
                post.Parameters.Add("VAL_PARCIAL_PEDIDO", GenericHelper.ParseString(_dOrder.totalAmount));
                post.Parameters.Add("SFRETE", GenericHelper.ParseString(_shippingRate));
                post.Parameters.Add("VAL_JUROS_DESCONTO", GenericHelper.ParseString(_dOrderInstallment[0].interestPercentage));
                post.Parameters.Add("VAL_FINAL_PEDIDO", GenericHelper.ParseString(_dOrder.finalAmount));
                post.Parameters.Add("QTD_PARCELAS", _dOrder.installmentQuantity.ToString());
                post.Parameters.Add("VAL_PARCELA", GenericHelper.ParseString(_dOrderInstallment[0].installmentValue));

                //parâmetros de controle do EasyPag
                post.Parameters.Add("CODE_RET", "0");
                post.Parameters.Add("DES_RET", "SUPER PAG OK");
                post.Parameters.Add("data_trans", _dOrder.creationDate.ToString("yyyyMMdd"));
                post.Parameters.Add("hora_trans", _dOrder.creationDate.ToString("HHmmss"));
                post.Parameters.Add("COD_CONTROLE", _dAttempt.paymentAttemptId.ToString());

                DPaymentForm paymentForm = DataFactory.PaymentForm().Locate(_dAttempt.paymentFormId);
                Ensure.IsNotNullPage(paymentForm, "Meio de pagamento inválido");

                post.Parameters.Add("INT_CODIGO_MEIO_PAGAMENTO", SuperPag.Handshake.Helper.GetSmartPagPaymentForm(paymentForm.paymentFormId).ToString());

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
                            post.Parameters.Add(String.Format("NUMERO_OCT{0}", installment.installmentNumber), boleto.oct);
                            post.Parameters.Add(String.Format("URLBOLETO{0}", installment.installmentNumber), String.Format("{0}/Agents/Boleto/showboleto.aspx?id={1}", serverUrl, attempt.paymentAttemptId));
                        }
                        break;
                    case (int)PaymentAgents.VBV:
                    case (int)PaymentAgents.VBVInBox:
                    case (int)PaymentAgents.VBV3:
                        DPaymentAttemptVBV vbv = DataFactory.PaymentAttemptVBV().Locate(_dAttempt.paymentAttemptId);
                        post.Parameters.Add("BANDEIRA", SuperPag.Handshake.Helper.GetSmartPagCreditCardFlag(paymentForm.paymentFormId).ToString());
                        post.Parameters.Add("ID_TRANS", vbv.tid);
                        post.Parameters.Add("NUM_AUTORIZACAO", vbv.arp.ToString());
                        break;
                    case (int)PaymentAgents.VisaMoset:
                    case (int)PaymentAgents.VisaMoset3:
                        DPaymentAttemptMoset moset = DataFactory.PaymentAttemptMoset().Locate(_dAttempt.paymentAttemptId);
                        post.Parameters.Add("BANDEIRA", SuperPag.Handshake.Helper.GetSmartPagCreditCardFlag(paymentForm.paymentFormId).ToString());
                        post.Parameters.Add("ID_TRANS", moset.tid);
                        break;
                    case (int)PaymentAgents.Komerci:
                    case (int)PaymentAgents.KomerciInBox:
                        DPaymentAttemptKomerci komerci = DataFactory.PaymentAttemptKomerci().Locate(_dAttempt.paymentAttemptId);
                        post.Parameters.Add("BANDEIRA", SuperPag.Handshake.Helper.GetSmartPagCreditCardFlag(paymentForm.paymentFormId).ToString());
                        post.Parameters.Add("COMPROV_VENDA", komerci.numautent);
                        post.Parameters.Add("NUM_AUTORIZACAO", komerci.numautor);
                        post.Parameters.Add("NSU_TRANS", komerci.numautent.ToString());
                        post.Parameters.Add("NUMERO_CARTAO", komerci.nr_cartao);
                        break;
                    case (int)PaymentAgents.KomerciWS:
                        DPaymentAttemptKomerciWS komerciws = DataFactory.PaymentAttemptKomerciWS().Locate(_dAttempt.paymentAttemptId);
                        post.Parameters.Add("BANDEIRA", SuperPag.Handshake.Helper.GetSmartPagCreditCardFlag(paymentForm.paymentFormId).ToString());
                        post.Parameters.Add("COMPROV_VENDA", komerciws.numautent);
                        post.Parameters.Add("NUM_AUTORIZACAO", komerciws.numautor);
                        post.Parameters.Add("NSU_TRANS", komerciws.numautent.ToString());
                        post.Parameters.Add("NUMERO_CARTAO", "");
                        break;
                    case (int)PaymentAgents.PaymentClientVirtual2Party:
                    case (int)PaymentAgents.PaymentClientVirtual3Party:
                        DPaymentAttemptPaymentClientVirtual pcv = DataFactory.PaymentAttemptPaymentClientVirtual().Locate(_dAttempt.paymentAttemptId);
                        post.Parameters.Add("BANDEIRA", SuperPag.Handshake.Helper.GetSmartPagCreditCardFlag(paymentForm.paymentFormId).ToString());
                        post.Parameters.Add("ID_TRANS", pcv.vpc_TransactionNo.ToString());
                        post.Parameters.Add("NUM_AUTORIZACAO", pcv.vpc_AuthorizeId.ToString());
                        break;
                    case (int)PaymentAgents.ItauShopLine:
                        DPaymentAttemptItauShopline itauShopline = DataFactory.PaymentAttemptItauShopline().Locate(_dAttempt.paymentAttemptId);
                        post.Parameters.Add("NUM_AUTORIZACAO", (_dAttempt.status == (int)PaymentAttemptStatus.Paid ? "00" : "01"));
                        post.Parameters.Add("PEDIDO_SHOPLINE", itauShopline.agentOrderReference.ToString());
                        break;
                    case (int)PaymentAgents.BBPag:
                        DPaymentAttemptBB bbpag = DataFactory.PaymentAttemptBB().Locate(_dAttempt.paymentAttemptId);
                        post.Parameters.Add("NUM_AUTORIZACAO", bbpag.agentOrderReference.ToString());
                        break;
                    case (int)PaymentAgents.FinanciamentoABN:
                        DPaymentAttemptABN abn = DataFactory.PaymentAttemptABN().Locate(_dAttempt.paymentAttemptId);
                        post.Parameters.Add("NumProposta", abn.numProposta);
                        post.Parameters.Add("NumControleABN", abn.numControle.ToString());
                        post.Parameters.Add("bStatus", abn.statusProposta);
                        post.Parameters.Add("strDesc", abn.msgRet);
                        break;
                }

                //parametros adicionais
                post.Parameters.Add("PARAM_OP1_PED", GenericHelper.GetSingleNodeString(xmlData, "//param_op1_ped"));
                post.Parameters.Add("PARAM_OP2_PED", GenericHelper.GetSingleNodeString(xmlData, "//param_op2_ped"));
                post.Parameters.Add("PARAM_OP3_PED", GenericHelper.GetSingleNodeString(xmlData, "//param_op3_ped"));

                post.Parameters.Add("OrderId", _dOrder.orderId.ToString());

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

                int status;

                if (post.Send())
                {
                    status = (int)PostStatus.Sent;
                    GenericHelper.LogFile("EasyPagObject::HelperHtml.cs::FinalizationPost.Send storeId=" + _dOrder.storeId.ToString() + " orderId=" + _dOrder.orderId.ToString() + " url=" + post.Url + " enviado=" + post.LastSentData + " recebido=" + post.Response, LogFileEntryType.Information);
                }
                else
                {
                    status = (int)PostStatus.Error;
                    GenericHelper.LogFile("EasyPagObject::HelperHtml.cs::FinalizationPost.Send storeId=" + _dOrder.storeId.ToString() + " orderId=" + _dOrder.orderId.ToString() + " url=" + post.Url + " enviado=" + post.LastSentData + " msg erro=" + post.Response, LogFileEntryType.Error);
                }

                DServiceFinalizationPost finalizationPost = DataFactory.ServiceFinalizationPost().Locate(_dAttempt.paymentAttemptId);

                if (finalizationPost != null)
                {
                    finalizationPost.postStatus = (finalizationPost.postStatus != (int)PostStatus.Confirmed ? status : (int)PostStatus.Confirmed);
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
                GenericHelper.LogFile("EasyPagObject::HelperHtml.cs::FinalizationPost.Send storeId=" + _dOrder.storeId.ToString() + " orderId=" + _dOrder.orderId.ToString() + " msg erro=" + e.Message, LogFileEntryType.Error);
            }
        }
        
        public void SendClient()
        {
            try
            {
                //procuro os dados de envio do pedido para pegar parametros enviados
                string xmlData = "", url = "", urlHandshake = "", urlHandshakeConfiguration = "";
                foreach (DHandshakeSessionLog hsLog in _hsLogs)
                {
                    if (hsLog.step == 3)
                        xmlData = hsLog.xmlData;
                }

                urlHandshake = (IsOffLine ? "" : GenericHelper.GetSingleNodeString(xmlData, (ConfigurationIsXml ? "//post_final" : "//urlpostloja")));
                urlHandshakeConfiguration = (ConfigurationIsXml ? (IsOffLine ? _dHandshakeConfigurationXml.urlFinalizationOffline : _dHandshakeConfigurationXml.urlFinalization) : (IsOffLine ? _dHandshakeConfigurationHtml.urlFinalizationOffline : _dHandshakeConfigurationHtml.urlFinalization));

                if (String.IsNullOrEmpty(urlHandshake) && String.IsNullOrEmpty(urlHandshakeConfiguration))
                {
                    GenericHelper.LogFile("EasyPagObject::HelperHtml.cs::FinalizationPost.SendClient A loja " + _dOrder.storeId.ToString() + " não possue url para o post de finalização configurada, o post não será enviado", LogFileEntryType.Information);

                    Result = true;
                    Error = "";
                    return;
                }

                if (!String.IsNullOrEmpty(urlHandshake))
                    url = urlHandshake;
                else if (!String.IsNullOrEmpty(urlHandshakeConfiguration))
                    url = urlHandshakeConfiguration;

                helper = new Helper();
                ClientHttpRequisition post = new ClientHttpRequisition();
                post.Method = "POST";
                post.Url = url;
                post.EndResponse = false;
                post.Encoding = (ConfigurationIsXml ? _dHandshakeConfigurationXml.responseEncoding : _dHandshakeConfigurationHtml.responseEncoding);

                post.Parameters.Add("91D4C3128BF7DA7F", _dOrder.storeReferenceOrder);
                post.Parameters.Add("5DED746B8F924F2E", _dStore.storeKey);
                post.Parameters.Add("VALIDA_KEY", GenericHelper.GetSingleNodeString(xmlData, "//valida_key"));

                post.Parameters.Add("nome", _dConsumer.name);
                //se pessoa juridica
                if (_dConsumer.CNPJ != null && _dConsumer.CNPJ != string.Empty)
                {
                    post.Parameters.Add("TIPO_PESSOA", "PJ");
                    post.Parameters.Add("costumer", "PJ");
                    post.Parameters.Add("CPF", "");
                    post.Parameters.Add("fone", "");
                    post.Parameters.Add("estcivil", "");
                    post.Parameters.Add("sexo", "");
                    post.Parameters.Add("profissao", "");
                    post.Parameters.Add("CNPJ", _dConsumer.CNPJ);
                    post.Parameters.Add("CGC", _dConsumer.CNPJ);
                    post.Parameters.Add("RAZAO_PJ", _dConsumer.name);
                    post.Parameters.Add("INSCRICAO_PJ", _dConsumer.IE);
                    post.Parameters.Add("NOME_RESP_PJ", _dConsumer.responsibleName);
                }
                else
                {
                    post.Parameters.Add("TIPO_PESSOA", "PF");
                    post.Parameters.Add("costumer", "PF");
                    post.Parameters.Add("CPF", _dConsumer.CPF);
                    post.Parameters.Add("fone", _dConsumer.phone);
                    post.Parameters.Add("estcivil", _dConsumer.civilState);
                    post.Parameters.Add("sexo", _dConsumer.ger);
                    post.Parameters.Add("profissao", _dConsumer.occupation);
                    post.Parameters.Add("CNPJ", "");
                    post.Parameters.Add("CGC", "");
                    post.Parameters.Add("RAZAO_PJ", "");
                    post.Parameters.Add("INSCRICAO_PJ", "");
                    post.Parameters.Add("NOME_RESP_PJ", "");
                }
                post.Parameters.Add("NASC", (_dConsumer.birthDate != DateTime.MinValue ? _dConsumer.birthDate.ToString("yyyy-MM-dd") : ""));
                post.Parameters.Add("dianasc", (_dConsumer.birthDate != DateTime.MinValue ? _dConsumer.birthDate.ToString("dd") : ""));
                post.Parameters.Add("mesnasc", (_dConsumer.birthDate != DateTime.MinValue ? _dConsumer.birthDate.ToString("MM") : ""));
                post.Parameters.Add("anonasc", (_dConsumer.birthDate != DateTime.MinValue ? _dConsumer.birthDate.ToString("yyyy") : ""));
                post.Parameters.Add("email", _dConsumer.email);
                post.Parameters.Add("FAX", _dConsumer.fax);

                //endereco de cobranca
                post.Parameters.Add("logradouro", _dBillingAddress.logradouro);
                post.Parameters.Add("endereco", _dBillingAddress.address);
                post.Parameters.Add("numero", _dBillingAddress.addressNumber);
                post.Parameters.Add("COMPLEMENTO", _dBillingAddress.addressComplement);
                post.Parameters.Add("bairro", _dBillingAddress.district);
                post.Parameters.Add("cidade", _dBillingAddress.city);
                post.Parameters.Add("cep", _dBillingAddress.cep);
                post.Parameters.Add("estado", _dBillingAddress.state.Trim());
                post.Parameters.Add("pais", _dBillingAddress.country);

                //endereco de entrega
                post.Parameters.Add("logradouro_d", _dDeliveryAddress.logradouro);
                post.Parameters.Add("endereco_d", _dDeliveryAddress.address);
                post.Parameters.Add("numero_d", _dDeliveryAddress.addressNumber);
                post.Parameters.Add("COMPLEMENTO_D", _dDeliveryAddress.addressComplement);
                post.Parameters.Add("BAIRRO_D", _dDeliveryAddress.district);
                post.Parameters.Add("cidade_d", _dDeliveryAddress.city);
                post.Parameters.Add("cep_d", _dDeliveryAddress.cep);
                post.Parameters.Add("estado_d", _dDeliveryAddress.state.Trim());
                post.Parameters.Add("pais_d", _dDeliveryAddress.country);

                //carinho de compras
                post.Parameters.Add("SITEM", _dOrderItemArr.Length.ToString());
                int i = 1;
                foreach (DOrderItem dOrderItem in _dOrderItemArr)
                {
                    post.Parameters.Add(string.Format("COD_{0}", i), dOrderItem.itemCode.ToString());
                    post.Parameters.Add(string.Format("DES_{0}", i), HttpUtility.HtmlEncode(dOrderItem.itemDescription));
                    post.Parameters.Add(string.Format("QTD_{0}", i), dOrderItem.itemQuantity.ToString());
                    post.Parameters.Add(string.Format("VAL_{0}", i), GenericHelper.ParseString(dOrderItem.itemValue));
                    post.Parameters.Add(string.Format("TOTAL_ITEM_{0}", i), GenericHelper.ParseString(dOrderItem.itemValue * dOrderItem.itemQuantity));
                    i++;
                }

                //valores e forma de pagamento do pedido
                post.Parameters.Add("VAL_PARCIAL_PEDIDO", GenericHelper.ParseString(_dOrder.totalAmount));
                post.Parameters.Add("SFRETE", GenericHelper.ParseString(_shippingRate));
                post.Parameters.Add("VAL_JUROS_DESCONTO", GenericHelper.ParseString(_dOrderInstallment[0].interestPercentage));
                post.Parameters.Add("VAL_FINAL_PEDIDO", GenericHelper.ParseString(_dOrder.finalAmount));
                post.Parameters.Add("QTD_PARCELAS", _dOrder.installmentQuantity.ToString());
                post.Parameters.Add("VAL_PARCELA", GenericHelper.ParseString(_dOrderInstallment[0].installmentValue));

                //parâmetros de controle do EasyPag
                post.Parameters.Add("CODE_RET", "0");
                post.Parameters.Add("DES_RET", "SUPERPAG OK");
                post.Parameters.Add("data_trans", _dOrder.creationDate.ToString("yyyyMMdd"));
                post.Parameters.Add("hora_trans", _dOrder.creationDate.ToString("HHmmss"));
                post.Parameters.Add("COD_CONTROLE", _dAttempt.paymentAttemptId.ToString());

                DPaymentForm paymentForm = DataFactory.PaymentForm().Locate(_dAttempt.paymentFormId);
                Ensure.IsNotNullPage(paymentForm, "Meio de pagamento inválido");

                post.Parameters.Add("meio_pagamento", SuperPag.Handshake.Helper.GetSmartPagPaymentForm(paymentForm.paymentFormId).ToString());
                post.Parameters.Add("COD_FORMA_PAGTO", SuperPag.Handshake.Helper.GetSmartPagPaymentForm(paymentForm.paymentFormId).ToString());
                post.Parameters.Add("FORMA_PAGTO", SuperPag.Handshake.Helper.GetSmartPagPaymentFormNameByGroup(_dPaymentForm.paymentFormId));

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
                            post.Parameters.Add(String.Format("NUMERO_OCT{0}", installment.installmentNumber), boleto.oct);
                            post.Parameters.Add(String.Format("URLBOLETO{0}", installment.installmentNumber), String.Format("{0}/Agents/Boleto/showboleto.aspx?id={1}", serverUrl, attempt.paymentAttemptId));
                        }
                        break;
                    case (int)PaymentAgents.VBV:
                    case (int)PaymentAgents.VBVInBox:
                    case (int)PaymentAgents.VBV3:
                        DPaymentAttemptVBV vbv = DataFactory.PaymentAttemptVBV().Locate(_dAttempt.paymentAttemptId);
                        post.Parameters.Add("bandeira", SuperPag.Handshake.Helper.GetSmartPagCreditCardFlag(paymentForm.paymentFormId).ToString());
                        post.Parameters.Add("operadora_cartao", SuperPag.Handshake.Helper.GetSmartPagCreditCardFlag(paymentForm.paymentFormId).ToString());
                        post.Parameters.Add("id_trans", vbv.tid);
                        post.Parameters.Add("num_autorizacao", vbv.arp.ToString());
                        post.Parameters.Add("tid", vbv.tid);
                        post.Parameters.Add("arp", vbv.arp.ToString());
                        post.Parameters.Add("visa_set", "1");
                        break;
                    case (int)PaymentAgents.VisaMoset:
                    case (int)PaymentAgents.VisaMoset3:
                        DPaymentAttemptMoset moset = DataFactory.PaymentAttemptMoset().Locate(_dAttempt.paymentAttemptId);
                        post.Parameters.Add("bandeira", SuperPag.Handshake.Helper.GetSmartPagCreditCardFlag(paymentForm.paymentFormId).ToString());
                        post.Parameters.Add("operadora_cartao", SuperPag.Handshake.Helper.GetSmartPagCreditCardFlag(paymentForm.paymentFormId).ToString());
                        post.Parameters.Add("id_trans", moset.tid);
                        post.Parameters.Add("tid", moset.tid);
                        post.Parameters.Add("visa_set", "1");
                        break;
                    case (int)PaymentAgents.Komerci:
                    case (int)PaymentAgents.KomerciInBox:
                        DPaymentAttemptKomerci komerci = DataFactory.PaymentAttemptKomerci().Locate(_dAttempt.paymentAttemptId);
                        post.Parameters.Add("bandeira", SuperPag.Handshake.Helper.GetSmartPagCreditCardFlag(paymentForm.paymentFormId).ToString());
                        post.Parameters.Add("operadora_cartao", SuperPag.Handshake.Helper.GetSmartPagCreditCardFlag(paymentForm.paymentFormId).ToString());
                        post.Parameters.Add("COMPROV_VENDA", komerci.numautent);
                        post.Parameters.Add("num_autorizacao", komerci.numautor);
                        post.Parameters.Add("NSU_TRANS", komerci.numautent.ToString());
                        post.Parameters.Add("NUMERO_CARTAO", komerci.nr_cartao);
                        break;
                    case (int)PaymentAgents.KomerciWS:
                        DPaymentAttemptKomerciWS komerciws = DataFactory.PaymentAttemptKomerciWS().Locate(_dAttempt.paymentAttemptId);
                        post.Parameters.Add("BANDEIRA", SuperPag.Handshake.Helper.GetSmartPagCreditCardFlag(paymentForm.paymentFormId).ToString());
                        post.Parameters.Add("COMPROV_VENDA", komerciws.numautent);
                        post.Parameters.Add("NUM_AUTORIZACAO", komerciws.numautor);
                        post.Parameters.Add("NSU_TRANS", komerciws.numautent.ToString());
                        post.Parameters.Add("NUMERO_CARTAO", "");
                        break;
                    case (int)PaymentAgents.PaymentClientVirtual2Party:
                    case (int)PaymentAgents.PaymentClientVirtual3Party:
                        DPaymentAttemptPaymentClientVirtual pcv = DataFactory.PaymentAttemptPaymentClientVirtual().Locate(_dAttempt.paymentAttemptId);
                        post.Parameters.Add("bandeira", SuperPag.Handshake.Helper.GetSmartPagCreditCardFlag(paymentForm.paymentFormId).ToString());
                        post.Parameters.Add("operadora_cartao", SuperPag.Handshake.Helper.GetSmartPagCreditCardFlag(paymentForm.paymentFormId).ToString());
                        post.Parameters.Add("id_trans", pcv.vpc_TransactionNo.ToString());
                        post.Parameters.Add("num_autorizacao", pcv.vpc_AuthorizeId.ToString());
                        break;
                    case (int)PaymentAgents.ItauShopLine:
                        DPaymentAttemptItauShopline itauShopline = DataFactory.PaymentAttemptItauShopline().Locate(_dAttempt.paymentAttemptId);
                        post.Parameters.Add("num_autorizacao", (_dAttempt.status == (int)PaymentAttemptStatus.Paid ? "00" : "01"));
                        post.Parameters.Add("pedido_shopline", itauShopline.agentOrderReference.ToString());
                        break;
                    case (int)PaymentAgents.BBPag:
                        DPaymentAttemptBB bbpag = DataFactory.PaymentAttemptBB().Locate(_dAttempt.paymentAttemptId);
                        post.Parameters.Add("num_autorizacao", bbpag.agentOrderReference.ToString());
                        break;
                    case (int)PaymentAgents.FinanciamentoABN:
                        DPaymentAttemptABN abn = DataFactory.PaymentAttemptABN().Locate(_dAttempt.paymentAttemptId);
                        post.Parameters.Add("NumProposta", abn.numProposta);
                        post.Parameters.Add("NumControleABN", abn.numControle.ToString());
                        post.Parameters.Add("bStatus", abn.statusProposta);
                        post.Parameters.Add("strDesc", abn.msgRet);
                        post.Parameters.Add("DAT_VENCIMENTO1_ABN", abn.dataVencimento.ToString("yyyy-MM-dd"));
                        post.Parameters.Add("QTDPARCELAS_ABN", abn.qtdPrestacao.ToString());
                        post.Parameters.Add("Prestacao", GenericHelper.ParseString(abn.prestacao));
                        post.Parameters.Add("Entrada", "000");
                        break;
                }

                //parametros adicionais
                post.Parameters.Add("PARAM_OP1_PED", GenericHelper.GetSingleNodeString(xmlData, "//param_op1_ped"));
                post.Parameters.Add("PARAM_OP2_PED", GenericHelper.GetSingleNodeString(xmlData, "//param_op2_ped"));
                post.Parameters.Add("PARAM_OP3_PED", GenericHelper.GetSingleNodeString(xmlData, "//param_op3_ped"));

                //parametros compativeis com o sp
                post.Parameters.Add("TIPO_SERVICO", "1");
                post.Parameters.Add("Regra8", "01");
                post.Parameters.Add("AutorizaCartaoOnLine", "S");
                post.Parameters.Add("processaAmex", "0");
                post.Parameters.Add("spf", "1");
                post.Parameters.Add("REDECARD", "");
                post.Parameters.Add("PAX2", "");
                post.Parameters.Add("check", "0");
                post.Parameters.Add("valida_cc", "N");
                post.Parameters.Add("VisaNet", "N");
                post.Parameters.Add("bEntrada", "S");
                post.Parameters.Add("NUMAUTENT", "");
                post.Parameters.Add("NUMDOC1", "");
                post.Parameters.Add("NUMAUTOR", "");
                post.Parameters.Add("NUMCV", "");
                post.Parameters.Add("NUMDOC2", "");
                post.Parameters.Add("SID", (_hsLogs != null && _hsLogs.Length > 0 ? "{" + _hsLogs[0].handshakeSessionId.ToString().ToUpper() + "}" : ""));
                post.Parameters.Add("SIDForm", (_hsLogs != null && _hsLogs.Length > 0 ? "{" + _hsLogs[0].handshakeSessionId.ToString().ToUpper() + "}" : ""));
                post.Parameters.Add("ppagamento", GenericHelper.GetSingleNodeString(xmlData, "//ppagamento"));
                post.Parameters.Add("optParcela", _dOrder.installmentQuantity.ToString());
                post.Parameters.Add("valida_email", GenericHelper.GetSingleNodeString(xmlData, "//valida_email"));
                post.Parameters.Add("SHOW_TELA_FINALIZACAO", GenericHelper.GetSingleNodeString(xmlData, "//show_tela_finalizacao"));
                post.Parameters.Add("TIPO_ACAO", GenericHelper.GetSingleNodeString(xmlData, "//tipo_acao"));
                post.Parameters.Add("state", GenericHelper.GetSingleNodeString(xmlData, "//state"));
                post.Parameters.Add("LINKBOTAO6", GenericHelper.GetSingleNodeString(xmlData, "//linkbotao6"));
                post.Parameters.Add("lang", GenericHelper.GetSingleNodeString(xmlData, "//lang"));
                post.Parameters.Add("cc", GenericHelper.GetSingleNodeString(xmlData, "//cc"));
                post.Parameters.Add("q", GenericHelper.GetSingleNodeString(xmlData, "//q"));
                post.Parameters.Add("v", GenericHelper.GetSingleNodeString(xmlData, "//v"));
                post.Parameters.Add("frame50", GenericHelper.GetSingleNodeString(xmlData, "//frame50"));

                int status;

                if (post.Send())
                {
                    status = (int)PostStatus.Sent;
                    GenericHelper.LogFile("EasyPagObject::HelperHtml.cs::FinalizationPost.SendClient storeId=" + _dOrder.storeId.ToString() + " orderId=" + _dOrder.orderId.ToString() + " url=" + post.Url + " enviado=" + post.LastSentData, LogFileEntryType.Information);
                }
                else
                {
                    status = (int)PostStatus.Error;
                    GenericHelper.LogFile("EasyPagObject::HelperHtml.cs::FinalizationPost.SendClient storeId=" + _dOrder.storeId.ToString() + " orderId=" + _dOrder.orderId.ToString() + " url=" + post.Url + " enviado=" + post.LastSentData + " msg erro=" + post.ErrorMessage, LogFileEntryType.Error);
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
                GenericHelper.LogFile("EasyPagObject::HelperHtml.cs::FinalizationPost.SendClient storeId=" + _dOrder.storeId.ToString() + " orderId=" + _dOrder.orderId.ToString() + " msg erro=" + e.Message, LogFileEntryType.Error);
            }
        }
    }

    public class PaymentPost
    {
        public bool Result;
        public string Error;
        public bool ConfigurationIsXml = false;
        public bool IsOffLine = false;

        DOrder _dOrder;
        DOrderInstallment[] _dOrderInstallment;
        DPaymentForm _dPaymentForm;
        decimal _shippingRate;
        DPaymentAttempt _dAttempt;
        DHandshakeConfigurationHtml _dHandshakeConfigurationHtml;
        DHandshakeConfigurationXml _dHandshakeConfigurationXml;
        Helper helper;

        public PaymentPost(Guid paymentAttemptId, int installmentNumber, bool confIsXml)
        {
            ConfigurationIsXml = confIsXml;
            LoadData(paymentAttemptId, installmentNumber);
        }

        public PaymentPost(Guid paymentAttemptId, int installmentNumber)
        {
            LoadData(paymentAttemptId, installmentNumber);
        }

        private void LoadData(Guid paymentAttemptId, int installmentNumber)
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

                if (ConfigurationIsXml)
                {
                    //localiza configuracoes do handshake xml
                    DHandshakeConfiguration dHandshakeConfiguration = DataFactory.HandshakeConfiguration().Locate(_dOrder.storeId, (int)HandshakeType.XmlSPag10);
                    Ensure.IsNotNull(dHandshakeConfiguration, "Configuração de handshake para a loja {0}  não encontrada.", _dOrder.storeId);
                    _dHandshakeConfigurationXml = DataFactory.HandshakeConfigurationXml().Locate(dHandshakeConfiguration.handshakeConfigurationId);
                    Ensure.IsNotNull(_dHandshakeConfigurationXml, "Configuração de handshake xml para a loja {0}  não encontrada.", _dOrder.storeId);
                }
                else
                {
                    //localiza configuracoes do handshake html
                    DHandshakeConfiguration dHandshakeConfiguration = DataFactory.HandshakeConfiguration().Locate(_dOrder.storeId, (int)HandshakeType.HtmlSPag10);
                    Ensure.IsNotNull(dHandshakeConfiguration, "Configuração de handshake para a loja {0}  não encontrada.", _dOrder.storeId);
                    _dHandshakeConfigurationHtml = DataFactory.HandshakeConfigurationHtml().Locate(dHandshakeConfiguration.handshakeConfigurationId);
                    Ensure.IsNotNull(_dHandshakeConfigurationHtml, "Configuração de handshake html para a loja {0}  não encontrada.", _dOrder.storeId);
                }
                
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

                //localiza o forma de pagamento
                _dPaymentForm = DataFactory.PaymentForm().Locate(dPaymentAttempt.paymentFormId);
                Ensure.IsNotNull(_dPaymentForm, "Forma de pagamento {0} não encontrada", dPaymentAttempt.paymentFormId);

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
                GenericHelper.LogFile("EasyPagObject::HelperHtml.cs::PaymentPost.PaymentPost(Guid, int) " + e.Message, LogFileEntryType.Error);
                throw;
            }
        }
        
        public void Send()
        {
            try
            {
                if (_dAttempt.status != (int)PaymentAttemptStatus.Paid)
                    Ensure.IsNotNull(null, "EasyPagObject::HelperHtml.cs::PaymentPost.Send storeId={0} O pedido {1} não está pago", _dOrder.storeId.ToString(), _dAttempt.paymentAttemptId);

                string urlHandshakeConfiguration = (ConfigurationIsXml ? (IsOffLine ? _dHandshakeConfigurationXml.urlPaymentConfirmationOffline : _dHandshakeConfigurationXml.urlPaymentConfirmation) : (IsOffLine ? _dHandshakeConfigurationHtml.urlPaymentConfirmationOffline : _dHandshakeConfigurationHtml.urlPaymentConfirmation));

                if (String.IsNullOrEmpty(urlHandshakeConfiguration))
                {
                    GenericHelper.LogFile("EasyPagObject::HelperHtml.cs::PaymentPost.Send storeId=" + _dOrder.storeId.ToString() + " A loja não possue url para o post de pagamento " + (IsOffLine ? "offline " : "") + "configurada, o post não será enviado", LogFileEntryType.Information);

                    Result = true;
                    Error = "";
                    return;
                }
                
                helper = new Helper();
                ServerHttpHtmlRequisition post = new ServerHttpHtmlRequisition();
                post.Url = urlHandshakeConfiguration;

                post.Parameters.Add("91D4C3128BF7DA7F", _dOrder.storeReferenceOrder.ToString());
                post.Parameters.Add("VAL_FINAL_PEDIDO", GenericHelper.ParseString(_dOrder.finalAmount));
                post.Parameters.Add("VAL_PARCIAL_PEDIDO", GenericHelper.ParseString(_dOrder.totalAmount));
                post.Parameters.Add("SFRETE", GenericHelper.ParseString(_shippingRate));
                post.Parameters.Add("FORMA_PAGTO", SuperPag.Handshake.Helper.GetSmartPagPaymentForm(_dPaymentForm.paymentFormId).ToString());
                post.Parameters.Add("COD_CONTROLE", _dAttempt.paymentAttemptId.ToString());

                //a busca da data de pagamento é diferenciada para boleto,
                //essa informação é recuperada do arquivo retornado do banco
                //e é armazenada na tabela PaymentAttemptBoletoReturn
                if (_dPaymentForm.paymentAgentId == (int)PaymentAgents.Boleto)
                {
                    DateTime date = _dAttempt.lastUpdate;

                    DPaymentAttemptBoleto boleto = DataFactory.PaymentAttemptBoleto().Locate(_dAttempt.paymentAttemptId);
                    if (Ensure.IsNotNull(boleto))
                    {
                        DPaymentAttemptBoletoReturn boletoReturn = DataFactory.PaymentAttemptBoletoReturn().Locate(boleto.agentOrderReference.ToString());
                        if (Ensure.IsNotNull(boletoReturn))
                            date = boletoReturn.dataLiquidacao;
                    }

                    post.Parameters.Add("DATA_PAGTO", date.ToString("yyyyMMdd"));
                }
                else
                    post.Parameters.Add("DATA_PAGTO", _dAttempt.lastUpdate.ToString("yyyyMMdd"));

                switch (_dPaymentForm.paymentAgentId)
                {
                    case (int)PaymentAgents.VBV:
                    case (int)PaymentAgents.VBVInBox:
                    case (int)PaymentAgents.VBV3:
                        DPaymentAttemptVBV vbv = DataFactory.PaymentAttemptVBV().Locate(_dAttempt.paymentAttemptId);
                        post.Parameters.Add("BANDEIRA", SuperPag.Handshake.Helper.GetSmartPagCreditCardFlag(_dPaymentForm.paymentFormId).ToString());
                        post.Parameters.Add("ID_TRANS", vbv.tid);
                        post.Parameters.Add("NUM_AUTORIZACAO", vbv.arp.ToString());
                        break;
                    case (int)PaymentAgents.VisaMoset:
                    case (int)PaymentAgents.VisaMoset3:
                        DPaymentAttemptMoset moset = DataFactory.PaymentAttemptMoset().Locate(_dAttempt.paymentAttemptId);
                        post.Parameters.Add("BANDEIRA", SuperPag.Handshake.Helper.GetSmartPagCreditCardFlag(_dPaymentForm.paymentFormId).ToString());
                        post.Parameters.Add("ID_TRANS", moset.tid);
                        break;
                    case (int)PaymentAgents.Komerci:
                    case (int)PaymentAgents.KomerciInBox:
                        DPaymentAttemptKomerci komerci = DataFactory.PaymentAttemptKomerci().Locate(_dAttempt.paymentAttemptId);
                        post.Parameters.Add("BANDEIRA", SuperPag.Handshake.Helper.GetSmartPagCreditCardFlag(_dPaymentForm.paymentFormId).ToString());
                        post.Parameters.Add("COMPROV_VENDA", komerci.numautent);
                        post.Parameters.Add("NUM_AUTORIZACAO", komerci.numautor);
                        post.Parameters.Add("NSU_TRANS", komerci.numautent.ToString());
                        post.Parameters.Add("NUMERO_CARTAO", komerci.nr_cartao);
                        break;
                    case (int)PaymentAgents.KomerciWS:
                        DPaymentAttemptKomerciWS komerciws = DataFactory.PaymentAttemptKomerciWS().Locate(_dAttempt.paymentAttemptId);
                        post.Parameters.Add("BANDEIRA", SuperPag.Handshake.Helper.GetSmartPagCreditCardFlag(_dPaymentForm.paymentFormId).ToString());
                        post.Parameters.Add("COMPROV_VENDA", komerciws.numautent);
                        post.Parameters.Add("NUM_AUTORIZACAO", komerciws.numautor);
                        post.Parameters.Add("NSU_TRANS", komerciws.numautent.ToString());
                        post.Parameters.Add("NUMERO_CARTAO", "");
                        break;
                    case (int)PaymentAgents.PaymentClientVirtual2Party:
                    case (int)PaymentAgents.PaymentClientVirtual3Party:
                        DPaymentAttemptPaymentClientVirtual pcv = DataFactory.PaymentAttemptPaymentClientVirtual().Locate(_dAttempt.paymentAttemptId);
                        post.Parameters.Add("BANDEIRA", SuperPag.Handshake.Helper.GetSmartPagCreditCardFlag(_dPaymentForm.paymentFormId).ToString());
                        post.Parameters.Add("ID_TRANS", pcv.vpc_TransactionNo.ToString());
                        post.Parameters.Add("NUM_AUTORIZACAO", pcv.vpc_AuthorizeId.ToString());
                        break;
                    case (int)PaymentAgents.ItauShopLine:
                        DPaymentAttemptItauShopline itauShopline = DataFactory.PaymentAttemptItauShopline().Locate(_dAttempt.paymentAttemptId);
                        post.Parameters.Add("NUM_AUTORIZACAO", (_dAttempt.status == (int)PaymentAttemptStatus.Paid ? "00" : "01"));
                        post.Parameters.Add("PEDIDO_SHOPLINE", itauShopline.agentOrderReference.ToString());
                        break;
                    case (int)PaymentAgents.BBPag:
                        DPaymentAttemptBB bbpag = DataFactory.PaymentAttemptBB().Locate(_dAttempt.paymentAttemptId);
                        post.Parameters.Add("NUM_AUTORIZACAO", bbpag.agentOrderReference.ToString());
                        break;
                }

                foreach (DOrderInstallment installment in _dOrderInstallment)
                {
                    post.Parameters.Set("VAL_PARCELA", GenericHelper.ParseString(installment.installmentValue));
                    post.Parameters.Set("NUM_PARCELA", installment.installmentNumber.ToString());

                    if (_dPaymentForm.paymentAgentId == (int)PaymentAgents.Boleto)
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

                        DPaymentAttemptBoleto boleto = DataFactory.PaymentAttemptBoleto().Locate(_dAttempt.paymentAttemptId);
                        post.Parameters.Set(String.Format("NUMERO_OCT{0}", installment.installmentNumber), boleto.oct);
                        post.Parameters.Set(String.Format("URLBOLETO{0}", installment.installmentNumber), String.Format("{0}/Agents/Boleto/showboleto.aspx?id={1}", serverUrl, _dAttempt.paymentAttemptId));
                    }

                    int status;

                    if (post.Send())
                    {
                        status = (int)PostStatus.Sent;
                        GenericHelper.LogFile("EasyPagObject::HelperHtml.cs::PaymentPost.Send storeId=" + _dOrder.storeId.ToString() + " orderId=" + _dOrder.orderId.ToString() + " installmentNumber=" + installment.installmentNumber.ToString() + " url=" + post.Url + " enviado=" + post.LastSentData + " recebido=" + post.Response, LogFileEntryType.Information);
                    }
                    else
                    {
                        status = (int)PostStatus.Error;
                        GenericHelper.LogFile("EasyPagObject::HelperHtml.cs::PaymentPost.Send storeId=" + _dOrder.storeId.ToString() + " orderId=" + _dOrder.orderId.ToString() + " installmentNumber=" + installment.installmentNumber.ToString() + " url=" + post.Url + " enviado=" + post.LastSentData + " msg erro=" + post.Response, LogFileEntryType.Error);
                    }

                    DServicePaymentPost paymentPost = DataFactory.ServicePaymentPost().Locate(_dAttempt.paymentAttemptId, installment.installmentNumber);

                    if (paymentPost != null)
                    {
                        paymentPost.postStatus = (paymentPost.postStatus != (int)PostStatus.Confirmed ? status : (int)PostStatus.Confirmed);
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
                GenericHelper.LogFile("EasyPagObject::HelperHtml.cs::PaymentPost.Send storeId=" + _dOrder.storeId.ToString() + " orderId=" + _dOrder.orderId.ToString() + " msg erro=" + e.Message, LogFileEntryType.Error);
            }

            #region Valores que o smartpag manda
            //VAL_PARCELA // valor da parcela
            //VAL_FINAL_PEDIDO //com frete
            //VAL_PARCIAL_PEDIDO //sem frete
            //SFRETE //frete
            //FORMA_PAGTO // codigo do meio de pagamento
            //NUM_PARCELA //numero da parcela
            //COD_CONTROLE //nosso pedido
            //DATA_PAGTO // data do pagamento

            //SEQ_AGENDAMENTO // cob?
            //COD_LOTE // cob?

            ////boleto
            //URLBOLETOX //onde x é o número da parcela
            //NUMERO_OCT

            ////cartao
            //BANDEIRA
            //MES_VENC_CARTAO
            //ANO_VENC_CARTAO
            //TITULAR_CARTAO
            //DAT_VENCIMENTO
            //NUMERO_CARTAO
            //CVC2


            ////vbv
            //ID_TRANS
            //NUM_AUTORIZACAO

            ////komerci
            //NUM_AUTORIZACAO
            //COMPROV_VENDA //numcv
            //NSU_TRANS //numautent

            ////bradesco
            //ASSINATURA

            ////itaushopline
            //NUM_AUTORIZACAO

            ////bbpag
            //NUM_AUTORIZACAO 
            #endregion
        }
    }
}
