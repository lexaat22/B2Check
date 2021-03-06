using B2Check.model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace B2Check
{
    public partial class MainForm : Form
    {
        Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        static string log, pass;
        List<BLSource> list = null;
        bool filterOn = false;

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

            Timer MyTimer = new Timer();
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
            for (int i = 0; i < idsArray.Length; i++)
            {
                foreach (BLSource item in list)
                {
                    if (item.Id.Equals(idsArray[i]))
                        sourceBox.CheckBoxItems[item.Name].Checked = true;
                }
            }
        }

        private void btShow_Click(object sender, EventArgs e)
        {
            try
            {
                int index = 0;
                try
                {
                    index = gridResult.CurrentRow.Index;
                }
                catch { }
                btShow.Enabled = false;
                config.AppSettings.Settings.Remove("procent");
                config.AppSettings.Settings.Add("procent", percentUpDown.Text);
                config.AppSettings.Settings.Remove("lists");
                config.AppSettings.Settings.Add("lists", getListIds());
                config.Save(ConfigurationSaveMode.Full);
                BindGrid(Convert.ToInt32(percentUpDown.Value), getListIds(), tbFio.Text);

                try { gridResult.FirstDisplayedScrollingRowIndex = index; } catch { }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                btShow.Enabled = true;
            }
        }

        private void gridResult_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void gridResult_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == gridResult.Columns["checked"].Index && e.RowIndex != -1)
            {
                if (gridResult.CurrentCell == null) return;
                gridResult.CurrentCell.Value = !(bool)gridResult.CurrentCell.Value;
                MessageBox.Show(gridResult.CurrentCell.Value.ToString());
            }
        }

        private void gridResult_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            var ch = 0;
            if ((ModifierKeys & Keys.Alt) != 0)
                ch = 1;
            if ((ModifierKeys & Keys.Control) != 0)
                ch = 2;
            if ((ModifierKeys & Keys.Shift) != 0)
                ch = 3;

            if (ch > 0 && e.RowIndex != -1)
            {
                if (gridResult.CurrentCell == null) return;
                var siteid = Convert.ToInt32(gridResult.CurrentRow.Cells["siteid"].Value);
                var id = Convert.ToInt32(gridResult.CurrentRow.Cells["id"].Value);
                var list_id = Convert.ToInt32(gridResult.CurrentRow.Cells["list_id"].Value);
                var check = Convert.ToInt32(gridResult.CurrentRow.Cells["checked"].Value);
                check = check > 0 ? 0 : ch;
                SetChecked(log, pass, siteid, id, list_id, check);
                gridResult.CurrentRow.Cells["checked"].Value = check;
                DrawRow(gridResult.CurrentRow);
            }
        }

        private async void BindGrid(int percent, string lists, string name)
        {
            try
            {
                gridResult.DataSource = await Utils.GetResultTable(log, pass, percent, lists, name);

                gridResult.Columns["checked"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                if (gridResult.Columns["checked"] is DataGridViewCheckBoxColumn)
                {
                    (gridResult.Columns["checked"] as DataGridViewCheckBoxColumn).HeaderText = "Галочка";
                    //(gridResult.Columns["checked"] as DataGridViewCheckBoxColumn).FalseValue = false;
                    //(gridResult.Columns["checked"] as DataGridViewCheckBoxColumn).TrueValue = true;
                }
                gridResult.Columns["checked"].Visible = false;

                gridResult.Columns["siteid"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                gridResult.Columns["siteid"].HeaderText = "МФО";

                gridResult.Columns["id"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                gridResult.Columns["id"].HeaderText = "Уникалка";

                gridResult.Columns["name_b2"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                gridResult.Columns["name_b2"].HeaderText = "ФИО Б2";

                gridResult.Columns["name_list"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                gridResult.Columns["name_list"].HeaderText = "ФИО в списке";

                gridResult.Columns["list_id"].Visible = false;

                //gridResult.Columns["entity_id"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                gridResult.Columns["entity_id"].HeaderText = "ID в списке";
                gridResult.Columns["similar"].Width = 150;

                gridResult.Columns["sname"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                gridResult.Columns["sname"].HeaderText = "Список";

                //gridResult.Columns["similar"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                gridResult.Columns["similar"].HeaderText = "% Совпадения";
                gridResult.Columns["similar"].Width = 150;

                gridResult.Columns["stime"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                gridResult.Columns["stime"].HeaderText = "Дата и время";

                //gridResult.Columns["checked_by"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                gridResult.Columns["checked_by"].HeaderText = "Кем выделено";
                gridResult.Columns["similar"].Width = 150;

                var sortedCol = string.IsNullOrEmpty(ConfigurationManager.AppSettings["sortedCol"]) ? "stime" : ConfigurationManager.AppSettings["sortedCol"];
                gridResult.Sort(gridResult.Columns[sortedCol], string.IsNullOrEmpty(ConfigurationManager.AppSettings["sortOrder"]) ?
                    ListSortDirection.Ascending : ConfigurationManager.AppSettings["sortOrder"].Equals("Ascending") ?
                    ListSortDirection.Ascending : ListSortDirection.Descending);

                DrawSelectedRows();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void gridResult_Sorted(object sender, EventArgs e)
        {
            DrawSelectedRows();
            var sortedCol = gridResult.SortedColumn.DataPropertyName;
            var sortOrder = gridResult.SortOrder;

            config.AppSettings.Settings.Remove("sortedCol");
            config.AppSettings.Settings.Add("sortedCol", sortedCol);
            config.AppSettings.Settings.Remove("sortOrder");
            config.AppSettings.Settings.Add("sortOrder", sortOrder.ToString());
            config.Save(ConfigurationSaveMode.Full);
        }

        private void DrawSelectedRows()
        {
            foreach (DataGridViewRow row in gridResult.Rows)
            {
                DrawRow(row);
            }
        }

        private void DrawRow(DataGridViewRow row)
        {
            if (row.Cells["checked"].Value != null && Convert.ToInt32(row.Cells["checked"].Value) == 1)
            {
                row.DefaultCellStyle.BackColor = Color.LightBlue;
            }
            else if (row.Cells["checked"].Value != null && Convert.ToInt32(row.Cells["checked"].Value) == 2)
            {
                row.DefaultCellStyle.BackColor = Color.Orange;
            }
            else if (row.Cells["checked"].Value != null && Convert.ToInt32(row.Cells["checked"].Value) == 3)
            {
                row.DefaultCellStyle.BackColor = Color.LightGreen;
            }
            else
                row.DefaultCellStyle.BackColor = Color.White;
        }

        private void FilterRows(string columnName, string filterValue)
        {
            string rowFilter;
            if (filterOn)
                rowFilter = string.Format("[{0}] = {1}", columnName, filterValue);
            else rowFilter = "";
            (gridResult.DataSource as DataTable).DefaultView.RowFilter = rowFilter;
            filterOn = !filterOn;
        }
        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {
            FilterRows("checked", "1");
            DrawSelectedRows();
        }

        private void toolStripStatusLabel2_Click(object sender, EventArgs e)
        {
            FilterRows("checked", "2");
            DrawSelectedRows();
        }

        private void toolStripStatusLabel3_Click(object sender, EventArgs e)
        {
            FilterRows("checked", "3");
            DrawSelectedRows();
        }

        private void toolStripStatusLabel4_Click(object sender, EventArgs e)
        {
            FilterRows("checked", "0");
            DrawSelectedRows();
        }

        private void AboutMenuItem_Click(object sender, EventArgs e)
        {
            string version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            MessageBox.Show(string.Format($"Версия программы: {version}"));
        }

        private async void SetChecked(string log, string pass, int site, int id, int list_id, int check)
        {
            try
            {
                await Utils.SetChecked(log, pass, site, id, list_id, check);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
