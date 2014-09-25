namespace Fanex.OandaExChangeRate.Business.Infrastructure.Configurations
{
    using System.Configuration;

    public class MailSettings:ConfigurationSection
    {
        [ConfigurationProperty("Server", IsRequired = false)]
        public ServerElement Server
        {
            get { return this["Server"] as ServerElement; }
        }

        [ConfigurationProperty("Send", IsRequired = true)]
        public SendElement Send
        {
            get { return this["Send"] as SendElement; }
        }

        public static MailSettings Instance
        {
            get
            {
                 var settings = ConfigurationManager.GetSection("MailSettings") as MailSettings;
                return settings;
            }
        }

    }

    public class ServerElement : ConfigurationElement
    {
        
        public ServerElement()
        {
        }

        [ConfigurationProperty("Host", DefaultValue = "localhost")]
        public string Host
        {
            get { return (string)base["Host"]; }
            set { base["Host"] = value; }
        }

       
        [ConfigurationProperty("Port", DefaultValue = 954)]
        public int Port
        {
            get { return (int)base["Port"]; }
            set { base["Port"] = value; }
        }

        [ConfigurationProperty("Username", DefaultValue = "")]
        public string Username
        {
            get { return (string)base["Username"]; }
            set { base["Username"] = value; }
        }


        [ConfigurationProperty("Password", DefaultValue = "")]
        public string Password
        {
            get { return (string)base["Password"]; }
            set { base["Password"] = value; }
        }

        [ConfigurationProperty("DeliveryMethod", DefaultValue = "")]
        public string DeliveryMethod
        {
            get { return (string)base["DeliveryMethod"]; }
            set { base["DeliveryMethod"] = value; }
        }

        [ConfigurationProperty("EnableSsl", DefaultValue = true)]
        public bool EnableSsl
        {
            get { return (bool)base["EnableSsl"]; }
            set { base["EnableSsl"] = value; }
        }


        [ConfigurationProperty("UseDefaultCredentials", DefaultValue = true)]
        public bool UseDefaultCredentials
        {
            get { return (bool)base["UseDefaultCredentials"]; }
            set { base["UseDefaultCredentials"] = value; }
        }

        
    }

    public class SendElement : ConfigurationElement
    {


        public SendElement()
        {
        }


        [ConfigurationProperty("FromEmail", DefaultValue = "error@sunplus.vn")]
        public string FromEmail
        {
            get { return (string)base["FromEmail"]; }
            set { base["ToEmail"] = value; }
        }

        [ConfigurationProperty("ToEmail", DefaultValue = "steven.nguyen@sunplus.vn")]
        public string ToEmail
        {
            get { return (string)base["ToEmail"]; }
            set { base["ToEmail"] = value; }
        }


        [ConfigurationProperty("Cc", DefaultValue = "")]
        public string Cc
        {
            get { return (string)base["Cc"]; }
            set { base["Cc"] = value; }
        }

        [ConfigurationProperty("Bcc", DefaultValue = "")]
        public string Bcc
        {
            get { return (string)base["Bcc"]; }
            set { base["Bcc"] = value; }
        }

    }
}
