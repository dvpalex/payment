using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using SuperPag.Business.Messages;
using SuperPag.Agents.DepId.Messages;
using System.IO;

namespace RetornoDepIdentificado
{
    public class Test
    {
        public static XmlDocument GetTestXml()
        {
            MPaymentAttemptDepIdReturn attemptReturn = new MPaymentAttemptDepIdReturn();
            attemptReturn.Header = new MPaymentAttemptDepIdReturnHeader();
            attemptReturn.Header.AgencyDV = "0";
            attemptReturn.Header.AgencyNumber = 0102;
            attemptReturn.Header.AssignorDV = "1";
            attemptReturn.Header.AssignorNumber = 0123456789;
            attemptReturn.Header.BankNumber = 77777777;
            attemptReturn.Header.CompanyCode = 237;
            attemptReturn.Header.CreationDateCapturedFile = DateTime.Now;
            attemptReturn.Header.HeaderId = 0;
            attemptReturn.Header.NameOfArquivedFile = "testeatila";
            attemptReturn.Header.NameOfCapturedFile = "testeatila";
            attemptReturn.Header.NumberOfDetailsRecords = 1;
            attemptReturn.Header.ProcessDate = DateTime.Now;
            attemptReturn.Header.RecordFileDate = DateTime.Now;
            attemptReturn.Header.SequencialReturnNumber = 1;
                       
            attemptReturn.Details = new MPaymentAttemptDepIdReturnDetails();
            //Detail Comum
            attemptReturn.Details.Detail = new List<MPaymentAttemptDepIdReturnDetail>();
            MPaymentAttemptDepIdReturnDetail detail1 = new MPaymentAttemptDepIdReturnDetail();
            detail1.Status = SuperPag.DepIdStatusEnum.AttemptNotFound;
            detail1.Ag_acolhedora = 0589;
            detail1.Cod_canal_distribuicao = 01;
            detail1.CreationDate = DateTime.Now;
            detail1.Data_deposito = DateTime.Now;
            detail1.Digito_agencia = 0;
            detail1.HeaderDepIdentId = 1;
            detail1.Num_sequencia_arquivo = 2;
            detail1.Numero_documento = 555555555;
            detail1.Remetente_deposito = "923940900-1";
            detail1.Valor_deposito_cheque = 0;
            detail1.Valor_deposito_dinheiro = 2;
            detail1.Valor_total_deposito = 3;
            detail1.Checks = new MPaymentAttemptDepIdReturnChks();
            detail1.Checks.Checks = new List<MPaymentAttemptDepIdReturnChk>();
            //MPaymentAttemptDepIdReturnChk cheque = new MPaymentAttemptDepIdReturnChk();
            //cheque.Ag_acolhedora_cheque = 9874;
            //cheque.Cod_agencia_cheque = 6952;
            //cheque.Cod_banco = 547;
            //cheque.CreationDate = DateTime.Now;
            //cheque.Data_deposito_cheque = DateTime.Now;
            //cheque.Dig_acolhedora_cheque = 8;
            //cheque.Numero_cheque = "1234567";
            //cheque.Sequencia_arquivo = 3;
            //cheque.Vlr_cheque = 1;
            //cheque.Vlr_deposito_total = 3;
            //detail1.Checks.Checks.Add(cheque);
            //MPaymentAttemptDepIdReturnChk cheque1 = new MPaymentAttemptDepIdReturnChk();
            //cheque1.Ag_acolhedora_cheque = 9874;
            //cheque1.Cod_agencia_cheque = 6952;
            //cheque1.Cod_banco = 547;
            //cheque1.CreationDate = DateTime.Now;
            //cheque1.Data_deposito_cheque = DateTime.Now;
            //cheque1.Dig_acolhedora_cheque = 8;
            //cheque1.Numero_cheque = "0011225";
            //cheque1.Sequencia_arquivo = 4;
            //cheque1.Vlr_cheque = 1;
            //cheque1.Vlr_deposito_total = 3;
            //detail1.Checks.Checks.Add(cheque1);

            //MPaymentAttemptDepIdReturnChk cheque2 = new MPaymentAttemptDepIdReturnChk();
            //cheque2.Ag_acolhedora_cheque = 9874;
            //cheque2.Cod_agencia_cheque = 6952;
            //cheque2.Cod_banco = 547;
            //cheque2.CreationDate = DateTime.Now;
            //cheque2.Data_deposito_cheque = DateTime.Now;
            //cheque2.Dig_acolhedora_cheque = 8;
            //cheque2.Numero_cheque = "0011225";
            //cheque2.Sequencia_arquivo = 4;
            //cheque2.Vlr_cheque = 1;
            //cheque2.Vlr_deposito_total = 3;
            //detail1.Checks.Checks.Add(cheque2);

            //MPaymentAttemptDepIdReturnChk cheque3 = new MPaymentAttemptDepIdReturnChk();
            //cheque3.Ag_acolhedora_cheque = 9874;
            //cheque3.Cod_agencia_cheque = 6952;
            //cheque3.Cod_banco = 547;
            //cheque3.CreationDate = DateTime.Now;
            //cheque3.Data_deposito_cheque = DateTime.Now;
            //cheque3.Dig_acolhedora_cheque = 8;
            //cheque3.Numero_cheque = "0011225";
            //cheque3.Sequencia_arquivo = 4;
            //cheque3.Vlr_cheque = 1;
            //cheque3.Vlr_deposito_total = 3;
            //detail1.Checks.Checks.Add(cheque3);

            //MPaymentAttemptDepIdReturnChk cheque4 = new MPaymentAttemptDepIdReturnChk();
            //cheque4.Ag_acolhedora_cheque = 9874;
            //cheque4.Cod_agencia_cheque = 6952;
            //cheque4.Cod_banco = 547;
            //cheque4.CreationDate = DateTime.Now;
            //cheque4.Data_deposito_cheque = DateTime.Now;
            //cheque4.Dig_acolhedora_cheque = 8;
            //cheque4.Numero_cheque = "0011225";
            //cheque4.Sequencia_arquivo = 4;
            //cheque4.Vlr_cheque = 1;
            //cheque4.Vlr_deposito_total = 3;
            //detail1.Checks.Checks.Add(cheque4);
            attemptReturn.Details.Detail.Add(detail1);

            XmlSerializer serializer = new XmlSerializer(typeof(MPaymentAttemptDepIdReturn));
            StringWriter a = new StringWriter();
            serializer.Serialize(a, attemptReturn);
            string xml = a.GetStringBuilder().ToString();
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);
            return xmlDoc;
        }
    }
}
