using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticNancy.Config
{
    class ResourceProviderCollection : ConfigurationElementCollection, IEnumerable<ResourceProviderElement>
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new ResourceProviderElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ResourceProviderElement)element).Name;
        }

        public new IEnumerator<ResourceProviderElement> GetEnumerator()
        {
            int count = base.Count;
            for (int i = 0; i < count; i++)
            {
                yield return base.BaseGet(i) as ResourceProviderElement;
            }
        }
    }
}
