using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageInstaller
{
    public class DependenciesTracker
    {
        private Dictionary<String, List<String>> m_dependencies;
        public DependenciesTracker(String[] dependencies)
        {
            m_dependencies = new Dictionary<String, List<String>>();

        }

        private HashSet<String> getDependents(String dependency)
        {
            var dependents = new HashSet<String>();
            
            List<String> directDependents;
            m_dependencies.TryGetValue(dependency, out directDependents);
            if (directDependents != null)
            {
                foreach (var String in directDependents)
                {
                    dependents.Add(String);
                    dependents.UnionWith(getDependents(String));
                }
            }
            return dependents;
        }

        public String PrintDependencies()
        {
            return "";
        }
    }
    /// <summary>
    /// Thrown when the DependenciesTracker detects a circular dependency
    /// </summary>
    public class CircularDependenciesException : Exception
    {
        public CircularDependenciesException()
        {
            
        }

        public CircularDependenciesException(String message)
            : base(message)
        {
            
        }
    }
}
