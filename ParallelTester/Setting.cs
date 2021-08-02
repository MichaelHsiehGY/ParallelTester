using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace ParallelTester
{
    class Setting
    {
        private string _ChannelList= "";
        private bool _IsEnableSN = true;
        private bool _IsAllowManualInput_SN = false;
        private string _characterCaseType_SN = "Normal";


        [Description("设置是否使用SN")]
        public bool IsEnableSN
        {
            get { return _IsEnableSN; }
            set { _IsEnableSN = value; }
        }


        public string ChannelList
        {
            get { return _ChannelList; }
            set { _ChannelList = value; }
        }

        [Description("设置是否允许手动输入SN")]
        public bool IsAllowManualInput_SN
        {
            get { return _IsAllowManualInput_SN; }
            set { _IsAllowManualInput_SN = value; }
        }

        [Browsable(false)]
        public string CharacterCaseType_SN
        {
            get { return _characterCaseType_SN; }
            set { _characterCaseType_SN = value; }
        }
    }
}
