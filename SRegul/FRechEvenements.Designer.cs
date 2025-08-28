
namespace SRegulV2
{
    partial class FRechEvenements
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FRechEvenements));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.bRecherche = new System.Windows.Forms.Button();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.bFermer = new System.Windows.Forms.Button();
            this.dTimePFin = new System.Windows.Forms.DateTimePicker();
            this.dTimePDeb = new System.Windows.Forms.DateTimePicker();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.tBCritere = new System.Windows.Forms.TextBox();
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
            this.splitContainer1.Panel1.Controls.Add(this.bRecherche);
            this.splitContainer1.Panel1.Controls.Add(this.bFermer);
            this.splitContainer1.Panel1.Controls.Add(this.dTimePFin);
            this.splitContainer1.Panel1.Controls.Add(this.dTimePDeb);
            this.splitContainer1.Panel1.Controls.Add(this.label6);
            this.splitContainer1.Panel1.Controls.Add(this.label5);
            this.splitContainer1.Panel1.Controls.Add(this.tBCritere);
            this.splitContainer1.Panel1.Controls.Add(this.label2);
            this.splitContainer1.Panel1.ForeColor = System.Drawing.SystemColors.Window;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.listView1);
            this.splitContainer1.Size = new System.Drawing.Size(1336, 629);
            this.splitContainer1.SplitterDistance = 114;
            this.splitContainer1.TabIndex = 1;
            // 
            // bRecherche
            // 
            this.bRecherche.FlatAppearance.BorderSize = 0;
            this.bRecherche.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bRecherche.ImageIndex = 1;
            this.bRecherche.ImageList = this.imageList1;
            this.bRecherche.Location = new System.Drawing.Point(1001, 22);
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
            this.bFermer.Location = new System.Drawing.Point(1260, 20);
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
            this.dTimePFin.Location = new System.Drawing.Point(129, 56);
            this.dTimePFin.Name = "dTimePFin";
            this.dTimePFin.Size = new System.Drawing.Size(200, 22);
            this.dTimePFin.TabIndex = 10;
            // 
            // dTimePDeb
            // 
            this.dTimePDeb.CalendarForeColor = System.Drawing.SystemColors.Window;
            this.dTimePDeb.CalendarMonthBackground = System.Drawing.SystemColors.ControlDarkDark;
            this.dTimePDeb.CalendarTitleForeColor = System.Drawing.SystemColors.Window;
            this.dTimePDeb.Location = new System.Drawing.Point(129, 15);
            this.dTimePDeb.Name = "dTimePDeb";
            this.dTimePDeb.Size = new System.Drawing.Size(200, 22);
            this.dTimePDeb.TabIndex = 9;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(45, 61);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(78, 16);
            this.label6.TabIndex = 85;
            this.label6.Text = "Date de fin :";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(24, 20);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(99, 16);
            this.label5.TabIndex = 83;
            this.label5.Text = "Date de début :";
            // 
            // tBCritere
            // 
            this.tBCritere.BackColor = System.Drawing.SystemColors.ControlDark;
            this.tBCritere.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tBCritere.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tBCritere.ForeColor = System.Drawing.SystemColors.Window;
            this.tBCritere.Location = new System.Drawing.Point(540, 46);
            this.tBCritere.Name = "tBCritere";
            this.tBCritere.Size = new System.Drawing.Size(302, 15);
            this.tBCritere.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(392, 45);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(133, 16);
            this.label2.TabIndex = 67;
            this.label2.Text = "critère de recherche :";
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
            this.listView1.Size = new System.Drawing.Size(1336, 511);
            this.listView1.TabIndex = 1;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.List;
            this.listView1.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.listView1_ColumnClick);
            // 
            // FRechEvenements
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.ClientSize = new System.Drawing.Size(1336, 629);
            this.ControlBox = false;
            this.Controls.Add(this.splitContainer1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FRechEvenements";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Recherche d\'évenements";
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
        private System.Windows.Forms.Button bFermer;
        private System.Windows.Forms.DateTimePicker dTimePFin;
        private System.Windows.Forms.DateTimePicker dTimePDeb;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tBCritere;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ImageList imageList1;
    }
}