using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Reflection.Metadata;

namespace OnlineBook
{
    [TestClass]
    public class Test1
    {
        private const string ConnectionString = "Data Source=LAPTOP-TPKNAAAB; Initial Catalog=OnlineBook; Integrated Security=True;";

        [TestMethod]
        public void PopulateDataGridView_WhenCalled_ShouldPopulateWithData()
        {
            //Arrange
            Test1 customerForm = new Test1();
            DataGridView dataGridView = new DataGridView();

            //Act
            customerForm.PopulateDataGridView(dataGridView);

            //Assert
            Assert.IsNotNull(dataGridView.DataSource);
            Assert.IsTrue(dataGridView.Rows.Count > 0);

        }


        }

       
    }
