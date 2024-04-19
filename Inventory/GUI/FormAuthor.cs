using Hi_TechLibrary.BLL.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InventoryManager.GUI
{
    public partial class FormAuthor : Form
    {
        private readonly AuthorController authorController;
        public int CreatedAuthorId { get; private set; }
        public FormAuthor()
        {
            authorController = new AuthorController();
            InitializeComponent();            
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            Authors newAuthor = new Authors
            {
                FirstName = txtFirstName.Text,
                LastName = txtLastName.Text,
                Email = txtEmail.Text
            };
            authorController.AddAuthor(newAuthor);
            CreatedAuthorId = newAuthor.AuthorID;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
