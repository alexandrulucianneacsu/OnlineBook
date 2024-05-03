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
    public partial class Order_Detail : Form
    {

        private string connectionString = "Data Source=LAPTOP-TPKNAAAB; Initial Catalog=OnlineBook; Integrated Security=True;";

        public Order_Detail()
        {
            InitializeComponent();
            this.dataGridView1.CellClick += new DataGridViewCellEventHandler(this.dataGridView1_CellClick);
            PopulateDataGridView();
        }

        private void PopulateDataGridView()
        {
            try
            {
                dataGridView1.AutoGenerateColumns = false;
                dataGridView1.Columns.Clear();

                dataGridView1.Columns.Add("customerID", "CustomerID");
                dataGridView1.Columns.Add("orderID", "OrderID");
                dataGridView1.Columns.Add("bookID", " BookID");

                dataGridView1.Columns["customerID"].DataPropertyName = "customerID";
                dataGridView1.Columns["orderID"].DataPropertyName = "orderID";
                dataGridView1.Columns["bookID"].DataPropertyName = "bookID";
                
            }
            catch (Exception)
            {
                dataGridView1.Columns["customerID"].Visible = false;
                btnDisplay_Click(this, EventArgs.Empty);
            }
          
        }

        private void btnDisplay_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("SELECT  customerID, orderID, bookID FROM Order_Detail", con);
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
                string.IsNullOrWhiteSpace(txtOrderID.Text) ||
                string.IsNullOrWhiteSpace(txtBookID.Text))

            {
                MessageBox.Show("Please fill in all fields.");
                return;
            }

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    var orderID = Convert.ToInt32(dataGridView1.CurrentRow.Cells["orderID"].Value);
                    SqlCommand cmd = new SqlCommand("INSERT INTO Order_Detail ( customerID, orderID, bookID) VALUES ( @customerID, @orderID, @bookID)", con);

                    cmd.Parameters.AddWithValue("@customerID", txtCustomerID.Text);
                    cmd.Parameters.AddWithValue("@orderID", txtOrderID.Text);
                    cmd.Parameters.AddWithValue("@bookID", txtBookID.Text);


                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Order_Detail successfully added.");
                        btnDisplay_Click(sender, e); // Refresh the display to include the new order_detail
                    }
                    else
                    {
                        MessageBox.Show("No order_detail was added to the database.");

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }

            // Clear the text boxes after saving
            txtCustomerID.Clear();
            txtOrderID.Clear();
            txtBookID.Clear();

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
                        var orderID = Convert.ToInt32(dataGridView1.CurrentRow.Cells["orderID"].Value);
                        SqlCommand cmd = new SqlCommand("UPDATE Order_Detail SET customerID = @customerID, bookID = @bookID  WHERE orderID = @orderID", con);

                        cmd.Parameters.AddWithValue("@customerID", txtCustomerID.Text);
                        cmd.Parameters.AddWithValue("@orderID", txtOrderID.Text);
                        cmd.Parameters.AddWithValue("@bookID", txtBookID.Text);
                        

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Order_Detail successfully updated.");
                            btnDisplay_Click(sender, e); // Refresh the display
                        }
                        else
                        {
                            MessageBox.Show("No order was updated. Please ensure the selected order exists.");
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
                var result = MessageBox.Show("Are you sure you want to delete this order_detail?", "Delete order_detail", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    try
                    {
                        using (SqlConnection con = new SqlConnection(connectionString))
                        {
                            con.Open();
                            var customerID = Convert.ToInt32(dataGridView1.CurrentRow.Cells["customerID"].Value);
                            SqlCommand cmd = new SqlCommand("DELETE FROM Order_Detail WHERE customerID = @customerID", con);
                            cmd.Parameters.AddWithValue("@customerID", customerID);
                            int rowsAffected = cmd.ExecuteNonQuery();
                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Order_Detail successfully deleted.");
                                btnDisplay_Click(sender, e); // Refresh the display
                            }
                            else
                            {
                                MessageBox.Show("The order_detail could not be found or was already deleted.");
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
                txtOrderID.Text = row.Cells["orderID"].Value?.ToString() ?? "";
                txtBookID.Text = row.Cells["bookID"].Value?.ToString() ?? "";

            }
        }

        
    }
}
