namespace SRegulV2
{
    partial class FMessage
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FMessage));
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.labelCaractere = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.bRappel = new System.Windows.Forms.Button();
            this.bPiecejointe = new System.Windows.Forms.Button();
            this.bBip = new System.Windows.Forms.Button();
            this.bEnvoi = new System.Windows.Forms.Button();
            this.bCancel = new System.Windows.Forms.Button();
            this.lPiecejointe = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "bondCancel.png");
            this.imageList1.Images.SetKeyName(1, "bEnvoi.png");
            this.imageList1.Images.SetKeyName(2, "bBip.png");
            this.imageList1.Images.SetKeyName(3, "bJoindre.png");
            this.imageList1.Images.SetKeyName(4, "bRappel.png");
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.ForeColor = System.Drawing.SystemColors.Window;
            this.textBox1.Location = new System.Drawing.Point(22, 33);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(420, 93);
            this.textBox1.TabIndex = 10;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            this.textBox1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox1_KeyPress);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.SystemColors.Window;
            this.label1.Location = new System.Drawing.Point(19, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(138, 16);
            this.label1.TabIndex = 11;
            this.label1.Text = "Envoi d\'un message...";
            // 
            // labelCaractere
            // 
            this.labelCaractere.AutoSize = true;
            this.labelCaractere.ForeColor = System.Drawing.SystemColors.Window;
            this.labelCaractere.Location = new System.Drawing.Point(317, 129);
            this.labelCaractere.Name = "labelCaractere";
            this.labelCaractere.Size = new System.Drawing.Size(126, 16);
            this.labelCaractere.TabIndex = 12;
            this.labelCaractere.Text = "0 caractères sur 250";
            // 
            // timer1
            // 
            this.timer1.Interval = 2000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // toolTip1
            // 
            this.toolTip1.IsBalloon = true;
            // 
            // bRappel
            // 
            this.bRappel.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.bRappel.FlatAppearance.BorderSize = 0;
            this.bRappel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bRappel.ImageIndex = 4;
            this.bRappel.ImageList = this.imageList1;
            this.bRappel.Location = new System.Drawing.Point(226, 197);
            this.bRappel.Name = "bRappel";
            this.bRappel.Size = new System.Drawing.Size(64, 64);
            this.bRappel.TabIndex = 17;
            this.toolTip1.SetToolTip(this.bRappel, "Téléphoner au médecin");
            this.bRappel.UseVisualStyleBackColor = true;
            this.bRappel.Click += new System.EventHandler(this.bRappel_Click);
            // 
            // bPiecejointe
            // 
            this.bPiecejointe.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.bPiecejointe.FlatAppearance.BorderSize = 0;
            this.bPiecejointe.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bPiecejointe.ImageIndex = 3;
            this.bPiecejointe.ImageList = this.imageList1;
            this.bPiecejointe.Location = new System.Drawing.Point(461, 50);
            this.bPiecejointe.Name = "bPiecejointe";
            this.bPiecejointe.Size = new System.Drawing.Size(64, 64);
            this.bPiecejointe.TabIndex = 15;
            this.toolTip1.SetToolTip(this.bPiecejointe, "Joindre un document");
            this.bPiecejointe.UseVisualStyleBackColor = true;
            this.bPiecejointe.Click += new System.EventHandler(this.bPiecejointe_Click);
            // 
            // bBip
            // 
            this.bBip.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.bBip.FlatAppearance.BorderSize = 0;
            this.bBip.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bBip.ImageIndex = 2;
            this.bBip.ImageList = this.imageList1;
            this.bBip.Location = new System.Drawing.Point(22, 197);
            this.bBip.Name = "bBip";
            this.bBip.Size = new System.Drawing.Size(64, 64);
            this.bBip.TabIndex = 14;
            this.toolTip1.SetToolTip(this.bBip, "Envoyer un bip de rappel");
            this.bBip.UseVisualStyleBackColor = true;
            this.bBip.Click += new System.EventHandler(this.bBip_Click);
            // 
            // bEnvoi
            // 
            this.bEnvoi.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.bEnvoi.FlatAppearance.BorderSize = 0;
            this.bEnvoi.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bEnvoi.ImageIndex = 1;
            this.bEnvoi.ImageList = this.imageList1;
            this.bEnvoi.Location = new System.Drawing.Point(123, 197);
            this.bEnvoi.Name = "bEnvoi";
            this.bEnvoi.Size = new System.Drawing.Size(64, 64);
            this.bEnvoi.TabIndex = 9;
            this.toolTip1.SetToolTip(this.bEnvoi, "Envoyer le message");
            this.bEnvoi.UseVisualStyleBackColor = true;
            this.bEnvoi.Click += new System.EventHandler(this.bEnvoi_Click);
            // 
            // bCancel
            // 
            this.bCancel.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.bCancel.FlatAppearance.BorderSize = 0;
            this.bCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bCancel.ImageIndex = 0;
            this.bCancel.ImageList = this.imageList1;
            this.bCancel.Location = new System.Drawing.Point(320, 197);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(64, 64);
            this.bCancel.TabIndex = 8;
            this.toolTip1.SetToolTip(this.bCancel, "Fermer");
            this.bCancel.UseVisualStyleBackColor = true;
            this.bCancel.Click += new System.EventHandler(this.bCancel_Click);
            // 
            // lPiecejointe
            // 
            this.lPiecejointe.ForeColor = System.Drawing.SystemColors.Window;
            this.lPiecejointe.Location = new System.Drawing.Point(24, 155);
            this.lPiecejointe.Name = "lPiecejointe";
            this.lPiecejointe.Size = new System.Drawing.Size(418, 21);
            this.lPiecejointe.TabIndex = 16;
            this.lPiecejointe.Text = "Piece jointe : ";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = global::SRegulV2.Properties.Resources.Smiley_Embete1;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.Location = new System.Drawing.Point(410, 197);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(115, 75);
            this.pictureBox1.TabIndex = 13;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Visible = false;
            // 
            // FMessage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.ClientSize = new System.Drawing.Size(537, 283);
            this.ControlBox = false;
            this.Controls.Add(this.bRappel);
            this.Controls.Add(this.lPiecejointe);
            this.Controls.Add(this.bPiecejointe);
            this.Controls.Add(this.bBip);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.labelCaractere);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.bEnvoi);
            this.Controls.Add(this.bCancel);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FMessage";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Envoi de message";
            this.Load += new System.EventHandler(this.FMessage_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Button bCancel;
        private System.Windows.Forms.Button bEnvoi;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelCaractere;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button bBip;
        private System.Windows.Forms.Button bPiecejointe;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label lPiecejointe;
        private System.Windows.Forms.Button bRappel;
    }
}