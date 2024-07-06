using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Sql;
using System.Data.SqlClient;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace apartmentManager
{
    // Data Source=LAPTOP-EK7LC1R2;Initial Catalog=apartmentmanagement;Integrated Security=True
    public partial class Form3 : Form
    {
        koneksyon kon = new koneksyon();
        SqlConnection conn;
        SqlCommand cmd;
        SqlDataReader rd;

        public Form3()
        {
            InitializeComponent();
            SetColumnHeaderStyles();

        }

        private void apartmentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Create an instance of Form2
            Form1 form1 = new Form1();

            // Show Form2
            form1.Show();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            button2_Click(this, EventArgs.Empty);
        }

        // Method to set column header styles
        private void SetColumnHeaderStyles()
        {
            DataGridViewCellStyle columnHeaderStyle = new DataGridViewCellStyle();
            columnHeaderStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            columnHeaderStyle.BackColor = SystemColors.Control;
            columnHeaderStyle.Font = new Font("Bahnschrift", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            columnHeaderStyle.ForeColor = SystemColors.WindowText;
            columnHeaderStyle.SelectionBackColor = SystemColors.Highlight;
            columnHeaderStyle.SelectionForeColor = SystemColors.HighlightText;
            columnHeaderStyle.WrapMode = DataGridViewTriState.True;

            dataGridView1.ColumnHeadersDefaultCellStyle = columnHeaderStyle;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            conn = kon.getCon();
            conn.Open();
            try
            {
                cmd = new SqlCommand("INSERT INTO lease (apartmentId, leaseStartDate, leaseEndDate, monthlyRentAmount, rentDueDate, securityDepositAmount, remainingBalance) VALUES (@apartmentId, @leaseStartDate, @leaseEndDate, @monthlyRentAmount, @rentDueDate, @securityDepositAmount, @remainingBalance); SELECT SCOPE_IDENTITY();", conn);
                cmd.Parameters.AddWithValue("@apartmentId", textBox1.Text);
                cmd.Parameters.AddWithValue("@leaseStartDate", dateTimePicker1.Value);
                cmd.Parameters.AddWithValue("@leaseEndDate", dateTimePicker2.Value);
                cmd.Parameters.AddWithValue("@monthlyRentAmount", textBox4.Text);
                cmd.Parameters.AddWithValue("@rentDueDate", dateTimePicker3.Value);
                cmd.Parameters.AddWithValue("@securityDepositAmount", textBox7.Text);
                cmd.Parameters.AddWithValue("@remainingBalance", textBox8.Text);
                int transactionId = Convert.ToInt32(cmd.ExecuteScalar());
                MessageBox.Show("New lease added with ID: " + transactionId.ToString());
            }
            catch (SqlException ex)
            {
                MessageBox.Show("SQL Error: " + ex.Message);
            }
            finally
            {
                textBox1.Text = "";
                textBox4.Text = "";
                textBox7.Text = "";
                textBox8.Text = "";
                conn.Close();
                button2_Click(this, EventArgs.Empty);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            conn = kon.getCon();
            conn.Open();
            dataGridView1.Rows.Clear();
            try
            {
                cmd = new SqlCommand("SELECT * FROM lease", conn);
                rd = cmd.ExecuteReader();
                while (rd.Read())
                {
                    dataGridView1.Rows.Add(rd["transactionId"], rd["apartmentId"], rd["leaseStartDate"], rd["leaseEndDate"], rd["monthlyRentAmount"], rd["rentDueDate"], rd["securityDepositAmount"], rd["remainingBalance"]);
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("SQL Error: " + ex.Message);
            }
            finally
            {
                rd.Close();
                conn.Close();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            conn = kon.getCon();
            conn.Open();

            if (string.IsNullOrWhiteSpace(textBox1.Text) ||
                string.IsNullOrWhiteSpace(textBox4.Text) ||
                string.IsNullOrWhiteSpace(textBox7.Text) ||
                string.IsNullOrWhiteSpace(textBox8.Text) ||
                string.IsNullOrWhiteSpace(textBox5.Text))
            {
                MessageBox.Show("Please fill all fields before updating.");
                conn.Close();
                return;
            }

            try
            {
                SqlCommand checkCmd = new SqlCommand("SELECT COUNT(*) FROM lease WHERE transactionId = @transactionId", conn);
                checkCmd.Parameters.AddWithValue("@transactionId", textBox5.Text);
                int count = (int)checkCmd.ExecuteScalar();
                if (count == 0)
                {
                    MessageBox.Show("No lease found with the provided Transaction ID");
                    conn.Close();
                    return;
                }

                cmd = new SqlCommand("UPDATE lease SET apartmentId = @apartmentId, leaseStartDate = @leaseStartDate, leaseEndDate = @leaseEndDate, monthlyRentAmount = @monthlyRentAmount, rentDueDate = @rentDueDate, securityDepositAmount = @securityDepositAmount, remainingBalance = @remainingBalance WHERE transactionId = @transactionId", conn);
                cmd.Parameters.AddWithValue("@apartmentId", textBox1.Text);
                cmd.Parameters.AddWithValue("@leaseStartDate", dateTimePicker1.Value);
                cmd.Parameters.AddWithValue("@leaseEndDate", dateTimePicker2.Value);
                cmd.Parameters.AddWithValue("@monthlyRentAmount", textBox4.Text);
                cmd.Parameters.AddWithValue("@rentDueDate", dateTimePicker3.Value);
                cmd.Parameters.AddWithValue("@securityDepositAmount", textBox7.Text);
                cmd.Parameters.AddWithValue("@remainingBalance", textBox8.Text);
                cmd.Parameters.AddWithValue("@transactionId", textBox5.Text);

                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    MessageBox.Show("Lease updated successfully");
                }
                else
                {
                    MessageBox.Show("Failed to update lease");
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("SQL Error: " + ex.Message);
            }
            finally
            {
                conn.Close();
                button2_Click(this, EventArgs.Empty);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            conn = kon.getCon();
            conn.Open();
            try
            {
                cmd = new SqlCommand("DELETE FROM lease WHERE transactionId = @transactionId", conn);
                cmd.Parameters.AddWithValue("@transactionId", textBox5.Text);
                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    MessageBox.Show("Lease deleted successfully");
                }
                else
                {
                    MessageBox.Show("No lease found with the provided Transaction ID");
                }
                textBox1.Text = "";
                textBox4.Text = "";
                textBox7.Text = "";
                textBox8.Text = "";
                textBox5.Text = "";
            }
            catch (SqlException ex)
            {
                MessageBox.Show("SQL Error: " + ex.Message);
            }
            finally
            {
                conn.Close();
                button2_Click(this, EventArgs.Empty);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem != null && !string.IsNullOrEmpty(textBox5.Text))
            {
                string selectedColumn = comboBox1.SelectedItem.ToString();
                string searchText = textBox5.Text;

                // Clear existing data in DataGridView
                dataGridView1.Rows.Clear();

                // Execute SELECT query to retrieve data from lease table
                string query = "SELECT * FROM lease";
                DateTime dateValue = DateTime.MinValue; // Initialize dateValue with a default value
                if (selectedColumn == "transactionId" && int.TryParse(searchText, out int transactionId))
                {
                    query += $" WHERE transactionId = {transactionId}";
                }
                else if (selectedColumn == "leaseStartDate" || selectedColumn == "leaseEndDate" || selectedColumn == "rentDueDate")
                {
                    if (DateTime.TryParse(searchText, out dateValue))
                    {
                        query += $" WHERE {selectedColumn} = @searchDate";
                    }
                    else
                    {
                        MessageBox.Show("Invalid date format. Please enter a valid date.");
                        return;
                    }
                }
                else
                {
                    query += $" WHERE {selectedColumn} LIKE @searchText";
                }

                conn = kon.getCon();
                conn.Open();
                cmd = new SqlCommand(query, conn);
                if (selectedColumn == "leaseStartDate" || selectedColumn == "leaseEndDate" || selectedColumn == "rentDueDate")
                {
                    cmd.Parameters.AddWithValue("@searchDate", dateValue);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@searchText", "%" + searchText + "%");
                }
                rd = cmd.ExecuteReader();

                // Iterate through the results and populate the DataGridView
                while (rd.Read())
                {
                    dataGridView1.Rows.Add(rd["transactionId"], rd["apartmentId"], rd["leaseStartDate"], rd["leaseEndDate"], rd["monthlyRentAmount"], rd["rentDueDate"], rd["securityDepositAmount"], rd["remainingBalance"]);
                }

                rd.Close();
                conn.Close();
            }
            else
            {
                // Show a message if no filter is selected or no search text is provided
                MessageBox.Show("Please select a column to search and enter a search value.");
            }
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void tenantToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Create an instance of Form2
            Form2 form2 = new Form2();

            // Show Form2
            form2.Show();
        }

        private void transactionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Create an instance of Form2
            Form4 form4 = new Form4();

            // Show Form2
            form4.Show();
        }

        private void availableRoomsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Create an instance of Form2
            Form5 form5 = new Form5();

            // Show Form2
            form5.Show();
        }
    }
}
