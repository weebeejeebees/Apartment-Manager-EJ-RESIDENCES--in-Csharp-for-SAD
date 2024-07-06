using System;
using System.Windows.Forms;

namespace apartmentManager
{
    public partial class LoginDialog : Form
    {
        public bool IsAdmin { get; private set; }
        public string Password { get; private set; }

        private string adminPassword = "1234"; // Default admin password

        public LoginDialog()
        {
            InitializeComponent();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (radioButtonAdmin.Checked && textBoxPassword.Enabled && textBoxPassword.Text != adminPassword)
            {
                MessageBox.Show("Incorrect password. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBoxPassword.Clear();
                textBoxPassword.Focus();
                return;
            }

            IsAdmin = radioButtonAdmin.Checked;
            Password = textBoxPassword.Text;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void radioButtonAdmin_CheckedChanged(object sender, EventArgs e)
        {
            textBoxPassword.Enabled = radioButtonAdmin.Checked;
            if (radioButtonAdmin.Checked)
            {
                textBoxPassword.Focus();
            }
        }
    }
}
