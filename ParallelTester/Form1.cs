using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Xml;
using System.IO;
using System.Reflection;
using System.Collections;
using System.Text.RegularExpressions;
using System.Diagnostics;


namespace ParallelTester
{
    public partial class Form1 : Form
    {
        private delegate void iDelegate(string message);
        private List<SlotPanel> list_SlotPanel = new List<SlotPanel>();
        private int ItemIndex = -1;
        private int tInt = -1;
        private Mutex iMutex = new Mutex();
        private Setting iSetting = new Setting();



        #region Add/Remove Channel

        private void toolStripButton_Add_Click(object sender, EventArgs e)
        {
            UpdateStatus("");
            toolStrip_Top.Enabled = false;
            Thread iAddThread = new Thread(() => SlotPanel_Add());
            iAddThread.Start();

        }

        private void toolStripButton_Remove_Click(object sender, EventArgs e)
        {
            UpdateStatus("");
            toolStrip_Top.Enabled = false;
            Thread iRemoveThread = new Thread(() => SlotPanel_Remove());
            iRemoveThread.Start();
        }


        public void SlotPanel_Remove()
        {
            try
            {
                this.Invoke(new iDelegate(SlotPanel_DoRemove), "");

            }
            catch (Exception ex)
            {
                DealwithException(ex);
            }
            finally
            {
                ItemIndex = -1;
                try
                {
                    this.Invoke(new iDelegate(SlotPanel_ClearSelection), "");
                    this.Invoke(new iDelegate(ResumeGUI), "");
                }
                catch { };
            }
        }

        private void SlotPanel_DoRemove(string message)
        {
            if (list_SlotPanel.Count == 0) return;

            if (ItemIndex != -1) SlotPanel_RemoveAt(ItemIndex); else SlotPanel_RemoveAt(list_SlotPanel.Count - 1);

        }

        public void SlotPanel_RemoveAt(int index)
        {
            if (index < 0) return;
            iMutex.WaitOne();
            list_SlotPanel.RemoveAt(index);
            iMutex.ReleaseMutex();
            ReloadTLP();
        }

        private void SlotPanel_ClearSelection(string message)
        {
            foreach (SlotPanel eSlotPanel in list_SlotPanel)
            {
                eSlotPanel.DeActive();
            }
        }

        public void SlotPanel_Add()
        {
            try
            {
                this.Invoke(new iDelegate(SlotPanel_DoAdd), "");
            }
            catch (Exception ex)
            {
                DealwithException(ex);
            }
            finally
            {
                try
                {
                    this.Invoke(new iDelegate(ResumeGUI), "");
                }
                catch { };
            }
        }

        private void SlotPanel_DoAdd(string message)
        {
            SlotPanel nSlotPanel = new SlotPanel();

            nSlotPanel.Dock = DockStyle.Left;

            nSlotPanel.SetChannelName(CreateChannelName(list_SlotPanel.Count));
            //nSlotPanel.SetChannelName("Channel " + list_SlotPanel.Count);

            if (list_SlotPanel.Contains(nSlotPanel))
                throw new Exception("The control already exists in the table.");
            iMutex.WaitOne();
            this.list_SlotPanel.Add(nSlotPanel);
            iMutex.ReleaseMutex();
            ReloadTLP();
            tableLayoutPanel1.ScrollControlIntoView(nSlotPanel);

        }

        private string CreateChannelName(int channelIndex)
        {
            string tChannelName = "Channel " + channelIndex.ToString();
            foreach (SlotPanel eSlotPanel in list_SlotPanel)
            {
                if (tChannelName == eSlotPanel.ChannelName)
                {
                    tChannelName = CreateChannelName(channelIndex + 1);
                    break;
                }
            }
            return tChannelName;
        }

        void SlotPanel_CheckSNEvent(object sender, EventArgs e)
        {
            string SNStr = "";
            foreach (SlotPanel eSlotPanel in list_SlotPanel)
            {
                if (eSlotPanel.IsRunning)
                {
                    SlotPanel iSlotPanel = (SlotPanel)sender;
                    iSlotPanel.SNStr = SNStr = toolStripTextBox_SN.Text;
                    return;
                }
            }
            if (CheckSN(out SNStr))
            {
                toolStripTextBox_SN.Enabled = false;
                SlotPanel iSlotPanel = (SlotPanel)sender;
                iSlotPanel.SNStr = SNStr;
            }
        }

