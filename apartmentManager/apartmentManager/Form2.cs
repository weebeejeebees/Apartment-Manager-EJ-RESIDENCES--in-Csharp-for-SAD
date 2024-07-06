using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace apartmentManager
{
    public partial class Form2 : Form
    {
        koneksyon kon = new koneksyon();
        SqlConnection conn;
        SqlCommand cmd;
        SqlDataReader rd;

        private static Form3 _form3;
        private static Form4 _form4;
        private static Form5 _form5;

        public Form2()
        {
            InitializeComponent();
            SetColumnHeaderStyles();
            button2_Click(this, EventArgs.Empty);
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            button2_Click(this, EventArgs.Empty);
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
            Form1.Instance.Show();
            Form1.Instance.BringToFront();
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

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            conn = kon.getCon();
            conn.Open();
            try
            {
                cmd = new SqlCommand("INSERT INTO tenants (apartmentNumber, name, age, gender, contactInformation) VALUES (@apartmentNumber, @name, @age, @gender, @contactInformation); SELECT SCOPE_IDENTITY();", conn);
                cmd.Parameters.AddWithValue("@apartmentNumber", textBox3.Text);
                cmd.Parameters.AddWithValue("@name", textBox1.Text);
                cmd.Parameters.AddWithValue("@age", textBox2.Text);
                cmd.Parameters.AddWithValue("@gender", comboBox1.SelectedItem?.ToString());
                cmd.Parameters.AddWithValue("@contactInformation", textBox4.Text);
                int tenantId = Convert.ToInt32(cmd.ExecuteScalar());
                MessageBox.Show("New tenant added with ID: " + tenantId.ToString());
            }
            catch (SqlException ex)
            {
                MessageBox.Show("SQL Error: " + ex.Message);
            }
            finally
            {
                textBox1.Text = "";
                textBox2.Text = "";
                comboBox1.SelectedIndex = -1;
                textBox4.Text = "";
                textBox3.Text = "";
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
                cmd = new SqlCommand("SELECT tenantId, apartmentNumber, name, age, gender, contactInformation FROM tenants", conn);
                rd = cmd.ExecuteReader();
                while (rd.Read())
                {
                    dataGridView1.Rows.Add(rd["tenantId"], rd["apartmentNumber"], rd["name"], rd["age"], rd["gender"], rd["contactInformation"]);
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
                string.IsNullOrWhiteSpace(textBox2.Text) ||
                string.IsNullOrWhiteSpace(comboBox1.SelectedItem?.ToString()) ||
                string.IsNullOrWhiteSpace(textBox4.Text) ||
                string.IsNullOrWhiteSpace(textBox5.Text) ||
                string.IsNullOrWhiteSpace(textBox3.Text))
            {
                MessageBox.Show("Please fill all fields before updating.");
                conn.Close();
                return;
            }

            try
            {
                SqlCommand checkCmd = new SqlCommand("SELECT COUNT(*) FROM tenants WHERE tenantId = @tenantId", conn);
                checkCmd.Parameters.AddWithValue("@tenantId", textBox5.Text);
                int count = (int)checkCmd.ExecuteScalar();

                if (count == 0)
                {
                    MessageBox.Show("No tenant found with the provided Tenant ID");
                    conn.Close();
                    return;
                }

                cmd = new SqlCommand("UPDATE tenants SET apartmentNumber = @apartmentNumber, name = @name, age = @age, gender = @gender, contactInformation = @contactInformation WHERE tenantId = @tenantId", conn);
                cmd.Parameters.AddWithValue("@apartmentNumber", textBox3.Text);
                cmd.Parameters.AddWithValue("@name", textBox1.Text);
                cmd.Parameters.AddWithValue("@age", textBox2.Text);
                cmd.Parameters.AddWithValue("@gender", comboBox1.SelectedItem?.ToString());
                cmd.Parameters.AddWithValue("@contactInformation", textBox4.Text);
                cmd.Parameters.AddWithValue("@tenantId", textBox5.Text);

                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    MessageBox.Show("Tenant updated successfully");
                }
                else
                {
                    MessageBox.Show("Failed to update tenant");
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
                cmd = new SqlCommand("DELETE FROM tenants WHERE tenantId = @tenantId", conn);
                cmd.Parameters.AddWithValue("@tenantId", textBox5.Text);
                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    MessageBox.Show("Tenant deleted successfully");
                }
                else
                {
                    MessageBox.Show("No tenant found with the provided Tenant ID");
                }
                textBox1.Text = "";
                textBox2.Text = "";
                comboBox1.SelectedIndex = -1;
                textBox4.Text = "";
                textBox5.Text = "";
                textBox3.Text = "";
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
            if (comboBox2.SelectedItem != null && !string.IsNullOrEmpty(textBox5.Text))
            {
                string selectedColumn = comboBox2.SelectedItem.ToString();
                string searchText = textBox5.Text;

                // Clear existing data in DataGridView
                dataGridView1.Rows.Clear();

                // Execute SELECT query to retrieve data from tenants table
                string query = "SELECT * FROM tenants";
                if (selectedColumn == "tenantId" && int.TryParse(searchText, out int tenantId))
                {
                    query += $" WHERE tenantId = {tenantId}";
                }
                else
                {
                    query += $" WHERE {selectedColumn} LIKE @searchText";
                }

                conn = kon.getCon();
                conn.Open();
                cmd = new SqlCommand(query, conn);
                if (selectedColumn != "tenantId")
                {
                    cmd.Parameters.AddWithValue("@searchText", "%" + searchText + "%");
                }
                rd = cmd.ExecuteReader();

                // Iterate through the results and populate the DataGridView
                while (rd.Read())
                {
                    dataGridView1.Rows.Add(rd["tenantId"], rd["apartmentNumber"], rd["name"], rd["age"], rd["gender"], rd["contactInformation"]);
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

        private void label5_Click(object sender, EventArgs e)
        {
        }

        private void label3_Click(object sender, EventArgs e)
        {
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void leaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_form3 == null || _form3.IsDisposed)
            {
                _form3 = new Form3();
                _form3.Show();
            }
            else
            {
                MessageBox.Show("Form is already open.");
            }
        }

        private void transactionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_form4 == null || _form4.IsDisposed)
            {
                _form4 = new Form4();
                _form4.Show();
            }
            else
            {
                MessageBox.Show("Form is already open.");
            }
        }

        private void availableRoomsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_form5 == null || _form5.IsDisposed)
            {
                _form5 = new Form5();
                _form5.Show();
            }
            else
            {
                MessageBox.Show("Form is already open.");
            }
        }
    }
}
