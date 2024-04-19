using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EmployeeManagement.GUI
{
    public partial class FormChangePassword : Form
    {
        public FormChangePassword()
        {
            InitializeComponent();
        }

        private void lblShowPassword_Click(object sender, EventArgs e)
        {
            if (lblShowPassword.Text == "show password")
            {
                lblShowPassword.Text = "hide password";
                txtPassword.PasswordChar = '\0';
                txtPasswordConfirm.PasswordChar = '\0';
            }
            else
            {
                lblShowPassword.Text = "show password";
                txtPassword.PasswordChar = '*';
                txtPasswordConfirm.PasswordChar = '*';
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            // Implement functionality to set a new password to the user account
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