        void SlotPanel_ActiveEvent(object sender, EventArgs e)
        {
            try
            {
                SlotPanel iSlotPanel = (SlotPanel)sender;
                if (iSlotPanel.Parent != null)
                {
                    tInt = tableLayoutPanel1.GetColumn(iSlotPanel);

                    if (ItemIndex != tInt && ItemIndex != -1 && ItemIndex < list_SlotPanel.Count)
                    {
                        list_SlotPanel[ItemIndex].DeActive();
                        //tStr += "DeActive:" + ItemIndex.ToString() + ",";
                    }
                    try
                    {
                        iMutex.WaitOne();
                        ItemIndex = tInt;

                        // UpdateStatus(tStr += " | Active:" + ItemIndex.ToString());
                        tableLayoutPanel1.ScrollControlIntoView(iSlotPanel);
                    }
                    finally
                    {
                        iMutex.ReleaseMutex();
                    }
                }
                else
                {
                    ItemIndex = -1;
                }

                if (iSetting.IsEnableSN)
                {
                    foreach (SlotPanel eSlotPanel in list_SlotPanel)
                    {
                        if (eSlotPanel.IsRunning) return;
                    }
                    toolStripTextBox_SN.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                DealwithException(ex);
            }

        }

        public void ReloadTLP()
        {
            tableLayoutPanel1.Controls.Clear();
            //tableLayoutPanel1.ColumnCount = 0;

            this.tableLayoutPanel1.ColumnCount = list_SlotPanel.Count;

            //float h = 100.0f / (float)list_SlotPanel.Count;
            //for (int i = 0; i < list_SlotPanel.Count; i++)
            //    this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));


            int cCount = 0;

            foreach (Control c in list_SlotPanel)
            {
                if (!c.Visible)
                    continue;
                this.tableLayoutPanel1.Controls.Add(c, cCount, 0);
                cCount++;
            }


            tableLayoutPanel1.RowStyles[0] = new RowStyle(SizeType.Percent, 100F);
            tableLayoutPanel1.ColumnStyles[0] = new ColumnStyle(SizeType.AutoSize);
        }

        #endregion

        #region GUI Event

        public Form1()
        {
            InitializeComponent();
            SlotPanel.ActiveEvent += new SlotPanel.Active_EventHandler(SlotPanel_ActiveEvent);
            SlotPanel.CheckSNEvent += new SlotPanel.CheckSN_EventHandler(SlotPanel_CheckSNEvent);
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            iSetting.ChannelList = "";
            foreach (SlotPanel eSlotPanel in list_SlotPanel)
            {
                iSetting.ChannelList += "," + eSlotPanel.ChannelName;
            }
            iSetting.ChannelList.TrimStart(',');
            SaveConfigToXML(Application.StartupPath + "\\Setting.xml");
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (toolStrip_Top.Enabled == false && e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                if (DialogResult.OK == MessageBox.Show("Please stop the test before you quit!")) return;
            }
            foreach (SlotPanel eSlotPanel in list_SlotPanel)
            {
                if (eSlotPanel.IsRunning)
                {
                    e.Cancel = true;
                    if (DialogResult.OK == MessageBox.Show("Please stop the test before you quit!")) return;
                    break;
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                LoadConfigFromXML(Application.StartupPath + "\\Setting.xml");
                if (iSetting.IsEnableSN) toolStripTextBox_SN.Enabled = true; else toolStripTextBox_SN.Enabled = false;
                if (iSetting.IsAllowManualInput_SN) toolStripMenuItem_Forbid.Text = "Forbid Manual Input";
                else toolStripMenuItem_Forbid.Text = "Allow Manual Input";
                switch (iSetting.CharacterCaseType_SN)
                {
                    case "Upper":
                        toolStripMenuItem_UpperCase_Click(null, null);
                        break;
                    case "Lower":
                        toolStripMenuItem_LowerCase_Click(null, null);
                        break;
                    case "Normal":
                        toolStripMenuItem_NormalCase_Click(null, null);
                        break;
                }
                //Thread runappThread = new Thread(() => RunAPP());
                //runappThread.Start();
            }
            catch (Exception ex)
            {
                DealwithException(ex);
            }
        }

        private void toolStripButton_Recall_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(iSetting.ChannelList)) { MessageBox.Show("No saved setup to recall!"); return; }
            UpdateStatus("");
            toolStrip_Top.Enabled = false;
            //LoadConfigFromXML(Application.StartupPath + "\\Setting.xml");
            Thread iLoadSlotPanelsThread = new Thread(() => LoadChannelSetting(""));
            iLoadSlotPanelsThread.Start();
        }

        #endregion

        #region Function

        private void LoadChannelSetting(string message)
        {
            try
            {
                this.Invoke(new iDelegate(LoadSlotPanels), "");
            }
            catch (Exception ex)
            {
                DealwithException(ex);
            }
            finally
            {
                try
                {
                    this.Invoke(new iDelegate(ResumeGUI), "");
                }
                catch { };
            }
        }

