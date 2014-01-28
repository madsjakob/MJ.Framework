using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MJS.Framework.Data.CO;

namespace MJS.Framework.Data.Types
{
    public class Entity : DataClass
    {
        private Guid _id;

        public Guid GetID()
        {
            return _id;
        }

        public void SetID(Guid value)
        {
            _id = value;
        }

        public void Save()
        {
            BeforeSave();
            CODataClass.Save(this);
            AfterSave();
        }

        public virtual void BeforeSave()
        {
        }

        public virtual void AfterSave()
        {
        }

        public void Load()
        {
            BeforeLoad();
            CODataClass.Load(this);
            AfterLoad();
        }

        public virtual void BeforeLoad()
        {
        }

        public virtual void AfterLoad()
        {
        }

        public void Delete()
        {
            BeforeDelete();
            CODataClass.Delete(this);
            AfterDelete();
        }

        public virtual void BeforeDelete()
        {
        }

        public virtual void AfterDelete()
        {
        }

        public virtual string GetDisplayName()
        {
            return ToString();
        }

    }
}
