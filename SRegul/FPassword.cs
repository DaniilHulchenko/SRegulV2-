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
    public partial class FPassword : Form
    {
        public string MotDePassEnClair = "";

        public FPassword()
        {
            InitializeComponent();
        }

        private void FPassword_Load(object sender, EventArgs e)
        {
            //On attribu le mot de passe (s'il y en a un)
            if (MotDePassEnClair != "")
                tBAncienP.Text = MotDePassEnClair;
        }

        private void bMasqueAffiche_Click(object sender, EventArgs e)
        {
            //On affiche/Masque les mot de passe
            if (tBAncienP.PasswordChar == '*')
            {
                tBAncienP.PasswordChar = new char();
                tBNvxP.PasswordChar = new char();
                tBReP.PasswordChar = new char();
            }
            else
            {
                tBAncienP.PasswordChar = '*';
                tBNvxP.PasswordChar = '*';
                tBReP.PasswordChar = '*';
            }
        }


        //On compare avec la chaine du Haut
        private void tBReP_TextChanged(object sender, EventArgs e)
        {
            int nbchar = 0;            
            //On compte le nombre de caratère tapé
            nbchar = tBReP.Text.Length;

            string pass = tBNvxP.Text.Substring(0, nbchar);

            if (tBReP.Text != pass)
            {
                tBReP.ForeColor = Color.Red;    //On change ou pas la couleur en fonction du résultat
                bValider.Enabled = false;
            }
            else
            {
                tBReP.ForeColor = SystemColors.Window;
                bValider.Enabled = true;
            }
        }

        private void bValider_Click(object sender, EventArgs e)
        {
            if (tBNvxP.Text != "")
                FUtilisateur.PassEnClair = tBNvxP.Text;

            //On ferme la forme
            this.Close();
        }

        private void bCancel_Click(object sender, EventArgs e)
        {
            //On ferme la forme
            this.Close();
        }


       
      
    }
}


//A faire
