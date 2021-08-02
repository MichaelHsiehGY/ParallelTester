using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.Design;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Drawing.Design;
using System.Reflection;
using System.Windows.Forms;
using System.Diagnostics;


namespace ParallelTester
{
    public class Config
    {
        private string _ChannelName = "";
        private bool _IsMultiShare = true;
        private string _Interface = "Ethernet";
        private int _GPIB_Card_Number = 0;
        private int _GPIB_Address = 0;
        private string _IP_Address = "";
        private string _MeasureItem = "BER";
        private int _MoudleID = 1;
        private string _ScopeChannel = "B";
        private AmplitudeTimeTest _AmplitudeTimeTest = new AmplitudeTimeTest();
        private MaskTest _MaskTest = new MaskTest();
       


        [Browsable(true)]
        [Description("自定义通道显示名称")]
        public String ChannelName
        {
            get { return _ChannelName; }
            set { _ChannelName = value; }
        }

        [Description("设置是否与其他通道共用同一台仪表")]
        public bool IsMultiShare
        {
            get { return _IsMultiShare; }
            set { _IsMultiShare = value; }
        }

     [CategoryAttribute("Interface")]
      [Description("选择仪器控制方式：GPIB|Ethernet")]
      [TypeConverter(typeof(Interface_Converter))]
      //[RefreshProperties(System.ComponentModel.RefreshProperties.All)]
        public String Interface
        {
            get { return _Interface; }
            set { _Interface = value; }
        }

      [CategoryAttribute("GPIB")]
      [Description("选择GPIB板卡卡号")]
      [TypeConverter(typeof(GPIBCard_Number_Converter))]
        public int GPIB_Card_Number
        {
            get { return _GPIB_Card_Number; }
            set { _GPIB_Card_Number = value; }
        }


        [CategoryAttribute("GPIB")]
        [Description("输入仪器的GPIB地址。\n\n如果同时连接多台电源，必须确保每台电源的GPIB地址不相同，\n否则会引起控制冲突。")]
        [TypeConverter(typeof(GPIB_Number_Converter))]
        public int GPIB_Address
        {
            get { return _GPIB_Address; }
            set { _GPIB_Address = value; }
        }


        [CategoryAttribute("Ethernet")]
      [Description("输入仪器IP地址")]
        public string IP_Address
        {
            get { return _IP_Address; }
            set { _IP_Address = value; }
        }


      [TypeConverter(typeof(MeasureItem_Converter))]
        [Description("选择BER或者Scope测试")]
        public string MeasureItem
        {
            get { return _MeasureItem; }
            set { _MeasureItem = value; }
        }

        [CategoryAttribute("BER")]
      [Description("选择MP2100A上对应的模块ID")]
      public int MoudleID
      {
          get { return _MoudleID; }
          set { _MoudleID = value; }
      }


        [CategoryAttribute("Scope")]
        [TypeConverter(typeof(ScopeChannel_Converter))]
        [Description("设置眼图测试通道，电口A|光口B")]
        public string ScopeChannel
        {
            get { return _ScopeChannel; }
            set { _ScopeChannel = value; }
        }

      [CategoryAttribute("Scope")]
      [Description("设置AmplitudeTime测试")]
      public AmplitudeTimeTest AmplitudeTimeTest
      {
          get { return _AmplitudeTimeTest; }
          set { _AmplitudeTimeTest = value; }
      }

