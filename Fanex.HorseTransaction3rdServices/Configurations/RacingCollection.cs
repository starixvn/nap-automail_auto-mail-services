namespace Fanex.HorseTransaction3rdServices.Configurations
{
    using System.Configuration;

    public class RacingCollection : ConfigurationElementCollection
    {
        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.AddRemoveClearMap;
            }
        }

        public RacingElement this[int index]
        {
            get
            {
                return (RacingElement)BaseGet(index);
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

        public RacingElement this[string name]
        {
            get
            {
                return (RacingElement)BaseGet(name);
            }
        }

        public void Add(RacingElement settingElement)
        {
            BaseAdd(settingElement);            
        }

        protected override void BaseAdd(ConfigurationElement element)
        {
            BaseAdd(element, false);
        }

        public void Remove(RacingElement settingElement)
        {
            if (BaseIndexOf(settingElement) >= 0)
            {
                BaseRemove(settingElement.Name);
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

        public int IndexOf(RacingElement settingElement)
        {
            return BaseIndexOf(settingElement);
        }

        protected override sealed ConfigurationElement CreateNewElement()
        {
            return new RacingElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((RacingElement)element).Name;
        }
    }
}
