using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Hi_TechLibrary.BLL.EntityFramework;
using Microsoft.VisualBasic;

namespace Customer_Order.GUI
{
    public partial class FormOrder : Form
    {
        private readonly CustomerController customerController = new CustomerController();
        private readonly BookController bookController = new BookController();

        public FormOrder()
        {
            InitializeComponent();
            DesignTabButtons();
            PopulateCustomersComboBox();
            PopulateBookComboBox();
            PopulateFilterCustomerComboBox();
            PopulateFilterOrderComboBox();
            PopulateOrdersDataGrid();
            SetListView();
        }

        #region Form Utlities
        public void DesignTabButtons()
        {
            panelMain.Padding = new Padding(7); // Adjust the padding of the panel in which the tab control is docked

            // Set the TabControl mode and handle the DrawItem event
            tabCtrlOrder.SizeMode = TabSizeMode.Fixed;
            tabCtrlOrder.DrawMode = TabDrawMode.OwnerDrawFixed;
            tabCtrlOrder.ItemSize = new Size(220, 30); // Set the desired width and height

            tabCtrlOrder.DrawItem += (sender, e) =>
            {
                // Determine the color based on the state of the tab
                Brush textBrush;
                if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                {
                    textBrush = SystemBrushes.ControlText; // Text color when the tab is selected
                }
                else
                {
                    textBrush = SystemBrushes.ControlText; // Default text color
                }

                e.Graphics.FillRectangle(SystemBrushes.Control, e.Bounds); // Fill the background
                StringFormat sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                e.Graphics.DrawString(tabCtrlOrder.TabPages[e.Index].Text, e.Font, textBrush, e.Bounds, sf);
            };
        }
        private void PopulateCustomersComboBox()
        {
            // Fetch customers and sort them by name
            var customersList = customerController.GetAllCustomers()
                                                  .OrderBy(c => c.Name)
                                                  .Select(c => new KeyValuePair<int, string>(c.CustomerID, c.Name))
                                                  .ToList();
            // Set up the ComboBox
            cbCustomer.DataSource = customersList;
            cbCustomer.ValueMember = "Key";
            cbCustomer.DisplayMember = "Value";
            // Set properties for the autocomplete feature
            cbCustomer.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cbCustomer.AutoCompleteSource = AutoCompleteSource.ListItems;
            cbCustomer.SelectedIndex = -1;
        }
        private void PopulateBookComboBox()
        {
            // Fetch books and sort them by title
            var booksList = bookController.GetAllBooks()
                                          .OrderBy(b => b.Title)
                                          .Select(b => new KeyValuePair<string, string>(b.ISBN, b.Title))
                                          .ToList();
            // Set up the ComboBox
            cbBook.DataSource = booksList;
            cbBook.ValueMember = "Key";
            cbBook.DisplayMember = "Value";
            // Set properties for the autocomplete feature
            cbBook.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cbBook.AutoCompleteSource = AutoCompleteSource.ListItems;
            cbBook.SelectedIndex = -1;
        }
        private void PopulateFilterCustomerComboBox()
        {
            try
            {
                // Fetch customers and sort them by name
                var customersList = customerController.GetAllCustomers()
                                                      .OrderBy(c => c.Name)
                                                      .Select(c => new KeyValuePair<int, string>(c.CustomerID, c.Name))
                                                      .ToList();
                if (customersList.Count > 0)
                {
                    cbFilterCustomer.DataSource = customersList;
                    cbFilterCustomer.ValueMember = "Key";
                    cbFilterCustomer.DisplayMember = "Value";
                    // Setting up autocomplete
                    cbFilterCustomer.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                    cbFilterCustomer.AutoCompleteSource = AutoCompleteSource.ListItems;
                    cbFilterCustomer.AutoCompleteCustomSource.AddRange(customersList.Select(c => c.Value).ToArray());
                    cbFilterCustomer.SelectedIndex = -1;
                }
                else
                {
                    MessageBox.Show("No customers found.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading customer data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void PopulateFilterOrderComboBox()
        {
            var orderController = new OrderController();
            IEnumerable<Orders> orders;

            // Check if a customer is selected
            if (cbFilterCustomer.SelectedValue != null && int.TryParse(cbFilterCustomer.SelectedValue.ToString(), out int customerId))
            {
                // Fetch orders only for the selected customer
                orders = orderController.GetAllOrders().Where(o => o.CustomerID == customerId);
            }
            else
            {
                // Fetch all orders if no customer is selected
                orders = orderController.GetAllOrders();
            }

            var orderList = orders.Select(o => new {
                o.OrderID,
                DisplayValue = o.OrderID
            }).ToList();

            if (orderList.Any())
            {
                cbFilterOrder.DataSource = orderList;
                cbFilterOrder.DisplayMember = "DisplayValue";
                cbFilterOrder.ValueMember = "OrderID";
                cbFilterOrder.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                cbFilterOrder.AutoCompleteSource = AutoCompleteSource.ListItems;
                cbFilterOrder.SelectedIndex = -1;
            }
            else
            {
                cbFilterOrder.DataSource = null;
                MessageBox.Show("No orders found.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void PopulateOrdersDataGrid()
        {
            int? selectedCustomerId = cbFilterCustomer.SelectedValue as int?;
            int? selectedOrderId = cbFilterOrder.SelectedValue as int?;
            // Initialize DataTable and define columns
            DataTable table = new DataTable();
            table.Columns.Add("OrderID", typeof(int));
            table.Columns.Add("CustomerName", typeof(string));
            table.Columns.Add("OrderDate", typeof(DateTime));
            table.Columns.Add("TotalAmount", typeof(decimal));
            table.Columns.Add("Status", typeof(string));
            // Fetch and filter order data based on selected criteria
            var ordersList = new OrderController().GetAllOrders()
                .Where(o => (!selectedCustomerId.HasValue || o.CustomerID == selectedCustomerId) &&
                            (!selectedOrderId.HasValue || o.OrderID == selectedOrderId))
                .Select(o => new
                {
                    o.OrderID,
                    CustomerName = o.Customers?.Name ?? "Unknown",
                    OrderDate = o.OrderDate ?? DateTime.MinValue,
                    TotalAmount = o.TotalAmount ?? 0m,
                    Status = o.Status?.State ?? "Unknown"
                })
                .ToList();
            // Populate the DataTable with the filtered order data
            foreach (var order in ordersList)
            {
                table.Rows.Add(order.OrderID, order.CustomerName, order.OrderDate, order.TotalAmount, order.Status);
            }
            // Assign the DataTable as the DataSource for the DataGridView
            dataGridOrders.DataSource = table;
            // Set properties to enable basic functionalities
            dataGridOrders.AutoResizeColumns();
            dataGridOrders.ReadOnly = true;
            dataGridOrders.AllowUserToAddRows = false;
            dataGridOrders.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            // Clear any default selection
            dataGridOrders.ClearSelection();
            SetDataGridView();
        }
        private void SetListView()
        {
            // Add columns to the ListView
            listBooks.Columns.Add("Quantity", -2, HorizontalAlignment.Left);
            listBooks.Columns.Add("Price", -2, HorizontalAlignment.Left);
            listBooks.Columns.Add("ISBN", -2, HorizontalAlignment.Left);
            listBooks.Columns.Add("Title", -2, HorizontalAlignment.Left);
            // Adjust the width of the columns
            listBooks.Columns[0].Width = 80;
            listBooks.Columns[1].Width = 80; 
            listBooks.Columns[2].Width = 160;  
            listBooks.Columns[3].Width = 534;
        }
        private void SetDataGridView()
        {
            // Ensure the columns are created
            dataGridOrders.AutoGenerateColumns = true;
            // Set custom header text for each column
            dataGridOrders.Columns["OrderID"].HeaderText = "Order";
            dataGridOrders.Columns["CustomerName"].HeaderText = "Customer";
            dataGridOrders.Columns["OrderDate"].HeaderText = "Order Date and Time";
            dataGridOrders.Columns["TotalAmount"].HeaderText = "Total Amount";
            dataGridOrders.Columns["Status"].HeaderText = "Status";
            //Set the width of each column
            dataGridOrders.Columns["OrderID"].Width = 90;
            dataGridOrders.Columns["CustomerName"].Width = 350;
            dataGridOrders.Columns["OrderDate"].Width = 215;
            dataGridOrders.Columns["TotalAmount"].Width = 152;
            dataGridOrders.Columns["Status"].Width = 130;
            // Customize appearance
            dataGridOrders.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft Sans Serif", 12,FontStyle.Bold);
            dataGridOrders.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 12);
            // Set read-only mode
            dataGridOrders.ReadOnly = true;
        }
        private void UpdateTotals()
        {
            int totalItems = 0;
            decimal totalAmount = 0;

            foreach (ListViewItem item in listBooks.Items)
            {
                int quantity = int.Parse(item.SubItems[0].Text);
                decimal price = decimal.Parse(item.SubItems[1].Text);

                totalItems += quantity;
                totalAmount += quantity * price;
            }

            txtTotalItems.Text = totalItems.ToString();
            txtTotalAmount.Text = totalAmount.ToString("F2");
        }
        private void UpdateBookQuantity(ListViewItem item)
        {
            string input = Microsoft.VisualBasic.Interaction.InputBox("Enter new quantity:", "Update Quantity", item.SubItems[0].Text);
            if (int.TryParse(input, out int newQuantity) && newQuantity > 0)
            {
                item.SubItems[0].Text = newQuantity.ToString();
                UpdateTotals();
                MessageBox.Show("Quantity updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Invalid quantity entered. Please try again.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void RetrieveAndDisplayOrderInfo(int orderId)
        {
            OrderController orderController = new OrderController();
            var order = orderController.GetOrderById(orderId);
            if (order != null)
            {
                txtOrderID.Text = order.OrderID.ToString();
                cbCustomer.SelectedValue = order.CustomerID;
                txtTotalItems.Text = order.OrderDetails.Sum(d => d.Quantity).ToString();
                txtTotalAmount.Text = order.TotalAmount?.ToString("F2");
                if (order.StatusID.HasValue)
                {
                    StatusController statusController = new StatusController();
                    var status = statusController.GetStatusById(order.StatusID.Value);
                    txtStatus.Text = status != null ? status.State : "Unknown";
                    btnCancelOrder.Enabled = true;
                    btnUpdateOrder.Visible = true;
                    btnUpdateOrder.Enabled = true;
                    btnPlaceOrder.Enabled = false;
                }
                else
                {
                    txtStatus.Text = "Unknown";
                }
                // Clear and fill the books list
                listBooks.Items.Clear();
                foreach (var detail in order.OrderDetails)
                {
                    var book = bookController.GetBookByISBN(detail.BookID);
                    if (book != null)
                    {
                        var listViewItem = new ListViewItem(new[]
                        {
                            detail.Quantity.ToString(),
                            detail.Price?.ToString("F2"),
                            book.ISBN,
                            book.Title
                        });
                        listBooks.Items.Add(listViewItem);
                    }
                }
                // Update UI based on the order details
                tabPageAddNew.Text = "Update Order";
            }
            else
            {
                MessageBox.Show("Failed to retrieve order information.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Tab Add Order Events
        private void btnAddBoook_Click(object sender, EventArgs e)
        {
            if (cbBook.SelectedValue != null && !string.IsNullOrWhiteSpace(txtQtty.Text) && int.TryParse(txtQtty.Text, out int quantity))
            {
                string isbn = cbBook.SelectedValue.ToString();
                var book = bookController.GetBookByISBN(isbn);

                if (book != null)
                {
                    // Check if the book is already added by looking at the third subitem (ISBN)
                    var existingItem = listBooks.Items.Cast<ListViewItem>().FirstOrDefault(item => item.SubItems[2].Text == book.ISBN);
                    if (existingItem != null)
                    {
                        var result = MessageBox.Show("This book is already added. Do you want to update the quantity?", "Book Exists", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (result == DialogResult.Yes)
                        {
                            UpdateBookQuantity(existingItem);
                        }
                    }
                    else
                    {
                        // Create a new ListViewItem and set its properties
                        ListViewItem item = new ListViewItem(quantity.ToString());
                        item.SubItems.Add(book.Price?.ToString("F2") ?? "N/A");
                        item.SubItems.Add(book.ISBN);
                        item.SubItems.Add(book.Title);
                        listBooks.Items.Add(item);
                        UpdateTotals();
                    }
                    // Reset the ComboBox and TextBox
                    cbBook.SelectedIndex = -1;
                    txtQtty.Clear();
                }
            }
            else
            {
                MessageBox.Show("Please select a book and enter a valid quantity.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void btnRemoveBook_Click(object sender, EventArgs e)
        {
            // Check if any item is selected
            if (listBooks.SelectedItems.Count > 0)
            {
                // Confirmation before removal
                DialogResult confirmResult = MessageBox.Show("Are you sure you want to remove the selected book?", "Remove Book", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (confirmResult == DialogResult.Yes)
                {
                    // Remove the selected item from the ListView
                    foreach (ListViewItem item in listBooks.SelectedItems)
                    {
                        listBooks.Items.Remove(item);
                    }
                    UpdateTotals();
                }
            }
            else
            {
                MessageBox.Show("Please select a book to remove.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void listBooks_ItemActivate(object sender, EventArgs e)
        {
            if (listBooks.SelectedItems.Count > 0)
            {
                var item = listBooks.SelectedItems[0];
                UpdateBookQuantity(item);
            }
        }
        private void btnPlaceOrder_Click(object sender, EventArgs e)
        {
            if (cbCustomer.SelectedValue == null || listBooks.Items.Count == 0)
            {
                MessageBox.Show("Please select a customer and add at least one book to the order.", "Incomplete Data", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                // Create a new order object
                Orders newOrder = new Orders
                {
                    CustomerID = (int?)cbCustomer.SelectedValue,
                    OrderDate = DateTime.Now,
                    DateCreated = DateTime.Now,
                    StatusID = 1,
                    TotalAmount = decimal.Parse(txtTotalAmount.Text)
                };
                // Add each book as an OrderDetail
                foreach (ListViewItem item in listBooks.Items)
                {
                    var orderDetail = new OrderDetails
                    {
                        BookID = item.SubItems[2].Text, // ISBN
                        Quantity = int.Parse(item.SubItems[0].Text),
                        Price = decimal.Parse(item.SubItems[1].Text)
                    };
                    newOrder.OrderDetails.Add(orderDetail);
                }
                // Use the controller to add the order to the database
                OrderController orderController = new OrderController();
                orderController.AddOrder(newOrder);
                MessageBox.Show("Order placed successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                // Retrieve and display order information
                RetrieveAndDisplayOrderInfo(newOrder.OrderID);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to place the order: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnUpdateOrder_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtOrderID.Text))
            {
                MessageBox.Show("No order selected to update.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int orderId = int.Parse(txtOrderID.Text);
            var order = new OrderController().GetOrderById(orderId);
            if (order != null)
            {
                // Update basic order details if necessary
                order.TotalAmount = listBooks.Items.Cast<ListViewItem>()
                                                  .Sum(item => decimal.Parse(item.SubItems[1].Text) * int.Parse(item.SubItems[0].Text));

                // Clear existing details to replace with current ListView items
                order.OrderDetails.Clear();

                foreach (ListViewItem item in listBooks.Items)
                {
                    var detail = new OrderDetails
                    {
                        OrderID = order.OrderID,
                        BookID = item.SubItems[2].Text,  // ISBN
                        Quantity = int.Parse(item.SubItems[0].Text),
                        Price = decimal.Parse(item.SubItems[1].Text)
                    };
                    order.OrderDetails.Add(detail);
                }

                // Save changes
                new OrderController().UpdateOrder(order);
                MessageBox.Show("Order updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Order not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnCancelOrder_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtOrderID.Text))
            {
                MessageBox.Show("No order selected to cancel.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            int orderId = int.Parse(txtOrderID.Text);
            try
            {
                var orderController = new OrderController();
                orderController.CancelOrder(orderId);
                MessageBox.Show("Order has been cancelled successfully.", "Order Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Update the status text box to reflect the new status
                txtStatus.Text = "Cancelled";
                btnUpdateOrder.Enabled = false;
                btnCancelOrder.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to cancel the order: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Tab View Orders Events
        private void cbFilterCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateFilterOrderComboBox();
            PopulateOrdersDataGrid();
        }
        private void cbFilterOrder_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateOrdersDataGrid();
        }
        private void dataGridOrders_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dataGridOrders.ClearSelection();
        }
        private void dataGridOrders_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                int selectedOrderId = int.Parse(dataGridOrders.Rows[e.RowIndex].Cells["OrderID"].Value.ToString());
                DialogResult dialogResult = MessageBox.Show("Do you want to view details of the selected order?", "View Order", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    RetrieveAndDisplayOrderInfo(selectedOrderId);
                    tabCtrlOrder.SelectedTab = tabPageAddNew;
                }
            }
        }
        #endregion

        private void btnExit_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure you want to exit?", "Exit Application", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                Close();
            }
        }
        private void btnClearAll_Click(object sender, EventArgs e)
        {
            // Clear all input fields and lists
            cbCustomer.SelectedIndex = -1;
            listBooks.Items.Clear();
            txtTotalItems.Text = "0";
            txtTotalAmount.Text = "0.00";
            txtOrderID.Clear();
            txtStatus.Clear();
            cbFilterOrder.SelectedIndex = -1;
            cbFilterCustomer.SelectedIndex = -1;
            dataGridOrders.ClearSelection();

            // Reset button states
            btnPlaceOrder.Enabled = true;
            btnUpdateOrder.Enabled = false;
            btnUpdateOrder.Visible = false;
            tabPageAddNew.Text = "Place Order";
            btnCancelOrder.Enabled = false;
        }
    }
}
