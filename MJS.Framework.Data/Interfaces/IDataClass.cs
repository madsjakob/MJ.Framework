using System;
using System.Collections.Generic;
using System.Text;

namespace MJS.Framework.Data.Interfaces
{
    /// <summary>
    /// A common handle for all dataclasses, both lists and single instances.
    /// </summary>
    public interface IDataClass : ICloneable
    {
        void Assign(IDataClass source);
        void Clear();
        IDataClass GetInstance();
        Type GetInstanceType();
    }
}
