
namespace SRegulV2
{
    partial class FListeVisiteOrg
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FListeVisiteOrg));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.listView1 = new System.Windows.Forms.ListView();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.listView2 = new System.Windows.Forms.ListView();
            this.bAnnulEV = new System.Windows.Forms.Button();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.bEntreeVisite = new System.Windows.Forms.Button();
            this.bFinVisite = new System.Windows.Forms.Button();
            this.bReprise = new System.Windows.Forms.Button();
            this.bFermer = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
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
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(1214, 460);
            this.splitContainer1.SplitterDistance = 277;
            this.splitContainer1.TabIndex = 0;
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
            this.listView1.Size = new System.Drawing.Size(273, 456);
            this.listView1.TabIndex = 1;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.List;
            this.listView1.Click += new System.EventHandler(this.listView1_Click);
            // 
            // splitContainer2
            // 
            this.splitContainer2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.listView2);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.bAnnulEV);
            this.splitContainer2.Panel2.Controls.Add(this.bEntreeVisite);
            this.splitContainer2.Panel2.Controls.Add(this.bFinVisite);
            this.splitContainer2.Panel2.Controls.Add(this.bReprise);
            this.splitContainer2.Panel2.Controls.Add(this.bFermer);
            this.splitContainer2.Size = new System.Drawing.Size(933, 460);
            this.splitContainer2.SplitterDistance = 341;
            this.splitContainer2.TabIndex = 0;
            // 
            // listView2
            // 
            this.listView2.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.listView2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listView2.ForeColor = System.Drawing.SystemColors.Window;
            this.listView2.FullRowSelect = true;
            this.listView2.HideSelection = false;
            this.listView2.Location = new System.Drawing.Point(0, 0);
            this.listView2.MultiSelect = false;
            this.listView2.Name = "listView2";
            this.listView2.Size = new System.Drawing.Size(929, 337);
            this.listView2.TabIndex = 1;
            this.listView2.UseCompatibleStateImageBehavior = false;
            this.listView2.View = System.Windows.Forms.View.List;
            this.listView2.Click += new System.EventHandler(this.listView2_Click);
            // 
            // bAnnulEV
            // 
            this.bAnnulEV.FlatAppearance.BorderSize = 0;
            this.bAnnulEV.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bAnnulEV.ImageIndex = 4;
            this.bAnnulEV.ImageList = this.imageList1;
            this.bAnnulEV.Location = new System.Drawing.Point(299, 23);
            this.bAnnulEV.Name = "bAnnulEV";
            this.bAnnulEV.Size = new System.Drawing.Size(64, 64);
            this.bAnnulEV.TabIndex = 33;
            this.toolTip1.SetToolTip(this.bAnnulEV, "Annule l\'entrée en visite");
            this.bAnnulEV.UseVisualStyleBackColor = true;
            this.bAnnulEV.Click += new System.EventHandler(this.bAnnulEV_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "bAQ.png");
            this.imageList1.Images.SetKeyName(1, "bEV.png");
            this.imageList1.Images.SetKeyName(2, "bFV.png");
            this.imageList1.Images.SetKeyName(3, "bReprise.png");
            this.imageList1.Images.SetKeyName(4, "bAnnulEV.png");
            this.imageList1.Images.SetKeyName(5, "bExitOn.png");
            // 
            // bEntreeVisite
            // 
            this.bEntreeVisite.FlatAppearance.BorderSize = 0;
            this.bEntreeVisite.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bEntreeVisite.ImageIndex = 1;
            this.bEntreeVisite.ImageList = this.imageList1;
            this.bEntreeVisite.Location = new System.Drawing.Point(200, 23);
            this.bEntreeVisite.Name = "bEntreeVisite";
            this.bEntreeVisite.Size = new System.Drawing.Size(64, 64);
            this.bEntreeVisite.TabIndex = 32;
            this.toolTip1.SetToolTip(this.bEntreeVisite, "Entrée en visite");
            this.bEntreeVisite.UseVisualStyleBackColor = true;
            this.bEntreeVisite.Click += new System.EventHandler(this.bEntreeVisite_Click);
            // 
            // bFinVisite
            // 
            this.bFinVisite.FlatAppearance.BorderSize = 0;
            this.bFinVisite.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bFinVisite.ImageIndex = 2;
            this.bFinVisite.ImageList = this.imageList1;
            this.bFinVisite.Location = new System.Drawing.Point(420, 23);
            this.bFinVisite.Name = "bFinVisite";
            this.bFinVisite.Size = new System.Drawing.Size(64, 64);
            this.bFinVisite.TabIndex = 31;
            this.toolTip1.SetToolTip(this.bFinVisite, "Fin de visite");
            this.bFinVisite.UseVisualStyleBackColor = true;
            this.bFinVisite.Click += new System.EventHandler(this.bFinVisite_Click);
            // 
            // bReprise
            // 
            this.bReprise.FlatAppearance.BorderSize = 0;
            this.bReprise.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bReprise.ImageIndex = 3;
            this.bReprise.ImageList = this.imageList1;
            this.bReprise.Location = new System.Drawing.Point(533, 23);
            this.bReprise.Name = "bReprise";
            this.bReprise.Size = new System.Drawing.Size(64, 64);
            this.bReprise.TabIndex = 30;
            this.toolTip1.SetToolTip(this.bReprise, "Reprise de la visite");
            this.bReprise.UseVisualStyleBackColor = true;
            this.bReprise.Click += new System.EventHandler(this.bReprise_Click);
            // 
            // bFermer
            // 
            this.bFermer.FlatAppearance.BorderSize = 0;
            this.bFermer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bFermer.ImageIndex = 5;
            this.bFermer.ImageList = this.imageList1;
            this.bFermer.Location = new System.Drawing.Point(816, 23);
            this.bFermer.Name = "bFermer";
            this.bFermer.Size = new System.Drawing.Size(64, 64);
            this.bFermer.TabIndex = 29;
            this.bFermer.UseVisualStyleBackColor = true;
            this.bFermer.Click += new System.EventHandler(this.bFermer_Click);
            // 
            // toolTip1
            // 
            this.toolTip1.AutomaticDelay = 250;
            this.toolTip1.IsBalloon = true;
            // 
            // FListeVisiteOrg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.ClientSize = new System.Drawing.Size(1214, 460);
            this.ControlBox = false;
            this.Controls.Add(this.splitContainer1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FListeVisiteOrg";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Gestion des visites attribuées aux organisations";
            this.Load += new System.EventHandler(this.FListeVisiteOrg_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ListView listView2;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button bFermer;
        private System.Windows.Forms.Button bReprise;
        private System.Windows.Forms.Button bFinVisite;
        private System.Windows.Forms.Button bEntreeVisite;
        private System.Windows.Forms.Button bAnnulEV;
    }
}