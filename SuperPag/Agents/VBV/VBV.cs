using System;
using System.Xml;
using System.Web;
using System.Text;
using System.IO;
using System.Collections.Generic;
using SuperPag.Helper;
using SuperPag.Data;
using SuperPag.Data.Messages;
using SuperPag.Agents.VBV.Messages;
using SuperPag.Agents.VBV.Helper;
using System.Configuration;
using System.Threading;


namespace SuperPag.Agents.VBV
{
    public class VBV : WebAgent
    {
        public DPaymentAttemptVBV attemptVBV;
        public DPaymentAgentSetupVBV agentsetup;

        public VBV() { }
        public VBV(Guid paymentAttemptId) : base(paymentAttemptId)
        {
            this.attemptVBV = DataFactory.PaymentAttemptVBV().Locate(paymentAttemptId);
            if (this.attempt != null)
                this.agentsetup = DataFactory.PaymentAgentSetupVBV().Locate(attempt.paymentAgentSetupId);
        }
        public VBV(DPaymentAttemptVBV paymentAttemptVBV) : base(paymentAttemptVBV.paymentAttemptId)
        {
            this.attemptVBV = paymentAttemptVBV;
            if (this.attempt != null)
                this.agentsetup = DataFactory.PaymentAgentSetupVBV().Locate(attempt.paymentAgentSetupId);
        }
        public VBV(string tid)
        {
            this.attemptVBV = DataFactory.PaymentAttemptVBV().Locate(tid);
            if(this.attemptVBV != null)
                this.attempt = DataFactory.PaymentAttempt().Locate(attemptVBV.paymentAttemptId);
            if (this.attempt != null)
                this.agentsetup = DataFactory.PaymentAgentSetupVBV().Locate(attempt.paymentAgentSetupId);
            if (this.order == null)
                this.order = DataFactory.Order().Locate(attempt.orderId);
        }

        
        public override void Start(Guid paymentAttemptId)
        {
            base.Start(paymentAttemptId);
            Fill(paymentAttemptId);
            try
            {
                //Start: Salvar attempt especializada, criação do arquivo XML VBV
                attemptVBV = DataFactory.PaymentAttemptVBV().Locate(attempt.paymentAttemptId);
                if (attemptVBV == null)
                {
                    attemptVBV = new DPaymentAttemptVBV();
                    attemptVBV.tid = GenerateTID(agentsetup.businessNumber.ToString(), attempt.paymentFormId, order.installmentQuantity, storePaymentInstallment.installmentType);
                    
                    while (SuperPag.Business.PaymentAttemptVBV.CheckTid(attemptVBV.tid))
                    {
                        attemptVBV.tid = GenerateTID(agentsetup.businessNumber.ToString(), attempt.paymentFormId, order.installmentQuantity, storePaymentInstallment.installmentType);
                    }

                    attemptVBV.paymentAttemptId = attempt.paymentAttemptId;
                    attemptVBV.vbvStatus = (byte)PaymentAttemptVBVStatus.Initial;
                    attemptVBV.vbvOrderId = attempt.orderId.ToString();
                    attemptVBV.free = attempt.paymentAttemptId.ToString();
                    attemptVBV.price = (int)(attempt.price * 100);
                    DataFactory.PaymentAttemptVBV().Insert(attemptVBV);
                    BuildRequestXml();
                }

                HttpContext.Current.Response.Redirect("~/Agents/VBV/checkpopup.aspx?id=" + attempt.paymentAttemptId.ToString());
            }
            catch (Exception ex)
            {
                string errorMsg = "Ocorreu um erro na criação da transação: " + ex.Message;
                //TODO: this.OnError(errorMsg, "SuperPag.Agents.VBV::VBV::Step1 paymentAttemptId=" + attempt.paymentAttemptId.ToString() + " error=" + errorMsg, LogFileEntryType.Error);
            }

        }

