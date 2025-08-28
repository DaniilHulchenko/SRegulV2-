using MySqlConnector;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SRegulV2
{
    public partial class FGarde : Form
    {
        public static DataTable dtGarde = new DataTable();
        private static string Etat = "Lecture";      

        public FGarde()
        {
            InitializeComponent();

            //Def des colonnes du Listeview
            listView1.Columns.Add("Idgarde", 1);         //Colonne invisible
            listView1.Columns.Add("Type de garde", 100);     
            listView1.Columns.Add("Heure début", 100);       
            listView1.Columns.Add("Heure Fin", 100);         
            listView1.View = View.Details;    //Pour afficher les subItems  

            //On vide les champs et iniitalise les boutons
            VideChamps();
        }

        private void FGarde_Load(object sender, EventArgs e)
        {
            dtGarde.Clear();
            dtGarde = FonctionsAppels.ChargeListeGardes();

            //On vide la liste pour la rafraichir                
            listView1.BeginUpdate();
            listView1.Items.Clear();

            for (int i = 0; i < dtGarde.Rows.Count; i++)
            {
                ListViewItem item = new ListViewItem(dtGarde.Rows[i]["IdGarde"].ToString());
                item.SubItems.Add(dtGarde.Rows[i]["TypeGarde"].ToString());
                item.SubItems.Add(dtGarde.Rows[i]["HeureDeb"].ToString());
                item.SubItems.Add(dtGarde.Rows[i]["HeureFin"].ToString());

                listView1.Items.Add(item);
            }

            listView1.EndUpdate();  //Rafraichi le controle        
        }

        private void VideChamps()
        {
            lIdGarde.Text = "";
            tBTypeGarde.Text = "";
            mTBHeureDeb.Text = "";
            mTBHeureFin.Text = "";
          
            //Pour les boutons
            bAjouter.ImageIndex = 0;
            bAjouter.Visible = true;

            bValider.ImageIndex = 4;
            bValider.Enabled = false;

            bSupprimer.ImageIndex = 6;
            bSupprimer.Enabled = false;

            bCancel.Visible = false;

            bExit.ImageIndex = 7;
        }

        //Selection d'une garde dans la liste
        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            int index = listView1.SelectedItems[0].Index;

            //On peuple les champs
            lIdGarde.Text = dtGarde.Rows[index]["IdGarde"].ToString();
            tBTypeGarde.Text = dtGarde.Rows[index]["TypeGarde"].ToString();
            mTBHeureDeb.Text = dtGarde.Rows[index]["HeureDeb"].ToString();
            mTBHeureFin.Text = dtGarde.Rows[index]["HeureFin"].ToString();           

            //Gestion des boutons: On passe en modif
            Etat = "Modif";

            bAjouter.Visible = false;
            bValider.Enabled = true;
            bCancel.Visible = true;
            bSupprimer.Enabled = true;
        }

        //***Gestion des boutons
        private void bAjouter_Click(object sender, EventArgs e)
        {
            //Gestion des boutons: On passe en Ajout
            Etat = "Ajout";

            VideChamps();

            bAjouter.Visible = false;
            bValider.Enabled = true;
            bCancel.Visible = true;
            bSupprimer.Enabled = false;
        }

        //On valide en fonction de l'état
        private void bValider_Click(object sender, EventArgs e)
        {
            if (VerifSaisie() == "OK")
            {
                if (Etat == "Ajout")
                {
                    if (AjoutEnreg() == "OK")
                    {
                        VideChamps();

                        //Gestion des boutons
                        bAjouter.Visible = true;
                        bValider.Enabled = false;
                        bCancel.Visible = false;
                        bSupprimer.Enabled = false;

                        Etat = "Lecture";

                        //On rafraichi la liste des SmartPhones
                        FGarde_Load(sender, e);
                    }
                    else
                    {
                        //Gestion des boutons
                        bAjouter.Visible = false;
                        bValider.Enabled = true;
                        bCancel.Visible = true;
                        bSupprimer.Enabled = false;
                    }
                }
                else if (Etat == "Modif")
                {
                    if (UpdateEnreg() == "OK")
                    {
                        VideChamps();
                        //Gestion des boutons
                        bAjouter.Visible = true;
                        bValider.Enabled = false;
                        bCancel.Visible = false;
                        bSupprimer.Enabled = false;

                        Etat = "Lecture";

                        //On rafraichi la liste des gardes
                        FGarde_Load(sender, e);
                    }
                    else
                    {
                        //Gestion des boutons
                        bAjouter.Visible = false;
                        bValider.Enabled = true;
                        bCancel.Visible = true;
                        bSupprimer.Enabled = false;
                    }
                }
            }
        }

       
        //vérification des saisies
        private string VerifSaisie()
        {
            string retour = "KO";
            int V1 = 0;
                    
            if (tBTypeGarde.Text != "")
                V1 += 1;
            else
                MessageBox.Show("Vous devez renseigner le type de garde. ", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);

            if (mTBHeureDeb.Text != "")
            {
                //On vérifie le format
                DateTime Heured;

                if (DateTime.TryParse(mTBHeureDeb.Text.Substring(0, 2) + ":" + mTBHeureDeb.Text.Substring(2, 2), out Heured) == true)
                    V1 += 1;
                else
                {
                    MessageBox.Show("Vous devez saisir une heure de début de garde valide. ", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else MessageBox.Show("Vous devez saisir une heure de début de garde. ", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);                                         

            if (mTBHeureFin.Text != "")
            {
                //On vérifie le format
                DateTime HeureF;

                if (DateTime.TryParse(mTBHeureFin.Text.Substring(0, 2) + ":" + mTBHeureFin.Text.Substring(2, 2), out HeureF) == true)
                    V1 += 1;
                else
                {
                    MessageBox.Show("Vous devez saisir une heure de fin de garde valide.. ", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else MessageBox.Show("Vous devez saisir une heure de fin de garde. ", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);    

            //En fonction des résultats
            switch (V1)
            {
                case 3: retour = "OK"; break;
                default: retour = "KO"; break;
            }

            return retour;
          }

          //On ajoute un nvl enregistrement 
          public string AjoutEnreg()
          {
              string Retour = "KO";

              //Chaine de connection... ici on attaque MariaDB
              string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Regul"].ToString();
              MySqlConnection dbConnection = new MySqlConnection(connex);

              dbConnection.Open();
              MySqlCommand cmd = new MySqlCommand();

              cmd.Connection = dbConnection;

              //On déclare ici la requete (à cause de la transaction)
              string SqlStr0 = "";

              try
              {
                  //Puis dans une transaction
                  MySqlTransaction trans;

                  //Def de la requete
                  //*****Commune***
                  SqlStr0 = "INSERT INTO garde ";
                  SqlStr0 += " (TypeGarde, HeureDeb, HeureFin)";
                  SqlStr0 += " VALUES('" + tBTypeGarde.Text + "','" + mTBHeureDeb.Text.Substring(0, 2) + ":" + mTBHeureDeb.Text.Substring(2, 2) + "','";
                  SqlStr0 += mTBHeureFin.Text.Substring(0, 2) + "." + mTBHeureFin.Text.Substring(2, 2) + "')";

                 
                  //Ouverture d'une transaction
                  trans = dbConnection.BeginTransaction();
                  try
                  {
                      //on execute les requettes                                       
                      cmd.CommandText = SqlStr0; cmd.Transaction = trans; cmd.ExecuteNonQuery();

                      //on valide la transaction
                      trans.Commit();

                      Retour = "OK";
                  }
                  catch (Exception e)
                  {
                      trans.Rollback();
                      MessageBox.Show("Erreur lors de la création de la garde (transaction). " + e.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                  }
                  finally
                  {
                      //fermeture des connexions
                      if (dbConnection.State == System.Data.ConnectionState.Open)
                      {
                          dbConnection.Close();
                      }
                  }
              }
              catch (Exception e)
              {
                  MessageBox.Show("Erreur lors de la création de la garde. " + e.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);

              }
              finally
              {
                  //fermeture des connexions
                  if (dbConnection.State == System.Data.ConnectionState.Open)
                  {
                      dbConnection.Close();
                  }
              }

              return Retour;
          }


          //On met à jour un enregistrement
          private string UpdateEnreg()
          {
              string Retour = "KO";

              //Chaine de connection... ici on attaque MariaDB
              string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Regul"].ToString();
              MySqlConnection dbConnection = new MySqlConnection(connex);

              dbConnection.Open();
              MySqlCommand cmd = new MySqlCommand();

              cmd.Connection = dbConnection;

              //On déclare ici la requete (à cause de la transaction)
              string SqlStr0 = "";

              try
              {
                  //Puis dans une transaction
                  MySqlTransaction trans;

                  //Def de la requete
                  //*****Garde***
                  SqlStr0 = "UPDATE garde ";
                  SqlStr0 += " SET TypeGarde = '" + tBTypeGarde.Text + "', HeureDeb = '" + mTBHeureDeb.Text.Substring(0, 2) + ":" + mTBHeureDeb.Text.Substring(2, 2);
                  SqlStr0 += "', HeureFin = '" + mTBHeureFin.Text.Substring(0, 2) + ":" + mTBHeureFin.Text.Substring(2, 2) + "'";
                  SqlStr0 += " WHERE IdGarde = '" + lIdGarde.Text + "'";

                  //Ouverture d'une transaction
                  trans = dbConnection.BeginTransaction();
                  try
                  {
                      //on execute les requettes                                       
                      cmd.CommandText = SqlStr0; cmd.Transaction = trans; cmd.ExecuteNonQuery();

                      //on valide la transaction
                      trans.Commit();
                      Retour = "OK";
                  }
                  catch (Exception e)
                  {
                      trans.Rollback();
                      MessageBox.Show("Erreur lors de la modification de la garde (transaction). " + e.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                  }
                  finally
                  {
                      //fermeture des connexions
                      if (dbConnection.State == System.Data.ConnectionState.Open)
                      {
                          dbConnection.Close();
                      }
                  }
              }
              catch (Exception e)
              {
                  MessageBox.Show("Erreur lors de la modification de la garde. " + e.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);

              }
              finally
              {
                  //fermeture des connexions
                  if (dbConnection.State == System.Data.ConnectionState.Open)
                  {
                      dbConnection.Close();
                  }
              }

              return Retour;
          }

          private void bSupprimer_Click(object sender, EventArgs e)
          {
              //On essaie de supprimer la garde...S'il elle n'a jamais été utilisée (HistoGarde)
              DataTable dtExiste = FonctionsAppels.RechercheHisto("IdGarde", lIdGarde.Text);
              if (dtExiste.Rows.Count > 0)
                  MessageBox.Show("Impossible de supprimer cette garde car elle a déjà été utilisée. ", "Suppression", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              else
              {
                  //Suppression de l'enregistrement
                  if (FonctionsAppels.SupprGarde(lIdGarde.Text) == "OK")
                  {
                      MessageBox.Show("Suppression de la garde => OK ", "Suppression", MessageBoxButtons.OK, MessageBoxIcon.Information);

                      VideChamps();
                      //Gestion des boutons
                      bAjouter.Visible = true;
                      bValider.Enabled = false;
                      bCancel.Visible = false;
                      bSupprimer.Enabled = false;

                      Etat = "Lecture";

                      //On rafraichi la liste des gardes
                      FGarde_Load(sender, e);
                  }
              }
          }

          private void bCancel_Click(object sender, EventArgs e)
          {
              //on annule
              VideChamps();
              //Gestion des boutons
              bAjouter.Visible = true;
              bValider.Enabled = false;
              bCancel.Visible = false;
              bSupprimer.Enabled = false;

              Etat = "Lecture";

              //On rafraichi la liste des médecins
              FGarde_Load(sender, e);
          }

          private void bExit_Click(object sender, EventArgs e)
          {
              if (Etat != "Lecture")
              {
                  var result = MessageBox.Show("Il y a des modification en cours...Voulez vous les abandonner? ", "Abandon", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                  if (result == DialogResult.Yes)
                      this.Close();
              }
              else
                  this.Close();
          }

          private void tBTypeGarde_Click(object sender, EventArgs e)
          {
              //si on est en lecture, on passe en ajout
              if (Etat == "Lecture")
              {
                  bAjouter_Click(sender, e);
              }
          }

        
        //Si on appuie sur enter
        private void mTBHeureFin_KeyDown(object sender, KeyEventArgs e)
          {                        
              if (e.KeyCode == Keys.Enter)
              {
                  bValider_Click(sender, e);
              }
          }

       




    }
}
