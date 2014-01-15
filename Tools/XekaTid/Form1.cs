using System;
using System.Configuration;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Windows.Forms;
using SuperPag.Agents.VBV;
using SuperPag.Data;
using SuperPag.Data.Messages;
using SuperPag.Agents.VBV.Messages;

namespace XekaTid
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                int storeId, paymentFormId;

                //Validacao dos dados de entrada
                if (!int.TryParse(txtStoreId.Text, out storeId))
                    MessageBox.Show("Por favor, digite um StoreId válido");

                if (!int.TryParse(txtPaymentFormId.Text, out paymentFormId))
                    MessageBox.Show("Por favor, digite um PaymentFormId válido");

                DStorePaymentForm storePaymentForm = DataFactory.StorePaymentForm().Locate(storeId, paymentFormId);
                if (storePaymentForm == null)
                    MessageBox.Show("Meio de Pagamento e/ou Lojista nao encontrado");

                VBV vbv = new VBV();
                VBVInquireReturn inquireReturn = vbv.Inquire(txtTid.Text.Trim(), "cfg" + storePaymentForm.paymentAgentSetupId);

                StringBuilder response = new StringBuilder();
                response.Append("LR: ");
                response.Append(inquireReturn.lr);
                response.AppendLine();
                response.Append("Mensagem: ");
                response.Append(inquireReturn.ars);
                response.AppendLine();
                response.Append("Autorizacao: ");
                response.Append(inquireReturn.arp);
                response.AppendLine();
                response.Append("Valor: ");
                response.Append(inquireReturn.price);
                response.AppendLine();
                response.Append("Banco: ");
                response.Append(inquireReturn.bank);
                response.AppendLine();
                response.Append("Autenticacao: ");
                response.Append(inquireReturn.authent);
                response.AppendLine();
                txtResponse.Text = response.ToString();
            }
            catch (Exception ex)
            {
                SuperPag.Helper.GenericHelper.LogFile("XekaTid::Program.cs storeid=" + txtStoreId.Text + " paymentFormId=" + txtPaymentFormId.Text + " message=" + ex.Message, SuperPag.LogFileEntryType.Error);
            }
        }

    }
}