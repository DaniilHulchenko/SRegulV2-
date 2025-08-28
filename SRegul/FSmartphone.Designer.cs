namespace SRegulV2
{
    partial class FSmartphone
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FSmartphone));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.listView1 = new System.Windows.Forms.ListView();
            this.labelId = new System.Windows.Forms.Label();
            this.bSupprimer = new System.Windows.Forms.Button();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.bExit = new System.Windows.Forms.Button();
            this.bValider = new System.Windows.Forms.Button();
            this.bAjouter = new System.Windows.Forms.Button();
            this.comboMedecin = new System.Windows.Forms.ComboBox();
            this.tBoxCodeMedecin = new System.Windows.Forms.TextBox();
            this.tBoxTel = new System.Windows.Forms.TextBox();
            this.tBoxSim = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tBoxNomSmart = new System.Windows.Forms.TextBox();
            this.cBoxActif = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.bCancel = new System.Windows.Forms.Button();
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
            this.splitContainer1.Panel2.Controls.Add(this.labelId);
            this.splitContainer1.Panel2.Controls.Add(this.bSupprimer);
            this.splitContainer1.Panel2.Controls.Add(this.bExit);
            this.splitContainer1.Panel2.Controls.Add(this.bValider);
            this.splitContainer1.Panel2.Controls.Add(this.bAjouter);
            this.splitContainer1.Panel2.Controls.Add(this.comboMedecin);
            this.splitContainer1.Panel2.Controls.Add(this.tBoxCodeMedecin);
            this.splitContainer1.Panel2.Controls.Add(this.tBoxTel);
            this.splitContainer1.Panel2.Controls.Add(this.tBoxSim);
            this.splitContainer1.Panel2.Controls.Add(this.label5);
            this.splitContainer1.Panel2.Controls.Add(this.label4);
            this.splitContainer1.Panel2.Controls.Add(this.label3);
            this.splitContainer1.Panel2.Controls.Add(this.label2);
            this.splitContainer1.Panel2.Controls.Add(this.tBoxNomSmart);
            this.splitContainer1.Panel2.Controls.Add(this.cBoxActif);
            this.splitContainer1.Panel2.Controls.Add(this.label1);
            this.splitContainer1.Panel2.Controls.Add(this.bCancel);
            this.splitContainer1.Size = new System.Drawing.Size(836, 393);
            this.splitContainer1.SplitterDistance = 289;
            this.splitContainer1.TabIndex = 0;
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
            this.listView1.Size = new System.Drawing.Size(289, 393);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.List;
            this.listView1.DoubleClick += new System.EventHandler(this.listView1_DoubleClick);
            // 
            // labelId
            // 
            this.labelId.AutoSize = true;
            this.labelId.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelId.ForeColor = System.Drawing.SystemColors.Window;
            this.labelId.Location = new System.Drawing.Point(158, 52);
            this.labelId.Name = "labelId";
            this.labelId.Size = new System.Drawing.Size(0, 16);
            this.labelId.TabIndex = 16;
            // 
            // bSupprimer
            // 
            this.bSupprimer.FlatAppearance.BorderSize = 0;
            this.bSupprimer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bSupprimer.ImageIndex = 6;
            this.bSupprimer.ImageList = this.imageList1;
            this.bSupprimer.Location = new System.Drawing.Point(48, 295);
            this.bSupprimer.Name = "bSupprimer";
            this.bSupprimer.Size = new System.Drawing.Size(64, 64);
            this.bSupprimer.TabIndex = 10;
            this.bSupprimer.UseVisualStyleBackColor = true;
            this.bSupprimer.Click += new System.EventHandler(this.bSupprimer_Click);
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
            // bExit
            // 
            this.bExit.FlatAppearance.BorderSize = 0;
            this.bExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bExit.ImageIndex = 7;
            this.bExit.ImageList = this.imageList1;
            this.bExit.Location = new System.Drawing.Point(411, 295);
            this.bExit.Name = "bExit";
            this.bExit.Size = new System.Drawing.Size(64, 64);
            this.bExit.TabIndex = 8;
            this.bExit.UseVisualStyleBackColor = true;
            this.bExit.Click += new System.EventHandler(this.bExit_Click);
            // 
            // bValider
            // 
            this.bValider.FlatAppearance.BorderSize = 0;
            this.bValider.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bValider.ImageIndex = 4;
            this.bValider.ImageList = this.imageList1;
            this.bValider.Location = new System.Drawing.Point(278, 295);
            this.bValider.Name = "bValider";
            this.bValider.Size = new System.Drawing.Size(64, 64);
            this.bValider.TabIndex = 7;
            this.bValider.UseVisualStyleBackColor = true;
            this.bValider.Click += new System.EventHandler(this.bValider_Click);
            // 
            // bAjouter
            // 
            this.bAjouter.FlatAppearance.BorderSize = 0;
            this.bAjouter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bAjouter.ImageIndex = 0;
            this.bAjouter.ImageList = this.imageList1;
            this.bAjouter.Location = new System.Drawing.Point(166, 295);
            this.bAjouter.Name = "bAjouter";
            this.bAjouter.Size = new System.Drawing.Size(64, 64);
            this.bAjouter.TabIndex = 9;
            this.bAjouter.UseVisualStyleBackColor = true;
            this.bAjouter.Click += new System.EventHandler(this.bAjouter_Click);
            // 
            // comboMedecin
            // 
            this.comboMedecin.BackColor = System.Drawing.SystemColors.ControlDark;
            this.comboMedecin.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboMedecin.ForeColor = System.Drawing.SystemColors.Window;
            this.comboMedecin.FormattingEnabled = true;
            this.comboMedecin.Location = new System.Drawing.Point(291, 215);
            this.comboMedecin.Name = "comboMedecin";
            this.comboMedecin.Size = new System.Drawing.Size(202, 24);
            this.comboMedecin.TabIndex = 5;
            this.comboMedecin.DropDown += new System.EventHandler(this.comboMedecin_DropDown);
            this.comboMedecin.SelectedIndexChanged += new System.EventHandler(this.comboMedecin_SelectedIndexChanged);
            this.comboMedecin.TextChanged += new System.EventHandler(this.comboMedecin_TextChanged);
            // 
            // tBoxCodeMedecin
            // 
            this.tBoxCodeMedecin.BackColor = System.Drawing.SystemColors.ControlDark;
            this.tBoxCodeMedecin.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tBoxCodeMedecin.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tBoxCodeMedecin.ForeColor = System.Drawing.SystemColors.Window;
            this.tBoxCodeMedecin.Location = new System.Drawing.Point(161, 224);
            this.tBoxCodeMedecin.Name = "tBoxCodeMedecin";
            this.tBoxCodeMedecin.Size = new System.Drawing.Size(85, 15);
            this.tBoxCodeMedecin.TabIndex = 4;
            this.tBoxCodeMedecin.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tBoxCodeMedecin_KeyDown);
            this.tBoxCodeMedecin.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tBoxCodeMedecin_KeyPress);
            this.tBoxCodeMedecin.Leave += new System.EventHandler(this.tBoxCodeMedecin_Leave);
            // 
            // tBoxTel
            // 
            this.tBoxTel.BackColor = System.Drawing.SystemColors.ControlDark;
            this.tBoxTel.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tBoxTel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tBoxTel.ForeColor = System.Drawing.SystemColors.Window;
            this.tBoxTel.Location = new System.Drawing.Point(161, 182);
            this.tBoxTel.Name = "tBoxTel";
            this.tBoxTel.Size = new System.Drawing.Size(134, 15);
            this.tBoxTel.TabIndex = 3;
            this.tBoxTel.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tBoxTel_KeyDown);
            this.tBoxTel.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tBoxTel_KeyPress);
            // 
            // tBoxSim
            // 
            this.tBoxSim.BackColor = System.Drawing.SystemColors.ControlDark;
            this.tBoxSim.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tBoxSim.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tBoxSim.ForeColor = System.Drawing.SystemColors.Window;
            this.tBoxSim.Location = new System.Drawing.Point(161, 138);
            this.tBoxSim.Name = "tBoxSim";
            this.tBoxSim.Size = new System.Drawing.Size(181, 15);
            this.tBoxSim.TabIndex = 2;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.SystemColors.Window;
            this.label5.Location = new System.Drawing.Point(53, 223);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(102, 16);
            this.label5.TabIndex = 6;
            this.label5.Text = "Code Médecin :";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.SystemColors.Window;
            this.label4.Location = new System.Drawing.Point(45, 181);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(110, 16);
            this.label4.TabIndex = 5;
            this.label4.Text = "N° de téléphone :";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.SystemColors.Window;
            this.label3.Location = new System.Drawing.Point(41, 138);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(114, 16);
            this.label3.TabIndex = 4;
            this.label3.Text = "N° ID Application :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.Window;
            this.label2.Location = new System.Drawing.Point(18, 93);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(137, 16);
            this.label2.TabIndex = 3;
            this.label2.Text = "Nom du Smartphone :";
            // 
            // tBoxNomSmart
            // 
            this.tBoxNomSmart.BackColor = System.Drawing.SystemColors.ControlDark;
            this.tBoxNomSmart.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tBoxNomSmart.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tBoxNomSmart.ForeColor = System.Drawing.SystemColors.Window;
            this.tBoxNomSmart.Location = new System.Drawing.Point(161, 94);
            this.tBoxNomSmart.Name = "tBoxNomSmart";
            this.tBoxNomSmart.Size = new System.Drawing.Size(181, 15);
            this.tBoxNomSmart.TabIndex = 1;
            this.tBoxNomSmart.Click += new System.EventHandler(this.tBoxNomSmart_Click);
            // 
            // cBoxActif
            // 
            this.cBoxActif.AutoSize = true;
            this.cBoxActif.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cBoxActif.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cBoxActif.ForeColor = System.Drawing.SystemColors.Window;
            this.cBoxActif.Location = new System.Drawing.Point(423, 122);
            this.cBoxActif.Name = "cBoxActif";
            this.cBoxActif.Size = new System.Drawing.Size(52, 20);
            this.cBoxActif.TabIndex = 6;
            this.cBoxActif.Text = "Actif";
            this.cBoxActif.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.Window;
            this.label1.Location = new System.Drawing.Point(54, 52);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Id Smartphone :";
            // 
            // bCancel
            // 
            this.bCancel.FlatAppearance.BorderSize = 0;
            this.bCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bCancel.ImageIndex = 8;
            this.bCancel.ImageList = this.imageList1;
            this.bCancel.Location = new System.Drawing.Point(166, 295);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(64, 64);
            this.bCancel.TabIndex = 17;
            this.bCancel.UseVisualStyleBackColor = true;
            this.bCancel.Click += new System.EventHandler(this.bCancel_Click);
            // 
            // FSmartphone
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(836, 393);
            this.ControlBox = false;
            this.Controls.Add(this.splitContainer1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Name = "FSmartphone";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Gestion des Smartphones";
            this.Load += new System.EventHandler(this.FSmartphone_Load);
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
        private System.Windows.Forms.TextBox tBoxNomSmart;
        private System.Windows.Forms.CheckBox cBoxActif;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tBoxSim;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboMedecin;
        private System.Windows.Forms.TextBox tBoxCodeMedecin;
        private System.Windows.Forms.TextBox tBoxTel;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Button bAjouter;
        private System.Windows.Forms.Button bValider;
        private System.Windows.Forms.Button bExit;
        private System.Windows.Forms.Button bSupprimer;
        private System.Windows.Forms.Label labelId;
        private System.Windows.Forms.Button bCancel;
    }
}