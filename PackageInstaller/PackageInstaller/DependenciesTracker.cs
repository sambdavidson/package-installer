using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageInstaller
{
    public class DependenciesTracker
    {
        private readonly Dictionary<String, HashSet<String>> m_dependencies;
        private readonly Dictionary<String, HashSet<String>> m_dependees; 
        public DependenciesTracker(IEnumerable<string> packageDependencies)
        {
            m_dependencies = new Dictionary<String, HashSet<String>>();

            foreach (var line in packageDependencies)
            {
                var parts = line.Split(':');
                String dependent = parts[0].Trim();

                HashSet<String> dependentDependenciesSet;
                m_dependencies.TryGetValue(dependent, out dependentDependenciesSet); //Check and build dependent set.
                if (dependentDependenciesSet == null) //Add it to the dictionary if it does not already exist in there.
                {
                    dependentDependenciesSet = new HashSet<string>();
                    m_dependencies.Add(dependent, dependentDependenciesSet);
                }

                HashSet<String> dependentDependeesSet;
                m_dependencies.TryGetValue(dependent, out dependentDependeesSet); //Check and build dependee set.
                if (dependentDependeesSet == null) //Add it to the dictionary if it does not already exist in there.
                {
                    dependentDependeesSet = new HashSet<string>();
                    m_dependencies.Add(dependent, dependentDependeesSet);
                }
                if (parts[1].Trim().Length > 0) // Check if the second dependency exists
                {
                    String dependee = parts[1].Trim();

                    HashSet<String> tmpList;
                    m_dependencies.TryGetValue(dependee, out tmpList);
                    if (tmpList == null)
                    {
                        tmpList = new HashSet<string> {dependent};
                        m_dependencies.Add(dependee, tmpList);
                    }
                    else
                    {
                        tmpList.Add(dependent);
                    }
                    dependentDependeesSet.Add(dependee);
                }
                
                CheckCircularDependency(dependent,dependent); //Check circular dependency
            }

        }

        private void CheckCircularDependency(String dependency, String startVal)
        {
            
            HashSet<String> directDependents;
            m_dependencies.TryGetValue(dependency, out directDependents);
            if (directDependents != null) //Base case
            {
                foreach (var str in directDependents) //Recursively check the dependents
                {
                    if (str == startVal)
                    {
                       throw new CircularDependencyException(); 
                    }
                    CheckCircularDependency(str, startVal);
                }
            }
        }
        /// <summary>
        /// Prints the packages in the correct order such that a sub-dependency is never installed before a super-dependency.
        /// </summary>
        /// <returns>List of packages</returns>
        public String PrintPackageDependencies()
        {
            return "";
        }
    }
    /// <summary>
    /// Thrown when the DependenciesTracker detects a circular dependency
    /// </summary>
    public class CircularDependencyException : Exception
    {
        public CircularDependencyException()
        {
            
        }

        public CircularDependencyException(String message)
            : base(message)
        {
            
        }
    }
}
