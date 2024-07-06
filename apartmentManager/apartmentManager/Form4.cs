using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace apartmentManager
{
    // Data Source=LAPTOP-EK7LC1R2;Initial Catalog=apartmentmanagement;Integrated Security=True
    public partial class Form4 : Form
    {
        koneksyon kon = new koneksyon();
        SqlConnection conn;
        SqlCommand cmd;
        SqlDataReader rd;

        public Form4()
        {
            InitializeComponent();
            SetColumnHeaderStyles();
        }

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

        private void apartmentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Create an instance of Form2
            Form1 form1 = new Form1();

            // Show Form2
            form1.Show();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            // Add your logic here if needed
        }

        private void label1_Click(object sender, EventArgs e)
        {
            // Add your logic here if needed
        }

        private void label2_Click(object sender, EventArgs e)
        {
            // Add your logic here if needed
        }

        private void label4_Click(object sender, EventArgs e)
        {
            // Add your logic here if needed
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            // Add your logic here if needed
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            button2_Click(this, EventArgs.Empty);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Add your logic here if needed
        }

        private void button1_Click(object sender, EventArgs e)
        {
            conn = kon.getCon();
            conn.Open();
            try
            {
                cmd = new SqlCommand("INSERT INTO payment (dateOfTransaction, amountPaid, tenantId, description, paymentStatus) VALUES (@dateOfTransaction, @amountPaid, @tenantId, @description, @paymentStatus); SELECT SCOPE_IDENTITY();", conn);
                cmd.Parameters.AddWithValue("@dateOfTransaction", dateTimePicker1.Value);
                cmd.Parameters.AddWithValue("@amountPaid", textBox2.Text);
                cmd.Parameters.AddWithValue("@tenantId", textBox4.Text);
                cmd.Parameters.AddWithValue("@description", textBox3.Text);
                cmd.Parameters.AddWithValue("@paymentStatus", comboBox2.Text);
                int paymentId = Convert.ToInt32(cmd.ExecuteScalar());
                MessageBox.Show("New payment added with ID: " + paymentId.ToString());
            }
            catch (SqlException ex)
            {
                MessageBox.Show("SQL Error: " + ex.Message);
            }
            finally
            {
                textBox2.Text = "";
                textBox4.Text = "";
                textBox3.Text = "";
                comboBox2.Text = "";
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
                cmd = new SqlCommand("SELECT * FROM payment", conn);
                rd = cmd.ExecuteReader();
                while (rd.Read())
                {
                    dataGridView1.Rows.Add(rd["paymentId"], rd["dateOfTransaction"], rd["amountPaid"], rd["tenantId"], rd["description"], rd["paymentStatus"]);
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

            if (string.IsNullOrWhiteSpace(textBox2.Text) ||
                string.IsNullOrWhiteSpace(textBox4.Text) ||
                string.IsNullOrWhiteSpace(textBox3.Text) ||
                string.IsNullOrWhiteSpace(comboBox2.Text) ||
                string.IsNullOrWhiteSpace(textBox5.Text))
            {
                MessageBox.Show("Please fill all fields before updating.");
                conn.Close();
                return;
            }

            try
            {
                SqlCommand checkCmd = new SqlCommand("SELECT COUNT(*) FROM payment WHERE paymentId = @paymentId", conn);
                checkCmd.Parameters.AddWithValue("@paymentId", textBox5.Text);
                int count = (int)checkCmd.ExecuteScalar();
                if (count == 0)
                {
                    MessageBox.Show("No payment found with the provided Payment ID");
                    conn.Close();
                    return;
                }

                cmd = new SqlCommand("UPDATE payment SET dateOfTransaction = @dateOfTransaction, amountPaid = @amountPaid, tenantId = @tenantId, description = @description, paymentStatus = @paymentStatus WHERE paymentId = @paymentId", conn);
                cmd.Parameters.AddWithValue("@dateOfTransaction", dateTimePicker1.Value);
                cmd.Parameters.AddWithValue("@amountPaid", textBox2.Text);
                cmd.Parameters.AddWithValue("@tenantId", textBox4.Text);
                cmd.Parameters.AddWithValue("@description", textBox3.Text);
                cmd.Parameters.AddWithValue("@paymentStatus", comboBox2.Text);
                cmd.Parameters.AddWithValue("@paymentId", textBox5.Text);

                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    MessageBox.Show("Payment updated successfully");
                }
                else
                {
                    MessageBox.Show("Failed to update payment");
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
                cmd = new SqlCommand("DELETE FROM payment WHERE paymentId = @paymentId", conn);
                cmd.Parameters.AddWithValue("@paymentId", textBox5.Text);
                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    MessageBox.Show("Payment deleted successfully");
                }
                else
                {
                    MessageBox.Show("No payment found with the provided Payment ID");
                }
                textBox2.Text = "";
                textBox4.Text = "";
                textBox3.Text = "";
                comboBox2.Text = "";
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

                // Execute SELECT query to retrieve data from payment table
                string query = "SELECT * FROM payment";
                DateTime dateValue = DateTime.MinValue; // Initialize dateValue with a default value
                if (selectedColumn == "paymentId" && int.TryParse(searchText, out int paymentId))
                {
                    query += $" WHERE paymentId = {paymentId}";
                }
                else if (selectedColumn == "dateOfTransaction")
                {
                    if (DateTime.TryParse(searchText, out dateValue))
                    {
                        query += $" WHERE dateOfTransaction = @searchDate";
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
                if (selectedColumn == "dateOfTransaction")
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
                    dataGridView1.Rows.Add(rd["paymentId"], rd["dateOfTransaction"], rd["amountPaid"], rd["tenantId"], rd["description"], rd["paymentStatus"]);
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
            // Add your logic here if needed
        }

        private void tenantToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Create an instance of Form2
            Form2 form2 = new Form2();

            // Show Form2
            form2.Show();
        }

        private void leaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Create an instance of Form2
            Form3 form3 = new Form3();

            // Show Form2
            form3.Show();
        }

        private void availableRoomsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Create an instance of Form2
            Form5 form5 = new Form5();

            // Show Form2
            form5.Show();
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }
    }
}
