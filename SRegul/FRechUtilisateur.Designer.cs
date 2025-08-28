namespace SRegulV2
{
    partial class FRechUtilisateur
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
            this.bExit = new System.Windows.Forms.Button();
            this.tBoxUtilisateur = new System.Windows.Forms.TextBox();
            this.listView1 = new System.Windows.Forms.ListView();
            this.SuspendLayout();
            // 
            // bExit
            // 
            this.bExit.BackgroundImage = global::SRegulV2.Properties.Resources.exit;
            this.bExit.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.bExit.FlatAppearance.BorderSize = 0;
            this.bExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bExit.Location = new System.Drawing.Point(264, 2);
            this.bExit.Name = "bExit";
            this.bExit.Size = new System.Drawing.Size(37, 35);
            this.bExit.TabIndex = 12;
            this.bExit.UseVisualStyleBackColor = true;
            this.bExit.Click += new System.EventHandler(this.bExit_Click);
            // 
            // tBoxUtilisateur
            // 
            this.tBoxUtilisateur.BackColor = System.Drawing.SystemColors.ControlDark;
            this.tBoxUtilisateur.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tBoxUtilisateur.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tBoxUtilisateur.ForeColor = System.Drawing.SystemColors.Window;
            this.tBoxUtilisateur.Location = new System.Drawing.Point(7, 17);
            this.tBoxUtilisateur.Margin = new System.Windows.Forms.Padding(4);
            this.tBoxUtilisateur.Name = "tBoxUtilisateur";
            this.tBoxUtilisateur.Size = new System.Drawing.Size(244, 15);
            this.tBoxUtilisateur.TabIndex = 10;
            this.tBoxUtilisateur.TextChanged += new System.EventHandler(this.tBoxUtilisateur_TextChanged);
            this.tBoxUtilisateur.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tBoxUtilisateur_KeyDown);
            // 
            // listView1
            // 
            this.listView1.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.listView1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.listView1.ForeColor = System.Drawing.SystemColors.Window;
            this.listView1.FullRowSelect = true;
            this.listView1.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.listView1.Location = new System.Drawing.Point(0, 51);
            this.listView1.Margin = new System.Windows.Forms.Padding(4);
            this.listView1.MultiSelect = false;
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(314, 380);
            this.listView1.TabIndex = 11;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.List;
            this.listView1.DoubleClick += new System.EventHandler(this.listView1_DoubleClick);
            // 
            // FRechUtilisateur
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.ClientSize = new System.Drawing.Size(314, 431);
            this.ControlBox = false;
            this.Controls.Add(this.bExit);
            this.Controls.Add(this.tBoxUtilisateur);
            this.Controls.Add(this.listView1);
            this.ForeColor = System.Drawing.SystemColors.Window;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Name = "FRechUtilisateur";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Recherche d\'un utilisateur";
            this.Load += new System.EventHandler(this.FRechUtilisateur_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button bExit;
        private System.Windows.Forms.TextBox tBoxUtilisateur;
        private System.Windows.Forms.ListView listView1;
    }
}