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
        [ConfigurationProperty("resourceProvider")]
        public ResourceProviderCollection ResourceProviders
        {
            get { return (ResourceProviderCollection)this["resourceProvider"]; }
            set { this["resourceProvider"] = value; }
        }
    }
}
