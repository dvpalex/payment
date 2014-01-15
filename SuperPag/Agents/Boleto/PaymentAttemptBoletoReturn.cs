using System;
using System.Collections.Generic;
using System.Text;
using SuperPag.Business.Messages;
using SuperPag.Data.Messages;
using SuperPag.Data;
using SuperPag.Framework;
using SuperPag.Agents.Boleto.Messages;

namespace SuperPag.Agents.Boleto
{
    public class PaymentAttemptBoletoReturn
    {
        private DPaymentAttemptBoleto _dPaymentAttemptBoleto = null;
        private DPaymentAttempt _dPaymentAttempt = null;
        private DOrder _dOrder = null;
    
        public void ProcessBoletoReturn(MPaymentAttemptBoletoReturn mPaymentAttemptBoletoReturn)
        {
            //TODO: contexto transacional

            //Salva o header do arquivo cnab
            mPaymentAttemptBoletoReturn.Header.ProcessDate = DateTime.Now;
            mPaymentAttemptBoletoReturn.Header.NumberOfDetailsRecords = (mPaymentAttemptBoletoReturn.Details == null || mPaymentAttemptBoletoReturn.Details.Detail == null) ? 0 : mPaymentAttemptBoletoReturn.Details.Detail.Count;
            MessageMapper mapper = new MessageMapper();
            DPaymentAttemptBoletoReturnHeader dPaymentAttemptBoletoReturnHeader = (DPaymentAttemptBoletoReturnHeader)mapper.Do(mPaymentAttemptBoletoReturn.Header, typeof(DPaymentAttemptBoletoReturnHeader));

            DataFactory.PaymentAttemptBoletoReturnHeader().Insert(dPaymentAttemptBoletoReturnHeader);
            mPaymentAttemptBoletoReturn.Header.HeaderId = dPaymentAttemptBoletoReturnHeader.headerId;

            //Localizo as attempts de todos os retornos recebidos no arquivo
            if (mPaymentAttemptBoletoReturn.Details != null && mPaymentAttemptBoletoReturn.Details.Detail != null)
            {
                foreach (MPaymentAttemptBoletoReturnDetail mDetail in mPaymentAttemptBoletoReturn.Details.Detail)
                {
                    //atribuo o id do header ao detail para vinculo com o cabeçalho
                    mDetail.HeaderBoletoId = mPaymentAttemptBoletoReturn.Header.HeaderId;
                    mDetail.CreationDate = DateTime.Now;

                    mDetail.BoletoReturnStatus = BoletoRules(mDetail);

                    //Gravo o retorno do boleto no banco de dados
                    mapper = new MessageMapper();
                    DPaymentAttemptBoletoReturn dPaymentAttemptBoletoReturn = (DPaymentAttemptBoletoReturn)mapper.Do(mDetail, typeof(DPaymentAttemptBoletoReturn));
                    DataFactory.PaymentAttemptBoletoReturn().Insert(dPaymentAttemptBoletoReturn);
                    mDetail.PaymentAttemptBoletoReturnId = dPaymentAttemptBoletoReturn.paymentAttemptBoletoReturnId;

                    if (mDetail.BoletoReturnStatus == MPaymentAttemptBoletoReturnDetail.BoletoReturnStatusEnum.ProcessOk)
                    {
                        //Salva o id do PaymentAttemptBoletoReturn na PaymentAttemptBoleto informando que existe uma resposta para a attempt
                        _dPaymentAttemptBoleto.paymentAttemptBoletoReturnId = mDetail.PaymentAttemptBoletoReturnId;
                        DataFactory.PaymentAttemptBoleto().Update(_dPaymentAttemptBoleto);

                        //Salva o status da attempt como paga
                        _dPaymentAttempt.status = (int)SuperPag.PaymentAttemptStatus.Paid;
                        _dPaymentAttempt.lastUpdate = DateTime.Now;
                        DataFactory.PaymentAttempt().Update(_dPaymentAttempt);

                        //Salva o status da order como paga
                        _dOrder = DataFactory.Order().Locate(_dPaymentAttempt.orderId);
                        _dOrder.status = (int)SuperPag.OrderStatus.Analysing;
                        _dOrder.lastUpdateDate = DateTime.Now;
                        DataFactory.Order().Update(_dOrder);

                        //Envia POST de Pagamento
                        SuperPag.Handshake.Helper.SendPaymentPost(_dPaymentAttempt.paymentAttemptId);
                    }
                }
            }
        }

        private MPaymentAttemptBoletoReturnDetail.BoletoReturnStatusEnum BoletoRules(MPaymentAttemptBoletoReturnDetail mDetail)
        {
            //Verificar de existe uma attempt relacionada            
            _dPaymentAttemptBoleto = DataFactory.PaymentAttemptBoleto().Locate(mDetail.NossoNumero);
            if (_dPaymentAttemptBoleto == null)
               return MPaymentAttemptBoletoReturnDetail.BoletoReturnStatusEnum.AttemptNotFound;

           // //Verificar se o pedido já foi relacionado com um retorno, ou seja, já foi liquidado 
           //if (_dPaymentAttemptBoleto.paymentAttemptBoletoReturnId > 0) 
           //     return MPaymentAttemptBoletoReturnDetail.BoletoReturnStatusEnum.DuplicatePayment;
            
            //Verifica o valor pago não é menor do que o valor do pedido
            _dPaymentAttempt = DataFactory.PaymentAttempt().Locate(_dPaymentAttemptBoleto.paymentAttemptId);
            if (mDetail.ValorRecebido < _dPaymentAttempt.price) //verdadeiro -> Boleto pago com valor menor do que consta no pedido
               return MPaymentAttemptBoletoReturnDetail.BoletoReturnStatusEnum.PaymentValueIncomplatible;


           return MPaymentAttemptBoletoReturnDetail.BoletoReturnStatusEnum.ProcessOk;
        }


        public bool HeaderExists(MPaymentAttemptBoletoReturnHeader mPaymentAttemptBoletoReturnHeader)
        {
            if (mPaymentAttemptBoletoReturnHeader == null)
                return false;
            if(DataFactory.PaymentAttemptBoletoReturnHeader().Locate(mPaymentAttemptBoletoReturnHeader.NameOfCapturedFile) == null)
                return false;
            if(DataFactory.PaymentAttemptBoletoReturnHeader().Locate(mPaymentAttemptBoletoReturnHeader.BankNumber, mPaymentAttemptBoletoReturnHeader.SequencialReturnNumber, mPaymentAttemptBoletoReturnHeader.CompanyCode) != null)
                return true;
            return false;
        }
    }
}
