namespace SRegulV2
{
    partial class FRechProv
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
            this.listView1 = new System.Windows.Forms.ListView();
            this.tBProvenance = new System.Windows.Forms.TextBox();
            this.bExit = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listView1
            // 
            this.listView1.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.listView1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.listView1.ForeColor = System.Drawing.SystemColors.Window;
            this.listView1.FullRowSelect = true;
            this.listView1.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.listView1.Location = new System.Drawing.Point(0, 59);
            this.listView1.Margin = new System.Windows.Forms.Padding(4);
            this.listView1.MultiSelect = false;
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(337, 283);
            this.listView1.TabIndex = 1;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.List;
            this.listView1.DoubleClick += new System.EventHandler(this.listView1_DoubleClick);
            // 
            // tBProvenance
            // 
            this.tBProvenance.BackColor = System.Drawing.SystemColors.ControlDark;
            this.tBProvenance.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tBProvenance.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tBProvenance.ForeColor = System.Drawing.SystemColors.Window;
            this.tBProvenance.Location = new System.Drawing.Point(13, 13);
            this.tBProvenance.Margin = new System.Windows.Forms.Padding(4);
            this.tBProvenance.Name = "tBProvenance";
            this.tBProvenance.Size = new System.Drawing.Size(244, 15);
            this.tBProvenance.TabIndex = 0;
            this.tBProvenance.TextChanged += new System.EventHandler(this.tBProvenance_TextChanged);
            this.tBProvenance.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tBProvenance_KeyDown);
            // 
            // bExit
            // 
            this.bExit.BackgroundImage = global::SRegulV2.Properties.Resources.exit;
            this.bExit.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.bExit.FlatAppearance.BorderSize = 0;
            this.bExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bExit.Location = new System.Drawing.Point(280, 6);
            this.bExit.Name = "bExit";
            this.bExit.Size = new System.Drawing.Size(37, 35);
            this.bExit.TabIndex = 6;
            this.bExit.UseVisualStyleBackColor = true;
            this.bExit.Click += new System.EventHandler(this.bExit_Click);
            // 
            // FRechProv
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.ClientSize = new System.Drawing.Size(337, 342);
            this.ControlBox = false;
            this.Controls.Add(this.bExit);
            this.Controls.Add(this.tBProvenance);
            this.Controls.Add(this.listView1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FRechProv";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Provenance";
            this.Load += new System.EventHandler(this.FRecherche_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.TextBox tBProvenance;
        private System.Windows.Forms.Button bExit;
    }
}