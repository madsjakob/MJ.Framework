using System.Collections;
using MJS.Framework.Data.Types;
namespace MJS.Framework.Data.Interfaces
{
    public interface IDataDictionary : IDataClass, ICollection
    {
        string[] Keys { get; }
        DataDictionaryEntry this[string key] { get; }
        void Add(string key, DataDictionaryEntry value);
    }
}
