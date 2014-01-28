using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MJS.Framework.Communication.CO;
using MJS.Framework.Data.CO;
using MJS.Framework.Data.Metadata;
using MJS.Framework.Data.Types;
using MJS.Framework.Win.Enums;
using MJS.Framework.Win.SO;
using MJS.Framework.Win.CO;
using System.ComponentModel;

namespace MJS.Framework.Win.FO
{
    public abstract class FOEntity<T> : FODataClass where T: Entity, new()
    {
        public FOEntity() : base()
        {
            DatabaseKeyAttribute keyAttribute = DatabaseKeyAttribute.GetKeyAttribute(typeof(T));
            if (keyAttribute != null)
            {
                KeyColumn = keyAttribute.FieldName;
            }
        }

        public new T Data
        {
            get { return (T)base.Data; }
            set { base.Data = value; }
        }

        protected override void Register()
        {
            FODataClass.RegisterFlow(typeof(T), GetType());
        }

        protected virtual void LoadListData()
        {
            CODataList.FillGrid(_searchForm.Grid, typeof(T));
        }

        private string KeyColumn { get; set; }

        protected override void EditCreate(SOBaseControl control)
        {
        }

        protected override void EditChange(SOBaseControl control)
        {
            
        }

        protected override void EditUpdate(SOBaseControl control)
        {
            _editForm.Text = Data.GetDisplayName();
            if (control != null)
            {
                control.SetData(Data);
            }
        }

        protected override void EditDestroy(SOBaseControl control)
        {
        }

        protected override void EditSave(SOBaseControl control)
        {
            Data.Save();
            SOBaseForm.UpdateAll();
        }

        protected override void ListCreate(SOBaseControl control)
        {
        }

        protected override void ListChange(SOBaseControl control)
        {
        }

        protected override void ListUpdate(SOBaseControl control)
        {
            DataGridViewColumn sortedColumn = _searchForm.Grid.SortedColumn;
            SortOrder sortOrder = _searchForm.Grid.SortOrder;
            List<int> selectedRows = new List<int>();
            foreach (DataGridViewRow row in _searchForm.Grid.SelectedRows)
            {
                selectedRows.Add(row.Index);
            }
            
            LoadListData();
            if (sortedColumn != null)
            {
                _searchForm.Grid.Sort(_searchForm.Grid.Columns[sortedColumn.Name], (sortOrder == SortOrder.Ascending ? ListSortDirection.Ascending : ListSortDirection.Descending));
                foreach (DataGridViewRow row in _searchForm.Grid.SelectedRows)
                {
                    row.Selected = false;
                }
                foreach (int index in selectedRows)
                {
                    _searchForm.Grid.Rows[index].Selected = true;
                }
            }
            _searchForm.FilterGrid();
        }

        protected override void ListDestroy(SOBaseControl control)
        {
        }

        protected override void ListNew(SOBaseControl control)
        {

            
            T temp = new T();
            if (NewEntity(temp))
            {
                FODataClass.Edit(temp);
            }
        }

        protected virtual bool NewEntity(T newEntity)
        {
            return true;
        }

        protected override void ListCopy(SOBaseControl control)
        {
            if (_searchForm.Grid.SelectedRows.Count > 0)
            {
                foreach (DataGridViewRow selectedRow in _searchForm.Grid.SelectedRows)
                {
                    T source = new T();
                    source.SetID((Guid)selectedRow.Cells[KeyColumn].Value);
                    source.Load();
                    source.SetID(Guid.NewGuid());
                    source.Save();
                }
                SOBaseForm.UpdateAll();
            }
        }

        protected override void ListEdit(SOBaseControl control)
        {
            foreach (DataGridViewRow selectedRow in _searchForm.Grid.SelectedRows)
            {
                T temp = new T();
                temp.SetID((Guid)selectedRow.Cells[KeyColumn].Value);
                temp.Load();
                FODataClass.Edit(temp);
            }
        }

        protected override void ListDelete(SOBaseControl control)
        {
            if (_searchForm.Grid.SelectedRows.Count > 0 && MessageBox.Show("Are you sure you want to delete " + _searchForm.Grid.SelectedRows.Count + " items?", "Delete?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                foreach (DataGridViewRow selectedRow in _searchForm.Grid.SelectedRows)
                {
                    T temp = new T();
                    temp.SetID((Guid)selectedRow.Cells[KeyColumn].Value);
                    temp.Delete();
                }
                SOBaseForm.UpdateAll();
            }
        }

        protected override void ListChoose(SOBaseControl control)
        {
            if (_searchForm.Grid.SelectedRows.Count == 1)
            {
                _selectedID = (Guid)_searchForm.Grid.SelectedRows[0].Cells[KeyColumn].Value;
                _searchForm.DialogResult = DialogResult.OK;
            }
        }

    }
}
