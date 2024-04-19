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


namespace Main.GUI
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
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
            MessageBox.Show("Customer module is not implemented yet.");
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
            MessageBox.Show("Login functionality is not implemented yet.");
        }
        private void lblForgotPassword_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Forgot password functionality is not implemented yet.");
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure you want to exit?", "Exit", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
    }
}
