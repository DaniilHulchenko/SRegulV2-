namespace CartoGeolocMedecins
{
    partial class FItineraire
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FItineraire));
            this.bCancel = new System.Windows.Forms.Button();
            this.bGeoloc = new System.Windows.Forms.Button();
            this.tbPaysDepart = new System.Windows.Forms.TextBox();
            this.tbVilleDepart = new System.Windows.Forms.TextBox();
            this.tbCpDepart = new System.Windows.Forms.TextBox();
            this.tbRueDepart = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tbPaysArrive = new System.Windows.Forms.TextBox();
            this.tbVilleArrive = new System.Windows.Forms.TextBox();
            this.tbCpArrive = new System.Windows.Forms.TextBox();
            this.tbRueArrive = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label10 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // bCancel
            // 
            this.bCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bCancel.FlatAppearance.BorderSize = 0;
            this.bCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bCancel.Image = global::Carto.Properties.Resources.Cancel;
            this.bCancel.Location = new System.Drawing.Point(16, 305);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(73, 79);
            this.bCancel.TabIndex = 10;
            this.bCancel.UseVisualStyleBackColor = true;
            // 
            // bGeoloc
            // 
            this.bGeoloc.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.bGeoloc.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.bGeoloc.FlatAppearance.BorderSize = 0;
            this.bGeoloc.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bGeoloc.Image = global::Carto.Properties.Resources.icone_itinéraire3;
            this.bGeoloc.Location = new System.Drawing.Point(283, 305);
            this.bGeoloc.Name = "bGeoloc";
            this.bGeoloc.Size = new System.Drawing.Size(75, 79);
            this.bGeoloc.TabIndex = 9;
            this.bGeoloc.UseVisualStyleBackColor = true;
            this.bGeoloc.Click += new System.EventHandler(this.BGeoloc_Click);
            this.bGeoloc.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.BGeoloc_KeyPress);
            // 
            // tbPaysDepart
            // 
            this.tbPaysDepart.Location = new System.Drawing.Point(214, 102);
            this.tbPaysDepart.Name = "tbPaysDepart";
            this.tbPaysDepart.Size = new System.Drawing.Size(113, 20);
            this.tbPaysDepart.TabIndex = 4;
            this.tbPaysDepart.Text = "Suisse";
            this.tbPaysDepart.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.BGeoloc_KeyPress);
            // 
            // tbVilleDepart
            // 
            this.tbVilleDepart.Location = new System.Drawing.Point(83, 76);
            this.tbVilleDepart.Name = "tbVilleDepart";
            this.tbVilleDepart.Size = new System.Drawing.Size(244, 20);
            this.tbVilleDepart.TabIndex = 2;
            this.tbVilleDepart.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.BGeoloc_KeyPress);
            // 
            // tbCpDepart
            // 
            this.tbCpDepart.Location = new System.Drawing.Point(83, 102);
            this.tbCpDepart.Name = "tbCpDepart";
            this.tbCpDepart.Size = new System.Drawing.Size(87, 20);
            this.tbCpDepart.TabIndex = 3;
            this.tbCpDepart.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.BGeoloc_KeyPress);
            // 
            // tbRueDepart
            // 
            this.tbRueDepart.Location = new System.Drawing.Point(83, 47);
            this.tbRueDepart.Name = "tbRueDepart";
            this.tbRueDepart.Size = new System.Drawing.Size(244, 20);
            this.tbRueDepart.TabIndex = 1;
            this.tbRueDepart.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.BGeoloc_KeyPress);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(176, 105);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(30, 13);
            this.label5.TabIndex = 15;
            this.label5.Text = "Pays";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(42, 79);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(26, 13);
            this.label4.TabIndex = 13;
            this.label4.Text = "Ville";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 105);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "Code postal";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(28, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "N°, rue";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(135, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(125, 17);
            this.label1.TabIndex = 7;
            this.label1.Text = "Adresse de départ";
            // 
            // tbPaysArrive
            // 
            this.tbPaysArrive.Location = new System.Drawing.Point(207, 102);
            this.tbPaysArrive.Name = "tbPaysArrive";
            this.tbPaysArrive.Size = new System.Drawing.Size(113, 20);
            this.tbPaysArrive.TabIndex = 8;
            this.tbPaysArrive.Text = "Suisse";
            this.tbPaysArrive.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.BGeoloc_KeyPress);
            // 
            // tbVilleArrive
            // 
            this.tbVilleArrive.Location = new System.Drawing.Point(76, 76);
            this.tbVilleArrive.Name = "tbVilleArrive";
            this.tbVilleArrive.Size = new System.Drawing.Size(244, 20);
            this.tbVilleArrive.TabIndex = 6;
            this.tbVilleArrive.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.BGeoloc_KeyPress);
            // 
            // tbCpArrive
            // 
            this.tbCpArrive.Location = new System.Drawing.Point(76, 102);
            this.tbCpArrive.Name = "tbCpArrive";
            this.tbCpArrive.Size = new System.Drawing.Size(87, 20);
            this.tbCpArrive.TabIndex = 7;
            this.tbCpArrive.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.BGeoloc_KeyPress);
            // 
            // tbRueArrive
            // 
            this.tbRueArrive.Location = new System.Drawing.Point(76, 47);
            this.tbRueArrive.Name = "tbRueArrive";
            this.tbRueArrive.Size = new System.Drawing.Size(244, 20);
            this.tbRueArrive.TabIndex = 5;
            this.tbRueArrive.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.BGeoloc_KeyPress);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(169, 105);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(30, 13);
            this.label6.TabIndex = 25;
            this.label6.Text = "Pays";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(35, 79);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(26, 13);
            this.label7.TabIndex = 24;
            this.label7.Text = "Ville";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(7, 105);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(63, 13);
            this.label8.TabIndex = 22;
            this.label8.Text = "Code postal";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(21, 50);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(40, 13);
            this.label9.TabIndex = 20;
            this.label9.Text = "N°, rue";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.tbPaysArrive);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.tbVilleArrive);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.tbCpArrive);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.tbRueArrive);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(16, 155);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(342, 144);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(117, 16);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(119, 17);
            this.label10.TabIndex = 26;
            this.label10.Text = "Adresse d\'arrivée";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.tbPaysDepart);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.tbVilleDepart);
            this.groupBox2.Controls.Add(this.tbRueDepart);
            this.groupBox2.Controls.Add(this.tbCpDepart);
            this.groupBox2.Location = new System.Drawing.Point(16, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(342, 144);
            this.groupBox2.TabIndex = 11;
            this.groupBox2.TabStop = false;
            // 
            // FItineraire
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(380, 395);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.bCancel);
            this.Controls.Add(this.bGeoloc);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FItineraire";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Itinéraire";
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.BGeoloc_KeyPress);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button bCancel;
        private System.Windows.Forms.Button bGeoloc;
        private System.Windows.Forms.TextBox tbPaysDepart;
        private System.Windows.Forms.TextBox tbVilleDepart;
        private System.Windows.Forms.TextBox tbCpDepart;
        private System.Windows.Forms.TextBox tbRueDepart;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbPaysArrive;
        private System.Windows.Forms.TextBox tbVilleArrive;
        private System.Windows.Forms.TextBox tbCpArrive;
        private System.Windows.Forms.TextBox tbRueArrive;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.GroupBox groupBox2;
    }
}