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
        [ConfigurationCollection(typeof(ResourceProviderCollection))]
        [ConfigurationProperty("resourceProviders")]
        public ResourceProviderCollection ResourceProviders
        {
            get { return (ResourceProviderCollection)this["resourceProviders"]; }
            set { this["resourceProviders"] = value; }
        }
    }
}
