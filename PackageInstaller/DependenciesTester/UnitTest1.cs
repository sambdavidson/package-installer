using System;
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
    }
}
