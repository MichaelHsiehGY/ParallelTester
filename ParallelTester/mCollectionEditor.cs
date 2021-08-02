using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Globalization;
using System.Drawing.Design;
using System.Reflection;
using System.Windows.Forms;
using System.Collections;

namespace ParallelTester
{

    public class mCollectionEditor : CollectionEditor
    {

        public delegate void mPropertyValueChangedEventHandler(object sender, PropertyValueChangedEventArgs e);
        public static event PropertyValueChangedEventHandler PropertyValueChanged;

        public delegate void AcceptButton_EventHandler(object sender, EventArgs e);
        public static event AcceptButton_EventHandler AcceptButton_Click;

        public mCollectionEditor(Type type): base(type)
        {

        }

        protected override CollectionForm CreateCollectionForm()
        {
            CollectionForm collectionForm = base.CreateCollectionForm();

            Form frm = collectionForm as Form;


            if (frm != null)
            {
                // Get OK button of the Collection Editor...
                Button button = frm.AcceptButton as Button;
                // Handle click event of the button
                button.Click += new EventHandler(button_Click);
            }

            //////////test//////////////

            TableLayoutPanel tlpLayout = frm.Controls[0] as TableLayoutPanel;

            if (tlpLayout != null)
            {
                // Get a reference to the inner PropertyGrid and hook
                // an event handler to it.
                if (tlpLayout.Controls[5] is PropertyGrid)
                {
                    PropertyGrid propertyGrid = tlpLayout.Controls[5] as PropertyGrid;
                    propertyGrid.HelpVisible = true;

                    propertyGrid.PropertyValueChanged += new PropertyValueChangedEventHandler(propertyGrid_PropertyValueChanged);
                }

            }



            ///////////////////////////


            return collectionForm;
        }

        void propertyGrid_PropertyValueChanged(object sender, PropertyValueChangedEventArgs e)
        {
            // Fire our customized collection event...
            if (mCollectionEditor.PropertyValueChanged != null)
            {
                mCollectionEditor.PropertyValueChanged(this, e);
            }
        }

        void button_Click(object sender, EventArgs e)
        {
            //expose the click event of AcceptButton
            if (mCollectionEditor.AcceptButton_Click != null)
            {
                mCollectionEditor.AcceptButton_Click(this, e);
            }
        }

        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            return base.EditValue(context, provider, value);
        }

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal; // disallow edit (hide the small browser button)
        }


        protected override object CreateInstance(Type itemType)
        {
            return base.CreateInstance(itemType);
        }
        protected override void DestroyInstance(object instance)
        {
            base.DestroyInstance(instance);
        }

        protected override object SetItems(object editValue, object[] value)
        {
            return base.SetItems(editValue, value);
        }

    }


}
