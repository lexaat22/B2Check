
namespace B2Check
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            PresentationControls.CheckBoxProperties checkBoxProperties2 = new PresentationControls.CheckBoxProperties();
            this.groupBoxFilter = new System.Windows.Forms.GroupBox();
            this.cbPhys = new System.Windows.Forms.CheckBox();
            this.tbFio = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btShow = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.sourceBox = new PresentationControls.CheckBoxComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.percentUpDown = new System.Windows.Forms.NumericUpDown();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.менюToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.miRunProcess = new System.Windows.Forms.ToolStripMenuItem();
            this.экспортToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.miExcel = new System.Windows.Forms.ToolStripMenuItem();
            this.miTXT = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.miExit = new System.Windows.Forms.ToolStripMenuItem();
            this.gridResult = new System.Windows.Forms.DataGridView();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel4 = new System.Windows.Forms.ToolStripStatusLabel();
            this.AboutMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBoxFilter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.percentUpDown)).BeginInit();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridResult)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxFilter
            // 
            this.groupBoxFilter.Controls.Add(this.cbPhys);
            this.groupBoxFilter.Controls.Add(this.tbFio);
            this.groupBoxFilter.Controls.Add(this.label4);
            this.groupBoxFilter.Controls.Add(this.btShow);
            this.groupBoxFilter.Controls.Add(this.label3);
            this.groupBoxFilter.Controls.Add(this.sourceBox);
            this.groupBoxFilter.Controls.Add(this.label2);
            this.groupBoxFilter.Controls.Add(this.label1);
            this.groupBoxFilter.Controls.Add(this.percentUpDown);
            this.groupBoxFilter.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxFilter.Location = new System.Drawing.Point(0, 33);
            this.groupBoxFilter.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBoxFilter.Name = "groupBoxFilter";
            this.groupBoxFilter.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBoxFilter.Size = new System.Drawing.Size(1512, 98);
            this.groupBoxFilter.TabIndex = 0;
            this.groupBoxFilter.TabStop = false;
            this.groupBoxFilter.Text = "Фильтры:";
            // 
            // cbPhys
            // 
            this.cbPhys.AutoSize = true;
            this.cbPhys.Checked = true;
            this.cbPhys.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbPhys.Enabled = false;
            this.cbPhys.Location = new System.Drawing.Point(469, 58);
            this.cbPhys.Name = "cbPhys";
            this.cbPhys.Size = new System.Drawing.Size(93, 24);
            this.cbPhys.TabIndex = 8;
            this.cbPhys.Text = "Физики";
            this.cbPhys.UseVisualStyleBackColor = true;
            // 
            // tbFio
            // 
            this.tbFio.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.tbFio.Location = new System.Drawing.Point(190, 57);
            this.tbFio.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tbFio.Name = "tbFio";
            this.tbFio.Size = new System.Drawing.Size(271, 26);
            this.tbFio.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.Location = new System.Drawing.Point(186, 34);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(275, 20);
            this.label4.TabIndex = 6;
            this.label4.Text = "По уникалке/ФИО/Название:";
            // 
            // btShow
            // 
            this.btShow.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btShow.BackColor = System.Drawing.SystemColors.HotTrack;
            this.btShow.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btShow.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btShow.Location = new System.Drawing.Point(1330, 34);
            this.btShow.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btShow.Name = "btShow";
            this.btShow.Size = new System.Drawing.Size(172, 58);
            this.btShow.TabIndex = 5;
            this.btShow.Text = "ПОКАЗАТЬ";
            this.btShow.UseVisualStyleBackColor = false;
            this.btShow.Click += new System.EventHandler(this.btShow_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(96, 62);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 20);
            this.label3.TabIndex = 4;
            this.label3.Text = "%%";
            // 
            // sourceBox
            // 
            this.sourceBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            checkBoxProperties2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.sourceBox.CheckBoxProperties = checkBoxProperties2;
            this.sourceBox.DisplayMemberSingleItem = "";
            this.sourceBox.FormattingEnabled = true;
            this.sourceBox.Location = new System.Drawing.Point(609, 57);
            this.sourceBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.sourceBox.MaxDropDownItems = 12;
            this.sourceBox.Name = "sourceBox";
            this.sourceBox.Size = new System.Drawing.Size(710, 28);
            this.sourceBox.TabIndex = 3;
            this.sourceBox.CheckBoxCheckedChanged += new System.EventHandler(this.sourceBox_CheckBoxCheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(605, 34);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(81, 20);
            this.label2.TabIndex = 2;
            this.label2.Text = "Списки:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(18, 34);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(104, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "Схожесть:";
            // 
            // percentUpDown
            // 
            this.percentUpDown.Location = new System.Drawing.Point(22, 58);
            this.percentUpDown.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.percentUpDown.Minimum = new decimal(new int[] {
            80,
            0,
            0,
            0});
            this.percentUpDown.Name = "percentUpDown";
            this.percentUpDown.Size = new System.Drawing.Size(64, 26);
            this.percentUpDown.TabIndex = 0;
            this.percentUpDown.Value = new decimal(new int[] {
            98,
            0,
            0,
            0});
            // 
            // menuStrip1
            // 
            this.menuStrip1.GripMargin = new System.Windows.Forms.Padding(2, 2, 0, 2);
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.менюToolStripMenuItem,
            this.AboutMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1512, 33);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // менюToolStripMenuItem
            // 
            this.менюToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miRunProcess,
            this.экспортToolStripMenuItem,
            this.toolStripMenuItem1,
            this.miExit});
            this.менюToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.менюToolStripMenuItem.Name = "менюToolStripMenuItem";
            this.менюToolStripMenuItem.Size = new System.Drawing.Size(82, 30);
            this.менюToolStripMenuItem.Text = "Меню";
            // 
            // miRunProcess
            // 
            this.miRunProcess.Name = "miRunProcess";
            this.miRunProcess.Size = new System.Drawing.Size(345, 34);
            this.miRunProcess.Text = "Запустить процесс сверки";
            this.miRunProcess.Click += new System.EventHandler(this.miRunProcess_Click);
            // 
            // экспортToolStripMenuItem
            // 
            this.экспортToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miExcel,
            this.miTXT});
            this.экспортToolStripMenuItem.Name = "экспортToolStripMenuItem";
            this.экспортToolStripMenuItem.Size = new System.Drawing.Size(345, 34);
            this.экспортToolStripMenuItem.Text = "Экспорт";
            // 
            // miExcel
            // 
            this.miExcel.Name = "miExcel";
            this.miExcel.Size = new System.Drawing.Size(158, 34);
            this.miExcel.Text = "Excel";
            // 
            // miTXT
            // 
            this.miTXT.Name = "miTXT";
            this.miTXT.Size = new System.Drawing.Size(158, 34);
            this.miTXT.Text = "TXT";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(342, 6);
            // 
            // miExit
            // 
            this.miExit.Name = "miExit";
            this.miExit.Size = new System.Drawing.Size(345, 34);
            this.miExit.Text = "Выход";
            this.miExit.Click += new System.EventHandler(this.miExit_Click);
            // 
            // gridResult
            // 
            this.gridResult.AllowUserToAddRows = false;
            this.gridResult.AllowUserToDeleteRows = false;
            this.gridResult.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridResult.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridResult.Location = new System.Drawing.Point(0, 134);
            this.gridResult.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.gridResult.Name = "gridResult";
            this.gridResult.ReadOnly = true;
            this.gridResult.RowHeadersWidth = 62;
            this.gridResult.Size = new System.Drawing.Size(1512, 954);
            this.gridResult.TabIndex = 2;
            this.gridResult.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.gridResult_CellMouseClick);
            this.gridResult.CellMouseUp += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.gridResult_CellMouseUp);
            this.gridResult.Sorted += new System.EventHandler(this.gridResult_Sorted);
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel2,
            this.toolStripStatusLabel3,
            this.toolStripStatusLabel4});
            this.statusStrip1.Location = new System.Drawing.Point(0, 1090);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(2, 0, 21, 0);
            this.statusStrip1.Size = new System.Drawing.Size(1512, 32);
            this.statusStrip1.TabIndex = 3;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.BackColor = System.Drawing.Color.LightBlue;
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(179, 25);
            this.toolStripStatusLabel1.Text = "Ложное совпадение";
            this.toolStripStatusLabel1.Click += new System.EventHandler(this.toolStripStatusLabel1_Click);
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.BackColor = System.Drawing.Color.Orange;
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(176, 25);
            this.toolStripStatusLabel2.Text = "Полное совпадение";
            this.toolStripStatusLabel2.Click += new System.EventHandler(this.toolStripStatusLabel2_Click);
            // 
            // toolStripStatusLabel3
            // 
            this.toolStripStatusLabel3.BackColor = System.Drawing.Color.LightGreen;
            this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            this.toolStripStatusLabel3.Size = new System.Drawing.Size(128, 25);
            this.toolStripStatusLabel3.Text = "До выяснения";
            this.toolStripStatusLabel3.Click += new System.EventHandler(this.toolStripStatusLabel3_Click);
            // 
            // toolStripStatusLabel4
            // 
            this.toolStripStatusLabel4.BackColor = System.Drawing.Color.White;
            this.toolStripStatusLabel4.Name = "toolStripStatusLabel4";
            this.toolStripStatusLabel4.Size = new System.Drawing.Size(120, 25);
            this.toolStripStatusLabel4.Text = "Не проверен";
            this.toolStripStatusLabel4.Click += new System.EventHandler(this.toolStripStatusLabel4_Click);
            // 
            // AboutMenuItem
            // 
            this.AboutMenuItem.Name = "AboutMenuItem";
            this.AboutMenuItem.Size = new System.Drawing.Size(141, 29);
            this.AboutMenuItem.Text = "О программе";
            this.AboutMenuItem.Click += new System.EventHandler(this.AboutMenuItem_Click);
            // 
            // MainForm
            // 
            this.AcceptButton = this.btShow;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1512, 1122);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.gridResult);
            this.Controls.Add(this.groupBoxFilter);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MinimumSize = new System.Drawing.Size(1525, 1151);
            this.Name = "MainForm";
            this.Text = "B2Check";
            this.groupBoxFilter.ResumeLayout(false);
            this.groupBoxFilter.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.percentUpDown)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridResult)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxFilter;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown percentUpDown;
        private PresentationControls.CheckBoxComboBox sourceBox;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem менюToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem miRunProcess;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem miExit;
        private System.Windows.Forms.DataGridView gridResult;
        private System.Windows.Forms.TextBox tbFio;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btShow;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ToolStripMenuItem экспортToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem miExcel;
        private System.Windows.Forms.ToolStripMenuItem miTXT;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.CheckBox cbPhys;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel3;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel4;
        private System.Windows.Forms.ToolStripMenuItem AboutMenuItem;
    }
}

