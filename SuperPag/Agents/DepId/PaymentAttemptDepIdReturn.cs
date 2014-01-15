using System;
using System.Collections.Generic;
using System.Text;
using SuperPag.Business.Messages;
using SuperPag.Data.Messages;
using SuperPag.Data;
using SuperPag.Framework;
using SuperPag.Agents.DepId.Messages;
using SuperPag.Helper;
using SuperPag;

namespace SuperPag.Agents.DepId.Messages
{
    public class PaymentAttemptDepIdReturn
    {
        private DPaymentAttemptDepId _dPaymentAttemptDepId = null;
        private DPaymentAttempt _dPaymentAttempt = null;
        private DOrder _dOrder = null;

        public void ProcessDepIdReturn(MPaymentAttemptDepIdReturn mPaymentAttemptDepIdReturn)
        {
            //TODO: contexto transacional
            //Salva o header do arquivo cnab
            mPaymentAttemptDepIdReturn.Header.ProcessDate = DateTime.Now;
            mPaymentAttemptDepIdReturn.Header.NumberOfDetailsRecords = (mPaymentAttemptDepIdReturn.Details == null || mPaymentAttemptDepIdReturn.Details.Detail == null) ? 0 : mPaymentAttemptDepIdReturn.Details.Detail.Count;
            MessageMapper mapper = new MessageMapper();
            DPaymentAttemptDepIdReturnHeader dPaymentAttemptDepIdReturnHeader = (DPaymentAttemptDepIdReturnHeader)mapper.Do(mPaymentAttemptDepIdReturn.Header, typeof(DPaymentAttemptDepIdReturnHeader));
            DataFactory.PaymentAttemptDepIdReturnHeader().Insert(dPaymentAttemptDepIdReturnHeader);
            mPaymentAttemptDepIdReturn.Header.HeaderId = dPaymentAttemptDepIdReturnHeader.headerId;
            

            //Localizo as attempts de todos os retornos recebidos no arquivo
            if (mPaymentAttemptDepIdReturn.Details != null && mPaymentAttemptDepIdReturn.Details.Detail != null)
            {
                foreach (MPaymentAttemptDepIdReturnDetail mDetail in mPaymentAttemptDepIdReturn.Details.Detail)
                {
                    // atribuo o id do header ao detail para vinculo com o cabeçalho
                    mDetail.HeaderDepIdentId = mPaymentAttemptDepIdReturn.Header.HeaderId;
                    mDetail.CreationDate = DateTime.Now;
                    
                    // gravo o retorno do deposito identificado no banco de dados
                    mapper = new MessageMapper();
                    DPaymentAttemptDepIdReturn dPaymentAttemptDepIdReturn = (DPaymentAttemptDepIdReturn)mapper.Do(mDetail, typeof(DPaymentAttemptDepIdReturn));
                    DataFactory.PaymentAttemptDepIdReturn().Insert(dPaymentAttemptDepIdReturn);
                    mDetail.PaymentAttemptDepIdReturnId = dPaymentAttemptDepIdReturn.paymentAttemptDepIdReturnId;

                    // gravo retorno dos cheques no banco
                    if (mDetail.Checks != null)
                    {
                        foreach (MPaymentAttemptDepIdReturnChk chk in mDetail.Checks.Checks)
                        {
                            DPaymentAttemptDepIdReturnChk dPaymentAttemptDepIdReturnChk = (DPaymentAttemptDepIdReturnChk)mapper.Do(chk, typeof(DPaymentAttemptDepIdReturnChk));
                            dPaymentAttemptDepIdReturnChk.paymentAttemptDepIdReturnId = mDetail.PaymentAttemptDepIdReturnId;
                            dPaymentAttemptDepIdReturnChk.creationDate = DateTime.Now;
                            DataFactory.PaymentAttemptDepIdReturnChk().Insert(dPaymentAttemptDepIdReturnChk);
                        }
                    }
                    _dPaymentAttemptDepId = DataFactory.PaymentAttemptDepId().LocateLast(mDetail.Remetente_deposito);
                    if (_dPaymentAttemptDepId == null)
                        mDetail.Status = SuperPag.DepIdStatusEnum.AttemptNotFound;
                    else
                        mDetail.Status = (SuperPag.DepIdStatusEnum)PaymentAttemptDepId.GetStatus(_dPaymentAttemptDepId.paymentAttemptId);



                    if (mDetail.Status == SuperPag.DepIdStatusEnum.PaymentValueOk
                        || mDetail.Status == SuperPag.DepIdStatusEnum.BiggerPaymentValue)
                    {
                        // salva o id do PaymentAttemptDepIdReturn na PaymentAttemptDepId informando que existe uma resposta para a attempt
                        _dPaymentAttemptDepId.paymentAttemptDepIdReturnId = mDetail.PaymentAttemptDepIdReturnId;
                        DataFactory.PaymentAttemptDepId().Update(_dPaymentAttemptDepId);


                        // atualizo o status calculado da tentativa de pagamento de deposito identificado
                        _dPaymentAttempt = DataFactory.PaymentAttempt().Locate(_dPaymentAttemptDepId.paymentAttemptId);
                        _dPaymentAttemptDepId.paymentStatus = (int)mDetail.Status;
                        DataFactory.PaymentAttemptDepId().Update(_dPaymentAttemptDepId);

                        // salva o status da attempt como paga
                        _dPaymentAttempt.status = (int)SuperPag.PaymentAttemptStatus.Paid;
                        _dPaymentAttempt.lastUpdate = DateTime.Now;
                        DataFactory.PaymentAttempt().Update(_dPaymentAttempt);

                        // salva o status da order como paga
                        _dOrder = DataFactory.Order().Locate(_dPaymentAttempt.orderId);
                        GenericHelper.UpdateOrderStatusByAttemptStatus(_dOrder, _dPaymentAttempt.status);

                        // envia POST de Pagamento
                        SuperPag.Handshake.Helper.SendPaymentPost(_dPaymentAttempt.paymentAttemptId);
                    }
                    else if (mDetail.Status == SuperPag.DepIdStatusEnum.LesserPaymentValue)
                    {
                        // salva o id do PaymentAttemptDepIdReturn na PaymentAttemptDepId informando que existe uma resposta para a attempt
                        _dPaymentAttemptDepId.paymentAttemptDepIdReturnId = mDetail.PaymentAttemptDepIdReturnId;
                        DataFactory.PaymentAttemptDepId().Update(_dPaymentAttemptDepId);


                        // atualizo o status calculado da tentativa de pagamento de deposito identificado
                        _dPaymentAttempt = DataFactory.PaymentAttempt().Locate(_dPaymentAttemptDepId.paymentAttemptId);
                        _dPaymentAttemptDepId.paymentStatus = (int)mDetail.Status;
                        DataFactory.PaymentAttemptDepId().Update(_dPaymentAttemptDepId);
                    }
                }
            }
        }

