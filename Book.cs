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
    public partial class Book : Form
    {
        private readonly string connectionString = "Data Source=LAPTOP-TPKNAAAB; Initial Catalog=OnlineBook; Integrated Security=True;";

        //delegate 

        public delegate void DatabaseOperationDelegate();

        public Book()
        {
            InitializeComponent();
            this.dataGridView1.CellClick += new DataGridViewCellEventHandler(this.dataGridView_CellClick);
            PopulateDataGridView();
        }

        private void PopulateDataGridView()
        {
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.Columns.Clear();

            dataGridView1.Columns.Add("bookID", "Book ID");
            dataGridView1.Columns.Add("name", " Name");
            dataGridView1.Columns.Add("price", " Price");
            dataGridView1.Columns.Add("author", " Author");

            dataGridView1.Columns["bookID"].DataPropertyName = "bookID";
            dataGridView1.Columns["name"].DataPropertyName = "name";
            dataGridView1.Columns["price"].DataPropertyName = "price";
            dataGridView1.Columns["author"].DataPropertyName = "author";


            dataGridView1.Columns["bookID"].Visible = false;

            btnDisplay_Click(this, EventArgs.Empty);
        }



        private void btnSave_Click(object sender, EventArgs e)
        {
            // Check if the input fields are not empty
            if (string.IsNullOrWhiteSpace(txtBookID.Text) ||
               string.IsNullOrWhiteSpace(txtBookName.Text) ||
               string.IsNullOrWhiteSpace(txtBookPrice.Text) ||
               string.IsNullOrWhiteSpace(txtBookAuthor.Text))

            {
                MessageBox.Show("Please fill in all fields.");
                return;
            }

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("INSERT INTO Book ( name, price, author) VALUES ( @name, @price, @author)", con);

                    cmd.Parameters.AddWithValue("@bookID", txtBookID.Text);
                    cmd.Parameters.AddWithValue("@name", txtBookName.Text);
                    cmd.Parameters.AddWithValue("@price", txtBookPrice.Text);
                    cmd.Parameters.AddWithValue("@author", txtBookAuthor.Text);


                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Book successfully added.");
                        btnDisplay_Click(sender, e); // Refresh the display to include the new book
                    }
                    else
                    {
                        MessageBox.Show("No book was added to the database.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }

            // Clear the text boxes after saving
            txtBookID.Clear();
            txtBookName.Clear();
            txtBookPrice.Clear();
            txtBookAuthor.Clear();
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
                        var bookId = Convert.ToInt32(dataGridView1.CurrentRow.Cells["bookID"].Value);
                        SqlCommand cmd = new SqlCommand("UPDATE Book SET name = @name, price = @price, author = @author WHERE bookID = @bookID", con);

                        cmd.Parameters.AddWithValue("@bookID", txtBookID.Text);
                        cmd.Parameters.AddWithValue("@name", txtBookName.Text);
                        cmd.Parameters.AddWithValue("@price", txtBookPrice.Text);
                        cmd.Parameters.AddWithValue("@author", txtBookAuthor.Text);


                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Book successfully updated.");
                            btnDisplay_Click(sender, e); // Refresh the display
                        }
                        else
                        {
                            MessageBox.Show("No book was updated. Please ensure the selected book exists.");
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
                var result = MessageBox.Show("Are you sure you want to delete this book?", "Delete Book", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    try
                    {
                        using (SqlConnection con = new SqlConnection(connectionString))
                        {
                            con.Open();
                            var bookID = Convert.ToInt32(dataGridView1.CurrentRow.Cells["bookID"].Value);
                            SqlCommand cmd = new SqlCommand("DELETE FROM Book WHERE bookID = @bookID", con);
                            cmd.Parameters.AddWithValue("@bookID", bookID);
                            int rowsAffected = cmd.ExecuteNonQuery();
                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Book successfully deleted.");
                                btnDisplay_Click(sender, e); // Refresh the display
                            }
                            else
                            {
                                MessageBox.Show("The book could not be found or was already deleted.");
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

        private void btnDisplay_Click(object sender, EventArgs e)
        {
            ExecuteDatabaseOperation(() =>
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        SqlCommand cmd = new SqlCommand("SELECT bookID, name,price, author FROM Book", connection);
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable table = new DataTable();
                        adapter.Fill(table);
                        dataGridView1.DataSource = table;
                    }

                });
        }

        /*private void ExecuteDatabaseOperation()
        {
            throw new NotImplementedException();
        }*/

        private void ExecuteDatabaseOperation(DatabaseOperationDelegate operation)
        {
            try
            {
                operation();
            }
            catch (Exception ex)
            {

                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }
        private void dataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0 && !dataGridView1.Rows[e.RowIndex].IsNewRow)
                {
                    var row = dataGridView1.Rows[e.RowIndex];
                    txtBookID.Text = row.Cells["bookID"].Value?.ToString() ?? "";
                    txtBookName.Text = row.Cells["name"].Value?.ToString() ?? "";
                    txtBookPrice.Text = row.Cells["price"].Value?.ToString() ?? "";
                    txtBookAuthor.Text = row.Cells["author"].Value?.ToString() ?? "";

                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }


        //cod before delegate

        /* public partial class Book : Form
         {
             private readonly string connectionString = "Data Source=LAPTOP-TPKNAAAB; Initial Catalog=OnlineBook; Integrated Security=True;";

             public Book()
             {
                 InitializeComponent();
                 this.dataGridView1.CellClick += new DataGridViewCellEventHandler(this.dataGridView1_CellClick);
                 PopulateDataGridView();
             }

             private void PopulateDataGridView()
             {
                 dataGridView1.AutoGenerateColumns = false;
                 dataGridView1.Columns.Clear();

                 dataGridView1.Columns.Add("bookID", "Book ID");
                 dataGridView1.Columns.Add("name", " Name");
                 dataGridView1.Columns.Add("price", " Price");
                 dataGridView1.Columns.Add("author", " Author");

                 dataGridView1.Columns["bookID"].DataPropertyName = "bookID";
                 dataGridView1.Columns["name"].DataPropertyName = "name";
                 dataGridView1.Columns["price"].DataPropertyName = "price";
                 dataGridView1.Columns["author"].DataPropertyName = "author";


                 dataGridView1.Columns["bookID"].Visible = false;

                 btnDisplay_Click(this, EventArgs.Empty);
             }



             private void btnSave_Click(object sender, EventArgs e)
             {
                 // Check if the input fields are not empty
                 if (string.IsNullOrWhiteSpace(txtBookID.Text) ||
                    string.IsNullOrWhiteSpace(txtBookName.Text) ||
                    string.IsNullOrWhiteSpace(txtBookPrice.Text) ||
                    string.IsNullOrWhiteSpace(txtBookAuthor.Text))

                 {
                     MessageBox.Show("Please fill in all fields.");
                     return;
                 }

                 try
                 {
                     using (SqlConnection con = new SqlConnection(connectionString))
                     {
                         con.Open();
                         SqlCommand cmd = new SqlCommand("INSERT INTO Book ( name, price, author) VALUES ( @name, @price, @author)", con);

                         cmd.Parameters.AddWithValue("@bookID", txtBookID.Text);
                         cmd.Parameters.AddWithValue("@name", txtBookName.Text);
                         cmd.Parameters.AddWithValue("@price", txtBookPrice.Text);
                         cmd.Parameters.AddWithValue("@author", txtBookAuthor.Text);


                         int rowsAffected = cmd.ExecuteNonQuery();
                         if (rowsAffected > 0)
                         {
                             MessageBox.Show("Book successfully added.");
                             btnDisplay_Click(sender, e); // Refresh the display to include the new book
                         }
                         else
                         {
                             MessageBox.Show("No book was added to the database.");
                         }
                     }
                 }
                 catch (Exception ex)
                 {
                     MessageBox.Show("An error occurred: " + ex.Message);
                 }

                 // Clear the text boxes after saving
                 txtBookID.Clear();
                 txtBookName.Clear();
                 txtBookPrice.Clear();
                 txtBookAuthor.Clear();
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
                             var bookId = Convert.ToInt32(dataGridView1.CurrentRow.Cells["bookID"].Value);
                             SqlCommand cmd = new SqlCommand("UPDATE Book SET name = @name, price = @price, author = @author WHERE bookID = @bookID", con);

                             cmd.Parameters.AddWithValue("@bookID", txtBookID.Text);
                             cmd.Parameters.AddWithValue("@name", txtBookName.Text);
                             cmd.Parameters.AddWithValue("@price", txtBookPrice.Text);
                             cmd.Parameters.AddWithValue("@author", txtBookAuthor.Text);


                             int rowsAffected = cmd.ExecuteNonQuery();
                             if (rowsAffected > 0)
                             {
                                 MessageBox.Show("Book successfully updated.");
                                 btnDisplay_Click(sender, e); // Refresh the display
                             }
                             else
                             {
                                 MessageBox.Show("No book was updated. Please ensure the selected book exists.");
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
                     var result = MessageBox.Show("Are you sure you want to delete this book?", "Delete Book", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                     if (result == DialogResult.Yes)
                     {
                         try
                         {
                             using (SqlConnection con = new SqlConnection(connectionString))
                             {
                                 con.Open();
                                 var bookID = Convert.ToInt32(dataGridView1.CurrentRow.Cells["bookID"].Value);
                                 SqlCommand cmd = new SqlCommand("DELETE FROM Book WHERE bookID = @bookID", con);
                                 cmd.Parameters.AddWithValue("@bookID", bookID);
                                 int rowsAffected = cmd.ExecuteNonQuery();
                                 if (rowsAffected > 0)
                                 {
                                     MessageBox.Show("Book successfully deleted.");
                                     btnDisplay_Click(sender, e); // Refresh the display
                                 }
                                 else
                                 {
                                     MessageBox.Show("The book could not be found or was already deleted.");
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

             private void btnDisplay_Click(object sender, EventArgs e)
             {
                 try
                 {
                     using (SqlConnection con = new SqlConnection(connectionString))
                     {
                         con.Open();
                         SqlCommand cmd = new SqlCommand("SELECT bookID, name, price, author FROM Book", con);
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
             private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
             {
                 if (e.RowIndex >= 0 && !dataGridView1.Rows[e.RowIndex].IsNewRow)
                 {
                     var row = dataGridView1.Rows[e.RowIndex];
                     txtBookID.Text = row.Cells["bookID"].Value?.ToString() ?? "";
                     txtBookName.Text = row.Cells["name"].Value?.ToString() ?? "";
                     txtBookPrice.Text = row.Cells["price"].Value?.ToString() ?? "";
                     txtBookAuthor.Text = row.Cells["author"].Value?.ToString() ?? "";

                 }
             }


         }*/

    }
}



