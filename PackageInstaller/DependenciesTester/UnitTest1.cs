using System;
using System.Collections.Generic;
using System.Runtime;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PackageInstaller;

namespace DependenciesTester
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void SimpleTest1() //Initial quick test using the example from the PDF.
        {
            string[] dependencies =
            {
                "KittenService:", "Leetmeme: Cyberportal", "Cyberportal: Ice",
                "CamelCaser: KittenService", "Fraudstream: Leetmeme", "Ice: "
            };
            DependenciesTracker DT;
            try
            {
                DT = new DependenciesTracker(dependencies);
                String packageDependencies = DT.PrintPackageDependencies();
                Assert.AreEqual("KittenService, Ice, Cyberportal, Leetmeme, CamelCaser, Fraudstream", packageDependencies);
            }
            catch (CircularDependencyException)
            {
                Assert.Fail("Unexpected CircularDependenciesException occured.");
            }

        }
        [TestMethod]
        public void SimpleTest2() //In order linear dependencies
        {
            string[] dependencies =
            {
                "A: ", "B: A", "C: B", "D: C", "E: D", "F: D"
            };
            DependenciesTracker DT;
            try
            {
                DT = new DependenciesTracker(dependencies);
                String packageDependencies = DT.PrintPackageDependencies();
                Assert.AreEqual("A, B, C, D, E, F", packageDependencies);
            }
            catch (CircularDependencyException)
            {
                Assert.Fail("Unexpected CircularDependenciesException occured.");
            }

        }
        [TestMethod]
        public void AdvancedTest1() //Repeat of SimpleTest1 however uses the Evaluate dependencies method
        {
            string[] dependencies =
            {
                "KittenService:", "Leetmeme: Cyberportal", "Cyberportal: Ice",
                "CamelCaser: KittenService", "Fraudstream: Leetmeme", "Ice: "
            };
            DependenciesTracker DT;
            try
            {
                DT = new DependenciesTracker(dependencies);
                String packageDependencies = DT.PrintPackageDependencies();
                EvaluateDependencies(packageDependencies, DT);
            }
            catch (CircularDependencyException)
            {
                Assert.Fail("Unexpected CircularDependenciesException occured.");
            }

        }
        [TestMethod]
        public void AdvancedTest2() //Single dependee, many dependents
        {
            string[] dependencies =
            {
                "B: A", "C: A", "D: A", "E: A", "F: A"
            };
            DependenciesTracker DT;
            try
            {
                DT = new DependenciesTracker(dependencies);
                String packageDependencies = DT.PrintPackageDependencies();
                EvaluateDependencies(packageDependencies, DT);
            }
            catch (CircularDependencyException)
            {
                Assert.Fail("Unexpected CircularDependenciesException occured.");
            }

        }
        [TestMethod]
        public void AdvancedTest3() //Triangular. A->[B,C]->D
        {
            string[] dependencies =
            {
                "B: A", "C: A", "D: B", "D: C", "A: "
            };
            DependenciesTracker DT;
            try
            {
                DT = new DependenciesTracker(dependencies);
                String packageDependencies = DT.PrintPackageDependencies();
                EvaluateDependencies(packageDependencies, DT);
            }
            catch (CircularDependencyException)
            {
                Assert.Fail("Unexpected CircularDependenciesException occured.");
            }

        }
        [TestMethod]
        public void CircularTest1() //Initial circular dependencies test using the example from the PDF.
        {
            string[] dependencies =
            {
                "KittenService:", "Leetmeme: Cyberportal", "Cyberportal: Ice",
                "CamelCaser: KittenService", "Fraudstream:", "Ice: Leetmeme"
            };
            try
            {
                var DT = new DependenciesTracker(dependencies);
            }
            catch (CircularDependencyException)
            {
                Assert.IsTrue(true, "CircularDependenciesException occured when expected.");
                return;
            }
            Assert.Fail();
        }
        [TestMethod]
        public void CircularTest2() //Check if referencing itself creates a circular exception
        {
            string[] dependencies =
            {
                "KittenService:", "Leetmeme: Cyberportal", "Cyberportal: Ice",
                "CamelCaser: CamelCaser"
            };
            try
            {
                var DT = new DependenciesTracker(dependencies);
            }
            catch (CircularDependencyException)
            {
                Assert.IsTrue(true, "CircularDependenciesException occured when expected.");
                return;
            }
            Assert.Fail();
        }
        [TestMethod]
        public void CircularTest3() //In order linear dependencies until circular reassignment
        {
            string[] dependencies =
            {
                "A: ", "B: A", "C: B", "D: C", "E: D", "F: D", "A: F"
            };
            DependenciesTracker DT;
            try
            {
                DT = new DependenciesTracker(dependencies);
            }
            catch (CircularDependencyException)
            {
                Assert.IsTrue(true, "CircularDependenciesException occured when expected.");
                return;
            }
            Assert.Fail();

        }
        /// <summary>
        /// Helper method for checking if the output is a correct.
        /// </summary>
        /// <param name="output"></param>
        /// <param name="dt"></param>
        public void EvaluateDependencies(String output, DependenciesTracker dt)
        {
            String[] splitOutput = output.Split(',');
            var visitedSet = new HashSet<String>();
            foreach (var package in splitOutput)
            {
                var dependees = dt.GetDependees(package.Trim());
                if (!dependees.IsSubsetOf(visitedSet))
                {
                    Assert.Fail("A dependee was not printed before its dependent.");
                }
                visitedSet.Add(package.Trim());
            }
            Assert.IsTrue(true, "The ordering of the packages was correct");
        }
    }
}
