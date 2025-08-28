namespace SRegulV2
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.bRDV = new System.Windows.Forms.Button();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.bInfirmiereEtat = new System.Windows.Forms.Button();
            this.bMessage = new System.Windows.Forms.Button();
            this.lnbFicheOuverte = new System.Windows.Forms.Label();
            this.bGardes = new System.Windows.Forms.Button();
            this.LNbVisiteEnAttente = new System.Windows.Forms.Label();
            this.bCarto = new System.Windows.Forms.Button();
            this.bRafraichi = new System.Windows.Forms.Button();
            this.bFindegarde = new System.Windows.Forms.Button();
            this.bEtatMed = new System.Windows.Forms.Button();
            this.bVisiteJournaliere = new System.Windows.Forms.Button();
            this.bRechercheAppel = new System.Windows.Forms.Button();
            this.bVisiteMed = new System.Windows.Forms.Button();
            this.bvisite = new System.Windows.Forms.Button();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.listView1 = new System.Windows.Forms.ListView();
            this.listView3 = new System.Windows.Forms.ListView();
            this.splitContainer4 = new System.Windows.Forms.SplitContainer();
            this.listView2 = new System.Windows.Forms.ListView();
            this.rTBoxAideRegul = new System.Windows.Forms.RichTextBox();
            this.MenuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fichierToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.quitterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuGardeMedecins = new System.Windows.Forms.ToolStripMenuItem();
            this.appelsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rechercheVisitesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuNvxMessage = new System.Windows.Forms.ToolStripMenuItem();
            this.rechercheDeMessagesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.recherchesDiversesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuRechEvent = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuHistoAttribSmart = new System.Windows.Forms.ToolStripMenuItem();
            this.diversListesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuOrganisation = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuProvenances = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuAssurance = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuRues = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuCommune = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuGarde = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuSmartphone = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuMedecin = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuUtilisateur = new System.Windows.Forms.ToolStripMenuItem();
            this.MenutelUtil = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuMotif = new System.Windows.Forms.ToolStripMenuItem();
            this.paramètresToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Menuressaisie = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuParamUtil = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuCTI = new System.Windows.Forms.ToolStripMenuItem();
            this.MenureinitialiserAlarmes = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuParametresDivers = new System.Windows.Forms.ToolStripMenuItem();
            this.MenudebloquerMed = new System.Windows.Forms.ToolStripMenuItem();
            this.menuGestDoc = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuAPropos = new System.Windows.Forms.ToolStripMenuItem();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).BeginInit();
            this.splitContainer4.Panel1.SuspendLayout();
            this.splitContainer4.Panel2.SuspendLayout();
            this.splitContainer4.SuspendLayout();
            this.MenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.BackColor = System.Drawing.Color.Black;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 24);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.splitContainer1.Panel1.Controls.Add(this.bRDV);
            this.splitContainer1.Panel1.Controls.Add(this.bInfirmiereEtat);
            this.splitContainer1.Panel1.Controls.Add(this.bMessage);
            this.splitContainer1.Panel1.Controls.Add(this.lnbFicheOuverte);
            this.splitContainer1.Panel1.Controls.Add(this.bGardes);
            this.splitContainer1.Panel1.Controls.Add(this.LNbVisiteEnAttente);
            this.splitContainer1.Panel1.Controls.Add(this.bCarto);
            this.splitContainer1.Panel1.Controls.Add(this.bRafraichi);
            this.splitContainer1.Panel1.Controls.Add(this.bFindegarde);
            this.splitContainer1.Panel1.Controls.Add(this.bEtatMed);
            this.splitContainer1.Panel1.Controls.Add(this.bVisiteJournaliere);
            this.splitContainer1.Panel1.Controls.Add(this.bRechercheAppel);
            this.splitContainer1.Panel1.Controls.Add(this.bVisiteMed);
            this.splitContainer1.Panel1.Controls.Add(this.bvisite);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(1641, 625);
            this.splitContainer1.SplitterDistance = 84;
            this.splitContainer1.TabIndex = 0;
            // 
            // bRDV
            // 
            this.bRDV.FlatAppearance.BorderSize = 0;
            this.bRDV.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bRDV.ImageIndex = 13;
            this.bRDV.ImageList = this.imageList1;
            this.bRDV.Location = new System.Drawing.Point(325, 10);
            this.bRDV.Name = "bRDV";
            this.bRDV.Size = new System.Drawing.Size(64, 64);
            this.bRDV.TabIndex = 81;
            this.toolTip1.SetToolTip(this.bRDV, "Liste des rendez-vous");
            this.bRDV.UseVisualStyleBackColor = true;
            this.bRDV.Click += new System.EventHandler(this.bRDV_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "bFinGarde.png");
            this.imageList1.Images.SetKeyName(1, "bRefresh.png");
            this.imageList1.Images.SetKeyName(2, "bCafeOn.png");
            this.imageList1.Images.SetKeyName(3, "bCafeOff.png");
            this.imageList1.Images.SetKeyName(4, "bvisiteOn.png");
            this.imageList1.Images.SetKeyName(5, "bvisiteJour.png");
            this.imageList1.Images.SetKeyName(6, "bvisiteAttribue.png");
            this.imageList1.Images.SetKeyName(7, "bAppel.png");
            this.imageList1.Images.SetKeyName(8, "bRechercheAppel.png");
            this.imageList1.Images.SetKeyName(9, "bCarto.png");
            this.imageList1.Images.SetKeyName(10, "bEngarde.png");
            this.imageList1.Images.SetKeyName(11, "bmessage.png");
            this.imageList1.Images.SetKeyName(12, "bInfirmiere.png");
            this.imageList1.Images.SetKeyName(13, "bRDV.png");
            this.imageList1.Images.SetKeyName(14, "bRDV1.png");
            // 
            // bInfirmiereEtat
            // 
            this.bInfirmiereEtat.FlatAppearance.BorderSize = 0;
            this.bInfirmiereEtat.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bInfirmiereEtat.ImageIndex = 12;
            this.bInfirmiereEtat.ImageList = this.imageList1;
            this.bInfirmiereEtat.Location = new System.Drawing.Point(1082, 10);
            this.bInfirmiereEtat.Name = "bInfirmiereEtat";
            this.bInfirmiereEtat.Size = new System.Drawing.Size(64, 64);
            this.bInfirmiereEtat.TabIndex = 80;
            this.toolTip1.SetToolTip(this.bInfirmiereEtat, "liste des organisations en garde avec leur visites");
            this.bInfirmiereEtat.UseVisualStyleBackColor = true;
            this.bInfirmiereEtat.Visible = false;
            this.bInfirmiereEtat.Click += new System.EventHandler(this.bInfirmiereEtat_Click);
            // 
            // bMessage
            // 
            this.bMessage.FlatAppearance.BorderSize = 0;
            this.bMessage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bMessage.ImageIndex = 11;
            this.bMessage.ImageList = this.imageList1;
            this.bMessage.Location = new System.Drawing.Point(851, 10);
            this.bMessage.Name = "bMessage";
            this.bMessage.Size = new System.Drawing.Size(64, 64);
            this.bMessage.TabIndex = 12;
            this.toolTip1.SetToolTip(this.bMessage, "Envoi de message au médecin sélectionné, ou à tous, si aucun médecin de sélection" +
        "né");
            this.bMessage.UseVisualStyleBackColor = true;
            this.bMessage.Click += new System.EventHandler(this.bMessage_Click);
            // 
            // lnbFicheOuverte
            // 
            this.lnbFicheOuverte.AutoSize = true;
            this.lnbFicheOuverte.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnbFicheOuverte.ForeColor = System.Drawing.SystemColors.Window;
            this.lnbFicheOuverte.Location = new System.Drawing.Point(34, 15);
            this.lnbFicheOuverte.Name = "lnbFicheOuverte";
            this.lnbFicheOuverte.Size = new System.Drawing.Size(52, 55);
            this.lnbFicheOuverte.TabIndex = 11;
            this.lnbFicheOuverte.Text = "0";
            this.toolTip1.SetToolTip(this.lnbFicheOuverte, "Nombre de fiches visites ouvertes");
            // 
            // bGardes
            // 
            this.bGardes.FlatAppearance.BorderSize = 0;
            this.bGardes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bGardes.ImageIndex = 10;
            this.bGardes.ImageList = this.imageList1;
            this.bGardes.Location = new System.Drawing.Point(127, 10);
            this.bGardes.Name = "bGardes";
            this.bGardes.Size = new System.Drawing.Size(64, 64);
            this.bGardes.TabIndex = 10;
            this.toolTip1.SetToolTip(this.bGardes, "Mise en garde des médecins");
            this.bGardes.UseVisualStyleBackColor = true;
            this.bGardes.Click += new System.EventHandler(this.bGardes_Click);
            // 
            // LNbVisiteEnAttente
            // 
            this.LNbVisiteEnAttente.AutoSize = true;
            this.LNbVisiteEnAttente.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LNbVisiteEnAttente.ForeColor = System.Drawing.SystemColors.Window;
            this.LNbVisiteEnAttente.Location = new System.Drawing.Point(1516, 15);
            this.LNbVisiteEnAttente.Name = "LNbVisiteEnAttente";
            this.LNbVisiteEnAttente.Size = new System.Drawing.Size(52, 55);
            this.LNbVisiteEnAttente.TabIndex = 9;
            this.LNbVisiteEnAttente.Text = "0";
            this.toolTip1.SetToolTip(this.LNbVisiteEnAttente, "Nombre de visite en attente (Y compris les conseils Tel.)");
            // 
            // bCarto
            // 
            this.bCarto.FlatAppearance.BorderSize = 0;
            this.bCarto.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bCarto.ImageIndex = 9;
            this.bCarto.ImageList = this.imageList1;
            this.bCarto.Location = new System.Drawing.Point(545, 10);
            this.bCarto.Name = "bCarto";
            this.bCarto.Size = new System.Drawing.Size(64, 64);
            this.bCarto.TabIndex = 7;
            this.toolTip1.SetToolTip(this.bCarto, "Affichage de la carte");
            this.bCarto.UseVisualStyleBackColor = true;
            this.bCarto.Click += new System.EventHandler(this.bCarto_Click);
            // 
            // bRafraichi
            // 
            this.bRafraichi.FlatAppearance.BorderSize = 0;
            this.bRafraichi.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bRafraichi.ImageIndex = 1;
            this.bRafraichi.ImageList = this.imageList1;
            this.bRafraichi.Location = new System.Drawing.Point(699, 10);
            this.bRafraichi.Name = "bRafraichi";
            this.bRafraichi.Size = new System.Drawing.Size(64, 64);
            this.bRafraichi.TabIndex = 6;
            this.toolTip1.SetToolTip(this.bRafraichi, "Rafraichi l\'écran");
            this.bRafraichi.UseVisualStyleBackColor = true;
            this.bRafraichi.Click += new System.EventHandler(this.bRafraichi_Click);
            // 
            // bFindegarde
            // 
            this.bFindegarde.FlatAppearance.BorderSize = 0;
            this.bFindegarde.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bFindegarde.ImageIndex = 0;
            this.bFindegarde.ImageList = this.imageList1;
            this.bFindegarde.Location = new System.Drawing.Point(1392, 10);
            this.bFindegarde.Name = "bFindegarde";
            this.bFindegarde.Size = new System.Drawing.Size(64, 64);
            this.bFindegarde.TabIndex = 5;
            this.toolTip1.SetToolTip(this.bFindegarde, "Fin de garde du médecin sélectionné");
            this.bFindegarde.UseVisualStyleBackColor = true;
            this.bFindegarde.Click += new System.EventHandler(this.bFindegarde_Click);
            // 
            // bEtatMed
            // 
            this.bEtatMed.FlatAppearance.BorderSize = 0;
            this.bEtatMed.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bEtatMed.ImageIndex = 2;
            this.bEtatMed.ImageList = this.imageList1;
            this.bEtatMed.Location = new System.Drawing.Point(921, 10);
            this.bEtatMed.Name = "bEtatMed";
            this.bEtatMed.Size = new System.Drawing.Size(64, 64);
            this.bEtatMed.TabIndex = 4;
            this.toolTip1.SetToolTip(this.bEtatMed, "Etat du médecin (En pause, Dispo, en visite)");
            this.bEtatMed.UseVisualStyleBackColor = true;
            this.bEtatMed.Click += new System.EventHandler(this.bEtatMed_Click);
            // 
            // bVisiteJournaliere
            // 
            this.bVisiteJournaliere.FlatAppearance.BorderSize = 0;
            this.bVisiteJournaliere.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bVisiteJournaliere.ImageIndex = 5;
            this.bVisiteJournaliere.ImageList = this.imageList1;
            this.bVisiteJournaliere.Location = new System.Drawing.Point(1161, 10);
            this.bVisiteJournaliere.Name = "bVisiteJournaliere";
            this.bVisiteJournaliere.Size = new System.Drawing.Size(64, 64);
            this.bVisiteJournaliere.TabIndex = 3;
            this.toolTip1.SetToolTip(this.bVisiteJournaliere, "Visite depuis le début de la garde du médecin");
            this.bVisiteJournaliere.UseVisualStyleBackColor = true;
            this.bVisiteJournaliere.Click += new System.EventHandler(this.bVisiteJournaliere_Click);
            // 
            // bRechercheAppel
            // 
            this.bRechercheAppel.FlatAppearance.BorderSize = 0;
            this.bRechercheAppel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bRechercheAppel.ImageIndex = 8;
            this.bRechercheAppel.ImageList = this.imageList1;
            this.bRechercheAppel.Location = new System.Drawing.Point(429, 10);
            this.bRechercheAppel.Name = "bRechercheAppel";
            this.bRechercheAppel.Size = new System.Drawing.Size(64, 64);
            this.bRechercheAppel.TabIndex = 2;
            this.toolTip1.SetToolTip(this.bRechercheAppel, "Rechercher un appel");
            this.bRechercheAppel.UseVisualStyleBackColor = true;
            this.bRechercheAppel.Click += new System.EventHandler(this.bRechercheAppel_Click);
            // 
            // bVisiteMed
            // 
            this.bVisiteMed.FlatAppearance.BorderSize = 0;
            this.bVisiteMed.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bVisiteMed.ImageIndex = 6;
            this.bVisiteMed.ImageList = this.imageList1;
            this.bVisiteMed.Location = new System.Drawing.Point(1237, 10);
            this.bVisiteMed.Name = "bVisiteMed";
            this.bVisiteMed.Size = new System.Drawing.Size(59, 64);
            this.bVisiteMed.TabIndex = 1;
            this.toolTip1.SetToolTip(this.bVisiteMed, "Visites attribuées ou pré attribuées  du médecin");
            this.bVisiteMed.UseVisualStyleBackColor = true;
            this.bVisiteMed.Click += new System.EventHandler(this.bVisiteMed_Click);
            // 
            // bvisite
            // 
            this.bvisite.FlatAppearance.BorderSize = 0;
            this.bvisite.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bvisite.ImageIndex = 7;
            this.bvisite.ImageList = this.imageList1;
            this.bvisite.Location = new System.Drawing.Point(234, 10);
            this.bvisite.Name = "bvisite";
            this.bvisite.Size = new System.Drawing.Size(64, 64);
            this.bvisite.TabIndex = 0;
            this.toolTip1.SetToolTip(this.bvisite, "Nouvel appel");
            this.bvisite.UseVisualStyleBackColor = true;
            this.bvisite.Click += new System.EventHandler(this.bvisite_Click);
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.splitContainer3);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.splitContainer4);
            this.splitContainer2.Size = new System.Drawing.Size(1641, 537);
            this.splitContainer2.SplitterDistance = 985;
            this.splitContainer2.TabIndex = 0;
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            this.splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.listView1);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.splitContainer3.Panel2.Controls.Add(this.listView3);
            this.splitContainer3.Size = new System.Drawing.Size(985, 537);
            this.splitContainer3.SplitterDistance = 360;
            this.splitContainer3.TabIndex = 0;
            // 
            // listView1
            // 
            this.listView1.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.ForeColor = System.Drawing.SystemColors.Window;
            this.listView1.FullRowSelect = true;
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(0, 0);
            this.listView1.MultiSelect = false;
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(985, 360);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.listView1_ItemDrag);
            this.listView1.Click += new System.EventHandler(this.listView1_Click);
            this.listView1.DragEnter += new System.Windows.Forms.DragEventHandler(this.listView1_DragEnter);
            this.listView1.DoubleClick += new System.EventHandler(this.listView1_DoubleClick);
            // 
            // listView3
            // 
            this.listView3.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.listView3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView3.ForeColor = System.Drawing.SystemColors.Window;
            this.listView3.FullRowSelect = true;
            this.listView3.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listView3.HideSelection = false;
            this.listView3.Location = new System.Drawing.Point(0, 0);
            this.listView3.MultiSelect = false;
            this.listView3.Name = "listView3";
            this.listView3.OwnerDraw = true;
            this.listView3.Size = new System.Drawing.Size(985, 173);
            this.listView3.TabIndex = 1;
            this.listView3.UseCompatibleStateImageBehavior = false;
            this.listView3.DrawColumnHeader += new System.Windows.Forms.DrawListViewColumnHeaderEventHandler(this.listView3_DrawColumnHeader);
            this.listView3.DrawItem += new System.Windows.Forms.DrawListViewItemEventHandler(this.listView3_DrawItem);
            this.listView3.MouseClick += new System.Windows.Forms.MouseEventHandler(this.listView3_MouseClick);
            this.listView3.MouseHover += new System.EventHandler(this.listView3_MouseHover);
            // 
            // splitContainer4
            // 
            this.splitContainer4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer4.Location = new System.Drawing.Point(0, 0);
            this.splitContainer4.Name = "splitContainer4";
            this.splitContainer4.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer4.Panel1
            // 
            this.splitContainer4.Panel1.Controls.Add(this.listView2);
            // 
            // splitContainer4.Panel2
            // 
            this.splitContainer4.Panel2.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.splitContainer4.Panel2.Controls.Add(this.rTBoxAideRegul);
            this.splitContainer4.Size = new System.Drawing.Size(652, 537);
            this.splitContainer4.SplitterDistance = 310;
            this.splitContainer4.TabIndex = 0;
            // 
            // listView2
            // 
            this.listView2.AllowDrop = true;
            this.listView2.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.listView2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView2.ForeColor = System.Drawing.SystemColors.Window;
            this.listView2.FullRowSelect = true;
            this.listView2.HideSelection = false;
            this.listView2.Location = new System.Drawing.Point(0, 0);
            this.listView2.Name = "listView2";
            this.listView2.Size = new System.Drawing.Size(652, 310);
            this.listView2.TabIndex = 0;
            this.listView2.UseCompatibleStateImageBehavior = false;
            this.listView2.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.listView2_ItemDrag);
            this.listView2.Click += new System.EventHandler(this.listView2_Click);
            this.listView2.DragEnter += new System.Windows.Forms.DragEventHandler(this.listView2_DragEnter);
            this.listView2.DoubleClick += new System.EventHandler(this.listView2_DoubleClick);
            this.listView2.MouseClick += new System.Windows.Forms.MouseEventHandler(this.listView2_MouseClick);
            // 
            // rTBoxAideRegul
            // 
            this.rTBoxAideRegul.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.rTBoxAideRegul.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rTBoxAideRegul.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rTBoxAideRegul.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rTBoxAideRegul.ForeColor = System.Drawing.SystemColors.Window;
            this.rTBoxAideRegul.Location = new System.Drawing.Point(0, 0);
            this.rTBoxAideRegul.Name = "rTBoxAideRegul";
            this.rTBoxAideRegul.ReadOnly = true;
            this.rTBoxAideRegul.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.rTBoxAideRegul.Size = new System.Drawing.Size(652, 223);
            this.rTBoxAideRegul.TabIndex = 1;
            this.rTBoxAideRegul.Text = "";
            // 
            // MenuStrip1
            // 
            this.MenuStrip1.BackColor = System.Drawing.Color.DimGray;
            this.MenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fichierToolStripMenuItem,
            this.MenuGardeMedecins,
            this.appelsToolStripMenuItem,
            this.rechercheVisitesToolStripMenuItem,
            this.MenuNvxMessage,
            this.rechercheDeMessagesToolStripMenuItem,
            this.recherchesDiversesToolStripMenuItem,
            this.diversListesToolStripMenuItem,
            this.paramètresToolStripMenuItem,
            this.MenuAPropos});
            this.MenuStrip1.Location = new System.Drawing.Point(0, 0);
            this.MenuStrip1.Name = "MenuStrip1";
            this.MenuStrip1.Size = new System.Drawing.Size(1641, 24);
            this.MenuStrip1.TabIndex = 1;
            this.MenuStrip1.Text = "MenuStrip1";
            // 
            // fichierToolStripMenuItem
            // 
            this.fichierToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.quitterToolStripMenuItem});
            this.fichierToolStripMenuItem.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.fichierToolStripMenuItem.Name = "fichierToolStripMenuItem";
            this.fichierToolStripMenuItem.Size = new System.Drawing.Size(54, 20);
            this.fichierToolStripMenuItem.Text = "&Fichier";
            // 
            // quitterToolStripMenuItem
            // 
            this.quitterToolStripMenuItem.Name = "quitterToolStripMenuItem";
            this.quitterToolStripMenuItem.Size = new System.Drawing.Size(111, 22);
            this.quitterToolStripMenuItem.Text = "&Quitter";
            this.quitterToolStripMenuItem.Click += new System.EventHandler(this.quitterToolStripMenuItem_Click);
            // 
            // MenuGardeMedecins
            // 
            this.MenuGardeMedecins.Name = "MenuGardeMedecins";
            this.MenuGardeMedecins.Size = new System.Drawing.Size(143, 20);
            this.MenuGardeMedecins.Text = "&Mise en Garde Médecin";
            this.MenuGardeMedecins.Click += new System.EventHandler(this.MenuGardeMedecins_Click);
            // 
            // appelsToolStripMenuItem
            // 
            this.appelsToolStripMenuItem.Name = "appelsToolStripMenuItem";
            this.appelsToolStripMenuItem.Size = new System.Drawing.Size(55, 20);
            this.appelsToolStripMenuItem.Text = "Appels";
            this.appelsToolStripMenuItem.Click += new System.EventHandler(this.bvisite_Click);
            // 
            // rechercheVisitesToolStripMenuItem
            // 
            this.rechercheVisitesToolStripMenuItem.Name = "rechercheVisitesToolStripMenuItem";
            this.rechercheVisitesToolStripMenuItem.Size = new System.Drawing.Size(110, 20);
            this.rechercheVisitesToolStripMenuItem.Text = "Recherche Visites";
            this.rechercheVisitesToolStripMenuItem.Click += new System.EventHandler(this.rechercheVisitesToolStripMenuItem_Click);
            // 
            // MenuNvxMessage
            // 
            this.MenuNvxMessage.Name = "MenuNvxMessage";
            this.MenuNvxMessage.Size = new System.Drawing.Size(116, 20);
            this.MenuNvxMessage.Text = "Nouveau message";
            this.MenuNvxMessage.Click += new System.EventHandler(this.MenuNvxMessage_Click);
            // 
            // rechercheDeMessagesToolStripMenuItem
            // 
            this.rechercheDeMessagesToolStripMenuItem.Name = "rechercheDeMessagesToolStripMenuItem";
            this.rechercheDeMessagesToolStripMenuItem.Size = new System.Drawing.Size(144, 20);
            this.rechercheDeMessagesToolStripMenuItem.Text = "Recherche de m&essages";
            this.rechercheDeMessagesToolStripMenuItem.Click += new System.EventHandler(this.rechercheDeMessagesToolStripMenuItem_Click);
            // 
            // recherchesDiversesToolStripMenuItem
            // 
            this.recherchesDiversesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuRechEvent,
            this.MenuHistoAttribSmart});
            this.recherchesDiversesToolStripMenuItem.Name = "recherchesDiversesToolStripMenuItem";
            this.recherchesDiversesToolStripMenuItem.Size = new System.Drawing.Size(124, 20);
            this.recherchesDiversesToolStripMenuItem.Text = "Recherches diverses";
            // 
            // MenuRechEvent
            // 
            this.MenuRechEvent.Name = "MenuRechEvent";
            this.MenuRechEvent.Size = new System.Drawing.Size(228, 22);
            this.MenuRechEvent.Text = "Recherche évenements";
            this.MenuRechEvent.Click += new System.EventHandler(this.MenuRechEvent_Click);
            // 
            // MenuHistoAttribSmart
            // 
            this.MenuHistoAttribSmart.Name = "MenuHistoAttribSmart";
            this.MenuHistoAttribSmart.Size = new System.Drawing.Size(228, 22);
            this.MenuHistoAttribSmart.Text = "Histo attribution smartphone";
            this.MenuHistoAttribSmart.Click += new System.EventHandler(this.MenuHistoAttribSmart_Click);
            // 
            // diversListesToolStripMenuItem
            // 
            this.diversListesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuOrganisation,
            this.MenuProvenances,
            this.MenuAssurance,
            this.MenuRues,
            this.MenuCommune,
            this.MenuGarde,
            this.MenuSmartphone,
            this.MenuMedecin,
            this.MenuUtilisateur,
            this.MenutelUtil,
            this.MenuMotif});
            this.diversListesToolStripMenuItem.Name = "diversListesToolStripMenuItem";
            this.diversListesToolStripMenuItem.Size = new System.Drawing.Size(80, 20);
            this.diversListesToolStripMenuItem.Text = "Divers listes";
            // 
            // MenuOrganisation
            // 
            this.MenuOrganisation.Name = "MenuOrganisation";
            this.MenuOrganisation.Size = new System.Drawing.Size(158, 22);
            this.MenuOrganisation.Text = "Organisations";
            this.MenuOrganisation.Click += new System.EventHandler(this.MenuOrganisation_Click);
            // 
            // MenuProvenances
            // 
            this.MenuProvenances.Name = "MenuProvenances";
            this.MenuProvenances.Size = new System.Drawing.Size(158, 22);
            this.MenuProvenances.Text = "Provenances";
            this.MenuProvenances.Click += new System.EventHandler(this.MenuProvenances_Click);
            // 
            // MenuAssurance
            // 
            this.MenuAssurance.Name = "MenuAssurance";
            this.MenuAssurance.Size = new System.Drawing.Size(158, 22);
            this.MenuAssurance.Text = "Assurances";
            this.MenuAssurance.Click += new System.EventHandler(this.MenuAssurance_Click);
            // 
            // MenuRues
            // 
            this.MenuRues.Name = "MenuRues";
            this.MenuRues.Size = new System.Drawing.Size(158, 22);
            this.MenuRues.Text = "Rues";
            this.MenuRues.Click += new System.EventHandler(this.MenuRues_Click);
            // 
            // MenuCommune
            // 
            this.MenuCommune.Name = "MenuCommune";
            this.MenuCommune.Size = new System.Drawing.Size(158, 22);
            this.MenuCommune.Text = "Communes";
            this.MenuCommune.Click += new System.EventHandler(this.MenuCommune_Click);
            // 
            // MenuGarde
            // 
            this.MenuGarde.Name = "MenuGarde";
            this.MenuGarde.Size = new System.Drawing.Size(158, 22);
            this.MenuGarde.Text = "Gardes";
            this.MenuGarde.Click += new System.EventHandler(this.MenuGarde_Click);
            // 
            // MenuSmartphone
            // 
            this.MenuSmartphone.Name = "MenuSmartphone";
            this.MenuSmartphone.Size = new System.Drawing.Size(158, 22);
            this.MenuSmartphone.Text = "Smarphones";
            this.MenuSmartphone.Click += new System.EventHandler(this.MenuSmartphone_Click);
            // 
            // MenuMedecin
            // 
            this.MenuMedecin.Name = "MenuMedecin";
            this.MenuMedecin.Size = new System.Drawing.Size(158, 22);
            this.MenuMedecin.Text = "Médecins";
            this.MenuMedecin.Click += new System.EventHandler(this.MenuMedecin_Click);
            // 
            // MenuUtilisateur
            // 
            this.MenuUtilisateur.Name = "MenuUtilisateur";
            this.MenuUtilisateur.Size = new System.Drawing.Size(158, 22);
            this.MenuUtilisateur.Text = "Utilisateurs";
            this.MenuUtilisateur.Click += new System.EventHandler(this.MenuUtilisateur_Click);
            // 
            // MenutelUtil
            // 
            this.MenutelUtil.Name = "MenutelUtil";
            this.MenutelUtil.Size = new System.Drawing.Size(158, 22);
            this.MenutelUtil.Text = "Téléphones utils";
            this.MenutelUtil.Click += new System.EventHandler(this.MenutelUtil_Click);
            // 
            // MenuMotif
            // 
            this.MenuMotif.Name = "MenuMotif";
            this.MenuMotif.Size = new System.Drawing.Size(158, 22);
            this.MenuMotif.Text = "Motifs";
            this.MenuMotif.Click += new System.EventHandler(this.MenuMotif_Click);
            // 
            // paramètresToolStripMenuItem
            // 
            this.paramètresToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Menuressaisie,
            this.MenuParamUtil,
            this.MenuCTI,
            this.MenureinitialiserAlarmes,
            this.MenuParametresDivers,
            this.MenudebloquerMed,
            this.menuGestDoc});
            this.paramètresToolStripMenuItem.Name = "paramètresToolStripMenuItem";
            this.paramètresToolStripMenuItem.Size = new System.Drawing.Size(78, 20);
            this.paramètresToolStripMenuItem.Text = "&Paramètres";
            // 
            // Menuressaisie
            // 
            this.Menuressaisie.Name = "Menuressaisie";
            this.Menuressaisie.Size = new System.Drawing.Size(198, 22);
            this.Menuressaisie.Text = "R&e-saisie";
            this.Menuressaisie.Click += new System.EventHandler(this.Menuressaisie_Click);
            // 
            // MenuParamUtil
            // 
            this.MenuParamUtil.Name = "MenuParamUtil";
            this.MenuParamUtil.Size = new System.Drawing.Size(198, 22);
            this.MenuParamUtil.Text = "Paramètres Utilisateur";
            this.MenuParamUtil.Click += new System.EventHandler(this.MenuParamUtil_Click);
            // 
            // MenuCTI
            // 
            this.MenuCTI.Name = "MenuCTI";
            this.MenuCTI.Size = new System.Drawing.Size(198, 22);
            this.MenuCTI.Text = "CTI";
            this.MenuCTI.Click += new System.EventHandler(this.MenuCTI_Click);
            // 
            // MenureinitialiserAlarmes
            // 
            this.MenureinitialiserAlarmes.Name = "MenureinitialiserAlarmes";
            this.MenureinitialiserAlarmes.Size = new System.Drawing.Size(198, 22);
            this.MenureinitialiserAlarmes.Text = "Ré-initialiser Alarmes";
            this.MenureinitialiserAlarmes.Click += new System.EventHandler(this.MenureinitialiserAlarmes_Click);
            // 
            // MenuParametresDivers
            // 
            this.MenuParametresDivers.Name = "MenuParametresDivers";
            this.MenuParametresDivers.Size = new System.Drawing.Size(198, 22);
            this.MenuParametresDivers.Text = "P&arametres divers";
            this.MenuParametresDivers.Click += new System.EventHandler(this.MenuParametresDivers_Click);
            // 
            // MenudebloquerMed
            // 
            this.MenudebloquerMed.Name = "MenudebloquerMed";
            this.MenudebloquerMed.Size = new System.Drawing.Size(198, 22);
            this.MenudebloquerMed.Text = "Débloquer le médecin";
            this.MenudebloquerMed.Click += new System.EventHandler(this.MenudebloquerMed_Click);
            // 
            // menuGestDoc
            // 
            this.menuGestDoc.Name = "menuGestDoc";
            this.menuGestDoc.Size = new System.Drawing.Size(198, 22);
            this.menuGestDoc.Text = "Gestion des documents";
            this.menuGestDoc.Click += new System.EventHandler(this.menuGestDoc_Click);
            // 
            // MenuAPropos
            // 
            this.MenuAPropos.Name = "MenuAPropos";
            this.MenuAPropos.Size = new System.Drawing.Size(92, 20);
            this.MenuAPropos.Text = "A propos de...";
            this.MenuAPropos.Click += new System.EventHandler(this.MenuAPropos_Click);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 30000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // toolTip1
            // 
            this.toolTip1.AutomaticDelay = 250;
            this.toolTip1.AutoPopDelay = 3500;
            this.toolTip1.InitialDelay = 250;
            this.toolTip1.IsBalloon = true;
            this.toolTip1.ReshowDelay = 50;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1641, 649);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.MenuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SOS régulation";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.splitContainer4.Panel1.ResumeLayout(false);
            this.splitContainer4.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).EndInit();
            this.splitContainer4.ResumeLayout(false);
            this.MenuStrip1.ResumeLayout(false);
            this.MenuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.SplitContainer splitContainer4;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ListView listView2;
        private System.Windows.Forms.MenuStrip MenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fichierToolStripMenuItem;
        private System.Windows.Forms.Button bVisiteMed;
        private System.Windows.Forms.Button bvisite;
        private System.Windows.Forms.ToolStripMenuItem quitterToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem MenuGardeMedecins;
        private System.Windows.Forms.ToolStripMenuItem paramètresToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem appelsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rechercheVisitesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem MenuCTI;
        private System.Windows.Forms.Button bRechercheAppel;
        private System.Windows.Forms.Button bVisiteJournaliere;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button bFindegarde;
        private System.Windows.Forms.Button bEtatMed;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button bRafraichi;
        private System.Windows.Forms.Button bCarto;
        private System.Windows.Forms.ToolStripMenuItem MenuParametresDivers;
        private System.Windows.Forms.ListView listView3;
        private System.Windows.Forms.ToolStripMenuItem rechercheDeMessagesToolStripMenuItem;
        private System.Windows.Forms.RichTextBox rTBoxAideRegul;
        private System.Windows.Forms.Label LNbVisiteEnAttente;
        private System.Windows.Forms.ToolStripMenuItem Menuressaisie;
        private System.Windows.Forms.ToolStripMenuItem MenuParamUtil;
        private System.Windows.Forms.Button bGardes;
        private System.Windows.Forms.ToolStripMenuItem MenuAPropos;
        private System.Windows.Forms.ToolStripMenuItem MenureinitialiserAlarmes;
        private System.Windows.Forms.Label lnbFicheOuverte;
        private System.Windows.Forms.Button bMessage;
        private System.Windows.Forms.ToolStripMenuItem MenuNvxMessage;
        private System.Windows.Forms.ToolStripMenuItem MenudebloquerMed;
        private System.Windows.Forms.ToolStripMenuItem menuGestDoc;
        private System.Windows.Forms.ToolStripMenuItem diversListesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem MenuOrganisation;
        private System.Windows.Forms.ToolStripMenuItem MenuProvenances;
        private System.Windows.Forms.ToolStripMenuItem MenuAssurance;
        private System.Windows.Forms.ToolStripMenuItem MenuRues;
        private System.Windows.Forms.ToolStripMenuItem MenuCommune;
        private System.Windows.Forms.ToolStripMenuItem MenuGarde;
        private System.Windows.Forms.ToolStripMenuItem MenuSmartphone;
        private System.Windows.Forms.ToolStripMenuItem MenuMedecin;
        private System.Windows.Forms.ToolStripMenuItem MenuUtilisateur;
        private System.Windows.Forms.ToolStripMenuItem MenutelUtil;
        private System.Windows.Forms.ToolStripMenuItem recherchesDiversesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem MenuRechEvent;
        private System.Windows.Forms.ToolStripMenuItem MenuHistoAttribSmart;
        private System.Windows.Forms.ToolStripMenuItem MenuMotif;
        private System.Windows.Forms.Button bInfirmiereEtat;
        private System.Windows.Forms.Button bRDV;
    }
}

