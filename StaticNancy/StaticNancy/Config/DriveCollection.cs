using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticNancy.Config
{
    class DriveCollection : ConfigurationElementCollection, IEnumerable<DriveElement>
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new DriveElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((DriveElement)element).Letter;
        }

        public new IEnumerator<DriveElement> GetEnumerator()
        {
            int count = base.Count;
            for (int i = 0; i < count; i++)
            {
                yield return base.BaseGet(i) as DriveElement;
            }
        }
    }
}
