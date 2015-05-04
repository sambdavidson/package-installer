using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageInstaller
{
    public class DependenciesTracker
    {
        private Dictionary<String, HashSet<String>> m_dependencies;
        public DependenciesTracker(String[] dependencies)
        {
            m_dependencies = new Dictionary<String, HashSet<String>>();

            foreach (var line in dependencies)
            {
                var parts = line.Split(':');
                String dependent = parts[0].Trim();
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
                }
                HashSet<String> dentSet;
                m_dependencies.TryGetValue(dependent, out dentSet);
                if (dentSet == null) //Add it to the dictionary if it does not already exist in there.
                {
                    dentSet = new HashSet<string> ();
                    m_dependencies.Add(dependent, dentSet);
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
                       throw new CircularDependenciesException(); 
                    }
                    CheckCircularDependency(str, startVal);
                }
            }
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
