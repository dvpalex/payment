using System;
using System.Collections.Generic;
using System.Text;
using Resp = SuperPag.Helper.Xml.Response;
using SuperPag.Data.Messages;
using System.IO;
using System.Configuration;
using SuperPag.Business;
using SuperPag.Business.Messages;
using SuperPag.Helper.Xml.Request;
using SuperPag.Helper.Xml.Response;
using SuperPag.Helper.Xml;
using SuperPag.Data;
using System.Collections;

namespace SuperPag.Agents.ContaCorrente
{
    public class ContaCorrente
    {
        public static Resp.responseOrdersOrderPaymentInstallment ProcessPayment(DOrder order, DOrderInstallment orderInstallment, DPaymentAttempt attempt, genericPaymentContaCorrenteInformation paymentInfo, requestOrderPaymentsPayment payment)
        {
            try
            {
                Ensure.IsNotNull(order, "O pedido deve ser informado");
                Ensure.IsNotNull(paymentInfo, "Os dados do cartão devem ser informados");

               
                responseOrdersOrderPaymentInstallment ObjInstallment = new responseOrdersOrderPaymentInstallment();
                responseOrdersOrderPaymentInstallmentPaymentFormDetail pfDetail = new SuperPag.Helper.Xml.Response.responseOrdersOrderPaymentInstallmentPaymentFormDetail();
                Resp.responseOrdersOrderPaymentInstallmentPaymentFormDetailContaCorrenteInformation ccInfo = new Resp.responseOrdersOrderPaymentInstallmentPaymentFormDetailContaCorrenteInformation();
                genericPaymentContaCorrenteInformationCSU ObjpaymentInfo = new genericPaymentContaCorrenteInformationCSU();
                DPaymentAttempt dPaymentAttempt;

                int NrDocum = PaymentAttemptContaCorrente.GetInstance().Insert(new MPaymentAttemptContaCorrente(attempt.paymentAttemptId, string.Empty, null, null, string.Empty, paymentInfo.DigVerAg, paymentInfo.NumAgencia, (int)ContaCorrenteStatus.AguardandoPagamento, paymentInfo.DigVerCont, paymentInfo.NumContCorrent, paymentInfo.Vencimento, null, null, attempt.price, null, null, null, string.Empty, null));
               
                ObjpaymentInfo.DigVerAg = paymentInfo.DigVerAg;
                ObjpaymentInfo.DigVerCont = paymentInfo.DigVerCont;
                ObjpaymentInfo.NumAgencia = paymentInfo.NumAgencia;
                ObjpaymentInfo.NumContCorrent = paymentInfo.NumContCorrent;
                ObjpaymentInfo.Vencimento = paymentInfo.Vencimento;

                CriaArquivo(order, orderInstallment, ObjpaymentInfo, attempt, payment, NrDocum);

                //Atualiza o status do pagamento
                dPaymentAttempt = DataFactory.PaymentAttempt().Locate(attempt.paymentAttemptId);
                dPaymentAttempt.status = (int)PaymentAttemptStatus.Delivered;
                DataFactory.PaymentAttempt().Update(dPaymentAttempt);

                ccInfo.ReturnMessage = "OK";
                pfDetail.Item = ccInfo;
                ObjInstallment.paymentFormDetail = pfDetail;
                ObjInstallment.number = (ulong)orderInstallment.installmentNumber;
                ObjInstallment.date = DateTime.Today;
                ObjInstallment.dateSpecified = true;
                ObjInstallment.paymentDateSpecified = (attempt.status == (int)PaymentAttemptStatus.Pending);
                ObjInstallment.paymentDate = DateTime.Today;
                ObjInstallment.status = (int)OrderStatus.Delivered;

                return ObjInstallment;
            }
            catch (Exception ex)
            {
                SuperPag.Helper.GenericHelper.LogFile("SuperPagWS::ContaCorrente.cs::ProcessPayment " + ex.Message + "  " + attempt.paymentAttemptId.ToString() + "  " + XmlHelper.GetXml(typeof(genericPaymentContaCorrenteInformation), paymentInfo), LogFileEntryType.Error);
                return null;
            }
        }

        /// <summary>
        /// Metodo para conta corrente CSU
        /// </summary>
        /// <param name="order"></param>
        /// <param name="orderInstallment"></param>
        /// <param name="attempt"></param>
        /// <param name="paymentInfo"></param>
        /// <param name="payment"></param>
        /// <returns></returns>
        public static Resp.responseOrdersOrderPaymentInstallment ProcessPayment(DOrder order, DOrderInstallment orderInstallment, DPaymentAttempt attempt, genericPaymentContaCorrenteInformationCSU paymentInfo, requestOrderPaymentsPayment payment)
        {
            try
            {

                Ensure.IsNotNull(order, "O pedido deve ser informado");
                Ensure.IsNotNull(paymentInfo, "Os dados do cartão devem ser informados");

                responseOrdersOrderPaymentInstallment ObjInstallment = new responseOrdersOrderPaymentInstallment();
                responseOrdersOrderPaymentInstallmentPaymentFormDetail pfDetail = new SuperPag.Helper.Xml.Response.responseOrdersOrderPaymentInstallmentPaymentFormDetail();
                Resp.responseOrdersOrderPaymentInstallmentPaymentFormDetailContaCorrenteInformation ccInfo = new Resp.responseOrdersOrderPaymentInstallmentPaymentFormDetailContaCorrenteInformation();
                DPaymentAttempt dPaymentAttempt;

                int NrDocum = PaymentAttemptContaCorrente.GetInstance().Insert(new MPaymentAttemptContaCorrente(attempt.paymentAttemptId, string.Empty, null, paymentInfo.CodigoLogo, paymentInfo.Plastico, paymentInfo.DigVerAg, paymentInfo.NumAgencia, (int)ContaCorrenteStatus.AguardandoPagamento, paymentInfo.DigVerCont, paymentInfo.NumContCorrent, paymentInfo.Vencimento, null, null, attempt.price, null, null, null, string.Empty, null));

                CriaArquivo(order, orderInstallment, paymentInfo, attempt, payment, NrDocum);

                //Atualiza o status do pagamento
                dPaymentAttempt = DataFactory.PaymentAttempt().Locate(attempt.paymentAttemptId);
                dPaymentAttempt.status = (int)PaymentAttemptStatus.Delivered;
                DataFactory.PaymentAttempt().Update(dPaymentAttempt);

                ccInfo.PaymentAttemptId = attempt.paymentAttemptId.ToString();
                pfDetail.Item = ccInfo;
                ObjInstallment.paymentFormDetail = pfDetail;
                ObjInstallment.number = (ulong)orderInstallment.installmentNumber;
                ObjInstallment.date = DateTime.Today;
                ObjInstallment.dateSpecified = true;
                ObjInstallment.paymentDateSpecified = (attempt.status == (int)PaymentAttemptStatus.Pending);
                ObjInstallment.paymentDate = DateTime.Today;
                ObjInstallment.status = (int)OrderStatus.Delivered;

                return ObjInstallment;
            }
            catch (Exception ex)
            {
                SuperPag.Helper.GenericHelper.LogFile("SuperPagWS::ContaCorrente.cs::ProcessPayment " + ex.Message + "  " + attempt.paymentAttemptId.ToString() + "  " + XmlHelper.GetXml(typeof(genericPaymentContaCorrenteInformation), paymentInfo), LogFileEntryType.Error);
                return null;
            }
        }

