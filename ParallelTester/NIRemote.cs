using System;
using System.Collections.Generic;
using System.Text;
using NationalInstruments.VisaNS;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using System.Diagnostics;

namespace ParallelTester
{
   public class NIRemote
    {
        public string resourceName = "";
        private MessageBasedSession mbSession;
        public bool IsConnected = false;
        public bool IsLocked = false;
        private bool _IsStop = false;
        private Stopwatch iStopwatch = null;
        private Random iRandom = new Random();



        public bool IsStop
        {
            get { return _IsStop; }
            set { _IsStop = value; }
        }

        public bool OpenInstrment()
        {
            try
            {
                //ResourceManager.GetLocalManager().FindResources("?*");
                mbSession = (MessageBasedSession)ResourceManager.GetLocalManager().Open(resourceName);

                mbSession.Timeout = 60000;
                IsConnected = true;
                // Setup termination
                switch (mbSession.HardwareInterfaceType)
                {
                    case HardwareInterfaceType.Tcpip:
                        mbSession.TerminationCharacterEnabled = true;
                        break;
                    default:
                        break;
                }
               
                //Write("*RST");
                //Query("*IDN?");
                
            }
            catch (Exception ex)
            {
                IsConnected = false;
                throw new ApplicationException("Connect Error:" + ex.Message.ToString());
            }

            return true;
        }

        public void Write(string command)
        {
            bool HasTakenLockAction = false;
            try
            {
               
                if (!IsConnected) OpenInstrment();
                //if (!IsLocked)
                //{
                //    Lock();
                //    HasTakenLockAction = true;
                //}
                mbSession.Write(command + "\n");
            }
            catch (Exception ex)
            {
                if (ex.Message.ToString().Contains("ErrorResourceLocked"))
                {
                    UnLock();
                    Close();
                    Lock();
                }
                else
                {
                    IsConnected = false;
                    throw new ApplicationException("Remote Error -->" + command + ":  " + ex.Message.ToString());
                }
            }
            finally
            {
                if (HasTakenLockAction) UnLock();
            }
           
        }

        public string Query(string command)
        {
            bool HasTakenLockAction=false;
            try
            {
                if (!IsConnected) OpenInstrment();
                //if (!IsLocked)
                //{
                //    Lock();
                //    HasTakenLockAction = true;
                //}
                return mbSession.Query(command + "\n").TrimEnd("\r\n".ToCharArray());
                
            }
            catch (Exception ex)
            {
                if (ex.Message.ToString().Contains("ErrorResourceLocked"))
                {
                    UnLock();
                    Close();
                    return Query(command);
                }
                else
                {
                    IsConnected = false;
                    throw new ApplicationException("Remote Error-->" + command + ":  " + ex.Message.ToString());
                }
            }
             finally
            {
                if (HasTakenLockAction) UnLock();
            }
        }

        public void Close()
        {
            try
            {
                IsConnected = false;
                if (mbSession == null) return;
                mbSession.Clear();
                mbSession.Dispose();
            }
            catch{};
        }

        public void Wait(string command, string flage,int Cycles=50,int delayTime=200)
        {
            string currentStatus;
            int cycleCount=0;
            do
            {
                iStopwatch=Stopwatch.StartNew();
                do
                {
                    Application.DoEvents();
                    Thread.Sleep(20);
                } while (iStopwatch.ElapsedMilliseconds < delayTime);
                currentStatus = Query(command);
                cycleCount++;
            } while (String.Compare(currentStatus, flage, false) != 0 && cycleCount < Cycles);
        }

        public bool CheckMeasureStatus(string command, string flage)
        {
            return (String.Compare(Query(command), flage, false)==0? true:false); 
        }

        public void Lock()
        {
            try
            {
                if (!IsConnected) OpenInstrment();
                iStopwatch = Stopwatch.StartNew();

                //while (mbSession.ResourceLockState != AccessModes.NoLock && iStopwatch.ElapsedMilliseconds <= 300000)
                //{
                //    Application.DoEvents();
                //    if (IsStop) throw new Exception("User Stop!");
                //    Thread.Sleep(iRandom.Next(100));//
                //    Thread.SpinWait(iRandom.Next(1000000,2000000));
                //} 
                try
                {
                    mbSession.LockResource();
                    IsLocked = true;
                }
                catch (Exception ex)
                {
                    if (ex.Message.ToString().Contains("ErrorResourceLocked"))
                    {
                        UnLock();
                        Close();
                        ex.ToString();
                        IsLocked = false;
                        Lock();
                    }
                    else throw new Exception(ex.Message.ToString());
                   
                }
            }
            catch (Exception ex)
            {
                IsLocked = false;
                throw new Exception(ex.Message + ": " + "Faild to lock instrument resource!");
            }

        }

        public void UnLock()
        {
            //if (!IsConnected) OpenInstrment();
            try
            {
                if (mbSession != null && IsLocked==true) mbSession.UnlockResource();
                //if (mbSession != null && mbSession.ResourceLockState != AccessModes.NoLock) mbSession.UnlockResource();
            }
            catch {}
            finally { IsLocked = false; Thread.SpinWait(iRandom.Next(1000000,2000000)); }
        }
    }
}
