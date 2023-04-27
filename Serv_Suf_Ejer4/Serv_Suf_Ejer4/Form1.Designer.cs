namespace Serv_Suf_Ejer4
{
    partial class Procesos
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Procesos));
            this.tbAmpliado = new System.Windows.Forms.TextBox();
            this.tbLinea = new System.Windows.Forms.TextBox();
            this.btnViewProc = new System.Windows.Forms.Button();
            this.btnProcessInfo = new System.Windows.Forms.Button();
            this.btnCloseProces = new System.Windows.Forms.Button();
            this.btnKillProces = new System.Windows.Forms.Button();
            this.btnRunApp = new System.Windows.Forms.Button();
            this.btnStartWith = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // tbAmpliado
            // 
            this.tbAmpliado.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbAmpliado.Location = new System.Drawing.Point(12, 12);
            this.tbAmpliado.Multiline = true;
            this.tbAmpliado.Name = "tbAmpliado";
            this.tbAmpliado.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbAmpliado.Size = new System.Drawing.Size(359, 426);
            this.tbAmpliado.TabIndex = 0;
            // 
            // tbLinea
            // 
            this.tbLinea.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbLinea.Location = new System.Drawing.Point(377, 117);
            this.tbLinea.Name = "tbLinea";
            this.tbLinea.Size = new System.Drawing.Size(218, 18);
            this.tbLinea.TabIndex = 1;
            // 
            // btnViewProc
            // 
            this.btnViewProc.Location = new System.Drawing.Point(377, 143);
            this.btnViewProc.Name = "btnViewProc";
            this.btnViewProc.Size = new System.Drawing.Size(218, 36);
            this.btnViewProc.TabIndex = 2;
            this.btnViewProc.Text = "View Processes";
            this.btnViewProc.UseVisualStyleBackColor = true;
            this.btnViewProc.Click += new System.EventHandler(this.btnViewProc_Click);
            // 
            // btnProcessInfo
            // 
            this.btnProcessInfo.Location = new System.Drawing.Point(377, 185);
            this.btnProcessInfo.Name = "btnProcessInfo";
            this.btnProcessInfo.Size = new System.Drawing.Size(218, 35);
            this.btnProcessInfo.TabIndex = 3;
            this.btnProcessInfo.Text = "Process info";
            this.btnProcessInfo.UseVisualStyleBackColor = true;
            this.btnProcessInfo.Click += new System.EventHandler(this.btnProcessInfo_Click);
            // 
            // btnCloseProces
            // 
            this.btnCloseProces.Location = new System.Drawing.Point(377, 226);
            this.btnCloseProces.Name = "btnCloseProces";
            this.btnCloseProces.Size = new System.Drawing.Size(218, 33);
            this.btnCloseProces.TabIndex = 4;
            this.btnCloseProces.Text = "Close process";
            this.btnCloseProces.UseVisualStyleBackColor = true;
            this.btnCloseProces.Click += new System.EventHandler(this.closeORkill);
            // 
            // btnKillProces
            // 
            this.btnKillProces.Location = new System.Drawing.Point(377, 265);
            this.btnKillProces.Name = "btnKillProces";
            this.btnKillProces.Size = new System.Drawing.Size(218, 35);
            this.btnKillProces.TabIndex = 5;
            this.btnKillProces.Text = "Kill process";
            this.btnKillProces.UseVisualStyleBackColor = true;
            this.btnKillProces.Click += new System.EventHandler(this.closeORkill);
            // 
            // btnRunApp
            // 
            this.btnRunApp.Location = new System.Drawing.Point(377, 306);
            this.btnRunApp.Name = "btnRunApp";
            this.btnRunApp.Size = new System.Drawing.Size(218, 33);
            this.btnRunApp.TabIndex = 6;
            this.btnRunApp.Text = "Run App";
            this.btnRunApp.UseVisualStyleBackColor = true;
            this.btnRunApp.Click += new System.EventHandler(this.btnRunApp_Click);
            // 
            // btnStartWith
            // 
            this.btnStartWith.Location = new System.Drawing.Point(377, 345);
            this.btnStartWith.Name = "btnStartWith";
            this.btnStartWith.Size = new System.Drawing.Size(218, 33);
            this.btnStartWith.TabIndex = 7;
            this.btnStartWith.Text = "Stars With";
            this.btnStartWith.UseVisualStyleBackColor = true;
            this.btnStartWith.Click += new System.EventHandler(this.btnStartWith_Click);
            // 
            // Procesos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(607, 450);
            this.Controls.Add(this.btnStartWith);
            this.Controls.Add(this.btnRunApp);
            this.Controls.Add(this.btnKillProces);
            this.Controls.Add(this.btnCloseProces);
            this.Controls.Add(this.btnProcessInfo);
            this.Controls.Add(this.btnViewProc);
            this.Controls.Add(this.tbLinea);
            this.Controls.Add(this.tbAmpliado);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Procesos";
            this.Text = "Procesos";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbAmpliado;
        private System.Windows.Forms.TextBox tbLinea;
        private System.Windows.Forms.Button btnViewProc;
        private System.Windows.Forms.Button btnProcessInfo;
        private System.Windows.Forms.Button btnCloseProces;
        private System.Windows.Forms.Button btnKillProces;
        private System.Windows.Forms.Button btnRunApp;
        private System.Windows.Forms.Button btnStartWith;
    }
}