        private void LoadSlotPanels(string message)
        {
            list_SlotPanel.Clear();
            foreach (string eChannelName in iSetting.ChannelList.Split(','))
            {
                if (string.IsNullOrEmpty(eChannelName)) continue;
                SlotPanel nSlotPanel = new SlotPanel();
                nSlotPanel.Dock = DockStyle.Left;
                nSlotPanel.SetChannelName(eChannelName);
                if (list_SlotPanel.Contains(nSlotPanel))
                    throw new Exception("The control already exists in the table.");
                iMutex.WaitOne();
                this.list_SlotPanel.Add(nSlotPanel);
                iMutex.ReleaseMutex();
            }
            if (list_SlotPanel.Count > 0) ReloadTLP();

            //tableLayoutPanel1.ScrollControlIntoView(nSlotPanel);

        }

        private void DealwithException(Exception ex)
        {
            try
            {
                if (this.IsDisposed) return;

                this.Invoke(new iDelegate(UpdateStatus), "!@" + DateTime.Now.ToLongTimeString() + ":  " + ex.Message.ToString());

                #region Save exception log

                string iFileName = Application.StartupPath + "\\Log\\Main\\ApplicaitonExceptionLog - " + DateTime.Now.ToString().Replace("/", "-").Replace(":", "-") + ".xml";
                if (!Directory.Exists(Application.StartupPath + "\\Log\\Main"))
                {
                    Directory.CreateDirectory(Application.StartupPath + "\\Log\\Main");
                }

                using (FileStream fileSteam = File.Create(iFileName))
                {
                    XmlWriterSettings iXmlWriterSettings = new XmlWriterSettings { Indent = true, Encoding = Encoding.UTF8 };

                    using (XmlWriter iXmlWriter = XmlWriter.Create(fileSteam, iXmlWriterSettings))
                    {
                        iXmlWriter.WriteStartDocument();

                        iXmlWriter.WriteStartElement("Exception");

                        iXmlWriter.WriteElementString("Log", ex.ToString());

                        iXmlWriter.WriteEndElement();

                        iXmlWriter.WriteEndDocument();

                    }
                }

                #endregion
            }
            catch (Exception fex)
            {
                fex.ToString();
            }
        }

        private void UpdateStatus(string statusContext)
        {
            toolStripLabel_Status.Text = statusContext;
            if (statusContext.Contains("!@")) toolStripLabel_Status.ForeColor = Color.Red;
            else toolStripLabel_Status.ForeColor = Color.Blue;

        }

        private void ResumeGUI(string message)
        {
            //toolStripButton_Add.Enabled = true;
            //toolStripButton_Remove.Enabled = true;
            //toolStripButton_Recall.Enabled = true;
            toolStrip_Top.Enabled = true;
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            tableLayoutPanel1.AutoScroll = false;//fix the bug that when scroll bar exsiting, controls in tablelayoutpanel don't resize properly when form1 resizes.
            tableLayoutPanel1.RowStyles[0] = new RowStyle(SizeType.Percent, 100F);//to make controls in tablelayoutpanel resize properly when form1 resizes.
            tableLayoutPanel1.AutoScroll = true;
        }

        private void RunAPP()
        {
            try
            {
                Process.Start(Application.StartupPath + "\\Server\\iSocketAgentServer.exe");
            }
            catch (Exception ex)
            {
                DealwithException(ex);
            }
        }

        #endregion

        #region Config files

        //Save Config
        private void SaveConfigToXML(string iFileName)
        {
            try
            {
                #region Write

                using (FileStream fileSteam = File.Create(iFileName))
                {
                    XmlWriterSettings iXmlWriterSettings = new XmlWriterSettings { Indent = true, Encoding = Encoding.UTF8 };

                    using (XmlWriter iXmlWriter = XmlWriter.Create(fileSteam, iXmlWriterSettings))
                    {
                        iXmlWriter.WriteStartDocument();

                        iXmlWriter.WriteStartElement("Config");

                        SaveClassToXML(iSetting, iXmlWriter);

                        iXmlWriter.WriteEndElement();//config


                        iXmlWriter.WriteEndDocument();


                    }
                }

                #endregion
            }
            catch (Exception ex)
            {
                DealwithException(ex);
            }

        }

