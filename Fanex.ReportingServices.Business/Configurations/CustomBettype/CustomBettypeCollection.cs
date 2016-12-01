// -----------------------------------------------------------------------
// <copyright file="CustomBettypeCollection.cs" company="Nexcel Solutions Vietnam">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.ReportingServices.Business.Configurations.CustomBettype
{
    using System.Configuration;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class CustomBettypeCollection : ConfigurationElementCollection
    {
        public CustomBettypeCollection()
        {
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.AddRemoveClearMap;
            }
        }

        public CustomBettypeElement this[int index]
        {
            get
            {
                return (CustomBettypeElement)BaseGet(index);
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

        public new CustomBettypeElement this[string name]
        {
            get
            {
                return (CustomBettypeElement)BaseGet(name);
            }
        }

        public void Add(CustomBettypeElement element)
        {
            BaseAdd(element);
        }

        public void Remove(CustomBettypeElement element)
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

        public int IndexOf(CustomBettypeElement element)
        {
            return BaseIndexOf(element);
        }

        protected override void BaseAdd(ConfigurationElement element)
        {
            BaseAdd(element, false);
        }

        protected override sealed ConfigurationElement CreateNewElement()
        {
            return new CustomBettypeElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((CustomBettypeElement)element).Id;
        }
    }
}