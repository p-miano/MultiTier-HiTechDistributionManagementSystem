using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Hi_TechLibrary.BLL.ADO;
using Hi_TechLibrary.VALIDATION;
using Hi_TechLibrary.UTILITIES;

namespace EmployeeManagement.GUI
{
    public partial class FormUserAccount : Form
    {
        private int? employeeId;
        public FormUserAccount()
        {
            InitializeComponent();
            PopulateUserRoleComboBox();
            SetInitialState();
        }
        // Overloaded constructor to receive the employeeId from the EmployeeManagement form
        public FormUserAccount(int employeeId, bool isNewAccount)
        {
            InitializeComponent();
            PopulateUserRoleComboBox();
            FillEmployeeInfo(employeeId);
            if (isNewAccount)
            {
                SetStateForCreatingNewUser();
            }
            else
            {
                FillUserAccountInfoUsingEmployeeId(employeeId);
                SetStateAfterSuccessfulSearch();
            }
            this.employeeId = employeeId;
        }

        #region Form Utilities
        private void SetInitialState()
        {
            btnUpdate.Enabled = false;
            btnDeleteUserAccount.Enabled = false;
            btnClearAll.Enabled = false;
            btnAddNewUserAccount.Enabled = true;
            btnSearch.Enabled = true;
            btnListAllUsers.Enabled = true;
            txtBoxEmployeeId.ReadOnly = true;
            txtBoxFirstName.ReadOnly = true;
            txtBoxLastName.ReadOnly = true;
            txtBoxEmail.ReadOnly = true;
            txtBoxPosition.ReadOnly = true;
            txtBoxUsername.ReadOnly = false;
            cmbBoxUserRole.Enabled = true;
        }
        private void SetStateAfterSuccessfulSearch()
        {
            btnUpdate.Enabled = true;
            btnDeleteUserAccount.Enabled = true;
            btnClearAll.Enabled = true;
            btnAddNewUserAccount.Enabled = false;
            txtBoxEmployeeId.ReadOnly = true;
            txtBoxFirstName.ReadOnly = true;
            txtBoxLastName.ReadOnly = true;
            txtBoxEmail.ReadOnly = true;
            txtBoxPosition.ReadOnly = true;
            txtBoxUsername.ReadOnly = false;
        }
        private void SetStateAfterListOrFailedSearch()
        {
            SetInitialState();
            btnListAllUsers.Enabled = true;
        }
        private void SetStateForCreatingNewUser()
        {
            btnUpdate.Enabled = false;
            btnDeleteUserAccount.Enabled = false;
            btnClearAll.Enabled = true;
            btnAddNewUserAccount.Enabled = true;
            btnSearch.Enabled = true;
            btnListAllUsers.Enabled = true;
            txtBoxEmployeeId.ReadOnly = true;
            txtBoxFirstName.ReadOnly = true;
            txtBoxLastName.ReadOnly = true;
            txtBoxEmail.ReadOnly = true;
            txtBoxPosition.ReadOnly = true;
            txtBoxUsername.ReadOnly = false;
            cmbBoxUserRole.Enabled = true;
        }
        private void FillEmployeeInfo(int employeeId)
        {
            Employee employee = new Employee().SearchEmployeeById(employeeId);
            if (employee != null)
            {
                txtBoxEmployeeId.Text = employee.EmployeeID.ToString();
                txtBoxFirstName.Text = employee.FirstName;
                txtBoxLastName.Text = employee.LastName;
                txtBoxEmail.Text = employee.Email;
                Position position = new Position();
                txtBoxPosition.Text = position.GetPositionById(employee.PositionID);
            }
        }
        private void FillUserAccountInfoUsingEmployeeId(int employeeId)
        {
            UserAccount userAccount = new UserAccount().SearchUserAccountByEmployeeId(employeeId);
            if (userAccount != null)
            {
                txtBoxUserId.Text = userAccount.UserID.ToString();
                txtBoxUsername.Text = userAccount.Username;                
                foreach (var item in cmbBoxUserRole.Items)
                {
                    if (item.ToString() == EnumUtilities.GetEnumDescription(userAccount.UserRole))
                    {
                        cmbBoxUserRole.SelectedItem = item;
                        break;
                    }
                }
            }
            else
            {
                txtBoxUserId.Clear();
                txtBoxUsername.Clear();
                cmbBoxUserRole.SelectedIndex = -1;
            }
        }
        private void PopulateUserRoleComboBox()
        {
            cmbBoxUserRole.Items.Clear();
            foreach (UserRole role in Enum.GetValues(typeof(UserRole)))
            {
                cmbBoxUserRole.Items.Add(EnumUtilities.GetEnumDescription(role));
            }
            cmbBoxUserRole.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cmbBoxUserRole.AutoCompleteSource = AutoCompleteSource.ListItems;
            cmbBoxUserRole.SelectedIndex = -1;
        }
        private void ClearAll()
        {
            txtBoxUserId.Clear();
            txtBoxUsername.Clear();
            cmbBoxUserRole.SelectedIndex = -1;
            dtGridUserAccounts.DataSource = null;
        }
        private void UpdateUserAccountInfoUI(UserAccount userAccount)
        {
            txtBoxUserId.Text = userAccount.UserID.ToString();
            txtBoxUsername.Text = userAccount.Username;
            cmbBoxUserRole.SelectedItem = EnumUtilities.GetEnumDescription(userAccount.UserRole);
        }
        private bool ValidateUserAccountInput()
        {
            if (!Validator.isValidUsername(txtBoxUsername.Text.Trim()))
            {
                MessageBox.Show("Invalid Username. Please enter a valid username.", "Validation Error");
                txtBoxUsername.Focus();
                return false;
            }
            return true;
        }
        #endregion


