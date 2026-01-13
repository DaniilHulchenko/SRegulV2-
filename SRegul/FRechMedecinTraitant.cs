using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace SRegulV2
{
    public partial class FRechMedecinTraitant : Form
    {
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

            IdMedecin = -1;
            NomMedecin = "";
            PrenomMedecin = "";
        }

        private void FRechMedecinTraitant_Load(object sender, EventArgs e)
        {
            listView1.BeginUpdate();
            listView1.Items.Clear();

            dtMedecin = FonctionsAppels.ChargeListeMedecinsTraitants();

            for (int i = 0; i < dtMedecin.Rows.Count; i++)
            {
                ListViewItem item = new ListViewItem(dtMedecin.Rows[i]["Num"].ToString());
                item.SubItems.Add(dtMedecin.Rows[i]["Nom"].ToString() + " " + dtMedecin.Rows[i]["Prenom"].ToString());
                item.Tag = new string[] { dtMedecin.Rows[i]["Nom"].ToString(), dtMedecin.Rows[i]["Prenom"].ToString() };
                listView1.Items.Add(item);
            }

            listView1.EndUpdate();
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

                for (int i = 0; i < dtMedTrie.Rows.Count; i++)
                {
                    ListViewItem item = new ListViewItem(dtMedTrie.Rows[i]["Num"].ToString());
                    item.SubItems.Add(dtMedTrie.Rows[i]["Nom"].ToString() + " " + dtMedTrie.Rows[i]["Prenom"].ToString());
                    item.Tag = new string[] { dtMedTrie.Rows[i]["Nom"].ToString(), dtMedTrie.Rows[i]["Prenom"].ToString() };
                    listView1.Items.Add(item);
                }

                listView1.EndUpdate();
            }
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            int index = listView1.SelectedItems[0].Index;

            IdMedecin = int.Parse(listView1.SelectedItems[index].Text);
            string[] nomPrenom = (string[])listView1.SelectedItems[index].Tag;
            NomMedecin = nomPrenom[0];
            PrenomMedecin = nomPrenom[1];

            DialogResult = DialogResult.OK;
            this.Close();
        }

        private void tBMedecin_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (listView1.Items.Count > 0)
                {
                    IdMedecin = int.Parse(listView1.Items[0].Text);
                    string[] nomPrenom = (string[])listView1.Items[0].Tag;
                    NomMedecin = nomPrenom[0];
                    PrenomMedecin = nomPrenom[1];

                    DialogResult = DialogResult.OK;
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
    }
}
