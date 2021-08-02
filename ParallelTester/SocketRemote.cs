using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using iSocketAgentClientDll;

namespace ParallelTester
{
    class SocketRemote
    {
        public string resourceName = "";
        private delegate void iDelegate(string ex);
        iSocketAgentClientDll.Base iRemote = new Base();
        bool IsConnected = false;

        #region Remote

        public bool OpenInstrment()
        {
            try
            {
                iRemote.Connect(resourceName);
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
               iRemote.Write(command);
              
            }
            catch (Exception ex)
            {
                IsConnected = false;
                throw new ApplicationException("Remote Error -->" + command +":  " + ex.Message.ToString());
            }
        }

        public string Query(string command)
        {
            try
            {
                if (!IsConnected) OpenInstrment();
                return iRemote.Query(command);
            }
            catch (Exception ex)
            {
                IsConnected = false;
                throw new ApplicationException("Remote Error-->" + command + ":  " + ex.Message.ToString());
            }
        }

        public void Close()
        {
            if (iRemote!=null) iRemote.Close();
            IsConnected = false;
        }

        #endregion
    }
}
