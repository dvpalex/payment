using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;

namespace LegacyImporter
{
    public partial class frmPrincipal : Form
    {
        private static string strConnSuperPag = ConfigurationManager.ConnectionStrings["superpag"].ConnectionString;
        private static string strConnSmartPag = ConfigurationManager.ConnectionStrings["smartpag"].ConnectionString;
        private static int storeId = int.Parse(ConfigurationManager.AppSettings["storeid"]);
        private static int storeIdSmartPag = int.Parse(ConfigurationManager.AppSettings["storeidsmartpag"]);
        private bool cancelar = false;
        public List<string> erros = new List<string>();

        public frmPrincipal()
        {
            InitializeComponent();
        }

        private void frmPrincipal_Load(object sender, EventArgs e)
        {
            lblTitle.Text = "Importar loja " + storeIdSmartPag + " do SmartPag para a loja " + storeId + " do SuperPag";
            cmdCancelar.Enabled = false;
        }
        
        private void linkVer_Click(object sender, EventArgs e)
        {

            Form f = new frmErro();
            f.ShowDialog(this);
        }

        private void cmdCancelar_Click(object sender, EventArgs e)
        {
            cancelar = true;
            cmdCancelar.Enabled = false;
        }
        
        private void cmdImportar_Click(object sender, EventArgs e)
        {
            SqlConnection oConnSmartPag = null;
            SqlConnection oConnSuperPag = null;
            SqlDataReader oDR = null;

            long i = 0, sucesso = 0, erro = 0, jaimportado = 0;
            bool result;
            
            try
            {
                cmdImportar.Enabled = false;

                linkVer.Visible = false;
                lblErros.Text = "";
                lblSucesso.Text = "";
                lblJaImportado.Text = "";
                lblCurrent.Text = "";
                lblId.Text = "Consultando...";
                this.Refresh();

                cancelar = false;
                
                erros.Clear();

                oConnSmartPag = new SqlConnection(strConnSmartPag);
                oConnSmartPag.Open();
                oConnSuperPag = new SqlConnection(strConnSuperPag);
                oConnSuperPag.Open();

                using (SqlCommand oCmd = new SqlCommand("select * from clientes_internet_pedido cip with(nolock), clientes_internet ci with(nolock), endereco_entrega_pedido eep with(nolock) where cip.str_cpf = ci.str_cpf and cip.int_numero_pedido = ci.int_numero_pedido and cip.str_cpf = eep.str_cpf and cip.int_numero_pedido = eep.int_numero_pedido and int_codigo_parceiro = " + storeIdSmartPag, oConnSmartPag))
                {
                    oCmd.CommandType = CommandType.Text;
                    oCmd.CommandTimeout = 300;
                    oDR = oCmd.ExecuteReader(System.Data.CommandBehavior.Default);

                    lblId.Text = "";
                    this.Refresh();

                    cmdCancelar.Enabled = true;
                    
                    while (oDR.Read())
                    {
                        if (!PedidoProcessado(oDR, oConnSuperPag))
                        {
                            result = ProcessaPedido(oDR, oConnSuperPag);
                            if(result)
                                lblSucesso.Text = (++sucesso).ToString();
                            else
                                lblErros.Text = (++erro).ToString();
                        }
                        else
                            lblJaImportado.Text = (++jaimportado).ToString();

                        lblCurrent.Text = (++i).ToString();

                        this.Refresh();
                        Application.DoEvents();

                        if (cancelar)
                            break;
                    }
                }

                if (erro > 0)
                    linkVer.Visible = true;

                lblId.Text = "Finalizado";
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, null, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                #region Dispose dos objetos
                if (oDR != null)
                    oDR.Close();
                if (oConnSmartPag != null)
                {
                    oConnSmartPag.Close();
                    oConnSmartPag.Dispose();
                }
                if (oConnSuperPag != null)
                {
                    oConnSuperPag.Close();
                    oConnSuperPag.Dispose();
                }
                #endregion

                cmdImportar.Enabled = true;
                cmdCancelar.Enabled = false;
            }
        }

        #region ProcessaPedido
        private bool ProcessaPedido(SqlDataReader reader, SqlConnection conn)
        {
            SqlTransaction oTran = null;
            long consumerId, orderId;
            decimal intNumeroPedido = 0M;
            int paymentFormId;
            byte intCodigoMeioPagamento;
            decimal frete = 0;

            try
            {
                intNumeroPedido = (decimal)reader["INT_NUMERO_PEDIDO"];
                lblId.Text = intNumeroPedido.ToString() + "... ";

                oTran = conn.BeginTransaction();

                consumerId = (long)InsereConsumer(reader, conn, oTran);
                InsereConsumerAddress(consumerId, 2, reader, conn, oTran);
                InsereConsumerAddress(consumerId, 1, reader, conn, oTran);
                orderId = (long)InsereOrder(consumerId, reader, conn, oTran);
                
                if (!Convert.IsDBNull(reader["NUM_VALOR_FRETE"]))
                    frete = (decimal)reader["NUM_VALOR_FRETE"];

                InsereOrderItem(orderId, intNumeroPedido, frete, conn, oTran);

                if (!Convert.IsDBNull(reader["INT_CODIGO_MEIO_PAGAMENTO"]))
                {
                    intCodigoMeioPagamento = (byte)reader["INT_CODIGO_MEIO_PAGAMENTO"];
                    paymentFormId = InsereOrderInstallmentEAttemp(orderId, intCodigoMeioPagamento, intNumeroPedido, (decimal)reader["NUM_VALOR_FINAL_PEDIDO"], conn, oTran);
                }

                InsereSPLEgacyDBImport(orderId, intNumeroPedido, conn, oTran);

                oTran.Commit();

                return true;
            }
            catch (Exception err)
            {
                erros.Add("Pedido smartPag: " + intNumeroPedido + " Msg: " + err.Message);

                try
                {
                    if (oTran != null)
                        oTran.Rollback();
                }
                catch(Exception errrb)
                {
                    erros.Add("Pedido smartPag: " + intNumeroPedido + " Msg (rollback): " + errrb.Message);
                }

                return false;
            }
        }
        #endregion
        #region PedidoProcessado
        private bool PedidoProcessado(SqlDataReader reader, SqlConnection conn)
        {
            SqlDataReader oDR = null;
            
            try
            {
                decimal intNumeroPedido = (decimal)reader["INT_NUMERO_PEDIDO"];

                using (SqlCommand oCmd = new SqlCommand("select * from SPLegacyDBImport where INT_NUMERO_PEDIDO = " + intNumeroPedido, conn))
                {
                    oCmd.CommandType = CommandType.Text;
                    oDR = oCmd.ExecuteReader(System.Data.CommandBehavior.Default);
                    
                    if(oDR.Read())
                        return true;

                    return false;
                }
            }
            finally
            {
                if (oDR != null)
                    oDR.Close();
            }
        }
        #endregion

