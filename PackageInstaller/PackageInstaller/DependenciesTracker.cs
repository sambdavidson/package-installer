using System;
using System.Collections.Generic;
using System.Text;

namespace PackageInstaller
{
    public class DependenciesTracker
    {
        private readonly Dictionary<String, HashSet<String>> m_dependencies; // Contains a map of package to dependent.
        private readonly Dictionary<String, HashSet<String>> m_dependees;  // Contains a map of package to dependee.
        public DependenciesTracker(IEnumerable<string> packageDependencies)
        {
            m_dependencies = new Dictionary<String, HashSet<String>>();
            m_dependees = new Dictionary<String, HashSet<String>>();

            foreach (var line in packageDependencies)
            {
                var parts = line.Split(':');
                String dependent = parts[0].Trim();

                HashSet<String> dependentDependenciesSet;
                m_dependencies.TryGetValue(dependent, out dependentDependenciesSet); //Check and build dependent set.
                if (dependentDependenciesSet == null) //Add it to the dictionary if it does not already exist.
                {
                    dependentDependenciesSet = new HashSet<string>();
                    m_dependencies.Add(dependent, dependentDependenciesSet);
                }

                HashSet<String> dependentDependeesSet;
                m_dependees.TryGetValue(dependent, out dependentDependeesSet); //Check and build dependee set.
                if (dependentDependeesSet == null) //Add it to the dictionary if it does not already exist.
                {
                    dependentDependeesSet = new HashSet<string>();
                    m_dependees.Add(dependent, dependentDependeesSet);
                }
                if (parts[1].Trim().Length > 0) // Check if the dependee exists.
                {
                    String dependee = parts[1].Trim();

                    HashSet<String> tmpList;
                    m_dependencies.TryGetValue(dependee, out tmpList); //Modify the dependees dependent map.
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
                
                CheckCircularDependency(dependent); //Check circular dependency after each addition.
            }

        }
        /// <summary>
        /// Helper method that recursively explores a package's dependents. 
        /// If the starting package is found as a dependent.
        /// This function could operate a "HasDependent" if neccessary by placing any dependent in startVal. 
        /// </summary>
        /// <param name="package">The package who's dependent tree to explore.</param>
        /// <param name="startVal">The starting package which upon finding indicates a circular dependency</param>
        private void CheckCircularDependency(String package, String startVal = "")
        {
            if (startVal == "") //Workaround for the variable parameter on startVal.
                startVal = package;

            HashSet<String> directDependents;
            m_dependencies.TryGetValue(package, out directDependents);
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
            var output = new StringBuilder();
            var visited = new HashSet<String>();
            foreach (var package in m_dependencies.Keys)
            {
                if(!visited.Contains(package))
                    output.Append(printDependees(package, visited));
            }
            output.Length -= 2; //Removes final ", "
            return output.ToString();
        }
        /// <summary>
        /// Private helper method that helps print the packages in dependency order.
        /// </summary>
        /// <param name="package">The package who's dependees to print</param>
        /// <param name="visited">Master set of all visited packages. Stops repeats</param>
        /// <returns>This package plus all its dependee classes.</returns>
        private String printDependees(String package, HashSet<String> visited)
        {
            visited.Add(package);

            var output = new StringBuilder();

            HashSet<String> dependees;
            m_dependees.TryGetValue(package, out dependees);
            if (dependees != null)
            {
                foreach (var packName in dependees)
                {
                    if(!visited.Contains(packName))
                        output.Append(printDependees(packName,visited));
                }
            }
            output.Append(package + ", ");
            return output.ToString();
        }
        /// <summary>
        /// Outputs a list of all dependees of a package. 
        /// The PDF does not specify whether the PrintPackageDependencies output must be sorted in any way other than respecting dependenies.
        /// This method will help check if a package is being printed before one of its dependees.
        /// </summary>
        /// <param name="package"></param>
        /// <returns></returns>
        public HashSet<String> GetDependees(string package)
        {
            var dependeeSet = new HashSet<String>();
            HashSet<String> tmpDependees;
            m_dependees.TryGetValue(package, out tmpDependees);
            if (tmpDependees != null)
            {
                foreach (var dependee in tmpDependees)
                {
                    dependeeSet.Add(dependee);
                    dependeeSet.UnionWith(GetDependees(dependee));
                }
            }
            return dependeeSet;
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
