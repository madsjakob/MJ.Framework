using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using MJS.Framework.Data.Metadata;
using System.Xml.Serialization;

namespace MJS.Framework.Win.SO
{
    public partial class SOEditControl : SOBaseControl
    {
        public SOEditControl()
        {
            InitializeComponent();
        }

        private const int LabelX = 0;
        private const int EditX = 150;

        public override void SetData(object data)
        {
            base.SetData(data);
            if (data != null)
            {
                int y = 0;
                Type dataType = data.GetType();
                PropertyInfo[] propertyList = dataType.GetProperties();
                foreach (PropertyInfo property in propertyList)
                {
                    XmlIgnoreAttribute xmlAtt = (XmlIgnoreAttribute)XmlIgnoreAttribute.GetCustomAttribute(property, typeof(XmlIgnoreAttribute));
                    if (xmlAtt != null)
                    {
                        continue;
                    } 
                    DatabaseKeyAttribute keyAtt = (DatabaseKeyAttribute)DatabaseKeyAttribute.GetCustomAttribute(property, typeof(DatabaseKeyAttribute));
                    if (keyAtt != null)
                    {
                        continue;
                    }
                    
                    Label label = new Label();
                    label.Width = 150;
                    label.Text = property.Name;
                    label.Parent = this;
                    label.Location = new Point(LabelX, y);
                    Control control;
                    Type propertyType = property.PropertyType;
                    if (propertyType == typeof(bool))
                    {
                        control = new CheckBox();
                    }
                    else if (propertyType == typeof(int) || propertyType == typeof(double))
                    {
                        control = new TextBox();
                        (control as TextBox).TextAlign = HorizontalAlignment.Right;
                    }
                    else if (propertyType == typeof(DateTime))
                    {
                        control = new DateTimePicker();
                    }
                    else if (propertyType.IsEnum)
                    {
                        control = new ComboBox();
                        (control as ComboBox).DataSource = Enum.GetValues(propertyType);
                        (control as ComboBox).DropDownStyle = ComboBoxStyle.DropDownList;
                    }
                    else
                    {
                        control = new TextBox();
                    }
                    control.Parent = this;
                    control.Location = new Point(EditX, y);
                    control.Width = Width - EditX - 10;
                    BindData(control, data, property.Name);
                    y += label.Height;
                }
            }
        }
    }
}
