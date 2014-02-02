using MJS.Framework.Base.Types;
using MJS.Framework.Base.Utils;
using MJS.Framework.Communication.CO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MJS.Framework.View.Types
{
    public class DataCache : Dictionary<Guid, DataCacheObject>
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

        private DataCacheObject GetEntity(Type dataType, Guid id)
        {
            DataCacheObject result = null;
            if(EntityDirty(dataType, id))
            {
                LoadEntity(dataType, id);
            }
            result = this[id];
            return result;
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
                if (ContainsKey(id))
                {
                    this[id] = dco;
                }
                else
                {
                    Add(id, dco);
                }
            }
            return result;
        }

        private bool EntityDirty(Type dataType, Guid id)
        {
            bool result = false;
            if (ContainsKey(id))
            {
                if (!this[id].EditState)
                {
                    DataObjectAttribute attribute = GetDataObjectAttribute(dataType);
                    string sql = string.Format("SELECT {2} FROM {0} WHERE {1} = @id", attribute.Table, attribute.KeyField, attribute.UpdatedField);
                    ParameterTable parameterTable = new ParameterTable();
                    parameterTable.Add("id", id);
                    DataTable table = CODataAccess.Main.Endpoint.ExecuteReader(sql, parameterTable);
                    if (table.Rows.Count == 1)
                    {
                        DateTime updated = (DateTime)SqlUtils.FromSqlValue(typeof(DateTime), table.Rows[0][attribute.UpdatedField]);
                        result = (updated > this[id].Loaded);
                    }
                    else
                    {
                        result = true;
                    }
                }
            }
            else
            {
                result = true;
            }
            return result;
        }

        private bool EntityLocked(Type dataType, Guid id)
        {
            string sql = "SELECT * FROM StatusEntity WHERE EntityID = @entityid AND EntityName = @entityname";
            ParameterTable parameterTable = new ParameterTable();
            parameterTable.Add("entityid", id);
            parameterTable.Add("entityname", dataType.Name);
            parameterTable.Add("aktoerid", Guid.Empty);
            DataTable table = CODataAccess.Main.Endpoint.ExecuteReader(sql, parameterTable);
            return table.Rows.Count > 0;
        }

        private bool LockEntity(Type dataType, Guid id)
        {
            string sql = "INSERT INTO StatusEntity (EntityID, EntityName, AktoerID, Kategori) VALUES (@entityid, @entityname, @aktoerid, @kategori";
            ParameterTable parameterTable = new ParameterTable();
            parameterTable.Add("entityid", id);
            parameterTable.Add("entityname", dataType.Name);
            parameterTable.Add("aktoerid", Guid.Empty);
            parameterTable.Add("kategori", -1);
            CODataAccess.Main.Endpoint.ExecuteNonQuery(sql, parameterTable);
            return false;
        }

        private void UnlockEntity(Type dataType, Guid id)
        {
            string sql = "DELETE FROM StatusEntity WHERE entityID = @entityid AND EntityName = @entityname AND AktoerID = @aktoerid";
            ParameterTable parameterTable = new ParameterTable();
            parameterTable.Add("entityid", id);
            parameterTable.Add("entityname", dataType.Name);
            parameterTable.Add("aktoerid", Guid.Empty);
            CODataAccess.Main.Endpoint.ExecuteNonQuery(sql, parameterTable);
            
            
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
