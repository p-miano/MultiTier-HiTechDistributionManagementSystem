﻿using System;
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

namespace EmployeeManagement.GUI
{
    public partial class FormEmployee : Form
    {
        private int? employeeId;
        public FormEmployee()
        {
            InitializeComponent();
            PopulatePositionsComboBox();
            SetInitialState();
        }
        // Overloaded constructor to receive the employeeId from the UserAccount form
        public FormEmployee(int employeeId)
        {
            InitializeComponent();
            PopulatePositionsComboBox();
            this.employeeId = employeeId;
            SetInitialState();
            SearchByEmployeeId(employeeId.ToString());
        }

        #region Form Utlities
        private void SetInitialState()
        {
            btnUpdate.Enabled = false;
            btnDeleteEmployee.Enabled = false;
            btnLinkUserAccount.Enabled = false;
            btnClearAll.Enabled = false;
            txtBoxEmployeeId.ReadOnly = false;
        }
        private void SetStateAfterSuccessfulSearch()
        {
            btnUpdate.Enabled = true;
            btnDeleteEmployee.Enabled = true;
            btnLinkUserAccount.Enabled = true;
            btnClearAll.Enabled = true;
            btnAddNewEmployee.Enabled = false;
            txtBoxEmployeeId.ReadOnly = true;
        }
        private void SetStateAfterListOrFailedSearch()
        {
            btnUpdate.Enabled = false;
            btnDeleteEmployee.Enabled = false;
            btnLinkUserAccount.Enabled = false;
            btnClearAll.Enabled = true;
            btnAddNewEmployee.Enabled = false;
        }
        private void PopulatePositionsComboBox()
        {
            cmbBoxPosition.Items.Clear();
            List<Position> positions = new Position().GetAllPositions();
            Dictionary<int, string> positionDictionary = new Dictionary<int, string>();
            foreach (var position in positions)
            {
                positionDictionary.Add(position.PositionID, position.PositionTitle);
            }
            // Enable autocomplete
            cmbBoxPosition.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cmbBoxPosition.AutoCompleteSource = AutoCompleteSource.ListItems;
            // Set the combobox DataSource to the dictionary
            cmbBoxPosition.DataSource = new BindingSource(positionDictionary, null);
            cmbBoxPosition.DisplayMember = "Value";
            cmbBoxPosition.ValueMember = "Key";
            // Set the SelectedIndex to -1 to show the ComboBox as empty
            cmbBoxPosition.SelectedIndex = -1;
        }
        private void ClearAll()
        {
            txtBoxEmployeeId.Clear();
            txtBoxFirstName.Clear();
            txtBoxLastName.Clear();
            txtBoxEmail.Clear();
            txtBoxPhoneNumber.Clear();
            cmbBoxPosition.SelectedIndex = -1;
            dtGridEmployees.DataSource = null;
            SetInitialState();
        }
        private void UpdateEmployeeInfoUI(Employee employee)
        {
            txtBoxEmployeeId.Text = employee.EmployeeID.ToString();
            txtBoxFirstName.Text = employee.FirstName;
            txtBoxLastName.Text = employee.LastName;
            txtBoxEmail.Text = employee.Email;
            txtBoxPhoneNumber.Text = employee.PhoneNumber;
            cmbBoxPosition.SelectedValue = employee.PositionID;
            SetStateAfterSuccessfulSearch();
        }
        private bool ValidateEmployeeInput()
        {
            // Validate First Name
            if (!Validator.IsValidName(txtBoxFirstName.Text.Trim()))
            {
                MessageBox.Show("Invalid first name.", "Input Error");
                txtBoxFirstName.Focus();
                return false;
            }
            // Validate Last Name
            if (!Validator.IsValidName(txtBoxLastName.Text.Trim()))
            {
                MessageBox.Show("Invalid last name.", "Input Error");
                txtBoxLastName.Focus();
                return false;
            }
            // Validate Email
            if (!Validator.IsValidEmail(txtBoxEmail.Text.Trim()))
            {
                MessageBox.Show("Invalid email address.", "Input Error");
                txtBoxEmail.Focus();
                return false;
            }
            // All validations passed
            return true;
        }
        #endregion


