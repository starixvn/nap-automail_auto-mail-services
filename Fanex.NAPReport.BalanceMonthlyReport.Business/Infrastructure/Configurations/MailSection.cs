using System.Configuration;

namespace Fanex.NAPReport.BalanceMonthlyReport.Business.Infrastructure.Configurations
{
    /// <summary>
    /// Summary description for MailSetting
    /// </summary>
    public class MailSection:ConfigurationSection
    {


        public enum Setting
        {
            Server, Send
        }

        public enum HostName
        {
            Gmail, IBC
        }

        [ConfigurationProperty("Server", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(MailCollection),
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public MailCollection Server
        {
            get
            {
                var spCollection =
                    (MailCollection)base["Server"];
                return spCollection;
            }
        }

        [ConfigurationProperty("Send", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(MailCollection),
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public MailCollection Send
        {
            get
            {
                var spCollection =
                    (MailCollection)base["Send"];
                return spCollection;
            }
        }


        public static MailSection Instance
        {
            get
            {
                var spSection =
                 ConfigurationManager.GetSection("MailSection") as MailSection;
                return spSection;
            }
        }


        public MailCollection this[Setting name]
        {
            get
            {

                var spCollection = (MailCollection)this.GetType().GetProperty(name.ToString()).GetValue(this, System.Reflection.BindingFlags.GetField, null, null, null);
                return spCollection;
            }
        }


        
    }


    public class MailCollection : ConfigurationElementCollection
    {

        public MailCollection()
        {
            //var storeProcedure = (StoreProcedureElement) CreateNewElement();
            //Add(storeProcedure);
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.AddRemoveClearMap;
            }
        }

        public void Add(MailElement mailElement)
        {
            BaseAdd(mailElement);
        }

        protected override void BaseAdd(ConfigurationElement element)
        {
            BaseAdd(element, false);
        }

        public void Remove(MailElement mailElement)
        {
            if (BaseIndexOf(mailElement) >= 0)
                BaseRemove(mailElement.Name);
        }

        public void RemoveAt(int index)
        {
            BaseRemoveAt(index);
        }

        public void Remove(string alias)
        {
            BaseRemove(alias);
        }

        public void Clear()
        {
            BaseClear();
        }


        public MailElement this[int index]
        {
            get
            {
                return (MailElement)BaseGet(index);
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

        new public MailElement this[string Name]
        {
            get
            {
                return (MailElement)BaseGet(Name);
            }
        }

        public int IndexOf(MailElement mailElement)
        {
            return BaseIndexOf(mailElement);
        }


        protected override sealed ConfigurationElement CreateNewElement()
        {
            return new MailElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((MailElement)element).Name;
        }
    }

    public class MailElement : ConfigurationElement
    {



        public MailElement()
        {

        }


        [ConfigurationProperty("host", IsRequired = false)]
        //[RegexStringValidator(@"\w+:\/\/[\w.]+\S*")]
        public string Host
        {
            get
            {
                return (string)this["host"];
            }
            set
            {
                this["host"] = value;
            }
        }

        [ConfigurationProperty("port", IsRequired = false, DefaultValue = 25)]
        //[RegexStringValidator(@"\w+:\/\/[\w.]+\S*")]
        public int Port
        {
            get
            {
                return (int)this["port"];
            }
            set
            {
                this["port"] = value;
            }
        }

        [ConfigurationProperty("order")]
        //[RegexStringValidator(@"\w+:\/\/[\w.]+\S*")]
        public int Order
        {
            get
            {
                return (int)this["order"];
            }
            set
            {
                this["order"] = value;
            }
        }

        [ConfigurationProperty("username", IsRequired = false)]
        //[RegexStringValidator(@"\w+:\/\/[\w.]+\S*")]
        public string Username
        {
            get
            {
                return (string)this["username"];
            }
            set
            {
                this["username"] = value;
            }
        }


        [ConfigurationProperty("password", IsRequired = false)]
        //[RegexStringValidator(@"\w+:\/\/[\w.]+\S*")]
        public string Password
        {
            get
            {
                return (string)this["password"];
            }
            set
            {
                this["password"] = value;
            }
        }
        
        
        [ConfigurationProperty("name", IsRequired = false)]
        //[RegexStringValidator(@"\w+:\/\/[\w.]+\S*")]
        public string Name
        {
            get
            {
                return (string)this["name"];
            }
            set
            {
                this["name"] = value;
            }
        }

        [ConfigurationProperty("from", IsRequired = false)]
        //[RegexStringValidator(@"\w+:\/\/[\w.]+\S*")]
        public string From
        {
            get
            {
                return (string)this["from"];
            }
            set
            {
                this["from"] = value;
            }
        }

        [ConfigurationProperty("to", IsRequired = false)]
        //[RegexStringValidator(@"\w+:\/\/[\w.]+\S*")]
        public string To
        {
            get
            {
                return (string)this["to"];
            }
            set
            {
                this["to"] = value;
            }
        }

        [ConfigurationProperty("bcc", IsRequired = false)]
        //[RegexStringValidator(@"\w+:\/\/[\w.]+\S*")]
        public string Bcc
        {
            get
            {
                return (string)this["bcc"];
            }
            set
            {
                this["bcc"] = value;
            }
        }

        [ConfigurationProperty("cc", IsRequired = false)]
        //[RegexStringValidator(@"\w+:\/\/[\w.]+\S*")]
        public string Cc
        {
            get
            {
                return (string)this["cc"];
            }
            set
            {
                this["cc"] = value;
            }
        }


        [ConfigurationProperty("ssl", IsRequired = false, DefaultValue = false)]
        //[RegexStringValidator(@"\w+:\/\/[\w.]+\S*")]
        public bool Ssl
        {
            get
            {
                return (bool)this["ssl"];
            }
            set
            {
                this["ssl"] = value;
            }
        }

        [ConfigurationProperty("mails", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(MailCollection),
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public MailCollection Mails
        {
            get
            {
                var stores =
                    (MailCollection)base["mails"];
                return stores;
            }
        }
    }
}
