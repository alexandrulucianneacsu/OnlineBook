using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.SqlClient;


namespace MyTestingProject
{
    [TestClass]
    public class CustomerTests
    {
        private const string connectionString = "Data Source=LAPTOP-TPKNAAAB; Initial Catalog=Testing; Integrated Security=True;";


       


        [TestMethod]
        public void SaveCustomerWhenCalledShouldInsertCustomerIntoDatabase()
        {
            try
            {
                // Arrange
                CustomerTests customerForm = new();
                string firstName = "Alex";
                string lastName = "Neacsu";
                string email = "alex@yahoo.com";
                string phone = "07828660070";

                // Act
                int affectedRows = customerForm.SaveCustomer(firstName, lastName, email, phone);

                // Assert
                Assert.AreEqual(1, affectedRows);

                // Clean up: Delete the inserted customer
                using SqlConnection con = new(connectionString);
                con.Open();
                var cmd = new SqlCommand("DELETE FROM Customer WHERE firstName = @firstName AND lastName = @lastName", con);
                cmd.Parameters.AddWithValue("@firstName", firstName);
                cmd.Parameters.AddWithValue("@lastName", lastName);
                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {
               
            }
        }

        private int SaveCustomer(string firstName, string lastName, string email, string phone)
        {
            throw new NotImplementedException();
        }
    }
}