        private static void CriaArquivo(DOrder order, DOrderInstallment orderInstallment, genericPaymentContaCorrenteInformationCSU paymentInfo, DPaymentAttempt attempt, requestOrderPaymentsPayment payment, int NrDocum)
        {

            StreamWriter ObjStreamWriter;
            string PathContaCorrente = ConfigurationManager.AppSettings["PathContaCorrente"].ToString();
            MPaymentAgentSetupDebitoContaCorrente ObjConteCorrente = PaymentAgentSetupDebitoContaCorrente.GetInstance().Locate(Convert.ToInt32(order.orderId));
            MConsumer ObjMConsumer = Consumer.Locate(order.consumerId);

            //CONCATENO A PATH DO WEB.CONFIG COM A PATH DA TABELA PaymentAgentSetupDebitoContaCorrente
            PathContaCorrente += ObjConteCorrente.Path + "INBOXSUPERPAG\\";

            PathContaCorrente += "\\REM_CC_" + DateTime.Now.ToString("dd_MM_yyyy") + "_" + order.storeId.ToString() + "_1.REM";

            // Layout Igual para Todos os Bancos
            #region Layout do arquivo
            switch (string.Format("{0}_{1}_{2}", ObjConteCorrente.Layout, ObjConteCorrente.Versao, ObjConteCorrente.Data.ToString("dd/MM/yyyy")))
            {
                case "400_01_08/09/2004"://layout_versao_datadoarquivo
                    //se o arquivo ja existe adiciona uma nova linha
                    if (File.Exists(PathContaCorrente))
                    {
                        int AllLines = GetLength(PathContaCorrente), contador = 0;

                        StringBuilder ObjStringBuilderCont = new StringBuilder();
                        StreamReader ObjStreamReaderCont = new StreamReader(PathContaCorrente);
                        IList<MCnab> Lista;
                        while (ObjStreamReaderCont.Peek() > 0)
                        {
                            contador++;
                            string strLine = ObjStreamReaderCont.ReadLine();
                            if (contador < AllLines)
                            {
                                ObjStringBuilderCont.Append(strLine);
                                ObjStringBuilderCont.AppendLine();
                            }
                        }
                        ObjStreamReaderCont.Close();

                        File.Delete(PathContaCorrente);
                        File.WriteAllText(PathContaCorrente, ObjStringBuilderCont.ToString());

                        ObjStreamWriter = new StreamWriter(PathContaCorrente, true);

                        #region Registro de transação - tipo 1
                        Lista = Cnab.GetCnab(ObjConteCorrente.Layout, ObjConteCorrente.Versao, ObjConteCorrente.Data, 3, 1);
                        Lista[0].Valor = "1";
                        Lista[1].Valor = paymentInfo.NumAgencia;
                        Lista[2].Valor = paymentInfo.DigVerAg;
                        Lista[3].Valor = string.Empty;//verificar o q é isso
                        Lista[4].Valor = paymentInfo.NumContCorrent;
                        Lista[5].Valor = paymentInfo.DigVerCont;
                        Lista[6].Valor = "0";

                        Lista[7].Valor = ObjConteCorrente.Carteira;
                        Lista[8].Valor = ObjConteCorrente.Agencia;
                        Lista[9].Valor = ObjConteCorrente.ContaCorrente;

                        Lista[10].Valor = "T" + NrDocum.ToString();
                        Lista[11].Valor = Convert.ToInt32(ObjConteCorrente.NumBanco).ToString();
                        Lista[12].Valor = string.Empty;
                        Lista[13].Valor = string.Empty;
                        Lista[14].Valor = string.Empty;
                        Lista[15].Valor = string.Empty;
                        Lista[16].Valor = "1";
                        Lista[17].Valor = "N";
                        Lista[18].Valor = string.Empty;
                        Lista[19].Valor = string.Empty;
                        Lista[20].Valor = "2";
                        Lista[21].Valor = string.Empty;
                        Lista[22].Valor = "1";
                        Lista[23].Valor = "1";
                        Lista[24].Valor = paymentInfo.Vencimento.Date.ToString("ddMMyy");
                        Lista[25].Valor = attempt.price.ToString().Replace(",", string.Empty);
                        Lista[26].Valor = Convert.ToInt32(ObjConteCorrente.NumBanco).ToString();
                        Lista[27].Valor = "0785";
                        Lista[28].Valor = "99";
                        Lista[29].Valor = "N";
                        Lista[30].Valor = DateTime.Now.Date.ToString("ddMMyy"); ;
                        Lista[31].Valor = string.Empty;
                        Lista[32].Valor = string.Empty;
                        Lista[33].Valor = string.Empty;
                        Lista[34].Valor = string.Empty;
                        Lista[35].Valor = string.Empty;
                        Lista[36].Valor = string.Empty;
                        Lista[37].Valor = string.Empty;

                        //verifica se é cpf ou cnpj
                        if (ObjMConsumer.CNPJ != null && !ObjMConsumer.CNPJ.Equals(string.Empty))
                        {
                            Lista[38].Valor = "02";
                        }
                        else if (ObjMConsumer.CPF != null)
                        {
                            Lista[38].Valor = "01";
                        }

                        //verifica se é cpf ou cnpj
                        if (ObjMConsumer.CNPJ != null && !ObjMConsumer.CNPJ.Equals(string.Empty))
                        {
                            Lista[39].Valor = ObjMConsumer.CNPJ;
                        }
                        else if (ObjMConsumer.CPF != null)
                        {
                            Lista[39].Valor = ObjMConsumer.CPF;
                        }

                        Lista[40].Valor = ObjMConsumer.Name;
                        Lista[41].Valor = string.Empty;
                        Lista[42].Valor = string.Empty;
                        Lista[43].Valor = string.Empty;
                        Lista[44].Valor = string.Empty;
                        Lista[45].Valor = string.Empty;
                        Lista[46].Valor = Convert.ToString(AllLines);

                        ObjStreamWriter.WriteLine(MCnab.GetLine(Lista));


                        #endregion

                        #region parametros Trailler

                        Lista = Cnab.GetCnab(ObjConteCorrente.Layout, ObjConteCorrente.Versao, ObjConteCorrente.Data, 19, 1);

                        Lista[0].Valor = "9";
                        Lista[1].Valor = string.Empty;
                        Lista[2].Valor = Convert.ToString(AllLines + 1);

                        ObjStreamWriter.WriteLine(MCnab.GetLine(Lista));

                        #endregion

                        ObjStreamWriter.Close();
                    }
                    //se o arquivo não existe adicina o Header
                    else
                    {
                        ObjStreamWriter = File.CreateText(PathContaCorrente);

                        IList<MCnab> Lista;
                        Lista = Cnab.GetCnab(ObjConteCorrente.Layout, ObjConteCorrente.Versao, ObjConteCorrente.Data, 1, 1);

                        #region Parametros Header
                        Lista[0].Valor = "0";
                        Lista[1].Valor = "1";
                        Lista[2].Valor = "REMESSA";
                        Lista[3].Valor = "O1";
                        Lista[4].Valor = "COBRANCA";
                        Lista[5].Valor = ObjConteCorrente.CodConvenio;
                        Lista[6].Valor = ObjConteCorrente.NEmpresa;
                        Lista[7].Valor = Convert.ToInt32(ObjConteCorrente.NumBanco).ToString();
                        Lista[8].Valor = ObjConteCorrente.NBanco;
                        Lista[9].Valor = DateTime.Now.Date.ToString("ddMMyy");
                        Lista[10].Valor = string.Empty;
                        Lista[11].Valor = "MX";
                        Lista[12].Valor = ObjConteCorrente.NumSeq.ToString();
                        Lista[13].Valor = string.Empty;
                        Lista[14].Valor = "1";

                        ObjStreamWriter.WriteLine(MCnab.GetLine(Lista));
                        #endregion

                        #region Registro de transação - tipo 1
                        Lista = Cnab.GetCnab(ObjConteCorrente.Layout, ObjConteCorrente.Versao, ObjConteCorrente.Data, 3, 1);
                        Lista[0].Valor = "1";
                        Lista[1].Valor = paymentInfo.NumAgencia;
                        Lista[2].Valor = paymentInfo.DigVerAg;
                        Lista[3].Valor = string.Empty;//verificar o q é isso
                        Lista[4].Valor = paymentInfo.NumContCorrent;
                        Lista[5].Valor = paymentInfo.DigVerCont;
                        Lista[6].Valor = "0";

                        Lista[7].Valor = ObjConteCorrente.Carteira;
                        Lista[8].Valor = ObjConteCorrente.Agencia;
                        Lista[9].Valor = ObjConteCorrente.ContaCorrente;

                        Lista[10].Valor = "T" + NrDocum.ToString();
                        Lista[11].Valor = Convert.ToInt32(ObjConteCorrente.NumBanco).ToString();
                        Lista[12].Valor = string.Empty;
                        Lista[13].Valor = string.Empty;
                        Lista[14].Valor = string.Empty;
                        Lista[15].Valor = string.Empty;
                        Lista[16].Valor = "1";
                        Lista[17].Valor = "N";
                        Lista[18].Valor = string.Empty;
                        Lista[19].Valor = string.Empty;
                        Lista[20].Valor = "2";
                        Lista[21].Valor = string.Empty;
                        Lista[22].Valor = "1";
                        Lista[23].Valor = "1";
                        Lista[24].Valor = paymentInfo.Vencimento.Date.ToString("ddMMyy");
                        Lista[25].Valor = attempt.price.ToString().Replace(",", string.Empty);
                        Lista[26].Valor = Convert.ToInt32(ObjConteCorrente.NumBanco).ToString();
                        Lista[27].Valor = "0785";
                        Lista[28].Valor = "99";
                        Lista[29].Valor = "N";
                        Lista[30].Valor = DateTime.Now.Date.ToString("ddMMyy"); ;
                        Lista[31].Valor = string.Empty;
                        Lista[32].Valor = string.Empty;
                        Lista[33].Valor = string.Empty;
                        Lista[34].Valor = string.Empty;
                        Lista[35].Valor = string.Empty;
                        Lista[36].Valor = string.Empty;
                        Lista[37].Valor = string.Empty;

                        //verifica se é cpf ou cnpj
                        if (ObjMConsumer.CNPJ != null && !ObjMConsumer.CNPJ.Equals(string.Empty))
                        {
                            Lista[38].Valor = "02";
                        }
                        else if (ObjMConsumer.CPF != null)
                        {
                            Lista[38].Valor = "01";
                        }

                        //verifica se é cpf ou cnpj
                        if (ObjMConsumer.CNPJ != null && !ObjMConsumer.CNPJ.Equals(string.Empty))
                        {
                            Lista[39].Valor = ObjMConsumer.CNPJ;
                        }
                        else if (ObjMConsumer.CPF != null)
                        {
                            Lista[39].Valor = ObjMConsumer.CPF;
                        }

                        Lista[40].Valor = ObjMConsumer.Name;
                        Lista[41].Valor = string.Empty;
                        Lista[42].Valor = string.Empty;
                        Lista[43].Valor = string.Empty;
                        Lista[44].Valor = string.Empty;
                        Lista[45].Valor = string.Empty;
                        Lista[46].Valor = "2";

                        ObjStreamWriter.WriteLine(MCnab.GetLine(Lista));
                        #endregion

                        #region parametros Trailler

                        Lista = Cnab.GetCnab(ObjConteCorrente.Layout, ObjConteCorrente.Versao, ObjConteCorrente.Data, 19, 1);

                        Lista[0].Valor = "9";
                        Lista[1].Valor = string.Empty;
                        Lista[2].Valor = "3";

                        ObjStreamWriter.WriteLine(MCnab.GetLine(Lista));

                        #endregion

                        ObjStreamWriter.Close();

                    }
                    break;
            }
            #endregion

            #region "Layouts Antigos"

            /*
            switch (ObjConteCorrente.NumBanco)
            {
                case BankNumber.Bradesco:
                    #region Layout do arquivo
                    switch (string.Format("{0}_{1}_{2}", ObjConteCorrente.Layout, ObjConteCorrente.Versao, ObjConteCorrente.Data.ToString("dd/MM/yyyy")))
                    {
                        case "400_01_08/09/2004"://layout_versao_datadoarquivo
                            //se o arquivo ja existe adiciona uma nova linha
                            if (File.Exists(PathContaCorrente))
                            {
                                int AllLines = GetLength(PathContaCorrente), contador = 0;

                                StringBuilder ObjStringBuilderCont = new StringBuilder();
                                StreamReader ObjStreamReaderCont = new StreamReader(PathContaCorrente);
                                IList<MCnab> Lista;
                                while (ObjStreamReaderCont.Peek() > 0)
                                {
                                    contador++;
                                    string strLine = ObjStreamReaderCont.ReadLine();
                                    if (contador < AllLines)
                                    {
                                        ObjStringBuilderCont.Append(strLine);
                                        ObjStringBuilderCont.AppendLine();
                                    }
                                }
                                ObjStreamReaderCont.Close();

                                File.Delete(PathContaCorrente);
                                File.WriteAllText(PathContaCorrente, ObjStringBuilderCont.ToString());

                                ObjStreamWriter = new StreamWriter(PathContaCorrente, true);

                                #region Registro de transação - tipo 1
                                Lista = Cnab.GetCnab(ObjConteCorrente.Layout, ObjConteCorrente.Versao, ObjConteCorrente.Data, 3, 1);
                                Lista[0].Valor = "1";
                                Lista[1].Valor = paymentInfo.NumAgencia;
                                Lista[2].Valor = paymentInfo.DigVerAg;
                                Lista[3].Valor = string.Empty;//verificar o q é isso
                                Lista[4].Valor = paymentInfo.NumContCorrent;
                                Lista[5].Valor = paymentInfo.DigVerCont;
                                Lista[6].Valor = "0";

                                Lista[7].Valor = ObjConteCorrente.Carteira;
                                Lista[8].Valor = ObjConteCorrente.Agencia;
                                Lista[9].Valor = ObjConteCorrente.ContaCorrente;

                                Lista[10].Valor = "T" + NrDocum.ToString();
                                Lista[11].Valor = Convert.ToInt32(ObjConteCorrente.NumBanco).ToString();
                                Lista[12].Valor = string.Empty;
                                Lista[13].Valor = string.Empty;
                                Lista[14].Valor = string.Empty;
                                Lista[15].Valor = string.Empty;
                                Lista[16].Valor = "1";
                                Lista[17].Valor = "N";
                                Lista[18].Valor = string.Empty;
                                Lista[19].Valor = string.Empty;
                                Lista[20].Valor = "2";
                                Lista[21].Valor = string.Empty;
                                Lista[22].Valor = "1";
                                Lista[23].Valor = "1";
                                Lista[24].Valor = paymentInfo.Vencimento.Date.ToString("ddMMyy");
                                Lista[25].Valor = attempt.price.ToString().Replace(",", string.Empty);
                                Lista[26].Valor = Convert.ToInt32(ObjConteCorrente.NumBanco).ToString();
                                Lista[27].Valor = "0785";
                                Lista[28].Valor = "99";
                                Lista[29].Valor = "N";
                                Lista[30].Valor = DateTime.Now.Date.ToString("ddMMyy"); ;
                                Lista[31].Valor = string.Empty;
                                Lista[32].Valor = string.Empty;
                                Lista[33].Valor = string.Empty;
                                Lista[34].Valor = string.Empty;
                                Lista[35].Valor = string.Empty;
                                Lista[36].Valor = string.Empty;
                                Lista[37].Valor = string.Empty;

                                //verifica se é cpf ou cnpj
                                if (ObjMConsumer.CNPJ != null && !ObjMConsumer.CNPJ.Equals(string.Empty))
                                {
                                    Lista[38].Valor = "02";
                                }
                                else if (ObjMConsumer.CPF != null)
                                {
                                    Lista[38].Valor = "01";
                                }

                                //verifica se é cpf ou cnpj
                                if (ObjMConsumer.CNPJ != null && !ObjMConsumer.CNPJ.Equals(string.Empty))
                                {
                                    Lista[39].Valor = ObjMConsumer.CNPJ;
                                }
                                else if (ObjMConsumer.CPF != null)
                                {
                                    Lista[39].Valor = ObjMConsumer.CPF;
                                }

                                Lista[40].Valor = ObjMConsumer.Name;
                                Lista[41].Valor = string.Empty;
                                Lista[42].Valor = string.Empty;
                                Lista[43].Valor = string.Empty;
                                Lista[44].Valor = string.Empty;
                                Lista[45].Valor = string.Empty;
                                Lista[46].Valor = Convert.ToString(AllLines);

                                ObjStreamWriter.WriteLine(MCnab.GetLine(Lista));


                                #endregion

                                #region parametros Trailler

                                Lista = Cnab.GetCnab(ObjConteCorrente.Layout, ObjConteCorrente.Versao, ObjConteCorrente.Data, 19, 1);

                                Lista[0].Valor = "9";
                                Lista[1].Valor = string.Empty;
                                Lista[2].Valor = Convert.ToString(AllLines + 1);

                                ObjStreamWriter.WriteLine(MCnab.GetLine(Lista));

                                #endregion

                                ObjStreamWriter.Close();
                            }
                            //se o arquivo não existe adicina o Header
                            else
                            {
                                ObjStreamWriter = File.CreateText(PathContaCorrente);

                                IList<MCnab> Lista;
                                Lista = Cnab.GetCnab(ObjConteCorrente.Layout, ObjConteCorrente.Versao, ObjConteCorrente.Data, 1, 1);

                                #region Parametros Header
                                Lista[0].Valor = "0";
                                Lista[1].Valor = "1";
                                Lista[2].Valor = "REMESSA";
                                Lista[3].Valor = "O1";
                                Lista[4].Valor = "COBRANCA";
                                Lista[5].Valor = ObjConteCorrente.CodConvenio;
                                Lista[6].Valor = ObjConteCorrente.NEmpresa;
                                Lista[7].Valor = Convert.ToInt32(ObjConteCorrente.NumBanco).ToString();
                                Lista[8].Valor = ObjConteCorrente.NBanco;
                                Lista[9].Valor = DateTime.Now.Date.ToString("ddMMyy");
                                Lista[10].Valor = string.Empty;
                                Lista[11].Valor = "MX";
                                Lista[12].Valor = ObjConteCorrente.NumSeq.ToString();
                                Lista[13].Valor = string.Empty;
                                Lista[14].Valor = "1";

                                ObjStreamWriter.WriteLine(MCnab.GetLine(Lista));
                                #endregion

                                #region Registro de transação - tipo 1
                                Lista = Cnab.GetCnab(ObjConteCorrente.Layout, ObjConteCorrente.Versao, ObjConteCorrente.Data, 3, 1);
                                Lista[0].Valor = "1";
                                Lista[1].Valor = paymentInfo.NumAgencia;
                                Lista[2].Valor = paymentInfo.DigVerAg;
                                Lista[3].Valor = string.Empty;//verificar o q é isso
                                Lista[4].Valor = paymentInfo.NumContCorrent;
                                Lista[5].Valor = paymentInfo.DigVerCont;
                                Lista[6].Valor = "0";

                                Lista[7].Valor = ObjConteCorrente.Carteira;
                                Lista[8].Valor = ObjConteCorrente.Agencia;
                                Lista[9].Valor = ObjConteCorrente.ContaCorrente;

                                Lista[10].Valor = "T" + NrDocum.ToString();
                                Lista[11].Valor = Convert.ToInt32(ObjConteCorrente.NumBanco).ToString();
                                Lista[12].Valor = string.Empty;
                                Lista[13].Valor = string.Empty;
                                Lista[14].Valor = string.Empty;
                                Lista[15].Valor = string.Empty;
                                Lista[16].Valor = "1";
                                Lista[17].Valor = "N";
                                Lista[18].Valor = string.Empty;
                                Lista[19].Valor = string.Empty;
                                Lista[20].Valor = "2";
                                Lista[21].Valor = string.Empty;
                                Lista[22].Valor = "1";
                                Lista[23].Valor = "1";
                                Lista[24].Valor = paymentInfo.Vencimento.Date.ToString("ddMMyy");
                                Lista[25].Valor = attempt.price.ToString().Replace(",", string.Empty);
                                Lista[26].Valor = Convert.ToInt32(ObjConteCorrente.NumBanco).ToString();
                                Lista[27].Valor = "0785";
                                Lista[28].Valor = "99";
                                Lista[29].Valor = "N";
                                Lista[30].Valor = DateTime.Now.Date.ToString("ddMMyy"); ;
                                Lista[31].Valor = string.Empty;
                                Lista[32].Valor = string.Empty;
                                Lista[33].Valor = string.Empty;
                                Lista[34].Valor = string.Empty;
                                Lista[35].Valor = string.Empty;
                                Lista[36].Valor = string.Empty;
                                Lista[37].Valor = string.Empty;

                                //verifica se é cpf ou cnpj
                                if (ObjMConsumer.CNPJ != null && !ObjMConsumer.CNPJ.Equals(string.Empty))
                                {
                                    Lista[38].Valor = "02";
                                }
                                else if (ObjMConsumer.CPF != null)
                                {
                                    Lista[38].Valor = "01";
                                }

                                //verifica se é cpf ou cnpj
                                if (ObjMConsumer.CNPJ != null && !ObjMConsumer.CNPJ.Equals(string.Empty))
                                {
                                    Lista[39].Valor = ObjMConsumer.CNPJ;
                                }
                                else if (ObjMConsumer.CPF != null)
                                {
                                    Lista[39].Valor = ObjMConsumer.CPF;
                                }

                                Lista[40].Valor = ObjMConsumer.Name;
                                Lista[41].Valor = string.Empty;
                                Lista[42].Valor = string.Empty;
                                Lista[43].Valor = string.Empty;
                                Lista[44].Valor = string.Empty;
                                Lista[45].Valor = string.Empty;
                                Lista[46].Valor = "2";

                                ObjStreamWriter.WriteLine(MCnab.GetLine(Lista));
                                #endregion

                                #region parametros Trailler

                                Lista = Cnab.GetCnab(ObjConteCorrente.Layout, ObjConteCorrente.Versao, ObjConteCorrente.Data, 19, 1);

                                Lista[0].Valor = "9";
                                Lista[1].Valor = string.Empty;
                                Lista[2].Valor = "3";

                                ObjStreamWriter.WriteLine(MCnab.GetLine(Lista));

                                #endregion

                                ObjStreamWriter.Close();

                            }
                            break;
                    }
                    #endregion
                    break;
                case BankNumber.Itaú:
                    #region Layout do arquivo
                    switch (string.Format("{0}_{1}_{2}", ObjConteCorrente.Layout, ObjConteCorrente.Versao, ObjConteCorrente.Data.ToString("dd/MM/yyyy")))
                    {
                        case "400_01_08/09/2004"://layout_versao_datadoarquivo
                            //se o arquivo ja existe adiciona uma nova linha
                            if (File.Exists(PathContaCorrente))
                            {
                                int AllLines = GetLength(PathContaCorrente), contador = 0;

                                StringBuilder ObjStringBuilderCont = new StringBuilder();
                                StreamReader ObjStreamReaderCont = new StreamReader(PathContaCorrente);
                                IList<MCnab> Lista;
                                while (ObjStreamReaderCont.Peek() > 0)
                                {
                                    contador++;
                                    string strLine = ObjStreamReaderCont.ReadLine();
                                    if (contador < AllLines)
                                    {
                                        ObjStringBuilderCont.Append(strLine);
                                        ObjStringBuilderCont.AppendLine();
                                    }
                                }
                                ObjStreamReaderCont.Close();

                                File.Delete(PathContaCorrente);
                                File.WriteAllText(PathContaCorrente, ObjStringBuilderCont.ToString());

                                ObjStreamWriter = new StreamWriter(PathContaCorrente, true);

                                #region Registro de transação - tipo 1
                                Lista = Cnab.GetCnab(ObjConteCorrente.Layout, ObjConteCorrente.Versao, ObjConteCorrente.Data, 3, 1);
                                Lista[0].Valor = "1";
                                Lista[1].Valor = paymentInfo.NumAgencia;
                                Lista[2].Valor = paymentInfo.DigVerAg;
                                Lista[3].Valor = string.Empty;//verificar o q é isso
                                Lista[4].Valor = paymentInfo.NumContCorrent;
                                Lista[5].Valor = paymentInfo.DigVerCont;
                                Lista[6].Valor = "0";

                                Lista[7].Valor = ObjConteCorrente.Carteira;
                                Lista[8].Valor = ObjConteCorrente.Agencia;
                                Lista[9].Valor = ObjConteCorrente.ContaCorrente;

                                Lista[10].Valor = "T" + NrDocum.ToString();
                                Lista[11].Valor = Convert.ToInt32(ObjConteCorrente.NumBanco).ToString();
                                Lista[12].Valor = string.Empty;
                                Lista[13].Valor = string.Empty;
                                Lista[14].Valor = string.Empty;
                                Lista[15].Valor = string.Empty;
                                Lista[16].Valor = "1";
                                Lista[17].Valor = "N";
                                Lista[18].Valor = string.Empty;
                                Lista[19].Valor = string.Empty;
                                Lista[20].Valor = "2";
                                Lista[21].Valor = string.Empty;
                                Lista[22].Valor = string.Empty;
                                Lista[23].Valor = "1";
                                Lista[24].Valor = paymentInfo.Vencimento.Date.ToString("ddMMyy");
                                Lista[25].Valor = attempt.price.ToString().Replace(",", string.Empty);
                                Lista[26].Valor = Convert.ToInt32(ObjConteCorrente.NumBanco).ToString();
                                Lista[27].Valor = "0785";
                                Lista[28].Valor = "99";
                                Lista[29].Valor = "N";
                                Lista[30].Valor = DateTime.Now.Date.ToString("ddMMyy"); ;
                                Lista[31].Valor = string.Empty;
                                Lista[32].Valor = string.Empty;
                                Lista[33].Valor = string.Empty;
                                Lista[34].Valor = string.Empty;
                                Lista[35].Valor = string.Empty;
                                Lista[36].Valor = string.Empty;
                                Lista[37].Valor = string.Empty;

                                //verifica se é cpf ou cnpj
                                if (ObjMConsumer.CNPJ != null && !ObjMConsumer.CNPJ.Equals(string.Empty))
                                {
                                    Lista[38].Valor = "02";
                                }
                                else if (ObjMConsumer.CPF != null)
                                {
                                    Lista[38].Valor = "01";
                                }

                                //verifica se é cpf ou cnpj
                                if (ObjMConsumer.CNPJ != null && !ObjMConsumer.CNPJ.Equals(string.Empty))
                                {
                                    Lista[39].Valor = ObjMConsumer.CNPJ;
                                }
                                else if (ObjMConsumer.CPF != null)
                                {
                                    Lista[39].Valor = ObjMConsumer.CPF;
                                }

                                Lista[40].Valor = ObjMConsumer.Name;
                                Lista[41].Valor = string.Empty;
                                Lista[42].Valor = string.Empty;
                                Lista[43].Valor = string.Empty;
                                Lista[44].Valor = string.Empty;
                                Lista[45].Valor = string.Empty;
                                Lista[46].Valor = Convert.ToString(AllLines);

                                ObjStreamWriter.WriteLine(MCnab.GetLine(Lista));


                                #endregion

                                #region parametros Trailler

                                Lista = Cnab.GetCnab(ObjConteCorrente.Layout, ObjConteCorrente.Versao, ObjConteCorrente.Data, 19, 1);

                                Lista[0].Valor = "9";
                                Lista[1].Valor = string.Empty;
                                Lista[2].Valor = Convert.ToString(AllLines + 1);

                                ObjStreamWriter.WriteLine(MCnab.GetLine(Lista));

                                #endregion

                                ObjStreamWriter.Close();
                            }
                            //se o arquivo não existe adicina o Header
                            else
                            {
                                ObjStreamWriter = File.CreateText(PathContaCorrente);

                                IList<MCnab> Lista;
                                Lista = Cnab.GetCnab(ObjConteCorrente.Layout, ObjConteCorrente.Versao, ObjConteCorrente.Data, 1, 1);

                                #region Parametros Header
                                Lista[0].Valor = "0";
                                Lista[1].Valor = "1";
                                Lista[2].Valor = "REMESSA";
                                Lista[3].Valor = "O1";
                                Lista[4].Valor = "COBRANCA";
                                Lista[5].Valor = ObjConteCorrente.CodConvenio;
                                Lista[6].Valor = ObjConteCorrente.NEmpresa;
                                Lista[7].Valor = Convert.ToInt32(ObjConteCorrente.NumBanco).ToString();
                                Lista[8].Valor = ObjConteCorrente.NBanco;
                                Lista[9].Valor = DateTime.Now.Date.ToString("ddMMyy");
                                Lista[10].Valor = string.Empty;
                                Lista[11].Valor = "MX";
                                Lista[12].Valor = ObjConteCorrente.NumSeq.ToString();
                                Lista[13].Valor = string.Empty;
                                Lista[14].Valor = "1";

                                ObjStreamWriter.WriteLine(MCnab.GetLine(Lista));
                                #endregion

                                #region Registro de transação - tipo 1
                                Lista = Cnab.GetCnab(ObjConteCorrente.Layout, ObjConteCorrente.Versao, ObjConteCorrente.Data, 3, 1);
                                Lista[0].Valor = "1";
                                Lista[1].Valor = paymentInfo.NumAgencia;
                                Lista[2].Valor = paymentInfo.DigVerAg;
                                Lista[3].Valor = string.Empty;//verificar o q é isso
                                Lista[4].Valor = paymentInfo.NumContCorrent;
                                Lista[5].Valor = paymentInfo.DigVerCont;
                                Lista[6].Valor = "0";

                                Lista[7].Valor = ObjConteCorrente.Carteira;
                                Lista[8].Valor = ObjConteCorrente.Agencia;
                                Lista[9].Valor = ObjConteCorrente.ContaCorrente;

                                Lista[10].Valor = "T" + NrDocum.ToString();
                                Lista[11].Valor = Convert.ToInt32(ObjConteCorrente.NumBanco).ToString();
                                Lista[12].Valor = string.Empty;
                                Lista[13].Valor = string.Empty;
                                Lista[14].Valor = string.Empty;
                                Lista[15].Valor = string.Empty;
                                Lista[16].Valor = "1";
                                Lista[17].Valor = "N";
                                Lista[18].Valor = string.Empty;
                                Lista[19].Valor = string.Empty;
                                Lista[20].Valor = "2";
                                Lista[21].Valor = string.Empty;
                                Lista[22].Valor = string.Empty;
                                Lista[23].Valor = "1";
                                Lista[24].Valor = paymentInfo.Vencimento.Date.ToString("ddMMyy");
                                Lista[25].Valor = attempt.price.ToString().Replace(",", string.Empty);
                                Lista[26].Valor = Convert.ToInt32(ObjConteCorrente.NumBanco).ToString();
                                Lista[27].Valor = "0785";
                                Lista[28].Valor = "99";
                                Lista[29].Valor = "N";
                                Lista[30].Valor = DateTime.Now.Date.ToString("ddMMyy"); ;
                                Lista[31].Valor = string.Empty;
                                Lista[32].Valor = string.Empty;
                                Lista[33].Valor = string.Empty;
                                Lista[34].Valor = string.Empty;
                                Lista[35].Valor = string.Empty;
                                Lista[36].Valor = string.Empty;
                                Lista[37].Valor = string.Empty;

                                //verifica se é cpf ou cnpj
                                if (ObjMConsumer.CNPJ != null && !ObjMConsumer.CNPJ.Equals(string.Empty))
                                {
                                    Lista[38].Valor = "02";
                                }
                                else if (ObjMConsumer.CPF != null)
                                {
                                    Lista[38].Valor = "01";
                                }

                                //verifica se é cpf ou cnpj
                                if (ObjMConsumer.CNPJ != null && !ObjMConsumer.CNPJ.Equals(string.Empty))
                                {
                                    Lista[39].Valor = ObjMConsumer.CNPJ;
                                }
                                else if (ObjMConsumer.CPF != null)
                                {
                                    Lista[39].Valor = ObjMConsumer.CPF;
                                }

                                Lista[40].Valor = ObjMConsumer.Name;
                                Lista[41].Valor = string.Empty;
                                Lista[42].Valor = string.Empty;
                                Lista[43].Valor = string.Empty;
                                Lista[44].Valor = string.Empty;
                                Lista[45].Valor = string.Empty;
                                Lista[46].Valor = "2";

                                ObjStreamWriter.WriteLine(MCnab.GetLine(Lista));
                                #endregion

                                #region parametros Trailler

                                Lista = Cnab.GetCnab(ObjConteCorrente.Layout, ObjConteCorrente.Versao, ObjConteCorrente.Data, 19, 1);

                                Lista[0].Valor = "9";
                                Lista[1].Valor = string.Empty;
                                Lista[2].Valor = "3";

                                ObjStreamWriter.WriteLine(MCnab.GetLine(Lista));

                                #endregion

                                ObjStreamWriter.Close();

                            }
                            break;
                    }
                    #endregion
                    break;
                case BankNumber.BancoSantander:
                    #region Layout do arquivo
                    switch (string.Format("{0}_{1}_{2}", ObjConteCorrente.Layout, ObjConteCorrente.Versao, ObjConteCorrente.Data.ToString("dd/MM/yyyy")))
                    {
                        case "400_01_08/09/2004"://layout_versao_datadoarquivo
                            //se o arquivo ja existe adiciona uma nova linha
                            if (File.Exists(PathContaCorrente))
                            {
                                int AllLines = GetLength(PathContaCorrente), contador = 0;

                                StringBuilder ObjStringBuilderCont = new StringBuilder();
                                StreamReader ObjStreamReaderCont = new StreamReader(PathContaCorrente);
                                IList<MCnab> Lista;
                                while (ObjStreamReaderCont.Peek() > 0)
                                {
                                    contador++;
                                    string strLine = ObjStreamReaderCont.ReadLine();
                                    if (contador < AllLines)
                                    {
                                        ObjStringBuilderCont.Append(strLine);
                                        ObjStringBuilderCont.AppendLine();
                                    }
                                }
                                ObjStreamReaderCont.Close();

                                File.Delete(PathContaCorrente);
                                File.WriteAllText(PathContaCorrente, ObjStringBuilderCont.ToString());

                                ObjStreamWriter = new StreamWriter(PathContaCorrente, true);

                                #region Registro de transação - tipo 1
                                Lista = Cnab.GetCnab(ObjConteCorrente.Layout, ObjConteCorrente.Versao, ObjConteCorrente.Data, 3, 1);
                                Lista[0].Valor = "1";
                                Lista[1].Valor = paymentInfo.NumAgencia;
                                Lista[2].Valor = paymentInfo.DigVerAg;
                                Lista[3].Valor = string.Empty;//verificar o q é isso
                                Lista[4].Valor = paymentInfo.NumContCorrent;
                                Lista[5].Valor = paymentInfo.DigVerCont;
                                Lista[6].Valor = "0";

                                Lista[7].Valor = ObjConteCorrente.Carteira;
                                Lista[8].Valor = ObjConteCorrente.Agencia;
                                Lista[9].Valor = ObjConteCorrente.ContaCorrente;

                                Lista[10].Valor = paymentInfo.NumContCorrent.Substring((paymentInfo.NumContCorrent.Length - 2)) + "T" + NrDocum.ToString();
                                Lista[11].Valor = Convert.ToInt32(ObjConteCorrente.NumBanco).ToString();
                                Lista[12].Valor = string.Empty;
                                Lista[13].Valor = string.Empty;
                                Lista[14].Valor = string.Empty;
                                Lista[15].Valor = string.Empty;
                                Lista[16].Valor = "1";
                                Lista[17].Valor = "N";
                                Lista[18].Valor = string.Empty;
                                Lista[19].Valor = string.Empty;
                                Lista[20].Valor = "2";
                                Lista[21].Valor = string.Empty;
                                Lista[22].Valor = string.Empty;
                                Lista[23].Valor = "1";
                                Lista[24].Valor = paymentInfo.Vencimento.Date.ToString("ddMMyy");
                                Lista[25].Valor = attempt.price.ToString().Replace(",", string.Empty);
                                Lista[26].Valor = Convert.ToInt32(ObjConteCorrente.NumBanco).ToString();
                                Lista[27].Valor = "0785";
                                Lista[28].Valor = "99";
                                Lista[29].Valor = "N";
                                Lista[30].Valor = DateTime.Now.Date.ToString("ddMMyy"); ;
                                Lista[31].Valor = string.Empty;
                                Lista[32].Valor = string.Empty;
                                Lista[33].Valor = string.Empty;
                                Lista[34].Valor = string.Empty;
                                Lista[35].Valor = string.Empty;
                                Lista[36].Valor = string.Empty;
                                Lista[37].Valor = string.Empty;

                                //verifica se é cpf ou cnpj
                                if (ObjMConsumer.CNPJ != null && !ObjMConsumer.CNPJ.Equals(string.Empty))
                                {
                                    Lista[38].Valor = "02";
                                }
                                else if (ObjMConsumer.CPF != null)
                                {
                                    Lista[38].Valor = "01";
                                }

                                //verifica se é cpf ou cnpj
                                if (ObjMConsumer.CNPJ != null && !ObjMConsumer.CNPJ.Equals(string.Empty))
                                {
                                    Lista[39].Valor = ObjMConsumer.CNPJ;
                                }
                                else if (ObjMConsumer.CPF != null)
                                {
                                    Lista[39].Valor = ObjMConsumer.CPF;
                                }

                                Lista[40].Valor = ObjMConsumer.Name;
                                Lista[41].Valor = string.Empty;
                                Lista[42].Valor = string.Empty;
                                Lista[43].Valor = string.Empty;
                                Lista[44].Valor = string.Empty;
                                Lista[45].Valor = string.Empty;
                                Lista[46].Valor = Convert.ToString(AllLines);

                                ObjStreamWriter.WriteLine(MCnab.GetLine(Lista));


                                #endregion

                                #region parametros Trailler

                                Lista = Cnab.GetCnab(ObjConteCorrente.Layout, ObjConteCorrente.Versao, ObjConteCorrente.Data, 19, 1);

                                Lista[0].Valor = "9";
                                Lista[1].Valor = string.Empty;
                                Lista[2].Valor = Convert.ToString(AllLines + 1);

                                ObjStreamWriter.WriteLine(MCnab.GetLine(Lista));

                                #endregion

                                ObjStreamWriter.Close();
                            }
                            //se o arquivo não existe adicina o Header
                            else
                            {
                                ObjStreamWriter = File.CreateText(PathContaCorrente);

                                IList<MCnab> Lista;
                                Lista = Cnab.GetCnab(ObjConteCorrente.Layout, ObjConteCorrente.Versao, ObjConteCorrente.Data, 1, 1);

                                #region Parametros Header
                                Lista[0].Valor = "0";
                                Lista[1].Valor = "1";
                                Lista[2].Valor = "REMESSA";
                                Lista[3].Valor = "O1";
                                Lista[4].Valor = "COBRANCA";
                                Lista[5].Valor = ObjConteCorrente.CodConvenio;
                                Lista[6].Valor = ObjConteCorrente.NEmpresa;
                                Lista[7].Valor = Convert.ToInt32(ObjConteCorrente.NumBanco).ToString();
                                Lista[8].Valor = ObjConteCorrente.NBanco;
                                Lista[9].Valor = DateTime.Now.Date.ToString("ddMMyy");
                                Lista[10].Valor = string.Empty;
                                Lista[11].Valor = "MX";
                                Lista[12].Valor = ObjConteCorrente.NumSeq.ToString();
                                Lista[13].Valor = string.Empty;
                                Lista[14].Valor = "1";

                                ObjStreamWriter.WriteLine(MCnab.GetLine(Lista));
                                #endregion

                                #region Registro de transação - tipo 1
                                Lista = Cnab.GetCnab(ObjConteCorrente.Layout, ObjConteCorrente.Versao, ObjConteCorrente.Data, 3, 1);
                                Lista[0].Valor = "1";
                                Lista[1].Valor = paymentInfo.NumAgencia;
                                Lista[2].Valor = paymentInfo.DigVerAg;
                                Lista[3].Valor = string.Empty;//verificar o q é isso
                                Lista[4].Valor = paymentInfo.NumContCorrent;
                                Lista[5].Valor = paymentInfo.DigVerCont;
                                Lista[6].Valor = "0";

                                Lista[7].Valor = ObjConteCorrente.Carteira;
                                Lista[8].Valor = ObjConteCorrente.Agencia;
                                Lista[9].Valor = ObjConteCorrente.ContaCorrente;

                                Lista[10].Valor = paymentInfo.NumContCorrent.Substring((paymentInfo.NumContCorrent.Length - 2)) + "T" + NrDocum.ToString();
                                Lista[11].Valor = Convert.ToInt32(ObjConteCorrente.NumBanco).ToString();
                                Lista[12].Valor = string.Empty;
                                Lista[13].Valor = string.Empty;
                                Lista[14].Valor = string.Empty;
                                Lista[15].Valor = string.Empty;
                                Lista[16].Valor = "1";
                                Lista[17].Valor = "N";
                                Lista[18].Valor = string.Empty;
                                Lista[19].Valor = string.Empty;
                                Lista[20].Valor = "2";
                                Lista[21].Valor = string.Empty;
                                Lista[22].Valor = string.Empty;
                                Lista[23].Valor = "1";
                                Lista[24].Valor = paymentInfo.Vencimento.Date.ToString("ddMMyy");
                                Lista[25].Valor = attempt.price.ToString().Replace(",", string.Empty);
                                Lista[26].Valor = Convert.ToInt32(ObjConteCorrente.NumBanco).ToString();
                                Lista[27].Valor = "0785";
                                Lista[28].Valor = "99";
                                Lista[29].Valor = "N";
                                Lista[30].Valor = DateTime.Now.Date.ToString("ddMMyy"); ;
                                Lista[31].Valor = string.Empty;
                                Lista[32].Valor = string.Empty;
                                Lista[33].Valor = string.Empty;
                                Lista[34].Valor = string.Empty;
                                Lista[35].Valor = string.Empty;
                                Lista[36].Valor = string.Empty;
                                Lista[37].Valor = string.Empty;

                                //verifica se é cpf ou cnpj
                                if (ObjMConsumer.CNPJ != null && !ObjMConsumer.CNPJ.Equals(string.Empty))
                                {
                                    Lista[38].Valor = "02";
                                }
                                else if (ObjMConsumer.CPF != null)
                                {
                                    Lista[38].Valor = "01";
                                }

                                //verifica se é cpf ou cnpj
                                if (ObjMConsumer.CNPJ != null && !ObjMConsumer.CNPJ.Equals(string.Empty))
                                {
                                    Lista[39].Valor = ObjMConsumer.CNPJ;
                                }
                                else if (ObjMConsumer.CPF != null)
                                {
                                    Lista[39].Valor = ObjMConsumer.CPF;
                                }

                                Lista[40].Valor = ObjMConsumer.Name;
                                Lista[41].Valor = string.Empty;
                                Lista[42].Valor = string.Empty;
                                Lista[43].Valor = string.Empty;
                                Lista[44].Valor = string.Empty;
                                Lista[45].Valor = string.Empty;
                                Lista[46].Valor = "2";

                                ObjStreamWriter.WriteLine(MCnab.GetLine(Lista));
                                #endregion

                                #region parametros Trailler

                                Lista = Cnab.GetCnab(ObjConteCorrente.Layout, ObjConteCorrente.Versao, ObjConteCorrente.Data, 19, 1);

                                Lista[0].Valor = "9";
                                Lista[1].Valor = string.Empty;
                                Lista[2].Valor = "3";

                                ObjStreamWriter.WriteLine(MCnab.GetLine(Lista));

                                #endregion

                                ObjStreamWriter.Close();

                            }
                            break;
                    }
                    #endregion
                    break;
                case BankNumber.BancoDoBrasil:
                    #region Layout do arquivo
                    switch (string.Format("{0}_{1}_{2}", ObjConteCorrente.Layout, ObjConteCorrente.Versao, ObjConteCorrente.Data.ToString("dd/MM/yyyy")))
                    {
                        case "400_01_08/09/2004"://layout_versao_datadoarquivo
                            //se o arquivo ja existe adiciona uma nova linha
                            if (File.Exists(PathContaCorrente))
                            {
                                int AllLines = GetLength(PathContaCorrente), contador = 0;

                                StringBuilder ObjStringBuilderCont = new StringBuilder();
                                StreamReader ObjStreamReaderCont = new StreamReader(PathContaCorrente);
                                IList<MCnab> Lista;
                                while (ObjStreamReaderCont.Peek() > 0)
                                {
                                    contador++;
                                    string strLine = ObjStreamReaderCont.ReadLine();
                                    if (contador < AllLines)
                                    {
                                        ObjStringBuilderCont.Append(strLine);
                                        ObjStringBuilderCont.AppendLine();
                                    }
                                }
                                ObjStreamReaderCont.Close();

                                File.Delete(PathContaCorrente);
                                File.WriteAllText(PathContaCorrente, ObjStringBuilderCont.ToString());

                                ObjStreamWriter = new StreamWriter(PathContaCorrente, true);

                                #region Registro de transação - tipo 1
                                Lista = Cnab.GetCnab(ObjConteCorrente.Layout, ObjConteCorrente.Versao, ObjConteCorrente.Data, 3, 1);
                                Lista[0].Valor = "1";
                                Lista[1].Valor = paymentInfo.NumAgencia;
                                Lista[2].Valor = paymentInfo.DigVerAg;
                                Lista[3].Valor = string.Empty;//verificar o q é isso
                                Lista[4].Valor = paymentInfo.NumContCorrent;
                                Lista[5].Valor = paymentInfo.DigVerCont;
                                Lista[6].Valor = "0";

                                Lista[7].Valor = ObjConteCorrente.Carteira;
                                Lista[8].Valor = ObjConteCorrente.Agencia;
                                Lista[9].Valor = ObjConteCorrente.ContaCorrente;

                                Lista[10].Valor = paymentInfo.NumContCorrent.Substring((paymentInfo.NumContCorrent.Length - 2)) + "T" + NrDocum.ToString();
                                Lista[11].Valor = Convert.ToInt32(ObjConteCorrente.NumBanco).ToString();
                                Lista[12].Valor = string.Empty;
                                Lista[13].Valor = string.Empty;
                                Lista[14].Valor = string.Empty;
                                Lista[15].Valor = string.Empty;
                                Lista[16].Valor = "1";
                                Lista[17].Valor = "N";
                                Lista[18].Valor = string.Empty;
                                Lista[19].Valor = string.Empty;
                                Lista[20].Valor = "2";
                                Lista[21].Valor = string.Empty;
                                Lista[22].Valor = string.Empty;
                                Lista[23].Valor = "1";
                                Lista[24].Valor = paymentInfo.Vencimento.Date.ToString("ddMMyy");
                                Lista[25].Valor = attempt.price.ToString().Replace(",", string.Empty);
                                Lista[26].Valor = Convert.ToInt32(ObjConteCorrente.NumBanco).ToString();
                                Lista[27].Valor = "0785";
                                Lista[28].Valor = "99";
                                Lista[29].Valor = "N";
                                Lista[30].Valor = DateTime.Now.Date.ToString("ddMMyy"); ;
                                Lista[31].Valor = string.Empty;
                                Lista[32].Valor = string.Empty;
                                Lista[33].Valor = string.Empty;
                                Lista[34].Valor = string.Empty;
                                Lista[35].Valor = string.Empty;
                                Lista[36].Valor = string.Empty;
                                Lista[37].Valor = string.Empty;

                                //verifica se é cpf ou cnpj
                                if (ObjMConsumer.CNPJ != null && !ObjMConsumer.CNPJ.Equals(string.Empty))
                                {
                                    Lista[38].Valor = "02";
                                }
                                else if (ObjMConsumer.CPF != null)
                                {
                                    Lista[38].Valor = "01";
                                }

                                //verifica se é cpf ou cnpj
                                if (ObjMConsumer.CNPJ != null && !ObjMConsumer.CNPJ.Equals(string.Empty))
                                {
                                    Lista[39].Valor = ObjMConsumer.CNPJ;
                                }
                                else if (ObjMConsumer.CPF != null)
                                {
                                    Lista[39].Valor = ObjMConsumer.CPF;
                                }

                                Lista[40].Valor = ObjMConsumer.Name;
                                Lista[41].Valor = string.Empty;
                                Lista[42].Valor = string.Empty;
                                Lista[43].Valor = string.Empty;
                                Lista[44].Valor = string.Empty;
                                Lista[45].Valor = string.Empty;
                                Lista[46].Valor = Convert.ToString(AllLines);

                                ObjStreamWriter.WriteLine(MCnab.GetLine(Lista));


                                #endregion

                                #region parametros Trailler

                                Lista = Cnab.GetCnab(ObjConteCorrente.Layout, ObjConteCorrente.Versao, ObjConteCorrente.Data, 19, 1);

                                Lista[0].Valor = "9";
                                Lista[1].Valor = string.Empty;
                                Lista[2].Valor = Convert.ToString(AllLines + 1);

                                ObjStreamWriter.WriteLine(MCnab.GetLine(Lista));

                                #endregion

                                ObjStreamWriter.Close();
                            }
                            //se o arquivo não existe adicina o Header
                            else
                            {
                                ObjStreamWriter = File.CreateText(PathContaCorrente);

                                IList<MCnab> Lista;
                                Lista = Cnab.GetCnab(ObjConteCorrente.Layout, ObjConteCorrente.Versao, ObjConteCorrente.Data, 1, 1);

                                #region Parametros Header
                                Lista[0].Valor = "0";
                                Lista[1].Valor = "1";
                                Lista[2].Valor = "REMESSA";
                                Lista[3].Valor = "O1";
                                Lista[4].Valor = "COBRANCA";
                                Lista[5].Valor = ObjConteCorrente.CodConvenio;
                                Lista[6].Valor = ObjConteCorrente.NEmpresa;
                                Lista[7].Valor = Convert.ToInt32(ObjConteCorrente.NumBanco).ToString();
                                Lista[8].Valor = ObjConteCorrente.NBanco;
                                Lista[9].Valor = DateTime.Now.Date.ToString("ddMMyy");
                                Lista[10].Valor = string.Empty;
                                Lista[11].Valor = "MX";
                                Lista[12].Valor = ObjConteCorrente.NumSeq.ToString();
                                Lista[13].Valor = string.Empty;
                                Lista[14].Valor = "1";

                                ObjStreamWriter.WriteLine(MCnab.GetLine(Lista));
                                #endregion

                                #region Registro de transação - tipo 1
                                Lista = Cnab.GetCnab(ObjConteCorrente.Layout, ObjConteCorrente.Versao, ObjConteCorrente.Data, 3, 1);
                                Lista[0].Valor = "1";
                                Lista[1].Valor = paymentInfo.NumAgencia;
                                Lista[2].Valor = paymentInfo.DigVerAg;
                                Lista[3].Valor = string.Empty;//verificar o q é isso
                                Lista[4].Valor = paymentInfo.NumContCorrent;
                                Lista[5].Valor = paymentInfo.DigVerCont;
                                Lista[6].Valor = "0";

                                Lista[7].Valor = ObjConteCorrente.Carteira;
                                Lista[8].Valor = ObjConteCorrente.Agencia;
                                Lista[9].Valor = ObjConteCorrente.ContaCorrente;

                                Lista[10].Valor = paymentInfo.NumContCorrent.Substring((paymentInfo.NumContCorrent.Length - 2)) + "T" + NrDocum.ToString();
                                Lista[11].Valor = Convert.ToInt32(ObjConteCorrente.NumBanco).ToString();
                                Lista[12].Valor = string.Empty;
                                Lista[13].Valor = string.Empty;
                                Lista[14].Valor = string.Empty;
                                Lista[15].Valor = string.Empty;
                                Lista[16].Valor = "1";
                                Lista[17].Valor = "N";
                                Lista[18].Valor = string.Empty;
                                Lista[19].Valor = string.Empty;
                                Lista[20].Valor = "2";
                                Lista[21].Valor = string.Empty;
                                Lista[22].Valor = string.Empty;
                                Lista[23].Valor = "1";
                                Lista[24].Valor = paymentInfo.Vencimento.Date.ToString("ddMMyy");
                                Lista[25].Valor = attempt.price.ToString().Replace(",", string.Empty);
                                Lista[26].Valor = Convert.ToInt32(ObjConteCorrente.NumBanco).ToString();
                                Lista[27].Valor = "0785";
                                Lista[28].Valor = "99";
                                Lista[29].Valor = "N";
                                Lista[30].Valor = DateTime.Now.Date.ToString("ddMMyy"); ;
                                Lista[31].Valor = string.Empty;
                                Lista[32].Valor = string.Empty;
                                Lista[33].Valor = string.Empty;
                                Lista[34].Valor = string.Empty;
                                Lista[35].Valor = string.Empty;
                                Lista[36].Valor = string.Empty;
                                Lista[37].Valor = string.Empty;

                                //verifica se é cpf ou cnpj
                                if (ObjMConsumer.CNPJ != null && !ObjMConsumer.CNPJ.Equals(string.Empty))
                                {
                                    Lista[38].Valor = "02";
                                }
                                else if (ObjMConsumer.CPF != null)
                                {
                                    Lista[38].Valor = "01";
                                }

                                //verifica se é cpf ou cnpj
                                if (ObjMConsumer.CNPJ != null && !ObjMConsumer.CNPJ.Equals(string.Empty))
                                {
                                    Lista[39].Valor = ObjMConsumer.CNPJ;
                                }
                                else if (ObjMConsumer.CPF != null)
                                {
                                    Lista[39].Valor = ObjMConsumer.CPF;
                                }

                                Lista[40].Valor = ObjMConsumer.Name;
                                Lista[41].Valor = string.Empty;
                                Lista[42].Valor = string.Empty;
                                Lista[43].Valor = string.Empty;
                                Lista[44].Valor = string.Empty;
                                Lista[45].Valor = string.Empty;
                                Lista[46].Valor = "2";

                                ObjStreamWriter.WriteLine(MCnab.GetLine(Lista));
                                #endregion

                                #region parametros Trailler

                                Lista = Cnab.GetCnab(ObjConteCorrente.Layout, ObjConteCorrente.Versao, ObjConteCorrente.Data, 19, 1);

                                Lista[0].Valor = "9";
                                Lista[1].Valor = string.Empty;
                                Lista[2].Valor = "3";

                                ObjStreamWriter.WriteLine(MCnab.GetLine(Lista));

                                #endregion

                                ObjStreamWriter.Close();

                            }
                            break;
                    }
                    #endregion
                    break;
                case BankNumber.Banrisul:
                    #region Layout do arquivo
                    switch (string.Format("{0}_{1}_{2}", ObjConteCorrente.Layout, ObjConteCorrente.Versao, ObjConteCorrente.Data.ToString("dd/MM/yyyy")))
                    {
                        case "400_01_08/09/2004"://layout_versao_datadoarquivo
                            //se o arquivo ja existe adiciona uma nova linha
                            if (File.Exists(PathContaCorrente))
                            {
                                int AllLines = GetLength(PathContaCorrente), contador = 0;

                                StringBuilder ObjStringBuilderCont = new StringBuilder();
                                StreamReader ObjStreamReaderCont = new StreamReader(PathContaCorrente);
                                IList<MCnab> Lista;
                                while (ObjStreamReaderCont.Peek() > 0)
                                {
                                    contador++;
                                    string strLine = ObjStreamReaderCont.ReadLine();
                                    if (contador < AllLines)
                                    {
                                        ObjStringBuilderCont.Append(strLine);
                                        ObjStringBuilderCont.AppendLine();
                                    }
                                }
                                ObjStreamReaderCont.Close();

                                File.Delete(PathContaCorrente);
                                File.WriteAllText(PathContaCorrente, ObjStringBuilderCont.ToString());

                                ObjStreamWriter = new StreamWriter(PathContaCorrente, true);

                                #region Registro de transação - tipo 1
                                Lista = Cnab.GetCnab(ObjConteCorrente.Layout, ObjConteCorrente.Versao, ObjConteCorrente.Data, 3, 1);
                                Lista[0].Valor = "1";
                                Lista[1].Valor = paymentInfo.NumAgencia;
                                Lista[2].Valor = paymentInfo.DigVerAg;
                                Lista[3].Valor = string.Empty;//verificar o q é isso
                                Lista[4].Valor = paymentInfo.NumContCorrent;
                                Lista[5].Valor = paymentInfo.DigVerCont;
                                Lista[6].Valor = "0";

                                Lista[7].Valor = ObjConteCorrente.Carteira;
                                Lista[8].Valor = ObjConteCorrente.Agencia;
                                Lista[9].Valor = ObjConteCorrente.ContaCorrente;

                                Lista[10].Valor = paymentInfo.NumContCorrent.Substring((paymentInfo.NumContCorrent.Length - 2)) + "T" + NrDocum.ToString();
                                Lista[11].Valor = Convert.ToInt32(ObjConteCorrente.NumBanco).ToString();
                                Lista[12].Valor = string.Empty;
                                Lista[13].Valor = string.Empty;
                                Lista[14].Valor = string.Empty;
                                Lista[15].Valor = string.Empty;
                                Lista[16].Valor = "1";
                                Lista[17].Valor = "N";
                                Lista[18].Valor = string.Empty;
                                Lista[19].Valor = string.Empty;
                                Lista[20].Valor = "2";
                                Lista[21].Valor = string.Empty;
                                Lista[22].Valor = string.Empty;
                                Lista[23].Valor = "1";
                                Lista[24].Valor = paymentInfo.Vencimento.Date.ToString("ddMMyy");
                                Lista[25].Valor = attempt.price.ToString().Replace(",", string.Empty);
                                Lista[26].Valor = Convert.ToInt32(ObjConteCorrente.NumBanco).ToString();
                                Lista[27].Valor = "0785";
                                Lista[28].Valor = "99";
                                Lista[29].Valor = "N";
                                Lista[30].Valor = DateTime.Now.Date.ToString("ddMMyy"); ;
                                Lista[31].Valor = string.Empty;
                                Lista[32].Valor = string.Empty;
                                Lista[33].Valor = string.Empty;
                                Lista[34].Valor = string.Empty;
                                Lista[35].Valor = string.Empty;
                                Lista[36].Valor = string.Empty;
                                Lista[37].Valor = string.Empty;

                                //verifica se é cpf ou cnpj
                                if (ObjMConsumer.CNPJ != null && !ObjMConsumer.CNPJ.Equals(string.Empty))
                                {
                                    Lista[38].Valor = "02";
                                }
                                else if (ObjMConsumer.CPF != null)
                                {
                                    Lista[38].Valor = "01";
                                }

                                //verifica se é cpf ou cnpj
                                if (ObjMConsumer.CNPJ != null && !ObjMConsumer.CNPJ.Equals(string.Empty))
                                {
                                    Lista[39].Valor = ObjMConsumer.CNPJ;
                                }
                                else if (ObjMConsumer.CPF != null)
                                {
                                    Lista[39].Valor = ObjMConsumer.CPF;
                                }

                                Lista[40].Valor = ObjMConsumer.Name;
                                Lista[41].Valor = string.Empty;
                                Lista[42].Valor = string.Empty;
                                Lista[43].Valor = string.Empty;
                                Lista[44].Valor = string.Empty;
                                Lista[45].Valor = string.Empty;
                                Lista[46].Valor = Convert.ToString(AllLines);

                                ObjStreamWriter.WriteLine(MCnab.GetLine(Lista));


                                #endregion

                                #region parametros Trailler

                                Lista = Cnab.GetCnab(ObjConteCorrente.Layout, ObjConteCorrente.Versao, ObjConteCorrente.Data, 19, 1);

                                Lista[0].Valor = "9";
                                Lista[1].Valor = string.Empty;
                                Lista[2].Valor = Convert.ToString(AllLines + 1);

                                ObjStreamWriter.WriteLine(MCnab.GetLine(Lista));

                                #endregion

                                ObjStreamWriter.Close();
                            }
                            //se o arquivo não existe adicina o Header
                            else
                            {
                                ObjStreamWriter = File.CreateText(PathContaCorrente);

                                IList<MCnab> Lista;
                                Lista = Cnab.GetCnab(ObjConteCorrente.Layout, ObjConteCorrente.Versao, ObjConteCorrente.Data, 1, 1);

                                #region Parametros Header
                                Lista[0].Valor = "0";
                                Lista[1].Valor = "1";
                                Lista[2].Valor = "REMESSA";
                                Lista[3].Valor = "O1";
                                Lista[4].Valor = "COBRANCA";
                                Lista[5].Valor = ObjConteCorrente.CodConvenio;
                                Lista[6].Valor = ObjConteCorrente.NEmpresa;
                                Lista[7].Valor = Convert.ToInt32(ObjConteCorrente.NumBanco).ToString();
                                Lista[8].Valor = ObjConteCorrente.NBanco;
                                Lista[9].Valor = DateTime.Now.Date.ToString("ddMMyy");
                                Lista[10].Valor = string.Empty;
                                Lista[11].Valor = "MX";
                                Lista[12].Valor = ObjConteCorrente.NumSeq.ToString();
                                Lista[13].Valor = string.Empty;
                                Lista[14].Valor = "1";

                                ObjStreamWriter.WriteLine(MCnab.GetLine(Lista));
                                #endregion

                                #region Registro de transação - tipo 1
                                Lista = Cnab.GetCnab(ObjConteCorrente.Layout, ObjConteCorrente.Versao, ObjConteCorrente.Data, 3, 1);
                                Lista[0].Valor = "1";
                                Lista[1].Valor = paymentInfo.NumAgencia;
                                Lista[2].Valor = paymentInfo.DigVerAg;
                                Lista[3].Valor = string.Empty;//verificar o q é isso
                                Lista[4].Valor = paymentInfo.NumContCorrent;
                                Lista[5].Valor = paymentInfo.DigVerCont;
                                Lista[6].Valor = "0";

                                Lista[7].Valor = ObjConteCorrente.Carteira;
                                Lista[8].Valor = ObjConteCorrente.Agencia;
                                Lista[9].Valor = ObjConteCorrente.ContaCorrente;

                                Lista[10].Valor = paymentInfo.NumContCorrent.Substring((paymentInfo.NumContCorrent.Length - 2)) + "T" + NrDocum.ToString();
                                Lista[11].Valor = Convert.ToInt32(ObjConteCorrente.NumBanco).ToString();
                                Lista[12].Valor = string.Empty;
                                Lista[13].Valor = string.Empty;
                                Lista[14].Valor = string.Empty;
                                Lista[15].Valor = string.Empty;
                                Lista[16].Valor = "1";
                                Lista[17].Valor = "N";
                                Lista[18].Valor = string.Empty;
                                Lista[19].Valor = string.Empty;
                                Lista[20].Valor = "2";
                                Lista[21].Valor = string.Empty;
                                Lista[22].Valor = string.Empty;
                                Lista[23].Valor = "1";
                                Lista[24].Valor = paymentInfo.Vencimento.Date.ToString("ddMMyy");
                                Lista[25].Valor = attempt.price.ToString().Replace(",", string.Empty);
                                Lista[26].Valor = Convert.ToInt32(ObjConteCorrente.NumBanco).ToString();
                                Lista[27].Valor = "0785";
                                Lista[28].Valor = "99";
                                Lista[29].Valor = "N";
                                Lista[30].Valor = DateTime.Now.Date.ToString("ddMMyy"); ;
                                Lista[31].Valor = string.Empty;
                                Lista[32].Valor = string.Empty;
                                Lista[33].Valor = string.Empty;
                                Lista[34].Valor = string.Empty;
                                Lista[35].Valor = string.Empty;
                                Lista[36].Valor = string.Empty;
                                Lista[37].Valor = string.Empty;

                                //verifica se é cpf ou cnpj
                                if (ObjMConsumer.CNPJ != null && !ObjMConsumer.CNPJ.Equals(string.Empty))
                                {
                                    Lista[38].Valor = "02";
                                }
                                else if (ObjMConsumer.CPF != null)
                                {
                                    Lista[38].Valor = "01";
                                }

                                //verifica se é cpf ou cnpj
                                if (ObjMConsumer.CNPJ != null && !ObjMConsumer.CNPJ.Equals(string.Empty))
                                {
                                    Lista[39].Valor = ObjMConsumer.CNPJ;
                                }
                                else if (ObjMConsumer.CPF != null)
                                {
                                    Lista[39].Valor = ObjMConsumer.CPF;
                                }

                                Lista[40].Valor = ObjMConsumer.Name;
                                Lista[41].Valor = string.Empty;
                                Lista[42].Valor = string.Empty;
                                Lista[43].Valor = string.Empty;
                                Lista[44].Valor = string.Empty;
                                Lista[45].Valor = string.Empty;
                                Lista[46].Valor = "2";

                                ObjStreamWriter.WriteLine(MCnab.GetLine(Lista));
                                #endregion

                                #region parametros Trailler

                                Lista = Cnab.GetCnab(ObjConteCorrente.Layout, ObjConteCorrente.Versao, ObjConteCorrente.Data, 19, 1);

                                Lista[0].Valor = "9";
                                Lista[1].Valor = string.Empty;
                                Lista[2].Valor = "3";

                                ObjStreamWriter.WriteLine(MCnab.GetLine(Lista));

                                #endregion

                                ObjStreamWriter.Close();

                            }
                            break;
                    }
                    #endregion
                    break;


                #region "Tecban"
                case BankNumber.Unibanco:
                    #region Layout do arquivo
                    switch (string.Format("{0}_{1}_{2}", ObjConteCorrente.Layout, ObjConteCorrente.Versao, ObjConteCorrente.Data.ToString("dd/MM/yyyy")))
                    {
                        case "400_01_08/09/2004"://layout_versao_datadoarquivo
                            //se o arquivo ja existe adiciona uma nova linha
                            if (File.Exists(PathContaCorrente))
                            {
                                int AllLines = GetLength(PathContaCorrente), contador = 0;

                                StringBuilder ObjStringBuilderCont = new StringBuilder();
                                StreamReader ObjStreamReaderCont = new StreamReader(PathContaCorrente);
                                IList<MCnab> Lista;
                                while (ObjStreamReaderCont.Peek() > 0)
                                {
                                    contador++;
                                    string strLine = ObjStreamReaderCont.ReadLine();
                                    if (contador < AllLines)
                                    {
                                        ObjStringBuilderCont.Append(strLine);
                                        ObjStringBuilderCont.AppendLine();
                                    }
                                }
                                ObjStreamReaderCont.Close();

                                File.Delete(PathContaCorrente);
                                File.WriteAllText(PathContaCorrente, ObjStringBuilderCont.ToString());

                                ObjStreamWriter = new StreamWriter(PathContaCorrente, true);

                                #region Registro de transação - tipo 1
                                Lista = Cnab.GetCnab(ObjConteCorrente.Layout, ObjConteCorrente.Versao, ObjConteCorrente.Data, 3, 1);
                                Lista[0].Valor = "1";
                                Lista[1].Valor = paymentInfo.NumAgencia;
                                Lista[2].Valor = paymentInfo.DigVerAg;
                                Lista[3].Valor = string.Empty;//verificar o q é isso
                                Lista[4].Valor = paymentInfo.NumContCorrent;
                                Lista[5].Valor = paymentInfo.DigVerCont;
                                Lista[6].Valor = "0";

                                Lista[7].Valor = ObjConteCorrente.Carteira;
                                Lista[8].Valor = ObjConteCorrente.Agencia;
                                Lista[9].Valor = ObjConteCorrente.ContaCorrente;

                                Lista[10].Valor = "T" + NrDocum.ToString();
                                Lista[11].Valor = Convert.ToInt32(ObjConteCorrente.NumBanco).ToString();
                                Lista[12].Valor = string.Empty;
                                Lista[13].Valor = string.Empty;
                                Lista[14].Valor = string.Empty;
                                Lista[15].Valor = string.Empty;
                                Lista[16].Valor = "1";
                                Lista[17].Valor = "N";
                                Lista[18].Valor = string.Empty;
                                Lista[19].Valor = string.Empty;
                                Lista[20].Valor = "2";
                                Lista[21].Valor = string.Empty;
                                Lista[22].Valor = string.Empty;
                                Lista[23].Valor = "1";
                                Lista[24].Valor = paymentInfo.Vencimento.Date.ToString("ddMMyy");
                                Lista[25].Valor = attempt.price.ToString().Replace(",", string.Empty);
                                Lista[26].Valor = Convert.ToInt32(ObjConteCorrente.NumBanco).ToString();
                                Lista[27].Valor = "0785";
                                Lista[28].Valor = "99";
                                Lista[29].Valor = "N";
                                Lista[30].Valor = DateTime.Now.Date.ToString("ddMMyy"); ;
                                Lista[31].Valor = string.Empty;
                                Lista[32].Valor = string.Empty;
                                Lista[33].Valor = string.Empty;
                                Lista[34].Valor = string.Empty;
                                Lista[35].Valor = string.Empty;
                                Lista[36].Valor = string.Empty;
                                Lista[37].Valor = string.Empty;

                                //verifica se é cpf ou cnpj
                                if (ObjMConsumer.CNPJ != null && !ObjMConsumer.CNPJ.Equals(string.Empty))
                                {
                                    Lista[38].Valor = "02";
                                }
                                else if (ObjMConsumer.CPF != null)
                                {
                                    Lista[38].Valor = "01";
                                }

                                //verifica se é cpf ou cnpj
                                if (ObjMConsumer.CNPJ != null && !ObjMConsumer.CNPJ.Equals(string.Empty))
                                {
                                    Lista[39].Valor = ObjMConsumer.CNPJ;
                                }
                                else if (ObjMConsumer.CPF != null)
                                {
                                    Lista[39].Valor = ObjMConsumer.CPF;
                                }

                                Lista[40].Valor = ObjMConsumer.Name;
                                Lista[41].Valor = string.Empty;
                                Lista[42].Valor = string.Empty;
                                Lista[43].Valor = string.Empty;
                                Lista[44].Valor = string.Empty;
                                Lista[45].Valor = string.Empty;
                                Lista[46].Valor = Convert.ToString(AllLines);

                                ObjStreamWriter.WriteLine(MCnab.GetLine(Lista));


                                #endregion

                                #region parametros Trailler

                                Lista = Cnab.GetCnab(ObjConteCorrente.Layout, ObjConteCorrente.Versao, ObjConteCorrente.Data, 19, 1);

                                Lista[0].Valor = "9";
                                Lista[1].Valor = string.Empty;
                                Lista[2].Valor = Convert.ToString(AllLines + 1);

                                ObjStreamWriter.WriteLine(MCnab.GetLine(Lista));

                                #endregion

                                ObjStreamWriter.Close();
                            }
                            //se o arquivo não existe adicina o Header
                            else
                            {
                                ObjStreamWriter = File.CreateText(PathContaCorrente);

                                IList<MCnab> Lista;
                                Lista = Cnab.GetCnab(ObjConteCorrente.Layout, ObjConteCorrente.Versao, ObjConteCorrente.Data, 1, 1);

                                #region Parametros Header
                                Lista[0].Valor = "0";
                                Lista[1].Valor = "1";
                                Lista[2].Valor = "REMESSA";
                                Lista[3].Valor = "O1";
                                Lista[4].Valor = "COBRANCA";
                                Lista[5].Valor = ObjConteCorrente.CodConvenio;
                                Lista[6].Valor = ObjConteCorrente.NEmpresa;
                                Lista[7].Valor = Convert.ToInt32(ObjConteCorrente.NumBanco).ToString();
                                Lista[8].Valor = ObjConteCorrente.NBanco;
                                Lista[9].Valor = DateTime.Now.Date.ToString("ddMMyy");
                                Lista[10].Valor = string.Empty;
                                Lista[11].Valor = "MX";
                                Lista[12].Valor = ObjConteCorrente.NumSeq.ToString();
                                Lista[13].Valor = string.Empty;
                                Lista[14].Valor = "1";

                                ObjStreamWriter.WriteLine(MCnab.GetLine(Lista));
                                #endregion

                                #region Registro de transação - tipo 1
                                Lista = Cnab.GetCnab(ObjConteCorrente.Layout, ObjConteCorrente.Versao, ObjConteCorrente.Data, 3, 1);
                                Lista[0].Valor = "1";
                                Lista[1].Valor = paymentInfo.NumAgencia;
                                Lista[2].Valor = paymentInfo.DigVerAg;
                                Lista[3].Valor = string.Empty;//verificar o q é isso
                                Lista[4].Valor = paymentInfo.NumContCorrent;
                                Lista[5].Valor = paymentInfo.DigVerCont;
                                Lista[6].Valor = "0";

                                Lista[7].Valor = ObjConteCorrente.Carteira;
                                Lista[8].Valor = ObjConteCorrente.Agencia;
                                Lista[9].Valor = ObjConteCorrente.ContaCorrente;

                                Lista[10].Valor = "T" + NrDocum.ToString();
                                Lista[11].Valor = Convert.ToInt32(ObjConteCorrente.NumBanco).ToString();
                                Lista[12].Valor = string.Empty;
                                Lista[13].Valor = string.Empty;
                                Lista[14].Valor = string.Empty;
                                Lista[15].Valor = string.Empty;
                                Lista[16].Valor = "1";
                                Lista[17].Valor = "N";
                                Lista[18].Valor = string.Empty;
                                Lista[19].Valor = string.Empty;
                                Lista[20].Valor = "2";
                                Lista[21].Valor = string.Empty;
                                Lista[22].Valor = string.Empty;
                                Lista[23].Valor = "1";
                                Lista[24].Valor = paymentInfo.Vencimento.Date.ToString("ddMMyy");
                                Lista[25].Valor = attempt.price.ToString().Replace(",", string.Empty);
                                Lista[26].Valor = Convert.ToInt32(ObjConteCorrente.NumBanco).ToString();
                                Lista[27].Valor = "0785";
                                Lista[28].Valor = "99";
                                Lista[29].Valor = "N";
                                Lista[30].Valor = DateTime.Now.Date.ToString("ddMMyy"); ;
                                Lista[31].Valor = string.Empty;
                                Lista[32].Valor = string.Empty;
                                Lista[33].Valor = string.Empty;
                                Lista[34].Valor = string.Empty;
                                Lista[35].Valor = string.Empty;
                                Lista[36].Valor = string.Empty;
                                Lista[37].Valor = string.Empty;

                                //verifica se é cpf ou cnpj
                                if (ObjMConsumer.CNPJ != null && !ObjMConsumer.CNPJ.Equals(string.Empty))
                                {
                                    Lista[38].Valor = "02";
                                }
                                else if (ObjMConsumer.CPF != null)
                                {
                                    Lista[38].Valor = "01";
                                }

                                //verifica se é cpf ou cnpj
                                if (ObjMConsumer.CNPJ != null && !ObjMConsumer.CNPJ.Equals(string.Empty))
                                {
                                    Lista[39].Valor = ObjMConsumer.CNPJ;
                                }
                                else if (ObjMConsumer.CPF != null)
                                {
                                    Lista[39].Valor = ObjMConsumer.CPF;
                                }

                                Lista[40].Valor = ObjMConsumer.Name;
                                Lista[41].Valor = string.Empty;
                                Lista[42].Valor = string.Empty;
                                Lista[43].Valor = string.Empty;
                                Lista[44].Valor = string.Empty;
                                Lista[45].Valor = string.Empty;
                                Lista[46].Valor = "2";

                                ObjStreamWriter.WriteLine(MCnab.GetLine(Lista));
                                #endregion

                                #region parametros Trailler

                                Lista = Cnab.GetCnab(ObjConteCorrente.Layout, ObjConteCorrente.Versao, ObjConteCorrente.Data, 19, 1);

                                Lista[0].Valor = "9";
                                Lista[1].Valor = string.Empty;
                                Lista[2].Valor = "3";

                                ObjStreamWriter.WriteLine(MCnab.GetLine(Lista));

                                #endregion

                                ObjStreamWriter.Close();

                            }
                            break;
                    }
                    #endregion
                    break;
                case BankNumber.Real:
                    #region Layout do arquivo
                    switch (string.Format("{0}_{1}_{2}", ObjConteCorrente.Layout, ObjConteCorrente.Versao, ObjConteCorrente.Data.ToString("dd/MM/yyyy")))
                    {
                        case "400_01_08/09/2004"://layout_versao_datadoarquivo
                            //se o arquivo ja existe adiciona uma nova linha
                            if (File.Exists(PathContaCorrente))
                            {
                                int AllLines = GetLength(PathContaCorrente), contador = 0;

                                StringBuilder ObjStringBuilderCont = new StringBuilder();
                                StreamReader ObjStreamReaderCont = new StreamReader(PathContaCorrente);
                                IList<MCnab> Lista;
                                while (ObjStreamReaderCont.Peek() > 0)
                                {
                                    contador++;
                                    string strLine = ObjStreamReaderCont.ReadLine();
                                    if (contador < AllLines)
                                    {
                                        ObjStringBuilderCont.Append(strLine);
                                        ObjStringBuilderCont.AppendLine();
                                    }
                                }
                                ObjStreamReaderCont.Close();

                                File.Delete(PathContaCorrente);
                                File.WriteAllText(PathContaCorrente, ObjStringBuilderCont.ToString());

                                ObjStreamWriter = new StreamWriter(PathContaCorrente, true);

                                #region Registro de transação - tipo 1
                                Lista = Cnab.GetCnab(ObjConteCorrente.Layout, ObjConteCorrente.Versao, ObjConteCorrente.Data, 3, 1);
                                Lista[0].Valor = "1";
                                Lista[1].Valor = paymentInfo.NumAgencia;
                                Lista[2].Valor = paymentInfo.DigVerAg;
                                Lista[3].Valor = string.Empty;//verificar o q é isso
                                Lista[4].Valor = paymentInfo.NumContCorrent;
                                Lista[5].Valor = paymentInfo.DigVerCont;
                                Lista[6].Valor = "0";

                                Lista[7].Valor = ObjConteCorrente.Carteira;
                                Lista[8].Valor = ObjConteCorrente.Agencia;
                                Lista[9].Valor = ObjConteCorrente.ContaCorrente;

                                Lista[10].Valor = "T" + NrDocum.ToString();
                                Lista[11].Valor = Convert.ToInt32(ObjConteCorrente.NumBanco).ToString();
                                Lista[12].Valor = string.Empty;
                                Lista[13].Valor = string.Empty;
                                Lista[14].Valor = string.Empty;
                                Lista[15].Valor = string.Empty;
                                Lista[16].Valor = "1";
                                Lista[17].Valor = "N";
                                Lista[18].Valor = string.Empty;
                                Lista[19].Valor = string.Empty;
                                Lista[20].Valor = "2";
                                Lista[21].Valor = string.Empty;
                                Lista[22].Valor = string.Empty;
                                Lista[23].Valor = "1";
                                Lista[24].Valor = paymentInfo.Vencimento.Date.ToString("ddMMyy");
                                Lista[25].Valor = attempt.price.ToString().Replace(",", string.Empty);
                                Lista[26].Valor = Convert.ToInt32(ObjConteCorrente.NumBanco).ToString();
                                Lista[27].Valor = "0785";
                                Lista[28].Valor = "99";
                                Lista[29].Valor = "N";
                                Lista[30].Valor = DateTime.Now.Date.ToString("ddMMyy"); ;
                                Lista[31].Valor = string.Empty;
                                Lista[32].Valor = string.Empty;
                                Lista[33].Valor = string.Empty;
                                Lista[34].Valor = string.Empty;
                                Lista[35].Valor = string.Empty;
                                Lista[36].Valor = string.Empty;
                                Lista[37].Valor = string.Empty;

                                //verifica se é cpf ou cnpj
                                if (ObjMConsumer.CNPJ != null && !ObjMConsumer.CNPJ.Equals(string.Empty))
                                {
                                    Lista[38].Valor = "02";
                                }
                                else if (ObjMConsumer.CPF != null)
                                {
                                    Lista[38].Valor = "01";
                                }

                                //verifica se é cpf ou cnpj
                                if (ObjMConsumer.CNPJ != null && !ObjMConsumer.CNPJ.Equals(string.Empty))
                                {
                                    Lista[39].Valor = ObjMConsumer.CNPJ;
                                }
                                else if (ObjMConsumer.CPF != null)
                                {
                                    Lista[39].Valor = ObjMConsumer.CPF;
                                }

                                Lista[40].Valor = ObjMConsumer.Name;
                                Lista[41].Valor = string.Empty;
                                Lista[42].Valor = string.Empty;
                                Lista[43].Valor = string.Empty;
                                Lista[44].Valor = string.Empty;
                                Lista[45].Valor = string.Empty;
                                Lista[46].Valor = Convert.ToString(AllLines);

                                ObjStreamWriter.WriteLine(MCnab.GetLine(Lista));


                                #endregion

                                #region parametros Trailler

                                Lista = Cnab.GetCnab(ObjConteCorrente.Layout, ObjConteCorrente.Versao, ObjConteCorrente.Data, 19, 1);

                                Lista[0].Valor = "9";
                                Lista[1].Valor = string.Empty;
                                Lista[2].Valor = Convert.ToString(AllLines + 1);

                                ObjStreamWriter.WriteLine(MCnab.GetLine(Lista));

                                #endregion

                                ObjStreamWriter.Close();
                            }
                            //se o arquivo não existe adicina o Header
                            else
                            {
                                ObjStreamWriter = File.CreateText(PathContaCorrente);

                                IList<MCnab> Lista;
                                Lista = Cnab.GetCnab(ObjConteCorrente.Layout, ObjConteCorrente.Versao, ObjConteCorrente.Data, 1, 1);

                                #region Parametros Header
                                Lista[0].Valor = "0";
                                Lista[1].Valor = "1";
                                Lista[2].Valor = "REMESSA";
                                Lista[3].Valor = "O1";
                                Lista[4].Valor = "COBRANCA";
                                Lista[5].Valor = ObjConteCorrente.CodConvenio;
                                Lista[6].Valor = ObjConteCorrente.NEmpresa;
                                Lista[7].Valor = Convert.ToInt32(ObjConteCorrente.NumBanco).ToString();
                                Lista[8].Valor = ObjConteCorrente.NBanco;
                                Lista[9].Valor = DateTime.Now.Date.ToString("ddMMyy");
                                Lista[10].Valor = string.Empty;
                                Lista[11].Valor = "MX";
                                Lista[12].Valor = ObjConteCorrente.NumSeq.ToString();
                                Lista[13].Valor = string.Empty;
                                Lista[14].Valor = "1";

                                ObjStreamWriter.WriteLine(MCnab.GetLine(Lista));
                                #endregion

                                #region Registro de transação - tipo 1
                                Lista = Cnab.GetCnab(ObjConteCorrente.Layout, ObjConteCorrente.Versao, ObjConteCorrente.Data, 3, 1);
                                Lista[0].Valor = "1";
                                Lista[1].Valor = paymentInfo.NumAgencia;
                                Lista[2].Valor = paymentInfo.DigVerAg;
                                Lista[3].Valor = string.Empty;//verificar o q é isso
                                Lista[4].Valor = paymentInfo.NumContCorrent;
                                Lista[5].Valor = paymentInfo.DigVerCont;
                                Lista[6].Valor = "0";

                                Lista[7].Valor = ObjConteCorrente.Carteira;
                                Lista[8].Valor = ObjConteCorrente.Agencia;
                                Lista[9].Valor = ObjConteCorrente.ContaCorrente;

                                Lista[10].Valor = "T" + NrDocum.ToString();
                                Lista[11].Valor = Convert.ToInt32(ObjConteCorrente.NumBanco).ToString();
                                Lista[12].Valor = string.Empty;
                                Lista[13].Valor = string.Empty;
                                Lista[14].Valor = string.Empty;
                                Lista[15].Valor = string.Empty;
                                Lista[16].Valor = "1";
                                Lista[17].Valor = "N";
                                Lista[18].Valor = string.Empty;
                                Lista[19].Valor = string.Empty;
                                Lista[20].Valor = "2";
                                Lista[21].Valor = string.Empty;
                                Lista[22].Valor = string.Empty;
                                Lista[23].Valor = "1";
                                Lista[24].Valor = paymentInfo.Vencimento.Date.ToString("ddMMyy");
                                Lista[25].Valor = attempt.price.ToString().Replace(",", string.Empty);
                                Lista[26].Valor = Convert.ToInt32(ObjConteCorrente.NumBanco).ToString();
                                Lista[27].Valor = "0785";
                                Lista[28].Valor = "99";
                                Lista[29].Valor = "N";
                                Lista[30].Valor = DateTime.Now.Date.ToString("ddMMyy"); ;
                                Lista[31].Valor = string.Empty;
                                Lista[32].Valor = string.Empty;
                                Lista[33].Valor = string.Empty;
                                Lista[34].Valor = string.Empty;
                                Lista[35].Valor = string.Empty;
                                Lista[36].Valor = string.Empty;
                                Lista[37].Valor = string.Empty;

                                //verifica se é cpf ou cnpj
                                if (ObjMConsumer.CNPJ != null && !ObjMConsumer.CNPJ.Equals(string.Empty))
                                {
                                    Lista[38].Valor = "02";
                                }
                                else if (ObjMConsumer.CPF != null)
                                {
                                    Lista[38].Valor = "01";
                                }

                                //verifica se é cpf ou cnpj
                                if (ObjMConsumer.CNPJ != null && !ObjMConsumer.CNPJ.Equals(string.Empty))
                                {
                                    Lista[39].Valor = ObjMConsumer.CNPJ;
                                }
                                else if (ObjMConsumer.CPF != null)
                                {
                                    Lista[39].Valor = ObjMConsumer.CPF;
                                }

                                Lista[40].Valor = ObjMConsumer.Name;
                                Lista[41].Valor = string.Empty;
                                Lista[42].Valor = string.Empty;
                                Lista[43].Valor = string.Empty;
                                Lista[44].Valor = string.Empty;
                                Lista[45].Valor = string.Empty;
                                Lista[46].Valor = "2";

                                ObjStreamWriter.WriteLine(MCnab.GetLine(Lista));
                                #endregion

                                #region parametros Trailler

                                Lista = Cnab.GetCnab(ObjConteCorrente.Layout, ObjConteCorrente.Versao, ObjConteCorrente.Data, 19, 1);

                                Lista[0].Valor = "9";
                                Lista[1].Valor = string.Empty;
                                Lista[2].Valor = "3";

                                ObjStreamWriter.WriteLine(MCnab.GetLine(Lista));

                                #endregion

                                ObjStreamWriter.Close();

                            }
                            break;
                    }
                    #endregion
                    break;
                case BankNumber.Banespa:
                    #region Layout do arquivo
                    switch (string.Format("{0}_{1}_{2}", ObjConteCorrente.Layout, ObjConteCorrente.Versao, ObjConteCorrente.Data.ToString("dd/MM/yyyy")))
                    {
                        case "400_01_08/09/2004"://layout_versao_datadoarquivo
                            //se o arquivo ja existe adiciona uma nova linha
                            if (File.Exists(PathContaCorrente))
                            {
                                int AllLines = GetLength(PathContaCorrente), contador = 0;

                                StringBuilder ObjStringBuilderCont = new StringBuilder();
                                StreamReader ObjStreamReaderCont = new StreamReader(PathContaCorrente);
                                IList<MCnab> Lista;
                                while (ObjStreamReaderCont.Peek() > 0)
                                {
                                    contador++;
                                    string strLine = ObjStreamReaderCont.ReadLine();
                                    if (contador < AllLines)
                                    {
                                        ObjStringBuilderCont.Append(strLine);
                                        ObjStringBuilderCont.AppendLine();
                                    }
                                }
                                ObjStreamReaderCont.Close();

                                File.Delete(PathContaCorrente);
                                File.WriteAllText(PathContaCorrente, ObjStringBuilderCont.ToString());

                                ObjStreamWriter = new StreamWriter(PathContaCorrente, true);

                                #region Registro de transação - tipo 1
                                Lista = Cnab.GetCnab(ObjConteCorrente.Layout, ObjConteCorrente.Versao, ObjConteCorrente.Data, 3, 1);
                                Lista[0].Valor = "1";
                                Lista[1].Valor = paymentInfo.NumAgencia;
                                Lista[2].Valor = paymentInfo.DigVerAg;
                                Lista[3].Valor = string.Empty;//verificar o q é isso
                                Lista[4].Valor = paymentInfo.NumContCorrent;
                                Lista[5].Valor = paymentInfo.DigVerCont;
                                Lista[6].Valor = "0";

                                Lista[7].Valor = ObjConteCorrente.Carteira;
                                Lista[8].Valor = ObjConteCorrente.Agencia;
                                Lista[9].Valor = ObjConteCorrente.ContaCorrente;

                                Lista[10].Valor = "T" + NrDocum.ToString();
                                Lista[11].Valor = Convert.ToInt32(ObjConteCorrente.NumBanco).ToString();
                                Lista[12].Valor = string.Empty;
                                Lista[13].Valor = string.Empty;
                                Lista[14].Valor = string.Empty;
                                Lista[15].Valor = string.Empty;
                                Lista[16].Valor = "1";
                                Lista[17].Valor = "N";
                                Lista[18].Valor = string.Empty;
                                Lista[19].Valor = string.Empty;
                                Lista[20].Valor = "2";
                                Lista[21].Valor = string.Empty;
                                Lista[22].Valor = string.Empty;
                                Lista[23].Valor = "1";
                                Lista[24].Valor = paymentInfo.Vencimento.Date.ToString("ddMMyy");
                                Lista[25].Valor = attempt.price.ToString().Replace(",", string.Empty);
                                Lista[26].Valor = Convert.ToInt32(ObjConteCorrente.NumBanco).ToString();
                                Lista[27].Valor = "0785";
                                Lista[28].Valor = "99";
                                Lista[29].Valor = "N";
                                Lista[30].Valor = DateTime.Now.Date.ToString("ddMMyy"); ;
                                Lista[31].Valor = string.Empty;
                                Lista[32].Valor = string.Empty;
                                Lista[33].Valor = string.Empty;
                                Lista[34].Valor = string.Empty;
                                Lista[35].Valor = string.Empty;
                                Lista[36].Valor = string.Empty;
                                Lista[37].Valor = string.Empty;

                                //verifica se é cpf ou cnpj
                                if (ObjMConsumer.CNPJ != null && !ObjMConsumer.CNPJ.Equals(string.Empty))
                                {
                                    Lista[38].Valor = "02";
                                }
                                else if (ObjMConsumer.CPF != null)
                                {
                                    Lista[38].Valor = "01";
                                }

                                //verifica se é cpf ou cnpj
                                if (ObjMConsumer.CNPJ != null && !ObjMConsumer.CNPJ.Equals(string.Empty))
                                {
                                    Lista[39].Valor = ObjMConsumer.CNPJ;
                                }
                                else if (ObjMConsumer.CPF != null)
                                {
                                    Lista[39].Valor = ObjMConsumer.CPF;
                                }

                                Lista[40].Valor = ObjMConsumer.Name;
                                Lista[41].Valor = string.Empty;
                                Lista[42].Valor = string.Empty;
                                Lista[43].Valor = string.Empty;
                                Lista[44].Valor = string.Empty;
                                Lista[45].Valor = string.Empty;
                                Lista[46].Valor = Convert.ToString(AllLines);

                                ObjStreamWriter.WriteLine(MCnab.GetLine(Lista));


                                #endregion

                                #region parametros Trailler

                                Lista = Cnab.GetCnab(ObjConteCorrente.Layout, ObjConteCorrente.Versao, ObjConteCorrente.Data, 19, 1);

                                Lista[0].Valor = "9";
                                Lista[1].Valor = string.Empty;
                                Lista[2].Valor = Convert.ToString(AllLines + 1);

                                ObjStreamWriter.WriteLine(MCnab.GetLine(Lista));

                                #endregion

                                ObjStreamWriter.Close();
                            }
                            //se o arquivo não existe adicina o Header
                            else
                            {
                                ObjStreamWriter = File.CreateText(PathContaCorrente);

                                IList<MCnab> Lista;
                                Lista = Cnab.GetCnab(ObjConteCorrente.Layout, ObjConteCorrente.Versao, ObjConteCorrente.Data, 1, 1);

                                #region Parametros Header
                                Lista[0].Valor = "0";
                                Lista[1].Valor = "1";
                                Lista[2].Valor = "REMESSA";
                                Lista[3].Valor = "O1";
                                Lista[4].Valor = "COBRANCA";
                                Lista[5].Valor = ObjConteCorrente.CodConvenio;
                                Lista[6].Valor = ObjConteCorrente.NEmpresa;
                                Lista[7].Valor = Convert.ToInt32(ObjConteCorrente.NumBanco).ToString();
                                Lista[8].Valor = ObjConteCorrente.NBanco;
                                Lista[9].Valor = DateTime.Now.Date.ToString("ddMMyy");
                                Lista[10].Valor = string.Empty;
                                Lista[11].Valor = "MX";
                                Lista[12].Valor = ObjConteCorrente.NumSeq.ToString();
                                Lista[13].Valor = string.Empty;
                                Lista[14].Valor = "1";

                                ObjStreamWriter.WriteLine(MCnab.GetLine(Lista));
                                #endregion

                                #region Registro de transação - tipo 1
                                Lista = Cnab.GetCnab(ObjConteCorrente.Layout, ObjConteCorrente.Versao, ObjConteCorrente.Data, 3, 1);
                                Lista[0].Valor = "1";
                                Lista[1].Valor = paymentInfo.NumAgencia;
                                Lista[2].Valor = paymentInfo.DigVerAg;
                                Lista[3].Valor = string.Empty;//verificar o q é isso
                                Lista[4].Valor = paymentInfo.NumContCorrent;
                                Lista[5].Valor = paymentInfo.DigVerCont;
                                Lista[6].Valor = "0";

                                Lista[7].Valor = ObjConteCorrente.Carteira;
                                Lista[8].Valor = ObjConteCorrente.Agencia;
                                Lista[9].Valor = ObjConteCorrente.ContaCorrente;

                                Lista[10].Valor = "T" + NrDocum.ToString();
                                Lista[11].Valor = Convert.ToInt32(ObjConteCorrente.NumBanco).ToString();
                                Lista[12].Valor = string.Empty;
                                Lista[13].Valor = string.Empty;
                                Lista[14].Valor = string.Empty;
                                Lista[15].Valor = string.Empty;
                                Lista[16].Valor = "1";
                                Lista[17].Valor = "N";
                                Lista[18].Valor = string.Empty;
                                Lista[19].Valor = string.Empty;
                                Lista[20].Valor = "2";
                                Lista[21].Valor = string.Empty;
                                Lista[22].Valor = string.Empty;
                                Lista[23].Valor = "1";
                                Lista[24].Valor = paymentInfo.Vencimento.Date.ToString("ddMMyy");
                                Lista[25].Valor = attempt.price.ToString().Replace(",", string.Empty);
                                Lista[26].Valor = Convert.ToInt32(ObjConteCorrente.NumBanco).ToString();
                                Lista[27].Valor = "0785";
                                Lista[28].Valor = "99";
                                Lista[29].Valor = "N";
                                Lista[30].Valor = DateTime.Now.Date.ToString("ddMMyy"); ;
                                Lista[31].Valor = string.Empty;
                                Lista[32].Valor = string.Empty;
                                Lista[33].Valor = string.Empty;
                                Lista[34].Valor = string.Empty;
                                Lista[35].Valor = string.Empty;
                                Lista[36].Valor = string.Empty;
                                Lista[37].Valor = string.Empty;

                                //verifica se é cpf ou cnpj
                                if (ObjMConsumer.CNPJ != null && !ObjMConsumer.CNPJ.Equals(string.Empty))
                                {
                                    Lista[38].Valor = "02";
                                }
                                else if (ObjMConsumer.CPF != null)
                                {
                                    Lista[38].Valor = "01";
                                }

                                //verifica se é cpf ou cnpj
                                if (ObjMConsumer.CNPJ != null && !ObjMConsumer.CNPJ.Equals(string.Empty))
                                {
                                    Lista[39].Valor = ObjMConsumer.CNPJ;
                                }
                                else if (ObjMConsumer.CPF != null)
                                {
                                    Lista[39].Valor = ObjMConsumer.CPF;
                                }

                                Lista[40].Valor = ObjMConsumer.Name;
                                Lista[41].Valor = string.Empty;
                                Lista[42].Valor = string.Empty;
                                Lista[43].Valor = string.Empty;
                                Lista[44].Valor = string.Empty;
                                Lista[45].Valor = string.Empty;
                                Lista[46].Valor = "2";

                                ObjStreamWriter.WriteLine(MCnab.GetLine(Lista));
                                #endregion

                                #region parametros Trailler

                                Lista = Cnab.GetCnab(ObjConteCorrente.Layout, ObjConteCorrente.Versao, ObjConteCorrente.Data, 19, 1);

                                Lista[0].Valor = "9";
                                Lista[1].Valor = string.Empty;
                                Lista[2].Valor = "3";

                                ObjStreamWriter.WriteLine(MCnab.GetLine(Lista));

                                #endregion

                                ObjStreamWriter.Close();

                            }
                            break;
                    }
                    #endregion
                    break;
                case BankNumber.NossaCaixa:
                    #region Layout do arquivo
                    switch (string.Format("{0}_{1}_{2}", ObjConteCorrente.Layout, ObjConteCorrente.Versao, ObjConteCorrente.Data.ToString("dd/MM/yyyy")))
                    {
                        case "400_01_08/09/2004"://layout_versao_datadoarquivo
                            //se o arquivo ja existe adiciona uma nova linha
                            if (File.Exists(PathContaCorrente))
                            {
                                int AllLines = GetLength(PathContaCorrente), contador = 0;

                                StringBuilder ObjStringBuilderCont = new StringBuilder();
                                StreamReader ObjStreamReaderCont = new StreamReader(PathContaCorrente);
                                IList<MCnab> Lista;
                                while (ObjStreamReaderCont.Peek() > 0)
                                {
                                    contador++;
                                    string strLine = ObjStreamReaderCont.ReadLine();
                                    if (contador < AllLines)
                                    {
                                        ObjStringBuilderCont.Append(strLine);
                                        ObjStringBuilderCont.AppendLine();
                                    }
                                }
                                ObjStreamReaderCont.Close();

                                File.Delete(PathContaCorrente);
                                File.WriteAllText(PathContaCorrente, ObjStringBuilderCont.ToString());

                                ObjStreamWriter = new StreamWriter(PathContaCorrente, true);

                                #region Registro de transação - tipo 1
                                Lista = Cnab.GetCnab(ObjConteCorrente.Layout, ObjConteCorrente.Versao, ObjConteCorrente.Data, 3, 1);
                                Lista[0].Valor = "1";
                                Lista[1].Valor = paymentInfo.NumAgencia;
                                Lista[2].Valor = paymentInfo.DigVerAg;
                                Lista[3].Valor = string.Empty;//verificar o q é isso
                                Lista[4].Valor = paymentInfo.NumContCorrent;
                                Lista[5].Valor = paymentInfo.DigVerCont;
                                Lista[6].Valor = "0";

                                Lista[7].Valor = ObjConteCorrente.Carteira;
                                Lista[8].Valor = ObjConteCorrente.Agencia;
                                Lista[9].Valor = ObjConteCorrente.ContaCorrente;

                                Lista[10].Valor = "T" + NrDocum.ToString();
                                Lista[11].Valor = Convert.ToInt32(ObjConteCorrente.NumBanco).ToString();
                                Lista[12].Valor = string.Empty;
                                Lista[13].Valor = string.Empty;
                                Lista[14].Valor = string.Empty;
                                Lista[15].Valor = string.Empty;
                                Lista[16].Valor = "1";
                                Lista[17].Valor = "N";
                                Lista[18].Valor = string.Empty;
                                Lista[19].Valor = string.Empty;
                                Lista[20].Valor = "2";
                                Lista[21].Valor = string.Empty;
                                Lista[22].Valor = string.Empty;
                                Lista[23].Valor = "1";
                                Lista[24].Valor = paymentInfo.Vencimento.Date.ToString("ddMMyy");
                                Lista[25].Valor = attempt.price.ToString().Replace(",", string.Empty);
                                Lista[26].Valor = Convert.ToInt32(ObjConteCorrente.NumBanco).ToString();
                                Lista[27].Valor = "0785";
                                Lista[28].Valor = "99";
                                Lista[29].Valor = "N";
                                Lista[30].Valor = DateTime.Now.Date.ToString("ddMMyy"); ;
                                Lista[31].Valor = string.Empty;
                                Lista[32].Valor = string.Empty;
                                Lista[33].Valor = string.Empty;
                                Lista[34].Valor = string.Empty;
                                Lista[35].Valor = string.Empty;
                                Lista[36].Valor = string.Empty;
                                Lista[37].Valor = string.Empty;

                                //verifica se é cpf ou cnpj
                                if (ObjMConsumer.CNPJ != null && !ObjMConsumer.CNPJ.Equals(string.Empty))
                                {
                                    Lista[38].Valor = "02";
                                }
                                else if (ObjMConsumer.CPF != null)
                                {
                                    Lista[38].Valor = "01";
                                }

                                //verifica se é cpf ou cnpj
                                if (ObjMConsumer.CNPJ != null && !ObjMConsumer.CNPJ.Equals(string.Empty))
                                {
                                    Lista[39].Valor = ObjMConsumer.CNPJ;
                                }
                                else if (ObjMConsumer.CPF != null)
                                {
                                    Lista[39].Valor = ObjMConsumer.CPF;
                                }

                                Lista[40].Valor = ObjMConsumer.Name;
                                Lista[41].Valor = string.Empty;
                                Lista[42].Valor = string.Empty;
                                Lista[43].Valor = string.Empty;
                                Lista[44].Valor = string.Empty;
                                Lista[45].Valor = string.Empty;
                                Lista[46].Valor = Convert.ToString(AllLines);

                                ObjStreamWriter.WriteLine(MCnab.GetLine(Lista));


                                #endregion

                                #region parametros Trailler

                                Lista = Cnab.GetCnab(ObjConteCorrente.Layout, ObjConteCorrente.Versao, ObjConteCorrente.Data, 19, 1);

                                Lista[0].Valor = "9";
                                Lista[1].Valor = string.Empty;
                                Lista[2].Valor = Convert.ToString(AllLines + 1);

                                ObjStreamWriter.WriteLine(MCnab.GetLine(Lista));

                                #endregion

                                ObjStreamWriter.Close();
                            }
                            //se o arquivo não existe adicina o Header
                            else
                            {
                                ObjStreamWriter = File.CreateText(PathContaCorrente);

                                IList<MCnab> Lista;
                                Lista = Cnab.GetCnab(ObjConteCorrente.Layout, ObjConteCorrente.Versao, ObjConteCorrente.Data, 1, 1);

                                #region Parametros Header
                                Lista[0].Valor = "0";
                                Lista[1].Valor = "1";
                                Lista[2].Valor = "REMESSA";
                                Lista[3].Valor = "O1";
                                Lista[4].Valor = "COBRANCA";
                                Lista[5].Valor = ObjConteCorrente.CodConvenio;
                                Lista[6].Valor = ObjConteCorrente.NEmpresa;
                                Lista[7].Valor = Convert.ToInt32(ObjConteCorrente.NumBanco).ToString();
                                Lista[8].Valor = ObjConteCorrente.NBanco;
                                Lista[9].Valor = DateTime.Now.Date.ToString("ddMMyy");
                                Lista[10].Valor = string.Empty;
                                Lista[11].Valor = "MX";
                                Lista[12].Valor = ObjConteCorrente.NumSeq.ToString();
                                Lista[13].Valor = string.Empty;
                                Lista[14].Valor = "1";

                                ObjStreamWriter.WriteLine(MCnab.GetLine(Lista));
                                #endregion

                                #region Registro de transação - tipo 1
                                Lista = Cnab.GetCnab(ObjConteCorrente.Layout, ObjConteCorrente.Versao, ObjConteCorrente.Data, 3, 1);
                                Lista[0].Valor = "1";
                                Lista[1].Valor = paymentInfo.NumAgencia;
                                Lista[2].Valor = paymentInfo.DigVerAg;
                                Lista[3].Valor = string.Empty;//verificar o q é isso
                                Lista[4].Valor = paymentInfo.NumContCorrent;
                                Lista[5].Valor = paymentInfo.DigVerCont;
                                Lista[6].Valor = "0";

                                Lista[7].Valor = ObjConteCorrente.Carteira;
                                Lista[8].Valor = ObjConteCorrente.Agencia;
                                Lista[9].Valor = ObjConteCorrente.ContaCorrente;

                                Lista[10].Valor = "T" + NrDocum.ToString();
                                Lista[11].Valor = Convert.ToInt32(ObjConteCorrente.NumBanco).ToString();
                                Lista[12].Valor = string.Empty;
                                Lista[13].Valor = string.Empty;
                                Lista[14].Valor = string.Empty;
                                Lista[15].Valor = string.Empty;
                                Lista[16].Valor = "1";
                                Lista[17].Valor = "N";
                                Lista[18].Valor = string.Empty;
                                Lista[19].Valor = string.Empty;
                                Lista[20].Valor = "2";
                                Lista[21].Valor = string.Empty;
                                Lista[22].Valor = string.Empty;
                                Lista[23].Valor = "1";
                                Lista[24].Valor = paymentInfo.Vencimento.Date.ToString("ddMMyy");
                                Lista[25].Valor = attempt.price.ToString().Replace(",", string.Empty);
                                Lista[26].Valor = Convert.ToInt32(ObjConteCorrente.NumBanco).ToString();
                                Lista[27].Valor = "0785";
                                Lista[28].Valor = "99";
                                Lista[29].Valor = "N";
                                Lista[30].Valor = DateTime.Now.Date.ToString("ddMMyy"); ;
                                Lista[31].Valor = string.Empty;
                                Lista[32].Valor = string.Empty;
                                Lista[33].Valor = string.Empty;
                                Lista[34].Valor = string.Empty;
                                Lista[35].Valor = string.Empty;
                                Lista[36].Valor = string.Empty;
                                Lista[37].Valor = string.Empty;

                                //verifica se é cpf ou cnpj
                                if (ObjMConsumer.CNPJ != null && !ObjMConsumer.CNPJ.Equals(string.Empty))
                                {
                                    Lista[38].Valor = "02";
                                }
                                else if (ObjMConsumer.CPF != null)
                                {
                                    Lista[38].Valor = "01";
                                }

                                //verifica se é cpf ou cnpj
                                if (ObjMConsumer.CNPJ != null && !ObjMConsumer.CNPJ.Equals(string.Empty))
                                {
                                    Lista[39].Valor = ObjMConsumer.CNPJ;
                                }
                                else if (ObjMConsumer.CPF != null)
                                {
                                    Lista[39].Valor = ObjMConsumer.CPF;
                                }

                                Lista[40].Valor = ObjMConsumer.Name;
                                Lista[41].Valor = string.Empty;
                                Lista[42].Valor = string.Empty;
                                Lista[43].Valor = string.Empty;
                                Lista[44].Valor = string.Empty;
                                Lista[45].Valor = string.Empty;
                                Lista[46].Valor = "2";

                                ObjStreamWriter.WriteLine(MCnab.GetLine(Lista));
                                #endregion

                                #region parametros Trailler

                                Lista = Cnab.GetCnab(ObjConteCorrente.Layout, ObjConteCorrente.Versao, ObjConteCorrente.Data, 19, 1);

                                Lista[0].Valor = "9";
                                Lista[1].Valor = string.Empty;
                                Lista[2].Valor = "3";

                                ObjStreamWriter.WriteLine(MCnab.GetLine(Lista));

                                #endregion

                                ObjStreamWriter.Close();

                            }
                            break;
                    }
                    #endregion
                    break;
                case BankNumber.HSBC:
                    #region Layout do arquivo
                    switch (string.Format("{0}_{1}_{2}", ObjConteCorrente.Layout, ObjConteCorrente.Versao, ObjConteCorrente.Data.ToString("dd/MM/yyyy")))
                    {
                        case "400_01_08/09/2004"://layout_versao_datadoarquivo
                            //se o arquivo ja existe adiciona uma nova linha
                            if (File.Exists(PathContaCorrente))
                            {
                                int AllLines = GetLength(PathContaCorrente), contador = 0;

                                StringBuilder ObjStringBuilderCont = new StringBuilder();
                                StreamReader ObjStreamReaderCont = new StreamReader(PathContaCorrente);
                                IList<MCnab> Lista;
                                while (ObjStreamReaderCont.Peek() > 0)
                                {
                                    contador++;
                                    string strLine = ObjStreamReaderCont.ReadLine();
                                    if (contador < AllLines)
                                    {
                                        ObjStringBuilderCont.Append(strLine);
                                        ObjStringBuilderCont.AppendLine();
                                    }
                                }
                                ObjStreamReaderCont.Close();

                                File.Delete(PathContaCorrente);
                                File.WriteAllText(PathContaCorrente, ObjStringBuilderCont.ToString());

                                ObjStreamWriter = new StreamWriter(PathContaCorrente, true);

                                #region Registro de transação - tipo 1
                                Lista = Cnab.GetCnab(ObjConteCorrente.Layout, ObjConteCorrente.Versao, ObjConteCorrente.Data, 3, 1);
                                Lista[0].Valor = "1";
                                Lista[1].Valor = paymentInfo.NumAgencia;
                                Lista[2].Valor = paymentInfo.DigVerAg;
                                Lista[3].Valor = string.Empty;//verificar o q é isso
                                Lista[4].Valor = paymentInfo.NumContCorrent;
                                Lista[5].Valor = paymentInfo.DigVerCont;
                                Lista[6].Valor = "0";

                                Lista[7].Valor = ObjConteCorrente.Carteira;
                                Lista[8].Valor = ObjConteCorrente.Agencia;
                                Lista[9].Valor = ObjConteCorrente.ContaCorrente;

                                Lista[10].Valor = "T" + NrDocum.ToString();
                                Lista[11].Valor = Convert.ToInt32(ObjConteCorrente.NumBanco).ToString();
                                Lista[12].Valor = string.Empty;
                                Lista[13].Valor = string.Empty;
                                Lista[14].Valor = string.Empty;
                                Lista[15].Valor = string.Empty;
                                Lista[16].Valor = "1";
                                Lista[17].Valor = "N";
                                Lista[18].Valor = string.Empty;
                                Lista[19].Valor = string.Empty;
                                Lista[20].Valor = "2";
                                Lista[21].Valor = string.Empty;
                                Lista[22].Valor = string.Empty;
                                Lista[23].Valor = "1";
                                Lista[24].Valor = paymentInfo.Vencimento.Date.ToString("ddMMyy");
                                Lista[25].Valor = attempt.price.ToString().Replace(",", string.Empty);
                                Lista[26].Valor = Convert.ToInt32(ObjConteCorrente.NumBanco).ToString();
                                Lista[27].Valor = "0785";
                                Lista[28].Valor = "99";
                                Lista[29].Valor = "N";
                                Lista[30].Valor = DateTime.Now.Date.ToString("ddMMyy"); ;
                                Lista[31].Valor = string.Empty;
                                Lista[32].Valor = string.Empty;
                                Lista[33].Valor = string.Empty;
                                Lista[34].Valor = string.Empty;
                                Lista[35].Valor = string.Empty;
                                Lista[36].Valor = string.Empty;
                                Lista[37].Valor = string.Empty;

                                //verifica se é cpf ou cnpj
                                if (ObjMConsumer.CNPJ != null && !ObjMConsumer.CNPJ.Equals(string.Empty))
                                {
                                    Lista[38].Valor = "02";
                                }
                                else if (ObjMConsumer.CPF != null)
                                {
                                    Lista[38].Valor = "01";
                                }

                                //verifica se é cpf ou cnpj
                                if (ObjMConsumer.CNPJ != null && !ObjMConsumer.CNPJ.Equals(string.Empty))
                                {
                                    Lista[39].Valor = ObjMConsumer.CNPJ;
                                }
                                else if (ObjMConsumer.CPF != null)
                                {
                                    Lista[39].Valor = ObjMConsumer.CPF;
                                }

                                Lista[40].Valor = ObjMConsumer.Name;
                                Lista[41].Valor = string.Empty;
                                Lista[42].Valor = string.Empty;
                                Lista[43].Valor = string.Empty;
                                Lista[44].Valor = string.Empty;
                                Lista[45].Valor = string.Empty;
                                Lista[46].Valor = Convert.ToString(AllLines);

                                ObjStreamWriter.WriteLine(MCnab.GetLine(Lista));


                                #endregion

                                #region parametros Trailler

                                Lista = Cnab.GetCnab(ObjConteCorrente.Layout, ObjConteCorrente.Versao, ObjConteCorrente.Data, 19, 1);

                                Lista[0].Valor = "9";
                                Lista[1].Valor = string.Empty;
                                Lista[2].Valor = Convert.ToString(AllLines + 1);

                                ObjStreamWriter.WriteLine(MCnab.GetLine(Lista));

                                #endregion

                                ObjStreamWriter.Close();
                            }
                            //se o arquivo não existe adicina o Header
                            else
                            {
                                ObjStreamWriter = File.CreateText(PathContaCorrente);

                                IList<MCnab> Lista;
                                Lista = Cnab.GetCnab(ObjConteCorrente.Layout, ObjConteCorrente.Versao, ObjConteCorrente.Data, 1, 1);

                                #region Parametros Header
                                Lista[0].Valor = "0";
                                Lista[1].Valor = "1";
                                Lista[2].Valor = "REMESSA";
                                Lista[3].Valor = "O1";
                                Lista[4].Valor = "COBRANCA";
                                Lista[5].Valor = ObjConteCorrente.CodConvenio;
                                Lista[6].Valor = ObjConteCorrente.NEmpresa;
                                Lista[7].Valor = Convert.ToInt32(ObjConteCorrente.NumBanco).ToString();
                                Lista[8].Valor = ObjConteCorrente.NBanco;
                                Lista[9].Valor = DateTime.Now.Date.ToString("ddMMyy");
                                Lista[10].Valor = string.Empty;
                                Lista[11].Valor = "MX";
                                Lista[12].Valor = ObjConteCorrente.NumSeq.ToString();
                                Lista[13].Valor = string.Empty;
                                Lista[14].Valor = "1";

                                ObjStreamWriter.WriteLine(MCnab.GetLine(Lista));
                                #endregion

                                #region Registro de transação - tipo 1
                                Lista = Cnab.GetCnab(ObjConteCorrente.Layout, ObjConteCorrente.Versao, ObjConteCorrente.Data, 3, 1);
                                Lista[0].Valor = "1";
                                Lista[1].Valor = paymentInfo.NumAgencia;
                                Lista[2].Valor = paymentInfo.DigVerAg;
                                Lista[3].Valor = string.Empty;//verificar o q é isso
                                Lista[4].Valor = paymentInfo.NumContCorrent;
                                Lista[5].Valor = paymentInfo.DigVerCont;
                                Lista[6].Valor = "0";

                                Lista[7].Valor = ObjConteCorrente.Carteira;
                                Lista[8].Valor = ObjConteCorrente.Agencia;
                                Lista[9].Valor = ObjConteCorrente.ContaCorrente;

                                Lista[10].Valor = "T" + NrDocum.ToString();
                                Lista[11].Valor = Convert.ToInt32(ObjConteCorrente.NumBanco).ToString();
                                Lista[12].Valor = string.Empty;
                                Lista[13].Valor = string.Empty;
                                Lista[14].Valor = string.Empty;
                                Lista[15].Valor = string.Empty;
                                Lista[16].Valor = "1";
                                Lista[17].Valor = "N";
                                Lista[18].Valor = string.Empty;
                                Lista[19].Valor = string.Empty;
                                Lista[20].Valor = "2";
                                Lista[21].Valor = string.Empty;
                                Lista[22].Valor = string.Empty;
                                Lista[23].Valor = "1";
                                Lista[24].Valor = paymentInfo.Vencimento.Date.ToString("ddMMyy");
                                Lista[25].Valor = attempt.price.ToString().Replace(",", string.Empty);
                                Lista[26].Valor = Convert.ToInt32(ObjConteCorrente.NumBanco).ToString();
                                Lista[27].Valor = "0785";
                                Lista[28].Valor = "99";
                                Lista[29].Valor = "N";
                                Lista[30].Valor = DateTime.Now.Date.ToString("ddMMyy"); ;
                                Lista[31].Valor = string.Empty;
                                Lista[32].Valor = string.Empty;
                                Lista[33].Valor = string.Empty;
                                Lista[34].Valor = string.Empty;
                                Lista[35].Valor = string.Empty;
                                Lista[36].Valor = string.Empty;
                                Lista[37].Valor = string.Empty;

                                //verifica se é cpf ou cnpj
                                if (ObjMConsumer.CNPJ != null && !ObjMConsumer.CNPJ.Equals(string.Empty))
                                {
                                    Lista[38].Valor = "02";
                                }
                                else if (ObjMConsumer.CPF != null)
                                {
                                    Lista[38].Valor = "01";
                                }

                                //verifica se é cpf ou cnpj
                                if (ObjMConsumer.CNPJ != null && !ObjMConsumer.CNPJ.Equals(string.Empty))
                                {
                                    Lista[39].Valor = ObjMConsumer.CNPJ;
                                }
                                else if (ObjMConsumer.CPF != null)
                                {
                                    Lista[39].Valor = ObjMConsumer.CPF;
                                }

                                Lista[40].Valor = ObjMConsumer.Name;
                                Lista[41].Valor = string.Empty;
                                Lista[42].Valor = string.Empty;
                                Lista[43].Valor = string.Empty;
                                Lista[44].Valor = string.Empty;
                                Lista[45].Valor = string.Empty;
                                Lista[46].Valor = "2";

                                ObjStreamWriter.WriteLine(MCnab.GetLine(Lista));
                                #endregion

                                #region parametros Trailler

                                Lista = Cnab.GetCnab(ObjConteCorrente.Layout, ObjConteCorrente.Versao, ObjConteCorrente.Data, 19, 1);

                                Lista[0].Valor = "9";
                                Lista[1].Valor = string.Empty;
                                Lista[2].Valor = "3";

                                ObjStreamWriter.WriteLine(MCnab.GetLine(Lista));

                                #endregion

                                ObjStreamWriter.Close();

                            }
                            break;
                    }
                    #endregion
                    break;
                case BankNumber.Citibank:
                    #region Layout do arquivo
                    switch (string.Format("{0}_{1}_{2}", ObjConteCorrente.Layout, ObjConteCorrente.Versao, ObjConteCorrente.Data.ToString("dd/MM/yyyy")))
                    {
                        case "400_01_08/09/2004"://layout_versao_datadoarquivo
                            //se o arquivo ja existe adiciona uma nova linha
                            if (File.Exists(PathContaCorrente))
                            {
                                int AllLines = GetLength(PathContaCorrente), contador = 0;

                                StringBuilder ObjStringBuilderCont = new StringBuilder();
                                StreamReader ObjStreamReaderCont = new StreamReader(PathContaCorrente);
                                IList<MCnab> Lista;
                                while (ObjStreamReaderCont.Peek() > 0)
                                {
                                    contador++;
                                    string strLine = ObjStreamReaderCont.ReadLine();
                                    if (contador < AllLines)
                                    {
                                        ObjStringBuilderCont.Append(strLine);
                                        ObjStringBuilderCont.AppendLine();
                                    }
                                }
                                ObjStreamReaderCont.Close();

                                File.Delete(PathContaCorrente);
                                File.WriteAllText(PathContaCorrente, ObjStringBuilderCont.ToString());

                                ObjStreamWriter = new StreamWriter(PathContaCorrente, true);

                                #region Registro de transação - tipo 1
                                Lista = Cnab.GetCnab(ObjConteCorrente.Layout, ObjConteCorrente.Versao, ObjConteCorrente.Data, 3, 1);
                                Lista[0].Valor = "1";
                                Lista[1].Valor = paymentInfo.NumAgencia;
                                Lista[2].Valor = paymentInfo.DigVerAg;
                                Lista[3].Valor = string.Empty;//verificar o q é isso
                                Lista[4].Valor = paymentInfo.NumContCorrent;
                                Lista[5].Valor = paymentInfo.DigVerCont;
                                Lista[6].Valor = "0";

                                Lista[7].Valor = ObjConteCorrente.Carteira;
                                Lista[8].Valor = ObjConteCorrente.Agencia;
                                Lista[9].Valor = ObjConteCorrente.ContaCorrente;

                                Lista[10].Valor = "T" + NrDocum.ToString();
                                Lista[11].Valor = Convert.ToInt32(ObjConteCorrente.NumBanco).ToString();
                                Lista[12].Valor = string.Empty;
                                Lista[13].Valor = string.Empty;
                                Lista[14].Valor = string.Empty;
                                Lista[15].Valor = string.Empty;
                                Lista[16].Valor = "1";
                                Lista[17].Valor = "N";
                                Lista[18].Valor = string.Empty;
                                Lista[19].Valor = string.Empty;
                                Lista[20].Valor = "2";
                                Lista[21].Valor = string.Empty;
                                Lista[22].Valor = string.Empty;
                                Lista[23].Valor = "1";
                                Lista[24].Valor = paymentInfo.Vencimento.Date.ToString("ddMMyy");
                                Lista[25].Valor = attempt.price.ToString().Replace(",", string.Empty);
                                Lista[26].Valor = Convert.ToInt32(ObjConteCorrente.NumBanco).ToString();
                                Lista[27].Valor = "0785";
                                Lista[28].Valor = "99";
                                Lista[29].Valor = "N";
                                Lista[30].Valor = DateTime.Now.Date.ToString("ddMMyy"); ;
                                Lista[31].Valor = string.Empty;
                                Lista[32].Valor = string.Empty;
                                Lista[33].Valor = string.Empty;
                                Lista[34].Valor = string.Empty;
                                Lista[35].Valor = string.Empty;
                                Lista[36].Valor = string.Empty;
                                Lista[37].Valor = string.Empty;

                                //verifica se é cpf ou cnpj
                                if (ObjMConsumer.CNPJ != null && !ObjMConsumer.CNPJ.Equals(string.Empty))
                                {
                                    Lista[38].Valor = "02";
                                }
                                else if (ObjMConsumer.CPF != null)
                                {
                                    Lista[38].Valor = "01";
                                }

                                //verifica se é cpf ou cnpj
                                if (ObjMConsumer.CNPJ != null && !ObjMConsumer.CNPJ.Equals(string.Empty))
                                {
                                    Lista[39].Valor = ObjMConsumer.CNPJ;
                                }
                                else if (ObjMConsumer.CPF != null)
                                {
                                    Lista[39].Valor = ObjMConsumer.CPF;
                                }

                                Lista[40].Valor = ObjMConsumer.Name;
                                Lista[41].Valor = string.Empty;
                                Lista[42].Valor = string.Empty;
                                Lista[43].Valor = string.Empty;
                                Lista[44].Valor = string.Empty;
                                Lista[45].Valor = string.Empty;
                                Lista[46].Valor = Convert.ToString(AllLines);

                                ObjStreamWriter.WriteLine(MCnab.GetLine(Lista));


                                #endregion

                                #region parametros Trailler

                                Lista = Cnab.GetCnab(ObjConteCorrente.Layout, ObjConteCorrente.Versao, ObjConteCorrente.Data, 19, 1);

                                Lista[0].Valor = "9";
                                Lista[1].Valor = string.Empty;
                                Lista[2].Valor = Convert.ToString(AllLines + 1);

                                ObjStreamWriter.WriteLine(MCnab.GetLine(Lista));

                                #endregion

                                ObjStreamWriter.Close();
                            }
                            //se o arquivo não existe adicina o Header
                            else
                            {
                                ObjStreamWriter = File.CreateText(PathContaCorrente);

                                IList<MCnab> Lista;
                                Lista = Cnab.GetCnab(ObjConteCorrente.Layout, ObjConteCorrente.Versao, ObjConteCorrente.Data, 1, 1);

                                #region Parametros Header
                                Lista[0].Valor = "0";
                                Lista[1].Valor = "1";
                                Lista[2].Valor = "REMESSA";
                                Lista[3].Valor = "O1";
                                Lista[4].Valor = "COBRANCA";
                                Lista[5].Valor = ObjConteCorrente.CodConvenio;
                                Lista[6].Valor = ObjConteCorrente.NEmpresa;
                                Lista[7].Valor = Convert.ToInt32(ObjConteCorrente.NumBanco).ToString();
                                Lista[8].Valor = ObjConteCorrente.NBanco;
                                Lista[9].Valor = DateTime.Now.Date.ToString("ddMMyy");
                                Lista[10].Valor = string.Empty;
                                Lista[11].Valor = "MX";
                                Lista[12].Valor = ObjConteCorrente.NumSeq.ToString();
                                Lista[13].Valor = string.Empty;
                                Lista[14].Valor = "1";

                                ObjStreamWriter.WriteLine(MCnab.GetLine(Lista));
                                #endregion

                                #region Registro de transação - tipo 1
                                Lista = Cnab.GetCnab(ObjConteCorrente.Layout, ObjConteCorrente.Versao, ObjConteCorrente.Data, 3, 1);
                                Lista[0].Valor = "1";
                                Lista[1].Valor = paymentInfo.NumAgencia;
                                Lista[2].Valor = paymentInfo.DigVerAg;
                                Lista[3].Valor = string.Empty;//verificar o q é isso
                                Lista[4].Valor = paymentInfo.NumContCorrent;
                                Lista[5].Valor = paymentInfo.DigVerCont;
                                Lista[6].Valor = "0";

                                Lista[7].Valor = ObjConteCorrente.Carteira;
                                Lista[8].Valor = ObjConteCorrente.Agencia;
                                Lista[9].Valor = ObjConteCorrente.ContaCorrente;

                                Lista[10].Valor = "T" + NrDocum.ToString();
                                Lista[11].Valor = Convert.ToInt32(ObjConteCorrente.NumBanco).ToString();
                                Lista[12].Valor = string.Empty;
                                Lista[13].Valor = string.Empty;
                                Lista[14].Valor = string.Empty;
                                Lista[15].Valor = string.Empty;
                                Lista[16].Valor = "1";
                                Lista[17].Valor = "N";
                                Lista[18].Valor = string.Empty;
                                Lista[19].Valor = string.Empty;
                                Lista[20].Valor = "2";
                                Lista[21].Valor = string.Empty;
                                Lista[22].Valor = string.Empty;
                                Lista[23].Valor = "1";
                                Lista[24].Valor = paymentInfo.Vencimento.Date.ToString("ddMMyy");
                                Lista[25].Valor = attempt.price.ToString().Replace(",", string.Empty);
                                Lista[26].Valor = Convert.ToInt32(ObjConteCorrente.NumBanco).ToString();
                                Lista[27].Valor = "0785";
                                Lista[28].Valor = "99";
                                Lista[29].Valor = "N";
                                Lista[30].Valor = DateTime.Now.Date.ToString("ddMMyy"); ;
                                Lista[31].Valor = string.Empty;
                                Lista[32].Valor = string.Empty;
                                Lista[33].Valor = string.Empty;
                                Lista[34].Valor = string.Empty;
                                Lista[35].Valor = string.Empty;
                                Lista[36].Valor = string.Empty;
                                Lista[37].Valor = string.Empty;

                                //verifica se é cpf ou cnpj
                                if (ObjMConsumer.CNPJ != null && !ObjMConsumer.CNPJ.Equals(string.Empty))
                                {
                                    Lista[38].Valor = "02";
                                }
                                else if (ObjMConsumer.CPF != null)
                                {
                                    Lista[38].Valor = "01";
                                }

                                //verifica se é cpf ou cnpj
                                if (ObjMConsumer.CNPJ != null && !ObjMConsumer.CNPJ.Equals(string.Empty))
                                {
                                    Lista[39].Valor = ObjMConsumer.CNPJ;
                                }
                                else if (ObjMConsumer.CPF != null)
                                {
                                    Lista[39].Valor = ObjMConsumer.CPF;
                                }

                                Lista[40].Valor = ObjMConsumer.Name;
                                Lista[41].Valor = string.Empty;
                                Lista[42].Valor = string.Empty;
                                Lista[43].Valor = string.Empty;
                                Lista[44].Valor = string.Empty;
                                Lista[45].Valor = string.Empty;
                                Lista[46].Valor = "2";

                                ObjStreamWriter.WriteLine(MCnab.GetLine(Lista));
                                #endregion

                                #region parametros Trailler

                                Lista = Cnab.GetCnab(ObjConteCorrente.Layout, ObjConteCorrente.Versao, ObjConteCorrente.Data, 19, 1);

                                Lista[0].Valor = "9";
                                Lista[1].Valor = string.Empty;
                                Lista[2].Valor = "3";

                                ObjStreamWriter.WriteLine(MCnab.GetLine(Lista));

                                #endregion

                                ObjStreamWriter.Close();

                            }
                            break;
                    }
                    #endregion
                    break;
                case BankNumber.Safra:
                    #region Layout do arquivo
                    switch (string.Format("{0}_{1}_{2}", ObjConteCorrente.Layout, ObjConteCorrente.Versao, ObjConteCorrente.Data.ToString("dd/MM/yyyy")))
                    {
                        case "400_01_08/09/2004"://layout_versao_datadoarquivo
                            //se o arquivo ja existe adiciona uma nova linha
                            if (File.Exists(PathContaCorrente))
                            {
                                int AllLines = GetLength(PathContaCorrente), contador = 0;

                                StringBuilder ObjStringBuilderCont = new StringBuilder();
                                StreamReader ObjStreamReaderCont = new StreamReader(PathContaCorrente);
                                IList<MCnab> Lista;
                                while (ObjStreamReaderCont.Peek() > 0)
                                {
                                    contador++;
                                    string strLine = ObjStreamReaderCont.ReadLine();
                                    if (contador < AllLines)
                                    {
                                        ObjStringBuilderCont.Append(strLine);
                                        ObjStringBuilderCont.AppendLine();
                                    }
                                }
                                ObjStreamReaderCont.Close();

                                File.Delete(PathContaCorrente);
                                File.WriteAllText(PathContaCorrente, ObjStringBuilderCont.ToString());

                                ObjStreamWriter = new StreamWriter(PathContaCorrente, true);

                                #region Registro de transação - tipo 1
                                Lista = Cnab.GetCnab(ObjConteCorrente.Layout, ObjConteCorrente.Versao, ObjConteCorrente.Data, 3, 1);
                                Lista[0].Valor = "1";
                                Lista[1].Valor = paymentInfo.NumAgencia;
                                Lista[2].Valor = paymentInfo.DigVerAg;
                                Lista[3].Valor = string.Empty;//verificar o q é isso
                                Lista[4].Valor = paymentInfo.NumContCorrent;
                                Lista[5].Valor = paymentInfo.DigVerCont;
                                Lista[6].Valor = "0";

                                Lista[7].Valor = ObjConteCorrente.Carteira;
                                Lista[8].Valor = ObjConteCorrente.Agencia;
                                Lista[9].Valor = ObjConteCorrente.ContaCorrente;

                                Lista[10].Valor = "T" + NrDocum.ToString();
                                Lista[11].Valor = Convert.ToInt32(ObjConteCorrente.NumBanco).ToString();
                                Lista[12].Valor = string.Empty;
                                Lista[13].Valor = string.Empty;
                                Lista[14].Valor = string.Empty;
                                Lista[15].Valor = string.Empty;
                                Lista[16].Valor = "1";
                                Lista[17].Valor = "N";
                                Lista[18].Valor = string.Empty;
                                Lista[19].Valor = string.Empty;
                                Lista[20].Valor = "2";
                                Lista[21].Valor = string.Empty;
                                Lista[22].Valor = string.Empty;
                                Lista[23].Valor = "1";
                                Lista[24].Valor = paymentInfo.Vencimento.Date.ToString("ddMMyy");
                                Lista[25].Valor = attempt.price.ToString().Replace(",", string.Empty);
                                Lista[26].Valor = Convert.ToInt32(ObjConteCorrente.NumBanco).ToString();
                                Lista[27].Valor = "0785";
                                Lista[28].Valor = "99";
                                Lista[29].Valor = "N";
                                Lista[30].Valor = DateTime.Now.Date.ToString("ddMMyy"); ;
                                Lista[31].Valor = string.Empty;
                                Lista[32].Valor = string.Empty;
                                Lista[33].Valor = string.Empty;
                                Lista[34].Valor = string.Empty;
                                Lista[35].Valor = string.Empty;
                                Lista[36].Valor = string.Empty;
                                Lista[37].Valor = string.Empty;

                                //verifica se é cpf ou cnpj
                                if (ObjMConsumer.CNPJ != null && !ObjMConsumer.CNPJ.Equals(string.Empty))
                                {
                                    Lista[38].Valor = "02";
                                }
                                else if (ObjMConsumer.CPF != null)
                                {
                                    Lista[38].Valor = "01";
                                }

                                //verifica se é cpf ou cnpj
                                if (ObjMConsumer.CNPJ != null && !ObjMConsumer.CNPJ.Equals(string.Empty))
                                {
                                    Lista[39].Valor = ObjMConsumer.CNPJ;
                                }
                                else if (ObjMConsumer.CPF != null)
                                {
                                    Lista[39].Valor = ObjMConsumer.CPF;
                                }

                                Lista[40].Valor = ObjMConsumer.Name;
                                Lista[41].Valor = string.Empty;
                                Lista[42].Valor = string.Empty;
                                Lista[43].Valor = string.Empty;
                                Lista[44].Valor = string.Empty;
                                Lista[45].Valor = string.Empty;
                                Lista[46].Valor = Convert.ToString(AllLines);

                                ObjStreamWriter.WriteLine(MCnab.GetLine(Lista));


                                #endregion

                                #region parametros Trailler

                                Lista = Cnab.GetCnab(ObjConteCorrente.Layout, ObjConteCorrente.Versao, ObjConteCorrente.Data, 19, 1);

                                Lista[0].Valor = "9";
                                Lista[1].Valor = string.Empty;
                                Lista[2].Valor = Convert.ToString(AllLines + 1);

                                ObjStreamWriter.WriteLine(MCnab.GetLine(Lista));

                                #endregion

                                ObjStreamWriter.Close();
                            }
                            //se o arquivo não existe adicina o Header
                            else
                            {
                                ObjStreamWriter = File.CreateText(PathContaCorrente);

                                IList<MCnab> Lista;
                                Lista = Cnab.GetCnab(ObjConteCorrente.Layout, ObjConteCorrente.Versao, ObjConteCorrente.Data, 1, 1);

                                #region Parametros Header
                                Lista[0].Valor = "0";
                                Lista[1].Valor = "1";
                                Lista[2].Valor = "REMESSA";
                                Lista[3].Valor = "O1";
                                Lista[4].Valor = "COBRANCA";
                                Lista[5].Valor = ObjConteCorrente.CodConvenio;
                                Lista[6].Valor = ObjConteCorrente.NEmpresa;
                                Lista[7].Valor = Convert.ToInt32(ObjConteCorrente.NumBanco).ToString();
                                Lista[8].Valor = ObjConteCorrente.NBanco;
                                Lista[9].Valor = DateTime.Now.Date.ToString("ddMMyy");
                                Lista[10].Valor = string.Empty;
                                Lista[11].Valor = "MX";
                                Lista[12].Valor = ObjConteCorrente.NumSeq.ToString();
                                Lista[13].Valor = string.Empty;
                                Lista[14].Valor = "1";

                                ObjStreamWriter.WriteLine(MCnab.GetLine(Lista));
                                #endregion

                                #region Registro de transação - tipo 1
                                Lista = Cnab.GetCnab(ObjConteCorrente.Layout, ObjConteCorrente.Versao, ObjConteCorrente.Data, 3, 1);
                                Lista[0].Valor = "1";
                                Lista[1].Valor = paymentInfo.NumAgencia;
                                Lista[2].Valor = paymentInfo.DigVerAg;
                                Lista[3].Valor = string.Empty;//verificar o q é isso
                                Lista[4].Valor = paymentInfo.NumContCorrent;
                                Lista[5].Valor = paymentInfo.DigVerCont;
                                Lista[6].Valor = "0";

                                Lista[7].Valor = ObjConteCorrente.Carteira;
                                Lista[8].Valor = ObjConteCorrente.Agencia;
                                Lista[9].Valor = ObjConteCorrente.ContaCorrente;

                                Lista[10].Valor = "T" + NrDocum.ToString();
                                Lista[11].Valor = Convert.ToInt32(ObjConteCorrente.NumBanco).ToString();
                                Lista[12].Valor = string.Empty;
                                Lista[13].Valor = string.Empty;
                                Lista[14].Valor = string.Empty;
                                Lista[15].Valor = string.Empty;
                                Lista[16].Valor = "1";
                                Lista[17].Valor = "N";
                                Lista[18].Valor = string.Empty;
                                Lista[19].Valor = string.Empty;
                                Lista[20].Valor = "2";
                                Lista[21].Valor = string.Empty;
                                Lista[22].Valor = string.Empty;
                                Lista[23].Valor = "1";
                                Lista[24].Valor = paymentInfo.Vencimento.Date.ToString("ddMMyy");
                                Lista[25].Valor = attempt.price.ToString().Replace(",", string.Empty);
                                Lista[26].Valor = Convert.ToInt32(ObjConteCorrente.NumBanco).ToString();
                                Lista[27].Valor = "0785";
                                Lista[28].Valor = "99";
                                Lista[29].Valor = "N";
                                Lista[30].Valor = DateTime.Now.Date.ToString("ddMMyy"); ;
                                Lista[31].Valor = string.Empty;
                                Lista[32].Valor = string.Empty;
                                Lista[33].Valor = string.Empty;
                                Lista[34].Valor = string.Empty;
                                Lista[35].Valor = string.Empty;
                                Lista[36].Valor = string.Empty;
                                Lista[37].Valor = string.Empty;

                                //verifica se é cpf ou cnpj
                                if (ObjMConsumer.CNPJ != null && !ObjMConsumer.CNPJ.Equals(string.Empty))
                                {
                                    Lista[38].Valor = "02";
                                }
                                else if (ObjMConsumer.CPF != null)
                                {
                                    Lista[38].Valor = "01";
                                }

                                //verifica se é cpf ou cnpj
                                if (ObjMConsumer.CNPJ != null && !ObjMConsumer.CNPJ.Equals(string.Empty))
                                {
                                    Lista[39].Valor = ObjMConsumer.CNPJ;
                                }
                                else if (ObjMConsumer.CPF != null)
                                {
                                    Lista[39].Valor = ObjMConsumer.CPF;
                                }

                                Lista[40].Valor = ObjMConsumer.Name;
                                Lista[41].Valor = string.Empty;
                                Lista[42].Valor = string.Empty;
                                Lista[43].Valor = string.Empty;
                                Lista[44].Valor = string.Empty;
                                Lista[45].Valor = string.Empty;
                                Lista[46].Valor = "2";

                                ObjStreamWriter.WriteLine(MCnab.GetLine(Lista));
                                #endregion

                                #region parametros Trailler

                                Lista = Cnab.GetCnab(ObjConteCorrente.Layout, ObjConteCorrente.Versao, ObjConteCorrente.Data, 19, 1);

                                Lista[0].Valor = "9";
                                Lista[1].Valor = string.Empty;
                                Lista[2].Valor = "3";

                                ObjStreamWriter.WriteLine(MCnab.GetLine(Lista));

                                #endregion

                                ObjStreamWriter.Close();

                            }
                            break;
                    }
                    #endregion
                    break;
                case BankNumber.MercantilDoBrasil:
                    #region Layout do arquivo
                    switch (string.Format("{0}_{1}_{2}", ObjConteCorrente.Layout, ObjConteCorrente.Versao, ObjConteCorrente.Data.ToString("dd/MM/yyyy")))
                    {
                        case "400_01_08/09/2004"://layout_versao_datadoarquivo
                            //se o arquivo ja existe adiciona uma nova linha
                            if (File.Exists(PathContaCorrente))
                            {
                                int AllLines = GetLength(PathContaCorrente), contador = 0;

                                StringBuilder ObjStringBuilderCont = new StringBuilder();
                                StreamReader ObjStreamReaderCont = new StreamReader(PathContaCorrente);
                                IList<MCnab> Lista;
                                while (ObjStreamReaderCont.Peek() > 0)
                                {
                                    contador++;
                                    string strLine = ObjStreamReaderCont.ReadLine();
                                    if (contador < AllLines)
                                    {
                                        ObjStringBuilderCont.Append(strLine);
                                        ObjStringBuilderCont.AppendLine();
                                    }
                                }
                                ObjStreamReaderCont.Close();

                                File.Delete(PathContaCorrente);
                                File.WriteAllText(PathContaCorrente, ObjStringBuilderCont.ToString());

                                ObjStreamWriter = new StreamWriter(PathContaCorrente, true);

                                #region Registro de transação - tipo 1
                                Lista = Cnab.GetCnab(ObjConteCorrente.Layout, ObjConteCorrente.Versao, ObjConteCorrente.Data, 3, 1);
                                Lista[0].Valor = "1";
                                Lista[1].Valor = paymentInfo.NumAgencia;
                                Lista[2].Valor = paymentInfo.DigVerAg;
                                Lista[3].Valor = string.Empty;//verificar o q é isso
                                Lista[4].Valor = paymentInfo.NumContCorrent;
                                Lista[5].Valor = paymentInfo.DigVerCont;
                                Lista[6].Valor = "0";

                                Lista[7].Valor = ObjConteCorrente.Carteira;
                                Lista[8].Valor = ObjConteCorrente.Agencia;
                                Lista[9].Valor = ObjConteCorrente.ContaCorrente;

                                Lista[10].Valor = "T" + NrDocum.ToString();
                                Lista[11].Valor = Convert.ToInt32(ObjConteCorrente.NumBanco).ToString();
                                Lista[12].Valor = string.Empty;
                                Lista[13].Valor = string.Empty;
                                Lista[14].Valor = string.Empty;
                                Lista[15].Valor = string.Empty;
                                Lista[16].Valor = "1";
                                Lista[17].Valor = "N";
                                Lista[18].Valor = string.Empty;
                                Lista[19].Valor = string.Empty;
                                Lista[20].Valor = "2";
                                Lista[21].Valor = string.Empty;
                                Lista[22].Valor = string.Empty;
                                Lista[23].Valor = "1";
                                Lista[24].Valor = paymentInfo.Vencimento.Date.ToString("ddMMyy");
                                Lista[25].Valor = attempt.price.ToString().Replace(",", string.Empty);
                                Lista[26].Valor = Convert.ToInt32(ObjConteCorrente.NumBanco).ToString();
                                Lista[27].Valor = "0785";
                                Lista[28].Valor = "99";
                                Lista[29].Valor = "N";
                                Lista[30].Valor = DateTime.Now.Date.ToString("ddMMyy"); ;
                                Lista[31].Valor = string.Empty;
                                Lista[32].Valor = string.Empty;
                                Lista[33].Valor = string.Empty;
                                Lista[34].Valor = string.Empty;
                                Lista[35].Valor = string.Empty;
                                Lista[36].Valor = string.Empty;
                                Lista[37].Valor = string.Empty;

                                //verifica se é cpf ou cnpj
                                if (ObjMConsumer.CNPJ != null && !ObjMConsumer.CNPJ.Equals(string.Empty))
                                {
                                    Lista[38].Valor = "02";
                                }
                                else if (ObjMConsumer.CPF != null)
                                {
                                    Lista[38].Valor = "01";
                                }

                                //verifica se é cpf ou cnpj
                                if (ObjMConsumer.CNPJ != null && !ObjMConsumer.CNPJ.Equals(string.Empty))
                                {
                                    Lista[39].Valor = ObjMConsumer.CNPJ;
                                }
                                else if (ObjMConsumer.CPF != null)
                                {
                                    Lista[39].Valor = ObjMConsumer.CPF;
                                }

                                Lista[40].Valor = ObjMConsumer.Name;
                                Lista[41].Valor = string.Empty;
                                Lista[42].Valor = string.Empty;
                                Lista[43].Valor = string.Empty;
                                Lista[44].Valor = string.Empty;
                                Lista[45].Valor = string.Empty;
                                Lista[46].Valor = Convert.ToString(AllLines);

                                ObjStreamWriter.WriteLine(MCnab.GetLine(Lista));


                                #endregion

                                #region parametros Trailler

                                Lista = Cnab.GetCnab(ObjConteCorrente.Layout, ObjConteCorrente.Versao, ObjConteCorrente.Data, 19, 1);

                                Lista[0].Valor = "9";
                                Lista[1].Valor = string.Empty;
                                Lista[2].Valor = Convert.ToString(AllLines + 1);

                                ObjStreamWriter.WriteLine(MCnab.GetLine(Lista));

                                #endregion

                                ObjStreamWriter.Close();
                            }
                            //se o arquivo não existe adicina o Header
                            else
                            {
                                ObjStreamWriter = File.CreateText(PathContaCorrente);

                                IList<MCnab> Lista;
                                Lista = Cnab.GetCnab(ObjConteCorrente.Layout, ObjConteCorrente.Versao, ObjConteCorrente.Data, 1, 1);

                                #region Parametros Header
                                Lista[0].Valor = "0";
                                Lista[1].Valor = "1";
                                Lista[2].Valor = "REMESSA";
                                Lista[3].Valor = "O1";
                                Lista[4].Valor = "COBRANCA";
                                Lista[5].Valor = ObjConteCorrente.CodConvenio;
                                Lista[6].Valor = ObjConteCorrente.NEmpresa;
                                Lista[7].Valor = Convert.ToInt32(ObjConteCorrente.NumBanco).ToString();
                                Lista[8].Valor = ObjConteCorrente.NBanco;
                                Lista[9].Valor = DateTime.Now.Date.ToString("ddMMyy");
                                Lista[10].Valor = string.Empty;
                                Lista[11].Valor = "MX";
                                Lista[12].Valor = ObjConteCorrente.NumSeq.ToString();
                                Lista[13].Valor = string.Empty;
                                Lista[14].Valor = "1";

                                ObjStreamWriter.WriteLine(MCnab.GetLine(Lista));
                                #endregion

                                #region Registro de transação - tipo 1
                                Lista = Cnab.GetCnab(ObjConteCorrente.Layout, ObjConteCorrente.Versao, ObjConteCorrente.Data, 3, 1);
                                Lista[0].Valor = "1";
                                Lista[1].Valor = paymentInfo.NumAgencia;
                                Lista[2].Valor = paymentInfo.DigVerAg;
                                Lista[3].Valor = string.Empty;//verificar o q é isso
                                Lista[4].Valor = paymentInfo.NumContCorrent;
                                Lista[5].Valor = paymentInfo.DigVerCont;
                                Lista[6].Valor = "0";

                                Lista[7].Valor = ObjConteCorrente.Carteira;
                                Lista[8].Valor = ObjConteCorrente.Agencia;
                                Lista[9].Valor = ObjConteCorrente.ContaCorrente;

                                Lista[10].Valor = "T" + NrDocum.ToString();
                                Lista[11].Valor = Convert.ToInt32(ObjConteCorrente.NumBanco).ToString();
                                Lista[12].Valor = string.Empty;
                                Lista[13].Valor = string.Empty;
                                Lista[14].Valor = string.Empty;
                                Lista[15].Valor = string.Empty;
                                Lista[16].Valor = "1";
                                Lista[17].Valor = "N";
                                Lista[18].Valor = string.Empty;
                                Lista[19].Valor = string.Empty;
                                Lista[20].Valor = "2";
                                Lista[21].Valor = string.Empty;
                                Lista[22].Valor = string.Empty;
                                Lista[23].Valor = "1";
                                Lista[24].Valor = paymentInfo.Vencimento.Date.ToString("ddMMyy");
                                Lista[25].Valor = attempt.price.ToString().Replace(",", string.Empty);
                                Lista[26].Valor = Convert.ToInt32(ObjConteCorrente.NumBanco).ToString();
                                Lista[27].Valor = "0785";
                                Lista[28].Valor = "99";
                                Lista[29].Valor = "N";
                                Lista[30].Valor = DateTime.Now.Date.ToString("ddMMyy"); ;
                                Lista[31].Valor = string.Empty;
                                Lista[32].Valor = string.Empty;
                                Lista[33].Valor = string.Empty;
                                Lista[34].Valor = string.Empty;
                                Lista[35].Valor = string.Empty;
                                Lista[36].Valor = string.Empty;
                                Lista[37].Valor = string.Empty;

                                //verifica se é cpf ou cnpj
                                if (ObjMConsumer.CNPJ != null && !ObjMConsumer.CNPJ.Equals(string.Empty))
                                {
                                    Lista[38].Valor = "02";
                                }
                                else if (ObjMConsumer.CPF != null)
                                {
                                    Lista[38].Valor = "01";
                                }

                                //verifica se é cpf ou cnpj
                                if (ObjMConsumer.CNPJ != null && !ObjMConsumer.CNPJ.Equals(string.Empty))
                                {
                                    Lista[39].Valor = ObjMConsumer.CNPJ;
                                }
                                else if (ObjMConsumer.CPF != null)
                                {
                                    Lista[39].Valor = ObjMConsumer.CPF;
                                }

                                Lista[40].Valor = ObjMConsumer.Name;
                                Lista[41].Valor = string.Empty;
                                Lista[42].Valor = string.Empty;
                                Lista[43].Valor = string.Empty;
                                Lista[44].Valor = string.Empty;
                                Lista[45].Valor = string.Empty;
                                Lista[46].Valor = "2";

                                ObjStreamWriter.WriteLine(MCnab.GetLine(Lista));
                                #endregion

                                #region parametros Trailler

                                Lista = Cnab.GetCnab(ObjConteCorrente.Layout, ObjConteCorrente.Versao, ObjConteCorrente.Data, 19, 1);

                                Lista[0].Valor = "9";
                                Lista[1].Valor = string.Empty;
                                Lista[2].Valor = "3";

                                ObjStreamWriter.WriteLine(MCnab.GetLine(Lista));

                                #endregion

                                ObjStreamWriter.Close();

                            }
                            break;
                    }
                    #endregion
                    break;
                case BankNumber.ItaúBank:
                    #region Layout do arquivo
                    switch (string.Format("{0}_{1}_{2}", ObjConteCorrente.Layout, ObjConteCorrente.Versao, ObjConteCorrente.Data.ToString("dd/MM/yyyy")))
                    {
                        case "400_01_08/09/2004"://layout_versao_datadoarquivo
                            //se o arquivo ja existe adiciona uma nova linha
                            if (File.Exists(PathContaCorrente))
                            {
                                int AllLines = GetLength(PathContaCorrente), contador = 0;

                                StringBuilder ObjStringBuilderCont = new StringBuilder();
                                StreamReader ObjStreamReaderCont = new StreamReader(PathContaCorrente);
                                IList<MCnab> Lista;
                                while (ObjStreamReaderCont.Peek() > 0)
                                {
                                    contador++;
                                    string strLine = ObjStreamReaderCont.ReadLine();
                                    if (contador < AllLines)
                                    {
                                        ObjStringBuilderCont.Append(strLine);
                                        ObjStringBuilderCont.AppendLine();
                                    }
                                }
                                ObjStreamReaderCont.Close();

                                File.Delete(PathContaCorrente);
                                File.WriteAllText(PathContaCorrente, ObjStringBuilderCont.ToString());

                                ObjStreamWriter = new StreamWriter(PathContaCorrente, true);

                                #region Registro de transação - tipo 1
                                Lista = Cnab.GetCnab(ObjConteCorrente.Layout, ObjConteCorrente.Versao, ObjConteCorrente.Data, 3, 1);
                                Lista[0].Valor = "1";
                                Lista[1].Valor = paymentInfo.NumAgencia;
                                Lista[2].Valor = paymentInfo.DigVerAg;
                                Lista[3].Valor = string.Empty;//verificar o q é isso
                                Lista[4].Valor = paymentInfo.NumContCorrent;
                                Lista[5].Valor = paymentInfo.DigVerCont;
                                Lista[6].Valor = "0";

                                Lista[7].Valor = ObjConteCorrente.Carteira;
                                Lista[8].Valor = ObjConteCorrente.Agencia;
                                Lista[9].Valor = ObjConteCorrente.ContaCorrente;

                                Lista[10].Valor = "T" + NrDocum.ToString();
                                Lista[11].Valor = Convert.ToInt32(ObjConteCorrente.NumBanco).ToString();
                                Lista[12].Valor = string.Empty;
                                Lista[13].Valor = string.Empty;
                                Lista[14].Valor = string.Empty;
                                Lista[15].Valor = string.Empty;
                                Lista[16].Valor = "1";
                                Lista[17].Valor = "N";
                                Lista[18].Valor = string.Empty;
                                Lista[19].Valor = string.Empty;
                                Lista[20].Valor = "2";
                                Lista[21].Valor = string.Empty;
                                Lista[22].Valor = string.Empty;
                                Lista[23].Valor = "1";
                                Lista[24].Valor = paymentInfo.Vencimento.Date.ToString("ddMMyy");
                                Lista[25].Valor = attempt.price.ToString().Replace(",", string.Empty);
                                Lista[26].Valor = Convert.ToInt32(ObjConteCorrente.NumBanco).ToString();
                                Lista[27].Valor = "0785";
                                Lista[28].Valor = "99";
                                Lista[29].Valor = "N";
                                Lista[30].Valor = DateTime.Now.Date.ToString("ddMMyy"); ;
                                Lista[31].Valor = string.Empty;
                                Lista[32].Valor = string.Empty;
                                Lista[33].Valor = string.Empty;
                                Lista[34].Valor = string.Empty;
                                Lista[35].Valor = string.Empty;
                                Lista[36].Valor = string.Empty;
                                Lista[37].Valor = string.Empty;

                                //verifica se é cpf ou cnpj
                                if (ObjMConsumer.CNPJ != null && !ObjMConsumer.CNPJ.Equals(string.Empty))
                                {
                                    Lista[38].Valor = "02";
                                }
                                else if (ObjMConsumer.CPF != null)
                                {
                                    Lista[38].Valor = "01";
                                }

                                //verifica se é cpf ou cnpj
                                if (ObjMConsumer.CNPJ != null && !ObjMConsumer.CNPJ.Equals(string.Empty))
                                {
                                    Lista[39].Valor = ObjMConsumer.CNPJ;
                                }
                                else if (ObjMConsumer.CPF != null)
                                {
                                    Lista[39].Valor = ObjMConsumer.CPF;
                                }

                                Lista[40].Valor = ObjMConsumer.Name;
                                Lista[41].Valor = string.Empty;
                                Lista[42].Valor = string.Empty;
                                Lista[43].Valor = string.Empty;
                                Lista[44].Valor = string.Empty;
                                Lista[45].Valor = string.Empty;
                                Lista[46].Valor = Convert.ToString(AllLines);

                                ObjStreamWriter.WriteLine(MCnab.GetLine(Lista));


                                #endregion

                                #region parametros Trailler

                                Lista = Cnab.GetCnab(ObjConteCorrente.Layout, ObjConteCorrente.Versao, ObjConteCorrente.Data, 19, 1);

                                Lista[0].Valor = "9";
                                Lista[1].Valor = string.Empty;
                                Lista[2].Valor = Convert.ToString(AllLines + 1);

                                ObjStreamWriter.WriteLine(MCnab.GetLine(Lista));

                                #endregion

                                ObjStreamWriter.Close();
                            }
                            //se o arquivo não existe adicina o Header
                            else
                            {
                                ObjStreamWriter = File.CreateText(PathContaCorrente);

                                IList<MCnab> Lista;
                                Lista = Cnab.GetCnab(ObjConteCorrente.Layout, ObjConteCorrente.Versao, ObjConteCorrente.Data, 1, 1);

                                #region Parametros Header
                                Lista[0].Valor = "0";
                                Lista[1].Valor = "1";
                                Lista[2].Valor = "REMESSA";
                                Lista[3].Valor = "O1";
                                Lista[4].Valor = "COBRANCA";
                                Lista[5].Valor = ObjConteCorrente.CodConvenio;
                                Lista[6].Valor = ObjConteCorrente.NEmpresa;
                                Lista[7].Valor = Convert.ToInt32(ObjConteCorrente.NumBanco).ToString();
                                Lista[8].Valor = ObjConteCorrente.NBanco;
                                Lista[9].Valor = DateTime.Now.Date.ToString("ddMMyy");
                                Lista[10].Valor = string.Empty;
                                Lista[11].Valor = "MX";
                                Lista[12].Valor = ObjConteCorrente.NumSeq.ToString();
                                Lista[13].Valor = string.Empty;
                                Lista[14].Valor = "1";

                                ObjStreamWriter.WriteLine(MCnab.GetLine(Lista));
                                #endregion

                                #region Registro de transação - tipo 1
                                Lista = Cnab.GetCnab(ObjConteCorrente.Layout, ObjConteCorrente.Versao, ObjConteCorrente.Data, 3, 1);
                                Lista[0].Valor = "1";
                                Lista[1].Valor = paymentInfo.NumAgencia;
                                Lista[2].Valor = paymentInfo.DigVerAg;
                                Lista[3].Valor = string.Empty;//verificar o q é isso
                                Lista[4].Valor = paymentInfo.NumContCorrent;
                                Lista[5].Valor = paymentInfo.DigVerCont;
                                Lista[6].Valor = "0";

                                Lista[7].Valor = ObjConteCorrente.Carteira;
                                Lista[8].Valor = ObjConteCorrente.Agencia;
                                Lista[9].Valor = ObjConteCorrente.ContaCorrente;

                                Lista[10].Valor = "T" + NrDocum.ToString();
                                Lista[11].Valor = Convert.ToInt32(ObjConteCorrente.NumBanco).ToString();
                                Lista[12].Valor = string.Empty;
                                Lista[13].Valor = string.Empty;
                                Lista[14].Valor = string.Empty;
                                Lista[15].Valor = string.Empty;
                                Lista[16].Valor = "1";
                                Lista[17].Valor = "N";
                                Lista[18].Valor = string.Empty;
                                Lista[19].Valor = string.Empty;
                                Lista[20].Valor = "2";
                                Lista[21].Valor = string.Empty;
                                Lista[22].Valor = string.Empty;
                                Lista[23].Valor = "1";
                                Lista[24].Valor = paymentInfo.Vencimento.Date.ToString("ddMMyy");
                                Lista[25].Valor = attempt.price.ToString().Replace(",", string.Empty);
                                Lista[26].Valor = Convert.ToInt32(ObjConteCorrente.NumBanco).ToString();
                                Lista[27].Valor = "0785";
                                Lista[28].Valor = "99";
                                Lista[29].Valor = "N";
                                Lista[30].Valor = DateTime.Now.Date.ToString("ddMMyy"); ;
                                Lista[31].Valor = string.Empty;
                                Lista[32].Valor = string.Empty;
                                Lista[33].Valor = string.Empty;
                                Lista[34].Valor = string.Empty;
                                Lista[35].Valor = string.Empty;
                                Lista[36].Valor = string.Empty;
                                Lista[37].Valor = string.Empty;

                                //verifica se é cpf ou cnpj
                                if (ObjMConsumer.CNPJ != null && !ObjMConsumer.CNPJ.Equals(string.Empty))
                                {
                                    Lista[38].Valor = "02";
                                }
                                else if (ObjMConsumer.CPF != null)
                                {
                                    Lista[38].Valor = "01";
                                }

                                //verifica se é cpf ou cnpj
                                if (ObjMConsumer.CNPJ != null && !ObjMConsumer.CNPJ.Equals(string.Empty))
                                {
                                    Lista[39].Valor = ObjMConsumer.CNPJ;
                                }
                                else if (ObjMConsumer.CPF != null)
                                {
                                    Lista[39].Valor = ObjMConsumer.CPF;
                                }

                                Lista[40].Valor = ObjMConsumer.Name;
                                Lista[41].Valor = string.Empty;
                                Lista[42].Valor = string.Empty;
                                Lista[43].Valor = string.Empty;
                                Lista[44].Valor = string.Empty;
                                Lista[45].Valor = string.Empty;
                                Lista[46].Valor = "2";

                                ObjStreamWriter.WriteLine(MCnab.GetLine(Lista));
                                #endregion

                                #region parametros Trailler

                                Lista = Cnab.GetCnab(ObjConteCorrente.Layout, ObjConteCorrente.Versao, ObjConteCorrente.Data, 19, 1);

                                Lista[0].Valor = "9";
                                Lista[1].Valor = string.Empty;
                                Lista[2].Valor = "3";

                                ObjStreamWriter.WriteLine(MCnab.GetLine(Lista));

                                #endregion

                                ObjStreamWriter.Close();

                            }
                            break;
                    }
                    #endregion
                    break;

                #endregion
            }
            */
            #endregion
        }

        private static int GetLength(string Path)
        {
            StreamReader ObjStreamReader = new StreamReader(Path);

            int cont = 0;
            while (ObjStreamReader.Peek() > 0)
            {
                ObjStreamReader.ReadLine();
                cont++;
            }
            ObjStreamReader.Close();

            return cont;
        }
    }
}
