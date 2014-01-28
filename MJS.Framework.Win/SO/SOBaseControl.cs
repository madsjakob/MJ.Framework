using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MJS.Framework.Win.Delegates;
using MJS.Framework.Win.Enums;

namespace MJS.Framework.Win.SO
{
    public partial class SOBaseControl : UserControl
    {
        public SOBaseControl()
        {
            InitializeComponent();
        }

        public virtual void SaveSettings()
        {

        }

        public virtual void LoadSettings()
        {

        }

        private bool _updating;
        private bool _isLoaded = false;
        public bool IsLoaded 
        {
            get { return _isLoaded; }
        }

        protected void BindData(Control control, object data, string property)
        {
            string controlProperty = "Text";
            if (control is ComboBox)
            {
                controlProperty = "SelectedItem";
            }
            else if (control is RichTextBox)
            {
                controlProperty = "Rtf";
            }
            else if (control is CheckBox)
            {
                controlProperty = "Checked";
            }
            else if (control is DateTimePicker)
            {
                controlProperty = "Value";
            }
            Binding binding = new Binding(controlProperty, data, property);
            control.DataBindings.Add(binding);
        }

        public virtual void SetData(object data)
        {
        }

        public void SetEventHandler(CustomEventHandler eventHandler)
        {
            EventHandler += eventHandler;
            foreach (Control control in Controls)
            {
                if (control is SOBaseControl)
                {
                    (control as SOBaseControl).SetEventHandler(eventHandler);
                }
                else if (control is ComboBox)
                {
                    (control as ComboBox).SelectedIndexChanged += ChangeEventHandler;
                }
                else if (control is RichTextBox)
                {
                    //(control as RichTextBox).Leave += ChangeEventHandler;
                    (control as RichTextBox).TextChanged += ChangeEventHandler;
                }
                else if (control is TextBox)
                {
                    (control as TextBox).TextChanged += ChangeEventHandler;
                }
                else if (control is CheckBox)
                {
                    (control as CheckBox).CheckedChanged += ChangeEventHandler;
                }
                else if (control is DateTimePicker)
                {
                    (control as DateTimePicker).ValueChanged += ChangeEventHandler;
                }
                else if (control is RadioButton)
                {
                    (control as RadioButton).CheckedChanged += ChangeEventHandler;
                }
            }
        }

        private void ChangeEventHandler(object sender, EventArgs ea)
        {
            if (sender is Control)
            {
                foreach (Binding binding in (sender as Control).DataBindings)
                {
                    binding.WriteValue();
                }
            }
            Notify();
        }
        

        internal void RemoveEventHandler(CustomEventHandler eventHandler)
        {
            EventHandler -= eventHandler;
            foreach (Control control in Controls)
            {
                if (control is SOBaseControl)
                {
                    (control as SOBaseControl).RemoveEventHandler(eventHandler);
                }
            }
        }

        public void DispatchEvent(Enum action)
        {
            if (action is ControlEvent)
            {
                if (_updating)
                {
                    return;
                }
                if ((ControlEvent)action == ControlEvent.Update)
                {
                    _updating = true;
                    try
                    {
                        if (_eventHandler != null)
                        {
                            _eventHandler(this, action);
                        }
                    }
                    finally
                    {
                        _updating = false;
                    }
                }
                else
                {
                    if (_eventHandler != null)
                    {
                        _eventHandler(this, action);
                    }
                }

            }
            else
            {
                if (_eventHandler != null)
                {
                    _eventHandler(this, action);
                }
            }
        }

        private event CustomEventHandler _eventHandler;
        public event CustomEventHandler EventHandler
        {
            add { _eventHandler += value; }
            remove { _eventHandler -= value; }
        }

        public static void WriteData(Control parent)
        {
            foreach (Control control in parent.Controls)
            {
                WriteData(control);
            }
            foreach (Binding binding in parent.DataBindings)
            {
                binding.WriteValue();
            }
        }

        public static void ReadData(Control parent)
        {
            foreach (Control control in parent.Controls)
            {
                ReadData(control);
            }
            foreach (Binding binding in parent.DataBindings)
            {
                binding.ReadValue();
            }
        }

        

        public void WriteValues()
        {
            WriteData(this);
        }



        public void Notify(ControlEvent action = ControlEvent.Change)
        {
            DispatchEvent(action);
        }

        private void SOBaseControl_Load(object sender, EventArgs e)
        {
            _isLoaded = true;
        }
    }
}
