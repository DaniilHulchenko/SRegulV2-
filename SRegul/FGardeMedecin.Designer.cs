namespace SRegulV2
{
    partial class FGardeMedecin
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FGardeMedecin));
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.listView1 = new System.Windows.Forms.ListView();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.bModif = new System.Windows.Forms.Button();
            this.tBSmartphone = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tBTypeGarde = new System.Windows.Forms.TextBox();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.listView3 = new System.Windows.Forms.ListView();
            this.listView4 = new System.Windows.Forms.ListView();
            this.label1 = new System.Windows.Forms.Label();
            this.tBoxPrenom = new System.Windows.Forms.TextBox();
            this.bFin = new System.Windows.Forms.Button();
            this.bExit = new System.Windows.Forms.Button();
            this.bDebut = new System.Windows.Forms.Button();
            this.tBoxNom = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tBCodeMedecin = new System.Windows.Forms.TextBox();
            this.listView2 = new System.Windows.Forms.ListView();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.SuspendLayout();
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "boutonsModifier.png");
            this.imageList1.Images.SetKeyName(1, "exit.png");
            this.imageList1.Images.SetKeyName(2, "bFinGarde.png");
            this.imageList1.Images.SetKeyName(3, "bDebGarde.png");
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
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(1270, 587);
            this.splitContainer1.SplitterDistance = 222;
            this.splitContainer1.TabIndex = 3;
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
            this.listView1.Size = new System.Drawing.Size(222, 587);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.List;
            this.listView1.DoubleClick += new System.EventHandler(this.listView1_DoubleClick);
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.bModif);
            this.splitContainer2.Panel1.Controls.Add(this.tBSmartphone);
            this.splitContainer2.Panel1.Controls.Add(this.label5);
            this.splitContainer2.Panel1.Controls.Add(this.tBTypeGarde);
            this.splitContainer2.Panel1.Controls.Add(this.splitContainer3);
            this.splitContainer2.Panel1.Controls.Add(this.label1);
            this.splitContainer2.Panel1.Controls.Add(this.tBoxPrenom);
            this.splitContainer2.Panel1.Controls.Add(this.bFin);
            this.splitContainer2.Panel1.Controls.Add(this.bExit);
            this.splitContainer2.Panel1.Controls.Add(this.bDebut);
            this.splitContainer2.Panel1.Controls.Add(this.tBoxNom);
            this.splitContainer2.Panel1.Controls.Add(this.label4);
            this.splitContainer2.Panel1.Controls.Add(this.label3);
            this.splitContainer2.Panel1.Controls.Add(this.label2);
            this.splitContainer2.Panel1.Controls.Add(this.tBCodeMedecin);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.listView2);
            this.splitContainer2.Size = new System.Drawing.Size(1044, 587);
            this.splitContainer2.SplitterDistance = 655;
            this.splitContainer2.TabIndex = 32;
            // 
            // bModif
            // 
            this.bModif.FlatAppearance.BorderSize = 0;
            this.bModif.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bModif.ImageIndex = 0;
            this.bModif.ImageList = this.imageList1;
            this.bModif.Location = new System.Drawing.Point(210, 12);
            this.bModif.Name = "bModif";
            this.bModif.Size = new System.Drawing.Size(64, 64);
            this.bModif.TabIndex = 7;
            this.toolTip1.SetToolTip(this.bModif, "Modifier");
            this.bModif.UseVisualStyleBackColor = true;
            this.bModif.Click += new System.EventHandler(this.bModif_Click);
            // 
            // tBSmartphone
            // 
            this.tBSmartphone.BackColor = System.Drawing.SystemColors.ControlDark;
            this.tBSmartphone.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tBSmartphone.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tBSmartphone.ForeColor = System.Drawing.SystemColors.Window;
            this.tBSmartphone.Location = new System.Drawing.Point(419, 276);
            this.tBSmartphone.Name = "tBSmartphone";
            this.tBSmartphone.ReadOnly = true;
            this.tBSmartphone.Size = new System.Drawing.Size(116, 15);
            this.tBSmartphone.TabIndex = 5;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.SystemColors.Window;
            this.label5.Location = new System.Drawing.Point(329, 276);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(84, 16);
            this.label5.TabIndex = 45;
            this.label5.Text = "Smartphone:";
            // 
            // tBTypeGarde
            // 
            this.tBTypeGarde.BackColor = System.Drawing.SystemColors.ControlDark;
            this.tBTypeGarde.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tBTypeGarde.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tBTypeGarde.ForeColor = System.Drawing.SystemColors.Window;
            this.tBTypeGarde.Location = new System.Drawing.Point(147, 277);
            this.tBTypeGarde.Name = "tBTypeGarde";
            this.tBTypeGarde.ReadOnly = true;
            this.tBTypeGarde.Size = new System.Drawing.Size(42, 15);
            this.tBTypeGarde.TabIndex = 4;
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitContainer3.Location = new System.Drawing.Point(0, 324);
            this.splitContainer3.Name = "splitContainer3";
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.listView3);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.listView4);
            this.splitContainer3.Size = new System.Drawing.Size(655, 263);
            this.splitContainer3.SplitterDistance = 274;
            this.splitContainer3.TabIndex = 43;
            // 
            // listView3
            // 
            this.listView3.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.listView3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listView3.ForeColor = System.Drawing.SystemColors.Window;
            this.listView3.FullRowSelect = true;
            this.listView3.Location = new System.Drawing.Point(0, 0);
            this.listView3.MultiSelect = false;
            this.listView3.Name = "listView3";
            this.listView3.Size = new System.Drawing.Size(274, 263);
            this.listView3.TabIndex = 31;
            this.listView3.UseCompatibleStateImageBehavior = false;
            this.listView3.View = System.Windows.Forms.View.List;
            this.listView3.DoubleClick += new System.EventHandler(this.listView3_DoubleClick);
            // 
            // listView4
            // 
            this.listView4.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.listView4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listView4.ForeColor = System.Drawing.SystemColors.Window;
            this.listView4.FullRowSelect = true;
            this.listView4.Location = new System.Drawing.Point(0, 0);
            this.listView4.MultiSelect = false;
            this.listView4.Name = "listView4";
            this.listView4.Size = new System.Drawing.Size(377, 263);
            this.listView4.TabIndex = 31;
            this.listView4.UseCompatibleStateImageBehavior = false;
            this.listView4.View = System.Windows.Forms.View.List;
            this.listView4.DoubleClick += new System.EventHandler(this.listView4_DoubleClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.Window;
            this.label1.Location = new System.Drawing.Point(40, 276);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 16);
            this.label1.TabIndex = 42;
            this.label1.Text = "Type de garde:";
            // 
            // tBoxPrenom
            // 
            this.tBoxPrenom.BackColor = System.Drawing.SystemColors.ControlDark;
            this.tBoxPrenom.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tBoxPrenom.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tBoxPrenom.ForeColor = System.Drawing.SystemColors.Window;
            this.tBoxPrenom.Location = new System.Drawing.Point(232, 209);
            this.tBoxPrenom.Name = "tBoxPrenom";
            this.tBoxPrenom.ReadOnly = true;
            this.tBoxPrenom.Size = new System.Drawing.Size(181, 15);
            this.tBoxPrenom.TabIndex = 3;
            // 
            // bFin
            // 
            this.bFin.FlatAppearance.BorderSize = 0;
            this.bFin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bFin.ImageIndex = 2;
            this.bFin.ImageList = this.imageList1;
            this.bFin.Location = new System.Drawing.Point(332, 12);
            this.bFin.Name = "bFin";
            this.bFin.Size = new System.Drawing.Size(64, 64);
            this.bFin.TabIndex = 8;
            this.toolTip1.SetToolTip(this.bFin, "Fin de garde");
            this.bFin.UseVisualStyleBackColor = true;
            this.bFin.Click += new System.EventHandler(this.bFin_Click);
            // 
            // bExit
            // 
            this.bExit.FlatAppearance.BorderSize = 0;
            this.bExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bExit.ImageIndex = 1;
            this.bExit.ImageList = this.imageList1;
            this.bExit.Location = new System.Drawing.Point(458, 12);
            this.bExit.Name = "bExit";
            this.bExit.Size = new System.Drawing.Size(64, 64);
            this.bExit.TabIndex = 10;
            this.toolTip1.SetToolTip(this.bExit, "Quitter");
            this.bExit.UseVisualStyleBackColor = true;
            this.bExit.Click += new System.EventHandler(this.bExit_Click);
            // 
            // bDebut
            // 
            this.bDebut.FlatAppearance.BorderSize = 0;
            this.bDebut.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bDebut.ImageIndex = 3;
            this.bDebut.ImageList = this.imageList1;
            this.bDebut.Location = new System.Drawing.Point(77, 12);
            this.bDebut.Name = "bDebut";
            this.bDebut.Size = new System.Drawing.Size(64, 64);
            this.bDebut.TabIndex = 6;
            this.toolTip1.SetToolTip(this.bDebut, "Début de garde");
            this.bDebut.UseVisualStyleBackColor = true;
            this.bDebut.Click += new System.EventHandler(this.bDebut_Click);
            // 
            // tBoxNom
            // 
            this.tBoxNom.BackColor = System.Drawing.SystemColors.ControlDark;
            this.tBoxNom.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tBoxNom.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tBoxNom.ForeColor = System.Drawing.SystemColors.Window;
            this.tBoxNom.Location = new System.Drawing.Point(232, 169);
            this.tBoxNom.Name = "tBoxNom";
            this.tBoxNom.ReadOnly = true;
            this.tBoxNom.Size = new System.Drawing.Size(181, 15);
            this.tBoxNom.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.SystemColors.Window;
            this.label4.Location = new System.Drawing.Point(160, 209);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(57, 16);
            this.label4.TabIndex = 37;
            this.label4.Text = "prénom:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.SystemColors.Window;
            this.label3.Location = new System.Drawing.Point(174, 169);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(43, 16);
            this.label3.TabIndex = 36;
            this.label3.Text = "Nom: ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.Window;
            this.label2.Location = new System.Drawing.Point(118, 127);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(99, 16);
            this.label2.TabIndex = 35;
            this.label2.Text = "Code médecin:";
            // 
            // tBCodeMedecin
            // 
            this.tBCodeMedecin.BackColor = System.Drawing.SystemColors.ControlDark;
            this.tBCodeMedecin.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tBCodeMedecin.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tBCodeMedecin.ForeColor = System.Drawing.SystemColors.Window;
            this.tBCodeMedecin.Location = new System.Drawing.Point(232, 128);
            this.tBCodeMedecin.Name = "tBCodeMedecin";
            this.tBCodeMedecin.ReadOnly = true;
            this.tBCodeMedecin.Size = new System.Drawing.Size(92, 15);
            this.tBCodeMedecin.TabIndex = 1;
            // 
            // listView2
            // 
            this.listView2.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.listView2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listView2.ForeColor = System.Drawing.SystemColors.Window;
            this.listView2.FullRowSelect = true;
            this.listView2.Location = new System.Drawing.Point(0, 0);
            this.listView2.MultiSelect = false;
            this.listView2.Name = "listView2";
            this.listView2.Size = new System.Drawing.Size(385, 587);
            this.listView2.TabIndex = 30;
            this.listView2.UseCompatibleStateImageBehavior = false;
            this.listView2.View = System.Windows.Forms.View.List;
            this.listView2.DoubleClick += new System.EventHandler(this.listView2_DoubleClick);
            // 
            // toolTip1
            // 
            this.toolTip1.AutomaticDelay = 250;
            this.toolTip1.IsBalloon = true;
            // 
            // FGardeMedecin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.ClientSize = new System.Drawing.Size(1270, 587);
            this.ControlBox = false;
            this.Controls.Add(this.splitContainer1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FGardeMedecin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Gestion des gardes des médecins";
            this.Load += new System.EventHandler(this.FGardeMedecin_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tBoxPrenom;
        private System.Windows.Forms.Button bFin;
        private System.Windows.Forms.Button bExit;
        private System.Windows.Forms.Button bDebut;
        private System.Windows.Forms.TextBox tBoxNom;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tBCodeMedecin;
        private System.Windows.Forms.ListView listView2;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.TextBox tBSmartphone;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tBTypeGarde;
        private System.Windows.Forms.Button bModif;
        private System.Windows.Forms.ListView listView3;
        private System.Windows.Forms.ListView listView4;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}