        #region InsereConsumer
        private decimal InsereConsumer(SqlDataReader reader, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmd = null;

            try
            {
                cmd = new SqlCommand("insert consumer(CPF, RG, CNPJ, IE, [name], birthDate, ger, civilState, occupation, phone, commercialPhone, celularPhone, fax, responsibleName, responsibleCPF, email) values(@CPF, @RG, @CNPJ, @IE, @name, @birthDate, @ger, @civilState, @occupation, @phone, @commercialPhone, @celularPhone, @fax, @responsibleName, @responsibleCPF, @email); select @@IDENTITY", conn, tran);

                SqlParameter parCPF = cmd.Parameters.Add("@CPF", SqlDbType.VarChar, 14);
                SqlParameter parRG = cmd.Parameters.Add("@RG", SqlDbType.VarChar, 14);
                SqlParameter parCNPJ = cmd.Parameters.Add("@CNPJ", SqlDbType.VarChar, 40);
                SqlParameter parIE = cmd.Parameters.Add("@IE", SqlDbType.VarChar, 40);
                SqlParameter parname = cmd.Parameters.Add("@name", SqlDbType.VarChar, 50);
                SqlParameter parbirthDate = cmd.Parameters.Add("@birthDate", SqlDbType.DateTime);
                SqlParameter parger = cmd.Parameters.Add("@ger", SqlDbType.VarChar, 20);
                SqlParameter parcivilState = cmd.Parameters.Add("@civilState", SqlDbType.VarChar, 20);
                SqlParameter paroccupation = cmd.Parameters.Add("@occupation", SqlDbType.VarChar, 40);
                SqlParameter parphone = cmd.Parameters.Add("@phone", SqlDbType.VarChar, 15);
                SqlParameter parcommercialPhone = cmd.Parameters.Add("@commercialPhone", SqlDbType.VarChar, 15);
                SqlParameter parcelularPhone = cmd.Parameters.Add("@celularPhone", SqlDbType.VarChar, 15);
                SqlParameter parfax = cmd.Parameters.Add("@fax", SqlDbType.VarChar, 15);
                SqlParameter parresponsibleName = cmd.Parameters.Add("@responsibleName", SqlDbType.VarChar, 50);
                SqlParameter parresponsibleCPF = cmd.Parameters.Add("@responsibleCPF", SqlDbType.VarChar, 14);
                SqlParameter paremail = cmd.Parameters.Add("@email", SqlDbType.VarChar, 255);

                parCPF.Value = (string)VerificaDBNull(reader["STR_CPF"]);
                parRG.Value = DBNull.Value;
                parCNPJ.Value = DBNull.Value;
                parIE.Value = (string)VerificaDBNull(reader["STR_INSCRICAO"]);
                parname.Value = (string)VerificaDBNull(reader["STR_NOME_CLIENTE"]);
                parbirthDate.Value = (DateTime)VerificaDBNull(reader["DAT_NASCIMENTO"]);
                parger.Value = (string)VerificaDBNull(reader["STR_SEXO"]);
                parcivilState.Value = (string)VerificaDBNull(reader["STR_ESTADO_CIVIL"]);
                paroccupation.Value = (string)VerificaDBNull(reader["STR_PROFISSAO"]);
                parphone.Value = (string)VerificaDBNull(reader["STR_TELEFONE"]);
                parcommercialPhone.Value = DBNull.Value;
                parcelularPhone.Value = (string)VerificaDBNull(reader["STR_CELULAR"]);
                parfax.Value = (string)VerificaDBNull(reader["STR_FAX"]);
                parresponsibleName.Value = (string)VerificaDBNull(reader["STR_NOME_RESPONSAVEL"]);
                parresponsibleCPF.Value = (string)VerificaDBNull(reader["STR_CPF_RESPONSAVEL"]);
                paremail.Value = (string)VerificaDBNull(reader["STR_EMAIL"]);

                return (decimal)cmd.ExecuteScalar();
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();
            }
        }
        #endregion
        #region InsereConsumerAddress
        private void InsereConsumerAddress(long consumerId, int addressType, SqlDataReader reader, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmd = null;

            try
            {
                cmd = new SqlCommand("INSERT INTO ConsumerAddress(consumerId, addressType, logradouro, address, addressNumber, addressComplement, cep, district, city, state, country) VALUES(@consumerId, @addressType, @logradouro, @address, @addressNumber, @addressComplement, @cep, @district, @city, @state, @country)", conn, tran);

                SqlParameter parconsumerId = cmd.Parameters.Add("@consumerId", SqlDbType.BigInt);
                SqlParameter paraddressType = cmd.Parameters.Add("@addressType", SqlDbType.Int);
                SqlParameter parlogradouro = cmd.Parameters.Add("@logradouro", SqlDbType.VarChar, 40);
                SqlParameter paraddress = cmd.Parameters.Add("@address", SqlDbType.VarChar, 150);
                SqlParameter paraddressNumber = cmd.Parameters.Add("@addressNumber", SqlDbType.VarChar, 10);
                SqlParameter paraddressComplement = cmd.Parameters.Add("@addressComplement", SqlDbType.VarChar, 100);
                SqlParameter parcep = cmd.Parameters.Add("@cep", SqlDbType.VarChar, 10);
                SqlParameter pardistrict = cmd.Parameters.Add("@district", SqlDbType.VarChar, 100);
                SqlParameter parcity = cmd.Parameters.Add("@city", SqlDbType.VarChar, 100);
                SqlParameter parcountry = cmd.Parameters.Add("@country", SqlDbType.Char, 50);
                SqlParameter parstate = cmd.Parameters.Add("@state", SqlDbType.VarChar, 100);

                parconsumerId.Value = (long)consumerId;
                paraddressType.Value = (int)addressType;
                if(addressType == 2)
                    parlogradouro.Value = (string)VerificaDBNull(reader["STR_LOGRADOURO"]);
                else
                    parlogradouro.Value = (string)VerificaDBNull(reader["STR_LOGRADOURO_ENTREGA_PEDIDO"]);
                if(addressType == 2)
                    paraddress.Value = (string)VerificaDBNull(reader["STR_ENDERECO"]);
                else
                    paraddress.Value = (string)VerificaDBNull(reader["STR_ENDERECO_ENTREGA_PEDIDO"]);
                if(addressType == 2)
                    paraddressNumber.Value = (string)VerificaDBNull(reader["STR_NUMERO_ENDERECO"]);
                else
                    paraddressNumber.Value = (string)VerificaDBNull(reader["STR_NUMERO_ENDERECO_ENTREGA_PEDIDO"]);
                if(addressType == 2)
                    paraddressComplement.Value = (string)VerificaDBNull(reader["STR_COMPLEMENTO_ENDERECO"]);
                else
                    paraddressComplement.Value = (string)VerificaDBNull(reader["STR_COMPLEMENTO_ENDERECO_ENTREGA_PEDIDO"]);
                if(addressType == 2)
                    parcep.Value = (string)VerificaDBNull(reader["STR_CEP"]);
                else
                    parcep.Value = (string)VerificaDBNull(reader["STR_CEP_ENTREGA_PEDIDO"]);
                if(addressType == 2)
                    pardistrict.Value = (string)VerificaDBNull(reader["STR_BAIRRO"]);
                else
                    pardistrict.Value = (string)VerificaDBNull(reader["STR_BAIRRO_ENTREGA_PEDIDO"]);
                if(addressType == 2)
                    parcity.Value = (string)VerificaDBNull(reader["STR_CIDADE"]);
                else
                    parcity.Value = (string)VerificaDBNull(reader["STR_CIDADE_ENTREGA_PEDIDO"]);
                if(addressType == 2)
                    parcountry.Value = (string)VerificaDBNull(reader["STR_PAIS"]);
                else
                    parcountry.Value = (string)VerificaDBNull(reader["STR_PAIS_ENTREGA_PEDIDO"]);
                if(addressType == 2)
                    parstate.Value = (string)VerificaDBNull(reader["STR_ESTADO"]);
                else
                    parstate.Value = (string)VerificaDBNull(reader["STR_ESTADO_ENTREGA_PEDIDO"]);

                cmd.ExecuteNonQuery();
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();
            }
        }
        #endregion
        #region InsereOrder
        private decimal InsereOrder(long consumerId, SqlDataReader reader, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmd = null;

            try
            {
                cmd = new SqlCommand("INSERT INTO [Order](storeId, consumerId, storeReferenceOrder, totalAmount, finalAmount, installmentQuantity, status, creationDate, lastUpdateDate, statusChangeUserId, statusChangeDate, cancelDescription) VALUES(@storeId, @consumerId, @storeReferenceOrder, @totalAmount, @finalAmount, @installmentQuantity, @status, @creationDate, @lastUpdateDate, @statusChangeUserId, @statusChangeDate, @cancelDescription); select @@IDENTITY", conn, tran);

                SqlParameter parstoreId = cmd.Parameters.Add("@storeId", SqlDbType.Int);
                SqlParameter parconsumerId = cmd.Parameters.Add("@consumerId", SqlDbType.BigInt);
                SqlParameter parstoreReferenceOrder = cmd.Parameters.Add("@storeReferenceOrder", SqlDbType.VarChar, 50);
                SqlParameter partotalAmount = cmd.Parameters.Add("@totalAmount", SqlDbType.Decimal);
                SqlParameter parfinalAmount = cmd.Parameters.Add("@finalAmount", SqlDbType.Decimal);
                SqlParameter parinstallmentQuantity = cmd.Parameters.Add("@installmentQuantity", SqlDbType.Int);
                SqlParameter parstatus = cmd.Parameters.Add("@status", SqlDbType.Int);
                SqlParameter parcreationDate = cmd.Parameters.Add("@creationDate", SqlDbType.DateTime);
                SqlParameter parlastUpdateDate = cmd.Parameters.Add("@lastUpdateDate", SqlDbType.DateTime);
                SqlParameter parstatusChangeUserId = cmd.Parameters.Add("@statusChangeUserId", SqlDbType.UniqueIdentifier);
                SqlParameter parstatusChangeDate = cmd.Parameters.Add("@statusChangeDate", SqlDbType.DateTime);
                SqlParameter parcancelDescription = cmd.Parameters.Add("@cancelDescription", SqlDbType.VarChar, 300);
                
                parstoreId.Value = storeId;
                parconsumerId.Value = consumerId;
                parstoreReferenceOrder.Value = VerificaDBNull(reader["STR_NUMERO_COMPRA_PARCEIRO"]);
                partotalAmount.Value = VerificaDBNull(reader["NUM_VALOR_PEDIDO"]);
                parfinalAmount.Value = VerificaDBNull(reader["NUM_VALOR_FINAL_PEDIDO"]);
                parinstallmentQuantity.Value = VerificaDBNull(reader["INT_NUMERO_PARCELAS_PEDIDO"]);
                parcreationDate.Value = VerificaDBNull(reader["DAT_ENTRADA_PEDIDO"]);
                parlastUpdateDate.Value = VerificaDBNull(reader["DAT_ATUALIZACAO"]);
                parstatusChangeUserId.Value = DBNull.Value;
                parstatusChangeDate.Value = DBNull.Value;
                parcancelDescription.Value = DBNull.Value;

                int status = 1; //... -> em analise
                if(!Convert.IsDBNull(reader["STR_FLAG_PEDIDO"]))
                    status = int.Parse((string)reader["STR_FLAG_PEDIDO"]);

                switch(status)
                {
                    case 0: //nao concluido -> nao concluido
                        parstatus.Value = (int)1;
                        break;
                    case 2: //em analise -> em analise
                        parstatus.Value = (int)2;
                        break;
                    case 3: //aprovado -> aprovado
                        parstatus.Value = (int)3;
                        break;
                    case 4: //cancelado -> cancelado
                        parstatus.Value = (int)4;
                        break;
                    case 5: //em transito -> em transito
                        parstatus.Value = (int)5;
                        break;
                    case 6: //entregue -> entregue
                        parstatus.Value = (int)6;
                        break;
                    case 7: //nao entregue -> nao entregue
                        parstatus.Value = (int)7;
                        break;
                    case 9: //cheques retirados -> em analise
                        parstatus.Value = (int)2;
                        break;
                    case 11: //devolver mercadoria -> em analise
                        parstatus.Value = (int)2;
                        break;
                    default: //... -> em analise
                        parstatus.Value = (int)2;
                        break;
                }
                
                return (decimal)cmd.ExecuteScalar();
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();
            }
        }
        #endregion
        #region InsereOrderInstallmentEAttemp
        private int InsereOrderInstallmentEAttemp(long orderId, byte intCodigoMeioPagamento, decimal intNumeroPedido, decimal numValorPedido, SqlConnection connSuperPag, SqlTransaction tranSuperPag)
        {
            SqlConnection connSmartPag = null;
            SqlCommand cmdSmartPag = null;
            SqlDataReader oDR = null;

            string tableName = "";
            object oNUMAUTOR = DBNull.Value, oNUMCV = DBNull.Value, oNUMAUTENT = DBNull.Value, oVN_ID_PEDIDO = DBNull.Value;
            object oVN_Cod_Autoriza = DBNull.Value, oMENSAGEM = DBNull.Value, oTIPO_PAGAMENTO = DBNull.Value, oTIPO_SHOPLINE = DBNull.Value, oCONVENIO = DBNull.Value;
            object oNUM_TIPO_PAGAMENTO = DBNull.Value, oNUM_PRAZO = DBNull.Value, oNUM_VALOR_PARCELA_PEDIDO = DBNull.Value, oSTR_CCNAME = DBNull.Value, oSTR_CCEMAIL = DBNull.Value, oSTR_CCTYPE = DBNull.Value, oSTR_ASSINATURA = DBNull.Value;
            DateTime dATUALIZACAO = DateTime.MinValue;
            int paymentFormId = 9999, stsuperpag = 1; //... -> nao concluido

            try
            {
                #region switch dos meios de pagamento
                switch (intCodigoMeioPagamento)
                {
                    case 1: //Boleto Bancário
                        tableName = "mp_boleto_bancario";
                        paymentFormId = 18;
                        break;
                    case 2: //Cartão de Crédito
                        tableName = "mp_cartao_credito";
                        paymentFormId = 2;
                        break;
                    case 3: //Espécie
                        tableName = "mp_especie";
                        paymentFormId = 9999;
                        break;
                    case 4: //Depósito Identificado
                        tableName = "mp_deposito_identificado";
                        paymentFormId = 9999;
                        break;
                    case 5: //Cheque
                        tableName = "mp_cheque";
                        paymentFormId = 9999;
                        break;
                    case 6: //Doc
                        tableName = "mp_doc";
                        paymentFormId = 19;
                        break;
                    case 7: //Financiamento Cheque-Pré
                        tableName = "mp_financiamento";
                        paymentFormId = 9999;
                        break;
                    case 9: //Cheque Eletrônico.com
                        tableName = "mp_cheque_eletronico";
                        paymentFormId = 20;
                        break;
                    case 10: //Financiamento PanAmericano
                        tableName = "mp_financiamento";
                        paymentFormId = 9999;
                        break;
                    case 12: //Financ. Finasa
                        tableName = "mp_financiamento";
                        paymentFormId = 9999;
                        break;
                    case 13: //Débito em Conta
                        tableName = "mp_debito_conta";
                        paymentFormId = 9999;
                        break;
                    case 14: //Pagamento Fácil Bradesco
                        tableName = "mp_pagamento_facil_bradesco";
                        paymentFormId = 17;
                        break;
                    case 15: //Boleto Bradesco (SPS)
                        tableName = "mp_pagamento_facil_bradesco";
                        paymentFormId = 9999;
                        break;
                    case 18: //Itaú ShopLine
                        tableName = "mp_itau_shopline";
                        paymentFormId = 7;
                        break;
                    case 19: //Boleto Itaú
                        tableName = "mp_boleto_bancario";
                        paymentFormId = 9999;
                        break;
                    case 20: //F2B
                        tableName = "mp_f2b";
                        paymentFormId = 9999;
                        break;
                    case 21: //BB PAG
                        tableName = "mp_bbpag";
                        paymentFormId = 5;
                        break;
                    case 23: //Financ. ABN-AMRO Bank
                        tableName = "mp_financiamento";
                        paymentFormId = 9999;
                        break;
                    case 24: //Vale Presente
                        tableName = "mp_vale_presente";
                        paymentFormId = 21;
                        break;
                    case 27: //Boleto Bancário Físico
                        tableName = "mp_boleto_bancario";
                        paymentFormId = 9999;
                        break;
                    case 31: //Financiamento VirtualCred
                        tableName = "mp_financiamento";
                        paymentFormId = 9999;
                        break;
                    case 32: //Boleto Bancário
                        tableName = "mp_boleto_bancario";
                        paymentFormId = 9;
                        break;
                    case 33: //Boleto Banco do Brasil
                        tableName = "mp_boleto_bancario";
                        paymentFormId = 6;
                        break;
                    case 36: //Boleto Bradesco Fisico
                        tableName = "mp_boleto_bancario";
                        paymentFormId = 9999;
                        break;
                }
                #endregion

                connSmartPag = new SqlConnection(strConnSmartPag);
                connSmartPag.Open();
                
                cmdSmartPag = new SqlCommand("select * from " + tableName + " where int_numero_pedido = " + intNumeroPedido, connSmartPag);
                oDR = cmdSmartPag.ExecuteReader(System.Data.CommandBehavior.Default);

                while (oDR.Read())
                {
                    #region if (intCodigoMeioPagamento == 2)
                    if (intCodigoMeioPagamento == 2)
                    {
                        byte opcartao = 99;
                        if (!Convert.IsDBNull(oDR["INT_CODIGO_OP_CARTAO"]))
                            opcartao = (byte)oDR["INT_CODIGO_OP_CARTAO"];

                        switch (opcartao)
                        {
                            case 23: //visa
                                paymentFormId = 8;
                                break;
                            case 41: //master
                                paymentFormId = 2;
                                break;
                            case 44: //diners
                                paymentFormId = 4;
                                break;
                            case 50: //amex
                                paymentFormId = 15;
                                break;
                            default:
                                paymentFormId = 9999;
                                break;
                        }
                    }
                    #endregion

                    int status = 5;
                    if (!Convert.IsDBNull(oDR["STR_FLAG_COBRANCA"]))
                        status = int.Parse((string)oDR["STR_FLAG_COBRANCA"]);

                    switch (status)
                    {
                        case 0: //restituido -> nao pago
                            stsuperpag = (int)3;
                            break;
                        case 1: //se boleto nao pago -> pendente caso contrário nao pago -> nao pago
                            if (paymentFormId == 6 || paymentFormId == 9)
                                stsuperpag = (int)5;
                            else
                                stsuperpag = (int)3;
                            break;
                        case 2: //pago -> pago
                            stsuperpag = (int)2;
                            break;
                        case 3: //a restituir -> pago
                            stsuperpag = (int)2;
                            break;
                        case 4: //restituido -> nao pago
                            stsuperpag = (int)3;
                            break;
                        default: //... -> nao concluido
                            stsuperpag = (int)1;
                            break;
                    }

                    InsereOrderInstallment(orderId, (tableName == "mp_cheque_eletronico" ? VerificaDBNull(oDR["INT_NUMERO_PARCELA"]) : VerificaDBNull(oDR["INT_NUMERO_PARCELA_MP"])), paymentFormId, VerificaDBNull(oDR["NUM_VALOR_PARCELA_PEDIDO"]), stsuperpag, connSuperPag, tranSuperPag);

                    if (paymentFormId == 6 || paymentFormId == 9)
                    {
                        Guid paymentAttemptIdBoleto = Guid.NewGuid();
                        InserePaymentAttempt(paymentAttemptIdBoleto, 1 /*utiliza um no. qualquer*/, paymentFormId, VerificaDBNull(oDR["NUM_VALOR_PARCELA_PEDIDO"]), orderId, oDR["DAT_ATUALIZACAO"], oDR["DAT_ATUALIZACAO"], stsuperpag, 0, oDR["INT_NUMERO_PARCELA_MP"], connSuperPag, tranSuperPag);
                        InserePaymentAttemptBoleto(paymentAttemptIdBoleto, "", "", "", "", "", "", DBNull.Value, DBNull.Value, oDR["STR_INSTRUCOES_BB_CAIXA"], oDR["STR_NUMERO_OCT"], oDR["DAT_ATUALIZACAO"], oDR["DAT_PARCELA_MP"], connSuperPag, tranSuperPag);
                    }
                    else
                    {
                        dATUALIZACAO = (DateTime)oDR["DAT_ATUALIZACAO"];

                        switch (paymentFormId)
                        {
                            case 2:
                            case 4:
                                oNUMAUTOR = oDR["STR_SAFENET_NUMAUTOR"];
                                oNUMCV = oDR["STR_SAFENET_NUMCV"];
                                oNUMAUTENT = oDR["STR_SAFENET_NUMAUTENT"];
                                break;
                            case 5:
                                byte b = 0;
                                int i = 0;
                                oTIPO_PAGAMENTO = oDR["STR_TIPO_PAGAMENTO"];
                                if(!byte.TryParse(oTIPO_PAGAMENTO.ToString(), out b))
                                    oTIPO_PAGAMENTO = 0;
                                oCONVENIO = oDR["STR_CONVENIO"];
                                if (!int.TryParse(oCONVENIO.ToString(), out i))
                                    oCONVENIO = 0;
                                break;
                            case 7:
                                oTIPO_SHOPLINE = oDR["INT_TIPO_SHOPLINE"];
                                oMENSAGEM = oDR["STR_MENSAGEM"];
                                break;
                            case 1:
                            case 8:
                                oVN_ID_PEDIDO = oDR["STR_VN_ID_PEDIDO"];
                                oVN_Cod_Autoriza = oDR["STR_VN_Cod_Autoriza"];
                                int res;
                                if (!int.TryParse((string)oVN_Cod_Autoriza, out res))
                                    oVN_Cod_Autoriza = DBNull.Value;
                                break;
                            case 17:
                                oNUM_TIPO_PAGAMENTO = oDR["NUM_TIPO_PAGAMENTO"];
                                oNUM_PRAZO = oDR["NUM_PRAZO"];
                                oNUM_VALOR_PARCELA_PEDIDO = oDR["NUM_VALOR_PARCELA_PEDIDO"];
                                oSTR_CCNAME = oDR["STR_CCNAME"];
                                oSTR_CCEMAIL = oDR["STR_CCEMAIL"];
                                oSTR_CCTYPE = oDR["STR_CCTYPE"];
                                oSTR_ASSINATURA = oDR["STR_ASSINATURA"];
                                break;
                        }
                    }
                }


                Guid paymentAttemptId = Guid.NewGuid();
                if (paymentFormId != 6 && paymentFormId != 9)
                    InserePaymentAttempt(paymentAttemptId, 1 /*utiliza um no. qualquer*/, paymentFormId, numValorPedido, orderId, (dATUALIZACAO == DateTime.MinValue ? new DateTime(9999, 12, 31) : dATUALIZACAO), (dATUALIZACAO == DateTime.MinValue ? new DateTime(9999, 12, 31) : dATUALIZACAO), stsuperpag, 0, DBNull.Value, connSuperPag, tranSuperPag);
                
                switch (paymentFormId)
                {
                    case 2:
                        InserePaymentAttemptKomerci(paymentAttemptId, "", "MASTERCARD", "", DBNull.Value, DBNull.Value, DBNull.Value, oNUMAUTOR, oNUMCV, oNUMAUTENT, DBNull.Value, DBNull.Value, DBNull.Value, 0, connSuperPag, tranSuperPag);
                        break;
                    case 4:
                        InserePaymentAttemptKomerci(paymentAttemptId, "", "DINERS", "", DBNull.Value, DBNull.Value, DBNull.Value, oNUMAUTOR, oNUMCV, oNUMAUTENT, DBNull.Value, DBNull.Value, DBNull.Value, 0, connSuperPag, tranSuperPag);
                        break;
                    case 5:
                        InserePaymentAttemptBB(paymentAttemptId, numValorPedido, oCONVENIO, oTIPO_PAGAMENTO, dATUALIZACAO, DBNull.Value, DBNull.Value, 2, 0, DBNull.Value, 0, connSuperPag, tranSuperPag);
                        break;
                    case 7:
                        InserePaymentAttemptItauShopline(paymentAttemptId, "", numValorPedido, "", DBNull.Value, oTIPO_SHOPLINE, DBNull.Value, dATUALIZACAO, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, 2, 0, oMENSAGEM, 0, connSuperPag, tranSuperPag);
                        break;
                    case 1:
                    case 8:
                        InserePaymentAttemptVBV(paymentAttemptId, DBNull.Value, oVN_ID_PEDIDO, DBNull.Value, oVN_Cod_Autoriza, DBNull.Value, DBNull.Value, numValorPedido, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, 0, connSuperPag, tranSuperPag);
                        break;
                    case 17:
                        InserePaymentAttemptPagFacilBradesco(paymentAttemptId, DBNull.Value, DBNull.Value, oNUM_TIPO_PAGAMENTO, oNUM_PRAZO, DBNull.Value, oNUM_VALOR_PARCELA_PEDIDO, numValorPedido, DBNull.Value, oSTR_CCNAME, oSTR_CCEMAIL, oSTR_CCTYPE, oSTR_ASSINATURA, 0, connSuperPag, tranSuperPag);
                        break;
                }

                return paymentFormId;
            }
            finally
            {
                if (oDR != null)
                    oDR.Close();
                if (cmdSmartPag != null)
                    cmdSmartPag.Dispose();
                if (connSmartPag != null)
                {
                    connSmartPag.Close();
                    connSmartPag.Dispose();
                }
            }
        }
        #endregion
        #region InsereOrderInstallment
        private void InsereOrderInstallment(long orderId, object installmentNumber, int paymentFormId, object installmentValue, int status, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmd = null;

            try
            {
                cmd = new SqlCommand("INSERT INTO OrderInstallment(orderId, installmentNumber, paymentFormId, installmentValue, interestPercentage, status) VALUES (@orderId, @installmentNumber, @paymentFormId, @installmentValue, @interestPercentage, @status)", conn, tran);

                SqlParameter parorderId = cmd.Parameters.Add("@orderId", SqlDbType.BigInt);
                SqlParameter parinstallmentNumber = cmd.Parameters.Add("@installmentNumber", SqlDbType.Int);
                SqlParameter parpaymentFormId = cmd.Parameters.Add("@paymentFormId", SqlDbType.Int);
                SqlParameter parinstallmentValue = cmd.Parameters.Add("@installmentValue", SqlDbType.Decimal);
                SqlParameter parinterestPercentage = cmd.Parameters.Add("@interestPercentage", SqlDbType.Decimal);
                SqlParameter parstatus = cmd.Parameters.Add("@status", SqlDbType.Int);

                parorderId.Value = orderId;
                parinstallmentNumber.Value = installmentNumber;
                parpaymentFormId.Value = paymentFormId;
                parinstallmentValue.Value = installmentValue;
                parinterestPercentage.Value = DBNull.Value;
                parstatus.Value = status;
                
                cmd.ExecuteNonQuery();
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();
            }
        }
        #endregion
        #region InsereOrderItem
        private void InsereOrderItem(long orderId, decimal intNumeroPedido, decimal frete, SqlConnection connSuperPag, SqlTransaction tranSuperPag)
        {
            SqlConnection connSmartPag = null;
            SqlCommand cmdSmartPag = null, cmdSuperPag = null;
            SqlDataReader oDR = null;

            try
            {
                cmdSuperPag = new SqlCommand("INSERT INTO OrderItem(orderId, itemType, itemNumber, itemCode, itemDescription, itemQuantity, itemValue) VALUES(@orderId, @itemType, @itemNumber, @itemCode, @itemDescription, @itemQuantity, @itemValue)", connSuperPag, tranSuperPag);

                SqlParameter parorderId = cmdSuperPag.Parameters.Add("@orderId", SqlDbType.BigInt);
                SqlParameter paritemType = cmdSuperPag.Parameters.Add("@itemType", SqlDbType.Int);
                SqlParameter paritemNumber = cmdSuperPag.Parameters.Add("@itemNumber", SqlDbType.Int);
                SqlParameter paritemCode = cmdSuperPag.Parameters.Add("@itemCode", SqlDbType.VarChar, 50);
                SqlParameter paritemDescription = cmdSuperPag.Parameters.Add("@itemDescription", SqlDbType.VarChar, 200);
                SqlParameter paritemQuantity = cmdSuperPag.Parameters.Add("@itemQuantity", SqlDbType.Int);
                SqlParameter paritemValue = cmdSuperPag.Parameters.Add("@itemValue", SqlDbType.Decimal);

                connSmartPag = new SqlConnection(strConnSmartPag);
                connSmartPag.Open();

                cmdSmartPag = new SqlCommand("select * from pedido_produto where int_numero_pedido = " + intNumeroPedido, connSmartPag);
                oDR = cmdSmartPag.ExecuteReader(System.Data.CommandBehavior.Default);

                while (oDR.Read())
                {
                    parorderId.Value = orderId;
                    paritemType.Value = 1;
                    paritemNumber.Value = VerificaDBNull(oDR["INT_SEQUENCIAL_PROD"]);
                    paritemCode.Value = VerificaDBNull(oDR["STR_CODIGO_PROD"]);
                    paritemDescription.Value = VerificaDBNull(oDR["STR_DESCRICAO_PROD"]);
                    paritemQuantity.Value = VerificaDBNull(oDR["INT_QUANTIDADE_PROD"]);
                    paritemValue.Value = VerificaDBNull(oDR["NUM_VALOR_UNITARIO_PROD"]);
                    
                    cmdSuperPag.ExecuteNonQuery();
                }

                if(frete > 0)
                {
                    parorderId.Value = orderId;
                    paritemType.Value = 2; //frete
                    paritemNumber.Value = 0;
                    paritemCode.Value = "";
                    paritemDescription.Value = "";
                    paritemQuantity.Value = 1;
                    paritemValue.Value = frete;

                    cmdSuperPag.ExecuteNonQuery();
                }
            }
            finally
            {
                if (oDR != null)
                    oDR.Close();
                if (cmdSmartPag != null)
                    cmdSmartPag.Dispose();
                if (cmdSuperPag != null)
                    cmdSuperPag.Dispose();
                if (connSmartPag != null)
                {
                    connSmartPag.Close();
                    connSmartPag.Dispose();
                }
            }
        }
        #endregion
        #region InserePaymentAttempt
        private void InserePaymentAttempt(Guid paymentAttemptId, object paymentAgentSetupId, object paymentFormId, object price, object orderId, object startTime, object lastUpdate, object status, object step, object installmentNumber, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmd = null;

            try
            {
                cmd = new SqlCommand("INSERT INTO PaymentAttempt(paymentAttemptId, paymentAgentSetupId, paymentFormId, price, orderId, startTime, lastUpdate, status, step, installmentNumber, returnMessage, billingScheduleId, isSimulation) VALUES(@paymentAttemptId, @paymentAgentSetupId, @paymentFormId, @price, @orderId, @startTime, @lastUpdate, @status, @step, @installmentNumber, @returnMessage, @billingScheduleId, @isSimulation)", conn, tran);

                SqlParameter parpaymentAttemptId = cmd.Parameters.Add("@paymentAttemptId", SqlDbType.UniqueIdentifier);
                SqlParameter parpaymentAgentSetupId = cmd.Parameters.Add("@paymentAgentSetupId", SqlDbType.Int);
                SqlParameter parpaymentFormId = cmd.Parameters.Add("@paymentFormId", SqlDbType.Int);
                SqlParameter parprice = cmd.Parameters.Add("@price", SqlDbType.Decimal);
                SqlParameter parorderId = cmd.Parameters.Add("@orderId", SqlDbType.BigInt);
                SqlParameter parstartTime = cmd.Parameters.Add("@startTime", SqlDbType.DateTime);
                SqlParameter parlastUpdate = cmd.Parameters.Add("@lastUpdate", SqlDbType.DateTime);
                SqlParameter parstatus = cmd.Parameters.Add("@status", SqlDbType.Int);
                SqlParameter parstep = cmd.Parameters.Add("@step", SqlDbType.Int);
                SqlParameter parinstallmentNumber = cmd.Parameters.Add("@installmentNumber", SqlDbType.Int);
                SqlParameter parreturnMessage = cmd.Parameters.Add("@returnMessage", SqlDbType.VarChar, 200);
                SqlParameter parbillingScheduleId = cmd.Parameters.Add("@billingScheduleId", SqlDbType.Int);
                SqlParameter parisSimulation = cmd.Parameters.Add("@isSimulation", SqlDbType.Bit);

                parpaymentAttemptId.Value = paymentAttemptId;
                parpaymentAgentSetupId.Value = VerificaDBNull(paymentAgentSetupId);
                parpaymentFormId.Value = VerificaDBNull(paymentFormId);
                parprice.Value = VerificaDBNull(price);
                parorderId.Value = VerificaDBNull(orderId);
                parstartTime.Value = VerificaDBNull(startTime);
                parlastUpdate.Value = VerificaDBNull(lastUpdate);
                parstatus.Value = VerificaDBNull(status);
                parstep.Value = VerificaDBNull(step);
                parinstallmentNumber.Value = VerificaDBNull(installmentNumber);
                parreturnMessage.Value = DBNull.Value;
                parbillingScheduleId.Value = DBNull.Value;
                parisSimulation.Value = 0;
                
                cmd.ExecuteNonQuery();
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();
            }
        }
        #endregion
        #region InserePaymentAttemptBB
        private void InserePaymentAttemptBB(Guid paymentAttemptId, object valor, object idConvenio, object tipoPagamento, object dataPagamento, object situacao, object dataSonda, object qtdSonda, object sondaOffline, object msgret, object bbpagStatus, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmd = null;

            try
            {
                cmd = new SqlCommand("INSERT INTO PaymentAttemptBB(paymentAttemptId, valor, idConvenio, tipoPagamento, dataPagamento, situacao, dataSonda, qtdSonda, sondaOffline, msgret, bbpagStatus) VALUES(@paymentAttemptId, @valor, @idConvenio, @tipoPagamento, @dataPagamento, @situacao, @dataSonda, @qtdSonda, @sondaOffline, @msgret, @bbpagStatus)", conn, tran);

                SqlParameter parpaymentAttemptId = cmd.Parameters.Add("@paymentAttemptId", SqlDbType.UniqueIdentifier);
                SqlParameter parvalor = cmd.Parameters.Add("@valor", SqlDbType.Decimal);
                SqlParameter paridConvenio = cmd.Parameters.Add("@idConvenio", SqlDbType.Int);
                SqlParameter partipoPagamento = cmd.Parameters.Add("@tipoPagamento", SqlDbType.TinyInt);
                SqlParameter pardataPagamento = cmd.Parameters.Add("@dataPagamento", SqlDbType.DateTime);
                SqlParameter parsituacao = cmd.Parameters.Add("@situacao", SqlDbType.Char, 2);
                SqlParameter pardataSonda = cmd.Parameters.Add("@dataSonda", SqlDbType.DateTime);
                SqlParameter parqtdSonda = cmd.Parameters.Add("@qtdSonda", SqlDbType.SmallInt);
                SqlParameter parsondaOffline = cmd.Parameters.Add("@sondaOffline", SqlDbType.Bit);
                SqlParameter parmsgret = cmd.Parameters.Add("@msgret", SqlDbType.VarChar, 200);
                SqlParameter parbbpagStatus = cmd.Parameters.Add("@bbpagStatus", SqlDbType.TinyInt);

                parpaymentAttemptId.Value = paymentAttemptId;
                parvalor.Value = VerificaDBNull(valor);
                paridConvenio.Value = VerificaDBNull(idConvenio);
                partipoPagamento.Value = VerificaDBNull(tipoPagamento);
                pardataPagamento.Value = VerificaDBNull(dataPagamento);
                parsituacao.Value = VerificaDBNull(situacao);
                pardataSonda.Value = VerificaDBNull(dataSonda);
                parqtdSonda.Value = VerificaDBNull(qtdSonda);
                parsondaOffline.Value = VerificaDBNull(sondaOffline);
                parmsgret.Value = VerificaDBNull(msgret);
                parbbpagStatus.Value = VerificaDBNull(bbpagStatus);
                
                cmd.ExecuteNonQuery();
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();
            }
        }
        #endregion
        #region InserePaymentAttemptBoleto
        private void InserePaymentAttemptBoleto(Guid paymentAttemptId, object documentNumber, object withdraw, object withdrawDoc, object address1, object address2, object address3, object oct, object barCode, object instructions, object ourNumber, object paymentDate, object expirationPaymentDate, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmd = null;

            try
            {
                cmd = new SqlCommand("INSERT INTO PaymentAttemptBoleto(paymentAttemptId, documentNumber, withdraw, withdrawDoc, address1, address2, address3, oct, barCode, instructions, ourNumber, paymentDate, expirationPaymentDate) VALUES(@paymentAttemptId, @documentNumber, @withdraw, @withdrawDoc, @address1, @address2, @address3, @oct, @barCode, @instructions, @ourNumber, @paymentDate, @expirationPaymentDate)", conn, tran);

                SqlParameter parpaymentAttemptId = cmd.Parameters.Add("@paymentAttemptId", SqlDbType.UniqueIdentifier);
                SqlParameter pardocumentNumber = cmd.Parameters.Add("@documentNumber", SqlDbType.VarChar, 50);
                SqlParameter parwithdraw = cmd.Parameters.Add("@withdraw", SqlDbType.VarChar, 50);
                SqlParameter parwithdrawDoc = cmd.Parameters.Add("@withdrawDoc", SqlDbType.VarChar, 14);
                SqlParameter paraddress1 = cmd.Parameters.Add("@address1", SqlDbType.VarChar, 200);
                SqlParameter paraddress2 = cmd.Parameters.Add("@address2", SqlDbType.VarChar, 200);
                SqlParameter paraddress3 = cmd.Parameters.Add("@address3", SqlDbType.VarChar, 200);
                SqlParameter paroct = cmd.Parameters.Add("@oct", SqlDbType.VarChar, 70);
                SqlParameter parbarCode = cmd.Parameters.Add("@barCode", SqlDbType.VarChar, 70);
                SqlParameter parourNumber = cmd.Parameters.Add("@ourNumber", SqlDbType.VarChar, 30);
                SqlParameter parinstructions = cmd.Parameters.Add("@instructions", SqlDbType.VarChar, 1000);
                SqlParameter parpaymentDate = cmd.Parameters.Add("@paymentDate", SqlDbType.DateTime);
                SqlParameter parexpirationPaymentDate = cmd.Parameters.Add("@expirationPaymentDate", SqlDbType.DateTime);
                SqlParameter parpaymentAttemptBoletoReturnId = cmd.Parameters.Add("@paymentAttemptBoletoReturnId", SqlDbType.Int);

                parpaymentAttemptId.Value = paymentAttemptId;
                pardocumentNumber.Value = VerificaDBNull(documentNumber);
                parwithdraw.Value = VerificaDBNull(withdraw);
                parwithdrawDoc.Value = VerificaDBNull(withdrawDoc);
                paraddress1.Value = VerificaDBNull(address1);
                paraddress2.Value = VerificaDBNull(address2);
                paraddress3.Value = VerificaDBNull(address3);
                paroct.Value = VerificaDBNull(oct);
                parbarCode.Value = VerificaDBNull(barCode);
                parinstructions.Value = VerificaDBNull(instructions);
                parourNumber.Value = VerificaDBNull(ourNumber);
                parpaymentDate.Value = VerificaDBNull(paymentDate);
                parexpirationPaymentDate.Value = VerificaDBNull(expirationPaymentDate);
                parpaymentAttemptBoletoReturnId.Value = 0;

                cmd.ExecuteNonQuery();
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();
            }
        }
        #endregion
        #region InserePaymentAttemptItauShopline
        private void InserePaymentAttemptItauShopline(Guid paymentAttemptId, object codEmp, object valor, object chave, object dc, object tipPag, object sitPag, object dtPag, object codAut, object numId, object compVend, object tipCart, object dataSonda, object qtdSonda, object sondaOffline, object msgret, object itauStatus, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmd = null;

            try
            {
                cmd = new SqlCommand("INSERT INTO PaymentAttemptItauShopline(paymentAttemptId, codEmp, valor, chave, dc, tipPag, sitPag, dtPag, codAut, numId, compVend, tipCart, dataSonda, qtdSonda, sondaOffline, msgret, itauStatus) VALUES(@paymentAttemptId, @codEmp, @valor, @chave, @dc, @tipPag, @sitPag, @dtPag, @codAut, @numId, @compVend, @tipCart, @dataSonda, @qtdSonda, @sondaOffline, @msgret, @itauStatus)", conn, tran);

                SqlParameter parpaymentAttemptId = cmd.Parameters.Add("@paymentAttemptId", SqlDbType.UniqueIdentifier);
                SqlParameter parcodEmp = cmd.Parameters.Add("@codEmp", SqlDbType.Char, 26);
                SqlParameter parvalor = cmd.Parameters.Add("@valor", SqlDbType.Char, 11);
                SqlParameter parchave = cmd.Parameters.Add("@chave", SqlDbType.Char, 16);
                SqlParameter pardc = cmd.Parameters.Add("@dc", SqlDbType.VarChar, 2000);
                SqlParameter partipPag = cmd.Parameters.Add("@tipPag", SqlDbType.Char, 2);
                SqlParameter parsitPag = cmd.Parameters.Add("@sitPag", SqlDbType.Char, 2);
                SqlParameter pardtPag = cmd.Parameters.Add("@dtPag", SqlDbType.Char, 8);
                SqlParameter parcodAut = cmd.Parameters.Add("@codAut", SqlDbType.Char, 6);
                SqlParameter parnumId = cmd.Parameters.Add("@numId", SqlDbType.Char, 40);
                SqlParameter parcompVend = cmd.Parameters.Add("@compVend", SqlDbType.Char, 9);
                SqlParameter partipCart = cmd.Parameters.Add("@tipCart", SqlDbType.Char, 1);
                SqlParameter pardataSonda = cmd.Parameters.Add("@dataSonda", SqlDbType.DateTime);
                SqlParameter parqtdSonda = cmd.Parameters.Add("@qtdSonda", SqlDbType.SmallInt);
                SqlParameter parsondaOffline = cmd.Parameters.Add("@sondaOffline", SqlDbType.Bit);
                SqlParameter parmsgret = cmd.Parameters.Add("@msgret", SqlDbType.VarChar, 200);
                SqlParameter paritauStatus = cmd.Parameters.Add("@itauStatus", SqlDbType.TinyInt);

                parpaymentAttemptId.Value = paymentAttemptId;
                parcodEmp.Value = VerificaDBNull(codEmp);
                parvalor.Value = VerificaDBNull(valor);
                parchave.Value = VerificaDBNull(chave);
                pardc.Value = VerificaDBNull(dc);
                partipPag.Value = VerificaDBNull(tipPag);
                parsitPag.Value = VerificaDBNull(sitPag);
                pardtPag.Value = VerificaDBNull(dtPag);
                parcodAut.Value = VerificaDBNull(codAut);
                parnumId.Value = VerificaDBNull(numId);
                parcompVend.Value = VerificaDBNull(compVend);
                partipCart.Value = VerificaDBNull(tipCart);
                pardataSonda.Value = VerificaDBNull(dataSonda);
                parqtdSonda.Value = VerificaDBNull(qtdSonda);
                parsondaOffline.Value = VerificaDBNull(sondaOffline);
                parmsgret.Value = VerificaDBNull(msgret);
                paritauStatus.Value = VerificaDBNull(itauStatus);
                
                cmd.ExecuteNonQuery();
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();
            }
        }
        #endregion
        #region InserePaymentAttemptKomerci
        private void InserePaymentAttemptKomerci(Guid paymentAttemptId, object transacao, object bandeira, object codver, object data, object nr_cartao, object origem_bin, object numautor, object numcv, object numautent, object numsqn, object codret, object msgret, object komerciStatus, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmd = null;

            try
            {
                cmd = new SqlCommand("INSERT INTO PaymentAttemptKomerci(paymentAttemptId, transacao, bandeira, codver, data, nr_cartao, origem_bin, numautor, numcv, numautent, numsqn, codret, msgret, avs, respavs, msgavs, komerciStatus) VALUES(@paymentAttemptId, @transacao, @bandeira, @codver, @data, @nr_cartao, @origem_bin, @numautor, @numcv, @numautent, @numsqn, @codret, @msgret, @avs, @respavs, @msgavs, @komerciStatus)", conn, tran);

                SqlParameter parpaymentAttemptId = cmd.Parameters.Add("@paymentAttemptId", SqlDbType.UniqueIdentifier);
                SqlParameter partransacao = cmd.Parameters.Add("@transacao", SqlDbType.Char, 2);
                SqlParameter parbandeira = cmd.Parameters.Add("@bandeira", SqlDbType.Char, 10);
                SqlParameter parcodver = cmd.Parameters.Add("@codver", SqlDbType.Char, 25);
                SqlParameter pardata = cmd.Parameters.Add("@data", SqlDbType.Char, 8);
                SqlParameter parnr_cartao = cmd.Parameters.Add("@nr_cartao", SqlDbType.Char, 16);
                SqlParameter parorigem_bin = cmd.Parameters.Add("@origem_bin", SqlDbType.Char, 3);
                SqlParameter parnumautor = cmd.Parameters.Add("@numautor", SqlDbType.Char, 6);
                SqlParameter parnumcv = cmd.Parameters.Add("@numcv", SqlDbType.Char, 9);
                SqlParameter parnumautent = cmd.Parameters.Add("@numautent", SqlDbType.Char, 27);
                SqlParameter parnumsqn = cmd.Parameters.Add("@numsqn", SqlDbType.Char, 12);
                SqlParameter parcodret = cmd.Parameters.Add("@codret", SqlDbType.Char, 2);
                SqlParameter parmsgret = cmd.Parameters.Add("@msgret", SqlDbType.VarChar, 200);
                SqlParameter paravs = cmd.Parameters.Add("@avs", SqlDbType.Char, 1);
                SqlParameter parrespavs = cmd.Parameters.Add("@respavs", SqlDbType.VarChar, 2);
                SqlParameter parmsgavs = cmd.Parameters.Add("@msgavs", SqlDbType.VarChar, 80);
                SqlParameter parkomerciStatus = cmd.Parameters.Add("@komerciStatus", SqlDbType.TinyInt);

                parpaymentAttemptId.Value = paymentAttemptId;
                partransacao.Value = VerificaDBNull(transacao);
                parbandeira.Value = VerificaDBNull(bandeira);
                parcodver.Value = VerificaDBNull(codver);
                pardata.Value = VerificaDBNull(data);
                parnr_cartao.Value = VerificaDBNull(nr_cartao);
                parorigem_bin.Value = VerificaDBNull(origem_bin);
                parnumautor.Value = VerificaDBNull(numautor);
                parnumcv.Value = VerificaDBNull(numcv);
                parnumautent.Value = VerificaDBNull(numautent);
                parnumsqn.Value = VerificaDBNull(numsqn);
                parcodret.Value = VerificaDBNull(codret);
                parmsgret.Value = VerificaDBNull(msgret);
                paravs.Value = DBNull.Value;
                parrespavs.Value = "";
                parmsgavs.Value = "";
                parkomerciStatus.Value = VerificaDBNull(komerciStatus);
                
                cmd.ExecuteNonQuery();
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();
            }
        }
        #endregion
        #region InserePaymentAttemptVBV
        private void InserePaymentAttemptVBV(Guid paymentAttemptId, object tidMaster, object tid, object lr, object arp, object ars, object vbvOrderId, object price, object free, object pan, object bank, object authent, object cap, object vbvStatus, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmd = null;

            try
            {
                cmd = new SqlCommand("INSERT INTO PaymentAttemptVBV(paymentAttemptId, tidMaster, tid, lr, arp, ars, vbvOrderId, price, [free], pan, bank, authent, cap, vbvStatus) VALUES (@paymentAttemptId, @tidMaster, @tid, @lr, @arp, @ars, @vbvOrderId, @price, @free, @pan, @bank, @authent, @cap, @vbvStatus)", conn, tran);

                SqlParameter parpaymentAttemptId = cmd.Parameters.Add("@paymentAttemptId", SqlDbType.UniqueIdentifier);
                SqlParameter partidMaster = cmd.Parameters.Add("@tidMaster", SqlDbType.VarChar, 30);
                SqlParameter partid = cmd.Parameters.Add("@tid", SqlDbType.VarChar, 30);
                SqlParameter parlr = cmd.Parameters.Add("@lr", SqlDbType.Decimal);
                SqlParameter pararp = cmd.Parameters.Add("@arp", SqlDbType.Int);
                SqlParameter parars = cmd.Parameters.Add("@ars", SqlDbType.VarChar, 128);
                SqlParameter parvbvOrderId = cmd.Parameters.Add("@vbvOrderId", SqlDbType.VarChar, 20);
                SqlParameter parprice = cmd.Parameters.Add("@price", SqlDbType.Int);
                SqlParameter parfree = cmd.Parameters.Add("@free", SqlDbType.VarChar, 128);
                SqlParameter parpan = cmd.Parameters.Add("@pan", SqlDbType.VarChar, 30);
                SqlParameter parbank = cmd.Parameters.Add("@bank", SqlDbType.Int);
                SqlParameter parauthent = cmd.Parameters.Add("@authent", SqlDbType.Int);
                SqlParameter parcap = cmd.Parameters.Add("@cap", SqlDbType.VarChar, 50);
                SqlParameter parvbvStatus = cmd.Parameters.Add("@vbvStatus", SqlDbType.TinyInt);

                parpaymentAttemptId.Value = paymentAttemptId;
                partidMaster.Value = VerificaDBNull(tidMaster);
                partid.Value = VerificaDBNull(tid);
                parlr.Value = VerificaDBNull(lr);
                pararp.Value = VerificaDBNull(arp);
                parars.Value = VerificaDBNull(ars);
                parvbvOrderId.Value = VerificaDBNull(vbvOrderId);
                parprice.Value = VerificaDBNull(price);
                parfree.Value = VerificaDBNull(free);
                parpan.Value = VerificaDBNull(pan);
                parbank.Value = VerificaDBNull(bank);
                parauthent.Value = VerificaDBNull(authent);
                parcap.Value = VerificaDBNull(cap);
                parvbvStatus.Value = VerificaDBNull(vbvStatus);

                cmd.ExecuteNonQuery();
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();
            }
        }
        #endregion
        #region InserePaymentAttemptPagFacilBradesco
        private void InserePaymentAttemptPagFacilBradesco(Guid paymentAttemptId, object numOrder, object merchantid, object tipoPagto, object prazo, object numparc, object valparc, object valtotal, object cod, object ccname, object ccemail, object cctype, object assinatura, object bradescoStatus, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmd = null;

            try
            {
                cmd = new SqlCommand("INSERT INTO PaymentAttemptBradesco(paymentAttemptId, numOrder, merchantid, tipoPagto, prazo, numparc, valparc, valtotal, cod, ccname, ccemail, cctype, assinatura, bradescoStatus) VALUES (@paymentAttemptId, @numOrder, @merchantid, @tipoPagto, @prazo, @numparc, @valparc, @valtotal, @cod, @ccname, @ccemail, @cctype, @assinatura, @bradescoStatus)", conn, tran);

                SqlParameter parpaymentAttemptId = cmd.Parameters.Add("@paymentAttemptId", SqlDbType.UniqueIdentifier);
                SqlParameter parnumOrder = cmd.Parameters.Add("@numOrder", SqlDbType.VarChar, 50);
                SqlParameter parmerchantid = cmd.Parameters.Add("@merchantid", SqlDbType.VarChar, 50);
                SqlParameter partipoPagto = cmd.Parameters.Add("@tipoPagto", SqlDbType.Int);
                SqlParameter parprazo = cmd.Parameters.Add("@prazo", SqlDbType.VarChar, 50);
                SqlParameter parnumparc = cmd.Parameters.Add("@numparc", SqlDbType.VarChar, 10);
                SqlParameter parvalparc = cmd.Parameters.Add("@valparc", SqlDbType.VarChar, 50);
                SqlParameter parvaltotal = cmd.Parameters.Add("@valtotal", SqlDbType.VarChar, 50);
                SqlParameter parcod = cmd.Parameters.Add("@cod", SqlDbType.VarChar, 50);
                SqlParameter parccname = cmd.Parameters.Add("@ccname", SqlDbType.VarChar, 50);
                SqlParameter parccemail = cmd.Parameters.Add("@ccemail", SqlDbType.VarChar, 50);
                SqlParameter parcctype = cmd.Parameters.Add("@cctype", SqlDbType.VarChar, 50);
                SqlParameter parassinatura = cmd.Parameters.Add("@assinatura", SqlDbType.VarChar, 300);
                SqlParameter parbradescoStatus = cmd.Parameters.Add("@bradescoStatus", SqlDbType.Int);

                parpaymentAttemptId.Value = paymentAttemptId;
                parnumOrder.Value = VerificaDBNull(numOrder);
                parmerchantid.Value = VerificaDBNull(merchantid);
                partipoPagto.Value = VerificaDBNull(tipoPagto);
                parprazo.Value = VerificaDBNull(prazo);
                parnumparc.Value = VerificaDBNull(numparc);
                parvalparc.Value = VerificaDBNull(valparc);
                parvaltotal.Value = VerificaDBNull(valtotal);
                parcod.Value = VerificaDBNull(cod);
                parccname.Value = VerificaDBNull(ccname);
                parccemail.Value = VerificaDBNull(ccemail);
                parcctype.Value = VerificaDBNull(cctype);
                parassinatura.Value = VerificaDBNull(assinatura);
                parbradescoStatus.Value = VerificaDBNull(bradescoStatus);

                cmd.ExecuteNonQuery();
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();
            }
        }
        #endregion
        #region InsereSPLEgacyDBImport
        private void InsereSPLEgacyDBImport(long orderId, decimal intNumeroPedido, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmd = null;

            try
            {
                cmd = new SqlCommand("INSERT INTO SPLegacyDBImport(INT_NUMERO_PEDIDO, orderId) VALUES(@INT_NUMERO_PEDIDO, @orderId)", conn, tran);

                SqlParameter parINT_NUMERO_PEDIDO = cmd.Parameters.Add("@INT_NUMERO_PEDIDO", SqlDbType.Decimal);
                SqlParameter parorderId = cmd.Parameters.Add("@orderId", SqlDbType.BigInt);

                parINT_NUMERO_PEDIDO.Value = intNumeroPedido;
                parorderId.Value = orderId;

                cmd.ExecuteNonQuery();
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();
            }
        }
        #endregion

        #region VerificaDBNull
        private object VerificaDBNull(object obj)
        {
            if (Convert.IsDBNull(obj))
                return DBNull.Value;
            else
            {
                if (obj != null && obj.GetType().Equals(typeof(string)))
                    obj = ((string)obj).Trim();

                return obj;
            }
        }
        #endregion
    }
}
