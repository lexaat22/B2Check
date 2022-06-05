using System;
using System.Windows.Forms;

namespace B2Check
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Login fLogin = new Login();
            try
            {
                if (fLogin.ShowDialog() == DialogResult.OK)
                {
                    Application.Run(new MainForm(fLogin.l, fLogin.p));

                }
                else
                {
                    Application.Exit();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