      [CategoryAttribute("Scope")]
      [Description("设置Mask测试")]
      public MaskTest MaskTest
      {
          get { return _MaskTest; }
          set { _MaskTest = value; }
      }


    }





    
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class AmplitudeTimeTest
    {
        private bool _Check = true;
        private AmplitudeTests _AmplitudeTests = new AmplitudeTests();
        private TimeTests _TimeTests = new TimeTests();



        [Description("是否选中这个测试")]
        public bool Check
        {
            get { return _Check; }
            set { _Check = value; }
        }

         [TypeConverter(typeof(ListConverter))]
         [Description("设置Amplitude测试")]
        public AmplitudeTests AmplitudeTests
        {
            get { return _AmplitudeTests; }
            set { _AmplitudeTests = value; }
        }

         [TypeConverter(typeof(ListConverter))]
        [Description("设置Time测试")]
         public TimeTests TimeTests
        {
            get { return _TimeTests; }
            set { _TimeTests = value; }
        }

         public override string ToString()
         {
             return "";
         }

    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class MaskTest
    {
        private bool _Check = true;
        private string _UpLimit = "N/A";
        private string _DownLimit = "N/A";
        private string _Result= "N/A";
        private bool _IsPass = false;


        [Description("是否选中这个测试模式")]
        public bool Check
        {
            get { return _Check; }
            set { _Check = value; }
        }

        [Browsable(false)]
        public string Result
        {
            get { return _Result; }
            set { _Result = value; IsPass = CheckResult(); }
        }

        [Description("设置下限指标")]
        public string DownLimit
        {
            get { return _DownLimit; }
            set { _DownLimit = value; }
        }

        [Description("设置上限指标")]
        public string UpLimit
        {
            get { return _UpLimit; }
            set { _UpLimit = value; }
        }

        public override string ToString()
        {
            //return base.ToString();
            return "";
        }

        [Browsable(false)]
        public bool IsPass
        {
            get { return _IsPass; }
            set { _IsPass = value; }
        }

        private bool CheckResult()
        {
            try
            {
                if (UpLimit != "N/A")
                    if (Convert.ToDouble(Result) > Convert.ToDouble(UpLimit)) return false;
                if (DownLimit != "N/A")
                    if (Convert.ToDouble(Result) < Convert.ToDouble(DownLimit)) return false;
            }
            catch { return false; }

            return true;
        }

    }

    [EditorAttribute(typeof(mCollectionEditor), typeof(System.Drawing.Design.UITypeEditor))]
    [TypeConverter(typeof(ListConverter))]
    [Description("设置Amplitude测试")]
    public class AmplitudeTests : CollectionBase
    {
        public AmplitudeTests()
        {
        }

        public void Add(AmplitudeTest newSegment)
        {
            this.List.Add(newSegment);
        }
        public void Remove(AmplitudeTest newSegment)
        {
            this.List.Remove(newSegment);
        }


        public AmplitudeTest this[int index]
        {
            get
            {
                return (AmplitudeTest)this.List[index];
            }
        }

        public override string ToString()
        {
            //return base.ToString();
            return "";
        }
    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class AmplitudeTest
    {
        private string _Name = "";
        private string _UpLimit = "N/A";
        private string _DownLimit = "N/A";
        private string _Result = "N/A";
        private bool _IsPass = false;



        [TypeConverter(typeof(TestItem_Amplitude_Converter))]
        [Description("选择测试项目")]
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        [Browsable(false)]
        public string Result
        {
            get { return _Result; }
            set { _Result = value; IsPass = CheckResult(); }
        }


        [Description("设置上限指标")]
        public string UpLimit
        {
            get { return _UpLimit; }
            set { _UpLimit = value; }
        }

        [Description("设置下限指标")]
        public string DownLimit
        {
            get { return _DownLimit; }
            set { _DownLimit = value; }
        }


        [Browsable(false)]
        public bool IsPass
        {
            get { return _IsPass; }
            set { _IsPass = value; }
        }


        public override string ToString()
        {
            //return base.ToString();
            return Name;
        }


        private bool CheckResult()
        {
            try
            {
                if (UpLimit != "N/A")
                    if (Convert.ToDouble(Result) > Convert.ToDouble(UpLimit)) return false;
                if (DownLimit != "N/A")
                    if (Convert.ToDouble(Result) < Convert.ToDouble(DownLimit)) return false;
            }
            catch { return false; }

            return true;
        }
















    }
    

    [EditorAttribute(typeof(mCollectionEditor), typeof(System.Drawing.Design.UITypeEditor))]
    [TypeConverter(typeof(ListConverter))]
    [Description("设置Time测试")]
    public class TimeTests : CollectionBase
    {
        public TimeTests()
        {
        }

        public void Add(TimeTest newSegment)
        {
            this.List.Add(newSegment);
        }
        public void Remove(TimeTest newSegment)
        {
            this.List.Remove(newSegment);
        }


        public TimeTest this[int index]
        {
            get
            {
                return (TimeTest)this.List[index];
            }
        }

        public override string ToString()
        {
            //return base.ToString();
            return "";
        }
    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class TimeTest
    {
        private string _Name = "";
        private string _UpLimit = "N/A";
        private string _DownLimit = "N/A";
        private string _Result = "N/A";
        private bool _IsPass = false;



        [TypeConverter(typeof(TestItem_Time_Converter))]
        [Description("选择测试项目")]
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        [Browsable(false)]
        public string Result
        {
            get { return _Result; }
            set { _Result = value; IsPass = CheckResult(); }
        }

        
        [Description("设置上限指标")]
        public string UpLimit
        {
            get { return _UpLimit; }
            set { _UpLimit = value; }
        }

        [Description("设置下限指标")]
        public string DownLimit
        {
            get { return _DownLimit; }
            set { _DownLimit = value; }
        }


        [Browsable(false)]
        public bool IsPass
        {
            get { return _IsPass; }
            set { _IsPass = value; }
        }


        public override string ToString()
        {
            //return base.ToString();
            return Name;
        }


        private bool CheckResult()
        {
            try
            {
                if (UpLimit != "N/A")
                    if (Convert.ToDouble(Result) > Convert.ToDouble(UpLimit)) return false;
                if (DownLimit != "N/A")
                    if (Convert.ToDouble(Result) < Convert.ToDouble(DownLimit)) return false;
            }
            catch { return false; }

            return true;
        }
















    }

    #region Converter


    public class Time_Number_Converter : Int32Converter
    {

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            var result = (int)base.ConvertFrom(context, culture, value);
            if (result < 0)
            {
                throw new Exception("输入值要大于0。");
            }

            return result;
        }
    }


    public class Cycle_Number_Converter : Int32Converter
    {

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            var result = (int)base.ConvertFrom(context, culture, value);
            if (result < 1)
            {
                throw new Exception("输入值要大于1。");
            }

            return result;
        }
    }


    public class GPIBCard_Number_Converter : Int32Converter
    {

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            var result = (int)base.ConvertFrom(context, culture, value);
            if (result < 0 || result > 30)
            {
                throw new Exception("输入值要在1至30之间。");
            }

            return result;
        }
    }


