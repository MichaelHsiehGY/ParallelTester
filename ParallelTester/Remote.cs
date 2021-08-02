using System;
using System.Collections.Generic;
using System.Text;

namespace ParallelTester
{
    class Remote
    {
        public string resourceName = "";
        bool IsConnected = false;
        private NIRemote iNIRemote = new NIRemote();
        private SocketRemote iSocketRemote = new SocketRemote();
        private bool _IsStop = false;


        public bool IsStop
        {
            get { return _IsStop; }
            set { _IsStop = value; }
        }

        public bool OpenInstrment()
        {
            try
            {
                if (resourceName.Contains("::"))
                {
                    iNIRemote.resourceName = resourceName;
                    iNIRemote.OpenInstrment();
                }
                else
                {
                    iSocketRemote.resourceName = resourceName;
                    iSocketRemote.OpenInstrment();
                }
                IsConnected = true;
                Query("*IDN?");
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
            try
            {
                if (!IsConnected) OpenInstrment();
                if (resourceName.Contains("::")) iNIRemote.Write(command);
                else iSocketRemote.Write(command);

            }
            catch (Exception ex)
            {
                IsConnected = false;
                throw new ApplicationException("Remote Error -->" + command + ":  " + ex.Message.ToString());
            }
        }

        public string Query(string command)
        {
            try
            {
                if (!IsConnected) OpenInstrment();
                if (resourceName.Contains("::")) return iNIRemote.Query(command);
                else return iSocketRemote.Query(command);
            }
            catch (Exception ex)
            {
                IsConnected = false;
                throw new ApplicationException("Remote Error-->" + command + ":  " + ex.Message.ToString());
            }
        }

        public void Close()
        {
            if (resourceName.Contains("::")) iNIRemote.Close();
            else iSocketRemote.Close();
            IsConnected = false;
        }

        public bool CheckMeasureStatus(string command, string flage)
        {
            string tStr = "";
            if (resourceName.Contains("::"))
            {
                tStr = iNIRemote.Query(command);
                return (String.Compare(tStr, flage, false) == 0 ? true : false);
            }
            else
            {
                return (String.Compare(iSocketRemote.Query(command).TrimEnd('\n'), flage, false) == 0 ? true : false);
            }
        }

        public bool CheckMeasureStatus1(string command, string flage)
        {
            if (resourceName.Contains("::"))
                return (String.Compare(iNIRemote.Query(command), flage, false) == 0 ? true : false);
            else return (String.Compare(iSocketRemote.Query(command), flage, false) == 0 ? true : false);
        }


    }
}
