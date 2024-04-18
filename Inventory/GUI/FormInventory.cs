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

namespace Inventory.GUI
{
    public partial class FormInventory : Form
    {
        private readonly BookController bookController;
        private readonly AuthorController authorController;
        // Initialize the selected author ID to -1 (add new)
        private int selectedAuthorId = -1;

        public FormInventory()
        {
            authorController = new AuthorController();
            bookController = new BookController();
            InitializeComponent();
            DesignTabButtons();
            PopulateCategoriesComboBox(cbCategory);
            PopulatePublishersComboBox(cbPublisher);
            PopulateAuthorsComboBox();
            PopulateIsbnComboBox();
            PopulateCategoriesComboBox(cbFilterCategory);
            PopulatePublishersComboBox(cbFilterPublisher);
            PopulateTitleComboBox();
            PopulateFilterAuthorComboBox();
            PopulateBooksDataGrid();
        }

        #region Form Utlities
        public void DesignTabButtons()
        {
            panelMain.Padding = new Padding(7); // Adjust the padding of the panel in which the tab control is docked

            // Set the TabControl mode and handle the DrawItem event
            tabCtrlInventory.SizeMode = TabSizeMode.Fixed;
            tabCtrlInventory.DrawMode = TabDrawMode.OwnerDrawFixed;
            tabCtrlInventory.ItemSize = new Size(220, 30); // Set the desired width and height

            tabCtrlInventory.DrawItem += (sender, e) =>
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
                e.Graphics.DrawString(tabCtrlInventory.TabPages[e.Index].Text, e.Font, textBrush, e.Bounds, sf);
            };
        }
        private void PopulateCategoriesComboBox(ComboBox comboBox)
        {
            var categoryController = new CategoryController();
            var categories = categoryController.GetAllCategories()
                                               .OrderBy(c => c.CategoryName)
                                               .Select(c => new { c.CategoryID, c.CategoryName })
                                               .ToList();
            // Set up the ComboBox
            comboBox.DataSource = new BindingSource(categories, null);
            comboBox.ValueMember = "CategoryID";
            comboBox.DisplayMember = "CategoryName";
            comboBox.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            comboBox.AutoCompleteSource = AutoCompleteSource.ListItems;
            // Set the ComboBox to have no item selected by default
            comboBox.SelectedIndex = -1;
        }
        private void PopulatePublishersComboBox(ComboBox comboBox)
        {
            var publisherController = new PublisherController();
            var publishers = publisherController.GetAllPublishers()
                                                .OrderBy(p => p.Name)
                                                .Select(p => new { p.PublisherID, p.Name })
                                                .ToList();
            // Set up the ComboBox
            comboBox.DataSource = publishers;
            comboBox.ValueMember = "PublisherID";
            comboBox.DisplayMember = "Name";
            comboBox.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            comboBox.AutoCompleteSource = AutoCompleteSource.ListItems;
            // Set the ComboBox to have no item selected by default
            comboBox.SelectedIndex = -1;
        }
        private void PopulateBooksDataGrid()
        {
            // Retrieve selected filter values
            int? selectedPublisherId = cbFilterPublisher.SelectedValue as int?;
            int? selectedCategoryId = cbFilterCategory.SelectedValue as int?;
            string selectedIsbn = cbFilterISBN.SelectedItem as string;
            string selectedTitle = cbFilterTitle.SelectedItem as string;
            int? selectedAuthorId = cbFilterAuthor.SelectedValue as int?;

            // Initialize DataTable and define columns
            DataTable table = new DataTable();
            table.Columns.Add("ISBN", typeof(string));
            table.Columns.Add("Title", typeof(string));
            table.Columns.Add("Price", typeof(decimal));
            table.Columns.Add("Quantity", typeof(int));
            table.Columns.Add("Category", typeof(string));
            table.Columns.Add("Publisher", typeof(string));
            table.Columns.Add("Year", typeof(int));
            table.Columns.Add("Authors", typeof(string));
            table.Columns.Add("Status", typeof(string));

            // Fetch and filter book data based on selected criteria
            var booksList = bookController.GetAllBooks()
                .Where(b => (!selectedPublisherId.HasValue || b.PublisherID == selectedPublisherId) &&
                            (!selectedCategoryId.HasValue || b.CategoryID == selectedCategoryId) &&
                            (string.IsNullOrEmpty(selectedIsbn) || b.ISBN == selectedIsbn) &&
                            (string.IsNullOrEmpty(selectedTitle) || b.Title.Contains(selectedTitle)) &&
                            (!selectedAuthorId.HasValue || b.Authors.Any(a => a.AuthorID == selectedAuthorId)))
                .Select(book => new
                {
                    ISBN = book.ISBN,
                    Title = book.Title,
                    Price = book.Price,
                    QOH = book.QOH,
                    CategoryName = book.Categories != null ? book.Categories.CategoryName : "Unknown Category",
                    PublisherName = book.Publishers != null ? book.Publishers.Name : "Unknown Publisher",
                    Year = book.YearPublished,
                    Authors = book.Authors != null ? String.Join(", ", book.Authors.Select(a => $"{a.FirstName} {a.LastName}")) : "No Authors",
                    Status = book.Status != null ? book.Status.State : "Unknown Status"
                })
                .ToList();

            // Populate the DataTable with the filtered book data
            foreach (var book in booksList)
            {
                table.Rows.Add(book.ISBN, book.Title, book.Price, book.QOH, book.CategoryName,
                               book.PublisherName, book.Year, book.Authors, book.Status);
            }

            // Assign the DataTable as the DataSource for the DataGridView
            dataGridBooks.DataSource = table;
            // Set properties to enable basic functionalities
            dataGridBooks.AutoResizeColumns();
            dataGridBooks.ReadOnly = true;
            dataGridBooks.AllowUserToAddRows = false;
            dataGridBooks.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            // Customize appearance
            dataGridBooks.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft Sans Serif", 10);
            dataGridBooks.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 8);
            // Clear any default selection
            dataGridBooks.ClearSelection();
        }
        private void PopulateAuthorsComboBox()
        {
            // Fetch authors and sort them by their display name
            var authorsList = authorController.GetAllAuthors()
                                        .OrderBy(a => a.FirstName)
                                        .ThenBy(a => a.LastName)
                                        .Select(a => new KeyValuePair<int, string>(a.AuthorID, $"{a.FirstName} {a.LastName}"))
                                        .ToList();
            // Add a special 'Add new...' entry at the beginning of the list
            authorsList.Insert(0, new KeyValuePair<int, string>(0, "Add new..."));
            // Set up the ComboBox
            cbAuthor.DataSource = authorsList;
            cbAuthor.ValueMember = "Key";
            cbAuthor.DisplayMember = "Value";
            // Set properties for the autocomplete feature
            cbAuthor.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cbAuthor.AutoCompleteSource = AutoCompleteSource.ListItems;
        }
        private void PopulateIsbnComboBox()
        {
            // Fetch a list of ISBNs from the books
            var isbnList = bookController.GetAllBooks()
                                         .Select(b => b.ISBN)
                                         .OrderBy(isbn => isbn)
                                         .ToList();
            // Set up the ComboBox
            cbFilterISBN.DataSource = isbnList;
            cbFilterISBN.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cbFilterISBN.AutoCompleteSource = AutoCompleteSource.ListItems;
            cbFilterISBN.SelectedIndex = -1;
        }
        private void PopulateTitleComboBox()
        {
            // Fetch a list of titles from the books
            var titleList = bookController.GetAllBooks()
                                          .Select(b => b.Title)
                                          .OrderBy(title => title)
                                          .ToList();
            // Set up the ComboBox
            cbFilterTitle.DataSource = titleList;
            cbFilterTitle.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cbFilterTitle.AutoCompleteSource = AutoCompleteSource.ListItems;
            cbFilterTitle.SelectedIndex = -1;
        }
        private void PopulateFilterAuthorComboBox()
        {
            // Fetch authors and sort them by their display name
            var authorsList = authorController.GetAllAuthors()
                                        .OrderBy(a => a.FirstName)
                                        .ThenBy(a => a.LastName)
                                        .Select(a => new KeyValuePair<int, string>(a.AuthorID, $"{a.FirstName} {a.LastName}"))
                                        .ToList();
            // Set up the ComboBox
            cbFilterAuthor.DataSource = authorsList;
            cbFilterAuthor.ValueMember = "Key";
            cbFilterAuthor.DisplayMember = "Value";
            // Set properties for the autocomplete feature
            cbFilterAuthor.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cbFilterAuthor.AutoCompleteSource = AutoCompleteSource.ListItems;
            cbFilterAuthor.SelectedIndex = -1;
        }
        private bool ValidateInputs()
        {
            bool isValid = !string.IsNullOrWhiteSpace(txtISBN.Text) &&
                           !string.IsNullOrWhiteSpace(txtTitle.Text) &&
                           cbCategory.SelectedIndex >= 0 &&
                           cbPublisher.SelectedIndex >= 0 &&
                           !string.IsNullOrWhiteSpace(txtPrice.Text) &&
                           !string.IsNullOrWhiteSpace(txtQuantity.Text) &&
                           listAuthors.Items.Count > 0;

            btnAddBook.Enabled = isValid;
            return isValid;
        }
        private void EnableFormControls(object sender, EventArgs e)
        {
            ValidateInputs();
        }
        private void ResetUpdateForm()
        {
            txtISBN.Clear();
            txtTitle.Clear();
            txtPrice.Clear();
            txtYear.Clear();
            txtQuantity.Clear();
            cbCategory.SelectedIndex = -1;
            cbPublisher.SelectedIndex = -1;
            listAuthors.Items.Clear();
            tabPageAddNew.Text = "Add New Book";
            btnAddBook.Text = "Add Book";
            btnDelete.Enabled = false;
        }
        private void ResetFilters()
        {
            cbFilterISBN.SelectedIndex = -1;
            cbFilterTitle.SelectedIndex = -1;
            cbFilterAuthor.SelectedIndex = -1;
            cbFilterCategory.SelectedIndex = -1;
            cbFilterPublisher.SelectedIndex = -1;
            dataGridBooks.ClearSelection();

        }
        private void LoadBookForEditing(string isbn)
        {
            Books book = bookController.GetBookByISBN(isbn);
            if (book != null)
            {
                txtISBN.Text = book.ISBN;
                txtTitle.Text = book.Title;
                txtPrice.Text = book.Price.ToString();
                txtYear.Text = book.YearPublished.ToString();
                txtQuantity.Text = book.QOH.ToString();
                cbCategory.SelectedValue = book.CategoryID;
                cbPublisher.SelectedValue = book.PublisherID;
                btnDelete.Enabled = true;
                tabPageAddNew.Text = "Update Book";
                btnAddBook.Text = "Update Book";

                listAuthors.Items.Clear();
                foreach (var author in book.Authors)
                {
                    listAuthors.Items.Add(new AuthorItem(author.AuthorID, $"{author.FirstName} {author.LastName}"));
                }

                // Enable clear button
                btnClearAll.Enabled = true;
            }
        }

        #endregion

        #region Tab Add Book Actions
        private void cbPublisher_Leave(object sender, EventArgs e)
        {
            string input = cbPublisher.Text.Trim();

            // Check if the input is empty
            if (string.IsNullOrEmpty(input))
            {
                cbPublisher.SelectedIndex = -1;  // Reset the ComboBox if no valid input
                return;
            }
            // Create a controller instance to check existence.
            var publisherController = new PublisherController();
            // Check if the entered name exists in the list of publishers
            bool exists = publisherController.GetAllPublishers().Any(p => p.Name.Equals(input, StringComparison.OrdinalIgnoreCase));
            if (!exists)
            {
                // Confirm with the user that they want to add a new publisher.
                string confirmedInput = (Interaction.InputBox("Confirm or edit the publisher name:", "Add New Publisher", input)).Trim();

                if (!string.IsNullOrEmpty(confirmedInput))
                {
                    // Check again if the publisher exists, in case the name was modified.
                    if (!publisherController.Exists(confirmedInput))
                    {
                        // Create the new publisher instance
                        Publishers newPublisher = new Publishers
                        {
                            Name = confirmedInput
                        };
                        // Add the new publisher to the database
                        int newPublisherId = publisherController.AddPublisher(newPublisher);
                        PopulatePublishersComboBox(cbPublisher);  // Refresh the list
                        cbPublisher.SelectedValue = newPublisherId;  // Select the newly added publisher
                        MessageBox.Show("New publisher added successfully.", "Add New Publisher");
                    }
                    else
                    {
                        MessageBox.Show("Publisher already exists.");
                        cbPublisher.SelectedIndex = -1;  // Reset the ComboBox if the publisher exists
                    }
                }
                else
                {
                    MessageBox.Show("Adding new publisher cancelled.","Add New Publiser", MessageBoxButtons.OK);
                    cbPublisher.SelectedIndex = -1;  // Reset the ComboBox if cancelled
                }
            }
        }
        private void cbFilterISBN_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateBooksDataGrid();
        }
        private void cbFilterTitle_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateBooksDataGrid();
        }
        private void cbFilterAuthor_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateBooksDataGrid();
        }
        private void cbAuthor_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Store the currently selected author ID
            if (cbAuthor.SelectedValue is int authorId)
            {
                selectedAuthorId = authorId;
            }
        }
        private void btnAddAuthor_Click(object sender, EventArgs e)
        {
            if (selectedAuthorId > 0)  // Check if an existing author is selected
            {
                // Retrieve the author's details
                var author = authorController.GetAuthorById(selectedAuthorId);
                var authorItem = new AuthorItem(author.AuthorID, $"{author.FirstName} {author.LastName}");

                // Add to ListBox if not already added
                if (!listAuthors.Items.Cast<AuthorItem>().Any(ai => ai.AuthorID == authorItem.AuthorID))
                {
                    listAuthors.Items.Add(authorItem);
                    cbAuthor.SelectedIndex = -1; // Reset ComboBox
                }
            }
            else if (selectedAuthorId == 0)
            {
                // 'Add new...' is selected, open the FormAuthor
                using (var addAuthorForm = new FormAuthor())
                {
                    if (addAuthorForm.ShowDialog() == DialogResult.OK)
                    {
                        // Refresh the ComboBox to include the newly added author
                        PopulateAuthorsComboBox();
                        // Select the newly added author in the ComboBox
                        cbAuthor.SelectedValue = addAuthorForm.CreatedAuthorId;
                    }
                }
            }
        }
        private void btnRemoveAuthor_Click(object sender, EventArgs e)
        {
            var selectedItem = listAuthors.SelectedItem;
            if (selectedItem != null)
            {
                listAuthors.Items.Remove(selectedItem);
                listAuthors.SelectedIndex = -1; 
            }
            else
            {
                MessageBox.Show("Please select an author to remove.", "No Author Selected", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs())
            {
                MessageBox.Show("Please ensure all fields are filled correctly.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // Create a new book object
            Books newBook = new Books
            {
                ISBN = txtISBN.Text.Trim(),
                Title = txtTitle.Text.Trim(),
                Price = decimal.Parse(txtPrice.Text.Trim()), // Add error handling for parsing
                YearPublished = int.Parse(txtYear.Text.Trim()), // Add error handling for parsing
                QOH = int.Parse(txtQuantity.Text.Trim()), // Add error handling for parsing
                CategoryID = (int)cbCategory.SelectedValue,
                PublisherID = (int)cbPublisher.SelectedValue,
                StatusID = 1 // Default status is 'Active'
            };
            // Collect author IDs from the list box
            List<int> authorIds = listAuthors.Items.Cast<AuthorItem>().Select(ai => ai.AuthorID).ToList();
            if (bookController.Exists(newBook.ISBN))
            {
                // Update existing book
                bookController.UpdateBook(newBook, authorIds);
                MessageBox.Show("Book updated successfully!", "Update Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ResetUpdateForm();
            }
            else
            {
                // Add new book
                bookController.AddBook(newBook, authorIds);
                MessageBox.Show("Book added successfully!", "Addition Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            PopulateBooksDataGrid(); // Refresh the grid to show new/updated book
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            // Confirm deletion with the user to avoid accidental data loss
            if (MessageBox.Show("Are you sure you want to delete this book?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                try
                {
                    string isbnToDelete = txtISBN.Text.Trim();
                    if (!string.IsNullOrEmpty(isbnToDelete))
                    {
                        bookController.DeleteBook(isbnToDelete);
                        MessageBox.Show("Book deleted successfully.", "Deletion Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ResetUpdateForm();
                        PopulateBooksDataGrid();
                    }
                    else
                    {
                        MessageBox.Show("No book selected or ISBN missing.", "Deletion Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    // Log the exception or handle it according to your error handling policy
                    MessageBox.Show($"An error occurred while deleting the book: {ex.Message}", "Deletion Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        #endregion

        #region Tab View Books Actions
        private void tabCtrlInventory_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Check if the current tab is the 'View All Books' tab
            if (tabCtrlInventory.SelectedTab == tabPageViewAll)
            {
                PopulateBooksDataGrid();
            }
        }        
        private void cbFilterCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateBooksDataGrid();
        }
        private void cbFilterPublisher_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateBooksDataGrid();
        }
        private void dataGridBooks_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                string selectedISBN = dataGridBooks.Rows[e.RowIndex].Cells["ISBN"].Value.ToString();
                DialogResult dialogResult = MessageBox.Show("Do you want to update the selected book?", "Update Book", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    LoadBookForEditing(selectedISBN);
                    tabCtrlInventory.SelectedTab = tabPageAddNew;
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
            ResetUpdateForm();
            ResetFilters();
        }
    }
}
