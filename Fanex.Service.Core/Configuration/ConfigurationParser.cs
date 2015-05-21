using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Fanex.Service.Core.Configuration
{
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

		public T Parse<T>()
		{
			IDictionary<string, string>  keyToValue = GetKeyToValue();

			T settings = Activator.CreateInstance<T>();

			foreach (var prop in typeof(T).GetProperties())
			{
				if (!prop.CanRead || !prop.CanWrite)
					continue;

				object value = TypeDescriptor.GetConverter(prop.PropertyType).ConvertFromInvariantString(keyToValue[prop.Name]);

				prop.SetValue(settings, value, null);
			}

			return settings;
		}

		private IDictionary<string, string> GetKeyToValue()
		{
			IDictionary<string, string> keyToValue = new Dictionary<string, string>();

			XElement xSettings = XElement.Load(_fileNamePath);
			foreach (XElement xSetting in xSettings.Elements())
			{
				keyToValue.Add(xSetting.Attribute("key").Value, xSetting.Attribute("value").Value);
			}

			return keyToValue;
		} 
	}
}
