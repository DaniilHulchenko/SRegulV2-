namespace SRegulV2
{
    partial class FPassword
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FPassword));
            this.tBReP = new System.Windows.Forms.TextBox();
            this.tBNvxP = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.bValider = new System.Windows.Forms.Button();
            this.bCancel = new System.Windows.Forms.Button();
            this.bMasqueAffiche = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.tBAncienP = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // tBReP
            // 
            this.tBReP.BackColor = System.Drawing.SystemColors.ControlDark;
            this.tBReP.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tBReP.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tBReP.ForeColor = System.Drawing.SystemColors.Window;
            this.tBReP.Location = new System.Drawing.Point(175, 102);
            this.tBReP.Name = "tBReP";
            this.tBReP.PasswordChar = '*';
            this.tBReP.Size = new System.Drawing.Size(214, 22);
            this.tBReP.TabIndex = 2;
            this.tBReP.TextChanged += new System.EventHandler(this.tBReP_TextChanged);
            // 
            // tBNvxP
            // 
            this.tBNvxP.BackColor = System.Drawing.SystemColors.ControlDark;
            this.tBNvxP.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tBNvxP.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tBNvxP.ForeColor = System.Drawing.SystemColors.Window;
            this.tBNvxP.Location = new System.Drawing.Point(175, 64);
            this.tBNvxP.Name = "tBNvxP";
            this.tBNvxP.PasswordChar = '*';
            this.tBNvxP.Size = new System.Drawing.Size(214, 22);
            this.tBNvxP.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.SystemColors.Window;
            this.label4.Location = new System.Drawing.Point(91, 107);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(74, 16);
            this.label4.TabIndex = 9;
            this.label4.Text = "re-tapez le:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.SystemColors.Window;
            this.label3.Location = new System.Drawing.Point(15, 67);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(154, 16);
            this.label3.TabIndex = 8;
            this.label3.Text = "Nouveau mot de passe: ";
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
            this.imageList1.Images.SetKeyName(9, "bPasswordOn.png");
            this.imageList1.Images.SetKeyName(10, "bvoir.png");
            // 
            // bValider
            // 
            this.bValider.FlatAppearance.BorderSize = 0;
            this.bValider.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bValider.ImageIndex = 4;
            this.bValider.ImageList = this.imageList1;
            this.bValider.Location = new System.Drawing.Point(277, 155);
            this.bValider.Name = "bValider";
            this.bValider.Size = new System.Drawing.Size(64, 64);
            this.bValider.TabIndex = 11;
            this.bValider.UseVisualStyleBackColor = true;
            this.bValider.Click += new System.EventHandler(this.bValider_Click);
            // 
            // bCancel
            // 
            this.bCancel.FlatAppearance.BorderSize = 0;
            this.bCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bCancel.ImageIndex = 8;
            this.bCancel.ImageList = this.imageList1;
            this.bCancel.Location = new System.Drawing.Point(159, 155);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(64, 64);
            this.bCancel.TabIndex = 14;
            this.bCancel.UseVisualStyleBackColor = true;
            this.bCancel.Click += new System.EventHandler(this.bCancel_Click);
            // 
            // bMasqueAffiche
            // 
            this.bMasqueAffiche.FlatAppearance.BorderSize = 0;
            this.bMasqueAffiche.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bMasqueAffiche.ImageIndex = 10;
            this.bMasqueAffiche.ImageList = this.imageList1;
            this.bMasqueAffiche.Location = new System.Drawing.Point(422, 60);
            this.bMasqueAffiche.Name = "bMasqueAffiche";
            this.bMasqueAffiche.Size = new System.Drawing.Size(64, 64);
            this.bMasqueAffiche.TabIndex = 15;
            this.bMasqueAffiche.UseVisualStyleBackColor = true;
            this.bMasqueAffiche.Click += new System.EventHandler(this.bMasqueAffiche_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.Window;
            this.label1.Location = new System.Drawing.Point(30, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(137, 16);
            this.label1.TabIndex = 16;
            this.label1.Text = "Ancien mot de passe:";
            // 
            // tBAncienP
            // 
            this.tBAncienP.BackColor = System.Drawing.SystemColors.ControlDark;
            this.tBAncienP.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tBAncienP.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tBAncienP.ForeColor = System.Drawing.SystemColors.Window;
            this.tBAncienP.Location = new System.Drawing.Point(175, 28);
            this.tBAncienP.Name = "tBAncienP";
            this.tBAncienP.PasswordChar = '*';
            this.tBAncienP.ReadOnly = true;
            this.tBAncienP.Size = new System.Drawing.Size(214, 22);
            this.tBAncienP.TabIndex = 17;
            // 
            // FPassword
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.ClientSize = new System.Drawing.Size(498, 231);
            this.ControlBox = false;
            this.Controls.Add(this.tBAncienP);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.bMasqueAffiche);
            this.Controls.Add(this.bCancel);
            this.Controls.Add(this.bValider);
            this.Controls.Add(this.tBReP);
            this.Controls.Add(this.tBNvxP);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Name = "FPassword";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Changement de mot de passe";
            this.Load += new System.EventHandler(this.FPassword_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tBReP;
        private System.Windows.Forms.TextBox tBNvxP;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Button bValider;
        private System.Windows.Forms.Button bCancel;
        private System.Windows.Forms.Button bMasqueAffiche;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tBAncienP;
    }
}