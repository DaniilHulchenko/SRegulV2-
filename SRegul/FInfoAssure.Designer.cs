
namespace SRegulV2
{
    partial class FInfoAssure
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FInfoAssure));
            this.rTBoxInfos = new System.Windows.Forms.RichTextBox();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.bExporter = new System.Windows.Forms.Button();
            this.bExit = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // rTBoxInfos
            // 
            this.rTBoxInfos.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.rTBoxInfos.Dock = System.Windows.Forms.DockStyle.Top;
            this.rTBoxInfos.ForeColor = System.Drawing.SystemColors.Window;
            this.rTBoxInfos.Location = new System.Drawing.Point(0, 0);
            this.rTBoxInfos.Margin = new System.Windows.Forms.Padding(4);
            this.rTBoxInfos.Name = "rTBoxInfos";
            this.rTBoxInfos.Size = new System.Drawing.Size(620, 293);
            this.rTBoxInfos.TabIndex = 0;
            this.rTBoxInfos.Text = "";
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "bExitOn.png");
            this.imageList1.Images.SetKeyName(1, "bExportBleu.png");
            // 
            // bExporter
            // 
            this.bExporter.FlatAppearance.BorderSize = 0;
            this.bExporter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bExporter.ImageIndex = 1;
            this.bExporter.ImageList = this.imageList1;
            this.bExporter.Location = new System.Drawing.Point(49, 306);
            this.bExporter.Name = "bExporter";
            this.bExporter.Size = new System.Drawing.Size(64, 64);
            this.bExporter.TabIndex = 13;
            this.toolTip1.SetToolTip(this.bExporter, "Exporte les données vers la fiche");
            this.bExporter.UseVisualStyleBackColor = true;
            this.bExporter.Click += new System.EventHandler(this.bExporter_Click);
            // 
            // bExit
            // 
            this.bExit.FlatAppearance.BorderSize = 0;
            this.bExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bExit.ImageIndex = 0;
            this.bExit.ImageList = this.imageList1;
            this.bExit.Location = new System.Drawing.Point(511, 306);
            this.bExit.Name = "bExit";
            this.bExit.Size = new System.Drawing.Size(64, 64);
            this.bExit.TabIndex = 14;
            this.toolTip1.SetToolTip(this.bExit, "ferme la fiche");
            this.bExit.UseVisualStyleBackColor = true;
            this.bExit.Click += new System.EventHandler(this.bExit_Click);
            // 
            // FInfoAssure
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.ClientSize = new System.Drawing.Size(620, 382);
            this.ControlBox = false;
            this.Controls.Add(this.bExit);
            this.Controls.Add(this.bExporter);
            this.Controls.Add(this.rTBoxInfos);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.SystemColors.Window;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FInfoAssure";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Information sur l\'assurance du patient";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox rTBoxInfos;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Button bExporter;
        private System.Windows.Forms.Button bExit;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}