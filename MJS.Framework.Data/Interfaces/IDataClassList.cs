using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace MJS.Framework.Data.Interfaces
{
    /// <summary>
    /// A common handle for all lists containing dataclasses.
    /// </summary>
    public interface IDataClassList : IDataClass, ICollection
    {
        void Add(IDataClass dataClass);
        IDataClass GetItem(int index);
        void RemoveAtIndex(int index);
        void RemoveObject(IDataClass removeObject);
    }
}
