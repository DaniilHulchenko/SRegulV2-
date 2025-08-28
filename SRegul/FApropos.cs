using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SRegulV2
{
    public partial class FApropos : Form
    {
        public FApropos()
        {
            InitializeComponent();

            var version = Assembly.GetExecutingAssembly().GetName().Version;
            FileInfo fileInfo = new System.IO.FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location);
            label2.Text = " V" + version.Major + "." + version.Minor + " R" + fileInfo.LastWriteTime.ToString("ddMMyyHHmm");

        }

        private void b1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
