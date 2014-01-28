using MJS.Framework.Data.Interfaces;
using MJS.Framework.Data.Types;
using MJS.Framework.Win.FO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MJS.Framework.Win.Enums;

namespace MJS.Framework.Win.SO
{
    public partial class SOSearchControl : SOBaseControl
    {
        public SOSearchControl()
        {
            InitializeComponent();
            DoubleClickEvent = ListEvent.Edit;
        }

        public Enum DoubleClickEvent { get; set; }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {            
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Enter)
            {
                if (tstbSearch.Focused)
                {
                    dgvGrid.Focus();
                }
                else
                {
                    DispatchEvent(DoubleClickEvent);
                }
                keyData = Keys.None;
            }
            if (keyData == Keys.F3)
            {
                tstbSearch.Focus();
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        public DataGridView Grid
        {
            get { return dgvGrid; }
        }

        private void tsbNew_Click(object sender, EventArgs e)
        {
            DispatchEvent(ListEvent.New);
        }

        private void tsbCopy_Click(object sender, EventArgs e)
        {
            DispatchEvent(ListEvent.Copy);
        }

        private void tsbEdit_Click(object sender, EventArgs e)
        {
            DispatchEvent(ListEvent.Edit);
        }

        private void tsbDelete_Click(object sender, EventArgs e)
        {
            DispatchEvent(ListEvent.Delete);
        }

        private void dgvGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvGrid.SelectedRows.Count == 1)
            {
                DispatchEvent(DoubleClickEvent);
            }
        }

        public void FilterGrid()
        {
            FilterGrid(tstbSearch.Text);
        }

        private void FilterGrid(string text)
        {
            SuspendLayout();
            try
            {
                bool rowPositionSat = false;
                bool firstFound = false;
                bool found = false;
                for (int rowIndex = 0; rowIndex < dgvGrid.Rows.Count; rowIndex++)
                {
                    found = string.IsNullOrEmpty(text);
                    if (!found)
                    {
                        for (int colIndex = 0; colIndex < dgvGrid.Columns.Count; colIndex++)
                        {
                            if ((dgvGrid.Columns[colIndex].Visible)
                                && (dgvGrid.Rows[rowIndex].Cells[colIndex].ValueType == typeof(string))
                                && ((dgvGrid.Rows[rowIndex].Cells[colIndex].Value.ToString().ToUpper()).Contains(text.ToUpper())))
                            {
                                found = true;
                                break;
                            }
                        }
                    }
                    dgvGrid.CurrentCell = null;
                    if (dgvGrid.Rows[rowIndex].Selected)
                    {
                        dgvGrid.Rows[rowIndex].Selected = found;
                    }
                    dgvGrid.Rows[rowIndex].Visible = found;
                    if (!firstFound)
                    {
                        dgvGrid.Rows[rowIndex].Selected = true;
                        firstFound = true;
                    }
                    if (!rowPositionSat)
                    {
                        rowPositionSat = true;
                    }
                }
                foreach (DataGridViewRow row in dgvGrid.SelectedRows)
                {
                    row.Selected = row.Visible;
                }
            }
            finally
            {
                ResumeLayout();
            }
        }

        private void SearchContent(object sender, EventArgs e)
        {
            searchTimer.Enabled = false;
            searchTimer.Enabled = true;
        }

        private void SearchGrid(object sender, EventArgs e)
        {
            searchTimer.Enabled = false;
            FilterGrid();
        }

        private void tsbSearch_Click(object sender, EventArgs e)
        {
            tstbSearch.Focus();
        }

        private void dgvGrid_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
            }
        }
    }
}
