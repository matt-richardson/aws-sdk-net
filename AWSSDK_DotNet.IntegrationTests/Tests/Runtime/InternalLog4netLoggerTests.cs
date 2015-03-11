using System;
using System.Collections;
using System.Configuration;
using System.Reflection;
using System.Xml;
using Amazon;
using Amazon.Runtime.Internal.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AWSSDK_DotNet.IntegrationTests.Tests.Runtime
{
    [TestClass, Ignore]
    public class InternalLog4netLoggerTests
    {
        [ClassInitialize]
        public static void Initialize(TestContext a)
        {
            AWSConfigs.LoggingConfig.LogTo = LoggingOptions.Log4Net;

            //Add the log4net configuration to the app config file
            var xmlDocument = new XmlDocument();
            var uri = new Uri(Assembly.GetExecutingAssembly().CodeBase);
            var filename = new System.IO.FileInfo(uri.AbsolutePath).Name + ".config";
            xmlDocument.Load(filename);
            var configurationElement = xmlDocument.DocumentElement;
            var configSections = configurationElement.SelectSingleNode("configSections") ?? configurationElement.AppendChild(xmlDocument.CreateElement("configSections"));
            var section = configSections.SelectSingleNode("section[@name='log4net']");
            if (section == null)
            {
                section = configSections.AppendChild(xmlDocument.CreateElement("section"));
                AddAttribute(xmlDocument, section, "name", "log4net");
                AddAttribute(xmlDocument, section, "type", "log4net.Config.Log4NetConfigurationSectionHandler, log4net");
            }
            var log4netSection = configurationElement.SelectSingleNode("log4net");
            if (log4netSection == null)
            {
                log4netSection = configurationElement.AppendChild(xmlDocument.CreateElement("log4net"));
                var appender = log4netSection.AppendChild(xmlDocument.CreateElement("appender"));
                AddAttribute(xmlDocument, appender, "name", "MemoryAppender");
                AddAttribute(xmlDocument, appender, "type", "log4net.Appender.MemoryAppender");
                var root = log4netSection.AppendChild(xmlDocument.CreateElement("root"));
                var level = root.AppendChild(xmlDocument.CreateElement("level"));
                AddAttribute(xmlDocument, level, "value", "INFO");
                var appenderRef = root.AppendChild(xmlDocument.CreateElement("appender-ref"));
                AddAttribute(xmlDocument, appenderRef, "ref", "MemoryAppender");
            }
            xmlDocument.Save(filename);
            ConfigurationManager.RefreshSection("log4net");
        }

        private static void AddAttribute(XmlDocument xmlDocument, XmlNode section, string name, string value)
        {
            var xmlAttribute = xmlDocument.CreateAttribute(name);
            xmlAttribute.Value = value;
            section.Attributes.Append(xmlAttribute);
        }

        [ClassCleanup]
        public static void Cleanup()
        {
            //Remove our added log4net configuration from the app config file
            var xmlDocument = new XmlDocument();
            var uri = new Uri(Assembly.GetExecutingAssembly().CodeBase);
            var filename = new System.IO.FileInfo(uri.AbsolutePath).Name + ".config";
            xmlDocument.Load(filename);
            var configurationElement = xmlDocument.DocumentElement;
            var configSections = configurationElement.SelectSingleNode("configSections");
            if (configSections != null)
            {
                var section = configSections.SelectSingleNode("section[@name='log4net']");
                if (section != null)
                    configSections.RemoveChild(section);
            }
            var log4netSection = configurationElement.SelectSingleNode("log4net");
            if (log4netSection != null)
                configurationElement.RemoveChild(log4netSection);
            xmlDocument.Save(filename);
            ConfigurationManager.RefreshSection("log4net");
        }

        [TestMethod, TestCategory("Runtime"), Ignore]
        public void CallsXmlConfiguratorDotConfigureIfLog4NetIsNotAlreadyConfigured()
        {
            //ignored, as it requires log4net.dll in the bin folder which would normally by done
            //by adding a reference, howerver this seems to go against the philosophy of the code base

            var logger = new InternalLog4netLogger(this.GetType());

            var loggerManagerType = Type.GetType("log4net.Core.LoggerManager, log4net");

            if (loggerManagerType == null)
                Assert.Inconclusive("This test requires log4net.dll to be in the bin folder");

            var repository = (loggerManagerType.GetMethod("GetAllRepositories").Invoke(null, null) as object[])[0];
            var configured = repository.GetType().GetProperty("Configured").GetValue(repository, null);
            Assert.AreEqual(configured, true);

            var root = repository.GetType().GetProperty("Root").GetValue(repository, null);
            var appenders = root.GetType().GetProperty("Appenders").GetValue(root, null);

            var appenderCount = 0;
            foreach (var appender in (IEnumerable)appenders)
            {
                appenderCount++;
                Assert.AreEqual("MemoryAppender", appender.GetType().Name);
            }
            Assert.AreEqual(1, appenderCount);
        }

        [TestMethod, TestCategory("Runtime"), Ignore]
        public void DoesNotCallXmlConfiguratorDotConfigureIfLog4NetIsAlreadyConfigured()
        {
            //ignored, as it requires log4net.dll in the bin folder which would normally by done
            //by adding a reference, howerver this seems to go against the philosophy of the code base

            var loggerManagerType = Type.GetType("log4net.Core.LoggerManager, log4net");

            if (loggerManagerType == null)
                Assert.Inconclusive("This test requires log4net.dll to be in the bin folder");

            var repository = loggerManagerType.GetMethod("GetRepository", new []{typeof (Assembly)}).Invoke(null, new[] {Assembly.GetExecutingAssembly()});

            var consoleAppender = Activator.CreateInstance("log4net", "log4net.Appender.ConsoleAppender").Unwrap();
            var consolePatternLayout = Activator.CreateInstance("log4net", "log4net.Layout.PatternLayout").Unwrap();
            consolePatternLayout.GetType().GetProperty("ConversionPattern").SetValue(consolePatternLayout, "%message%newline", null);
            consoleAppender.GetType().GetProperty("Layout").SetValue(consoleAppender, consolePatternLayout, null);
            consoleAppender.GetType().GetMethod("ActivateOptions").Invoke(consoleAppender, null);

            var root = repository.GetType().GetProperty("Root").GetValue(repository, null);
            root.GetType().GetMethod("RemoveAllAppenders").Invoke(root, null);
            root.GetType().GetMethod("AddAppender").Invoke(root, new[] {consoleAppender});
            repository.GetType().GetProperty("Configured").SetValue(repository, true, null);

            var logger = new InternalLog4netLogger(GetType());
            var enumType = Type.GetType("Amazon.Runtime.Internal.Util.InternalLog4netLogger+LoadState, AWSSDK");
            var enumValue = Enum.Parse(enumType, "Uninitialized");
            logger.GetType().GetField("loadState", BindingFlags.Static | BindingFlags.NonPublic).SetValue(logger, enumValue);

            var appenders = root.GetType().GetProperty("Appenders").GetValue(root, null);
            var appenderCount = 0;
            foreach (var appender in (IEnumerable)appenders)
            {
                appenderCount++;
                Assert.AreEqual("ConsoleAppender", appender.GetType().Name);
            }
            Assert.AreEqual(1, appenderCount);
        }
    }
}