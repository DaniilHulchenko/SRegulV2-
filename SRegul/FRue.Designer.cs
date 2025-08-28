namespace SRegulV2
{
    partial class FRue
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FRue));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tBRue = new System.Windows.Forms.TextBox();
            this.listView1 = new System.Windows.Forms.ListView();
            this.label1 = new System.Windows.Forms.Label();
            this.tBNomRue = new System.Windows.Forms.TextBox();
            this.tBCodePostal = new System.Windows.Forms.TextBox();
            this.lIdRue = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.bAjouter = new System.Windows.Forms.Button();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.bSupprimer = new System.Windows.Forms.Button();
            this.bExit = new System.Windows.Forms.Button();
            this.bValider = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tBNomCommune = new System.Windows.Forms.TextBox();
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
            this.splitContainer1.Panel1.Controls.Add(this.tBRue);
            this.splitContainer1.Panel1.Controls.Add(this.listView1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.splitContainer1.Panel2.Controls.Add(this.label1);
            this.splitContainer1.Panel2.Controls.Add(this.tBNomRue);
            this.splitContainer1.Panel2.Controls.Add(this.tBCodePostal);
            this.splitContainer1.Panel2.Controls.Add(this.lIdRue);
            this.splitContainer1.Panel2.Controls.Add(this.label3);
            this.splitContainer1.Panel2.Controls.Add(this.bAjouter);
            this.splitContainer1.Panel2.Controls.Add(this.bSupprimer);
            this.splitContainer1.Panel2.Controls.Add(this.bExit);
            this.splitContainer1.Panel2.Controls.Add(this.bValider);
            this.splitContainer1.Panel2.Controls.Add(this.label5);
            this.splitContainer1.Panel2.Controls.Add(this.label2);
            this.splitContainer1.Panel2.Controls.Add(this.tBNomCommune);
            this.splitContainer1.Panel2.Controls.Add(this.bCancel);
            this.splitContainer1.Size = new System.Drawing.Size(1038, 348);
            this.splitContainer1.SplitterDistance = 594;
            this.splitContainer1.TabIndex = 4;
            // 
            // tBRue
            // 
            this.tBRue.BackColor = System.Drawing.SystemColors.ControlDark;
            this.tBRue.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tBRue.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tBRue.ForeColor = System.Drawing.SystemColors.Window;
            this.tBRue.Location = new System.Drawing.Point(13, 13);
            this.tBRue.Margin = new System.Windows.Forms.Padding(4);
            this.tBRue.Name = "tBRue";
            this.tBRue.Size = new System.Drawing.Size(379, 15);
            this.tBRue.TabIndex = 10;
            this.tBRue.TextChanged += new System.EventHandler(this.tBRue_TextChanged);
            // 
            // listView1
            // 
            this.listView1.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.listView1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listView1.ForeColor = System.Drawing.SystemColors.Window;
            this.listView1.FullRowSelect = true;
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(0, 44);
            this.listView1.MultiSelect = false;
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(594, 304);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.List;
            this.listView1.DoubleClick += new System.EventHandler(this.listView1_DoubleClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(43, 89);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 16);
            this.label1.TabIndex = 27;
            this.label1.Text = "Nom rue :";
            // 
            // tBNomRue
            // 
            this.tBNomRue.BackColor = System.Drawing.SystemColors.ControlDark;
            this.tBNomRue.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tBNomRue.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tBNomRue.ForeColor = System.Drawing.SystemColors.Window;
            this.tBNomRue.Location = new System.Drawing.Point(114, 89);
            this.tBNomRue.Name = "tBNomRue";
            this.tBNomRue.Size = new System.Drawing.Size(314, 15);
            this.tBNomRue.TabIndex = 1;
            this.tBNomRue.Click += new System.EventHandler(this.tBNomRue_Click);
            // 
            // tBCodePostal
            // 
            this.tBCodePostal.BackColor = System.Drawing.SystemColors.ControlDark;
            this.tBCodePostal.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tBCodePostal.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tBCodePostal.ForeColor = System.Drawing.SystemColors.Window;
            this.tBCodePostal.Location = new System.Drawing.Point(114, 158);
            this.tBCodePostal.Name = "tBCodePostal";
            this.tBCodePostal.Size = new System.Drawing.Size(96, 15);
            this.tBCodePostal.TabIndex = 3;
            this.tBCodePostal.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tBCodePostal_KeyDown);
            this.tBCodePostal.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tBCodePostal_KeyPress);
            // 
            // lIdRue
            // 
            this.lIdRue.AutoSize = true;
            this.lIdRue.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lIdRue.Location = new System.Drawing.Point(369, 52);
            this.lIdRue.Name = "lIdRue";
            this.lIdRue.Size = new System.Drawing.Size(29, 16);
            this.lIdRue.TabIndex = 25;
            this.lIdRue.Text = "999";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(310, 52);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 16);
            this.label3.TabIndex = 24;
            this.label3.Text = "Id Rue :";
            // 
            // bAjouter
            // 
            this.bAjouter.FlatAppearance.BorderSize = 0;
            this.bAjouter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bAjouter.ImageIndex = 0;
            this.bAjouter.ImageList = this.imageList1;
            this.bAjouter.Location = new System.Drawing.Point(140, 249);
            this.bAjouter.Name = "bAjouter";
            this.bAjouter.Size = new System.Drawing.Size(64, 64);
            this.bAjouter.TabIndex = 4;
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
            this.bSupprimer.Location = new System.Drawing.Point(39, 249);
            this.bSupprimer.Name = "bSupprimer";
            this.bSupprimer.Size = new System.Drawing.Size(64, 64);
            this.bSupprimer.TabIndex = 8;
            this.bSupprimer.UseVisualStyleBackColor = true;
            this.bSupprimer.Click += new System.EventHandler(this.bSupprimer_Click);
            // 
            // bExit
            // 
            this.bExit.FlatAppearance.BorderSize = 0;
            this.bExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bExit.ImageIndex = 7;
            this.bExit.ImageList = this.imageList1;
            this.bExit.Location = new System.Drawing.Point(334, 249);
            this.bExit.Name = "bExit";
            this.bExit.Size = new System.Drawing.Size(64, 64);
            this.bExit.TabIndex = 6;
            this.bExit.UseVisualStyleBackColor = true;
            this.bExit.Click += new System.EventHandler(this.bExit_Click);
            // 
            // bValider
            // 
            this.bValider.FlatAppearance.BorderSize = 0;
            this.bValider.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bValider.ImageIndex = 4;
            this.bValider.ImageList = this.imageList1;
            this.bValider.Location = new System.Drawing.Point(241, 249);
            this.bValider.Name = "bValider";
            this.bValider.Size = new System.Drawing.Size(64, 64);
            this.bValider.TabIndex = 5;
            this.bValider.UseVisualStyleBackColor = true;
            this.bValider.Click += new System.EventHandler(this.bValider_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(21, 158);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(87, 16);
            this.label5.TabIndex = 6;
            this.label5.Text = "Code postal :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(3, 122);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(105, 16);
            this.label2.TabIndex = 3;
            this.label2.Text = "Nom commune :";
            // 
            // tBNomCommune
            // 
            this.tBNomCommune.BackColor = System.Drawing.SystemColors.ControlDark;
            this.tBNomCommune.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tBNomCommune.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tBNomCommune.ForeColor = System.Drawing.SystemColors.Window;
            this.tBNomCommune.Location = new System.Drawing.Point(114, 122);
            this.tBNomCommune.Name = "tBNomCommune";
            this.tBNomCommune.Size = new System.Drawing.Size(271, 15);
            this.tBNomCommune.TabIndex = 2;
            this.tBNomCommune.Click += new System.EventHandler(this.tBNomCommune_Click);
            // 
            // bCancel
            // 
            this.bCancel.FlatAppearance.BorderSize = 0;
            this.bCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bCancel.ImageIndex = 8;
            this.bCancel.ImageList = this.imageList1;
            this.bCancel.Location = new System.Drawing.Point(140, 249);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(64, 64);
            this.bCancel.TabIndex = 7;
            this.bCancel.UseVisualStyleBackColor = true;
            this.bCancel.Click += new System.EventHandler(this.bCancel_Click);
            // 
            // FRue
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.ClientSize = new System.Drawing.Size(1038, 348);
            this.ControlBox = false;
            this.Controls.Add(this.splitContainer1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.SystemColors.Window;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FRue";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Ajout / Modification des rues";
            this.Load += new System.EventHandler(this.FRue_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.TextBox tBCodePostal;
        private System.Windows.Forms.Label lIdRue;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button bAjouter;
        private System.Windows.Forms.Button bSupprimer;
        private System.Windows.Forms.Button bExit;
        private System.Windows.Forms.Button bValider;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tBNomCommune;
        private System.Windows.Forms.Button bCancel;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tBNomRue;
        private System.Windows.Forms.TextBox tBRue;
    }
}