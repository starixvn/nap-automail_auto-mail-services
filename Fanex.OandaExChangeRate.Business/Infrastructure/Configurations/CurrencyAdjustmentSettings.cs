namespace Fanex.OandaExChangeRate.Business.Infrastructure.Configurations
{
    using System;
    using System.Configuration;

    public class CurrencyAdjustmentSettings : ConfigurationSection
    {
        [ConfigurationProperty("Currencies", IsDefaultCollection = false),
         ConfigurationCollection(typeof(CurrencyElementCollection), AddItemName = "add", ClearItemsName = "clearCurrencies", RemoveItemName = "removeCurrency")]
        public CurrencyElementCollection Currencies
        {
            get { return this["Currencies"] as CurrencyElementCollection; }
        }



    }

    public class CurrencyElementCollection : ConfigurationElementCollection
    {
        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.AddRemoveClearMap;
            }
        }
        protected override ConfigurationElement CreateNewElement()
        {
            return new CurrencyElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((CurrencyElement)element).Name;
        }

        public void Add(CurrencyElement element)
        {
            BaseAdd(element);
        }

        public void Clear()
        {
            BaseClear();
        }

        public void Remove(CurrencyElement element)
        {
            BaseRemove(element.Name);
        }

        public void Remove(string name)
        {
            BaseRemove(name);
        }
        public void RemoveAt(int index)
        {
            BaseRemoveAt(index);
        }
    }

    public class CurrencyElement : ConfigurationElement
    {
        public enum AdjustmentMethod
        {
            Addition = 0,
            Substraction = 1,
            Multiplication = 2,
            Devision = 3
        }

        public CurrencyElement()
        {
        }

        [ConfigurationProperty("Name", DefaultValue = 1000)]
        public string Name
        {
            get { return (string) base["Name"]; }
            set { base["Name"] = value; }
        }

        [ConfigurationProperty("Method", DefaultValue = 1000)]
        public AdjustmentMethod Method
        {
            get { return (AdjustmentMethod)Enum.Parse(typeof(AdjustmentMethod),  (string)base["Method"]); }
            set { base["Method"] = value; }
        }

        [ConfigurationProperty("Value", DefaultValue = 1000)]
        public double Value
        {
            get { return (double)base["Value"]; }
            set { base["Value"] = value; }
        }
    }
}