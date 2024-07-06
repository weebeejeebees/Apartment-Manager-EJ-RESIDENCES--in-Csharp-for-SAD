using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace apartmentManager
{
    public partial class Form1 : Form
    {
        // Singleton instance
        private static Form1 _instance;

        // Public property to access the instance
        public static Form1 Instance
        {
            get
            {
                if (_instance == null || _instance.IsDisposed)
                {
                    _instance = new Form1();
                }
                return _instance;
            }
        }

        koneksyon kon = new koneksyon();
        SqlConnection conn;
        SqlCommand cmd;
        SqlDataReader rd;

        private static Form2 _form2;
        private static Form3 _form3;
        private static Form4 _form4;
        private static Form5 _form5;

        // Public constructor
        public Form1()
        {
            InitializeComponent();
            comboBox2.Items.AddRange(new string[] { "apartmentId", "apartmentNumber", "availability", "notes" });
            comboBox2.SelectedIndex = 0; // Default selection to apartmentId
            SetColumnHeaderStyles();
        }

        private void apartmentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_form2 == null || _form2.IsDisposed)
            {
                _form2 = new Form2();
                _form2.Show();
            }
            else
            {
                _form2.BringToFront();
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {
        }

        private void Form1_Load(object sender, EventArgs e)
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

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            conn = kon.getCon();
            conn.Open();
            cmd = new SqlCommand("INSERT INTO apartments (apartmentNumber, availability, notes) VALUES (@apartmentNumber, @availability, @notes)", conn);
            cmd.Parameters.AddWithValue("@apartmentNumber", textBox1.Text);
            cmd.Parameters.AddWithValue("@availability", comboBox1.Text); // Use comboBox1 for availability
            cmd.Parameters.AddWithValue("@notes", textBox4.Text);
            cmd.ExecuteNonQuery();

            MessageBox.Show("New record saved");
            textBox1.Text = "";
            comboBox1.Text = ""; // Clear comboBox1
            textBox4.Text = "";

            cmd.Dispose();
            conn.Close();

            button2_Click(this, EventArgs.Empty);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            conn = kon.getCon();
            conn.Open();

            // Clear existing data in DataGridView
            dataGridView1.Rows.Clear();

            // Execute SELECT query to retrieve data from apartments table
            string query = "SELECT * FROM apartments";
            if (!string.IsNullOrEmpty(textBox5.Text) && comboBox2.SelectedItem != null) // Apply filter only if textBox5 is not empty and comboBox2 has a selection
            {
                string selectedColumn = comboBox2.SelectedItem.ToString();
                if (selectedColumn == "apartmentId" && int.TryParse(textBox5.Text, out int apartmentId))
                {
                    query += $" WHERE apartmentId = {apartmentId}";
                }
                else
                {
                    query += $" WHERE {selectedColumn} LIKE @searchText";
                }
            }

            cmd = new SqlCommand(query, conn);
            if (!string.IsNullOrEmpty(textBox5.Text) && comboBox2.SelectedItem != null)
            {
                cmd.Parameters.AddWithValue("@searchText", "%" + textBox5.Text + "%");
            }
            rd = cmd.ExecuteReader();

            // Iterate through the results and populate the DataGridView
            while (rd.Read())
            {
                dataGridView1.Rows.Add(rd["apartmentId"], rd["apartmentNumber"], rd["availability"], rd["notes"]);
            }

            rd.Close();
            conn.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            conn = kon.getCon();
            conn.Open();
            cmd = new SqlCommand("UPDATE apartments SET apartmentNumber = @apartmentNumber, availability = @availability, notes = @notes WHERE apartmentId = @apartmentId", conn);
            cmd.Parameters.AddWithValue("@apartmentId", textBox5.Text); // Use textBox5 for apartmentId
            cmd.Parameters.AddWithValue("@apartmentNumber", textBox1.Text);
            cmd.Parameters.AddWithValue("@availability", comboBox1.Text); // Use comboBox1 for availability
            cmd.Parameters.AddWithValue("@notes", textBox4.Text);

            int rowsAffected = cmd.ExecuteNonQuery();
            if (rowsAffected > 0)
            {
                MessageBox.Show("Record updated successfully");
            }

            else
            {
                MessageBox.Show("No record found with the provided Apartment ID");
            }

            textBox1.Text = "";
            comboBox1.Text = ""; // Clear comboBox1
            textBox4.Text = "";
            textBox5.Text = "";

            cmd.Dispose();
            conn.Close();
            button2_Click(this, EventArgs.Empty);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            conn = kon.getCon();
            conn.Open();
            cmd = new SqlCommand("DELETE FROM apartments WHERE apartmentId = @apartmentId", conn);
            cmd.Parameters.AddWithValue("@apartmentId", textBox5.Text); // Use textBox5 for apartmentId

            int rowsAffected = cmd.ExecuteNonQuery();
            if (rowsAffected > 0)
            {
                MessageBox.Show("Record deleted successfully");
            }
            else
            {
                MessageBox.Show("No record found with the provided Apartment ID");
            }

            textBox1.Text = "";
            comboBox1.Text = ""; // Clear comboBox1
            textBox4.Text = "";
            textBox5.Text = "";

            cmd.Dispose();
            conn.Close();
            button2_Click(this, EventArgs.Empty);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
        }

        private void tenantToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_form2 == null || _form2.IsDisposed)
            {
                _form2 = new Form2();
                _form2.Show();
            }
            else
            {
                _form2.BringToFront();
            }
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
                _form3.BringToFront();
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
                _form4.BringToFront();
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
                _form5.BringToFront();
            }
        }

        private void reportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_form5 == null || _form5.IsDisposed)
            {
                _form5 = new Form5();
                _form5.Show();
            }
            else
            {
                _form5.BringToFront();
            }
        }


        private void label3_Click(object sender, EventArgs e)
        {
        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (comboBox2.SelectedItem != null && !string.IsNullOrEmpty(textBox5.Text))
            {
                string selectedColumn = comboBox2.SelectedItem.ToString();
                string searchText = textBox5.Text;

                // Clear existing data in DataGridView
                dataGridView1.Rows.Clear();

                // Execute SELECT query to retrieve data from apartments table
                string query = "SELECT * FROM apartments";
                if (selectedColumn == "apartmentId" && int.TryParse(searchText, out int apartmentId))
                {
                    query += $" WHERE apartmentId = {apartmentId}";
                }
                else
                {
                    query += $" WHERE {selectedColumn} LIKE @searchText";
                }

                conn = kon.getCon();
                conn.Open();
                cmd = new SqlCommand(query, conn);
                if (selectedColumn != "apartmentId")
                {
                    cmd.Parameters.AddWithValue("@searchText", "%" + searchText + "%");
                }
                rd = cmd.ExecuteReader();

                // Iterate through the results and populate the DataGridView
                while (rd.Read())
                {
                    dataGridView1.Rows.Add(rd["apartmentId"], rd["apartmentNumber"], rd["availability"], rd["notes"]);
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
    }
}
