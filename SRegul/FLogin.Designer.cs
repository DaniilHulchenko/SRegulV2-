namespace SRegulV2
{
    partial class FLogin
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FLogin));
            this.bOk = new System.Windows.Forms.Button();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.bCancel = new System.Windows.Forms.Button();
            this.TBpass = new System.Windows.Forms.TextBox();
            this.TButilisateur = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // bOk
            // 
            this.bOk.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.bOk.FlatAppearance.BorderSize = 0;
            this.bOk.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bOk.ImageIndex = 0;
            this.bOk.ImageList = this.imageList1;
            this.bOk.Location = new System.Drawing.Point(103, 151);
            this.bOk.Name = "bOk";
            this.bOk.Size = new System.Drawing.Size(64, 64);
            this.bOk.TabIndex = 4;
            this.bOk.UseVisualStyleBackColor = true;
            this.bOk.Click += new System.EventHandler(this.bOk_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "bPasswordOn.png");
            this.imageList1.Images.SetKeyName(1, "bondCancel.png");
            // 
            // bCancel
            // 
            this.bCancel.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.bCancel.FlatAppearance.BorderSize = 0;
            this.bCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bCancel.ImageIndex = 1;
            this.bCancel.ImageList = this.imageList1;
            this.bCancel.Location = new System.Drawing.Point(207, 151);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(64, 64);
            this.bCancel.TabIndex = 5;
            this.bCancel.UseVisualStyleBackColor = true;
            this.bCancel.Click += new System.EventHandler(this.bCancel_Click);
            // 
            // TBpass
            // 
            this.TBpass.BackColor = System.Drawing.SystemColors.ControlDark;
            this.TBpass.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.TBpass.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TBpass.ForeColor = System.Drawing.SystemColors.Window;
            this.TBpass.Location = new System.Drawing.Point(167, 95);
            this.TBpass.Name = "TBpass";
            this.TBpass.PasswordChar = '*';
            this.TBpass.Size = new System.Drawing.Size(104, 15);
            this.TBpass.TabIndex = 2;
            this.TBpass.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TBpass_KeyPress);
            // 
            // TButilisateur
            // 
            this.TButilisateur.BackColor = System.Drawing.SystemColors.ControlDark;
            this.TButilisateur.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.TButilisateur.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TButilisateur.ForeColor = System.Drawing.SystemColors.Window;
            this.TButilisateur.Location = new System.Drawing.Point(167, 57);
            this.TButilisateur.Name = "TButilisateur";
            this.TButilisateur.Size = new System.Drawing.Size(104, 15);
            this.TButilisateur.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.Window;
            this.label1.Location = new System.Drawing.Point(88, 56);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 16);
            this.label1.TabIndex = 4;
            this.label1.Text = "Utilisateur :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.Window;
            this.label2.Location = new System.Drawing.Point(65, 95);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(96, 16);
            this.label2.TabIndex = 5;
            this.label2.Text = "Mot de passe :";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.SystemColors.Window;
            this.label3.Location = new System.Drawing.Point(136, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(72, 16);
            this.label3.TabIndex = 6;
            this.label3.Text = "Version du";
            // 
            // FLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.ClientSize = new System.Drawing.Size(352, 224);
            this.ControlBox = false;
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.TButilisateur);
            this.Controls.Add(this.TBpass);
            this.Controls.Add(this.bCancel);
            this.Controls.Add(this.bOk);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "FLogin";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Connexion...";
            this.Load += new System.EventHandler(this.FLogin_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button bOk;
        private System.Windows.Forms.Button bCancel;
        private System.Windows.Forms.TextBox TBpass;
        private System.Windows.Forms.TextBox TButilisateur;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ImageList imageList1;
    }
}