        public void Step1()
        {
            //Step2: MPG
            ClientHttpRequisition post = new ClientHttpRequisition();
            try
            {
                Ensure.IsNotNullPage(attemptVBV, "Tentativa de pagamento não encontrada.");
                DPaymentAttempt attempt = DataFactory.PaymentAttempt().Locate(attemptVBV.paymentAttemptId);
                DPaymentAgentSetupVBV agentsetup = DataFactory.PaymentAgentSetupVBV().Locate(attempt.paymentAgentSetupId);
                DOrder order = DataFactory.Order().Locate(attempt.orderId);

                //Historico de Navegacao
                GenericHelper.SetOrderStatus(HttpContext.Current, WorkflowOrderStatus.AgentCalled, attempt.paymentFormId + "," + order.installmentQuantity + "," + (int)PaymentAgents.VBV + ",Step1(Mpg)");

                attemptVBV.vbvStatus = (byte)PaymentAttemptVBVStatus.Mpg;
                post.FormName = "pay_VerifiedByVisa";
                post.Method = "POST";
                post.Url = System.Configuration.ConfigurationManager.AppSettings["VBVComponentUrl"] + "mpg.exe?";
                post.Parameters.Add("tid", attemptVBV.tid);
                post.Parameters.Add("order", "Pedido no. " + order.storeReferenceOrder + ": Itens: produtos - " + GenericHelper.FormatCurrencyBrasil(attemptVBV.price / 100));
                post.Parameters.Add("orderid", attemptVBV.vbvOrderId);
                post.Parameters.Add("merchid", "cfg" + agentsetup.paymentAgentSetupId.ToString());
                post.Parameters.Add("free", attemptVBV.free);
                post.Parameters.Add("damount", GenericHelper.FormatCurrencyBrasil(((decimal)attemptVBV.price) / 100));
                DataFactory.PaymentAttemptVBV().Update(attemptVBV);
            }
            catch (Exception ex)
            {
                string errorMsg = "Ocorreu um erro no envio da transação: " + ex.Message;
                GenericHelper.LogFile("SuperPag.Agents.VBV::VBV::Step1 paymentAttemptId=" + attempt.paymentAttemptId.ToString() + " error=" + errorMsg, LogFileEntryType.Error);
                GenericHelper.RedirectToErrorPage(errorMsg);
            }
            finally
            {
                post.Send();
            }
        }
        public void Step2()
        {
            try
            {
                Ensure.IsNotNullPage(attemptVBV, "Tentativa de pagamento não encontrada");
                string LR, ARP, PRICE, ORDERID, FREE, PAN, BANK, ARS, AUTHENT, TID;
                #region FillParams
                string Path = System.Configuration.ConfigurationManager.AppSettings["VBVDirectory"] + "\\results\\" + attemptVBV.tid + ".xml";
                System.IO.FileInfo FI = new System.IO.FileInfo(Path);
                // Via LocalHost, o XML não é gerado
                if (!FI.Exists)
                {

                    TID = HttpContext.Current.Request["TID"];
                    LR = HttpContext.Current.Request["LR"];
                    ARP = HttpContext.Current.Request["ARP"];
                    PRICE = HttpContext.Current.Request["PRICE"];
                    ORDERID = HttpContext.Current.Request["ORDERID"];
                    FREE = HttpContext.Current.Request["FREE"];
                    PAN = HttpContext.Current.Request["PAN"];
                    BANK = HttpContext.Current.Request["BANK"];
                    ARS = HttpContext.Current.Request["ARS"];
                    AUTHENT = HttpContext.Current.Request["AUTHENT"];

                    GenericHelper.LogFile("SuperPag.Agents.VBV::VBV::Step2::Arquivo não encontrado paymentAttemptId=" + attempt.paymentAttemptId.ToString() + " tid=" + TID + " lrForm=" + HttpContext.Current.Request.Form["LR"] + " lrQS=" + HttpContext.Current.Request.QueryString["LR"] + " lr=" + HttpContext.Current.Request["LR"], LogFileEntryType.Warning);
                }
                else
                {
                    XmlDocument xml = new XmlDocument();
                    FileStream fs1 = new FileStream(Path, FileMode.Open, FileAccess.Read);
                    using (fs1)
                    {
                        StreamReader sr = new StreamReader(fs1, System.Text.Encoding.GetEncoding("iso-8859-1"));
                        using (sr)
                        {
                            xml.Load(sr);
                        }
                    }
                    TID = xml.DocumentElement.GetElementsByTagName("TID").Item(0).InnerText;
                    LR = xml.DocumentElement.GetElementsByTagName("LR").Item(0).InnerText;
                    ARP = xml.DocumentElement.GetElementsByTagName("ARP").Item(0).InnerText;
                    PRICE = xml.DocumentElement.GetElementsByTagName("PRICE").Item(0).InnerText;
                    ORDERID = xml.DocumentElement.GetElementsByTagName("ORDERID").Item(0).InnerText;
                    FREE = xml.DocumentElement.GetElementsByTagName("FREE").Item(0).InnerText;
                    PAN = xml.DocumentElement.GetElementsByTagName("PAN").Item(0).InnerText;
                    BANK = xml.DocumentElement.GetElementsByTagName("BANK").Item(0).InnerText;
                    ARS = xml.DocumentElement.GetElementsByTagName("ARS").Item(0).InnerText;
                    AUTHENT = xml.DocumentElement.GetElementsByTagName("AUTHENT").Item(0).InnerText;
                }
                #endregion

                //Historico de Navegacao
                GenericHelper.SetOrderStatus(HttpContext.Current, WorkflowOrderStatus.AgentCalled, attempt.paymentFormId + "," + order.installmentQuantity + "," + (int)PaymentAgents.VBV + ",Step2(MpgReturn)");

                //VBVLog
                DPaymentAttemptVBVLog attemptVBVLog = new DPaymentAttemptVBVLog();
                attemptVBVLog.paymentAttemptId = attemptVBV.paymentAttemptId;
                attemptVBVLog.interfaceType = (int)PaymentAttemptVBVInterfaces.Mpg;
                attemptVBVLog.returnDate = DateTime.Now;
                attemptVBVLog.tid = attemptVBV.tid = TID;
                attemptVBVLog.lr = attemptVBV.lr = GenericHelper.ParseDecimalLR(GenericHelper.ParseInt(LR));
                attemptVBVLog.pan = attemptVBV.pan = PAN;
                attemptVBVLog.bank = attemptVBV.bank = GenericHelper.ParseInt(BANK);
                attemptVBVLog.ars = attemptVBV.ars = ARS;
                attemptVBVLog.authent = attemptVBV.authent = GenericHelper.ParseInt(AUTHENT);
                attemptVBVLog.arp = attemptVBV.arp = GenericHelper.ParseInt(ARP);
                DataFactory.PaymentAttemptVBVLog().Insert(attemptVBVLog);

                // verifico se o TID retornado no post é mesmo que está gravado
                // no registro recupera pelo paymentAttemptId
                if (TID != attemptVBV.tid)
                    GenericHelper.RedirectToErrorPage("TID inconsistente");

                // Checa preço do retorno com o da ordem para
                // certificar que não foi adulterado
                if (PRICE != null && attempt.price != (int.Parse(PRICE) / 100.0m))
                    GenericHelper.RedirectToErrorPage("Arquivo de retorno adulterado");

                // Recupera Session perdida
                HttpContext.Current.Session["PaymentAttemptId"] = attempt.paymentAttemptId;

                if (attemptVBV.lr == 0 || attemptVBV.lr == 11)
                {
                    // Transação autorizada.
                    DPaymentAgentSetupVBV agentsetup = DataFactory.PaymentAgentSetupVBV().Locate(attempt.paymentAgentSetupId);
                    if (agentsetup.autoCapture)
                    {
                        // Capturar transação.
                        Capture((int)PaymentAttemptVBVInterfaces.AutoCapture);
                        if (attempt.status == (int)PaymentAttemptStatus.Paid)
                            HttpContext.Current.Response.Redirect("~/finalization.aspx?id=" + attempt.paymentAttemptId);
                    }
                    else
                    {
                        // Não capturar a transação.
                        // Atualizo para pago, mas deixo o status VBV para pendente de captura.
                        attempt.status = (int)PaymentAttemptStatus.Paid;
                        attempt.lastUpdate = DateTime.Now;
                        if (!String.IsNullOrEmpty(attemptVBV.ars))
                            attempt.returnMessage = attemptVBV.ars;

                        attemptVBV.vbvStatus = (byte)PaymentAttemptVBVStatus.WaitingCapture;
                        DataFactory.PaymentAttemptVBV().Update(attemptVBV);
                        DataFactory.PaymentAttempt().Update(attempt);
                        GenericHelper.UpdateOrderStatusByAttemptStatus(order, attempt.status);
                        HttpContext.Current.Response.Redirect("~/finalization.aspx?id=" + attempt.paymentAttemptId);
                    }
                }
                else if (attemptVBV.lr == 203) //FalhaHttp -1
                {
                    // Erro durante a transação.
                    attemptVBV.vbvStatus = (byte)PaymentAttemptVBVStatus.Initial;
                    attempt.status = (int)PaymentAttemptStatus.Pending;
                    attempt.lastUpdate = DateTime.Now;
                    attempt.returnMessage = attemptVBV.ars;

                    DataFactory.PaymentAttempt().Update(attempt);
                    DataFactory.PaymentAttemptVBV().Update(attemptVBV);
                    GenericHelper.UpdateOrderStatusByAttemptStatus(order, attempt.status);
                }
                else
                {
                    // Transação não autorizada.
                    attemptVBV.vbvStatus = (byte)PaymentAttemptVBVStatus.End;
                    attempt.status = (int)PaymentAttemptStatus.NotPaid;
                    attempt.lastUpdate = DateTime.Now;
                    attempt.returnMessage = attemptVBV.ars;

                    DataFactory.PaymentAttempt().Update(attempt);
                    DataFactory.PaymentAttemptVBV().Update(attemptVBV);
                    GenericHelper.UpdateOrderStatusByAttemptStatus(order, attempt.status);
                }
                HttpContext.Current.Response.Redirect("~/Agents/VBV/popupclose.aspx?id=" + attempt.paymentAttemptId);
            }
            catch (ThreadAbortException) { }
            catch (Exception ex)
            {
                string message = "SuperPag.Agents.VBV::VBV::Step2 ";
                if (attempt != null)
                    message += "paymentattemptid =" + attempt.paymentAttemptId;
                message += " exception=" + ex.Message + " stack=" + ex.StackTrace;
                GenericHelper.LogFile(message, LogFileEntryType.Error);
            }
        }

