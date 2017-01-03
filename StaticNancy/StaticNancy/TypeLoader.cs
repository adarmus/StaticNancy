using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace StaticNancy
{
    class TypeLoader
    {
        public Type GetType<T>(string typeName)
        {
            string assemblyName;
            string className;

            GetClassAndAssemblyName(typeName, out assemblyName, out className);

            Type type = GetType<T>(assemblyName, className);

            if (type == null)
                throw new ApplicationException(string.Format("Failed to find type : '{0}'", typeName));

            return type;
        }

        Type GetType<T>(string assemblyName, string className)
        {
            Assembly assembly = GetAssembly(assemblyName);

            Type type = assembly.GetType(className);

            if (type == null)
                throw new ArgumentException(string.Format("Failed to load type {0}", className));

            if (type.GetInterface(typeof(T).Name) == null)
                throw new ArgumentException(string.Format("Type {0} is not {1}", type.Name, typeof(T).Name));

            return type;
        }

        void GetClassAndAssemblyName(string typeName, out string assemblyName, out string className)
        {
            if (string.IsNullOrEmpty(typeName))
                throw new ArgumentNullException("typeName");

            int index = typeName.LastIndexOf(',');

            if (index > -1)
            {
                className = typeName.Substring(0, index);
                assemblyName = typeName.Substring(index + 1);
            }
            else
            {
                index = typeName.LastIndexOf('.');

                assemblyName = typeName.Substring(0, index);
                className = typeName;
            }

            if (string.IsNullOrEmpty(assemblyName) || string.IsNullOrEmpty(className))
                throw new ArgumentException(string.Format("Invalid typeName {0}", typeName));
        }

        public Assembly GetAssembly(string assemblyName)
        {
            try
            {
                return Assembly.Load(assemblyName);
            }
            catch (Exception ex)
            {
                throw new ArgumentException(string.Format("Failed to load assembly {0} [{1}]", assemblyName, ex.Message));
            }
        }
    }
}
