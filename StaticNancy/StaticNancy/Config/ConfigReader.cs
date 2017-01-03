using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticNancy.Config
{
    static class ConfigReader
    {
        public static T GetConfigurationSection<T>(string sectionName) where T : class
        {
            object osection = ConfigurationManager.GetSection(sectionName);

            if (osection == null)
                throw new ConfigurationErrorsException(string.Format("Section '{0}' not found", sectionName));

            var section = osection as T;

            if (section == null)
                throw new ConfigurationErrorsException(string.Format("Section '{0}' is not of type {1}", sectionName, typeof(T).Name));

            return section;
        }
    }
}
