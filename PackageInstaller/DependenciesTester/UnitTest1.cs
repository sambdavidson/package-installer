﻿using System;
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
                Assert.AreEqual(DT.PrintPackageDependencies(), "KittenService, Ice, Cyberportal, Leetmeme, CamelCaser, Fraudstream");
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
