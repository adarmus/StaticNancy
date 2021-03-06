﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticNancy.Config
{
    class ResourceProviderElement : ConfigurationElement
    {
        [ConfigurationProperty("name", IsRequired = true)]
        public string Name
        {
            get { return (string)this["name"]; }
            set { this["name"] = value; }
        }

        [ConfigurationProperty("assemblyName", IsRequired = true)]
        public string AssemblyName
        {
            get { return (string)this["assemblyName"]; }
            set { this["assemblyName"] = value; }
        }

        [ConfigurationProperty("resourcePrefix", IsRequired = true)]
        public string ResourcePrefix
        {
            get { return (string)this["resourcePrefix"]; }
            set { this["resourcePrefix"] = value; }
        }

        [ConfigurationProperty("requestRootPath", IsRequired = true)]
        public string RequestRootPath
        {
            get { return (string)this["requestRootPath"]; }
            set { this["requestRootPath"] = value; }
        }

        [ConfigurationProperty("enabled", IsRequired = false, DefaultValue = true)]
        public bool Enabled
        {
            get { return (bool)this["enabled"]; }
            set { this["enabled"] = value; }
        }

        [ConfigurationProperty("outputResourcesList", IsRequired = false, DefaultValue = false)]
        public bool OutputResourcesList
        {
            get { return (bool)this["outputResourcesList"]; }
            set { this["outputResourcesList"] = value; }
        }

        [ConfigurationProperty("defaultResource", IsRequired = false)]
        public string DefaultResource
        {
            get { return (string)this["defaultResource"]; }
            set { this["defaultResource"] = value; }
        }
    }
}
