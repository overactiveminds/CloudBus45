using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CloudBus.Core.Assemblies
{
    public class AssemblyScanner
    {
        public IEnumerable<Type> FindAllTypesImplementing(Type type, IEnumerable<Assembly> assemblies)
        {
            return assemblies.SelectMany(x => x.GetTypes().Where(t => t.IsAssignableFrom(type) && t.IsInterface == false && t.IsAbstract == false));
        }

        public IEnumerable<Type> FindAllTypesExtending(Type type, IEnumerable<Assembly> assemblies)
        {
            return assemblies.SelectMany(x => x.GetTypes().Where(t => t.IsSubclassOf(type)));
        } 
    }
}
