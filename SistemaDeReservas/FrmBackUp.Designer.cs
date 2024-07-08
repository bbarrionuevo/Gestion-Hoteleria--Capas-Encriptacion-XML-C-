namespace SistemaDeReservas
{
    partial class FrmBackUp
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
            this.btnRealizarBackup = new System.Windows.Forms.Button();
            this.btnRestaurarBackup = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.listBox2 = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // btnRealizarBackup
            // 
            this.btnRealizarBackup.Location = new System.Drawing.Point(84, 45);
            this.btnRealizarBackup.Name = "btnRealizarBackup";
            this.btnRealizarBackup.Size = new System.Drawing.Size(100, 44);
            this.btnRealizarBackup.TabIndex = 0;
            this.btnRealizarBackup.Text = "Realizar  Backup";
            this.btnRealizarBackup.UseVisualStyleBackColor = true;
            this.btnRealizarBackup.Click += new System.EventHandler(this.btnRealizarBackup_Click_1);
            // 
            // btnRestaurarBackup
            // 
            this.btnRestaurarBackup.Location = new System.Drawing.Point(84, 273);
            this.btnRestaurarBackup.Name = "btnRestaurarBackup";
            this.btnRestaurarBackup.Size = new System.Drawing.Size(100, 44);
            this.btnRestaurarBackup.TabIndex = 1;
            this.btnRestaurarBackup.Text = "Restaurar  Backup";
            this.btnRestaurarBackup.UseVisualStyleBackColor = true;
            this.btnRestaurarBackup.Click += new System.EventHandler(this.btnRestaurarBackup_Click);
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(287, 45);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(514, 134);
            this.listBox1.TabIndex = 2;
            // 
            // listBox2
            // 
            this.listBox2.FormattingEnabled = true;
            this.listBox2.Location = new System.Drawing.Point(287, 273);
            this.listBox2.Name = "listBox2";
            this.listBox2.Size = new System.Drawing.Size(514, 134);
            this.listBox2.TabIndex = 3;
            // 
            // FrmBackUp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(962, 473);
            this.Controls.Add(this.listBox2);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.btnRestaurarBackup);
            this.Controls.Add(this.btnRealizarBackup);
            this.Name = "FrmBackUp";
            this.Text = "BackUp";
            this.Load += new System.EventHandler(this.FrmBackUp_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnRealizarBackup;
        private System.Windows.Forms.Button btnRestaurarBackup;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.ListBox listBox2;
    }
}