        public bool HeaderExists(MPaymentAttemptDepIdReturnHeader mPaymentAttemptDepIdReturnHeader)
        {
            if (DataFactory.PaymentAttemptDepIdReturnHeader().Locate(mPaymentAttemptDepIdReturnHeader.NameOfCapturedFile) != null
                && DataFactory.PaymentAttemptDepIdReturnHeader().Locate(mPaymentAttemptDepIdReturnHeader.BankNumber,
                mPaymentAttemptDepIdReturnHeader.SequencialReturnNumber, mPaymentAttemptDepIdReturnHeader.CompanyCode) != null)
                return true;

            return false;
        }

        public List<MDeposit> ListDeposits(MPaymentAttemptDepId mPaymentAttemptDepId)
        {
            DPaymentAttemptDepIdReturn[] returns = DataFactory.PaymentAttemptDepIdReturn().List(mPaymentAttemptDepId.IdNumber);
            if (returns == null)
                return null;
            List<MDeposit> mDeposit = new List<MDeposit>();
            foreach (DPaymentAttemptDepIdReturn _depidReturn in returns)
            {
                MDeposit deposit = new MDeposit();
                deposit.Tipo = "Dinheiro";
                deposit.Data = _depidReturn.data_deposito.ToString("dd-MM-yyyy");
                deposit.Status = "Confirmado";
                deposit.Valor = _depidReturn.valor_deposito_dinheiro;
                deposit.Agencia = _depidReturn.ag_acolhedora;
                deposit.NumDocto = _depidReturn.numero_documento;
                deposit.Cod_Depositante = _depidReturn.remetente_deposito;
                mDeposit.Add(deposit);
                DPaymentAttemptDepIdReturnChk[] chk = DataFactory.PaymentAttemptDepIdReturnChk().List(_depidReturn.paymentAttemptDepIdReturnId);
                if (chk != null)
                {
                    foreach (DPaymentAttemptDepIdReturnChk _depIdReturnChk in chk)
                    {
                        MDeposit _deposit = new MDeposit();
                        _deposit.CheckId = _depIdReturnChk.checkId;
                        _deposit.Tipo = "Cheque";
                        _deposit.Data = _depIdReturnChk.data_deposito_cheque.ToString("dd-MM-yyyy");
                        _deposit.Status = (_depIdReturnChk.status == (int)MPaymentAttemptDepIdReturnChk.DepositStatusEnum.Confirmado) ? "Confirmado" : "A Confirmar";
                        _deposit.Valor = _depIdReturnChk.vlr_cheque;
                        _deposit.NumDocto = _depidReturn.numero_documento;
                        _deposit.Num_Cheque = _depIdReturnChk.numero_cheque;
                        _deposit.Cod_Depositante = _depIdReturnChk.cod_depositante_cheque;
                        _deposit.Agencia = _depidReturn.ag_acolhedora;
                        mDeposit.Add(_deposit);
                    }
                }
            }
            return mDeposit;
        }
    }
}
