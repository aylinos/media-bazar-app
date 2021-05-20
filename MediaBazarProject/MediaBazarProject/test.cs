using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MediaBazarProject
{
    public partial class test : Form
    {
        UserController UserController;
        public test()
        {
            InitializeComponent();
            UserController = new UserController();           
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (UserController.Register(username.Text,password.Text))
            {
                MessageBox.Show("Success");
            }
        }
    }
}
