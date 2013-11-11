using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using MyTfsMobile.App.ViewModel;

namespace MyTfsMobile.App.Tests
{
    [TestClass]
    public class SettingsViewModel_should
    {
        private static SettingsViewModel settingsViewModel;
        [ClassInitialize]
        public static void SettingsViewModel_should_Initialize(TestContext testContext)
        {
            settingsViewModel = new SettingsViewModel();
        }

        [TestMethod]
        [TestCategory("ViewModels")]
        public void Notify_property_changed_when_tfsserver_changes()
        {
            var propertiesChanged = new List<string>();

            settingsViewModel.PropertyChanged += (s, e) => propertiesChanged.Add(e.PropertyName);

            settingsViewModel.TfsServerAdress = "http://www.test.no/";

            Assert.IsTrue(propertiesChanged.Any(c => c.Contains("TfsServerAdress")));
            Assert.AreEqual("http://www.test.no/", settingsViewModel.TfsServerAdress);
        }

        [TestMethod]
        [TestCategory("ViewModels")]
        public void Dont_notify_property_changed_when_tfsserver_dont_changes()
        {
            var propertiesChanged = new List<string>();
            settingsViewModel.TfsServerAdress = "http://www.test.no/";

            settingsViewModel.PropertyChanged += (s, e) => propertiesChanged.Add(e.PropertyName);

            settingsViewModel.TfsServerAdress = "http://www.test.no/";

            Assert.AreEqual(0, propertiesChanged.Count);
        }

        [TestMethod]
        [TestCategory("ViewModels")]
        public void Notify_property_changed_when_tfsusername_changes()
        {
            var propertiesChanged = new List<string>();

            settingsViewModel.PropertyChanged += (s, e) => propertiesChanged.Add(e.PropertyName);

            settingsViewModel.TfsServerUsername = "testUser";

            Assert.IsTrue(propertiesChanged.Any(c => c.Contains("TfsServerUsername")));
            Assert.AreEqual("testUser", settingsViewModel.TfsServerUsername);
        }

        [TestMethod]
        [TestCategory("ViewModels")]
        public void Dont_notify_property_changed_when_tfsusername_dont_changes()
        {
            var propertiesChanged = new List<string>();
            settingsViewModel.TfsServerUsername = "testUser";

            settingsViewModel.PropertyChanged += (s, e) => propertiesChanged.Add(e.PropertyName);

            settingsViewModel.TfsServerUsername = "testUser";

            Assert.AreEqual(0, propertiesChanged.Count); 
        }

        [TestMethod]
        [TestCategory("ViewModels")]
        public void Notify_property_changed_when_tfspassword_changes()
        {
            var propertiesChanged = new List<string>();

            settingsViewModel.PropertyChanged += (s, e) => propertiesChanged.Add(e.PropertyName);

            settingsViewModel.TfsServerPassword = "testPassword";

            Assert.IsTrue(propertiesChanged.Any(c => c.Contains("TfsServerPassword")));
            Assert.AreEqual("testPassword", settingsViewModel.TfsServerPassword);
        }

        [TestMethod]
        [TestCategory("ViewModels")]
        public void Dont_notify_property_changed_when_tfspassword_dont_changes()
        {
            var propertiesChanged = new List<string>();
            settingsViewModel.TfsServerUsername = "testPassword";

            settingsViewModel.PropertyChanged += (s, e) => propertiesChanged.Add(e.PropertyName);

            settingsViewModel.TfsServerPassword = "testPassword";

            Assert.AreEqual(0, propertiesChanged.Count);
        }
    }
}
