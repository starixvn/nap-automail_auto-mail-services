// -----------------------------------------------------------------------
// <copyright file="ExcludeVirtualLineTotalCollection.cs" company="Nexcel Solutions Vietnam">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.ReportingServices.Business.Configurations.ExcludeVirtualLineTotal
{
    using System.Configuration;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ExcludeVirtualLineTotalCollection : ConfigurationElementCollection
    {
        public ExcludeVirtualLineTotalCollection()
        {
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.AddRemoveClearMap;
            }
        }

        public ExcludeVirtualLineTotalElement this[int index]
        {
            get
            {
                return (ExcludeVirtualLineTotalElement)BaseGet(index);
            }

            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }

                BaseAdd(index, value);
            }
        }

        public new ExcludeVirtualLineTotalElement this[string name]
        {
            get
            {
                return (ExcludeVirtualLineTotalElement)BaseGet(name);
            }
        }

        public void Add(ExcludeVirtualLineTotalElement element)
        {
            BaseAdd(element);
        }

        public void Remove(ExcludeVirtualLineTotalElement element)
        {
            if (BaseIndexOf(element) >= 0)
            {
                BaseRemove(element.Id);
            }
        }

        public void RemoveAt(int index)
        {
            BaseRemoveAt(index);
        }

        public void Remove(string name)
        {
            BaseRemove(name);
        }

        public void Clear()
        {
            BaseClear();
        }

        public int IndexOf(ExcludeVirtualLineTotalElement element)
        {
            return BaseIndexOf(element);
        }

        protected override void BaseAdd(ConfigurationElement element)
        {
            BaseAdd(element, false);
        }

        protected override sealed ConfigurationElement CreateNewElement()
        {
            return new ExcludeVirtualLineTotalElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ExcludeVirtualLineTotalElement)element).Id;
        }
    }
}