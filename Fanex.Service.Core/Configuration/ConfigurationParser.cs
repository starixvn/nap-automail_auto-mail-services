namespace Fanex.Service.Core.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Xml.Linq;

    public class ConfigurationParser
    {
        private readonly string _fileNamePath;

        public ConfigurationParser(string fileNamePath)
        {
            _fileNamePath = fileNamePath;
        }

        public IDictionary<string, string> Parse()
        {
            IDictionary<string, string> keyToValue = new Dictionary<string, string>();

            XElement settings = XElement.Load(_fileNamePath);
            foreach (XElement setting in settings.Elements())
            {
                keyToValue.Add(setting.Attribute("key").Value, setting.Attribute("value").Value);
            }

            return keyToValue;
        }

        public T Parse<T>() where T : ISettings
        {
            IDictionary<string, string> keyToValue = Parse();

            T settings = Activator.CreateInstance<T>();

            foreach (var prop in typeof(T).GetProperties())
            {
                if (!prop.CanRead || !prop.CanWrite)
                {
                    continue;
                }

                object value = TypeDescriptor.GetConverter(prop.PropertyType).ConvertFromInvariantString(keyToValue[prop.Name]);

                prop.SetValue(settings, value, null);
            }

            return settings;
        }
    }
}