using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsGuiClient
{
    public partial class AddHostForm : Form
    {
        public AddHostForm()
        {
            InitializeComponent();
            this.button1.DialogResult = DialogResult.OK;
        }

        public String getName()
        {
            return this.txtName.Text;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