        #region Search
        private void SearchUserAccountByUserId(string userIdInput)
        {
            int userId = Convert.ToInt32(userIdInput);
            UserAccount userAccount = new UserAccount().SearchUserAccountByUserAccountId(userId);
            if (userAccount != null)
            {
                employeeId = userAccount.EmployeeID; // Update the employeeId global variable
                UpdateUserAccountInfoUI(userAccount);
                FillEmployeeInfo(employeeId.Value);
            }
            else
            {
                MessageBox.Show("User Account ID not found", "Search User Account");
            }
        }
        private void SearchUserAccountByUsername(string usernameInput)
        {
            UserAccount userAccount = new UserAccount().SearchUserAccountByUsername(usernameInput);
            if (userAccount != null)
            {
                employeeId = userAccount.EmployeeID;
                UpdateUserAccountInfoUI(userAccount);
                FillEmployeeInfo(employeeId.Value);
            }
            else
            {
                MessageBox.Show("Username not found", "Search User Account");
            }
        }
        private void SearchUserAccountByUserRole(string userRoleInput)
        {
            userRoleInput = userRoleInput.Replace(" ", "");
            UserRole selectedRole = (UserRole)Enum.Parse(typeof(UserRole), userRoleInput);
            List<UserAccount> userAccounts = new UserAccount().SearchUserAccountByUserRole(selectedRole);
            DisplaySearchResults(userAccounts);
        }
        private void DisplaySearchResults(List<UserAccount> userAccounts)
        {
            if (userAccounts.Any(user => user != null))
            {
                dtGridUserAccounts.DataSource = userAccounts.Where(user => user != null).ToList();
            }
            else
            {
                MessageBox.Show("No users found", "Search Result");
                dtGridUserAccounts.DataSource = null;
            }
        }
        #endregion


