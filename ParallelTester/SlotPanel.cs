using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Xml;
using System.IO;
using System.Reflection;
using System.Collections;
using System.Diagnostics;


namespace ParallelTester
{
    public partial class SlotPanel : UserControl
    {
        private delegate void iDelegate(string message);
        private string RunStatus = "";
        private Config iConfig = new Config();
        public string ChannelName = "Channel X";
        private string _SNStr="";
        private bool _IsRunning = false;

        public delegate void Active_EventHandler(object sender, EventArgs e);
        public static event Active_EventHandler ActiveEvent;
        public delegate void CheckSN_EventHandler(object sender, EventArgs e);
        public static event CheckSN_EventHandler CheckSNEvent;
        private Remote iRemote = new Remote();
        Stopwatch iStopwatch = null;
        private List<string> list_Result = new List<string>();

        public bool IsRunning
        {
            get { return _IsRunning; }
            set { _IsRunning = value; }
        }

        public string SNStr
        {
            get { return _SNStr; }
            set { _SNStr = value; }
        }

        #region GUI Event

        private void toolStripButton_Start_Click(object sender, EventArgs e)
        {
            if (this.Parent == null) return;
            if (SlotPanel.CheckSNEvent != null)
            {
                SlotPanel.CheckSNEvent(this, null);
            }
            if (string.IsNullOrEmpty(SNStr)) return; //else MessageBox.Show(SNStr);
            /////////////////////
            toolStripButton_Start.Enabled = false;
            toolStripButton_Pause.Enabled = true;
            toolStripButton_Stop.Enabled = true;
            toolStripButton_Config.Enabled = false;
            IsRunning = true;
            iRemote.IsStop = false;
            listView_Status.Items.Clear();
            if (!splitContainer1.Panel2Collapsed) splitContainer1.Panel2Collapsed = true;
            Thread iTestThread = new Thread(() => RunTest());
            iTestThread.Start();
            //iTestThread.Join();
        }

        private void toolStripButton_Pause_Click(object sender, EventArgs e)
        {
            try
            {
                switch (toolStripButton_Pause.Text.ToUpper())
                {
                    case "PAUSE":
                        RunStatus = "Pause";
                        toolStripButton_Pause.Text = "Resume";
                        toolStripButton_Pause.ToolTipText = "Resume";
                        break;
                    case "RESUME":
                        RunStatus = "Resume";
                        toolStripButton_Pause.Text = "Pause";
                        toolStripButton_Pause.ToolTipText = "Pause";

                        break;
                    default:
                        RunStatus = "Pause";
                        toolStripButton_Pause.Text = "Resume";
                        toolStripButton_Pause.ToolTipText = "Resume";
                        break;
                }
            }
            catch (Exception ex)
            {
                DealwithException(ex);
            }
        }

        private void toolStripButton_Stop_Click(object sender, EventArgs e)
        {
            RunStatus = "Stop";
            iRemote.IsStop = true;
        }

        private void toolStripButton_Config_Click(object sender, EventArgs e)
        {
            try
            {
                if (splitContainer1.Panel2Collapsed) splitContainer1.Panel2Collapsed = false;
                else
                {
                    splitContainer1.Panel2Collapsed = true;

                    SaveConfigToXML(Application.StartupPath + "\\" + ChannelName + ".xml");
                }
            }
            catch (Exception ex) { DealwithException(ex); }
        }

        public SlotPanel()
        {
            try
            {
                InitializeComponent();
                //string fileName = Application.StartupPath + "\\" + ChannelName + ".xml";
                //if (File.Exists(fileName)) LoadConfigFromXML(fileName);
            }
            catch (Exception ex)
            {
                DealwithException(ex);
            }
        }

        private void SlotPanel_Load(object sender, EventArgs e)
        {
            try
            {
                string fileName = Application.StartupPath + "\\" + ChannelName + ".xml";
               
                if (File.Exists(fileName)) LoadConfigFromXML(fileName);

                InitializeSetting();

                listView_Status.Columns.Add("", -2, HorizontalAlignment.Left);
            }
            catch (Exception ex) { DealwithException(ex); }
        }

