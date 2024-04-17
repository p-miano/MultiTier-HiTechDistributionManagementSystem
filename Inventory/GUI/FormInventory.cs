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
        private readonly AuthorController authorController;
        // Initialize the selected author ID to -1 (add new)
        private int selectedAuthorId = -1; 
        public FormInventory()
        {
            authorController = new AuthorController();
            InitializeComponent();
            DesignTabButtons();
            PopulateCategoriesComboBox();
            PopulatePublishersComboBox();
            PopulateAuthorsComboBox();
        }

        private void FormInventory_Load(object sender, EventArgs e)
        {
            // TODO: Add the code to be executed when the form is loaded.
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

        private void PopulateCategoriesComboBox()
        {
            var categoryController = new CategoryController();
            var categories = categoryController.GetAllCategories()
                                               .OrderBy(c => c.CategoryName)
                                               .Select(c => new { c.CategoryID, c.CategoryName })
                                               .ToList();
            // Set up the ComboBox
            cbCategory.DataSource = categories;
            cbCategory.ValueMember = "CategoryID";
            cbCategory.DisplayMember = "CategoryName";
            // Enable autocomplete
            cbCategory.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cbCategory.AutoCompleteSource = AutoCompleteSource.ListItems;
            // Set the ComboBox to have no item selected by default
            cbCategory.SelectedIndex = -1;
        }
        private void PopulatePublishersComboBox()
        {
            var publisherController = new PublisherController();
            var publishers = publisherController.GetAllPublishers()
                                                .OrderBy(p => p.Name)
                                                .Select(p => new { p.PublisherID, p.Name })
                                                .ToList();
            // Set up the ComboBox
            cbPublisher.DataSource = publishers;
            cbPublisher.ValueMember = "PublisherID";
            cbPublisher.DisplayMember = "Name";
            // Enable autocomplete
            cbPublisher.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cbPublisher.AutoCompleteSource = AutoCompleteSource.ListItems;
            // Set the ComboBox to have no item selected by default
            cbPublisher.SelectedIndex = -1;
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
        #endregion
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
                        PopulatePublishersComboBox();  // Refresh the list
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
            if (selectedAuthorId == 0)
            {
                // Check if 'Add new...' is selected based on the special key value of 0
                if (selectedAuthorId == 0)
                {
                    // 'Add new...' is selected, so open the FormAuthor
                    using (var addAuthorForm = new FormAuthor())
                    {
                        var result = addAuthorForm.ShowDialog();
                        if (result == DialogResult.OK)
                        {
                            // Refresh the ComboBox to include the newly added author
                            PopulateAuthorsComboBox();
                            // Select the newly added author in the ComboBox
                            cbAuthor.SelectedValue = addAuthorForm.CreatedAuthorId;
                        }
                    }
                }
                else
                {
                    // TODO: Perform action for when an existing author is selected
                }
            }
        }


    }
}
