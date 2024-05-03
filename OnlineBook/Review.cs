using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Security.Policy;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OnlineBook
{
    public partial class Review : Form
    {
        private string connectionString = "Data Source=LAPTOP-TPKNAAAB; Initial Catalog=OnlineBook; Integrated Security=True;";

        public Review()
        {
            InitializeComponent();
            this.dataGridView1.CellClick += new DataGridViewCellEventHandler(this.dataGridView1_CellClick);
            PopulateDataGridView();
        }

        private void PopulateDataGridView()
        {
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.Columns.Clear();

            dataGridView1.Columns.Add("reviewID", "Review ID");
            dataGridView1.Columns.Add("reviewDate", " Review Date");
            dataGridView1.Columns.Add("reviewName", " Review Name");

            dataGridView1.Columns["reviewID"].DataPropertyName = "reviewID";
            dataGridView1.Columns["reviewDate"].DataPropertyName = "reviewDate";
            dataGridView1.Columns["reviewName"].DataPropertyName = "reviewName";


            dataGridView1.Columns["reviewID"].Visible = false;

            btnDisplay_Click(this, EventArgs.Empty);
        }

      

        private void btnDisplay_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("SELECT reviewID, reviewDate, reviewName FROM Review", con);
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

            if (string.IsNullOrWhiteSpace(txtReviewID.Text) ||
                string.IsNullOrWhiteSpace(txtReviewDate.Text) ||
                string.IsNullOrWhiteSpace(txtReviewName.Text))

            {
                MessageBox.Show("Please fill in all fields.");
                return;
            }

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("INSERT INTO Review (reviewDate, reviewName) VALUES (@reviewDate, @reviewName)", con);

                    cmd.Parameters.AddWithValue("@reviewID", txtReviewID.Text);
                    cmd.Parameters.AddWithValue("@reviewDate", txtReviewDate.Text);
                    cmd.Parameters.AddWithValue("@reviewName", txtReviewName.Text);


                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Review successfully added.");
                        btnDisplay_Click(sender, e); // Refresh the display to include the new review
                    }
                    else
                    {
                        MessageBox.Show("No review was added to the database.");

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }

            // Clear the text boxes after saving
            txtReviewID.Clear();
            txtReviewDate.Clear();
            txtReviewName.Clear();

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
                        var reviewID = Convert.ToString(dataGridView1.CurrentRow.Cells["reviewID"].Value);
                        SqlCommand cmd = new SqlCommand("UPDATE Review SET reviewDate = @reviewDate, reviewName = @reviewName WHERE reviewID = @reviewID", con);

                        cmd.Parameters.AddWithValue("@reviewID", txtReviewID.Text);
                        cmd.Parameters.AddWithValue("@reviewDate", txtReviewDate.Text);
                        cmd.Parameters.AddWithValue("@reviewName", txtReviewName.Text);


                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Review successfully updated.");
                            btnDisplay_Click(sender, e); // Refresh the display
                        }
                        else
                        {
                            MessageBox.Show("No review was updated. Please ensure the selected review exists. ");
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
                var result = MessageBox.Show("Are you sure you want to delete this review?", "Delete Review", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    try
                    {
                        using (SqlConnection con = new SqlConnection(connectionString))
                        {
                            con.Open();
                            var reviewID = Convert.ToInt32(dataGridView1.CurrentRow.Cells["reviewID"].Value);
                            SqlCommand cmd = new SqlCommand("DELETE FROM Review WHERE reviewID = @reviewID", con);
                            cmd.Parameters.AddWithValue("@reviewID", reviewID);
                            int rowsAffected = cmd.ExecuteNonQuery();
                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Review successfully deleted.");
                                btnDisplay_Click(sender, e); // Refresh the display
                            }
                            else
                            {
                                MessageBox.Show("The review could not be found or was already deleted.");
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
                txtReviewID.Text = row.Cells["reviewID"].Value?.ToString() ?? "";
                txtReviewDate.Text = row.Cells["reviewDate"].Value?.ToString() ?? "";
                txtReviewName.Text = row.Cells["reviewName"].Value?.ToString() ?? "";

            }
        }
    }
}
