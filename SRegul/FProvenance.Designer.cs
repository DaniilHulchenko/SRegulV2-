namespace SRegulV2
{
    partial class FProvenance
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FProvenance));
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.listView1 = new System.Windows.Forms.ListView();
            this.tBCodeProvenance = new System.Windows.Forms.TextBox();
            this.lIdProvenance = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.bAjouter = new System.Windows.Forms.Button();
            this.bSupprimer = new System.Windows.Forms.Button();
            this.bExit = new System.Windows.Forms.Button();
            this.bValider = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tBProvenance = new System.Windows.Forms.TextBox();
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
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.listView1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.splitContainer1.Panel2.Controls.Add(this.tBCodeProvenance);
            this.splitContainer1.Panel2.Controls.Add(this.lIdProvenance);
            this.splitContainer1.Panel2.Controls.Add(this.label3);
            this.splitContainer1.Panel2.Controls.Add(this.bAjouter);
            this.splitContainer1.Panel2.Controls.Add(this.bSupprimer);
            this.splitContainer1.Panel2.Controls.Add(this.bExit);
            this.splitContainer1.Panel2.Controls.Add(this.bValider);
            this.splitContainer1.Panel2.Controls.Add(this.label5);
            this.splitContainer1.Panel2.Controls.Add(this.label2);
            this.splitContainer1.Panel2.Controls.Add(this.tBProvenance);
            this.splitContainer1.Panel2.Controls.Add(this.bCancel);
            this.splitContainer1.Size = new System.Drawing.Size(909, 346);
            this.splitContainer1.SplitterDistance = 400;
            this.splitContainer1.SplitterWidth = 5;
            this.splitContainer1.TabIndex = 4;
            // 
            // listView1
            // 
            this.listView1.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listView1.ForeColor = System.Drawing.SystemColors.Window;
            this.listView1.FullRowSelect = true;
            this.listView1.Location = new System.Drawing.Point(0, 0);
            this.listView1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.listView1.MultiSelect = false;
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(400, 346);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.List;
            this.listView1.DoubleClick += new System.EventHandler(this.listView1_DoubleClick);
            // 
            // tBCodeProvenance
            // 
            this.tBCodeProvenance.BackColor = System.Drawing.SystemColors.ControlDark;
            this.tBCodeProvenance.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tBCodeProvenance.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tBCodeProvenance.ForeColor = System.Drawing.SystemColors.Window;
            this.tBCodeProvenance.Location = new System.Drawing.Point(165, 141);
            this.tBCodeProvenance.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tBCodeProvenance.Name = "tBCodeProvenance";
            this.tBCodeProvenance.Size = new System.Drawing.Size(73, 15);
            this.tBCodeProvenance.TabIndex = 2;
            this.tBCodeProvenance.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tBCodeProvenance_KeyDown);
            // 
            // lIdProvenance
            // 
            this.lIdProvenance.AutoSize = true;
            this.lIdProvenance.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lIdProvenance.ForeColor = System.Drawing.SystemColors.Window;
            this.lIdProvenance.Location = new System.Drawing.Point(358, 25);
            this.lIdProvenance.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lIdProvenance.Name = "lIdProvenance";
            this.lIdProvenance.Size = new System.Drawing.Size(29, 16);
            this.lIdProvenance.TabIndex = 25;
            this.lIdProvenance.Text = "999";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.SystemColors.Window;
            this.label3.Location = new System.Drawing.Point(249, 25);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(101, 16);
            this.label3.TabIndex = 24;
            this.label3.Text = "Id Provenance :";
            // 
            // bAjouter
            // 
            this.bAjouter.FlatAppearance.BorderSize = 0;
            this.bAjouter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bAjouter.ImageIndex = 0;
            this.bAjouter.ImageList = this.imageList1;
            this.bAjouter.Location = new System.Drawing.Point(141, 224);
            this.bAjouter.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.bAjouter.Name = "bAjouter";
            this.bAjouter.Size = new System.Drawing.Size(85, 79);
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
            this.bSupprimer.Location = new System.Drawing.Point(21, 224);
            this.bSupprimer.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.bSupprimer.Name = "bSupprimer";
            this.bSupprimer.Size = new System.Drawing.Size(85, 79);
            this.bSupprimer.TabIndex = 15;
            this.bSupprimer.UseVisualStyleBackColor = true;
            this.bSupprimer.Click += new System.EventHandler(this.bSupprimer_Click);
            // 
            // bExit
            // 
            this.bExit.FlatAppearance.BorderSize = 0;
            this.bExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bExit.ImageIndex = 7;
            this.bExit.ImageList = this.imageList1;
            this.bExit.Location = new System.Drawing.Point(396, 224);
            this.bExit.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.bExit.Name = "bExit";
            this.bExit.Size = new System.Drawing.Size(85, 79);
            this.bExit.TabIndex = 5;
            this.bExit.UseVisualStyleBackColor = true;
            this.bExit.Click += new System.EventHandler(this.bExit_Click);
            // 
            // bValider
            // 
            this.bValider.FlatAppearance.BorderSize = 0;
            this.bValider.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bValider.ImageIndex = 4;
            this.bValider.ImageList = this.imageList1;
            this.bValider.Location = new System.Drawing.Point(279, 224);
            this.bValider.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.bValider.Name = "bValider";
            this.bValider.Size = new System.Drawing.Size(85, 79);
            this.bValider.TabIndex = 4;
            this.bValider.UseVisualStyleBackColor = true;
            this.bValider.Click += new System.EventHandler(this.bValider_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.SystemColors.Window;
            this.label5.Location = new System.Drawing.Point(33, 140);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(123, 16);
            this.label5.TabIndex = 6;
            this.label5.Text = "Code Provenance :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.Window;
            this.label2.Location = new System.Drawing.Point(25, 82);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(87, 16);
            this.label2.TabIndex = 3;
            this.label2.Text = "Provenance :";
            // 
            // tBProvenance
            // 
            this.tBProvenance.BackColor = System.Drawing.SystemColors.ControlDark;
            this.tBProvenance.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tBProvenance.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.tBProvenance.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tBProvenance.ForeColor = System.Drawing.SystemColors.Window;
            this.tBProvenance.Location = new System.Drawing.Point(120, 83);
            this.tBProvenance.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tBProvenance.Name = "tBProvenance";
            this.tBProvenance.Size = new System.Drawing.Size(361, 15);
            this.tBProvenance.TabIndex = 1;
            this.tBProvenance.Click += new System.EventHandler(this.tBProvenance_Click);
            // 
            // bCancel
            // 
            this.bCancel.FlatAppearance.BorderSize = 0;
            this.bCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bCancel.ImageIndex = 8;
            this.bCancel.ImageList = this.imageList1;
            this.bCancel.Location = new System.Drawing.Point(153, 224);
            this.bCancel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(85, 79);
            this.bCancel.TabIndex = 17;
            this.bCancel.UseVisualStyleBackColor = true;
            this.bCancel.Click += new System.EventHandler(this.bCancel_Click);
            // 
            // FProvenance
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(909, 346);
            this.ControlBox = false;
            this.Controls.Add(this.splitContainer1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "FProvenance";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Provenance";
            this.Load += new System.EventHandler(this.FProvenance_Load);
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
        private System.Windows.Forms.TextBox tBCodeProvenance;
        private System.Windows.Forms.Label lIdProvenance;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button bAjouter;
        private System.Windows.Forms.Button bSupprimer;
        private System.Windows.Forms.Button bExit;
        private System.Windows.Forms.Button bValider;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tBProvenance;
        private System.Windows.Forms.Button bCancel;
    }
}