
namespace SRegulV2
{
    partial class FOrganisation
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FOrganisation));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.listView1 = new System.Windows.Forms.ListView();
            this.cBoxAvecAppli = new System.Windows.Forms.CheckBox();
            this.bAjouter = new System.Windows.Forms.Button();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.bSupprimer = new System.Windows.Forms.Button();
            this.bExit = new System.Windows.Forms.Button();
            this.bValider = new System.Windows.Forms.Button();
            this.bCancel = new System.Windows.Forms.Button();
            this.lIdOrg = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tBLibelleOrg = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.BackColor = System.Drawing.SystemColors.ControlDarkDark;
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
            this.splitContainer1.Panel2.Controls.Add(this.cBoxAvecAppli);
            this.splitContainer1.Panel2.Controls.Add(this.bAjouter);
            this.splitContainer1.Panel2.Controls.Add(this.bSupprimer);
            this.splitContainer1.Panel2.Controls.Add(this.bExit);
            this.splitContainer1.Panel2.Controls.Add(this.bValider);
            this.splitContainer1.Panel2.Controls.Add(this.bCancel);
            this.splitContainer1.Panel2.Controls.Add(this.lIdOrg);
            this.splitContainer1.Panel2.Controls.Add(this.label3);
            this.splitContainer1.Panel2.Controls.Add(this.label2);
            this.splitContainer1.Panel2.Controls.Add(this.tBLibelleOrg);
            this.splitContainer1.Size = new System.Drawing.Size(766, 273);
            this.splitContainer1.SplitterDistance = 315;
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
            this.listView1.Size = new System.Drawing.Size(311, 269);
            this.listView1.TabIndex = 1;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.List;
            this.listView1.DoubleClick += new System.EventHandler(this.listView1_DoubleClick);
            // 
            // cBoxAvecAppli
            // 
            this.cBoxAvecAppli.AutoSize = true;
            this.cBoxAvecAppli.ForeColor = System.Drawing.SystemColors.Window;
            this.cBoxAvecAppli.Location = new System.Drawing.Point(38, 117);
            this.cBoxAvecAppli.Name = "cBoxAvecAppli";
            this.cBoxAvecAppli.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.cBoxAvecAppli.Size = new System.Drawing.Size(139, 20);
            this.cBoxAvecAppli.TabIndex = 35;
            this.cBoxAvecAppli.Text = "Utilise l\'application";
            this.cBoxAvecAppli.UseVisualStyleBackColor = true;
            // 
            // bAjouter
            // 
            this.bAjouter.FlatAppearance.BorderSize = 0;
            this.bAjouter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bAjouter.ImageIndex = 0;
            this.bAjouter.ImageList = this.imageList1;
            this.bAjouter.Location = new System.Drawing.Point(140, 157);
            this.bAjouter.Name = "bAjouter";
            this.bAjouter.Size = new System.Drawing.Size(64, 64);
            this.bAjouter.TabIndex = 32;
            this.bAjouter.UseVisualStyleBackColor = true;
            this.bAjouter.Click += new System.EventHandler(this.bAjouter_Click);
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
            // bSupprimer
            // 
            this.bSupprimer.FlatAppearance.BorderSize = 0;
            this.bSupprimer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bSupprimer.ImageIndex = 6;
            this.bSupprimer.ImageList = this.imageList1;
            this.bSupprimer.Location = new System.Drawing.Point(38, 157);
            this.bSupprimer.Name = "bSupprimer";
            this.bSupprimer.Size = new System.Drawing.Size(64, 64);
            this.bSupprimer.TabIndex = 33;
            this.bSupprimer.UseVisualStyleBackColor = true;
            this.bSupprimer.Click += new System.EventHandler(this.bSupprimer_Click);
            // 
            // bExit
            // 
            this.bExit.FlatAppearance.BorderSize = 0;
            this.bExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bExit.ImageIndex = 7;
            this.bExit.ImageList = this.imageList1;
            this.bExit.Location = new System.Drawing.Point(333, 157);
            this.bExit.Name = "bExit";
            this.bExit.Size = new System.Drawing.Size(64, 64);
            this.bExit.TabIndex = 31;
            this.bExit.UseVisualStyleBackColor = true;
            this.bExit.Click += new System.EventHandler(this.bExit_Click);
            // 
            // bValider
            // 
            this.bValider.FlatAppearance.BorderSize = 0;
            this.bValider.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bValider.ImageIndex = 4;
            this.bValider.ImageList = this.imageList1;
            this.bValider.Location = new System.Drawing.Point(240, 157);
            this.bValider.Name = "bValider";
            this.bValider.Size = new System.Drawing.Size(64, 64);
            this.bValider.TabIndex = 30;
            this.bValider.UseVisualStyleBackColor = true;
            this.bValider.Click += new System.EventHandler(this.bValider_Click);
            // 
            // bCancel
            // 
            this.bCancel.FlatAppearance.BorderSize = 0;
            this.bCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bCancel.ImageIndex = 8;
            this.bCancel.ImageList = this.imageList1;
            this.bCancel.Location = new System.Drawing.Point(139, 157);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(64, 64);
            this.bCancel.TabIndex = 34;
            this.bCancel.UseVisualStyleBackColor = true;
            this.bCancel.Click += new System.EventHandler(this.bCancel_Click);
            // 
            // lIdOrg
            // 
            this.lIdOrg.AutoSize = true;
            this.lIdOrg.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lIdOrg.ForeColor = System.Drawing.SystemColors.Window;
            this.lIdOrg.Location = new System.Drawing.Point(359, 36);
            this.lIdOrg.Name = "lIdOrg";
            this.lIdOrg.Size = new System.Drawing.Size(29, 16);
            this.lIdOrg.TabIndex = 29;
            this.lIdOrg.Text = "999";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.SystemColors.Window;
            this.label3.Location = new System.Drawing.Point(252, 36);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(101, 16);
            this.label3.TabIndex = 28;
            this.label3.Text = "Id Organisation:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.Window;
            this.label2.Location = new System.Drawing.Point(35, 82);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(87, 16);
            this.label2.TabIndex = 27;
            this.label2.Text = "Organisation:";
            // 
            // tBLibelleOrg
            // 
            this.tBLibelleOrg.BackColor = System.Drawing.SystemColors.ControlDark;
            this.tBLibelleOrg.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tBLibelleOrg.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tBLibelleOrg.ForeColor = System.Drawing.SystemColors.Window;
            this.tBLibelleOrg.Location = new System.Drawing.Point(139, 83);
            this.tBLibelleOrg.Name = "tBLibelleOrg";
            this.tBLibelleOrg.Size = new System.Drawing.Size(271, 15);
            this.tBLibelleOrg.TabIndex = 26;
            this.tBLibelleOrg.Click += new System.EventHandler(this.tBLibelleOrg_Click);
            // 
            // FOrganisation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(766, 273);
            this.ControlBox = false;
            this.Controls.Add(this.splitContainer1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FOrganisation";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Organisations";
            this.Load += new System.EventHandler(this.FOrganisation_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Label lIdOrg;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tBLibelleOrg;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Button bAjouter;
        private System.Windows.Forms.Button bSupprimer;
        private System.Windows.Forms.Button bExit;
        private System.Windows.Forms.Button bValider;
        private System.Windows.Forms.Button bCancel;
        private System.Windows.Forms.CheckBox cBoxAvecAppli;
    }
}