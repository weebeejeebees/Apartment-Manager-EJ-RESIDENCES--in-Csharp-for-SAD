using iTextSharp.text.pdf;
using iTextSharp.text;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace apartmentManager
{
    public partial class Form5 : Form
    {
        private readonly koneksyon _kon = new koneksyon();

        public Form5()
        {
            InitializeComponent();
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "MMMM yyyy";
            SetColumnHeaderStyles();
        }

        private void LoadData()
        {
            using (SqlConnection conn = _kon.getCon())
            {
                conn.Open();

                string query = @"
            SELECT 
                t.tenantId, 
                t.name, 
                t.apartmentNumber, 
                p.paymentStatus, 
                SUM(p.amountPaid) AS amountPaid
            FROM 
                tenants t
            INNER JOIN payment p ON t.tenantId = p.tenantId
            GROUP BY 
                t.tenantId, 
                t.name, 
                t.apartmentNumber, 
                p.paymentStatus
            ORDER BY 
                t.tenantId";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataReader rd = cmd.ExecuteReader())
                    {
                        dataGridView1.Rows.Clear();
                        while (rd.Read())
                        {
                            dataGridView1.Rows.Add(rd["tenantId"], rd["name"], rd["apartmentNumber"], rd["paymentStatus"], rd["amountPaid"]);
                        }
                    }
                }
            }
        }

        private void SetColumnHeaderStyles()
        {
            DataGridViewCellStyle columnHeaderStyle = new DataGridViewCellStyle();
            columnHeaderStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            columnHeaderStyle.BackColor = SystemColors.Control;
            columnHeaderStyle.Font = new System.Drawing.Font("Bahnschrift", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            columnHeaderStyle.ForeColor = SystemColors.WindowText;
            columnHeaderStyle.SelectionBackColor = SystemColors.Highlight;
            columnHeaderStyle.SelectionForeColor = SystemColors.HighlightText;
            columnHeaderStyle.WrapMode = DataGridViewTriState.True;

            dataGridView1.ColumnHeadersDefaultCellStyle = columnHeaderStyle;
        }

        private void FilterData()
        {
            using (SqlConnection conn = _kon.getCon())
            {
                conn.Open();

                string query = @"
            SELECT 
                t.tenantId, 
                t.name, 
                t.apartmentNumber, 
                (SELECT TOP 1 CAST(p.paymentStatus AS varchar(50)) FROM payment p WHERE p.tenantId = t.tenantId ORDER BY p.dateOfTransaction DESC) AS paymentStatus, 
                (SELECT SUM(p.amountPaid) FROM payment p WHERE p.tenantId = t.tenantId AND MONTH(p.dateOfTransaction) = @month AND YEAR(p.dateOfTransaction) = @year) AS amountPaid
            FROM 
                tenants t
            WHERE 
                EXISTS (SELECT 1 FROM payment p WHERE p.tenantId = t.tenantId AND MONTH(p.dateOfTransaction) = @month AND YEAR(p.dateOfTransaction) = @year)
            ORDER BY 
                t.tenantId";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    DateTime selectedDate = dateTimePicker1.Value;
                    Console.WriteLine($"Selected date: {selectedDate}");

                    cmd.Parameters.AddWithValue("@month", selectedDate.Month);
                    cmd.Parameters.AddWithValue("@year", selectedDate.Year);

                    Console.WriteLine($"@month: {cmd.Parameters["@month"].Value}");
                    Console.WriteLine($"@year: {cmd.Parameters["@year"].Value}");

                    using (SqlDataReader rd = cmd.ExecuteReader())
                    {
                        dataGridView1.Rows.Clear();
                        int totalTenants = 0;
                        decimal totalAmountPaid = 0;
                        int fullyPaidOrOngoingTenants = 0;
                        int totalOngoingTenants = 0;
                        int totalUnpaidTenants = 0;

                        while (rd.Read())
                        {
                            dataGridView1.Rows.Add(rd["tenantId"], rd["name"], rd["apartmentNumber"], rd["paymentStatus"], rd["amountPaid"]);

                            totalTenants++;

                            if (rd["amountPaid"] != DBNull.Value)
                            {
                                totalAmountPaid += Convert.ToDecimal(rd["amountPaid"]);
                            }

                            if (rd["paymentStatus"].ToString() == "Fully Paid" || rd["paymentStatus"].ToString() == "Ongoing")
                            {
                                fullyPaidOrOngoingTenants++;
                            }

                            if (rd["paymentStatus"].ToString() == "Ongoing")
                            {
                                totalOngoingTenants++;
                            }
                            else if (rd["paymentStatus"].ToString() == "Unpaid")
                            {
                                totalUnpaidTenants++;
                            }
                        }

                        label3.Text = $"Fully Paid or Ongoing Tenants: {fullyPaidOrOngoingTenants}";
                        label4.Text = $"Total Amount Paid: {totalAmountPaid:C}";
                        label5.Text = $"Total Ongoing Tenants: {totalOngoingTenants}";
                        label7.Text = $"Total Unpaid Tenants: {totalUnpaidTenants}";
                    }
                }
            }
        }

        private void Form5_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FilterData();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            FilterData();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "PDF Files (*.pdf)|*.pdf";
            saveFileDialog.FileName = "FilteredData.pdf";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                Document document = new Document();
                PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(saveFileDialog.FileName, FileMode.Create));

                document.Open();

                // Add a title to the PDF
                Paragraph title = new Paragraph("Filtered Data", iTextSharp.text.FontFactory.GetFont("Arial", 20, iTextSharp.text.Font.BOLD));
                title.Alignment = Element.ALIGN_CENTER;
                document.Add(title);

                // Add a line break
                document.Add(new Paragraph(" "));

                // Add the labels to the PDF
                if (label3 != null)
                {
                    Paragraph labelParagraph = new Paragraph(label3.Text, iTextSharp.text.FontFactory.GetFont("Arial", 12, iTextSharp.text.Font.NORMAL));
                    labelParagraph.Alignment = Element.ALIGN_LEFT;
                    document.Add(labelParagraph);
                }

                if (label4 != null)
                {
                    Paragraph labelParagraph = new Paragraph(label4.Text, iTextSharp.text.FontFactory.GetFont("Arial", 12, iTextSharp.text.Font.NORMAL));
                    labelParagraph.Alignment = Element.ALIGN_LEFT;
                    document.Add(labelParagraph);
                }

                if (label5 != null)
                {
                    Paragraph labelParagraph = new Paragraph(label5.Text, iTextSharp.text.FontFactory.GetFont("Arial", 12, iTextSharp.text.Font.NORMAL));
                    labelParagraph.Alignment = Element.ALIGN_LEFT;
                    document.Add(labelParagraph);
                }

                if (label7 != null)
                {
                    Paragraph labelParagraph = new Paragraph(label7.Text, iTextSharp.text.FontFactory.GetFont("Arial", 12, iTextSharp.text.Font.NORMAL));
                    labelParagraph.Alignment = Element.ALIGN_LEFT;
                    document.Add(labelParagraph);
                }

                // Add a line break
                document.Add(new Paragraph(" "));

                // Add the DataGridView to the PDF
                if (dataGridView1 != null && dataGridView1.RowCount > 0)
                {
                    PdfPTable table = new PdfPTable(dataGridView1.ColumnCount);
                    table.DefaultCell.Padding = 3;
                    table.WidthPercentage = 100;
                    table.DefaultCell.BorderWidth = 1;

                    foreach (DataGridViewColumn column in dataGridView1.Columns)
                    {
                        table.AddCell(new Phrase(column.HeaderText, iTextSharp.text.FontFactory.GetFont("Arial", 12, iTextSharp.text.Font.BOLD)));
                    }

                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        foreach (DataGridViewCell cell in row.Cells)
                        {
                            if (cell.Value != null)
                            {
                                table.AddCell(new Phrase(cell.Value.ToString(), iTextSharp.text.FontFactory.GetFont("Arial", 12, iTextSharp.text.Font.NORMAL)));
                            }
                            else
                            {
                                table.AddCell(new Phrase("", iTextSharp.text.FontFactory.GetFont("Arial", 12, iTextSharp.text.Font.NORMAL)));
                            }
                        }
                    }

                    document.Add(table);
                }

                document.Close();

                MessageBox.Show("PDF generated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void apartmentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Create an instance of Form2
            Form1 form1 = new Form1();

            // Show Form2
            form1.Show();
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

        private void transactionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Create an instance of Form2
            Form4 form4 = new Form4();

            // Show Form2
            form4.Show();
        }

        private void label3_Click_1(object sender, EventArgs e)
        {

        }
    }
}