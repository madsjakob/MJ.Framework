using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MJS.Framework.Data.Interfaces;

namespace MJS.Framework.Data.Types
{
    public class DataClassList<T> : List<T>, IDataClassList where T: IDataClass, new()
    {
        public void Add(IDataClass dataClass)
        {
            base.Add((T)dataClass);
        }

        public IDataClass GetItem(int index)
        {
            return this[index];
        }

        public void RemoveAtIndex(int index)
        {
            base.RemoveAt(index);
        }

        public void RemoveObject(IDataClass removeObject)
        {
            base.Remove((T)removeObject);
        }

        public void Assign(IDataClass source)
        {
            
        }

        public new void Clear()
        {
            base.Clear();
        }

        public IDataClass GetInstance()
        {
            return new T();
        }

        public Type GetInstanceType()
        {
            return typeof(T);
        }

        public object Clone()
        {
            return base.MemberwiseClone();
        }

        public void CopyTo(Array array, int index)
        {
            base.CopyTo((T[])array, index);   
        }

        public new int Count
        {
            get { return base.Count; }
        }

        public bool IsSynchronized
        {
            get { return false; }
        }

        public object SyncRoot
        {
            get { return null; }
        }

        public new IEnumerator GetEnumerator()
        {
            return base.GetEnumerator();
        }
    }
}