        #region Search
        private void SearchByEmployeeId(string employeeIdInput)
        {
            int employeeId = Convert.ToInt32(employeeIdInput);
            Employee employee = new Employee().SearchEmployeeById(employeeId);
            if (employee != null)
            {
                UpdateEmployeeInfoUI(employee);
                SetStateAfterSuccessfulSearch();
            }
            else
            {
                MessageBox.Show("Employee ID not found", "Search Employee");
                SetStateAfterListOrFailedSearch();
            }
        }
        private void SearchByEmployeeEmail(string emailInput)
        {
            Employee employee = new Employee().SearchEmployeeByEmail(emailInput);
            if (employee != null)
            {
                UpdateEmployeeInfoUI(employee);
                SetStateAfterSuccessfulSearch();
            }
            else
            {
                MessageBox.Show("Employee email not found", "Search Employee");
                SetStateAfterListOrFailedSearch();
            }
        }
        private void SearchByEmployeePosition(string positionInput)
        {
            int positionID = ((KeyValuePair<int, string>)cmbBoxPosition.SelectedItem).Key;
            List<Employee> employees = new Employee().SearchEmployeesByPosition(positionID);
            DisplaySearchResults(employees);
            SetStateAfterListOrFailedSearch();
        }
        private void SearchByEmployeeFullName(string firstNameInput, string lastNameInput)
        {
            List<Employee> employees = new Employee().SearchEmployees(firstNameInput, lastNameInput);
            DisplaySearchResults(employees);
            SetStateAfterListOrFailedSearch();
        }
        // Search employee by first name, last name, phone number or position
        private void SearchByEmployeeInput(string input)
        {
            List<Employee> employees = new Employee().SearchEmployees(input);
            DisplaySearchResults(employees);
            SetStateAfterListOrFailedSearch();
        }
        private void DisplaySearchResults(List<Employee> employees)
        {
            if (employees.Any())
            {
                var dataSource = employees.Select(emp => new
                {
                    EmployeeID = emp.EmployeeID,
                    FirstName = emp.FirstName,
                    LastName = emp.LastName,
                    Email = emp.Email,
                    PhoneNumber = emp.PhoneNumber,
                    PositionTitle = new Position().GetPositionById(emp.PositionID) // Assume GetPositionById() returns the position title
                }).ToList();
                dtGridEmployees.DataSource = dataSource;
                dtGridEmployees.Columns["EmployeeID"].HeaderText = "Employee ID";
                dtGridEmployees.Columns["FirstName"].HeaderText = "First Name";
                dtGridEmployees.Columns["LastName"].HeaderText = "Last Name";
                dtGridEmployees.Columns["Email"].HeaderText = "Email";
                dtGridEmployees.Columns["PhoneNumber"].HeaderText = "Phone Number";
                dtGridEmployees.Columns["PositionTitle"].HeaderText = "Position";
                dtGridEmployees.Columns["PositionTitle"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
            else
            {
                MessageBox.Show("No employees found", "Search Result");
                dtGridEmployees.DataSource = null;
            }
        }

        #endregion


        #region Buttons
        private void btnLinkUserAccount_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtBoxEmployeeId.Text.Trim(), out int parsedId))
            {
                MessageBox.Show("Invalid Employee ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // Attempt to find an existing user account for this employee
            UserAccount userAccount = new UserAccount().SearchUserAccountByEmployeeId(parsedId);
            // If a user account exists, open the form in edit mode
            if (userAccount != null && userAccount.UserID > 0)
            {
                FormUserAccount formUserAccount = new FormUserAccount(parsedId, false);
                formUserAccount.ShowDialog();
                this.Close();
            }
            // If no user account exists, ask if one should be created
            else
            {
                DialogResult result = MessageBox.Show("No User Account found for this employee. Do you want to create one?", "User Account", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    FormUserAccount formUserAccount = new FormUserAccount(parsedId, true);
                    formUserAccount.ShowDialog();
                    this.Close();
                }
            }
        }

        private void btnListAllEmployees_Click(object sender, EventArgs e)
        {
            ClearAll();
            SetInitialState();
            List<Employee> employees = new Employee().GetAllEmployees();
            DisplaySearchResults(employees);
        }
        private void btnAddNewEmployee_Click(object sender, EventArgs e)
        {
            if (!ValidateEmployeeInput())
            {
                return;
            }
            if (cmbBoxPosition.SelectedIndex >= 0) // Ensure a position is selected
            {
                int selectedPositionID = ((KeyValuePair<int, string>)cmbBoxPosition.SelectedItem).Key;
                Employee employee = new Employee
                {
                    FirstName = txtBoxFirstName.Text.Trim(),
                    LastName = txtBoxLastName.Text.Trim(),
                    Email = txtBoxEmail.Text.Trim(),
                    PhoneNumber = txtBoxPhoneNumber.Text.Trim(),
                    PositionID = selectedPositionID
                };
                int newEmployeeId = employee.SaveEmployee(employee);
                if (newEmployeeId > 0)
                {
                    MessageBox.Show("Employee record saved successfully", "Save Employee");
                    employee.EmployeeID = newEmployeeId;
                    UpdateEmployeeInfoUI(employee);
                }
                else
                {
                    MessageBox.Show("Failed to save employee.", "Error");
                }
            }
            else
            {
                MessageBox.Show("Please select a position.", "Position Required");
            }
        }
        private void btnSearch_Click(object sender, EventArgs e)
        {
            // Employee search fields
            string employeeIdInput = txtBoxEmployeeId.Text.Trim();
            string firstNameInput = txtBoxFirstName.Text.Trim();
            string lastNameInput = txtBoxLastName.Text.Trim();
            string emailInput = txtBoxEmail.Text.Trim();
            string phoneNumberInput = txtBoxPhoneNumber.Text.Trim();
            string positionInput = cmbBoxPosition.Text.Trim();

            // Call method to search by Employee ID (highest priority)
            if (!string.IsNullOrEmpty(employeeIdInput))
            {
                if (Validator.IsValidID(employeeIdInput))
                {
                    SearchByEmployeeId(employeeIdInput);
                    return;
                }
                else
                {
                    MessageBox.Show("Invalid Employee ID. Please enter a numeric value.", "Validation Error");
                    txtBoxEmployeeId.Focus();
                    return;
                }
            }
            // Call method to search by Email (third priority)
            else if (!string.IsNullOrEmpty(emailInput))
            {
                if (Validator.IsValidEmail(emailInput))
                {
                    SearchByEmployeeEmail(emailInput);
                    return;
                }
                else
                {
                    MessageBox.Show("Invalid Email format. Please enter a valid email.", "Validation Error");
                    txtBoxEmail.Focus();
                    return;
                }
            }
            // Call method to search by First and Last Name. Fills the grid view with the results
            else if (!string.IsNullOrEmpty(firstNameInput) && !string.IsNullOrEmpty(lastNameInput))
            {
                SearchByEmployeeFullName(firstNameInput, lastNameInput);
                return;
            }
            // Call method to search by Position. Fills the grid view with the results
            else if (!string.IsNullOrEmpty(positionInput))
            {
                SearchByEmployeePosition(positionInput);
                return;
            }
            // Call method to search by First Name or Last Name or Phone Number. Fills the grid view with the results
            else if (!string.IsNullOrEmpty(firstNameInput) || !string.IsNullOrEmpty(lastNameInput) || !string.IsNullOrEmpty(phoneNumberInput))
            {
                if (!string.IsNullOrEmpty(firstNameInput))
                {
                    SearchByEmployeeInput(firstNameInput);
                }
                else if (!string.IsNullOrEmpty(lastNameInput))
                {
                    SearchByEmployeeInput(lastNameInput);
                }
                else if (!string.IsNullOrEmpty(phoneNumberInput))
                {
                    SearchByEmployeeInput(phoneNumberInput);
                }
            }
            else
            {
                MessageBox.Show("Please enter an information to search", "Search Employee");
            }
        }
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!ValidateEmployeeInput())
            {
                return;
            }
            int employeeId = Convert.ToInt32(txtBoxEmployeeId.Text.Trim());
            Employee employee = new Employee().SearchEmployeeById(employeeId);
            if (employee != null)
            {
                employee.FirstName = txtBoxFirstName.Text.Trim();
                employee.LastName = txtBoxLastName.Text.Trim();
                employee.Email = txtBoxEmail.Text.Trim();
                employee.PhoneNumber = txtBoxPhoneNumber.Text.Trim();
                employee.PositionID = ((KeyValuePair<int, string>)cmbBoxPosition.SelectedItem).Key;
                employee.UpdateEmployee(employee);
                MessageBox.Show("Employee record updated successfully", "Update Employee");
            }
            else
            {
                MessageBox.Show("Employee ID not found", "Update Employee");
            }
        }
        private void btnDeleteEmployee_Click(object sender, EventArgs e)
        {
            Employee employee = new Employee();
            employee.EmployeeID = Convert.ToInt32(txtBoxEmployeeId.Text.Trim());
            if (MessageBox.Show("Are you sure you want to delete this employee from the database?", "Delete Employee", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                employee.DeleteEmployee(employee.EmployeeID);
                MessageBox.Show("Employee record deleted successfully", "Confirmation", MessageBoxButtons.OK);
                ClearAll();
            }
        }
        private void btnClearAll_Click(object sender, EventArgs e)
        {
            ClearAll();
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

        private void dtGridEmployees_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dtGridEmployees.Rows[e.RowIndex];
                txtBoxEmployeeId.Text = row.Cells["EmployeeID"].Value.ToString();
                txtBoxFirstName.Text = row.Cells["FirstName"].Value.ToString();
                txtBoxLastName.Text = row.Cells["LastName"].Value.ToString();
                txtBoxEmail.Text = row.Cells["Email"].Value.ToString();
                txtBoxPhoneNumber.Text = row.Cells["PhoneNumber"].Value.ToString();
                string positionTitle = row.Cells["PositionTitle"].Value.ToString();
                foreach (var item in cmbBoxPosition.Items)
                {
                    KeyValuePair<int, string> pair = (KeyValuePair<int, string>)item;
                    if (pair.Value == positionTitle)
                    {
                        cmbBoxPosition.SelectedValue = pair.Key;
                        break;
                    }
                }
                SetStateAfterSuccessfulSearch();
            }
        }
    }
}
