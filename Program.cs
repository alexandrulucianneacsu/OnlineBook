using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Testing

   // delegate
{
    public delegate void NextActionDelegate();

    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
           // Application.Run(new OnlineBook());

            OnlineBook onlineBook = new OnlineBook();   

            void NextAction()
            {
                onlineBook.PerformNextAction();
            }
            NextActionDelegate nextActionDelegate = new NextActionDelegate(NextAction);

            onlineBook.SetNextActionDelegate(nextActionDelegate);   

            Application.Run(onlineBook);
        }
    }
}
