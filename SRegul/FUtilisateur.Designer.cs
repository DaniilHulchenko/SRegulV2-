namespace SRegulV2
{
    partial class FUtilisateur
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FUtilisateur));
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.listView1 = new System.Windows.Forms.ListView();
            this.tBPassCTI = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.rBChefRegul = new System.Windows.Forms.RadioButton();
            this.rBdesactif = new System.Windows.Forms.RadioButton();
            this.rBInvite = new System.Windows.Forms.RadioButton();
            this.rBAdmin = new System.Windows.Forms.RadioButton();
            this.rBRegul = new System.Windows.Forms.RadioButton();
            this.label8 = new System.Windows.Forms.Label();
            this.tBPosteTel = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.tBIdCTI = new System.Windows.Forms.TextBox();
            this.bPassword = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.tBIdSmartRapport = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tBoxPrenom = new System.Windows.Forms.TextBox();
            this.bAjouter = new System.Windows.Forms.Button();
            this.bSupprimer = new System.Windows.Forms.Button();
            this.bExit = new System.Windows.Forms.Button();
            this.bValider = new System.Windows.Forms.Button();
            this.tBoxNom = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tBIdUtilisateur = new System.Windows.Forms.TextBox();
            this.bCancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
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
            this.splitContainer1.Panel2.Controls.Add(this.tBPassCTI);
            this.splitContainer1.Panel2.Controls.Add(this.label9);
            this.splitContainer1.Panel2.Controls.Add(this.rBChefRegul);
            this.splitContainer1.Panel2.Controls.Add(this.rBdesactif);
            this.splitContainer1.Panel2.Controls.Add(this.rBInvite);
            this.splitContainer1.Panel2.Controls.Add(this.rBAdmin);
            this.splitContainer1.Panel2.Controls.Add(this.rBRegul);
            this.splitContainer1.Panel2.Controls.Add(this.label8);
            this.splitContainer1.Panel2.Controls.Add(this.tBPosteTel);
            this.splitContainer1.Panel2.Controls.Add(this.label7);
            this.splitContainer1.Panel2.Controls.Add(this.tBIdCTI);
            this.splitContainer1.Panel2.Controls.Add(this.bPassword);
            this.splitContainer1.Panel2.Controls.Add(this.label6);
            this.splitContainer1.Panel2.Controls.Add(this.tBIdSmartRapport);
            this.splitContainer1.Panel2.Controls.Add(this.label1);
            this.splitContainer1.Panel2.Controls.Add(this.tBoxPrenom);
            this.splitContainer1.Panel2.Controls.Add(this.bAjouter);
            this.splitContainer1.Panel2.Controls.Add(this.bSupprimer);
            this.splitContainer1.Panel2.Controls.Add(this.bExit);
            this.splitContainer1.Panel2.Controls.Add(this.bValider);
            this.splitContainer1.Panel2.Controls.Add(this.tBoxNom);
            this.splitContainer1.Panel2.Controls.Add(this.label5);
            this.splitContainer1.Panel2.Controls.Add(this.label4);
            this.splitContainer1.Panel2.Controls.Add(this.label3);
            this.splitContainer1.Panel2.Controls.Add(this.label2);
            this.splitContainer1.Panel2.Controls.Add(this.tBIdUtilisateur);
            this.splitContainer1.Panel2.Controls.Add(this.bCancel);
            this.splitContainer1.Size = new System.Drawing.Size(895, 356);
            this.splitContainer1.SplitterDistance = 253;
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
            this.listView1.Size = new System.Drawing.Size(253, 356);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.List;
            this.listView1.DoubleClick += new System.EventHandler(this.listView1_DoubleClick);
            // 
            // tBPassCTI
            // 
            this.tBPassCTI.BackColor = System.Drawing.SystemColors.ControlDark;
            this.tBPassCTI.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tBPassCTI.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tBPassCTI.ForeColor = System.Drawing.SystemColors.Window;
            this.tBPassCTI.Location = new System.Drawing.Point(310, 207);
            this.tBPassCTI.Name = "tBPassCTI";
            this.tBPassCTI.Size = new System.Drawing.Size(101, 15);
            this.tBPassCTI.TabIndex = 30;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.SystemColors.Window;
            this.label9.Location = new System.Drawing.Point(238, 206);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(69, 16);
            this.label9.TabIndex = 31;
            this.label9.Text = "Pass CTI :";
            // 
            // rBChefRegul
            // 
            this.rBChefRegul.AutoSize = true;
            this.rBChefRegul.FlatAppearance.BorderSize = 0;
            this.rBChefRegul.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.rBChefRegul.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rBChefRegul.ForeColor = System.Drawing.SystemColors.Window;
            this.rBChefRegul.Location = new System.Drawing.Point(515, 176);
            this.rBChefRegul.Name = "rBChefRegul";
            this.rBChefRegul.Size = new System.Drawing.Size(116, 20);
            this.rBChefRegul.TabIndex = 29;
            this.rBChefRegul.Text = "Chef régulateur";
            this.rBChefRegul.UseVisualStyleBackColor = true;
            // 
            // rBdesactif
            // 
            this.rBdesactif.AutoSize = true;
            this.rBdesactif.FlatAppearance.BorderSize = 0;
            this.rBdesactif.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.rBdesactif.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rBdesactif.ForeColor = System.Drawing.SystemColors.Window;
            this.rBdesactif.Location = new System.Drawing.Point(515, 98);
            this.rBdesactif.Name = "rBdesactif";
            this.rBdesactif.Size = new System.Drawing.Size(87, 20);
            this.rBdesactif.TabIndex = 28;
            this.rBdesactif.Text = "Désactivé";
            this.rBdesactif.UseVisualStyleBackColor = true;
            // 
            // rBInvite
            // 
            this.rBInvite.AutoSize = true;
            this.rBInvite.FlatAppearance.BorderSize = 0;
            this.rBInvite.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.rBInvite.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rBInvite.ForeColor = System.Drawing.SystemColors.Window;
            this.rBInvite.Location = new System.Drawing.Point(515, 124);
            this.rBInvite.Name = "rBInvite";
            this.rBInvite.Size = new System.Drawing.Size(57, 20);
            this.rBInvite.TabIndex = 27;
            this.rBInvite.Text = "Invité";
            this.rBInvite.UseVisualStyleBackColor = true;
            // 
            // rBAdmin
            // 
            this.rBAdmin.AutoSize = true;
            this.rBAdmin.FlatAppearance.BorderSize = 0;
            this.rBAdmin.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.rBAdmin.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rBAdmin.ForeColor = System.Drawing.SystemColors.Window;
            this.rBAdmin.Location = new System.Drawing.Point(515, 202);
            this.rBAdmin.Name = "rBAdmin";
            this.rBAdmin.Size = new System.Drawing.Size(111, 20);
            this.rBAdmin.TabIndex = 9;
            this.rBAdmin.Text = "Administrateur";
            this.rBAdmin.UseVisualStyleBackColor = true;
            // 
            // rBRegul
            // 
            this.rBRegul.AutoSize = true;
            this.rBRegul.FlatAppearance.BorderSize = 0;
            this.rBRegul.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.rBRegul.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rBRegul.ForeColor = System.Drawing.SystemColors.Window;
            this.rBRegul.Location = new System.Drawing.Point(515, 150);
            this.rBRegul.Name = "rBRegul";
            this.rBRegul.Size = new System.Drawing.Size(92, 20);
            this.rBRegul.TabIndex = 8;
            this.rBRegul.Text = "Régulateur";
            this.rBRegul.UseVisualStyleBackColor = true;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.SystemColors.Window;
            this.label8.Location = new System.Drawing.Point(512, 75);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(49, 16);
            this.label8.TabIndex = 26;
            this.label8.Text = "Droits :";
            // 
            // tBPosteTel
            // 
            this.tBPosteTel.BackColor = System.Drawing.SystemColors.ControlDark;
            this.tBPosteTel.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tBPosteTel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tBPosteTel.ForeColor = System.Drawing.SystemColors.Window;
            this.tBPosteTel.Location = new System.Drawing.Point(131, 207);
            this.tBPosteTel.Name = "tBPosteTel";
            this.tBPosteTel.Size = new System.Drawing.Size(81, 15);
            this.tBPosteTel.TabIndex = 6;
            this.tBPosteTel.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tBPosteTel_KeyDown);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.SystemColors.Window;
            this.label7.Location = new System.Drawing.Point(21, 206);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(107, 16);
            this.label7.TabIndex = 24;
            this.label7.Text = "N° de poste Tel :";
            // 
            // tBIdCTI
            // 
            this.tBIdCTI.BackColor = System.Drawing.SystemColors.ControlDark;
            this.tBIdCTI.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tBIdCTI.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tBIdCTI.ForeColor = System.Drawing.SystemColors.Window;
            this.tBIdCTI.Location = new System.Drawing.Point(310, 162);
            this.tBIdCTI.Name = "tBIdCTI";
            this.tBIdCTI.Size = new System.Drawing.Size(101, 15);
            this.tBIdCTI.TabIndex = 5;
            // 
            // bPassword
            // 
            this.bPassword.FlatAppearance.BorderSize = 0;
            this.bPassword.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bPassword.ImageKey = "bPasswordOn.png";
            this.bPassword.ImageList = this.imageList1;
            this.bPassword.Location = new System.Drawing.Point(373, 52);
            this.bPassword.Name = "bPassword";
            this.bPassword.Size = new System.Drawing.Size(64, 64);
            this.bPassword.TabIndex = 7;
            this.bPassword.UseVisualStyleBackColor = true;
            this.bPassword.Click += new System.EventHandler(this.bPassword_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.SystemColors.Window;
            this.label6.Location = new System.Drawing.Point(334, 33);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(141, 16);
            this.label6.TabIndex = 21;
            this.label6.Text = "Ajouter/Modifier pass :";
            // 
            // tBIdSmartRapport
            // 
            this.tBIdSmartRapport.BackColor = System.Drawing.SystemColors.ControlDark;
            this.tBIdSmartRapport.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tBIdSmartRapport.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tBIdSmartRapport.ForeColor = System.Drawing.SystemColors.Window;
            this.tBIdSmartRapport.Location = new System.Drawing.Point(131, 164);
            this.tBIdSmartRapport.Name = "tBIdSmartRapport";
            this.tBIdSmartRapport.Size = new System.Drawing.Size(81, 15);
            this.tBIdSmartRapport.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.Window;
            this.label1.Location = new System.Drawing.Point(258, 161);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 16);
            this.label1.TabIndex = 19;
            this.label1.Text = "Id CTI :";
            // 
            // tBoxPrenom
            // 
            this.tBoxPrenom.BackColor = System.Drawing.SystemColors.ControlDark;
            this.tBoxPrenom.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tBoxPrenom.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tBoxPrenom.ForeColor = System.Drawing.SystemColors.Window;
            this.tBoxPrenom.Location = new System.Drawing.Point(96, 113);
            this.tBoxPrenom.Name = "tBoxPrenom";
            this.tBoxPrenom.Size = new System.Drawing.Size(211, 15);
            this.tBoxPrenom.TabIndex = 3;
            // 
            // bAjouter
            // 
            this.bAjouter.FlatAppearance.BorderSize = 0;
            this.bAjouter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bAjouter.ImageIndex = 0;
            this.bAjouter.ImageList = this.imageList1;
            this.bAjouter.Location = new System.Drawing.Point(148, 269);
            this.bAjouter.Name = "bAjouter";
            this.bAjouter.Size = new System.Drawing.Size(64, 64);
            this.bAjouter.TabIndex = 11;
            this.bAjouter.UseVisualStyleBackColor = true;
            this.bAjouter.Click += new System.EventHandler(this.bAjouter_Click);
            // 
            // bSupprimer
            // 
            this.bSupprimer.FlatAppearance.BorderSize = 0;
            this.bSupprimer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bSupprimer.ImageIndex = 6;
            this.bSupprimer.ImageList = this.imageList1;
            this.bSupprimer.Location = new System.Drawing.Point(22, 269);
            this.bSupprimer.Name = "bSupprimer";
            this.bSupprimer.Size = new System.Drawing.Size(64, 64);
            this.bSupprimer.TabIndex = 14;
            this.bSupprimer.UseVisualStyleBackColor = true;
            this.bSupprimer.Click += new System.EventHandler(this.bSupprimer_Click);
            // 
            // bExit
            // 
            this.bExit.FlatAppearance.BorderSize = 0;
            this.bExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bExit.ImageIndex = 7;
            this.bExit.ImageList = this.imageList1;
            this.bExit.Location = new System.Drawing.Point(383, 269);
            this.bExit.Name = "bExit";
            this.bExit.Size = new System.Drawing.Size(64, 64);
            this.bExit.TabIndex = 12;
            this.bExit.UseVisualStyleBackColor = true;
            this.bExit.Click += new System.EventHandler(this.bExit_Click);
            // 
            // bValider
            // 
            this.bValider.FlatAppearance.BorderSize = 0;
            this.bValider.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bValider.ImageIndex = 4;
            this.bValider.ImageList = this.imageList1;
            this.bValider.Location = new System.Drawing.Point(261, 269);
            this.bValider.Name = "bValider";
            this.bValider.Size = new System.Drawing.Size(64, 64);
            this.bValider.TabIndex = 10;
            this.bValider.UseVisualStyleBackColor = true;
            this.bValider.Click += new System.EventHandler(this.bValider_Click);
            // 
            // tBoxNom
            // 
            this.tBoxNom.BackColor = System.Drawing.SystemColors.ControlDark;
            this.tBoxNom.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tBoxNom.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tBoxNom.ForeColor = System.Drawing.SystemColors.Window;
            this.tBoxNom.Location = new System.Drawing.Point(96, 76);
            this.tBoxNom.Name = "tBoxNom";
            this.tBoxNom.Size = new System.Drawing.Size(211, 15);
            this.tBoxNom.TabIndex = 2;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.SystemColors.Window;
            this.label5.Location = new System.Drawing.Point(16, 164);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(112, 16);
            this.label5.TabIndex = 6;
            this.label5.Text = "Id SmartRapport :";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.SystemColors.Window;
            this.label4.Location = new System.Drawing.Point(28, 113);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(60, 16);
            this.label4.TabIndex = 5;
            this.label4.Text = "prénom :";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.SystemColors.Window;
            this.label3.Location = new System.Drawing.Point(46, 76);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 16);
            this.label3.TabIndex = 4;
            this.label3.Text = "Nom : ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.Window;
            this.label2.Location = new System.Drawing.Point(3, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(87, 16);
            this.label2.TabIndex = 3;
            this.label2.Text = "Id Utilisateur :";
            // 
            // tBIdUtilisateur
            // 
            this.tBIdUtilisateur.BackColor = System.Drawing.SystemColors.ControlDark;
            this.tBIdUtilisateur.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tBIdUtilisateur.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tBIdUtilisateur.ForeColor = System.Drawing.SystemColors.Window;
            this.tBIdUtilisateur.Location = new System.Drawing.Point(96, 36);
            this.tBIdUtilisateur.Name = "tBIdUtilisateur";
            this.tBIdUtilisateur.Size = new System.Drawing.Size(92, 15);
            this.tBIdUtilisateur.TabIndex = 1;
            this.tBIdUtilisateur.Click += new System.EventHandler(this.tBIdUtilisateur_Click);
            // 
            // bCancel
            // 
            this.bCancel.FlatAppearance.BorderSize = 0;
            this.bCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bCancel.ImageIndex = 8;
            this.bCancel.ImageList = this.imageList1;
            this.bCancel.Location = new System.Drawing.Point(148, 269);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(64, 64);
            this.bCancel.TabIndex = 13;
            this.bCancel.UseVisualStyleBackColor = true;
            this.bCancel.Click += new System.EventHandler(this.bCancel_Click);
            // 
            // FUtilisateur
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(895, 356);
            this.ControlBox = false;
            this.Controls.Add(this.splitContainer1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Name = "FUtilisateur";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Utilisateur";
            this.Load += new System.EventHandler(this.FUtilisateur_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tBoxPrenom;
        private System.Windows.Forms.Button bAjouter;
        private System.Windows.Forms.Button bSupprimer;
        private System.Windows.Forms.Button bExit;
        private System.Windows.Forms.Button bValider;
        private System.Windows.Forms.TextBox tBoxNom;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tBIdUtilisateur;
        private System.Windows.Forms.Button bCancel;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tBIdSmartRapport;
        private System.Windows.Forms.Button bPassword;
        private System.Windows.Forms.TextBox tBPosteTel;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox tBIdCTI;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.RadioButton rBAdmin;
        private System.Windows.Forms.RadioButton rBRegul;
        private System.Windows.Forms.RadioButton rBInvite;
        private System.Windows.Forms.RadioButton rBdesactif;
        private System.Windows.Forms.RadioButton rBChefRegul;
        private System.Windows.Forms.TextBox tBPassCTI;
        private System.Windows.Forms.Label label9;
    }
}