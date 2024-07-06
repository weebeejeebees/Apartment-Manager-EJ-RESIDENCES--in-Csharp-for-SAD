using System;
using System.Windows.Forms;

namespace apartmentManager
{
    public partial class ChangePasswordForm : Form
    {
        public string NewPassword { get; private set; }
        private string currentPassword;
        private string adminPassword; // Add adminPassword here

        // Constructor to accept currentPassword and adminPassword
        public ChangePasswordForm(string currentPassword, string adminPassword)
        {
            InitializeComponent();
            this.currentPassword = currentPassword;
            this.adminPassword = adminPassword; // Initialize adminPassword
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (textBoxCurrentPassword.Text != currentPassword)
            {
                MessageBox.Show("Current password is incorrect.");
                return;
            }

            if (textBoxNewPassword.Text != textBoxConfirmPassword.Text)
            {
                MessageBox.Show("New passwords do not match.");
                return;
            }

            NewPassword = textBoxNewPassword.Text;
            adminPassword = NewPassword; // Update adminPassword with the new password
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
