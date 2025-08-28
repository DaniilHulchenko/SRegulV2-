namespace SRegulV2
{
    partial class FTelUtils
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FTelUtils));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.listView1 = new System.Windows.Forms.ListView();
            this.tBoxTel = new System.Windows.Forms.TextBox();
            this.bAjouter = new System.Windows.Forms.Button();
            this.bSupprimer = new System.Windows.Forms.Button();
            this.bExit = new System.Windows.Forms.Button();
            this.bValider = new System.Windows.Forms.Button();
            this.tBoxLibelle = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.bCancel = new System.Windows.Forms.Button();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.cBoxCategorie = new System.Windows.Forms.ComboBox();
            this.Num = new System.Windows.Forms.Label();
            this.cBoxSScategorie = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.listView1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.splitContainer1.Panel2.Controls.Add(this.cBoxSScategorie);
            this.splitContainer1.Panel2.Controls.Add(this.label1);
            this.splitContainer1.Panel2.Controls.Add(this.Num);
            this.splitContainer1.Panel2.Controls.Add(this.cBoxCategorie);
            this.splitContainer1.Panel2.Controls.Add(this.tBoxTel);
            this.splitContainer1.Panel2.Controls.Add(this.bAjouter);
            this.splitContainer1.Panel2.Controls.Add(this.bSupprimer);
            this.splitContainer1.Panel2.Controls.Add(this.bExit);
            this.splitContainer1.Panel2.Controls.Add(this.bValider);
            this.splitContainer1.Panel2.Controls.Add(this.tBoxLibelle);
            this.splitContainer1.Panel2.Controls.Add(this.label5);
            this.splitContainer1.Panel2.Controls.Add(this.label4);
            this.splitContainer1.Panel2.Controls.Add(this.label3);
            this.splitContainer1.Panel2.Controls.Add(this.bCancel);
            this.splitContainer1.Size = new System.Drawing.Size(1319, 352);
            this.splitContainer1.SplitterDistance = 782;
            this.splitContainer1.TabIndex = 2;
            // 
            // listView1
            // 
            this.listView1.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listView1.ForeColor = System.Drawing.SystemColors.Window;
            this.listView1.FullRowSelect = true;
            this.listView1.Location = new System.Drawing.Point(0, 0);
            this.listView1.MultiSelect = false;
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(782, 352);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.List;
            this.listView1.DoubleClick += new System.EventHandler(this.listView1_DoubleClick);
            // 
            // tBoxTel
            // 
            this.tBoxTel.BackColor = System.Drawing.SystemColors.ControlDark;
            this.tBoxTel.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tBoxTel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tBoxTel.ForeColor = System.Drawing.SystemColors.Window;
            this.tBoxTel.Location = new System.Drawing.Point(106, 77);
            this.tBoxTel.Name = "tBoxTel";
            this.tBoxTel.Size = new System.Drawing.Size(154, 15);
            this.tBoxTel.TabIndex = 1;
            // 
            // bAjouter
            // 
            this.bAjouter.FlatAppearance.BorderSize = 0;
            this.bAjouter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bAjouter.ImageIndex = 0;
            this.bAjouter.ImageList = this.imageList1;
            this.bAjouter.Location = new System.Drawing.Point(179, 249);
            this.bAjouter.Name = "bAjouter";
            this.bAjouter.Size = new System.Drawing.Size(64, 64);
            this.bAjouter.TabIndex = 8;
            this.bAjouter.UseVisualStyleBackColor = true;
            this.bAjouter.Click += new System.EventHandler(this.bAjouter_Click);
            // 
            // bSupprimer
            // 
            this.bSupprimer.FlatAppearance.BorderSize = 0;
            this.bSupprimer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bSupprimer.ImageIndex = 6;
            this.bSupprimer.ImageList = this.imageList1;
            this.bSupprimer.Location = new System.Drawing.Point(25, 249);
            this.bSupprimer.Name = "bSupprimer";
            this.bSupprimer.Size = new System.Drawing.Size(64, 64);
            this.bSupprimer.TabIndex = 9;
            this.bSupprimer.UseVisualStyleBackColor = true;
            this.bSupprimer.Click += new System.EventHandler(this.bSupprimer_Click);
            // 
            // bExit
            // 
            this.bExit.FlatAppearance.BorderSize = 0;
            this.bExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bExit.ImageIndex = 7;
            this.bExit.ImageList = this.imageList1;
            this.bExit.Location = new System.Drawing.Point(463, 249);
            this.bExit.Name = "bExit";
            this.bExit.Size = new System.Drawing.Size(64, 64);
            this.bExit.TabIndex = 7;
            this.bExit.UseVisualStyleBackColor = true;
            this.bExit.Click += new System.EventHandler(this.bExit_Click);
            // 
            // bValider
            // 
            this.bValider.FlatAppearance.BorderSize = 0;
            this.bValider.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bValider.ImageIndex = 4;
            this.bValider.ImageList = this.imageList1;
            this.bValider.Location = new System.Drawing.Point(314, 249);
            this.bValider.Name = "bValider";
            this.bValider.Size = new System.Drawing.Size(64, 64);
            this.bValider.TabIndex = 6;
            this.bValider.UseVisualStyleBackColor = true;
            this.bValider.Click += new System.EventHandler(this.bValider_Click);
            // 
            // tBoxLibelle
            // 
            this.tBoxLibelle.BackColor = System.Drawing.SystemColors.ControlDark;
            this.tBoxLibelle.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tBoxLibelle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tBoxLibelle.ForeColor = System.Drawing.SystemColors.Window;
            this.tBoxLibelle.Location = new System.Drawing.Point(106, 46);
            this.tBoxLibelle.Name = "tBoxLibelle";
            this.tBoxLibelle.Size = new System.Drawing.Size(364, 15);
            this.tBoxLibelle.TabIndex = 0;
            this.tBoxLibelle.Click += new System.EventHandler(this.tBoxLibelle_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.SystemColors.Window;
            this.label5.Location = new System.Drawing.Point(103, 132);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(70, 16);
            this.label5.TabIndex = 6;
            this.label5.Text = "Catégorie:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.SystemColors.Window;
            this.label4.Location = new System.Drawing.Point(20, 77);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(80, 16);
            this.label4.TabIndex = 5;
            this.label4.Text = "Téléphone :";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.SystemColors.Window;
            this.label3.Location = new System.Drawing.Point(46, 45);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(54, 16);
            this.label3.TabIndex = 4;
            this.label3.Text = "Libellé :";
            // 
            // bCancel
            // 
            this.bCancel.FlatAppearance.BorderSize = 0;
            this.bCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bCancel.ImageIndex = 8;
            this.bCancel.ImageList = this.imageList1;
            this.bCancel.Location = new System.Drawing.Point(179, 249);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(64, 64);
            this.bCancel.TabIndex = 17;
            this.bCancel.UseVisualStyleBackColor = true;
            this.bCancel.Click += new System.EventHandler(this.bCancel_Click);
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
            // 
            // cBoxCategorie
            // 
            this.cBoxCategorie.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.cBoxCategorie.ForeColor = System.Drawing.SystemColors.Window;
            this.cBoxCategorie.FormattingEnabled = true;
            this.cBoxCategorie.Items.AddRange(new object[] {
            "Accueil et hébergement pour jeunes",
            "Aide et soins à domicile",
            "Centre suisse antipoison",
            "Divers",
            "Famille",
            "FMH",
            "Hôpitaux",
            "Laboratoires d\'analyses médicales",
            "Livraison d\'oxygène",
            "Médecin cantonal",
            "Pharmacie de garde",
            "Psychiatrie",
            "Physiothérapie",
            "Pompes funèbres",
            "Taxis",
            "Toxicodépendance",
            "Transports",
            "Serruriers"});
            this.cBoxCategorie.Location = new System.Drawing.Point(179, 124);
            this.cBoxCategorie.Name = "cBoxCategorie";
            this.cBoxCategorie.Size = new System.Drawing.Size(273, 24);
            this.cBoxCategorie.TabIndex = 2;
            // 
            // Num
            // 
            this.Num.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Num.ForeColor = System.Drawing.SystemColors.Window;
            this.Num.Location = new System.Drawing.Point(28, 16);
            this.Num.Name = "Num";
            this.Num.Size = new System.Drawing.Size(54, 16);
            this.Num.TabIndex = 19;
            this.Num.Visible = false;
            // 
            // cBoxSScategorie
            // 
            this.cBoxSScategorie.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.cBoxSScategorie.ForeColor = System.Drawing.SystemColors.Window;
            this.cBoxSScategorie.FormattingEnabled = true;
            this.cBoxSScategorie.Items.AddRange(new object[] {
            "Alcool",
            "Beau-Séjour",
            "Centre de thérapie brèves",
            "Clinique de Carouge",
            "Consultations",
            "Drogues",
            "Grangettes",
            "Hôpital de la tour",
            "Hôpital des Trois-Chêne",
            "HUG",
            "IMAD",
            "SIDA",
            "Suicide"});
            this.cBoxSScategorie.Location = new System.Drawing.Point(179, 177);
            this.cBoxSScategorie.Name = "cBoxSScategorie";
            this.cBoxSScategorie.Size = new System.Drawing.Size(273, 24);
            this.cBoxSScategorie.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.Window;
            this.label1.Location = new System.Drawing.Point(6, 185);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(169, 16);
            this.label1.TabIndex = 21;
            this.label1.Text = "Sous-catégorie (optionnel):";
            // 
            // FTelUtils
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.ClientSize = new System.Drawing.Size(1319, 352);
            this.ControlBox = false;
            this.Controls.Add(this.splitContainer1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.SystemColors.Window;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "FTelUtils";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Téléphones utils";
            this.Load += new System.EventHandler(this.FTelUtils_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.TextBox tBoxTel;
        private System.Windows.Forms.Button bAjouter;
        private System.Windows.Forms.Button bSupprimer;
        private System.Windows.Forms.Button bExit;
        private System.Windows.Forms.Button bValider;
        private System.Windows.Forms.TextBox tBoxLibelle;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button bCancel;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ComboBox cBoxCategorie;
        private System.Windows.Forms.Label Num;
        private System.Windows.Forms.ComboBox cBoxSScategorie;
        private System.Windows.Forms.Label label1;
    }
}