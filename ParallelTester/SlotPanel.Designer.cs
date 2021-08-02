namespace ParallelTester
{
    partial class SlotPanel
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SlotPanel));
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.listView_Status = new System.Windows.Forms.ListView();
            this.toolStrip_Top = new System.Windows.Forms.ToolStrip();
            this.toolStripButton_Start = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton_Pause = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton_Stop = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_Config = new System.Windows.Forms.ToolStripButton();
            this.PropertyGrid1 = new ParallelTester.FilteredPropertyGrid();
            this.groupBox1.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.toolStrip_Top.SuspendLayout();
            this.SuspendLayout();
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(0, 0);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 20);
            this.comboBox1.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.groupBox1.Controls.Add(this.splitContainer1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(337, 446);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "groupBox1";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 17);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.listView_Status);
            this.splitContainer1.Panel1.Controls.Add(this.toolStrip_Top);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.PropertyGrid1);
            this.splitContainer1.Panel2Collapsed = true;
            this.splitContainer1.Size = new System.Drawing.Size(331, 426);
            this.splitContainer1.SplitterDistance = 120;
            this.splitContainer1.TabIndex = 1;
            // 
            // listView_Status
            // 
            this.listView_Status.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.listView_Status.BackColor = System.Drawing.Color.White;
            this.listView_Status.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listView_Status.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView_Status.GridLines = true;
            this.listView_Status.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.listView_Status.Location = new System.Drawing.Point(0, 0);
            this.listView_Status.Name = "listView_Status";
            this.listView_Status.ShowGroups = false;
            this.listView_Status.ShowItemToolTips = true;
            this.listView_Status.Size = new System.Drawing.Size(331, 387);
            this.listView_Status.TabIndex = 1;
            this.listView_Status.UseCompatibleStateImageBehavior = false;
            this.listView_Status.View = System.Windows.Forms.View.Details;
            // 
            // toolStrip_Top
            // 
            this.toolStrip_Top.BackColor = System.Drawing.Color.LightBlue;
            this.toolStrip_Top.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStrip_Top.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip_Top.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.toolStrip_Top.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton_Start,
            this.toolStripSeparator1,
            this.toolStripButton_Pause,
            this.toolStripSeparator2,
            this.toolStripButton_Stop,
            this.toolStripButton_Config});
            this.toolStrip_Top.Location = new System.Drawing.Point(0, 387);
            this.toolStrip_Top.Name = "toolStrip_Top";
            this.toolStrip_Top.Size = new System.Drawing.Size(331, 39);
            this.toolStrip_Top.Stretch = true;
            this.toolStrip_Top.TabIndex = 0;
            this.toolStrip_Top.Text = "toolStrip1";
            this.toolStrip_Top.Click += new System.EventHandler(this.toolStrip_Top_Click);
            // 
            // toolStripButton_Start
            // 
            this.toolStripButton_Start.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_Start.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_Start.Image")));
            this.toolStripButton_Start.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_Start.Name = "toolStripButton_Start";
            this.toolStripButton_Start.Size = new System.Drawing.Size(36, 36);
            this.toolStripButton_Start.Text = "Start";
            this.toolStripButton_Start.Click += new System.EventHandler(this.toolStripButton_Start_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 39);
            // 
            // toolStripButton_Pause
            // 
            this.toolStripButton_Pause.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_Pause.Enabled = false;
            this.toolStripButton_Pause.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_Pause.Image")));
            this.toolStripButton_Pause.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_Pause.Name = "toolStripButton_Pause";
            this.toolStripButton_Pause.Size = new System.Drawing.Size(36, 36);
            this.toolStripButton_Pause.Text = "Pause";
            this.toolStripButton_Pause.Click += new System.EventHandler(this.toolStripButton_Pause_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 39);
            // 
            // toolStripButton_Stop
            // 
            this.toolStripButton_Stop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_Stop.Enabled = false;
            this.toolStripButton_Stop.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_Stop.Image")));
            this.toolStripButton_Stop.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_Stop.Name = "toolStripButton_Stop";
            this.toolStripButton_Stop.Size = new System.Drawing.Size(36, 36);
            this.toolStripButton_Stop.Text = "Stop";
            this.toolStripButton_Stop.Click += new System.EventHandler(this.toolStripButton_Stop_Click);
            // 
            // toolStripButton_Config
            // 
            this.toolStripButton_Config.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripButton_Config.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_Config.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_Config.Image")));
            this.toolStripButton_Config.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_Config.Name = "toolStripButton_Config";
            this.toolStripButton_Config.Size = new System.Drawing.Size(36, 36);
            this.toolStripButton_Config.Text = "Config";
            this.toolStripButton_Config.Click += new System.EventHandler(this.toolStripButton_Config_Click);
            // 
            // PropertyGrid1
            // 
            this.PropertyGrid1.BackColor = System.Drawing.Color.White;
            this.PropertyGrid1.BrowsableProperties = null;
            this.PropertyGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PropertyGrid1.HiddenAttributes = null;
            this.PropertyGrid1.HiddenProperties = null;
            this.PropertyGrid1.Location = new System.Drawing.Point(0, 0);
            this.PropertyGrid1.Name = "PropertyGrid1";
            this.PropertyGrid1.PropertySort = System.Windows.Forms.PropertySort.NoSort;
            this.PropertyGrid1.Size = new System.Drawing.Size(150, 46);
            this.PropertyGrid1.TabIndex = 0;
            this.PropertyGrid1.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.propertyGrid1_PropertyValueChanged);
            // 
            // SlotPanel
            // 
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.Transparent;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.Controls.Add(this.groupBox1);
            this.Name = "SlotPanel";
            this.Size = new System.Drawing.Size(337, 446);
            this.Load += new System.EventHandler(this.SlotPanel_Load);
            this.Enter += new System.EventHandler(this.SlotPanel_Enter);
            this.groupBox1.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.toolStrip_Top.ResumeLayout(false);
            this.toolStrip_Top.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ToolStrip toolStrip_Top;
        private System.Windows.Forms.ToolStripButton toolStripButton_Start;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolStripButton_Pause;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton toolStripButton_Stop;
        private System.Windows.Forms.ListView listView_Status;
        private FilteredPropertyGrid PropertyGrid1;
        private System.Windows.Forms.ToolStripButton toolStripButton_Config;
    }
}
