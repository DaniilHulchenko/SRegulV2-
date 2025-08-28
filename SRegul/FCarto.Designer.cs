namespace SRegulV2
{
    partial class FCarto
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        ///<param name="disposing">true if managed resources should be disposed; otherwise, false.</param>        
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FCarto));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.bSatellite = new System.Windows.Forms.Button();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.bCarteAvTraffic = new System.Windows.Forms.Button();
            this.bCarteSStrafic = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.bRafraichi = new System.Windows.Forms.Button();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.bSatellite);
            this.splitContainer1.Panel1.Controls.Add(this.bCarteAvTraffic);
            this.splitContainer1.Panel1.Controls.Add(this.bCarteSStrafic);
            this.splitContainer1.Panel1.Controls.Add(this.pictureBox1);
            this.splitContainer1.Panel1.Controls.Add(this.bRafraichi);
            this.splitContainer1.Size = new System.Drawing.Size(1152, 596);
            this.splitContainer1.SplitterDistance = 81;
            this.splitContainer1.TabIndex = 1;
            // 
            // bSatellite
            // 
            this.bSatellite.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.bSatellite.FlatAppearance.BorderSize = 0;
            this.bSatellite.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bSatellite.ImageIndex = 3;
            this.bSatellite.ImageList = this.imageList1;
            this.bSatellite.Location = new System.Drawing.Point(729, 11);
            this.bSatellite.Name = "bSatellite";
            this.bSatellite.Size = new System.Drawing.Size(64, 64);
            this.bSatellite.TabIndex = 11;
            this.toolTip1.SetToolTip(this.bSatellite, "Vue Satellite");
            this.bSatellite.UseVisualStyleBackColor = true;
            this.bSatellite.Click += new System.EventHandler(this.bSatellite_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "bRefresh.png");
            this.imageList1.Images.SetKeyName(1, "bTrafficOn.png");
            this.imageList1.Images.SetKeyName(2, "bTrafficOff.png");
            this.imageList1.Images.SetKeyName(3, "bSatellite.png");
            // 
            // bCarteAvTraffic
            // 
            this.bCarteAvTraffic.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.bCarteAvTraffic.FlatAppearance.BorderSize = 0;
            this.bCarteAvTraffic.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bCarteAvTraffic.ImageIndex = 1;
            this.bCarteAvTraffic.ImageList = this.imageList1;
            this.bCarteAvTraffic.Location = new System.Drawing.Point(593, 11);
            this.bCarteAvTraffic.Name = "bCarteAvTraffic";
            this.bCarteAvTraffic.Size = new System.Drawing.Size(64, 64);
            this.bCarteAvTraffic.TabIndex = 10;
            this.toolTip1.SetToolTip(this.bCarteAvTraffic, "Carte avec le traffic routier");
            this.bCarteAvTraffic.UseVisualStyleBackColor = true;
            this.bCarteAvTraffic.Click += new System.EventHandler(this.bCarteAvTraffic_Click);
            // 
            // bCarteSStrafic
            // 
            this.bCarteSStrafic.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.bCarteSStrafic.FlatAppearance.BorderSize = 0;
            this.bCarteSStrafic.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bCarteSStrafic.ImageIndex = 2;
            this.bCarteSStrafic.ImageList = this.imageList1;
            this.bCarteSStrafic.Location = new System.Drawing.Point(461, 11);
            this.bCarteSStrafic.Name = "bCarteSStrafic";
            this.bCarteSStrafic.Size = new System.Drawing.Size(64, 64);
            this.bCarteSStrafic.TabIndex = 9;
            this.toolTip1.SetToolTip(this.bCarteSStrafic, "Carte sans le traffic routier");
            this.bCarteSStrafic.UseVisualStyleBackColor = true;
            this.bCarteSStrafic.Click += new System.EventHandler(this.bCarteSStrafic_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.Enabled = false;
            this.pictureBox1.Image = global::SRegulV2.Properties.Resources.Gif_refresh_bleu;
            this.pictureBox1.Location = new System.Drawing.Point(94, 11);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(64, 64);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 8;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Visible = false;
            // 
            // bRafraichi
            // 
            this.bRafraichi.FlatAppearance.BorderSize = 0;
            this.bRafraichi.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bRafraichi.ImageIndex = 0;
            this.bRafraichi.ImageList = this.imageList1;
            this.bRafraichi.Location = new System.Drawing.Point(12, 11);
            this.bRafraichi.Name = "bRafraichi";
            this.bRafraichi.Size = new System.Drawing.Size(64, 64);
            this.bRafraichi.TabIndex = 7;
            this.toolTip1.SetToolTip(this.bRafraichi, "Rafraichir la carte");
            this.bRafraichi.UseVisualStyleBackColor = true;
            this.bRafraichi.Click += new System.EventHandler(this.bRafraichi_Click);
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 5000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // toolTip1
            // 
            this.toolTip1.AutoPopDelay = 5000;
            this.toolTip1.InitialDelay = 250;
            this.toolTip1.IsBalloon = true;
            this.toolTip1.ReshowDelay = 100;
            // 
            // FCarto
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.ClientSize = new System.Drawing.Size(1152, 596);
            this.ControlBox = false;
            this.Controls.Add(this.splitContainer1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.SystemColors.Window;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FCarto";
            this.Text = "Carte";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FCarto_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FCarto_FormClosed);
            this.Load += new System.EventHandler(this.FCarto_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        //private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Button bRafraichi;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button bCarteSStrafic;
        private System.Windows.Forms.Button bCarteAvTraffic;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button bSatellite;
    }
}