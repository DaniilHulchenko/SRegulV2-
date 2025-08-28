namespace CartoGeolocMedecins
{
    partial class FDialogue
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FDialogue));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.tbRue = new System.Windows.Forms.TextBox();
            this.tbCp = new System.Windows.Forms.TextBox();
            this.tbVille = new System.Windows.Forms.TextBox();
            this.tbPays = new System.Windows.Forms.TextBox();
            this.bCancel = new System.Windows.Forms.Button();
            this.bGeoloc = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(110, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(195, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Entrez l\'adresse à rechercher";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(31, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "N°, rue";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(17, 105);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Code postal";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(45, 79);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(26, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Ville";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(179, 105);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(30, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Pays";
            // 
            // tbRue
            // 
            this.tbRue.Location = new System.Drawing.Point(86, 47);
            this.tbRue.Name = "tbRue";
            this.tbRue.Size = new System.Drawing.Size(244, 20);
            this.tbRue.TabIndex = 0;
            this.tbRue.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TbVille_KeyPress);
            // 
            // tbCp
            // 
            this.tbCp.Location = new System.Drawing.Point(86, 102);
            this.tbCp.Name = "tbCp";
            this.tbCp.Size = new System.Drawing.Size(87, 20);
            this.tbCp.TabIndex = 2;
            this.tbCp.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TbVille_KeyPress);
            // 
            // tbVille
            // 
            this.tbVille.Location = new System.Drawing.Point(86, 76);
            this.tbVille.Name = "tbVille";
            this.tbVille.Size = new System.Drawing.Size(244, 20);
            this.tbVille.TabIndex = 1;
            this.tbVille.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TbVille_KeyPress);
            // 
            // tbPays
            // 
            this.tbPays.Location = new System.Drawing.Point(217, 102);
            this.tbPays.Name = "tbPays";
            this.tbPays.Size = new System.Drawing.Size(113, 20);
            this.tbPays.TabIndex = 3;
            this.tbPays.Text = "Suisse";
            this.tbPays.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TbVille_KeyPress);
            // 
            // bCancel
            // 
            this.bCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bCancel.FlatAppearance.BorderSize = 0;
            this.bCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bCancel.Image = global::Carto.Properties.Resources.Cancel;
            this.bCancel.Location = new System.Drawing.Point(74, 128);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(73, 79);
            this.bCancel.TabIndex = 5;
            this.bCancel.UseVisualStyleBackColor = true;
            // 
            // bGeoloc
            // 
            this.bGeoloc.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.bGeoloc.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.bGeoloc.FlatAppearance.BorderSize = 0;
            this.bGeoloc.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bGeoloc.Image = global::Carto.Properties.Resources.icone_loupe_terre;
            this.bGeoloc.Location = new System.Drawing.Point(255, 128);
            this.bGeoloc.Name = "bGeoloc";
            this.bGeoloc.Size = new System.Drawing.Size(75, 79);
            this.bGeoloc.TabIndex = 4;
            this.bGeoloc.UseVisualStyleBackColor = true;
            this.bGeoloc.Click += new System.EventHandler(this.bGeoloc_Click);
            // 
            // FDialogue
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(387, 213);
            this.Controls.Add(this.bCancel);
            this.Controls.Add(this.bGeoloc);
            this.Controls.Add(this.tbPays);
            this.Controls.Add(this.tbVille);
            this.Controls.Add(this.tbCp);
            this.Controls.Add(this.tbRue);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FDialogue";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Recherche d\'adresse";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbRue;
        private System.Windows.Forms.TextBox tbCp;
        private System.Windows.Forms.TextBox tbVille;
        private System.Windows.Forms.TextBox tbPays;
        private System.Windows.Forms.Button bGeoloc;
        private System.Windows.Forms.Button bCancel;
    }
}