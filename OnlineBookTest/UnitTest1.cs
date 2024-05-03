using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace OnlineBook.Tests
{
    [TestClass]
    public class OnlineBookTests

    {

        [TestMethod]
        public void Test_AuthorButtonClick_OpenAuthorForm()
        {
            //Arrange
            OnlineBook onlineBookForm = new OnlineBook();   
            

            //Act
            onlineBookForm.btnAuthor_Click(null!, null!);

            //Assert
          
            Assert.IsTrue(IsFormOpened<Author>(), "Author form should be opened.");  


        }
        private bool IsFormOpened<T>() where T : Author
        {
            foreach(Author openedAuthor in Application.OpenAuthors)
            { 
                if (openedAuthor is T)
                {
                    return true;
                }
            }
            return false;
        }

       
    }
    public class OnlineBook : Form 
    { 
        public void btnAuthor_Click(object sender, EventArgs e) 
        {
            Author authorForm = new Author();
            authorForm.Show();
        }
    }
   
    
}