        #region Buttons
        private void btnLinkEmployee_Click(object sender, EventArgs e)
        {
            if (int.TryParse(txtBoxEmployeeId.Text.Trim(), out int employeeId))
            {
                Employee employee = new Employee().SearchEmployeeById(employeeId);
                if (employee != null)
                {
                    FormEmployee formEmployee = new FormEmployee(employeeId);
                    formEmployee.ShowDialog();
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("Invalid EmployeeManagement ID format.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnListAllUsers_Click(object sender, EventArgs e)
        {
            List<UserAccount> userAccounts = new UserAccount().GetAllUserAccounts();
            DisplaySearchResults(userAccounts);
        }
        private void btnAddNewUserAccount_Click(object sender, EventArgs e)
        {
            if (!ValidateUserAccountInput())
            {
                return;
            }
            if (txtBoxUsername.Text.Trim() != string.Empty)
            {
                if (cmbBoxUserRole.SelectedIndex >= 0) // Ensure a UserRole is selected
                {
                    // Retrieve the selected item, convert it to string, and remove spaces
                    string roleWithoutSpaces = cmbBoxUserRole.SelectedItem.ToString().Replace(" ", "");
                    // Parse the modified string to the UserRole enum
                    UserRole selectedRole = (UserRole)Enum.Parse(typeof(UserRole), roleWithoutSpaces);
                    // Generate a random password for the new account
                    string tempPassword = UserAccount.GenerateRandomPassword();
                    UserAccount userAccount = new UserAccount
                    {
                        EmployeeID = Convert.ToInt32(txtBoxEmployeeId.Text.Trim()),
                        Username = txtBoxUsername.Text.Trim(),
                        Password = tempPassword,
                        UserRole = selectedRole,
                        StatusID = 1, // Default status is Active
                        MustChangePassword = true // New users must change their password
                    };
                    int newUserId = userAccount.SaveUserAccount(userAccount);
                    txtBoxUserId.Text = newUserId.ToString();
                    MessageBox.Show($"User account for '{userAccount.Username}' was successfully created with a temporary password: {tempPassword}. Please ensure to change it on the first login.", "Save User Account", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Please select a user role.", "User Role Required");
                }
            }
        }
        private void btnSearch_Click(object sender, EventArgs e)
        {
            // UserAccount search fields
            string userIdInput = txtBoxUserId.Text.Trim();
            string usernameInput = txtBoxUsername.Text.Trim();
            string userRoleInput = cmbBoxUserRole.Text.Trim();
            // Call method to search by User ID (highest priority)
            if (!string.IsNullOrEmpty(userIdInput))
            {
                if (Validator.IsValidID(userIdInput))
                {
                    SearchUserAccountByUserId(userIdInput);
                    SetStateAfterSuccessfulSearch();
                    return;
                }
                else
                {
                    MessageBox.Show("Invalid User ID. Please enter a numeric value.", "Validation Error");
                    txtBoxUserId.Focus();
                    return;
                }
            }
            // Call method to search by Username (second priority)
            else if (!string.IsNullOrEmpty(usernameInput))
            {
                if (Validator.isValidUsername(usernameInput))
                {
                    SearchUserAccountByUsername(usernameInput);
                    SetStateAfterSuccessfulSearch();
                    return;
                }
                else
                {
                    MessageBox.Show("Invalid Username. Please enter a valid username.", "Validation Error");
                    txtBoxUsername.Focus();
                    return;
                }
            }
            // Call method to search by UserRole (third priority). Returns a list of users.
            else if (!string.IsNullOrEmpty(userRoleInput))
            {
                SearchUserAccountByUserRole(userRoleInput);
                SetStateAfterListOrFailedSearch();
            }
            else
            {
                MessageBox.Show("Please enter an information to search", "Search User Account");
            }
        }
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!ValidateUserAccountInput())
            {
                return;
            }
            if (!string.IsNullOrEmpty(txtBoxUserId.Text.Trim()))
            {
                int userId = Convert.ToInt32(txtBoxUserId.Text.Trim());
                UserAccount userAccount = new UserAccount().SearchUserAccountByUserAccountId(userId);
                if (userAccount != null)
                {
                    userAccount.Username = txtBoxUsername.Text.Trim();
                    userAccount.UserRole = (UserRole)Enum.Parse(typeof(UserRole), cmbBoxUserRole.SelectedItem.ToString().Replace(" ", ""));
                    userAccount.DateModified = DateTime.Now;
                    userAccount.UpdateUserAccount(userAccount);
                    MessageBox.Show("User account updated successfully", "Update User Account");
                }
                else
                {
                    MessageBox.Show("User Account ID not found", "Update User Account");
                }
            }
            else
            {
                MessageBox.Show("User Account ID not found", "Update User Account");
            }
        }
        private void btnDeleteUserAccount_Click(object sender, EventArgs e)
        {
            UserAccount userAccount = new UserAccount();
            userAccount.UserID = Convert.ToInt32(txtBoxUserId.Text.Trim());
            if (MessageBox.Show("Are you sure you want to delete this account from the database?", "Delete User Account", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                userAccount.DeleteUserAccount(userAccount.UserID);
                MessageBox.Show("User account deleted successfully", "Delete User Account");
                ClearAll();
            }
        }
        private void btnClearAll_Click(object sender, EventArgs e)
        {
            ClearAll();
            SetInitialState();
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to exit?", "Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                this.Close();
            }
        }
        #endregion

    }
}
