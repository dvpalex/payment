using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace LegacyImporter
{
    public partial class frmErro : Form
    {
        public frmErro()
        {
            InitializeComponent();
        }

        private void frmErro_Load(object sender, EventArgs e)
        {
            long i = 1;
            frmPrincipal f = (frmPrincipal)this.Owner;
            foreach (string s in f.erros)
            {
                textBox1.Text += i++ + " - " + s + System.Environment.NewLine;
            }
        }
    }
}