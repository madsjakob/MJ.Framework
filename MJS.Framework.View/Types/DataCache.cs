using MJS.Framework.Base.Types;
using MJS.Framework.Communication.CO;
using System;
using System.Collections.Generic;
using System.Data;
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

        private Dictionary<Guid, DataObjectAttribute> _mappingCache = new Dictionary<Guid,DataObjectAttribute>();

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

        private bool LoadEntity(Type dataType, Guid id)
        {
            DataObjectAttribute attribute = GetDataObjectAttribute(dataType);
            string sql = string.Format("SELECT * FROM {0} WHERE {1} = @id", attribute.Table, attribute.KeyField);
            ParameterTable parameterTable = new ParameterTable();
            parameterTable.Add("id", id);
            DataTable table = CODataAccess.Main.Endpoint.ExecuteReader(sql, parameterTable);
            bool result = (table.Rows.Count == 1);
            if (result)
            {
                DataRow row = table.Rows[0];
                DataCacheObject dco = new DataCacheObject();
                dco.ID = id;
                dco.DataType = dataType;
                dco.Loaded = DateTime.Now;
                dco.Blobdata = (byte[])row[attribute.BlobField];
            }
            return result;
        }

        private DataObjectAttribute GetDataObjectAttribute(Type dataType)
        {
            DataObjectAttribute attribute = null;
            if (!_mappingCache.ContainsKey(dataType.GUID))
            {
                attribute = (DataObjectAttribute)DataObjectAttribute.GetCustomAttribute(dataType, typeof(DataObjectAttribute));
                if (attribute == null)
                {
                    throw new Exception("Type " + dataType.Name + " not configured for DataCache");
                }
                _mappingCache.Add(dataType.GUID, attribute);
            }
            else
            {
                attribute = _mappingCache[dataType.GUID];
            }
            return attribute;
        }
    }
}
