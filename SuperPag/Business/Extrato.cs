using System;
using System.Collections.Generic;
using System.Text;
using SuperPag.Business.Messages;
using SuperPag.Framework;
using SuperPag.Data.Messages;
using SuperPag.Data;

namespace SuperPag.Business
{
    public class Extrato
    {
        public static void CalculateTotal(int storeId, DateTime startDate, DateTime endDate, out decimal totalPaid, out decimal totalNonPaid, int[] paymentForms)
        {
            int status = (int)SuperPag.Business.Messages.MPaymentAttempt.PaymentAttemptStatus.Paid;
            DPaymentAttemptComplete[] dAttempts = DataFactory.PaymentAttempt().List(storeId, startDate, endDate, status, paymentForms);

            totalPaid = 0m;
            totalNonPaid = 0m;
            if (dAttempts != null)
            {
                foreach (DPaymentAttemptComplete dAttempt in dAttempts)
                {
                    if (dAttempt.status == (int)SuperPag.Business.Messages.MPaymentAttempt.PaymentAttemptStatus.Paid)
                    {
                        totalPaid += dAttempt.dOrder.finalAmount;
                    }
                    else
                    {
                        totalNonPaid += dAttempt.dOrder.finalAmount;
                    }
                }
            }
        }

        public static MCExtrato ListFinancial(int storeId, DateTime startDate, DateTime endDate, int[] paymentForms)
        {
            MCExtrato mcExtrato = new MCExtrato();

            int status = (int)SuperPag.Business.Messages.MPaymentAttempt.PaymentAttemptStatus.Paid;
            DPaymentAttemptComplete[] dAttempts = DataFactory.PaymentAttempt().ListFinancial(storeId, startDate, endDate, status, paymentForms, new int[] { (int)SuperPag.PaymentForms.BoletoBancoDoBrasil, (int)SuperPag.PaymentForms.BoletoBradesco, (int)SuperPag.PaymentForms.BoletoItau, (int)SuperPag.PaymentForms.BoletoHSBC });

            //para cada tentativa
            if (dAttempts != null)
            {
                foreach (DPaymentAttemptComplete dAttempt in dAttempts)
                {
                    CreateRecord(mcExtrato, dAttempt);
                }
            }

            return mcExtrato;
        }
        
        public static MCExtrato2 ListFinancial2(int storeId, DateTime startDate, DateTime endDate)
        {
            DExtratoFinanceiro2[] dExtrato = DataFactory.ExtratoFinanceiro2().List(startDate, endDate, storeId);
            MCExtrato2 mcExtrato2 = new MCExtrato2();

            if(dExtrato != null)
            {
                foreach (DExtratoFinanceiro2 linha in dExtrato)
                {
                    MExtrato2 mLinha = new MExtrato2();
                    mLinha.pedido = linha.pedido;
                    mLinha.arp = linha.arp;
                    mLinha.tid = linha.tid;
                    mLinha.numautor = linha.numautor;
                    mLinha.numcv = linha.numcv;
                    mLinha.numautent = linha.numautent;
                    mLinha.nossoNumero = linha.nossoNumero;
                    mLinha.valorParcela = linha.valorParcela;
                    mLinha.valorPedido = linha.valorPedido;
                    mLinha.valorRecebido = linha.valorRecebido;
                    mLinha.statusConciliacao = linha.statusConciliacao;
                    mLinha.valorLiq = linha.valorLiq;
                    mLinha.dataEntradaPedido = linha.dataEntradaPedido;
                    mLinha.dataParcela = linha.dataParcela;
                    mLinha.dataPagamento = linha.dataPagamento;
                    mLinha.statusCobranca = linha.statusCobranca;
                    mLinha.formaPagamento = linha.formaPagamento;
                    mLinha.qtdParcelas = linha.qtdParcelas;
                    mLinha.statusPostFinalizacao = linha.statusPostFinalizacao;
                    mLinha.statusPostPagamento = linha.statusPostPagamento;
                    mcExtrato2.Add(mLinha);
                }
            }

            return mcExtrato2;
        }

        public static MCExtrato ListMovement(int storeId, DateTime startDate, DateTime endDate, int[] paymentForms)
        {
            MCExtrato mcExtrato = new MCExtrato();

            DPaymentAttemptComplete[] dAttempts = DataFactory.PaymentAttempt().List(storeId, startDate, endDate, int.MinValue, paymentForms);

            //para cada tentativa
            if (dAttempts != null)
            {
                foreach (DPaymentAttemptComplete dAttempt in dAttempts)
                {
                    CreateRecord(mcExtrato, dAttempt);
                }
            }

            return mcExtrato;
        }

        private static void CreateRecord(MCExtrato mcExtrato, DPaymentAttemptComplete dAttempt)
        {
            //verifico se a attempt é de boleto
            if (dAttempt.dPaymentForm.paymentFormGroupId == (int)SuperPag.PaymentGroups.BoletoBancario)
            {
                //crio uma MExtrato para cada parcela
                for (int i = 1; i <= dAttempt.dOrder.installmentQuantity; i++)
                {
                    MExtrato mExtrato = new MExtrato();
                    mExtrato.Date = dAttempt.startTime;
                    mExtrato.AttemptId = dAttempt.paymentAttemptId;
                    mExtrato.PaymentDate = dAttempt.startTime;
                    mExtrato.Group = dAttempt.dPaymentForm.paymentFormGroupId;
                    mExtrato.StoreReferenceOrder = dAttempt.dOrder.storeReferenceOrder;
                    mExtrato.InstalmentNumber = i;
                    mExtrato.Status = dAttempt.status;

                    mExtrato.Value = dAttempt.dOrder.finalAmount / dAttempt.dOrder.installmentQuantity;

                    //TODO: usar data relation (performance)
                    DConsumer dConsumer = DataFactory.Consumer().Locate(dAttempt.dOrder.consumerId);
                    mExtrato.ConsumerName = dConsumer.name;

                    //adiciono
                    mcExtrato.Add(mExtrato);
                }
            }
            else //crio uma MExtrato só
            {
                MExtrato mExtrato = new MExtrato();
                mExtrato.Date = dAttempt.startTime;
                
                //TODO: modificar o super pag para incluir a data de pagamento
                mExtrato.PaymentDate = dAttempt.startTime;

                mExtrato.Group = dAttempt.dPaymentForm.paymentFormGroupId;
                mExtrato.StoreReferenceOrder = dAttempt.dOrder.storeReferenceOrder;
                mExtrato.InstalmentNumber = 1;
                mExtrato.Status = dAttempt.status;
                mExtrato.AttemptId = dAttempt.paymentAttemptId;
                mExtrato.Value = dAttempt.dOrder.finalAmount;

                DConsumer dConsumer = DataFactory.Consumer().Locate(dAttempt.dOrder.consumerId); //TODO: usar data relation

                mExtrato.ConsumerName = dConsumer.name;

                //adiciono
                mcExtrato.Add(mExtrato);
            }
        }
    }
}
