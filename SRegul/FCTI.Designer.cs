namespace SRegulV2
{
    partial class FCTI
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FCTI));
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.bValider = new System.Windows.Forms.Button();
            this.rBdesactive = new System.Windows.Forms.RadioButton();
            this.rBActive = new System.Windows.Forms.RadioButton();
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
            this.imageList1.Images.SetKeyName(9, "bPasswordOn.png");
            // 
            // bValider
            // 
            this.bValider.FlatAppearance.BorderSize = 0;
            this.bValider.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bValider.ImageIndex = 4;
            this.bValider.ImageList = this.imageList1;
            this.bValider.Location = new System.Drawing.Point(78, 144);
            this.bValider.Name = "bValider";
            this.bValider.Size = new System.Drawing.Size(64, 64);
            this.bValider.TabIndex = 11;
            this.bValider.UseVisualStyleBackColor = true;
            this.bValider.Click += new System.EventHandler(this.bValider_Click);
            // 
            // rBdesactive
            // 
            this.rBdesactive.AutoSize = true;
            this.rBdesactive.FlatAppearance.BorderSize = 0;
            this.rBdesactive.FlatAppearance.CheckedBackColor = System.Drawing.Color.Red;
            this.rBdesactive.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rBdesactive.ForeColor = System.Drawing.SystemColors.Window;
            this.rBdesactive.Location = new System.Drawing.Point(47, 87);
            this.rBdesactive.Name = "rBdesactive";
            this.rBdesactive.Size = new System.Drawing.Size(129, 20);
            this.rBdesactive.TabIndex = 30;
            this.rBdesactive.Text = "Désactiver le CTI";
            this.rBdesactive.UseVisualStyleBackColor = true;
            // 
            // rBActive
            // 
            this.rBActive.AutoSize = true;
            this.rBActive.Checked = true;
            this.rBActive.FlatAppearance.BorderSize = 0;
            this.rBActive.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.rBActive.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rBActive.ForeColor = System.Drawing.SystemColors.Window;
            this.rBActive.Location = new System.Drawing.Point(47, 44);
            this.rBActive.Name = "rBActive";
            this.rBActive.Size = new System.Drawing.Size(105, 20);
            this.rBActive.TabIndex = 29;
            this.rBActive.TabStop = true;
            this.rBActive.Text = "Activer le CTI";
            this.rBActive.UseVisualStyleBackColor = true;
            // 
            // FCTI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.ClientSize = new System.Drawing.Size(220, 225);
            this.ControlBox = false;
            this.Controls.Add(this.rBdesactive);
            this.Controls.Add(this.rBActive);
            this.Controls.Add(this.bValider);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Name = "FCTI";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Activation du CTI";
            this.Load += new System.EventHandler(this.FCTI_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Button bValider;
        private System.Windows.Forms.RadioButton rBdesactive;
        private System.Windows.Forms.RadioButton rBActive;
    }
}