    public class GPIB_Number_Converter : Int32Converter
    {

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            var result = (int)base.ConvertFrom(context, culture, value);
            if (result < 1 || result > 30)
            {
                throw new Exception("输入值要在1至30之间。");
            }

            return result;
        }
    }

    public class ListConverter : CollectionConverter
    {
        public override bool GetPropertiesSupported(ITypeDescriptorContext context)
        {
            return true;
        }


        public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
        {
            IList list = value as IList;
            if (list == null || list.Count == 0)
                return base.GetProperties(context, value, attributes);

            var items = new PropertyDescriptorCollection(null);
            for (int i = 0; i < list.Count; i++)
            {
                object item = list[i];
                items.Add(new ExpandableCollectionPropertyDescriptor(list, i));
            }
            return items;
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            //return "";
            //if (destinationType == typeof(string))
            //{
            //    if (value is CollectionBase) return "Add/Remove Items";
            //}

            //return "添加/删除测试项目";

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }





    public class Interface_Converter : StringConverter
    {
        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(StringArray);
        }
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return true;
        }
        public static string[] StringArray = new[] { "GPIB","Ethernet" };
    }

    public class TestItem_Amplitude_Converter : StringConverter
    {
        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(StringArray);
        }
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return true;
        }
        public static string[] StringArray = new[] { "One Level", "Zero Level", "Eye amplitude", "Eye height", "Crossing", "SNR", "Average Power", "Extinction Ratio", "OMA (dBm)" };


    }

    public class TestItem_Time_Converter : StringConverter
    {
        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(StringArray);
        }
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return true;
        }
        public static string[] StringArray = new[] { "Jitter p-p", "Jitter RMS", "Rise Time", "Fall Time", "Eye Width", "DCD" };
    }

    public class ScopeChannel_Converter : StringConverter
    {
        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(StringArray);
        }
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return true;
        }
        public static string[] StringArray = new[] { "A", "B"};
    }

    public class TestMode_Converter : StringConverter
    {
        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(StringArray);
        }
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return true;
        }
        public static string[] StringArray = new[] { "AmplitudeTime", "Mask" };
    }

    public class MeasureItem_Converter : StringConverter
    {
        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(StringArray);
        }
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return true;
        }
        public static string[] StringArray = new[] { "BER", "Scope" };
    }

    public class TimeUnit_Converter : StringConverter
    {
        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(StringArray);
        }
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return true;
        }
        public static string[] StringArray = new[] { "天", "时", "分", "秒"};
    }


    #endregion

    #region Editor


   


    #endregion

    #region Descriptor

    public class ExpandableCollectionPropertyDescriptor : PropertyDescriptor
    {
        private IList _collection;

        private readonly int _index = -1;

        internal event EventHandler RefreshRequired;

        public ExpandableCollectionPropertyDescriptor(IList coll, int idx): base(GetDisplayName(coll, idx), null)
        {
            _collection = coll;
            _index = idx;
        }

        public override bool SupportsChangeEvents
        {
            get { return true; }
        }

        private static string GetDisplayName(IList list, int index)
        {

            return list[index].GetType().Name + (index + 1).ToString();
            //return "[" + index + "]  " + CSharpName(list[index].GetType());
        }

 

        public override AttributeCollection Attributes
        {
            get
            {
                return new AttributeCollection(null);
            }
        }

        public override bool CanResetValue(object component)
        {

            return true;
        }

        public override Type ComponentType
        {
            get
            {
                return _collection.GetType();
            }
        }

        public override object GetValue(object component)
        {
            return _collection[_index];
        }

        public override bool IsReadOnly
        {
            get { return false; }
        }

        public override string Name
        {
            get { return _index.ToString(); }
        }

        public override Type PropertyType
        {
            get { return _collection[_index].GetType(); }
        }

        public override void ResetValue(object component)
        {
        }

        public override bool ShouldSerializeValue(object component)
        {
            return true;
        }

        public override void SetValue(object component, object value)
        {
            _collection[_index] = value;
        }

        protected virtual void OnRefreshRequired()
        {
            var handler = RefreshRequired;
            if (handler != null) handler(this, EventArgs.Empty);
        }
    }

   

    #endregion
}
