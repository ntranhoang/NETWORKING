using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CLIENT
{
    public partial class readDialog : Form
    {
        public readDialog(string Data)
        {
            InitializeComponent();
            readBox.Text = Data;
        }

        private void readDialog_Load(object sender, EventArgs e)
        {

        }
    }
}
