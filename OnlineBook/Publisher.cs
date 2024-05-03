using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OnlineBook
{
    public partial class Publisher : Form
    {
        private string connectionString = "Data Source=LAPTOP-TPKNAAAB; Initial Catalog=OnlineBook; Integrated Security=True;";

        public Publisher()
        {
            InitializeComponent();
            this.dataGridView1.CellClick += new DataGridViewCellEventHandler(this.dataGridView1_CellClick);
            PopulateDataGridView();
        }

        private void PopulateDataGridView()
        {
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.Columns.Clear();

            dataGridView1.Columns.Add("publisherID", "Publisher ID");
            dataGridView1.Columns.Add("name", " Name");
            dataGridView1.Columns.Add("country", " Country");

            dataGridView1.Columns["publisherID"].DataPropertyName = "publisherID";
            dataGridView1.Columns["name"].DataPropertyName = "name";
            dataGridView1.Columns["country"].DataPropertyName = "country";


            dataGridView1.Columns["publisherID"].Visible = false;

            btnDisplay_Click(this, EventArgs.Empty);
        }


        private void btnDisplay_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("SELECT publisherID, name, country FROM Publisher", con);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable table = new DataTable();
                    da.Fill(table);
                    dataGridView1.DataSource = table;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // Check if the input fields are not empty

            if (string.IsNullOrWhiteSpace(txtPublisherID.Text) ||
                string.IsNullOrWhiteSpace(txtPublisherName.Text) ||
                string.IsNullOrWhiteSpace(txtPublisherCountry.Text))

            {
                MessageBox.Show("Please fill in all fields.");
                return;
            }

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("INSERT INTO Publisher ( name,country) VALUES ( @name, @country)", con);

                    cmd.Parameters.AddWithValue("@publisherID", txtPublisherID.Text);
                    cmd.Parameters.AddWithValue("@name", txtPublisherName.Text);
                    cmd.Parameters.AddWithValue("@country", txtPublisherCountry.Text);


                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Publisher successfully added.");
                        btnDisplay_Click(sender, e); // Refresh the display to include the new publisher
                    }
                    else
                    {
                        MessageBox.Show("No publisher was added to the database.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }

            // Clear the text boxes after saving
            txtPublisherID.Clear();
            txtPublisherName.Clear();
            txtPublisherCountry.Clear();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null && !dataGridView1.CurrentRow.IsNewRow)
            {
                try
                {
                    using (SqlConnection con = new SqlConnection(connectionString))
                    {
                        con.Open();
                        var publisherId = Convert.ToInt32(dataGridView1.CurrentRow.Cells["publisherID"].Value);
                        SqlCommand cmd = new SqlCommand("UPDATE Publisher SET name = @name, country = @country WHERE publisherID = @publisherID", con);

                        cmd.Parameters.AddWithValue("@publisherID", txtPublisherID.Text);
                        cmd.Parameters.AddWithValue("@name", txtPublisherName.Text);
                        cmd.Parameters.AddWithValue("@country", txtPublisherCountry.Text);


                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Publisher successfully updated.");
                            btnDisplay_Click(sender, e); // Refresh the display
                        }
                        else
                        {
                            MessageBox.Show("No publisher was updated. Please ensure the selected publisher exists.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Please select a valid row to update.");
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null && !dataGridView1.CurrentRow.IsNewRow)
            {
                var result = MessageBox.Show("Are you sure you want to delete this publisher?", "Delete Publisher", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    try
                    {
                        using (SqlConnection con = new SqlConnection(connectionString))
                        {
                            con.Open();
                            var publisherID = Convert.ToInt32(dataGridView1.CurrentRow.Cells["publisherID"].Value);
                            SqlCommand cmd = new SqlCommand("DELETE FROM Publisher WHERE publisherID = @publisherID", con);
                            cmd.Parameters.AddWithValue("@publisherID", publisherID);
                            int rowsAffected = cmd.ExecuteNonQuery();
                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Publisher successfully deleted.");
                                btnDisplay_Click(sender, e); // Refresh the display
                            }
                            else
                            {
                                MessageBox.Show("The publisher could not be found or was already deleted.");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("An error occurred: " + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a valid row to delete.");
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && !dataGridView1.Rows[e.RowIndex].IsNewRow)
            {
                var row = dataGridView1.Rows[e.RowIndex];
                txtPublisherID.Text = row.Cells["publisherID"].Value?.ToString() ?? "";
                txtPublisherName.Text = row.Cells["name"].Value?.ToString() ?? "";
                txtPublisherCountry.Text = row.Cells["country"].Value?.ToString() ?? "";

            }
        }
    }
}
