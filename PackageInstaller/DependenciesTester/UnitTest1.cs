using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PackageInstaller;

namespace DependenciesTester
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            string[] dependencies =
            {
                "KittenService:", "Leetmeme: Cyberportal", "Cyberportal: Ice",
                "CamelCaser: KittenService", "Fraudstream: Leetmeme", "Ice: "
            };
            var DT = new DependenciesTracker(dependencies);
            Assert.AreEqual(DT.PrintDependencies(),"KittenService, Ice, Cyberportal, Leetmeme, CamelCaser, Fraudstream");
        }
    }
}
