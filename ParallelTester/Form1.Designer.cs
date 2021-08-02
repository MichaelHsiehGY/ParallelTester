namespace ParallelTester
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.toolStrip_Top = new System.Windows.Forms.ToolStrip();
            this.toolStripButton_Recall = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton_Add = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton_Remove = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.SN = new System.Windows.Forms.ToolStripLabel();
            this.toolStripTextBox_SN = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripLabel_Status = new System.Windows.Forms.ToolStripLabel();
            this.contextMenuStrip_SN = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem_Forbid = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_UpperCase = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_LowerCase = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_NormalCase = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.toolStrip_Top.SuspendLayout();
            this.contextMenuStrip_SN.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoScroll = true;
            this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.AddColumns;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(994, 406);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.toolStrip_Top);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tableLayoutPanel1);
            this.splitContainer1.Size = new System.Drawing.Size(994, 443);
            this.splitContainer1.SplitterDistance = 36;
            this.splitContainer1.SplitterWidth = 1;
            this.splitContainer1.TabIndex = 0;
            // 
            // toolStrip_Top
            // 
            this.toolStrip_Top.BackColor = System.Drawing.Color.LightSkyBlue;
            this.toolStrip_Top.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStrip_Top.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip_Top.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.toolStrip_Top.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton_Recall,
            this.toolStripSeparator1,
            this.toolStripButton_Add,
            this.toolStripSeparator2,
            this.toolStripButton_Remove,
            this.toolStripSeparator3,
            this.SN,
            this.toolStripTextBox_SN,
            this.toolStripLabel_Status});
            this.toolStrip_Top.Location = new System.Drawing.Point(0, 0);
            this.toolStrip_Top.Name = "toolStrip_Top";
            this.toolStrip_Top.Size = new System.Drawing.Size(994, 36);
            this.toolStrip_Top.TabIndex = 6;
            this.toolStrip_Top.Text = "toolStrip1";
            // 
            // toolStripButton_Recall
            // 
            this.toolStripButton_Recall.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_Recall.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_Recall.Image")));
            this.toolStripButton_Recall.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_Recall.Name = "toolStripButton_Recall";
            this.toolStripButton_Recall.Size = new System.Drawing.Size(36, 33);
            this.toolStripButton_Recall.Text = "Recall Setup";
            this.toolStripButton_Recall.Click += new System.EventHandler(this.toolStripButton_Recall_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 36);
            // 
            // toolStripButton_Add
            // 
            this.toolStripButton_Add.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_Add.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_Add.Image")));
            this.toolStripButton_Add.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_Add.Name = "toolStripButton_Add";
            this.toolStripButton_Add.Size = new System.Drawing.Size(36, 33);
            this.toolStripButton_Add.Text = "Add Channel";
            this.toolStripButton_Add.Click += new System.EventHandler(this.toolStripButton_Add_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 36);
            // 
            // toolStripButton_Remove
            // 
            this.toolStripButton_Remove.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_Remove.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_Remove.Image")));
            this.toolStripButton_Remove.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_Remove.Name = "toolStripButton_Remove";
            this.toolStripButton_Remove.Size = new System.Drawing.Size(36, 33);
            this.toolStripButton_Remove.Text = "Remove Channel";
            this.toolStripButton_Remove.Click += new System.EventHandler(this.toolStripButton_Remove_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 36);
            // 
            // SN
            // 
            this.SN.Name = "SN";
            this.SN.Size = new System.Drawing.Size(28, 33);
            this.SN.Text = "SN:";
            // 
            // toolStripTextBox_SN
            // 
            this.toolStripTextBox_SN.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.toolStripTextBox_SN.Name = "toolStripTextBox_SN";
            this.toolStripTextBox_SN.Size = new System.Drawing.Size(160, 36);
            this.toolStripTextBox_SN.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.toolStripTextBox_SN_KeyPress);
            this.toolStripTextBox_SN.MouseDown += new System.Windows.Forms.MouseEventHandler(this.toolStripTextBox_SN_MouseDown);
            // 
            // toolStripLabel_Status
            // 
            this.toolStripLabel_Status.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripLabel_Status.Name = "toolStripLabel_Status";
            this.toolStripLabel_Status.Size = new System.Drawing.Size(0, 33);
            // 
            // contextMenuStrip_SN
            // 
            this.contextMenuStrip_SN.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem_Forbid,
            this.toolStripMenuItem_UpperCase,
            this.toolStripMenuItem_LowerCase,
            this.toolStripMenuItem_NormalCase});
            this.contextMenuStrip_SN.Name = "contextMenuStrip_SN";
            this.contextMenuStrip_SN.Size = new System.Drawing.Size(213, 92);
            // 
            // toolStripMenuItem_Forbid
            // 
            this.toolStripMenuItem_Forbid.Name = "toolStripMenuItem_Forbid";
            this.toolStripMenuItem_Forbid.Size = new System.Drawing.Size(212, 22);
            this.toolStripMenuItem_Forbid.Text = "Forbid Manual Input";
            this.toolStripMenuItem_Forbid.Click += new System.EventHandler(this.toolStripMenuItem_Forbid_Click);
            // 
            // toolStripMenuItem_UpperCase
            // 
            this.toolStripMenuItem_UpperCase.Name = "toolStripMenuItem_UpperCase";
            this.toolStripMenuItem_UpperCase.Size = new System.Drawing.Size(212, 22);
            this.toolStripMenuItem_UpperCase.Text = "Only Upper Characters";
            this.toolStripMenuItem_UpperCase.Click += new System.EventHandler(this.toolStripMenuItem_UpperCase_Click);
            // 
            // toolStripMenuItem_LowerCase
            // 
            this.toolStripMenuItem_LowerCase.Name = "toolStripMenuItem_LowerCase";
            this.toolStripMenuItem_LowerCase.Size = new System.Drawing.Size(212, 22);
            this.toolStripMenuItem_LowerCase.Text = "Only Lower Characters";
            this.toolStripMenuItem_LowerCase.Click += new System.EventHandler(this.toolStripMenuItem_LowerCase_Click);
            // 
            // toolStripMenuItem_NormalCase
            // 
            this.toolStripMenuItem_NormalCase.Name = "toolStripMenuItem_NormalCase";
            this.toolStripMenuItem_NormalCase.Size = new System.Drawing.Size(212, 22);
            this.toolStripMenuItem_NormalCase.Text = "Normal Character Case";
            this.toolStripMenuItem_NormalCase.Click += new System.EventHandler(this.toolStripMenuItem_NormalCase_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightCyan;
            this.ClientSize = new System.Drawing.Size(994, 443);
            this.Controls.Add(this.splitContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Parallel System";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.toolStrip_Top.ResumeLayout(false);
            this.toolStrip_Top.PerformLayout();
            this.contextMenuStrip_SN.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ToolStrip toolStrip_Top;
        private System.Windows.Forms.ToolStripButton toolStripButton_Recall;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolStripButton_Add;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton toolStripButton_Remove;
        private System.Windows.Forms.ToolStripLabel toolStripLabel_Status;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripLabel SN;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBox_SN;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip_SN;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_Forbid;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_UpperCase;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_LowerCase;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_NormalCase;


    }
}

