
namespace SRegulV2
{
    partial class FDebloqueMed
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FDebloqueMed));
            this.listViewMed = new System.Windows.Forms.ListView();
            this.label12 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tBoxMedecin = new System.Windows.Forms.TextBox();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.bFermer = new System.Windows.Forms.Button();
            this.bDebloquer = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // listViewMed
            // 
            this.listViewMed.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.listViewMed.ForeColor = System.Drawing.SystemColors.Window;
            this.listViewMed.FullRowSelect = true;
            this.listViewMed.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.listViewMed.HideSelection = false;
            this.listViewMed.LabelWrap = false;
            this.listViewMed.Location = new System.Drawing.Point(0, 43);
            this.listViewMed.Margin = new System.Windows.Forms.Padding(4);
            this.listViewMed.MultiSelect = false;
            this.listViewMed.Name = "listViewMed";
            this.listViewMed.Size = new System.Drawing.Size(199, 212);
            this.listViewMed.TabIndex = 1;
            this.listViewMed.UseCompatibleStateImageBehavior = false;
            this.listViewMed.View = System.Windows.Forms.View.Details;
            this.listViewMed.DoubleClick += new System.EventHandler(this.listViewMed_DoubleClick);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.ForeColor = System.Drawing.SystemColors.Window;
            this.label12.Location = new System.Drawing.Point(30, 14);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(117, 16);
            this.label12.TabIndex = 14;
            this.label12.Text = "Médecin en garde";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.Window;
            this.label1.Location = new System.Drawing.Point(233, 65);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(98, 16);
            this.label1.TabIndex = 15;
            this.label1.Text = "Médecin choisi";
            // 
            // tBoxMedecin
            // 
            this.tBoxMedecin.BackColor = System.Drawing.SystemColors.ControlDark;
            this.tBoxMedecin.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tBoxMedecin.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tBoxMedecin.ForeColor = System.Drawing.SystemColors.Window;
            this.tBoxMedecin.Location = new System.Drawing.Point(337, 66);
            this.tBoxMedecin.MaxLength = 30;
            this.tBoxMedecin.Name = "tBoxMedecin";
            this.tBoxMedecin.Size = new System.Drawing.Size(138, 15);
            this.tBoxMedecin.TabIndex = 16;
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "bExitOn.png");
            this.imageList1.Images.SetKeyName(1, "cadenasVert.png");
            // 
            // bFermer
            // 
            this.bFermer.FlatAppearance.BorderSize = 0;
            this.bFermer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bFermer.ImageIndex = 0;
            this.bFermer.ImageList = this.imageList1;
            this.bFermer.Location = new System.Drawing.Point(480, 183);
            this.bFermer.Name = "bFermer";
            this.bFermer.Size = new System.Drawing.Size(64, 64);
            this.bFermer.TabIndex = 17;
            this.bFermer.UseVisualStyleBackColor = true;
            this.bFermer.Click += new System.EventHandler(this.bFermer_Click);
            // 
            // bDebloquer
            // 
            this.bDebloquer.FlatAppearance.BorderSize = 0;
            this.bDebloquer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bDebloquer.ImageIndex = 1;
            this.bDebloquer.ImageList = this.imageList1;
            this.bDebloquer.Location = new System.Drawing.Point(324, 126);
            this.bDebloquer.Name = "bDebloquer";
            this.bDebloquer.Size = new System.Drawing.Size(64, 64);
            this.bDebloquer.TabIndex = 18;
            this.toolTip1.SetToolTip(this.bDebloquer, "Débloquer le médecin");
            this.bDebloquer.UseVisualStyleBackColor = true;
            this.bDebloquer.Click += new System.EventHandler(this.bDebloquer_Click);
            // 
            // toolTip1
            // 
            this.toolTip1.AutomaticDelay = 250;
            this.toolTip1.AutoPopDelay = 3500;
            this.toolTip1.InitialDelay = 250;
            this.toolTip1.IsBalloon = true;
            this.toolTip1.ReshowDelay = 50;
            // 
            // FDebloqueMed
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.ClientSize = new System.Drawing.Size(568, 259);
            this.ControlBox = false;
            this.Controls.Add(this.bDebloquer);
            this.Controls.Add(this.bFermer);
            this.Controls.Add(this.tBoxMedecin);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.listViewMed);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MinimizeBox = false;
            this.Name = "FDebloqueMed";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Débloque un médecin";
            this.Load += new System.EventHandler(this.FDebloqueMed_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView listViewMed;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tBoxMedecin;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Button bFermer;
        private System.Windows.Forms.Button bDebloquer;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}