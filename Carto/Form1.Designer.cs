namespace CartoGeolocMedecins
{
    partial class Form1
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.gmap = new GMap.NET.WindowsForms.GMapControl();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.lsbMed = new System.Windows.Forms.ListBox();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.btnItineraire = new System.Windows.Forms.Button();
            this.BQuitter = new System.Windows.Forms.Button();
            this.btnDetacherCarte = new System.Windows.Forms.Button();
            this.btnRecherche = new System.Windows.Forms.Button();
            this.btnReinitialiser = new System.Windows.Forms.Button();
            this.btnRecentrer = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.SuspendLayout();
            // 
            // gmap
            // 
            this.gmap.Bearing = 0F;
            this.gmap.CanDragMap = true;
            this.gmap.Cursor = System.Windows.Forms.Cursors.Hand;
            this.gmap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gmap.EmptyTileColor = System.Drawing.Color.Navy;
            this.gmap.GrayScaleMode = false;
            this.gmap.HelperLineOption = GMap.NET.WindowsForms.HelperLineOptions.DontShow;
            this.gmap.LevelsKeepInMemmory = 5;
            this.gmap.Location = new System.Drawing.Point(0, 0);
            this.gmap.MarkersEnabled = true;
            this.gmap.MaxZoom = 19;
            this.gmap.MinZoom = 3;
            this.gmap.MouseWheelZoomEnabled = true;
            this.gmap.MouseWheelZoomType = GMap.NET.MouseWheelZoomType.MousePositionWithoutCenter;
            this.gmap.Name = "gmap";
            this.gmap.NegativeMode = false;
            this.gmap.PolygonsEnabled = true;
            this.gmap.RetryLoadTile = 0;
            this.gmap.RoutesEnabled = true;
            this.gmap.ScaleMode = GMap.NET.WindowsForms.ScaleModes.Integer;
            this.gmap.SelectedAreaFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(65)))), ((int)(((byte)(105)))), ((int)(((byte)(225)))));
            this.gmap.ShowTileGridLines = false;
            this.gmap.Size = new System.Drawing.Size(1140, 516);
            this.gmap.TabIndex = 0;
            this.gmap.Zoom = 10D;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.lsbMed);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(1326, 590);
            this.splitContainer1.SplitterDistance = 182;
            this.splitContainer1.TabIndex = 1;
            // 
            // lsbMed
            // 
            this.lsbMed.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lsbMed.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lsbMed.FormattingEnabled = true;
            this.lsbMed.ItemHeight = 16;
            this.lsbMed.Location = new System.Drawing.Point(0, 0);
            this.lsbMed.Name = "lsbMed";
            this.lsbMed.Size = new System.Drawing.Size(182, 590);
            this.lsbMed.TabIndex = 0;
            this.lsbMed.SelectedIndexChanged += new System.EventHandler(this.LsbMed_SelectedIndexChanged);
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer2.IsSplitterFixed = true;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer2.Panel1.Controls.Add(this.btnItineraire);
            this.splitContainer2.Panel1.Controls.Add(this.BQuitter);
            this.splitContainer2.Panel1.Controls.Add(this.btnDetacherCarte);
            this.splitContainer2.Panel1.Controls.Add(this.btnRecherche);
            this.splitContainer2.Panel1.Controls.Add(this.btnReinitialiser);
            this.splitContainer2.Panel1.Controls.Add(this.btnRecentrer);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.gmap);
            this.splitContainer2.Size = new System.Drawing.Size(1140, 590);
            this.splitContainer2.SplitterDistance = 70;
            this.splitContainer2.TabIndex = 1;
            // 
            // btnItineraire
            // 
            this.btnItineraire.BackColor = System.Drawing.SystemColors.Control;
            this.btnItineraire.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnItineraire.ForeColor = System.Drawing.SystemColors.Control;
            this.btnItineraire.Image = global::Carto.Properties.Resources.icone_itinéraire3;
            this.btnItineraire.Location = new System.Drawing.Point(76, 0);
            this.btnItineraire.Name = "btnItineraire";
            this.btnItineraire.Size = new System.Drawing.Size(70, 70);
            this.btnItineraire.TabIndex = 2;
            this.toolTip1.SetToolTip(this.btnItineraire, "Créer un itinéraire");
            this.btnItineraire.UseVisualStyleBackColor = false;
            this.btnItineraire.Click += new System.EventHandler(this.btnItineraire_Click);
            // 
            // BQuitter
            // 
            this.BQuitter.BackColor = System.Drawing.SystemColors.Control;
            this.BQuitter.Dock = System.Windows.Forms.DockStyle.Right;
            this.BQuitter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BQuitter.ForeColor = System.Drawing.SystemColors.Control;
            this.BQuitter.Image = global::Carto.Properties.Resources.Cancel;
            this.BQuitter.Location = new System.Drawing.Point(1070, 0);
            this.BQuitter.Name = "BQuitter";
            this.BQuitter.Size = new System.Drawing.Size(70, 70);
            this.BQuitter.TabIndex = 1;
            this.toolTip1.SetToolTip(this.BQuitter, "Quitter");
            this.BQuitter.UseVisualStyleBackColor = false;
            this.BQuitter.Click += new System.EventHandler(this.BQuitter_Click);
            // 
            // btnDetacherCarte
            // 
            this.btnDetacherCarte.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnDetacherCarte.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDetacherCarte.Image = global::Carto.Properties.Resources.Icone_Detacher_Carte2;
            this.btnDetacherCarte.Location = new System.Drawing.Point(304, 0);
            this.btnDetacherCarte.Name = "btnDetacherCarte";
            this.btnDetacherCarte.Size = new System.Drawing.Size(70, 70);
            this.btnDetacherCarte.TabIndex = 5;
            this.toolTip1.SetToolTip(this.btnDetacherCarte, "Ouvrir la carte dans une autre fenêtre");
            this.btnDetacherCarte.UseVisualStyleBackColor = true;
            this.btnDetacherCarte.Click += new System.EventHandler(this.Button1_Click);
            // 
            // btnRecherche
            // 
            this.btnRecherche.BackColor = System.Drawing.SystemColors.Control;
            this.btnRecherche.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRecherche.ForeColor = System.Drawing.SystemColors.Control;
            this.btnRecherche.Image = global::Carto.Properties.Resources.icone_loupe_terre;
            this.btnRecherche.Location = new System.Drawing.Point(0, 0);
            this.btnRecherche.Name = "btnRecherche";
            this.btnRecherche.Size = new System.Drawing.Size(70, 70);
            this.btnRecherche.TabIndex = 0;
            this.toolTip1.SetToolTip(this.btnRecherche, "Rechercher une adresse");
            this.btnRecherche.UseVisualStyleBackColor = false;
            this.btnRecherche.Click += new System.EventHandler(this.BtnRecherche_Click);
            // 
            // btnReinitialiser
            // 
            this.btnReinitialiser.BackColor = System.Drawing.SystemColors.Control;
            this.btnReinitialiser.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReinitialiser.ForeColor = System.Drawing.SystemColors.Control;
            this.btnReinitialiser.Image = global::Carto.Properties.Resources.Icone_Rafraichir;
            this.btnReinitialiser.Location = new System.Drawing.Point(152, 0);
            this.btnReinitialiser.Name = "btnReinitialiser";
            this.btnReinitialiser.Size = new System.Drawing.Size(70, 70);
            this.btnReinitialiser.TabIndex = 4;
            this.toolTip1.SetToolTip(this.btnReinitialiser, "Réinitialiser les points");
            this.btnReinitialiser.UseVisualStyleBackColor = false;
            this.btnReinitialiser.Click += new System.EventHandler(this.BtnReinitialiser_Click);
            // 
            // btnRecentrer
            // 
            this.btnRecentrer.BackColor = System.Drawing.SystemColors.Control;
            this.btnRecentrer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRecentrer.ForeColor = System.Drawing.SystemColors.Control;
            this.btnRecentrer.Image = global::Carto.Properties.Resources.Fleches_Centre;
            this.btnRecentrer.Location = new System.Drawing.Point(228, 0);
            this.btnRecentrer.Name = "btnRecentrer";
            this.btnRecentrer.Size = new System.Drawing.Size(70, 70);
            this.btnRecentrer.TabIndex = 3;
            this.toolTip1.SetToolTip(this.btnRecentrer, "Recentrer la carte sur Genève");
            this.btnRecentrer.UseVisualStyleBackColor = false;
            this.btnRecentrer.Click += new System.EventHandler(this.brecentre_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1326, 590);
            this.Controls.Add(this.splitContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "Carte";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private GMap.NET.WindowsForms.GMapControl gmap;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button btnRecherche;
        private System.Windows.Forms.Button BQuitter;
        private System.Windows.Forms.Button btnItineraire;
        private System.Windows.Forms.Button btnRecentrer;
        private System.Windows.Forms.Button btnReinitialiser;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button btnDetacherCarte;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.ListBox lsbMed;
    }
}

