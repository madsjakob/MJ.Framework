using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MJS.Framework.Win.SO
{
    public partial class SOSearchForm : SOBaseForm
    {
        public SOSearchForm()
        {
            InitializeComponent();
        }

        public Enum DoubleClickEvent
        {
            get { return soSearchControl1.DoubleClickEvent; }
            set { soSearchControl1.DoubleClickEvent = value; }
        }

        public DataGridView Grid
        {
            get { return soSearchControl1.Grid; }
        }

        public void FilterGrid()
        {
            soSearchControl1.FilterGrid();
        }
    }
}
