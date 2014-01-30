using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MJS.Framework.View.Types
{
    public class DataCache : Dictionary<Guid, ViewObject>
    {
        // Private constructor to ensure singleton
        private DataCache()
        {
        }
        // A cache for entities from the database
        // Keeps track of data 
        private static DataCache _cache;
        public static DataCache Cache
        {
            get
            {
                if (_cache == null)
                {
                    _cache = new DataCache();
                }
                return _cache;
            }
        }

        public ViewObject LoadEntity<T>(Guid id)
        {
            if (!_cache.ContainsKey(id) || _cache[id].Dirty)
            {
                // Load entity
            }
            return _cache[id];
        }

        public void EditEntity<T>(Guid id)
        { 
        }
    }
}
