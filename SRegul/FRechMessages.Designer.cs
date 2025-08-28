namespace SRegulV2
{
    partial class FRechMessages
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FRechMessages));
            this.listView1 = new System.Windows.Forms.ListView();
            this.bExit = new System.Windows.Forms.Button();
            this.bRecherche = new System.Windows.Forms.Button();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.dTimePFin = new System.Windows.Forms.DateTimePicker();
            this.dTimePDeb = new System.Windows.Forms.DateTimePicker();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // listView1
            // 
            this.listView1.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.listView1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.listView1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listView1.ForeColor = System.Drawing.SystemColors.Window;
            this.listView1.FullRowSelect = true;
            this.listView1.Location = new System.Drawing.Point(0, 89);
            this.listView1.MultiSelect = false;
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(1100, 484);
            this.listView1.TabIndex = 3;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.List;
            this.listView1.DoubleClick += new System.EventHandler(this.listView1_DoubleClick);
            // 
            // bExit
            // 
            this.bExit.BackgroundImage = global::SRegulV2.Properties.Resources.exit;
            this.bExit.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.bExit.FlatAppearance.BorderSize = 0;
            this.bExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bExit.Location = new System.Drawing.Point(1024, 7);
            this.bExit.Name = "bExit";
            this.bExit.Size = new System.Drawing.Size(64, 64);
            this.bExit.TabIndex = 11;
            this.bExit.UseVisualStyleBackColor = true;
            this.bExit.Click += new System.EventHandler(this.bExit_Click);
            // 
            // bRecherche
            // 
            this.bRecherche.FlatAppearance.BorderSize = 0;
            this.bRecherche.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bRecherche.ImageIndex = 1;
            this.bRecherche.ImageList = this.imageList1;
            this.bRecherche.Location = new System.Drawing.Point(346, 7);
            this.bRecherche.Name = "bRecherche";
            this.bRecherche.Size = new System.Drawing.Size(64, 64);
            this.bRecherche.TabIndex = 88;
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
            // dTimePFin
            // 
            this.dTimePFin.CalendarForeColor = System.Drawing.SystemColors.Window;
            this.dTimePFin.CalendarMonthBackground = System.Drawing.SystemColors.ControlDarkDark;
            this.dTimePFin.CalendarTitleForeColor = System.Drawing.SystemColors.Window;
            this.dTimePFin.Location = new System.Drawing.Point(106, 44);
            this.dTimePFin.Name = "dTimePFin";
            this.dTimePFin.Size = new System.Drawing.Size(200, 22);
            this.dTimePFin.TabIndex = 87;
            // 
            // dTimePDeb
            // 
            this.dTimePDeb.CalendarForeColor = System.Drawing.SystemColors.Window;
            this.dTimePDeb.CalendarMonthBackground = System.Drawing.SystemColors.ControlDarkDark;
            this.dTimePDeb.CalendarTitleForeColor = System.Drawing.SystemColors.Window;
            this.dTimePDeb.Location = new System.Drawing.Point(108, 7);
            this.dTimePDeb.Name = "dTimePDeb";
            this.dTimePDeb.Size = new System.Drawing.Size(200, 22);
            this.dTimePDeb.TabIndex = 86;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(22, 50);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(78, 16);
            this.label6.TabIndex = 90;
            this.label6.Text = "Date de fin :";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(9, 12);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(99, 16);
            this.label5.TabIndex = 89;
            this.label5.Text = "Date de début :";
            // 
            // FRechMessages
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.ClientSize = new System.Drawing.Size(1100, 573);
            this.ControlBox = false;
            this.Controls.Add(this.bRecherche);
            this.Controls.Add(this.dTimePFin);
            this.Controls.Add(this.dTimePDeb);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.bExit);
            this.Controls.Add(this.listView1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.SystemColors.Window;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FRechMessages";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Recherche de messages";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.Button bExit;
        private System.Windows.Forms.Button bRecherche;
        private System.Windows.Forms.DateTimePicker dTimePFin;
        private System.Windows.Forms.DateTimePicker dTimePDeb;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ImageList imageList1;
    }
}