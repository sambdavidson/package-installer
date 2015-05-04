using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageInstaller
{
    public class DependenciesTracker
    {
        public DependenciesTracker(String[] dependencies)
        {
            
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
