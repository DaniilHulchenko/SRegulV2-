namespace SRegulV2
{
    partial class FParamUtilisateur
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FParamUtilisateur));
            this.panel4 = new System.Windows.Forms.Panel();
            this.rB3 = new System.Windows.Forms.RadioButton();
            this.rB2 = new System.Windows.Forms.RadioButton();
            this.rB1 = new System.Windows.Forms.RadioButton();
            this.bNotifs = new System.Windows.Forms.Button();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.label17 = new System.Windows.Forms.Label();
            this.cBoxMessage = new System.Windows.Forms.ComboBox();
            this.label16 = new System.Windows.Forms.Label();
            this.cBoxFinVisite = new System.Windows.Forms.ComboBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.bTestSon1 = new System.Windows.Forms.Button();
            this.bTestSon2 = new System.Windows.Forms.Button();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel4
            // 
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel4.Controls.Add(this.bTestSon2);
            this.panel4.Controls.Add(this.bTestSon1);
            this.panel4.Controls.Add(this.rB3);
            this.panel4.Controls.Add(this.rB2);
            this.panel4.Controls.Add(this.rB1);
            this.panel4.Controls.Add(this.bNotifs);
            this.panel4.Controls.Add(this.label17);
            this.panel4.Controls.Add(this.cBoxMessage);
            this.panel4.Controls.Add(this.label16);
            this.panel4.Controls.Add(this.cBoxFinVisite);
            this.panel4.Location = new System.Drawing.Point(12, 12);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(350, 406);
            this.panel4.TabIndex = 31;
            // 
            // rB3
            // 
            this.rB3.AutoSize = true;
            this.rB3.Location = new System.Drawing.Point(20, 240);
            this.rB3.Name = "rB3";
            this.rB3.Size = new System.Drawing.Size(107, 20);
            this.rB3.TabIndex = 105;
            this.rB3.TabStop = true;
            this.rB3.Text = "Carte satellite";
            this.rB3.UseVisualStyleBackColor = true;
            // 
            // rB2
            // 
            this.rB2.AutoSize = true;
            this.rB2.Location = new System.Drawing.Point(20, 214);
            this.rB2.Name = "rB2";
            this.rB2.Size = new System.Drawing.Size(179, 20);
            this.rB2.TabIndex = 104;
            this.rB2.TabStop = true;
            this.rB2.Text = "Carte avec le traffic routier";
            this.rB2.UseVisualStyleBackColor = true;
            // 
            // rB1
            // 
            this.rB1.AutoSize = true;
            this.rB1.Location = new System.Drawing.Point(20, 188);
            this.rB1.Name = "rB1";
            this.rB1.Size = new System.Drawing.Size(167, 20);
            this.rB1.TabIndex = 103;
            this.rB1.TabStop = true;
            this.rB1.Text = "Carte avec les n° de rue";
            this.rB1.UseVisualStyleBackColor = true;
            // 
            // bNotifs
            // 
            this.bNotifs.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.bNotifs.FlatAppearance.BorderSize = 0;
            this.bNotifs.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bNotifs.ImageIndex = 4;
            this.bNotifs.ImageList = this.imageList1;
            this.bNotifs.Location = new System.Drawing.Point(101, 301);
            this.bNotifs.Name = "bNotifs";
            this.bNotifs.Size = new System.Drawing.Size(64, 64);
            this.bNotifs.TabIndex = 102;
            this.bNotifs.UseVisualStyleBackColor = true;
            this.bNotifs.Click += new System.EventHandler(this.bNotifs_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "bajout.png");
            this.imageList1.Images.SetKeyName(1, "bajoutOff.png");
            this.imageList1.Images.SetKeyName(2, "boutonsModifier.png");
            this.imageList1.Images.SetKeyName(3, "boutonsModifierOff.png");
            this.imageList1.Images.SetKeyName(4, "brondValider.png");
            this.imageList1.Images.SetKeyName(5, "brondValiderOff.png");
            this.imageList1.Images.SetKeyName(6, "bSupprRouge.png");
            this.imageList1.Images.SetKeyName(7, "exit.png");
            this.imageList1.Images.SetKeyName(8, "bondCancel.png");
            this.imageList1.Images.SetKeyName(9, "bPasswordOn.png");
            this.imageList1.Images.SetKeyName(10, "bTest.png");
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label17.Location = new System.Drawing.Point(18, 105);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(153, 16);
            this.label17.TabIndex = 82;
            this.label17.Text = "Notification de message";
            // 
            // cBoxMessage
            // 
            this.cBoxMessage.BackColor = System.Drawing.SystemColors.ControlDark;
            this.cBoxMessage.ForeColor = System.Drawing.SystemColors.Window;
            this.cBoxMessage.FormattingEnabled = true;
            this.cBoxMessage.Items.AddRange(new object[] {
            "Clavecin",
            "come_and_get_your_love",
            "huawei_sms.wav",
            "iphone_new_sound",
            "iphone_sms_original",
            "Le_bon_la_brute_et_le_truand",
            "lg_timeless",
            "message_recu",
            "r2d2-sms",
            "reflection-iphone-x",
            "Samsung-S9-Skyline",
            "Si-tetais-la",
            "sms-airport",
            "Soupe_aux_choux",
            "viber-original-sms"});
            this.cBoxMessage.Location = new System.Drawing.Point(21, 124);
            this.cBoxMessage.Name = "cBoxMessage";
            this.cBoxMessage.Size = new System.Drawing.Size(226, 24);
            this.cBoxMessage.TabIndex = 81;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.Location = new System.Drawing.Point(17, 25);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(241, 16);
            this.label16.TabIndex = 80;
            this.label16.Text = "Notification Fin de visite/médecin dispo";
            // 
            // cBoxFinVisite
            // 
            this.cBoxFinVisite.BackColor = System.Drawing.SystemColors.ControlDark;
            this.cBoxFinVisite.ForeColor = System.Drawing.SystemColors.Window;
            this.cBoxFinVisite.FormattingEnabled = true;
            this.cBoxFinVisite.Items.AddRange(new object[] {
            "Clavecin",
            "come_and_get_your_love",
            "huawei_sms.wav",
            "iphone_new_sound",
            "iphone_sms_original",
            "Le_bon_la_brute_et_le_truand",
            "lg_timeless",
            "message_recu",
            "r2d2-sms",
            "reflection-iphone-x",
            "Samsung-S9-Skyline",
            "Si-tetais-la",
            "sms-airport",
            "Soupe_aux_choux",
            "viber-original-sms"});
            this.cBoxFinVisite.Location = new System.Drawing.Point(20, 44);
            this.cBoxFinVisite.Name = "cBoxFinVisite";
            this.cBoxFinVisite.Size = new System.Drawing.Size(226, 24);
            this.cBoxFinVisite.TabIndex = 0;
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // bTestSon1
            // 
            this.bTestSon1.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.bTestSon1.FlatAppearance.BorderSize = 0;
            this.bTestSon1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bTestSon1.ImageIndex = 10;
            this.bTestSon1.ImageList = this.imageList1;
            this.bTestSon1.Location = new System.Drawing.Point(264, 25);
            this.bTestSon1.Name = "bTestSon1";
            this.bTestSon1.Size = new System.Drawing.Size(64, 64);
            this.bTestSon1.TabIndex = 106;
            this.bTestSon1.UseVisualStyleBackColor = true;
            this.bTestSon1.Click += new System.EventHandler(this.bTestSon1_Click);
            // 
            // bTestSon2
            // 
            this.bTestSon2.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.bTestSon2.FlatAppearance.BorderSize = 0;
            this.bTestSon2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bTestSon2.ImageIndex = 10;
            this.bTestSon2.ImageList = this.imageList1;
            this.bTestSon2.Location = new System.Drawing.Point(264, 105);
            this.bTestSon2.Name = "bTestSon2";
            this.bTestSon2.Size = new System.Drawing.Size(64, 64);
            this.bTestSon2.TabIndex = 107;
            this.bTestSon2.UseVisualStyleBackColor = true;
            this.bTestSon2.Click += new System.EventHandler(this.bTestSon2_Click);
            // 
            // FParamUtilisateur
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.ClientSize = new System.Drawing.Size(374, 430);
            this.ControlBox = false;
            this.Controls.Add(this.panel4);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.SystemColors.Window;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FParamUtilisateur";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Paramètres de l\'utilisateur...";
            this.Load += new System.EventHandler(this.FParamUtilisateur_Load);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Button bNotifs;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.ComboBox cBoxMessage;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.ComboBox cBoxFinVisite;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.RadioButton rB2;
        private System.Windows.Forms.RadioButton rB1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.RadioButton rB3;
        private System.Windows.Forms.Button bTestSon2;
        private System.Windows.Forms.Button bTestSon1;
    }
}