        //Load Config
        private void LoadConfigFromXML(string iFileName)
        {
            try
            {
                #region read

                using (FileStream fileStream = File.OpenRead(iFileName))
                {
                    XmlReaderSettings iXmlReaderSettings = new XmlReaderSettings(); ;
                    iXmlReaderSettings.ConformanceLevel = ConformanceLevel.Document;

                    using (XmlReader iXmlReader = XmlReader.Create(fileStream, iXmlReaderSettings))
                    {
                        iXmlReader.MoveToContent();
                        do
                        {
                            if (string.IsNullOrEmpty(iXmlReader.Name)) continue;

                            if (iXmlReader.Name == "Config" && iXmlReader.NodeType == XmlNodeType.Element)
                            {
                                ReadXMLToClass(iSetting, "Config", iXmlReader);
                            }

                        } while (iXmlReader.Read());

                    }
                }


                #endregion
            }
            catch (XmlException ex)
            {
                DealwithException(ex);
            }

        }

        private void SaveClassToXML(object iClassObject, XmlWriter iXmlWriter)
        {


            object value = null;

            foreach (var iElement in iClassObject.GetType().GetProperties())
            {
                //if (IsNotBrowsable(iElement)) continue;
                value = iElement.GetValue(iClassObject, new object[] { });
                if (iElement.PropertyType.BaseType.Name == "CollectionBase")
                {
                    iXmlWriter.WriteStartElement(iElement.Name);
                    foreach (var eSubObject in (CollectionBase)value)
                    {
                        iXmlWriter.WriteStartElement(iElement.Name.Substring(0, iElement.Name.Length - 1));
                        if (eSubObject == null) continue;

                        SaveClassToXML(eSubObject, iXmlWriter);
                        iXmlWriter.WriteEndElement();
                    }
                    iXmlWriter.WriteEndElement();
                }
                else
                {

                    if (iElement.PropertyType.Namespace == "System")
                    {
                        iXmlWriter.WriteElementString(iElement.Name, value.ToString());
                    }
                    else
                    {
                        iXmlWriter.WriteStartElement(iElement.Name);
                        SaveClassToXML(value, iXmlWriter);
                        iXmlWriter.WriteEndElement();
                    }
                }

            }
        }

        private void ReadXMLToClass(object iClassObject, string ClassName, XmlReader iXmlReader)
        {
            if (iClassObject == null) return;
            object value = null;
            while (iXmlReader.Read())
            {
                if (string.IsNullOrEmpty(iXmlReader.Name) || iXmlReader.IsEmptyElement) continue;
                if (iXmlReader.Name == ClassName && iXmlReader.NodeType == XmlNodeType.EndElement)
                {
                    break;
                }
                else
                {
                    try
                    {
                        foreach (var iElement in iClassObject.GetType().GetProperties())
                        {
                            //if (IsNotBrowsable(iElement)) continue;
                            if (iElement.Name == iXmlReader.Name)
                            {
                                if (iElement.PropertyType.BaseType.Name == "CollectionBase" && !iXmlReader.IsEmptyElement)
                                {
                                    value = iElement.GetValue(iClassObject, new object[] { });
                                    ReadXMLToCollection(value, iElement.Name, iXmlReader);
                                }
                                else
                                {
                                    if (iElement.PropertyType.Namespace == "System")
                                    {
                                        //iElement.SetValue(iClassObject, (IComparable)Convert.ChangeType(iXmlReader.ReadString(), iElement.PropertyType), new object[] { });

                                        if (iElement.CanWrite)
                                        {
                                            iElement.SetValue(iClassObject, (IComparable)Convert.ChangeType(iXmlReader.ReadString(), iElement.PropertyType), new object[] { });
                                        }

                                    }
                                    else
                                    {
                                        value = iElement.GetValue(iClassObject, new object[] { });
                                        ReadXMLToClass(value, iElement.Name, iXmlReader);
                                    }
                                }
                                break;
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        DealwithException(ex);
                    }
                }

            }
        }

        private void ReadXMLToCollection(object iCollection, string iCollectionName, XmlReader iXmlReader)
        {
            MethodInfo method = iCollection.GetType().GetMethod("Add", (BindingFlags)int.MaxValue);

            ParameterInfo[] iParameterInfo = method.GetParameters();
            if (iParameterInfo.Length <= 0) return;
            Type ParameterType = iParameterInfo[0].ParameterType;

            var arguments = new List<object>();

            object nObject = Activator.CreateInstance(ParameterType);
            //object nObject = Activator.CreateInstance(iCollection.GetType().GetProperties()[0].PropertyType);
            string[] tArray = nObject.GetType().ToString().Split('.');
            string ClassName = tArray[tArray.Length - 1];

            while (iXmlReader.Read())
            {
                if (string.IsNullOrEmpty(iXmlReader.Name) || iXmlReader.IsEmptyElement) continue;
                if (iXmlReader.Name == iCollectionName && iXmlReader.NodeType == XmlNodeType.EndElement)
                {
                    break;
                }
                else if (iXmlReader.Name == ClassName && iXmlReader.NodeType == XmlNodeType.Element)
                {
                    arguments.Clear();
                    nObject = null;
                    nObject = Activator.CreateInstance(ParameterType);
                    ReadXMLToClass(nObject, ClassName, iXmlReader);
                    arguments.Add(nObject);
                    method.Invoke(iCollection, arguments.ToArray());
                }
            }





        }

        public static bool IsNotBrowsable(PropertyInfo pi)
        {
            object[] Atts = pi.GetCustomAttributes(typeof(BrowsableAttribute), false);
            foreach (var eAtt in Atts)
            {
                if (eAtt is System.ComponentModel.BrowsableAttribute)
                {
                    BrowsableAttribute ibAtt = (BrowsableAttribute)eAtt;
                    if (!ibAtt.Browsable) return true;
                }
            }

            return false;
        }

        #endregion

        #region SN Input

        private void toolStripTextBox_SN_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Right)
                {
                    toolStripTextBox_SN.TextBox.ContextMenuStrip = contextMenuStrip_SN;
                }
            }
            catch (Exception ex)
            {
                DealwithException(ex);
            }
        }