        public override void Simulate()
        {
            attemptVBV.lr = 0;
            attemptVBV.arp = 1001;
            attemptVBV.vbvStatus = (byte)PaymentAttemptVBVStatus.End;
            DataFactory.PaymentAttemptVBV().Update(attemptVBV);

            base.Simulate();
        }
        
        public void CheckStatus()
        {
            bool sendPost = false;
            try
            {
                int oldAttemptStatus = this.attempt.status;

                ServerHttpHtmlRequisition post = new ServerHttpHtmlRequisition();
                post.Method = "POST";
                post.UpperKeys = false;
                post.Url = ConfigurationManager.AppSettings["VBVComponentUrl"] + "inquire.exe?";
                post.Parameters.Add("tid", attemptVBV.tid);
                post.Parameters.Add("merchid", "cfg" + attempt.paymentAgentSetupId.ToString());

                if (!post.Send("iso-8859-1"))
                    GenericHelper.LogFile("EasyPagObject::VBV::Sonda.cs::UpdateStatus storeId=" + order.storeId + " orderId=" + order.orderId + " paymentAttemptId=" + attempt.paymentAttemptId + " erro ao enviar requisição de consulta: " + post.Response, LogFileEntryType.Warning);

                VBVInquireReturn inquireReturn = Inquire(attemptVBV.tid, "cfg" + attempt.paymentAgentSetupId.ToString());
                if (String.IsNullOrEmpty(inquireReturn.tid))
                    return;
                if (String.IsNullOrEmpty(inquireReturn.ars))
                    inquireReturn.ars = "";

                DPaymentAttemptVBVLog attemptVBVLog = new DPaymentAttemptVBVLog();
                attemptVBVLog.paymentAttemptId = attemptVBV.paymentAttemptId;
                attemptVBVLog.interfaceType = (int)PaymentAttemptVBVInterfaces.Sonda;
                attemptVBVLog.returnDate = DateTime.Now;
                attemptVBVLog.tid = attemptVBV.tid = inquireReturn.tid;
                attemptVBVLog.lr = attemptVBV.lr = GenericHelper.ParseDecimalLR(inquireReturn.lr);
                attemptVBVLog.ars = attemptVBV.ars = inquireReturn.ars;
                attemptVBVLog.arp = attemptVBV.arp = inquireReturn.arp;
                attemptVBVLog.authent = attemptVBV.authent = inquireReturn.authent;
                attemptVBVLog.bank = attemptVBV.bank = inquireReturn.bank;
                attemptVBVLog.free = attemptVBV.free = inquireReturn.free;
                attemptVBVLog.pan = attemptVBV.pan = inquireReturn.pan;
                attemptVBVLog.price = attemptVBV.price = inquireReturn.price;
                DataFactory.PaymentAttemptVBVLog().Insert(attemptVBVLog);

                if (inquireReturn.lr == 0 && inquireReturn.ars.ToLower().Trim() == "capturada")
                {
                    // lr = 0 e ars = "Capturada": Transação Autorizada e Capturada
                    attemptVBV.vbvStatus = (byte)PaymentAttemptVBVStatus.End;
                    attempt.status = (int)PaymentAttemptStatus.Paid;
                    attempt.lastUpdate = DateTime.Now;
                    attempt.returnMessage = attemptVBV.ars;
                    sendPost = oldAttemptStatus != (int)PaymentAttemptStatus.Paid;
                    GenericHelper.LogFile("EasyPagObject::VBV::Sonda.cs::UpdateStatus storeId=" + order.storeId + " orderId=" + order.orderId + " paymentAttemptId=" + attempt.paymentAttemptId + " a tentativa de pagamento será atualizada para pago. RespostaVisa=" + post.Response, LogFileEntryType.Information);
                }
                else if (inquireReturn.lr == 0)
                {
                    // lr = 0 e ars = "Autorizada": Transação Autorizada, não capturada.
                    if (attempt.startTime >= DateTime.Now.AddDays(-5))
                    {
                        // manter como paga até a data limite de captura.
                        attemptVBV.vbvStatus = (byte)PaymentAttemptVBVStatus.WaitingCapture;
                        attempt.status = (int)PaymentAttemptStatus.PendingPaid;
                        attempt.lastUpdate = DateTime.Now;
                        attempt.returnMessage = attemptVBV.ars;
                        GenericHelper.LogFile("EasyPagObject::VBV::Sonda.cs::UpdateStatus storeId=" + order.storeId + " orderId=" + order.orderId + " paymentAttemptId=" + attempt.paymentAttemptId + " a tentativa de pagamento será atualizada para pendente pois necessita fazer a captura. RespostaVisa=" + post.Response, LogFileEntryType.Information);
                    }
                    else
                    {
                        // não é possível capturar essa transação pelo sistema, setar para expirada
                        attemptVBV.vbvStatus = (byte)PaymentAttemptVBVStatus.Expired;
                        attempt.status = (int)PaymentAttemptStatus.Pending;
                        attempt.lastUpdate = DateTime.Now;
                        attempt.returnMessage = attemptVBV.ars;
                        GenericHelper.LogFile("EasyPagObject::VBV::Sonda.cs::UpdateStatus storeId=" + order.storeId + " orderId=" + order.orderId + " paymentAttemptId=" + attempt.paymentAttemptId + " a tentativa de pagamento será atualizada para expirada", LogFileEntryType.Information);
                    }
                }
                else
                {
                    // lr != 0: Não foi aprovada
                    attemptVBV.vbvStatus = (byte)PaymentAttemptVBVStatus.End;
                    attempt.status = (int)PaymentAttemptStatus.NotPaid;
                    attempt.lastUpdate = DateTime.Now;
                    attempt.returnMessage = attemptVBV.ars;
                }

                // atualizo parametros da sonda
                attemptVBV.dataSonda = DateTime.Now;
                attemptVBV.qtdSonda++;
                attemptVBV.sondaOffline = true;
                DataFactory.PaymentAttemptVBV().Update(attemptVBV);

                // se houve mudança de status, atualizo a attempt
                if (attempt.status != oldAttemptStatus)
                {
                    attempt.TruncateStringFields();
                    attempt.lastUpdate = DateTime.Now;
                    DataFactory.PaymentAttempt().Update(attempt);
                    GenericHelper.UpdateOrderStatusByAttemptStatus(order, attempt.status);
                }

                // se houve mudança de status para pago, envio posts
                if (sendPost)
                {
                    DStore store = DataFactory.Store().Locate(order.storeId);
                    DHandshakeConfiguration handshakeConfiguration = DataFactory.HandshakeConfiguration().Locate(store.handshakeConfigurationId);
                    Ensure.IsNotNull(handshakeConfiguration, "Configuração de handshake {0} não encontrada, o post nao será enviado", store.handshakeConfigurationId);

                    SuperPag.Handshake.Helper.SendFinalizationPost(attempt.paymentAttemptId);

                    if (handshakeConfiguration.autoPaymentConfirm)
                        SuperPag.Handshake.Helper.SendPaymentPost(attempt.paymentAttemptId);
                }
            }
            catch (Exception ex)
            {
                GenericHelper.LogFile("EasyPagObject::VBV::Sonda.cs::UpdateStatus orderId=" + this.order.orderId + " paymentAttemptId=" + this.attempt.paymentAttemptId + " " + ex.Message, LogFileEntryType.Error);
            }
        }
        public void CancelCapture()
        {
            attempt.status = (int)PaymentAttemptStatus.Canceled;
            attemptVBV.vbvStatus = (byte)PaymentAttemptVBVStatus.End;
            DataFactory.PaymentAttemptVBV().Update(attemptVBV);
            attempt.lastUpdate = DateTime.Now;
            DataFactory.PaymentAttempt().Update(attempt);
        }
        public void Capture(int interfaceType)
        {
            VBVCaptureReturn captureReturn = Capture(attemptVBV.tid, attemptVBV.free, "cfg" + agentsetup.paymentAgentSetupId.ToString());
            if (captureReturn == null || String.IsNullOrEmpty(captureReturn.tid))
                return;

            //VBVLog
            DPaymentAttemptVBVLog attemptVBVLog = new DPaymentAttemptVBVLog();
            attemptVBVLog.paymentAttemptId = attemptVBV.paymentAttemptId;
            attemptVBVLog.interfaceType = interfaceType;
            attemptVBVLog.returnDate = DateTime.Now;
            attemptVBVLog.tid = attemptVBV.tid = captureReturn.tid;
            attemptVBVLog.lr = attemptVBV.lr = GenericHelper.ParseDecimalLR(captureReturn.lr);
            attemptVBVLog.ars = attemptVBV.ars = captureReturn.ars;
            attemptVBVLog.cap = attemptVBV.cap = captureReturn.cap;
            DataFactory.PaymentAttemptVBVLog().Insert(attemptVBVLog);

            if (captureReturn.lr == 0 || captureReturn.lr == 3)
            {
                attempt.status = (int)PaymentAttemptStatus.Paid;
                attempt.lastUpdate = DateTime.Now;
                attemptVBV.vbvStatus = (int)PaymentAttemptVBVStatus.End;
                GenericHelper.LogFile("EasyPagObject::VBV::VBV.cs::Capture paymentAttemptId=" + attempt.paymentAttemptId + " atualizando para status pago. ARP=" + attemptVBV.arp, LogFileEntryType.Information);
                DataFactory.PaymentAttempt().Update(attempt);
                DataFactory.PaymentAttemptVBV().Update(attemptVBV);
                GenericHelper.UpdateOrderStatusByAttemptStatus(order, attempt.status);

                DStore store = DataFactory.Store().Locate(order.storeId);
                DHandshakeConfiguration handshakeConfiguration = DataFactory.HandshakeConfiguration().Locate(store.handshakeConfigurationId);
                if (handshakeConfiguration != null && (interfaceType == (int)PaymentAttemptVBVInterfaces.CaptureJob || interfaceType == (int)PaymentAttemptVBVInterfaces.Sonda))
                {
                    SuperPag.Handshake.Helper.SendFinalizationPost(attempt.paymentAttemptId);

                    if (handshakeConfiguration.autoPaymentConfirm)
                        SuperPag.Handshake.Helper.SendPaymentPost(attempt.paymentAttemptId);
                }
            }
            else
            {
                // Caso não seja capturado com sucesso, quem deve atualizar o status é a Sonda.
                // Isso porque o retorno da Visa para a captura é limitado.
                this.CheckStatus();
            }
        }

