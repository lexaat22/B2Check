using B2Check.model;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace B2Check
{
    public partial class MainForm : Form
    {
        Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        static string log, pass;
        List<BLSource> list = null;

        public MainForm(string l, string p)
        {
            InitializeComponent();
            log = l;
            pass = p;

            try
            {
                percentUpDown.Text = string.IsNullOrEmpty(ConfigurationManager.AppSettings["procent"]) ? "95" : ConfigurationManager.AppSettings["procent"];
            }
            catch { }

            System.Windows.Forms.Timer MyTimer = new System.Windows.Forms.Timer();
            MyTimer.Interval = 2000;
            MyTimer.Tick += new EventHandler(Count);
            MyTimer.Start();

            list = Utils.GetBLSources(log, pass);
            PopulateSourceBox();
            setCheckBoxes(string.IsNullOrEmpty(ConfigurationManager.AppSettings["lists"]) ? "1" : ConfigurationManager.AppSettings["lists"]);

        }

        private void Count(object sender, EventArgs e)
        {
            Text = string.Format($"B2Check (процессов: {Utils.GetProcCount(log, pass)}, прогресс: {Utils.GetProgress(log, pass) * 100:0.000} %)");
        }

        private void PopulateSourceBox()
        {
            foreach (var i in list)
            {
                sourceBox.Items.Add(i.Name);
            }
        }

        private void sourceBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            getListIds();
        }

        private void sourceBox_CheckBoxCheckedChanged(object sender, EventArgs e)
        {
            getListIds();
        }

        private void miExit_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Закрыть программу?", "Выход", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                Close();
            }
        }

        private void miRunProcess_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Запустить процесс сверки контрагентов со списками?", "Процесс", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                new Task(delegate { Utils.StartSearch(log, pass); }).Start();
            }
        }

        private string getListIds()
        {
            string ids = "";
            for (int i = 0; i < sourceBox.CheckBoxItems.Count; i++)
            {
                if (sourceBox.CheckBoxItems[i].Checked)
                    ids = string.Format($"{ids},{list[i].Id}");
            }
            if (ids.Length > 1) ids = ids.Substring(1, ids.Length - 1);
            return ids;
        }

        private void setCheckBoxes(string ids)
        {
            for (int i = 0; i < sourceBox.CheckBoxItems.Count; i++)
            {
                sourceBox.CheckBoxItems[i].Checked = false;
            }
            if (list == null) list = new List<BLSource>();
            string[] idsArray = ids.Split(',');
            for (int i = 0; i<idsArray.Length; i++)
            {
                foreach(BLSource item in list)
                {
                    if (item.Id.Equals(idsArray[i]))
                        sourceBox.CheckBoxItems[item.Name].Checked = true;
                }
            }
        }

        private void btShow_Click(object sender, EventArgs e)
        {
            config.AppSettings.Settings.Remove("procent");
            config.AppSettings.Settings.Add("procent", percentUpDown.Text);
            config.AppSettings.Settings.Remove("lists");
            config.AppSettings.Settings.Add("lists", getListIds());
            config.Save(ConfigurationSaveMode.Full);

            BindGrid(Convert.ToInt32(percentUpDown.Value), getListIds(), tbFio.Text);
        }

        private async void BindGrid(int percent, string lists, string name)
        {
            try
            {
                gridResult.DataSource = await Utils.GetResultTable(log, pass, percent, lists, name);

                gridResult.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                gridResult.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                gridResult.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                gridResult.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                gridResult.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                gridResult.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                gridResult.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                gridResult.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
