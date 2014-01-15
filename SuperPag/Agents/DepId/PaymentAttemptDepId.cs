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
using SuperPag.Business;

namespace SuperPag.Agents.DepId.Messages
{
    public class PaymentAttemptDepId
    {
        public static MPaymentAttemptDepId Locate(Guid paymentAttemptId)
        {
            DPaymentAttemptDepId dPaymentAttemptDepId = DataFactory.PaymentAttemptDepId().Locate(paymentAttemptId);
            DPaymentAttempt dPaymentAttempt = DataFactory.PaymentAttempt().Locate(paymentAttemptId);

            MessageMapper mapper = new MessageMapper();
            MPaymentAttemptDepId mPaymentAttemptDepId = (MPaymentAttemptDepId)mapper.Do(dPaymentAttemptDepId, typeof(MPaymentAttemptDepId));
            mapper = new MessageMapper();
            MPaymentAttempt mPaymentAttempt = (MPaymentAttempt)mapper.Do(dPaymentAttempt, typeof(MPaymentAttempt));
            SuperPag.Framework.Helper.HierarchySet.BaseToChild(mPaymentAttempt, mPaymentAttemptDepId);
            return mPaymentAttemptDepId;
        }
        public static void Update(MPaymentAttemptDepId mPaymentAttemptDepId)
        {
            MessageMapper mapper = new MessageMapper();
            DPaymentAttemptDepId dPaymentAttemptDepId = (DPaymentAttemptDepId)mapper.Do(mPaymentAttemptDepId, typeof(DPaymentAttemptDepId));
            dPaymentAttemptDepId.paymentStatus = mPaymentAttemptDepId.PaymentStatus;
            DataFactory.PaymentAttemptDepId().Update(dPaymentAttemptDepId);
        }

        public static int GetStatus(Guid paymentAttemptId)
        {
            MPaymentAttemptDepId mPaymentAttemptDepId = PaymentAttemptDepId.Locate(paymentAttemptId);
            DPaymentAttemptDepIdReturn[] returns = DataFactory.PaymentAttemptDepIdReturn().List(mPaymentAttemptDepId.IdNumber);
            return GetStatus(returns, mPaymentAttemptDepId.Price);
        }
        public static int GetStatus(DPaymentAttemptDepIdReturn[] returns, decimal price)
        {
            decimal sum = Decimal.Zero;
            int returnStatus = (int)SuperPag.DepIdStatusEnum.LesserPaymentValue;

            foreach (DPaymentAttemptDepIdReturn depIdReturn in returns)
            {
                sum += depIdReturn.valor_deposito_dinheiro;
                DPaymentAttemptDepIdReturnChk[] chk = DataFactory.PaymentAttemptDepIdReturnChk().List(depIdReturn.paymentAttemptDepIdReturnId, (int)MPaymentAttemptDepIdReturnChk.DepositStatusEnum.Confirmado);

                if (chk != null)
                    foreach (DPaymentAttemptDepIdReturnChk _depIdReturnChk in chk)
                        sum += _depIdReturnChk.vlr_cheque;
            }

            if (sum == Decimal.Zero)
                return returnStatus;

            if (sum < price)
                returnStatus = (int)SuperPag.DepIdStatusEnum.LesserPaymentValue;
            else if (sum > price)
                returnStatus = (int)SuperPag.DepIdStatusEnum.BiggerPaymentValue;
            else
                returnStatus = (int)SuperPag.DepIdStatusEnum.PaymentValueOk;

            return returnStatus;
        }
    }
}

      