        private void propertyGrid1_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            if (e.ChangedItem.Label == "IsMultiShare")
            {
                if ((bool)e.ChangedItem.Value)
                {
                    PropertyGrid1.HiddenAttributes = new AttributeCollection(new Attribute[] { new CategoryAttribute("GPIB"), new CategoryAttribute("Interface") });
                   
                }
                else
                {
                    PropertyGrid1.HiddenAttributes =  new AttributeCollection(new Attribute[] { new CategoryAttribute("GPIB")});

                }
                PropertyGrid1.Refresh();
            }


            if (e.ChangedItem.Label == "Interface")
            {
                switch (e.ChangedItem.Value.ToString())
                {
                    case "GPIB":
                        PropertyGrid1.HiddenAttributes = new AttributeCollection(
                              new Attribute[] { new CategoryAttribute("Ethernet") });
                        //PropertyGrid1.HiddenProperties = new string[] { "IP_Address" };
                        break;
                    case "Ethernet":
                        PropertyGrid1.HiddenAttributes = new AttributeCollection(
                              new Attribute[] { new CategoryAttribute("GPIB") });
                        // PropertyGrid1.HiddenProperties = new string[] { "GPIB_Card_Number", "GPIB_Address" };
                        break;
                }

                PropertyGrid1.Refresh();
            }

            if (e.ChangedItem.Label == "ChannelName")
            {
                this.ChannelName = groupBox1.Text = e.ChangedItem.Value.ToString();
            }

            if (e.ChangedItem.Label == "MeasureItem")
            {
                switch (e.ChangedItem.Value.ToString())
                {
                    case "BER":
                        PropertyGrid1.HideAttribute(new CategoryAttribute("Scope"));
                        PropertyGrid1.ShowAttribute(new CategoryAttribute("BER"));
                        break;
                    case "Scope":
                        PropertyGrid1.ShowAttribute(new CategoryAttribute("Scope"));
                        PropertyGrid1.HideAttribute(new CategoryAttribute("BER"));
                        break;
                }

                PropertyGrid1.Refresh();
            }

        }

        #endregion


        #region Function

        private void InitializeSetting()
        {
            PropertyGrid1.SelectedObject = iConfig;
            InitializePropertyGrid();
        }

        private void InitializePropertyGrid()
        {
            if (iConfig.IsMultiShare)
            {
                PropertyGrid1.HiddenAttributes = new AttributeCollection(new Attribute[] { new CategoryAttribute("GPIB"), new CategoryAttribute("Interface") });
            }
            else
            {
                switch (iConfig.Interface)
                {
                    case "GPIB":
                        PropertyGrid1.HiddenAttributes = new AttributeCollection(
                              new Attribute[] { new CategoryAttribute("Ethernet") });
                        //PropertyGrid1.HiddenProperties = new string[] { "IP_Address" };
                        break;
                    case "Ethernet":
                        PropertyGrid1.HiddenAttributes = new AttributeCollection(
                              new Attribute[] { new CategoryAttribute("GPIB") });
                        // PropertyGrid1.HiddenProperties = new string[] { "GPIB_Card_Number", "GPIB_Address" };
                        break;
                }
            }

            if (iConfig.MeasureItem == "BER")
            {
                PropertyGrid1.HideAttribute(new CategoryAttribute("Scope"));
                PropertyGrid1.ShowAttribute(new CategoryAttribute("BER"));

            }
            else
            {
                PropertyGrid1.ShowAttribute(new CategoryAttribute("Scope"));
                PropertyGrid1.HideAttribute(new CategoryAttribute("BER"));
            }

       
        }

        public void SetChannelName(string channelName)
        {
            try
            {
                ChannelName=groupBox1.Text = channelName;
                iConfig.ChannelName = channelName;
                //PropertyGrid1.SelectedObject = null;
               // PropertyGrid1.SelectedObject = iConfig;
            }
            catch (Exception ex) { DealwithException(ex); }

        }

