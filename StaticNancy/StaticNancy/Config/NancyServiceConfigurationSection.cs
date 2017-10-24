using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticNancy.Config
{
    class NancyServiceConfigurationSection : ConfigurationSection
    {
        internal const string CONFIG_SECTION = "nancyService";

        [ConfigurationProperty("port", IsRequired = false)]
        public int Port
        {
            get { return (int)this["port"]; }
            set { this["port"] = value; }
        }

        [ConfigurationProperty("drive", IsRequired = false)]
        public string Drive
        {
            get { return (string)this["drive"]; }
            set { this["drive"] = value; }
        }

        [ConfigurationCollection(typeof(ResourceProviderCollection))]
        [ConfigurationProperty("resourceProviders")]
        public ResourceProviderCollection ResourceProviders
        {
            get { return (ResourceProviderCollection)this["resourceProviders"]; }
            set { this["resourceProviders"] = value; }
        }
    }
}
