using System;
using System.Reflection;
using System.Xml.Serialization;
using MJS.Framework.Data.Interfaces;


namespace MJS.Framework.Data.Types
{
    /// <summary>
    /// A common handle for all single instance
    /// </summary>
    public abstract class DataClass : IDataClass, IComparable
    {
        /// <summary>
        /// Default constructor for a dataclass.
        /// </summary>
        public DataClass()
        {
            _property = new PropertyByName(this);
            PropertyInfo[] info = this.GetType().GetProperties();
            for (int index = 0; index < info.Length; index++)
            {
                if (info[index].PropertyType.IsAbstract)
                {
                    continue;
                }
                if (info[index].CanWrite)
                {
                    if (typeof(IDataClass).IsAssignableFrom(info[index].PropertyType))
                    {
                        try
                        {
                            info[index].SetValue(this, Activator.CreateInstance(info[index].PropertyType), null);
                        }
                        catch (ArgumentException e)
                        {
                            throw new DataClassException("DataClass Error: trying to create " + this.GetType().Name + ". Property of type: " + info[index].PropertyType.Name, e);
                        }
                        catch (TargetException e)
                        {
                            throw new DataClassException("DataClass Error: trying to create " + this.GetType().Name + ". Property of type: " + info[index].PropertyType.Name, e);
                        }
                        catch (TargetParameterCountException e)
                        {
                            throw new DataClassException("DataClass Error: trying to create " + this.GetType().Name + ". Property of type: " + info[index].PropertyType.Name, e);
                        }
                        catch (MethodAccessException e)
                        {
                            throw new DataClassException("DataClass Error: trying to create " + this.GetType().Name + ". Property of type: " + info[index].PropertyType.Name, e);
                        }
                    }
                    else if (info[index].PropertyType == typeof(string))
                    {
                        info[index].SetValue(this, "", null);
                    }
                    else if (info[index].PropertyType == typeof(DateTime))
                    {
                        info[index].SetValue(this, DateTime.Now, null);
                    }
                    else if (info[index].PropertyType == typeof(int))
                    {
                        info[index].SetValue(this, 0, null);
                    }
                    else if (info[index].PropertyType == typeof(double))
                    {
                        info[index].SetValue(this, 0, null);
                    }
                }
            }
        }

        public virtual void Clear()
        {
            PropertyInfo[] info = this.GetType().GetProperties();
            for (int index = 0; index < info.Length; index++)
            {
                if (info[index].PropertyType.IsAbstract || !info[index].CanWrite)
                {
                    continue;
                }
                if (typeof(IDataClass).IsAssignableFrom(info[index].PropertyType) && info[index].CanWrite)
                {
                    try
                    {
                        info[index].SetValue(this, Activator.CreateInstance(info[index].PropertyType), null);
                    }
                    catch (ArgumentException e)
                    {
                        throw new DataClassException("DataClass Error: trying to create " + this.GetType().Name + ". Property of type: " + info[index].PropertyType.Name, e);
                    }
                    catch (TargetException e)
                    {
                        throw new DataClassException("DataClass Error: trying to create " + this.GetType().Name + ". Property of type: " + info[index].PropertyType.Name, e);
                    }
                    catch (TargetParameterCountException e)
                    {
                        throw new DataClassException("DataClass Error: trying to create " + this.GetType().Name + ". Property of type: " + info[index].PropertyType.Name, e);
                    }
                    catch (MethodAccessException e)
                    {
                        throw new DataClassException("DataClass Error: trying to create " + this.GetType().Name + ". Property of type: " + info[index].PropertyType.Name, e);
                    }
                }
                else if (info[index].PropertyType == typeof(string))
                {
                    info[index].SetValue(this, "", null);
                }
                else if (info[index].PropertyType == typeof(DateTime))
                {
                    info[index].SetValue(this, DateTime.Now, null);
                }
                else if (info[index].PropertyType == typeof(int))
                {
                    info[index].SetValue(this, 0, null);
                }
                else if (info[index].PropertyType == typeof(double))
                {
                    info[index].SetValue(this, 0, null);
                }
            }
        }

        /// <summary>
        /// Create a clone of the current dataclass
        /// </summary>
        /// <returns>An object, a dataclass of the same type, with a copy of the data</returns>
        public object Clone()
        {
            DataClass oResult = (DataClass)Activator.CreateInstance(this.GetType());
            oResult.Assign(this);
            return oResult;
        }

        private PropertyByName _property;
        [XmlIgnore]
        public PropertyByName Property
        {
            get { return _property; }
        }        

        /// <summary>
        /// Get this instance
        /// </summary>
        /// <returns>this</returns>
        public IDataClass GetInstance()
        {
            return this;
        }

        public Type GetInstanceType()
        {
            return GetType();
        }

        /// <summary>
        /// Copies data from the source into this
        /// </summary>
        /// <param name="source">the object to copy from</param>
        public void Assign(IDataClass source)
        {
            if (source == null || this.GetType() != source.GetType())
            {
                return;
            }
            Object tempPropertyValue;
            Object targetPropertyValue;
            Type sourcePropertyType;
            Type type = this.GetType();
            PropertyInfo[] members = type.GetProperties();
            for (int index = 0; index < members.Length; index++)
            {
                if (Attribute.IsDefined(members[index], typeof(XmlIgnoreAttribute)))
                {
                    continue;
                }
                tempPropertyValue = members[index].GetValue(source, null);
                if (tempPropertyValue != null)
                {
                    sourcePropertyType = tempPropertyValue.GetType();
                    if (!(members[index].CanRead && members[index].CanWrite))
                    {
                        continue;
                    }
                    if (typeof(IDataClass).IsAssignableFrom(sourcePropertyType))
                    {
                        targetPropertyValue = Activator.CreateInstance(sourcePropertyType);
                        ((IDataClass)targetPropertyValue).Assign((IDataClass)tempPropertyValue);
                        members[index].SetValue(this, targetPropertyValue, null);
                    }
                    else
                    {
                        members[index].SetValue(this, tempPropertyValue, null);
                    }
                }
            }
        }

        /// <summary>
        /// A virtual function for comparing if you want to sort the objects. 
        /// </summary>
        /// <param name="objekt">The object to compare this instance to</param>
        /// <returns>A value that specifies if this is less than (-), equal (0) or greater than (+) the object passed</returns>
        public virtual int CompareTo(object otherClass)
        {
            if (otherClass != null && this.GetType() != otherClass.GetType())
            {
                throw new DataClassException("Objects to compare must be of the same type");
            }
            return 0;
        }
    }
}
