namespace Fanex.HorseTransaction3rdServices.Configurations
{
    using System.Configuration;

    public class RacingElement : ConfigurationElement
    {
        [ConfigurationProperty("id", IsRequired = true)]
        public string Id
        {
            get
            {
                return (string)this["id"];
            }

            set
            {
                this["id"] = value;
            }
        }

        [ConfigurationProperty("name", IsRequired = true)]
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

        [ConfigurationProperty("functionid", IsRequired = true)]
        public int FunctionId
        {
            get
            {
                return (int)this["functionid"];
            }

            set
            {
                this["functionid"] = value;
            }
        }

        [ConfigurationProperty("active", IsRequired = false, DefaultValue = false)]
        public bool Active
        {
            get
            {
                return (bool)this["active"];
            }

            set
            {
                this["active"] = value;
            }
        }

        [ConfigurationProperty("filename", IsRequired = true, DefaultValue = "txndump")]
        public string FileName
        {
            get
            {
                return (string)this["filename"];
            }

            set
            {
                this["filename"] = value;
            }
        }

        [ConfigurationProperty("ftpacc", IsRequired = true)]
        public string FtpAcc
        {
            get
            {
                return (string)this["ftpacc"];
            }

            set
            {
                this["ftpacc"] = value;
            }
        }

        [ConfigurationProperty("ftppass", IsRequired = true)]
        public string FtpPass
        {
            get
            {
                return (string)this["ftppass"];
            }

            set
            {
                this["ftppass"] = value;
            }
        }

        [ConfigurationProperty("ftpfolder", IsRequired = false)]
        public string FtpFolder
        {
            get
            {
                return (string)this["ftpfolder"];
            }

            set
            {
                this["ftpfolder"] = value;
            }
        }

        [ConfigurationProperty("ftphost", IsRequired = true)]
        public string FtpHost
        {
            get
            {
                return (string)this["ftphost"];
            }

            set
            {
                this["ftphost"] = value;
            }
        }

        [ConfigurationProperty("ftpdomain", IsRequired = true)]
        public string FtpDomain
        {
            get
            {
                return (string)this["ftpdomain"];
            }

            set
            {
                this["ftpdomain"] = value;
            }
        }

        [ConfigurationProperty("savefilepath", IsRequired = false, DefaultValue = "D:\\Files")]
        public string SaveFilePath
        {
            get
            {
                return (string)this["savefilepath"];
            }

            set
            {
                this["savefilepath"] = value;
            }
        }

        [ConfigurationProperty("mask", IsRequired = false, DefaultValue = "/txndump/")]
        public string Mask
        {
            get
            {
                return (string)this["mask"];
            }

            set
            {
                this["mask"] = value;
            }
        }
    }
}
