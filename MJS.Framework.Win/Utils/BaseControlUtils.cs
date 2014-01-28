using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MJS.Framework.Win.Utils
{
    public class BaseControlUtils
    {
        public static void UpdateControlData(Control control)
        {
            for (int bindingIndex = 0; bindingIndex < control.DataBindings.Count; bindingIndex++)
            {
                control.DataBindings[bindingIndex].ReadValue();
            }
            for (int controlIndex = 0; controlIndex < control.Controls.Count; controlIndex++)
            {
                UpdateControlData(control.Controls[controlIndex]);
            }
        }

        public static void UpdateObjectData(Control control)
        {
            for (int bindingIndex = 0; bindingIndex < control.DataBindings.Count; bindingIndex++)
            {
                control.DataBindings[bindingIndex].WriteValue();
            }
            for (int controlIndex = 0; controlIndex < control.Controls.Count; controlIndex++)
            {
                UpdateObjectData(control.Controls[controlIndex]);
            }
        }

    }
}
