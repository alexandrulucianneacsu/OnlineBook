using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Security.Policy;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace OnlineBook
{
    public partial class Customer : Form
    {
        private string connectionString = "Data Source=LAPTOP-TPKNAAAB; Initial Catalog=OnlineBook; Integrated Security=True;";

        public string ConnectionString { get => connectionString; set => connectionString = value; }

        public Customer()    
        {
            InitializeComponent();
            this.dataGridView1.CellClick += new DataGridViewCellEventHandler(this.dataGridView1_CellClick);
            PopulateDataGridView();
        }

        private void PopulateDataGridView()
        {
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.Columns.Clear();

            dataGridView1.Columns.Add("customerID", "Customer ID");
            dataGridView1.Columns.Add("firstName", "First Name");
            dataGridView1.Columns.Add("lastName", "Last Name");
            dataGridView1.Columns.Add("email", "Email");
            dataGridView1.Columns.Add("phone", "Phone");

            dataGridView1.Columns["customerID"].DataPropertyName = "customerID";
            dataGridView1.Columns["firstName"].DataPropertyName = "firstName";
            dataGridView1.Columns["lastName"].DataPropertyName = "lastName";
            dataGridView1.Columns["email"].DataPropertyName = "email";
            dataGridView1.Columns["phone"].DataPropertyName = "phone";

            dataGridView1.Columns["customerID"].Visible = false;

            btnDisplay_Click(this, EventArgs.Empty);
        }

        private void btnDisplay_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("SELECT customerID, firstName, lastName, email, phone FROM Customer", con);
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
            if (string.IsNullOrWhiteSpace(txtCustomerID.Text) ||
                string.IsNullOrWhiteSpace(txtFirstName.Text) ||
                string.IsNullOrWhiteSpace(txtLastName.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text) ||
                string.IsNullOrWhiteSpace(txtPhone.Text))
            {
                MessageBox.Show("Please fill in all fields.");
                return;
            }

            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("INSERT INTO Customer (firstName, lastName, email, phone) VALUES (@firstName, @lastName, @email, @phone)", con);

                    cmd.Parameters.AddWithValue("@customerID", txtCustomerID.Text);
                    cmd.Parameters.AddWithValue("@firstName", txtFirstName.Text);
                    cmd.Parameters.AddWithValue("@lastName", txtLastName.Text);
                    cmd.Parameters.AddWithValue("@email", txtEmail.Text);
                    cmd.Parameters.AddWithValue("@phone", txtPhone.Text);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Customer successfully added.");
                        btnDisplay_Click(sender, e); // Refresh the display to include the new customer
                    }
                    else
                    {
                        MessageBox.Show("No customer was added to the database.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }

            // Clear the text boxes after saving
            txtCustomerID.Clear();
            txtFirstName.Clear();
            txtLastName.Clear();
            txtEmail.Clear();
            txtPhone.Clear();
        }


       

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null && !dataGridView1.CurrentRow.IsNewRow)
            {
                try
                {
                    using (SqlConnection con = new SqlConnection(ConnectionString))
                    {
                        con.Open();
                        var customerID = Convert.ToInt32(dataGridView1.CurrentRow.Cells["customerID"].Value);
                        SqlCommand cmd = new SqlCommand("UPDATE Customer SET firstName = @firstName, lastName = @lastName, email = @email, phone = @phone WHERE customerID = @customerID", con);

                        cmd.Parameters.AddWithValue("@customerID", txtCustomerID.Text);
                        cmd.Parameters.AddWithValue("@firstName", txtFirstName.Text);
                        cmd.Parameters.AddWithValue("@lastName", txtLastName.Text);
                        cmd.Parameters.AddWithValue("@email", txtEmail.Text);
                        cmd.Parameters.AddWithValue("@phone", txtPhone.Text);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Customer successfully updated.");
                            btnDisplay_Click(sender, e); // Refresh the display
                        }
                        else
                        {
                            MessageBox.Show("No customer was updated. Please ensure the selected customer exists.");
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
                var result = MessageBox.Show("Are you sure you want to delete this customer?", "Delete Customer", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    try
                    {
                        using (SqlConnection con = new SqlConnection(ConnectionString))
                        {
                            con.Open();
                            var customerID = Convert.ToInt32(dataGridView1.CurrentRow.Cells["customerID"].Value);
                            SqlCommand cmd = new SqlCommand("DELETE FROM Customer WHERE customerID = @customerID", con);
                            cmd.Parameters.AddWithValue("@customerID", customerID);
                            int rowsAffected = cmd.ExecuteNonQuery();
                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Customer successfully deleted.");
                                btnDisplay_Click(sender, e); // Refresh the display
                            }
                            else
                            {
                                MessageBox.Show("The customer could not be found or was already deleted.");
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
                txtCustomerID.Text = row.Cells["customerID"].Value?.ToString() ?? "";
                txtFirstName.Text = row.Cells["firstName"].Value?.ToString() ?? "";
                txtLastName.Text = row.Cells["lastName"].Value?.ToString() ?? "";
                txtEmail.Text = row.Cells["email"].Value?.ToString() ?? "";
                txtPhone.Text = row.Cells["phone"].Value?.ToString() ?? "";
            }
        }
    }
}


