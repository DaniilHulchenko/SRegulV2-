
namespace SRegulV2
{
    partial class FMailMutuaide
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
#pragma warning disable CS0115 // 'FMailMutuaide.Dispose(bool)' : aucune méthode appropriée n'a été trouvée pour la substitution
        protected override void Dispose(bool disposing)
#pragma warning restore CS0115 // 'FMailMutuaide.Dispose(bool)' : aucune méthode appropriée n'a été trouvée pour la substitution
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FMailMutuaide));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tBoxMessage = new System.Windows.Forms.TextBox();
            this.labelSujet = new System.Windows.Forms.Label();
            this.labelDe = new System.Windows.Forms.Label();
            this.tBoxReponse = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.BEnvoi = new System.Windows.Forms.Button();
            this.BExit = new System.Windows.Forms.Button();
            this.labelEnvoiEnCours = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tBoxMessage);
            this.splitContainer1.Panel1.Controls.Add(this.labelSujet);
            this.splitContainer1.Panel1.Controls.Add(this.labelDe);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tBoxReponse);
            this.splitContainer1.Panel2.Controls.Add(this.label1);
            this.splitContainer1.Size = new System.Drawing.Size(906, 457);
            this.splitContainer1.SplitterDistance = 310;
            this.splitContainer1.TabIndex = 0;
            // 
            // tBoxMessage
            // 
            this.tBoxMessage.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.tBoxMessage.ForeColor = System.Drawing.SystemColors.Window;
            this.tBoxMessage.ImeMode = System.Windows.Forms.ImeMode.On;
            this.tBoxMessage.Location = new System.Drawing.Point(13, 59);
            this.tBoxMessage.Multiline = true;
            this.tBoxMessage.Name = "tBoxMessage";
            this.tBoxMessage.ReadOnly = true;
            this.tBoxMessage.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tBoxMessage.Size = new System.Drawing.Size(879, 243);
            this.tBoxMessage.TabIndex = 4;
            // 
            // labelSujet
            // 
            this.labelSujet.Location = new System.Drawing.Point(10, 37);
            this.labelSujet.Name = "labelSujet";
            this.labelSujet.Size = new System.Drawing.Size(873, 19);
            this.labelSujet.TabIndex = 3;
            this.labelSujet.Text = "Sujet :";
            // 
            // labelDe
            // 
            this.labelDe.Location = new System.Drawing.Point(10, 7);
            this.labelDe.Name = "labelDe";
            this.labelDe.Size = new System.Drawing.Size(873, 19);
            this.labelDe.TabIndex = 2;
            this.labelDe.Text = "De : ";
            // 
            // tBoxReponse
            // 
            this.tBoxReponse.Location = new System.Drawing.Point(18, 33);
            this.tBoxReponse.Multiline = true;
            this.tBoxReponse.Name = "tBoxReponse";
            this.tBoxReponse.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tBoxReponse.Size = new System.Drawing.Size(870, 90);
            this.tBoxReponse.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(121, 16);
            this.label1.TabIndex = 3;
            this.label1.Text = "Repondre à l\'email";
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "exit.png");
            this.imageList1.Images.SetKeyName(1, "bondCancel.png");
            this.imageList1.Images.SetKeyName(2, "bEnvoiBleu.png");
            // 
            // toolTip1
            // 
            this.toolTip1.AutomaticDelay = 250;
            this.toolTip1.AutoPopDelay = 3500;
            this.toolTip1.InitialDelay = 250;
            this.toolTip1.IsBalloon = true;
            this.toolTip1.ReshowDelay = 50;
            // 
            // BEnvoi
            // 
            this.BEnvoi.FlatAppearance.BorderSize = 0;
            this.BEnvoi.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BEnvoi.ImageIndex = 2;
            this.BEnvoi.ImageList = this.imageList1;
            this.BEnvoi.Location = new System.Drawing.Point(696, 463);
            this.BEnvoi.Name = "BEnvoi";
            this.BEnvoi.Size = new System.Drawing.Size(64, 64);
            this.BEnvoi.TabIndex = 10;
            this.toolTip1.SetToolTip(this.BEnvoi, "Envoyer la réponse");
            this.BEnvoi.UseVisualStyleBackColor = true;
            this.BEnvoi.Click += new System.EventHandler(this.BEnvoi_Click);
            // 
            // BExit
            // 
            this.BExit.FlatAppearance.BorderSize = 0;
            this.BExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BExit.ImageIndex = 1;
            this.BExit.ImageList = this.imageList1;
            this.BExit.Location = new System.Drawing.Point(785, 463);
            this.BExit.Name = "BExit";
            this.BExit.Size = new System.Drawing.Size(64, 64);
            this.BExit.TabIndex = 11;
            this.toolTip1.SetToolTip(this.BExit, "Fermer sans envoyer de réponse");
            this.BExit.UseVisualStyleBackColor = true;
            this.BExit.Click += new System.EventHandler(this.BExit_Click);
            // 
            // labelEnvoiEnCours
            // 
            this.labelEnvoiEnCours.AutoSize = true;
            this.labelEnvoiEnCours.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.labelEnvoiEnCours.Location = new System.Drawing.Point(367, 487);
            this.labelEnvoiEnCours.Name = "labelEnvoiEnCours";
            this.labelEnvoiEnCours.Size = new System.Drawing.Size(115, 16);
            this.labelEnvoiEnCours.TabIndex = 12;
            this.labelEnvoiEnCours.Text = "Envoie en cours....";
            // 
            // FMailMutuaide
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.ClientSize = new System.Drawing.Size(906, 533);
            this.ControlBox = false;
            this.Controls.Add(this.labelEnvoiEnCours);
            this.Controls.Add(this.BExit);
            this.Controls.Add(this.BEnvoi);
            this.Controls.Add(this.splitContainer1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.SystemColors.Window;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FMailMutuaide";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Arrivée d\'un nouvel email de Mutuaide";
            this.Load += new System.EventHandler(this.FMailMutuaide_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TextBox tBoxMessage;
        private System.Windows.Forms.Label labelSujet;
        private System.Windows.Forms.Label labelDe;
        private System.Windows.Forms.TextBox tBoxReponse;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button BEnvoi;
        private System.Windows.Forms.Button BExit;
        private System.Windows.Forms.Label labelEnvoiEnCours;
    }
}