        public static void CheckStatus(DPaymentAttemptVBV[] arrAttemptVBV)
        {
            foreach (DPaymentAttemptVBV attemptVBV in arrAttemptVBV)
                (new VBV(attemptVBV)).CheckStatus();
        }

        private void BuildRequestXml()
        {
            System.IO.FileStream fs = new System.IO.FileStream(System.Configuration.ConfigurationManager.AppSettings["VBVDirectory"].ToString() + "\\requests\\" + attemptVBV.tid + ".xml", System.IO.FileMode.Create);
            try
            {
                StringBuilder tidfile = new StringBuilder();
                tidfile.Append("<MESSAGE>");
                tidfile.AppendFormat("<PRICE>{0}</PRICE>", attemptVBV.price);
                tidfile.Append("<AUTHENTTYPE>0</AUTHENTTYPE>");
                tidfile.Append("</MESSAGE>");
                byte[] XML = System.Text.Encoding.ASCII.GetBytes(tidfile.ToString());
                fs.Write(XML, 0, XML.Length);
            }
            finally
            {
                fs.Close();
            }
        }
        private string GenerateTID(string businessNumber, int paymentFormId, int installmentQuantity, byte installmentType)
        {
            string tid = "";
            DateTime tidDate = DateTime.Now;
            tid = businessNumber.ToString().PadLeft(10, '0').Substring(4, 5);
            tid += tidDate.Year.ToString("0000").Substring(3, 1);
            tid += tidDate.DayOfYear.ToString("000");
            tid += tidDate.ToString("hhmmss");
            tid += tidDate.Millisecond.ToString().Substring(0, 1);
            if (paymentFormId == (int)PaymentForms.VisaElectronVBV)
            {
                if (installmentQuantity != 1)
                    Ensure.IsNotNullPage(null, "Parcelamento inválido para o meio de pagamento Visa Electron.");
                tid += "A001"; // Cartões VISA ELECTRON

            }
            else
            {
                if (installmentQuantity == 1)
                    tid += "1001"; // Cartões VISA à vista
                else
                {
                    //Enviar tipo de pagamento especifico
                    tid += (installmentType == (byte)InstallmentType.Emissor ? "3" : "2") + installmentQuantity.ToString("000");
                }
            }
            return tid;
        }
        public VBVInquireReturn Inquire(string tid, string cfg)
        {
            ServerHttpHtmlRequisition post = new ServerHttpHtmlRequisition();
            post.Method = "POST";
            post.UpperKeys = false;
            post.Url = ConfigurationManager.AppSettings["VBVComponentUrl"] + "inquire.exe?";
            post.Parameters.Add("tid", tid);
            post.Parameters.Add("merchid", cfg);

            if (!post.Send("iso-8859-1"))
            {
                GenericHelper.LogFile("EasyPagObject::VBV::VBV.cs::Inquire tid=" + tid + " cfg=" + cfg + " erro ao enviar requisição de consulta: " + post.Response, LogFileEntryType.Warning);
                return null;
            }

            ParseHTMLVBV parse = new ParseHTMLVBV();
            parse.Source = post.Response;
            return parse.GetVBVInquireReturn();
        }
        private VBVCaptureReturn Capture(string tid, string free, string cfg)
        {
            ServerHttpHtmlRequisition post = new ServerHttpHtmlRequisition();
            post.Method = "POST";
            post.UpperKeys = false;
            post.Url = ConfigurationManager.AppSettings["VBVComponentUrl"] + "capture.exe?";
            post.Parameters.Add("tid", tid);
            post.Parameters.Add("merchid", cfg);
            post.Parameters.Add("free", free);

            if (!post.Send("iso-8859-1"))
            {
                GenericHelper.LogFile("EasyPagObject::VBV::VBV.cs::Capture tid=" + tid + " cfg=" + cfg + " free=" + free + " erro ao enviar requisição de captura: " + post.Response, LogFileEntryType.Warning);
                return null;
            }

            ParseHTMLVBV parse = new ParseHTMLVBV();
            parse.Source = post.Response;
            return parse.GetVBVCaptureReturn();
        }
        private VBVCancelReturn Cancel(string tid, string cfg)
        {
            //TODO: VBVCancel
            return null;
        }
    }
}
