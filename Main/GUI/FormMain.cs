using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EmployeeManagement.GUI;
using InventoryManager.GUI;
using OrderManager.GUI;
using Hi_TechLibrary.BLL.ADO;
using CustomerManager.GUI;

namespace Main.GUI
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
            InitializeState();
        }

        public void InitializeState()
        {
            this.KeyPreview = true;
            btnChangePassword.Visible = false;
            btnLogout.Visible = false;
            panelWelcome.Visible = false;
            btnEmployee.Enabled = false;
            btnUserAccount.Enabled = false;
            btnCustomer.Enabled = false;
            btnInventory.Enabled = false;
            btnOrders.Enabled = false;
            gbLogin.Visible = true;
            txtUsername.Text = "";
            txtPassword.Text = "";
        }
        public void UpdateButtonEnabled(UserRole userRole)
        {
            //btnEmployee.Enabled = false;
            //btnUserAccount.Enabled = false;
            //btnCustomer.Enabled = false;
            //btnInventory.Enabled = false;
            //btnOrders.Enabled = false;
            // Update visibility based on user role
            switch (userRole)
            {
                case UserRole.Administrator:
                    btnEmployee.Enabled = true;
                    btnInventory.Enabled = true;
                    btnOrders.Enabled = true;
                    btnUserAccount.Enabled = true;
                    btnCustomer.Enabled = true;
                    break;
                case UserRole.MISManager:
                    btnEmployee.Enabled = true;
                    btnUserAccount.Enabled = true;
                    break;
                case UserRole.SalesManager:
                    btnCustomer.Enabled = true;
                    break;
                case UserRole.InventoryController:
                    btnInventory.Enabled = true;                    
                    break;
                case UserRole.OrderClerk:
                    btnOrders.Enabled = true;
                    break;
            }
        }

        private void btnEmployee_Click(object sender, EventArgs e)
        {
            FormEmployee formEmployee = new FormEmployee();
            formEmployee.ShowDialog();
        }
        private void btnUserAccount_Click(object sender, EventArgs e)
        {
            FormUserAccount formUserAccount = new FormUserAccount();
            formUserAccount.ShowDialog();
        }
        private void btnCustomer_Click(object sender, EventArgs e)
        {
            FormCustomer formCustomer = new FormCustomer();
            formCustomer.ShowDialog();
        }
        private void btnInventory_Click(object sender, EventArgs e)
        {
            FormInventory formInventory = new FormInventory();
            formInventory.ShowDialog();
        }
        private void btnOrders_Click(object sender, EventArgs e)
        {
            FormOrder formOrders = new FormOrder();
            formOrders.ShowDialog();
        }
        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Both username and password are required.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            UserAccount userAccount = new UserAccount().Login(username, password);
            if (userAccount == null)
            {
                MessageBox.Show("Invalid username or password.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (userAccount.MustChangePassword)
            {
                MessageBox.Show("You must change your password before you can continue.", "Change Password Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                FormChangePassword formChangePassword = new FormChangePassword(userAccount.UserID, false);
                formChangePassword.ShowDialog();
                txtPassword.Text = "";
            }
            else
            {
                MessageBox.Show($"Login Successful!", "Login Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                panelWelcome.Visible = true;
                btnChangePassword.Visible = true;
                btnLogout.Visible = true;
                gbLogin.Visible = false;
                UpdateButtonEnabled(userAccount.UserRole);
            }
        }
        private void btnChangePassword_Click(object sender, EventArgs e)
        {
            UserAccount userAccount = new UserAccount().SearchUserAccountByUsername(txtUsername.Text.Trim());
            FormChangePassword formChangePassword = new FormChangePassword(userAccount.UserID, false);
            formChangePassword.ShowDialog();
        }

        private void lblForgotPassword_Click(object sender, EventArgs e)
        {
            // Check if the username field is filled
            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                MessageBox.Show("Please enter your username first.", "Username Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            UserAccount userAccount = new UserAccount().SearchUserAccountByUsername(txtUsername.Text.Trim());
            if (userAccount != null)
            {
                FormChangePassword formChangePassword = new FormChangePassword(userAccount.UserID, true);
                formChangePassword.ShowDialog();
                txtPassword.Text = "";
            }
            else
            {
                MessageBox.Show("Username not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure you want to exit?", "Exit", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
        private void lblShowPassword_Click(object sender, EventArgs e)
        {
            if (lblShowPassword.Text == "show password")
            {
                lblShowPassword.Text = "hide password";
                txtPassword.PasswordChar = '\0';
            }
            else
            {
                lblShowPassword.Text = "show password";
                txtPassword.PasswordChar = '*';
            }
        }

        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnLogin.PerformClick();
                e.Handled = true; //Mark the event as handled
                e.SuppressKeyPress = true; // Prevent the system from processing the key press further
            }
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure you want to logout?", "Logout", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                InitializeState();
            }
        }
    }
}
