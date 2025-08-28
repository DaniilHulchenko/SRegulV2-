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

    //Menu Popus Fait maison avec positionnement au niveau du pointeur
    public partial class CustomMenuBox1 : Form
    {
        private int PositionX;
        private int PositionY;
       
        public CustomMenuBox1(string buttonText1, string buttonText2, int x, int y)
        {
            InitializeComponent();
            PositionX = x;
            PositionY = y;
            
            button1.Text = buttonText1;
            button2.Text = buttonText2;

            Load += new EventHandler(CustomMenuBox1_Load);

            button1.DialogResult = DialogResult.OK;
            button2.DialogResult = DialogResult.No;
        }

        private void CustomMenuBox1_Load(object sender, EventArgs e)
        {
            //Au chargement on récupère la position de la souris, passé en paramètre
            SetDesktopLocation(PositionX, PositionY);
        }
       
    }
}
