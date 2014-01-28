using MJS.Framework.Data.Interfaces;
using MJS.Framework.Data.Extensions;

namespace MJS.Framework.Data.Types
{
    public class PropertyByName
    {
        private IDataClass _owner;

        public PropertyByName()
        {
        }

        public PropertyByName(IDataClass owner)
        {
            _owner = owner;
        }

        public object this[string propertyName]
        {
            get { return _owner.GetPropertyByName(propertyName); }
            set { _owner.SetPropertyByName(propertyName, value); }
        }
    }
}
