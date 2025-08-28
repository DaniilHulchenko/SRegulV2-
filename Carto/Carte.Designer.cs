namespace CartoGeolocMedecins
{
    partial class Carte
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Carte));
            this.gmap2 = new GMap.NET.WindowsForms.GMapControl();
            this.SuspendLayout();
            // 
            // gmap2
            // 
            this.gmap2.Bearing = 0F;
            this.gmap2.CanDragMap = true;
            this.gmap2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gmap2.EmptyTileColor = System.Drawing.Color.Navy;
            this.gmap2.GrayScaleMode = false;
            this.gmap2.HelperLineOption = GMap.NET.WindowsForms.HelperLineOptions.DontShow;
            this.gmap2.LevelsKeepInMemmory = 5;
            this.gmap2.Location = new System.Drawing.Point(0, 0);
            this.gmap2.MarkersEnabled = true;
            this.gmap2.MaxZoom = 17;
            this.gmap2.MinZoom = 4;
            this.gmap2.MouseWheelZoomEnabled = true;
            this.gmap2.MouseWheelZoomType = GMap.NET.MouseWheelZoomType.MousePositionAndCenter;
            this.gmap2.Name = "gmap2";
            this.gmap2.NegativeMode = false;
            this.gmap2.PolygonsEnabled = true;
            this.gmap2.RetryLoadTile = 0;
            this.gmap2.RoutesEnabled = true;
            this.gmap2.ScaleMode = GMap.NET.WindowsForms.ScaleModes.Integer;
            this.gmap2.SelectedAreaFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(65)))), ((int)(((byte)(105)))), ((int)(((byte)(225)))));
            this.gmap2.ShowTileGridLines = false;
            this.gmap2.Size = new System.Drawing.Size(1067, 507);
            this.gmap2.TabIndex = 0;
            this.gmap2.Zoom = 6D;
            // 
            // Carte
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1067, 507);
            this.Controls.Add(this.gmap2);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Carte";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Carte";
            this.Load += new System.EventHandler(this.Carte_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private GMap.NET.WindowsForms.GMapControl gmap2;
    }
}