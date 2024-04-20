using Hi_TechLibrary.BLL.ADO;
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
        private int userId;
        private bool isReset;

        public FormChangePassword()
        {
            InitializeComponent();
            this.KeyPreview = true;
        }
        // Overloaded constructor for password reset
        public FormChangePassword(int userId, bool isReset)
        {
            InitializeComponent();
            this.userId = userId;
            this.isReset = isReset;
            lblOldPassword.Visible = !isReset;
            txtOldPassword.Visible = !isReset;
        }

        private void lblShowPassword_Click(object sender, EventArgs e)
        {
            if (lblShowPassword.Text == "show password")
            {
                lblShowPassword.Text = "hide password";
                txtOldPassword.PasswordChar = '\0';
                txtPassword.PasswordChar = '\0';
                txtPasswordConfirm.PasswordChar = '\0';
            }
            else
            {
                lblShowPassword.Text = "show password";
                txtOldPassword.PasswordChar = '*';
                txtPassword.PasswordChar = '*';
                txtPasswordConfirm.PasswordChar = '*';
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            string newPassword = txtPassword.Text;
            string confirmPassword = txtPasswordConfirm.Text;
            if (newPassword != confirmPassword)
            {
                MessageBox.Show("The new passwords do not match.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // Retrieve the current user details
            UserAccount userAccount = new UserAccount().SearchUserAccountByUserAccountId(userId);
            if (userAccount == null)
            {
                MessageBox.Show("User account not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!isReset)
            {
                string oldPassword = txtOldPassword.Text.Trim();
                if (userAccount.Password != oldPassword)
                {
                    MessageBox.Show("The old password is incorrect.", "Authentication Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            // Update the password in the database and set MustChangePassword to false
            userAccount.Password = newPassword;
            userAccount.MustChangePassword = false;
            userAccount.UpdateUserAccount(userAccount);
            MessageBox.Show("Password changed successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtPasswordConfirm_KeyDown(object sender, KeyEventArgs e)
        {
            btnOK.PerformClick();
            e.Handled = true; //Mark the event as handled
            e.SuppressKeyPress = true; // Prevent the system from processing the key press further
        }
    }
}
