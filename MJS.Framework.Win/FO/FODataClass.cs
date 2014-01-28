using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MJS.Framework.Data.Interfaces;
using MJS.Framework.Win.DO;
using MJS.Framework.Win.SO;
using MJS.Framework.Data.Metadata;
using MJS.Framework.Communication.CO;
using System.Data;
using MJS.Framework.Win.Enums;
using System.Windows.Forms;
using System.Reflection;

namespace MJS.Framework.Win.FO
{
    public abstract class FODataClass
    {
        protected SOEditForm _editForm;
        protected SOSearchForm _searchForm;
        private Dictionary<EditFormState, DOEditFormNode> _stateTree = new Dictionary<EditFormState, DOEditFormNode>();

        protected Guid _selectedID;
        public Guid SelectedID
        {
            get { return _selectedID; }
        }

        protected IDataClass Data { get; set; }

        public static void Edit(IDataClass dataClass)
        {
            FODataClass flow = GetFlow(dataClass.GetType());
            flow.EditDataClass(dataClass);
        }

        public static void SelectAndEdit(Type dataClassType, Form mdiParent = null)
        {
            FODataClass flow = GetFlow(dataClassType);
            flow.SelectAndEditDataClass(mdiParent);
        }

        public static Guid Choose(Type dataClassType)
        {
            FODataClass flow = GetFlow(dataClassType);
            return flow.ChooseDataClass();
        }

        private void EditDataClass(IDataClass dataClass)
        {
            Data = dataClass;
            CreateEditTree();
            _editForm = new SOEditForm();
            _editForm.SetEventHandler(EditHandler);
            _editForm.SetData(_stateTree);
            _editForm.Show();
        }
        
        private void SelectAndEditDataClass(Form mdiParent = null)
        {
            _searchForm = new SOSearchForm();
            _searchForm.DoubleClickEvent = ListEvent.Edit;
            _searchForm.SetEventHandler(ListHandler);
            if (mdiParent != null)
            {
                _searchForm.MdiParent = mdiParent;
                _searchForm.WindowState = FormWindowState.Maximized;
            }
            _searchForm.Show();
        }

        private Guid ChooseDataClass()
        {
            _searchForm = new SOSearchForm();
            _searchForm.DoubleClickEvent = ListEvent.Choose;
            _searchForm.SetEventHandler(ListHandler);
            Guid result = Guid.Empty;
            if (_searchForm.ShowDialog() == DialogResult.OK)
            {
                result = SelectedID; 
            }
            return result;
        }

        protected virtual void EditHandler(SOBaseControl control, Enum action)
        {
            if (action is ControlEvent)
            {
                ControlEvent eventID = (ControlEvent)action;
                switch (eventID)
                {
                    case ControlEvent.Create:
                        EditCreate(control);
                        break;
                    case ControlEvent.Change:
                        EditChange(control);
                        break;
                    case ControlEvent.Update:
                        EditUpdate(control);
                        break;
                    case ControlEvent.Destroy:
                        EditDestroy(control);
                        break;
                }
            }
            else if (action is EditEvent)
            {
                EditEvent eventID = (EditEvent)action;
                switch (eventID)
                {
                    case EditEvent.Save:
                        EditSave(control);
                        break;
                }
            }
            else
            {
                EditCustom(control, action);
            }
        }

        protected virtual void ListHandler(SOBaseControl control, Enum action)
        {
            if (action is ControlEvent)
            {
                ControlEvent eventID = (ControlEvent)action;
                switch (eventID)
                {
                    case ControlEvent.Create:
                        ListCreate(control);
                        break;
                    case ControlEvent.Change:
                        ListChange(control);
                        break;
                    case ControlEvent.Update:
                        ListUpdate(control);
                        break;
                    case ControlEvent.Destroy:
                        ListDestroy(control);
                        break;
                }
            }
            else if (action is ListEvent)
            {
                ListEvent eventID = (ListEvent)action;
                switch (eventID)
                {
                    case ListEvent.New:
                        ListNew(control);
                        break;
                    case ListEvent.Copy:
                        ListCopy(control);
                        break;
                    case ListEvent.Edit:
                        ListEdit(control);
                        break;
                    case ListEvent.Delete:
                        ListDelete(control);
                        break;
                    case ListEvent.Choose:
                        ListChoose(control);
                        break;
                }
            }
            else
            {
                ListCustom(control, action);
            }
        }

