using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace SRegulV2
{
    public partial class FRechMedecinTraitant : Form
    {
        private class MedecinTraitantInfo
        {
            public int Id { get; set; }
            public string Nom { get; set; } = "";
            public string Prenom { get; set; } = "";
        }

        private int IdMedecin;
        public int idMedecin
        {
            get { return IdMedecin; }
            set { IdMedecin = value; }
        }

        private string NomMedecin;
        public string nomMedecin
        {
            get { return NomMedecin; }
            set { NomMedecin = value; }
        }

        private string PrenomMedecin;
        public string prenomMedecin
        {
            get { return PrenomMedecin; }
            set { PrenomMedecin = value; }
        }

        private DataTable dtMedecin = new DataTable();

        public FRechMedecinTraitant()
        {
            InitializeComponent();

            listView1.Columns.Add("Code Médecin", 1);
            listView1.Columns.Add("Médecin", 150);
            listView1.View = View.Details;
            ResizeMedecinColumn();

            IdMedecin = -1;
            NomMedecin = "";
            PrenomMedecin = "";
        }

        private void FRechMedecinTraitant_Load(object sender, EventArgs e)
        {
            ResizeMedecinColumn();
            listView1.BeginUpdate();
            listView1.Items.Clear();

            dtMedecin = FonctionsAppels.ChargeListeMedecinsTraitants();

            AddMedecinsToList(dtMedecin);

            listView1.EndUpdate();
        }

        private void ResizeMedecinColumn()
        {
            if (listView1.Columns.Count < 2)
            {
                return;
            }

            int availableWidth = listView1.ClientSize.Width
                - listView1.Columns[0].Width
                - SystemInformation.VerticalScrollBarWidth
                - 4;
            listView1.Columns[1].Width = Math.Max(300, availableWidth);
        }

        private void tBMedecin_TextChanged(object sender, EventArgs e)
        {
            string SqlSelect = "Nom like '" + tBMedecin.Text.Replace("'", "''") + "%'";
            string Trie = "Nom";

            if (dtMedecin.Select(SqlSelect, Trie).Any())
            {
                DataTable dtMedTrie = dtMedecin.Select(SqlSelect, Trie).CopyToDataTable();

                listView1.BeginUpdate();
                listView1.Items.Clear();

                AddMedecinsToList(dtMedTrie);

                listView1.EndUpdate();
            }
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
                return;

            ListViewItem selectedItem = listView1.SelectedItems[0];
            if (!TryGetMedecinInfo(selectedItem, out MedecinTraitantInfo info))
            {
                return;
            }

            IdMedecin = info.Id;
            NomMedecin = info.Nom;
            PrenomMedecin = info.Prenom;

            DialogResult = DialogResult.OK;
            this.Close();
        }


        private void tBMedecin_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (listView1.Items.Count > 0)
                {
                    if (TryGetMedecinInfo(listView1.Items[0], out MedecinTraitantInfo info))
                    {
                        IdMedecin = info.Id;
                        NomMedecin = info.Nom;
                        PrenomMedecin = info.Prenom;
                        DialogResult = DialogResult.OK;
                    }
                    else
                    {
                        DialogResult = DialogResult.Cancel;
                    }
                }
                else
                {
                    DialogResult = DialogResult.Cancel;
                }

                this.Close();
            }
        }

        private void bExit_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void AddMedecinsToList(DataTable data)
        {
            for (int i = 0; i < data.Rows.Count; i++)
            {
                DataRow row = data.Rows[i];
                string nom = row["Nom"].ToString().Trim();
                if (string.IsNullOrWhiteSpace(nom))
                {
                    continue;
                }

                string prenom = row["Prenom"].ToString().Trim();
                if (!int.TryParse(row["Num"].ToString(), out int id))
                {
                    continue;
                }

                string location = GetMedecinLocation(row);
                string nomPrenom = (nom + " " + prenom).Trim();
                string displayName = string.IsNullOrWhiteSpace(location) ? nomPrenom : $"{nomPrenom} ({location})";

                ListViewItem item = new ListViewItem(id.ToString());
                item.SubItems.Add(displayName);
                item.Tag = new MedecinTraitantInfo
                {
                    Id = id,
                    Nom = nom,
                    Prenom = prenom
                };
                listView1.Items.Add(item);
            }
        }

        private static string GetMedecinLocation(DataRow row)
        {
            string[] locationFields = { "Commune", "Ville", "Localite", "Lieu" };

            foreach (string field in locationFields)
            {
                if (!row.Table.Columns.Contains(field))
                {
                    continue;
                }

                string value = row[field].ToString().Trim();
                if (!string.IsNullOrWhiteSpace(value))
                {
                    return value;
                }
            }

            return "";
        }

        private static bool TryGetMedecinInfo(ListViewItem item, out MedecinTraitantInfo info)
        {
            info = item?.Tag as MedecinTraitantInfo;
            if (info == null)
            {
                return false;
            }

            return info.Id > 0 && !string.IsNullOrWhiteSpace(info.Nom);
        }
    }
}
