using Oracle.ManagedDataAccess.Client;
using System;
using System.Configuration;
using System.IO;
using System.Windows.Forms;

namespace B2Check
{
    public partial class Login : Form
    {
        Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        public string decKey;
        public string l;
        public string p;

        public Login()
        {
            InitializeComponent();
            tbLogin.Text = string.IsNullOrEmpty(ConfigurationManager.AppSettings["Login"]) ? "" : ConfigurationManager.AppSettings["Login"];
            if (tbLogin.Text.Length == 0)
                ActiveControl = tbLogin;
            else ActiveControl = tbPass;
        }

        private void btEnter_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.None;
            try
            {
                //OracleConnection bkConn = Utils.GetConnection("BkConn", tbLogin.Text.ToUpper(), tbPass.Text);
                //OracleCommand cmd = new OracleCommand("", bkConn);
                //string subSql = "select count(*) from bank974.ISEEKJU where upper(login) = upper(':Login')";
                try
                {
                    btEnter.Enabled = false;
                    //bkConn.Open();

                    //subSql = subSql.Replace(":Login", tbLogin.Text);
                    //cmd.CommandText = subSql;
                    //int n = Convert.ToInt32(cmd.ExecuteScalar());
                    if (Utils.HasAccess(tbLogin.Text.ToUpper(), tbPass.Text))
                    {
                        config.AppSettings.Settings.Remove("Login");
                        config.AppSettings.Settings.Add("Login", tbLogin.Text);
                        config.Save(ConfigurationSaveMode.Full);
                        l = tbLogin.Text;
                        p = tbPass.Text;
                        DialogResult = DialogResult.OK;
                    }
                    else
                    {
                        MessageBox.Show("Не разрешён вход с Вашего логина.");
                        DialogResult = DialogResult.Cancel;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                   // bkConn.Close();
                    btEnter.Enabled = true;
                    tbPass.Text = "";
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        private void tbLogin_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.KeyChar = char.ToUpper(e.KeyChar);
        }

        private void Login_Load(object sender, EventArgs e)
        {
            ActiveControl = tbLogin;
        }

        private void Login_Shown(object sender, EventArgs e)
        {
            if (tbLogin.Text.Length > 0)
                ActiveControl = tbPass;
        }
    }
}
