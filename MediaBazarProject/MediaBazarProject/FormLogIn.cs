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
    public partial class FormLogIn : Form
    {
        UserController uc;
        public FormLogIn()
        {
            InitializeComponent();
            uc = new UserController();

            //Load Remember me 
            if (Properties.Settings.Default.saveUsername != string.Empty)
            {
                tbUsername.text = Properties.Settings.Default.saveUsername;
                tbPassword.text = Properties.Settings.Default.savePassword;
                cbxRememberMe.Checked = true;
            }
            else
            {
                cbxRememberMe.Checked = false;
            }
        }

        private void RememberMe()
        {
            //Remember me functionality
            if (cbxRememberMe.Checked)
            {
                Properties.Settings.Default.saveUsername = tbUsername.text;
                Properties.Settings.Default.savePassword = tbPassword.text;
                Properties.Settings.Default.Save();
            }
            else
            {
                Properties.Settings.Default.saveUsername = string.Empty;
                Properties.Settings.Default.savePassword = string.Empty;
                Properties.Settings.Default.Save();
                tbUsername.text = "";
                tbPassword.text = "";
            }
        }

        private void tbPassword_OnTextChange(object sender, EventArgs e)
        {
            tbPassword._TextBox.PasswordChar = '*';
        }

        private void lblRememberMe_Click(object sender, EventArgs e)
        {
            if (cbxRememberMe.Checked)
            {
                cbxRememberMe.Checked = false;
            }
            else
            {
                cbxRememberMe.Checked = true;
            }
        }

        private void btnLogIn_Click(object sender, EventArgs e)
        {
            //Login
            if(uc.Login(tbUsername.text, tbPassword.text))
            {
                uc.ExtractAllUsers();
                int id = uc.GetCurrentUserId();
                if (uc.GetUser(id).Role == Role.ADMINISTRATOR)
                {
                    Form formAdmin = new FormAdmin(this.FindForm(), id);
                    RememberMe();
                    this.Hide();
                    formAdmin.ShowDialog();
                    this.Show();
                }
                else if(uc.GetUser(id).Role == Role.MANAGER)
                {
                    Form formManager = new FormManager(id);
                    RememberMe();
                    this.Hide();
                    formManager.ShowDialog();
                    this.Show();
                }
                else
                {
                    CustomMessageBoxController.ShowMessage("Employees should log in via the website.", MessageBoxButtons.OK);
                }
            }
            else
            {
                CustomMessageBoxController.ShowMessage("Invalid credentials. Please try again.", MessageBoxButtons.OK);
            }
        }

        private void btnCloseApp_Click(object sender, EventArgs e)
        {
            System.Environment.Exit(1);
        }

    }
}
