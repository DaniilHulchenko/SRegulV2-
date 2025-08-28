using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SRegulV2
{
    public partial class FListeInfirmiere : Form
    {

        //Pour retourner le résultat de la recherche
        private string NumVisite;
        public string numVisite
        {
            get { return NumVisite; }
            set { NumVisite = value; }
        }
       

        private DataTable dtInfirmiere = new DataTable();

        public FListeInfirmiere()
        {
            InitializeComponent();

            listView1.Columns.Add("Code médecin", 1);
            listView1.Columns.Add("Organisation", 100);
            listView1.Columns.Add("Nom Prenom", 250);
            listView1.Columns.Add("Nb de visite", 250);
            listView1.Columns.Add("Patient", 250);            
            listView1.Columns.Add("Adresse", 250);   //Rue + N° Rue,                     
            listView1.Columns.Add("Commune", 100);   //Ville
            listView1.View = View.Details;    //Pour afficher les subItems       
        }

       
        private void FListeInfirmiere_Load(object sender, EventArgs e)
        {
            //On vide la liste pour la rafraichir                
            listView1.BeginUpdate();
            listView1.Items.Clear();

            dtInfirmiere = FonctionsAppels.ChargeListeInfirmiere("En garde avec details visite");            

            for (int i = 0; i < dtInfirmiere.Rows.Count; i++)
            {
                ListViewItem item = new ListViewItem(dtInfirmiere.Rows[i]["CodeMedecin"].ToString());
                item.SubItems.Add(dtInfirmiere.Rows[i]["LibelleOrg"].ToString());
                item.SubItems.Add(dtInfirmiere.Rows[i]["NomMedecin"].ToString());
                item.SubItems.Add(dtInfirmiere.Rows[i]["nb_Visite"].ToString());
                item.SubItems.Add(dtInfirmiere.Rows[i]["NomPrenom"].ToString());
                item.SubItems.Add(dtInfirmiere.Rows[i]["Adresse"].ToString());
                item.SubItems.Add(dtInfirmiere.Rows[i]["Commune"].ToString());
                listView1.Items.Add(item);
            }

            //Puis on rajoute "Aucun"
            ListViewItem itemAucun = new ListViewItem("-1");
            itemAucun.SubItems.Add("");
            itemAucun.SubItems.Add("Aucun");
            itemAucun.SubItems.Add("");
            itemAucun.SubItems.Add("");
            itemAucun.SubItems.Add("");
            itemAucun.SubItems.Add("");
            listView1.Items.Add(itemAucun);

            listView1.EndUpdate();  //Rafraichi le controle        
        }

        //On affecte la visite à cette infirmière (sauf si c'est Aucun qui est choisie)
        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            if (listView1.SelectedItems[0].Text != "-1")
            {
                affecte(numVisite, listView1.SelectedItems[0].Text);
            }

            //Puis on ferme la form
            this.Close();
        }


        public void affecte(string visite, string Infirmiere)
        {
            //On affecte la visite à l'infirmière Sauf si c'est une pré-attribué
            //Maj des tables dans la base

            string Etat = FonctionsAppels.AttributionVisiteOrg(visite, Infirmiere);

            if (Etat == "AT")    //Elle était libre donc on lui attribue
            {
                //Et si c'est ok, Maj de la ligne...
                //ici on incrémente le compteur de visite (sans tout rafraichir)
                int cpt = int.Parse(listView1.SelectedItems[0].SubItems[3].Text) + 1;
                listView1.SelectedItems[0].SubItems[3] = new ListViewItem.ListViewSubItem() { Text = cpt.ToString() };       //Compteur                                

                //Coloration du nom du médecin (En jaune: Attribuée)               
                //ColoriseSelectionListview2("AT");                                
            }
            else if (Etat == "PR")
            {
                //On incrémente le compteur de visite
                int cpt = int.Parse(listView1.SelectedItems[0].SubItems[3].Text) + 1;
                listView1.SelectedItems[0].SubItems[3] = new ListViewItem.ListViewSubItem() { Text = cpt.ToString() };       //Compteur                  
            }  
            else if (Etat == "T")    //Terminée
                MessageBox.Show("Cette visite est déjà terminée par l'infirmière ou l'organisme.");

            //Sinon on ne fait rien                                        
        }


        private void bExit_Click(object sender, EventArgs e)
        {
            //Puis on ferme la form
            this.Close();
        }
    }
}
