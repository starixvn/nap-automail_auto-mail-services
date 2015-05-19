namespace Fanex.HorseTransaction3rdServices.Libs
{
    using System;
    using System.IO;
    using System.Windows.Forms;
    using System.Xml;

    public class LogService
    {
        public static void Write(string functionName, string error, string userName = "")
        {
            var path = string.Format("{0}/Logs", Application.StartupPath);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var filePath = string.Format("{0}/{1}_log.xml", path, DateTime.Now.ToString("MM-dd-yyyy"));
            var xmlDoc = new XmlDocument();
            if (File.Exists(filePath))
            {
                xmlDoc.Load(filePath);
            }
            else
            {
                var xmlWriter = new XmlTextWriter(filePath, System.Text.Encoding.UTF8);
                xmlWriter.Formatting = Formatting.Indented;
                xmlWriter.WriteProcessingInstruction("xml", "version='1.0' encoding='UTF-8'");
                xmlWriter.WriteStartElement("Logs");
                xmlWriter.Close();
                xmlDoc.Load(filePath);
            }
            
            XmlNode root = xmlDoc.DocumentElement;
            XmlElement log = xmlDoc.CreateElement("Log");
            XmlElement errorTime = xmlDoc.CreateElement("Time");
            XmlElement errorUser = xmlDoc.CreateElement("User");
            XmlElement errorFunction = xmlDoc.CreateElement("Function");
            XmlElement errorDetail = xmlDoc.CreateElement("Error");
            XmlNode nodeTime = xmlDoc.CreateTextNode("Time");
            XmlNode nodeUser = xmlDoc.CreateTextNode("User");
            XmlNode nodeFunction = xmlDoc.CreateTextNode("Function");
            XmlNode nodeDetail = xmlDoc.CreateTextNode("Error");
            
            root.AppendChild(log);
            log.AppendChild(errorTime);
            log.AppendChild(errorUser);
            log.AppendChild(errorFunction);
            log.AppendChild(errorDetail);
            
            errorTime.AppendChild(nodeTime);
            errorUser.AppendChild(nodeUser);
            errorFunction.AppendChild(nodeFunction);
            errorDetail.AppendChild(nodeDetail);
            
            nodeTime.Value = DateTime.Now.ToString();
            nodeUser.Value = userName;
            nodeFunction.Value = functionName;
            nodeDetail.Value = error;
            
            xmlDoc.Save(filePath);
        }
    }
}