        private void DealwithException(Exception ex)
        {
            try
            {
                if (ex.Message.ToString().Contains("ErrorResourceLocked: ")) MessageBox.Show("程序出现严重问题，通道 "+this.ChannelName+" 对应的仪表需要重启！");
                if (this.Parent == null) return;
                if (this.IsDisposed) return;
                
                this.Invoke(new iDelegate(UpdateStatus), "!@"+ ex.Message.ToString());

                #region Save exception log

                string iFileName = Application.StartupPath + "\\Log\\"+iConfig.ChannelName+"\\ApplicaitonExceptionLog - " + DateTime.Now.ToString().Replace("/", "-").Replace(":", "-") + ".xml";
                if (!Directory.Exists(Application.StartupPath + "\\Log\\" + iConfig.ChannelName))
                {
                    Directory.CreateDirectory(Application.StartupPath + "\\Log\\" + iConfig.ChannelName);
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
            listView_Status.Items.Add(new ListViewItem(DateTime.Now.ToString() + ": " + statusContext.Replace("!@", "").Replace("!#", "")));
            if (statusContext.Contains("!@"))
            {
                listView_Status.Items[listView_Status.Items.Count - 1].ForeColor = Color.Red;
            }
            else
            {
                listView_Status.Items[listView_Status.Items.Count - 1].ForeColor = Color.Blue;
                if(statusContext.ToLower().Contains("!#pass"))listView_Status.Items[listView_Status.Items.Count - 1].BackColor=Color.LightGreen;
                if (statusContext.ToLower().Contains("!#fail")) listView_Status.Items[listView_Status.Items.Count - 1].BackColor = Color.Red;
            }
            listView_Status.Items[listView_Status.Items.Count - 1].EnsureVisible();
            Application.DoEvents();
        }

        private void ResumeGUI(string message)
        {
            //UpdateStatus(GC.GetTotalMemory(false).ToString());
            GC.Collect();
            //UpdateStatus(GC.GetTotalMemory(true).ToString());
            //System.Media.SoundPlayer player = new System.Media.SoundPlayer();
            //player.SoundLocation = Application.StartupPath+"\\loveFiona.wav";
            //player.Play();
;

            toolStripButton_Start.Enabled = true;
            toolStripButton_Pause.Enabled = false;
            toolStripButton_Stop.Enabled = false;
            toolStripButton_Config.Enabled = true;
            IsRunning = false;
            Active();
  
            if (this.Parent == null) this.Dispose();

        }

        private int CheckRunStatus()
        {
            if (this.Parent == null) return -1;
            switch (RunStatus.ToUpper())
            {
                case "PAUSE":
                    this.Invoke(new iDelegate(UpdateStatus), "User Pause");
                    do
                    {
                        Application.DoEvents();
                        Thread.Sleep(50);
                        switch (RunStatus.ToUpper())
                        {
                            case "RESUME":
                                this.Invoke(new iDelegate(UpdateStatus), "Testing...");
                                break;
                            case "STOP":
                                RunStatus = "";
                                this.Invoke(new iDelegate(UpdateStatus), "User Stop");
                                return -1;
                        }
                    } while (RunStatus.ToUpper() == "PAUSE");


                    return 0;
                case "STOP":
                    RunStatus = "";
                    this.Invoke(new iDelegate(UpdateStatus), "User Stop");
                    return -1;
                default:
                    //this.Invoke(new iDelegate(UpdateStatus), "Testing...");
                    break;
            }

            RunStatus = "";
            return 0;
        }

        private void RunTest()
        {
            try
            {
                list_Result.Clear();
                list_Result.Add(DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss"));
                if (iConfig.IsMultiShare)
                {
                    iRemote.resourceName = iConfig.IP_Address;
                }
                else
                {
                    switch (iConfig.Interface)
                    {
                        case "GPIB":
                            iRemote.resourceName = "GPIB" + iConfig.GPIB_Card_Number.ToString() + "::" + iConfig.GPIB_Address.ToString() + "::INSTR";
                            break;
                        case "Ethernet":
                            iRemote.resourceName = "TCPIP0::" + iConfig.IP_Address + "::5001::SOCKET";
                            break;
                    }
                }
            
                    
                switch (iConfig.MeasureItem)
                { 
                    case "BER":
                        DoBERTest();
                        break;
                    case "Scope":
                        DoScopeTest();
                        break;
                }

                SaveTestData();
            }
            catch (Exception ex)
            {
                DealwithException(ex);
            }
            finally
            {
                iRemote.IsStop = false;
                iRemote.Close();
                this.Invoke(new iDelegate(ResumeGUI), "");
            }
        }

        private void DoBERTest()
        {
            string er = "";
            this.Invoke(new iDelegate(UpdateStatus), "----- Input Sensitivity Test(BERT) -----");

            //Log("-- Setup mesurement period");
            //Write(":BERT:SENSe:MEASure:EALarm:MODE SINGle");
            //Write(":BERT:SENSe:MEASure:EALarm:PERiod 0,0,0,10"); // 10 sec

 
            iRemote.Write(":MOD:ID " + iConfig.MoudleID + ";:SENSe:MEASure:STARt");


            do
            {
                Application.DoEvents();
                Thread.Sleep(200);
                if (CheckRunStatus() == -1) break;
    
                if (iRemote.CheckMeasureStatus(":MOD:ID " + iConfig.MoudleID + ";:SENSe:MEASure:EALarm:STATe?","0")) break;

                
                er = (iRemote.Query(":MOD:ID " + iConfig.MoudleID + ";:CALCulate:DATA:EALarm? \"CURRent:ER:TOTal\"").TrimEnd('\n')).Replace("\"","");

  
                this.Invoke(new iDelegate(UpdateStatus), "Error Rate: " +er);

            } while (Convert.ToDouble(er.Replace("-------", "0")) == 0.0);

            if (Convert.ToDouble(er.Replace("-------", "0")) == 0.0)
            {
                list_Result.Add("Test Result: Pass");
                this.Invoke(new iDelegate(UpdateStatus), "!#Pass!");
            }
            else
            {
                list_Result.Add("Test Result: Fail");
                this.Invoke(new iDelegate(UpdateStatus), "!#Fail!");
            }

            list_Result.Add("Error Rate: " + er);
     
            
        }

        private void DoScopeTest()
        {
            List<string> tList = new List<string>();
            string tStr = "";
            bool IsPass = true;
            this.Invoke(new iDelegate(UpdateStatus), "----- Scope Test -----");
            ////Log("-- Select CHA");
            //iRemote.Write(":MOD:ID 5;:INPut:CHA ON");
            //iRemote.Write(":MOD:ID 5;:INPut:CHB OFF");
            //iRemote.Write(":MOD:ID 5;:CONFigure:MEASure:CHANnel A");

            ////Log("-- Setup sampling parameter");
            //iRemote.Write(":MOD:ID 5;:OPTion:MAX:SAMPles:NUMber 1350");
            //iRemote.Write(":MOD:ID 5;:ACCUmulation:TYPe LIMited");
            //iRemote.Write(":MOD:ID 5;:ACCUmulation:LIMit WAVeform,100");

            ////Log("-- Setup test mode");
           

            ////Log("-- Setup display item");
            //iRemote.Write(":MOD:ID 5;:CONFigure:MEASure:AMPTIME1 CHA, 9");
            //iRemote.Write(":MOD:ID 5;:CONFigure:MEASure:AMPTIME2 CHA, 10");
            //iRemote.Write(":MOD:ID 5;:CONFigure:MEASure:AMPTIME3 CHA, 11");
            //iRemote.Write(":MOD:ID 5;:CONFigure:MEASure:AMPTIME4 CHA, 12");

            #region "AmplitudeTime Test"

            if (iConfig.AmplitudeTimeTest.Check)
            {
                iRemote.Write(":MOD:ID 5;:CONFigure:MEASure:TYPe AMPTIME");
                this.Invoke(new iDelegate(UpdateStatus), "-- Setup scale");
                iRemote.Write(":MOD:ID 5;:DISPlay:WINDow:SCALe:AUTOscale BOTH");
                iRemote.Query(":SYSTem:ERRor?");
                this.Invoke(new iDelegate(UpdateStatus), "-- Check error and setup operation complete");
                this.Invoke(new iDelegate(UpdateStatus), "AmplitudeTime-- Start sampling and query mesurement results");
                iRemote.Write(":MOD:ID 5;:SAMPling:STATus RUN");
                Wait(":MOD:ID 5;:SAMPling:STATus?", "HOLD");
             
                tList.AddRange(iRemote.Query(":MOD:ID 5;:CONFigure:MEASure:CHANnel "+iConfig.ScopeChannel+";:FETCh:AMPLitude:MEASurement?").TrimEnd('\n').Split(','));
                foreach (AmplitudeTest eAmplitudeTest in iConfig.AmplitudeTimeTest.AmplitudeTests)
                {
                    switch (eAmplitudeTest.Name)
                    {
                        case "One Level":
                            eAmplitudeTest.Result = tList[0];
                            tStr = "One Level： " + eAmplitudeTest.Result;
                            break;
                        case "Zero Level":
                            eAmplitudeTest.Result = tList[1];
                            tStr = "Zero Level： " + eAmplitudeTest.Result;
                            break;
                        case "Eye amplitude":
                            eAmplitudeTest.Result = tList[2];
                            tStr = "Eye amplitude： " + eAmplitudeTest.Result;
                            break;
                        case "Eye height":
                            eAmplitudeTest.Result = tList[3];
                            tStr = "Eye height： " + eAmplitudeTest.Result;
                            break;
                        case "Crossing":
                            eAmplitudeTest.Result = tList[4];
                            tStr = "Crossing： " + eAmplitudeTest.Result;
                            break;
                        case "SNR":
                            eAmplitudeTest.Result = tList[5];
                            tStr = "SNR： " + eAmplitudeTest.Result;
                            break;
                        case "Average Power":
                            eAmplitudeTest.Result = tList[6];
                            tStr = "Average Power： " + eAmplitudeTest.Result;
                            break;
                        case "Extinction Ratio":
                            eAmplitudeTest.Result = tList[8];
                            tStr = "Extinction Ratio： " + eAmplitudeTest.Result;
                            break;
                        case "OMA (dBm)":
                            eAmplitudeTest.Result = tList[10];
                            tStr = "OMA (dBm)： " + eAmplitudeTest.Result;
                            break;
                        default:
                            tList.Clear();
                            break;
                    }
                    if (!eAmplitudeTest.IsPass) IsPass = false;
                    list_Result.Add(tStr);
                    if (!eAmplitudeTest.IsPass) this.Invoke(new iDelegate(UpdateStatus), "!@"+tStr);
                    else this.Invoke(new iDelegate(UpdateStatus), tStr);
                   
                }
                tList.Clear();
                tList.AddRange(iRemote.Query(":MOD:ID 5;:CONFigure:MEASure:CHANnel " + iConfig.ScopeChannel + ";:FETCh:TIME:MEASurement?").TrimEnd('\n').Split(','));
                foreach (TimeTest eTimeTest in iConfig.AmplitudeTimeTest.TimeTests)
                {
                    switch (eTimeTest.Name)
                    {
                        case "Jitter p-p":
                            eTimeTest.Result = tList[0];
                            tStr = "Jitter p-p： " + eTimeTest.Result;
                            break;
                        case "Jitter RMS":
                            eTimeTest.Result = tList[1];
                            tStr = "Jitter RMS： " + eTimeTest.Result;
                            break;
                        case "Rise Time":
                            eTimeTest.Result = tList[2];
                            tStr = "Rise Time： " + eTimeTest.Result;
                            break;
                        case "Fall Time":
                            eTimeTest.Result = tList[3];
                            tStr = "Fall Time： " + eTimeTest.Result;
                            break;
                        case "Eye Width":
                            eTimeTest.Result = tList[4];
                            tStr = "Eye Width： " + eTimeTest.Result;
                            break;
                        case "DCD":
                            eTimeTest.Result = tList[5];
                            tStr = "DCD： " + eTimeTest.Result;
                            break;
                        default:
                            tList.Clear();
                            break;
                    }
                    if (!eTimeTest.IsPass) IsPass = false;
                    list_Result.Add(tStr);
                    if (!eTimeTest.IsPass) this.Invoke(new iDelegate(UpdateStatus), "!@" + tStr);
                    else this.Invoke(new iDelegate(UpdateStatus), tStr);
                }

            }
            tList.Clear();
            #endregion

            #region "Mask Test"

            if (iConfig.MaskTest.Check)
            { 
                iRemote.Write(":MOD:ID 5;:CONFigure:MEASure:TYPe MASK");
                this.Invoke(new iDelegate(UpdateStatus), "-- Setup scale");
                iRemote.Write(":MOD:ID 5;:DISPlay:WINDow:SCALe:AUTOscale BOTH");
                iRemote.Query(":SYSTem:ERRor?");
                this.Invoke(new iDelegate(UpdateStatus), "-- Check error and setup operation complete");
                this.Invoke(new iDelegate(UpdateStatus), "Mask-- Start sampling and query mesurement results");
                iRemote.Write(":MOD:ID 5;:SAMPling:STATus RUN");
                Wait(":MOD:ID 5;:SAMPling:STATus?", "HOLD");
                //
                iConfig.MaskTest.Result = iRemote.Query(":MOD:ID 5;:CONFigure:MEASure:CHANnel " + iConfig.ScopeChannel + ";:MEASure:MASK:MARGin?").TrimEnd('\n');
                tStr = "Mask Margin： " + iConfig.MaskTest.Result;
                list_Result.Add(tStr);
                if (!iConfig.MaskTest.IsPass) { this.Invoke(new iDelegate(UpdateStatus), "!@" + tStr); IsPass = false; }
                else this.Invoke(new iDelegate(UpdateStatus), tStr); 
            }

            #endregion
            if (IsPass) this.Invoke(new iDelegate(UpdateStatus), "!#Pass!"); else this.Invoke(new iDelegate(UpdateStatus), "!#Fail!");

        }


        private void Wait(string command, string flage, int Cycles = 50, int delayTime = 200)
        {
            int cycleCount = 0;
            do
            {
                if (CheckRunStatus() == -1) break;
                iStopwatch = Stopwatch.StartNew();
                do
                {
                    Application.DoEvents();
                    Thread.Sleep(20);
                } while (iStopwatch.ElapsedMilliseconds < delayTime);

                cycleCount++;
            } while (!iRemote.CheckMeasureStatus(command, flage) && cycleCount < Cycles);
        }

        private void SaveTestData()
        {
            if (this.Parent == null) return;
 
            string fileName = Application.StartupPath + "\\Test Result\\" +SNStr+"_"+ Path.GetFileNameWithoutExtension(iConfig.ChannelName) + ".csv";
            if (!File.Exists(fileName))
            {
                if (!Directory.Exists(Application.StartupPath + "\\Test Result\\")) Directory.CreateDirectory(Application.StartupPath + "\\Test Result\\");
            }
            
            File.WriteAllText(fileName, string.Join("\r\n",list_Result.ToArray()));
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

                        SaveClassToXML(iConfig, iXmlWriter);

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
                                ReadXMLToClass(iConfig, "Config", iXmlReader);
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


        #region for Active Event

        public void Active()
        {
            this.BackColor = Color.Blue;
            if (SlotPanel.ActiveEvent != null)
            {
                SlotPanel.ActiveEvent(this, null);
            }
            //expose the click event of AcceptButton
                //if (this.BackColor != Color.Blue)
                //{
                //    this.BackColor = Color.Blue;
                //    if (SlotPanel.ActiveEvent != null)
                //    {
                //        SlotPanel.ActiveEvent(this, null);
                //    }
                //}
        }

        public void DeActive()
        {
            //expose the click event of AcceptButton
            if (this.BackColor == Color.Blue)
            {
                this.BackColor = Color.White;
            }
        }

        //private void listView_Status_Enter(object sender, EventArgs e)
        //{
        //    Active();
        //}

        private void SlotPanel_Enter(object sender, EventArgs e)
        {
            Active();
        }

        //private void groupBox1_Enter(object sender, EventArgs e)
        //{
        //    Active();
        //}

        //private void PropertyGrid1_Enter(object sender, EventArgs e)
        //{
        //    Active();
        //}

        private void toolStrip_Top_Click(object sender, EventArgs e)
        {
            Active();
        }

        #endregion

    }
}
