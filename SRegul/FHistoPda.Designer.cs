
namespace SRegulV2
{
    partial class FHistoPda
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FHistoPda));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.lCodeMedecin = new System.Windows.Forms.Label();
            this.tBoxMedecin = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.bRecherche = new System.Windows.Forms.Button();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.bFermer = new System.Windows.Forms.Button();
            this.dTimePFin = new System.Windows.Forms.DateTimePicker();
            this.dTimePDeb = new System.Windows.Forms.DateTimePicker();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.tBsmartphone = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.listView1 = new System.Windows.Forms.ListView();
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
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.lCodeMedecin);
            this.splitContainer1.Panel1.Controls.Add(this.tBoxMedecin);
            this.splitContainer1.Panel1.Controls.Add(this.label3);
            this.splitContainer1.Panel1.Controls.Add(this.bRecherche);
            this.splitContainer1.Panel1.Controls.Add(this.bFermer);
            this.splitContainer1.Panel1.Controls.Add(this.dTimePFin);
            this.splitContainer1.Panel1.Controls.Add(this.dTimePDeb);
            this.splitContainer1.Panel1.Controls.Add(this.label6);
            this.splitContainer1.Panel1.Controls.Add(this.label5);
            this.splitContainer1.Panel1.Controls.Add(this.tBsmartphone);
            this.splitContainer1.Panel1.Controls.Add(this.label2);
            this.splitContainer1.Panel1.ForeColor = System.Drawing.SystemColors.Window;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.listView1);
            this.splitContainer1.Size = new System.Drawing.Size(980, 485);
            this.splitContainer1.SplitterDistance = 87;
            this.splitContainer1.TabIndex = 2;
            // 
            // lCodeMedecin
            // 
            this.lCodeMedecin.AutoSize = true;
            this.lCodeMedecin.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lCodeMedecin.Location = new System.Drawing.Point(707, 21);
            this.lCodeMedecin.Name = "lCodeMedecin";
            this.lCodeMedecin.Size = new System.Drawing.Size(19, 16);
            this.lCodeMedecin.TabIndex = 89;
            this.lCodeMedecin.Text = "-1";
            // 
            // tBoxMedecin
            // 
            this.tBoxMedecin.BackColor = System.Drawing.SystemColors.ControlDark;
            this.tBoxMedecin.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tBoxMedecin.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tBoxMedecin.ForeColor = System.Drawing.SystemColors.Window;
            this.tBoxMedecin.Location = new System.Drawing.Point(494, 20);
            this.tBoxMedecin.Name = "tBoxMedecin";
            this.tBoxMedecin.Size = new System.Drawing.Size(207, 15);
            this.tBoxMedecin.TabIndex = 87;
            this.tBoxMedecin.TextChanged += new System.EventHandler(this.tBoxMedecin_TextChanged);
            this.tBoxMedecin.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tBoxMedecin_KeyDown);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(390, 20);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(98, 16);
            this.label3.TabIndex = 88;
            this.label3.Text = "Médecin <F5> :";
            // 
            // bRecherche
            // 
            this.bRecherche.FlatAppearance.BorderSize = 0;
            this.bRecherche.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bRecherche.ImageIndex = 1;
            this.bRecherche.ImageList = this.imageList1;
            this.bRecherche.Location = new System.Drawing.Point(760, 12);
            this.bRecherche.Name = "bRecherche";
            this.bRecherche.Size = new System.Drawing.Size(64, 64);
            this.bRecherche.TabIndex = 11;
            this.bRecherche.UseVisualStyleBackColor = true;
            this.bRecherche.Click += new System.EventHandler(this.bRecherche_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "bExitOn.png");
            this.imageList1.Images.SetKeyName(1, "bRecherche.png");
            // 
            // bFermer
            // 
            this.bFermer.FlatAppearance.BorderSize = 0;
            this.bFermer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bFermer.ImageIndex = 0;
            this.bFermer.ImageList = this.imageList1;
            this.bFermer.Location = new System.Drawing.Point(869, 12);
            this.bFermer.Name = "bFermer";
            this.bFermer.Size = new System.Drawing.Size(64, 64);
            this.bFermer.TabIndex = 12;
            this.bFermer.UseVisualStyleBackColor = true;
            this.bFermer.Click += new System.EventHandler(this.bFermer_Click);
            // 
            // dTimePFin
            // 
            this.dTimePFin.CalendarForeColor = System.Drawing.SystemColors.Window;
            this.dTimePFin.CalendarMonthBackground = System.Drawing.SystemColors.ControlDarkDark;
            this.dTimePFin.CalendarTitleForeColor = System.Drawing.SystemColors.Window;
            this.dTimePFin.Location = new System.Drawing.Point(140, 56);
            this.dTimePFin.Name = "dTimePFin";
            this.dTimePFin.Size = new System.Drawing.Size(200, 22);
            this.dTimePFin.TabIndex = 10;
            // 
            // dTimePDeb
            // 
            this.dTimePDeb.CalendarForeColor = System.Drawing.SystemColors.Window;
            this.dTimePDeb.CalendarMonthBackground = System.Drawing.SystemColors.ControlDarkDark;
            this.dTimePDeb.CalendarTitleForeColor = System.Drawing.SystemColors.Window;
            this.dTimePDeb.Location = new System.Drawing.Point(140, 15);
            this.dTimePDeb.Name = "dTimePDeb";
            this.dTimePDeb.Size = new System.Drawing.Size(200, 22);
            this.dTimePDeb.TabIndex = 9;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(25, 62);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(107, 16);
            this.label6.TabIndex = 85;
            this.label6.Text = "Date de fin rech.:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(6, 20);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(128, 16);
            this.label5.TabIndex = 83;
            this.label5.Text = "Date de début rech.:";
            // 
            // tBsmartphone
            // 
            this.tBsmartphone.BackColor = System.Drawing.SystemColors.ControlDark;
            this.tBsmartphone.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tBsmartphone.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tBsmartphone.ForeColor = System.Drawing.SystemColors.Window;
            this.tBsmartphone.Location = new System.Drawing.Point(494, 63);
            this.tBsmartphone.Name = "tBsmartphone";
            this.tBsmartphone.Size = new System.Drawing.Size(207, 15);
            this.tBsmartphone.TabIndex = 2;
            this.tBsmartphone.TextChanged += new System.EventHandler(this.tBsmartphone_TextChanged);
            this.tBsmartphone.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tBsmartphone_KeyDown);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(369, 62);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(119, 16);
            this.label2.TabIndex = 67;
            this.label2.Text = "Smartphone <F5> :";
            // 
            // listView1
            // 
            this.listView1.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listView1.ForeColor = System.Drawing.SystemColors.Window;
            this.listView1.FullRowSelect = true;
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(0, 0);
            this.listView1.MultiSelect = false;
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(980, 394);
            this.listView1.TabIndex = 1;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.List;
            this.listView1.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.listView1_ColumnClick);
            // 
            // FHistoPda
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.ClientSize = new System.Drawing.Size(980, 485);
            this.ControlBox = false;
            this.Controls.Add(this.splitContainer1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.SystemColors.Window;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FHistoPda";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Historique d\'affectation des Smartphones";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button bRecherche;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Button bFermer;
        private System.Windows.Forms.DateTimePicker dTimePFin;
        private System.Windows.Forms.DateTimePicker dTimePDeb;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tBsmartphone;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.TextBox tBoxMedecin;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lCodeMedecin;
    }
}