        protected DOEditFormNode AddNode(string text, Type controlType, object data, EditFormState state = EditFormState.Edit, Enum[] actions = null)
        {
            if (!_stateTree.ContainsKey(state))
            {
                _stateTree.Add(state, new DOEditFormNode());
            }
            DOEditFormNode tree = _stateTree[state];
            DOEditFormNode node = new DOEditFormNode(text, controlType, data);
            node.ExtraActions = actions;
            tree.Nodes.Add(node);
            return node;
        }

        protected abstract void CreateEditTree();

        protected abstract void EditCreate(SOBaseControl control);
        protected abstract void EditUpdate(SOBaseControl control);
        protected abstract void EditChange(SOBaseControl control);
        protected abstract void EditDestroy(SOBaseControl control);
        protected abstract void EditSave(SOBaseControl control);
        protected virtual void EditCustom(SOBaseControl control, Enum action)
        {
        }

        protected abstract void ListCreate(SOBaseControl control);
        protected abstract void ListUpdate(SOBaseControl control);
        protected abstract void ListChange(SOBaseControl control);
        protected abstract void ListDestroy(SOBaseControl control);
        protected abstract void ListNew(SOBaseControl control);
        protected abstract void ListCopy(SOBaseControl control);
        protected abstract void ListEdit(SOBaseControl control);
        protected abstract void ListDelete(SOBaseControl control);
        protected abstract void ListChoose(SOBaseControl control);
        protected virtual void ListCustom(SOBaseControl control, Enum action)
        {
        }

        private static Dictionary<Type, Type> _flowRegister = new Dictionary<Type,Type>();
        
        public static void RegisterFlow(Type dataClassType, Type flowType)
        {
            if (!_flowRegister.ContainsKey(dataClassType))
            {
                _flowRegister.Add(dataClassType, flowType);
            }
            else
            {
                throw new Exception("Flow already registered for " + dataClassType.Name);
            }
        }

        public static FODataClass GetFlow(Type dataClassType)
        {
            FODataClass flow = null;
            FODataClass.Configure();
            if (_flowRegister.ContainsKey(dataClassType))
            {
                flow = (FODataClass)Activator.CreateInstance( _flowRegister[dataClassType]);
            }
            else
            {
                throw new Exception("No flow registered for " + dataClassType.Name);
            }
            return flow;
        }


        private static List<string> _configuredAssemblies = new List<string>();
        public static void Configure()
        {
            Assembly entry = Assembly.GetEntryAssembly();
            
            ConfigureAssembly(entry);
            AssemblyName[] referencedAssemblies = entry.GetReferencedAssemblies();
            foreach(AssemblyName refAss in referencedAssemblies)
            {
                Assembly refA = AppDomain.CurrentDomain.Load(refAss);
                ConfigureAssembly(refA);
            }
            
        }

        private static void ConfigureAssembly(Assembly refA)
        {
            if (!_configuredAssemblies.Contains(refA.FullName))
            {
                foreach (Type t in refA.GetTypes())
                {
                    if (typeof(FODataClass) != t && typeof(FODataClass).IsAssignableFrom(t))
                    {
                        if (!t.IsGenericType)
                        {
                            FODataClass flow = (FODataClass)Activator.CreateInstance(t);
                            flow.Register();
                        }
                    }
                }
                _configuredAssemblies.Add(refA.FullName);
            }
        }

        protected virtual void Register()
        {
        }
    }
}
