namespace LegacyImporter
{
    partial class frmPrincipal
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.cmdImportar = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.lblId = new System.Windows.Forms.Label();
            this.lblCurrent = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblErros = new System.Windows.Forms.Label();
            this.lblSucesso = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lblJaImportado = new System.Windows.Forms.Label();
            this.linkVer = new System.Windows.Forms.LinkLabel();
            this.cmdCancelar = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cmdImportar
            // 
            this.cmdImportar.Location = new System.Drawing.Point(225, 34);
            this.cmdImportar.Name = "cmdImportar";
            this.cmdImportar.Size = new System.Drawing.Size(75, 23);
            this.cmdImportar.TabIndex = 0;
            this.cmdImportar.Text = "Importar";
            this.cmdImportar.UseVisualStyleBackColor = true;
            this.cmdImportar.Click += new System.EventHandler(this.cmdImportar_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Status:";
            // 
            // lblId
            // 
            this.lblId.Location = new System.Drawing.Point(73, 39);
            this.lblId.Name = "lblId";
            this.lblId.Size = new System.Drawing.Size(77, 13);
            this.lblId.TabIndex = 2;
            // 
            // lblCurrent
            // 
            this.lblCurrent.Location = new System.Drawing.Point(156, 39);
            this.lblCurrent.Name = "lblCurrent";
            this.lblCurrent.Size = new System.Drawing.Size(52, 13);
            this.lblCurrent.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 62);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Erro:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 75);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(51, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Sucesso:";
            // 
            // lblErros
            // 
            this.lblErros.Location = new System.Drawing.Point(89, 62);
            this.lblErros.Name = "lblErros";
            this.lblErros.Size = new System.Drawing.Size(61, 13);
            this.lblErros.TabIndex = 6;
            this.lblErros.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblSucesso
            // 
            this.lblSucesso.Location = new System.Drawing.Point(89, 75);
            this.lblSucesso.Name = "lblSucesso";
            this.lblSucesso.Size = new System.Drawing.Size(61, 13);
            this.lblSucesso.TabIndex = 7;
            this.lblSucesso.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Location = new System.Drawing.Point(12, 9);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(60, 13);
            this.lblTitle.TabIndex = 8;
            this.lblTitle.Text = "Importando";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 88);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(71, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Já Importado:";
            // 
            // lblJaImportado
            // 
            this.lblJaImportado.Location = new System.Drawing.Point(89, 88);
            this.lblJaImportado.Name = "lblJaImportado";
            this.lblJaImportado.Size = new System.Drawing.Size(61, 13);
            this.lblJaImportado.TabIndex = 10;
            this.lblJaImportado.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // linkVer
            // 
            this.linkVer.AutoSize = true;
            this.linkVer.Location = new System.Drawing.Point(156, 62);
            this.linkVer.Name = "linkVer";
            this.linkVer.Size = new System.Drawing.Size(22, 13);
            this.linkVer.TabIndex = 11;
            this.linkVer.TabStop = true;
            this.linkVer.Text = "ver";
            this.linkVer.Visible = false;
            this.linkVer.Click += new System.EventHandler(this.linkVer_Click);
            // 
            // cmdCancelar
            // 
            this.cmdCancelar.Location = new System.Drawing.Point(225, 65);
            this.cmdCancelar.Name = "cmdCancelar";
            this.cmdCancelar.Size = new System.Drawing.Size(75, 23);
            this.cmdCancelar.TabIndex = 12;
            this.cmdCancelar.Text = "Cancelar";
            this.cmdCancelar.UseVisualStyleBackColor = true;
            this.cmdCancelar.Click += new System.EventHandler(this.cmdCancelar_Click);
            // 
            // frmPrincipal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(316, 120);
            this.Controls.Add(this.cmdCancelar);
            this.Controls.Add(this.linkVer);
            this.Controls.Add(this.lblJaImportado);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.lblSucesso);
            this.Controls.Add(this.lblErros);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblCurrent);
            this.Controls.Add(this.lblId);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmdImportar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "frmPrincipal";
            this.Text = "Importador SmartPag -> SuperPag";
            this.Load += new System.EventHandler(this.frmPrincipal_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cmdImportar;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblId;
        private System.Windows.Forms.Label lblCurrent;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblErros;
        private System.Windows.Forms.Label lblSucesso;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblJaImportado;
        private System.Windows.Forms.LinkLabel linkVer;
        private System.Windows.Forms.Button cmdCancelar;
    }
}

