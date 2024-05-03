using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace OnlineBook
{
    public partial class OnlineBook : Form
    {
        public OnlineBook() => InitializeComponent();

        private void btnCustomer_Click(object sender, EventArgs e)
        {
            Customer customer = new Customer();

            customer.Show();
        }

        private void btnBook_Click(object sender, EventArgs e)
        {
            Book book = new Book();

            book.Show();
        }

        private void btnOrderDetail_Click(object sender, EventArgs e)
        {
            Order_Detail orderDetail = new Order_Detail();

            orderDetail.Show();
        }

        private void btnAuthor_Click(object sender, EventArgs e)
        {
            Author author = new Author();

            author.Show();
        }

        private void btnPublisher_Click(object sender, EventArgs e)
        {
            Publisher publisher = new Publisher();

            publisher.Show();
        }

        private void btnReview_Click(object sender, EventArgs e)
        {
            Review review = new Review();

            review.Show();
        }
    }
}
