using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MJS.Framework.Win.Delegates;
using MJS.Framework.Win.DO;
using MJS.Framework.Win.Enums;
using MJS.Framework.Win.Utils;

namespace MJS.Framework.Win.SO
{
    public partial class SOEditForm : SOBaseForm
    {
        public SOEditForm()
        {
            InitializeComponent();
        }

        private bool _dataChanged = false;
        private bool DataChanged
        {
            get { return _dataChanged; }
            set 
            {
                if (_currentControl != null && _currentControl.IsLoaded)
                {
                    _dataChanged = value;
                }
            }
        }

        private Dictionary<EditFormState, DOEditFormNode> _stateTree;

        public void SetData(Dictionary<EditFormState,DOEditFormNode> stateTree)
        {
            _stateTree = stateTree;
            var temp = GetTempImage();
            bool first = true;
            foreach (EditFormState state in _stateTree.Keys)
            {
                var tsb = InsertStateButton(state, temp);
                if (first)
                {
                    tsb.Checked = true;
                    SelectState((EditFormState)tsb.Tag);
                    first = false;
                }
            }
        }

        private static Bitmap GetTempImage()
        {
            Bitmap temp = new Bitmap(24, 24);
            using (Graphics graphics = Graphics.FromImage(temp))
            {
                graphics.FillEllipse(Brushes.Blue, new Rectangle(0, 0, temp.Width, temp.Height));
            }
            return temp;
        }

        private ToolStripButton InsertStateButton(EditFormState state, Bitmap temp)
        {
            ToolStripButton tsb = new ToolStripButton();
            tsb.Tag = state;
            tsb.Image = temp;
            tsb.Click += tsbEdit_Click;
            int index = tsMenu.Items.IndexOf(tssSaveSeparator);
            tsMenu.Items.Insert(index, tsb);
            return tsb;
        }

        private ToolStripButton InsertActionButton(Enum action)
        {
            
            ToolStripButton tsb = new ToolStripButton();
            tsb.Tag = action;
            tsb.Image = GetTempImage();
            tsb.Click += ActionButtonClick;
            tsMenu.Items.Add(tsb);
            return tsb;
        }

        private void SelectState(EditFormState state)
        {
            tvMenu.Nodes.Clear();
            DOEditFormNode tree = _stateTree[state];
            foreach (DOEditFormNode node in tree.Nodes)
            {
                tvMenu.Nodes.Add(node);
            }
            tvMenu.ExpandAll();
            
            tvMenu.SelectedNode = tvMenu.Nodes[0];
        }

        private SOBaseControl _currentControl = null;

        private void SelectNode(object sender, TreeViewEventArgs e)
        {
            if (_currentControl != null)
            {
                int saveIndex = tsMenu.Items.IndexOf(tsbSave);
                while (tsMenu.Items.Count > saveIndex + 1)
                {
                    tsMenu.Items.RemoveAt(tsMenu.Items.Count - 1);   
                }
                _currentControl.SaveSettings();
                _currentControl.DispatchEvent(ControlEvent.Destroy);
                scMain.Panel2.Controls.Remove(_currentControl);
                _currentControl.RemoveEventHandler(DispatchEvent);
                _currentControl.Dispose();
            }
            if (e.Node is DOEditFormNode)
            {
                try
                {
                    DOEditFormNode node = e.Node as DOEditFormNode;
                    if (node.ExtraActions != null)
                    {
                        foreach (Enum action in node.ExtraActions)
                        {
                            InsertActionButton(action);
                        }
                    }
                    _currentControl = (SOBaseControl)Activator.CreateInstance(node.EditControlType);
                    _currentControl.Parent = scMain.Panel2;
                    _currentControl.Dock = DockStyle.Fill;
                    _currentControl.SetEventHandler(DispatchEvent);
                    _currentControl.DispatchEvent(ControlEvent.Create);
                    _currentControl.LoadSettings();
                    _currentControl.DispatchEvent(ControlEvent.Update);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        private void tsbSave_Click(object sender, EventArgs e)
        {
            SaveData();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            bool result = false;
            if (keyData == (Keys.Control | Keys.S))
            {
                SaveData();
                result = true;
            }
            else if (keyData == Keys.Escape)
            {
                CloseForm();
                result = true;
            }
            else if (keyData == (Keys.Control | Keys.F1))
            {
                if (_currentControl != null)
                {
                    MessageBox.Show(_currentControl.GetType().Name);
                }
                else
                {
                    MessageBox.Show("<null>");
                }
            }
            else
            {
                result = base.ProcessCmdKey(ref msg, keyData);
            }
            return result;
        }

        private void SaveData()
        {
            DispatchEvent(_currentControl, EditEvent.Save);
            _dataChanged = false;
        }

        private bool _updating = false;
        private bool _creating = false;
        public override void DispatchEvent(SOBaseControl control, Enum action)
        {

            if (!_creating && !_updating && action is ControlEvent && (ControlEvent)action == ControlEvent.Change)
            {
                DataChanged = true;
            }
            base.DispatchEvent(control, action);
        }

        private void CloseForm()
        {
            Close();
        }

        private void tvMenu_BeforeCollapse(object sender, TreeViewCancelEventArgs e)
        {
            e.Cancel = true;
        }

        private void ActionButtonClick(object sender, EventArgs e)
        {
            ToolStripButton button = (sender as ToolStripButton);
            if(button != null)
            {
                Enum action = (Enum) button.Tag;
                DispatchEvent(_currentControl, action);
            }
        }

        private void tsbEdit_Click(object sender, EventArgs e)
        {
            int sepIndex = tsMenu.Items.IndexOf(tssSaveSeparator);
            for (int index = 0; index < sepIndex; index++)
            {
                ToolStripItem item = tsMenu.Items[index];
                if (item is ToolStripButton)
                {
                    (item as ToolStripButton).Checked = false;
                }
            }
            (sender as ToolStripButton).Checked = true;
            SelectState((EditFormState)(sender as ToolStripButton).Tag);
        }

        private void SOEditForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (DataChanged)
            {  
                DialogResult result = MessageBox.Show( "Data er blevet ændret, ønsker du at gemme?", "Gem data?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                e.Cancel = (result == DialogResult.Cancel);
                if(result == DialogResult.Yes)
                {
                     _currentControl.DispatchEvent(EditEvent.Save);
                }
            }
        }
    }
}
