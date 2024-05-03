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

namespace Testing
{
    public partial class Author : Form
    {
        private readonly string connectionString = "Data Source=LAPTOP-TPKNAAAB; Initial Catalog=OnlineBook; Integrated Security=True;";

        public Author()
        {
            InitializeComponent();
            this.dataGridView1.CellClick += new DataGridViewCellEventHandler(this.dataGridView1_CellClick);
            PopulateDataGridView();
        }

        private void PopulateDataGridView()
        {
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.Columns.Clear();

            dataGridView1.Columns.Add("authorID", "Author ID");
            dataGridView1.Columns.Add("name", " Name");
            dataGridView1.Columns.Add("country", " Country");

            dataGridView1.Columns["authorID"].DataPropertyName = "authorID";
            dataGridView1.Columns["name"].DataPropertyName = "name";
            dataGridView1.Columns["country"].DataPropertyName = "country";
            

            dataGridView1.Columns["authorID"].Visible = false;

            btnDisplay_Click_1(this, EventArgs.Empty);
        }

       

        private void btnDisplay_Click_1(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("SELECT authorID, name, country FROM Author", con);
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

        private void btnSave_Click_1(object sender, EventArgs e)
        {
            // Check if the input fields are not empty

            if (string.IsNullOrWhiteSpace(txtAuthorID.Text) ||
                string.IsNullOrWhiteSpace(txtAuthorName.Text) ||
                string.IsNullOrWhiteSpace(txtAuthorCountry.Text))

            {
                MessageBox.Show("Please fill in all fields.");
                return;
            }

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("INSERT INTO Author ( name,country) VALUES ( @name, @country)", con);

                    cmd.Parameters.AddWithValue("@authorID", txtAuthorID.Text);
                    cmd.Parameters.AddWithValue("@name", txtAuthorName.Text);
                    cmd.Parameters.AddWithValue("@country", txtAuthorCountry.Text);


                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Author successfully added.");
                        btnDisplay_Click_1(sender, e); // Refresh the display to include the new customer
                    }
                    else
                    {
                        MessageBox.Show("No author was added to the database.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }

            // Clear the text boxes after saving
            txtAuthorID.Clear();
            txtAuthorName.Clear();
            txtAuthorCountry.Clear();
        }

        private void btnUpdate_Click_1(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null && !dataGridView1.CurrentRow.IsNewRow)
            {
                try
                {
                    using (SqlConnection con = new SqlConnection(connectionString))
                    {
                        con.Open();
                        var authorID = Convert.ToInt32(dataGridView1.CurrentRow.Cells["authorID"].Value);
                        SqlCommand cmd = new SqlCommand("UPDATE Author SET name = @name, country = @country WHERE authorID = @authorID", con);

                        cmd.Parameters.AddWithValue("@authorID", txtAuthorID.Text);
                        cmd.Parameters.AddWithValue("@name", txtAuthorName.Text);
                        cmd.Parameters.AddWithValue("@country", txtAuthorCountry.Text);


                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Author successfully updated.");
                            btnDisplay_Click_1(sender, e); // Refresh the display
                        }
                        else
                        {
                            MessageBox.Show("No author was updated. Please ensure the selected author exists.");
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

        private void btnDelete_Click_1(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null && !dataGridView1.CurrentRow.IsNewRow)
            {
                var result = MessageBox.Show("Are you sure you want to delete this author?", "Delete Author", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    try
                    {
                        using (SqlConnection con = new SqlConnection(connectionString))
                        {
                            con.Open();
                            var authorID = Convert.ToInt32(dataGridView1.CurrentRow.Cells["authorID"].Value);
                            SqlCommand cmd = new SqlCommand("DELETE FROM Author WHERE authorID = @authorID", con);
                            cmd.Parameters.AddWithValue("@authorID", authorID);
                            int rowsAffected = cmd.ExecuteNonQuery();
                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Author successfully deleted.");
                                btnDisplay_Click_1(sender, e); // Refresh the display
                            }
                            else
                            {
                                MessageBox.Show("The author could not be found or was already deleted.");
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
                txtAuthorID.Text = row.Cells["authorID"].Value?.ToString() ?? "";
                txtAuthorName.Text = row.Cells["name"].Value?.ToString() ?? "";
                txtAuthorCountry.Text = row.Cells["country"].Value?.ToString() ?? "";

            }
        }
    }
}