        private void toolStripMenuItem_Forbid_Click(object sender, EventArgs e)
        {
            try
            {
                if (toolStripMenuItem_Forbid.Text == "Forbid Manual Input")
                {
                    iSetting.IsAllowManualInput_SN = false;
                    toolStripMenuItem_Forbid.Text = "Allow Manual Input";
                    toolStripTextBox_SN.Text = "";
                }
                else
                {
                    iSetting.IsAllowManualInput_SN = true;
                    toolStripMenuItem_Forbid.Text = "Forbid Manual Input";
                }
                toolStripTextBox_SN.TextBox.Focus();
            }
            catch (Exception ex)
            {
                DealwithException(ex);
            }
        }

        private void toolStripMenuItem_UpperCase_Click(object sender, EventArgs e)
        {
            try
            {
                toolStripTextBox_SN.CharacterCasing = CharacterCasing.Upper;
                iSetting.CharacterCaseType_SN = "Upper";
                toolStripMenuItem_UpperCase.Checked = true;
                toolStripMenuItem_LowerCase.Checked = false;
                toolStripMenuItem_NormalCase.Checked = false;
                toolStripTextBox_SN.TextBox.Focus();
            }
            catch (Exception ex)
            {
                DealwithException(ex);
            }
        }

        private void toolStripMenuItem_LowerCase_Click(object sender, EventArgs e)
        {
            try
            {
                toolStripTextBox_SN.CharacterCasing = CharacterCasing.Lower;
                iSetting.CharacterCaseType_SN = "Lower";
                toolStripMenuItem_UpperCase.Checked = false;
                toolStripMenuItem_LowerCase.Checked = true;
                toolStripMenuItem_NormalCase.Checked = false;
                toolStripTextBox_SN.TextBox.Focus();
            }
            catch (Exception ex)
            {
                DealwithException(ex);
            }
        }

        private void toolStripMenuItem_NormalCase_Click(object sender, EventArgs e)
        {
            try
            {
                toolStripTextBox_SN.CharacterCasing = CharacterCasing.Normal;
                iSetting.CharacterCaseType_SN = "Normal";
                toolStripMenuItem_UpperCase.Checked = false;
                toolStripMenuItem_LowerCase.Checked = false;
                toolStripMenuItem_NormalCase.Checked = true;
                toolStripTextBox_SN.TextBox.Focus();
            }
            catch (Exception ex)
            {
                DealwithException(ex);
            }
        }

        private void toolStripTextBox_SN_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (iSetting.IsAllowManualInput_SN == false) e.Handled = true;
        }

        private bool CheckSN(out string SNStr)
        {
            if (toolStripTextBox_SN.Text == "")
            {
                SNStr = "";
                MessageBox.Show("SN number is empty !");
                return false;
            }
            else
            {
                Regex SNtext = new Regex(@"^[A-Za-z0-9]+$");
                if (!SNtext.IsMatch(toolStripTextBox_SN.Text))
                {
                    SNStr = "";
                    MessageBox.Show("Only characters and numbers is permitted !");
                    return false;
                }
                else
                {
                    SNStr = toolStripTextBox_SN.Text;
                    if (SNStr.Length > 30)
                    {

                        MessageBox.Show("The length of the Inputed characters is too long !");
                        return false;
                    }
                }
            }
            return true;
        }

        #endregion


    }
}
