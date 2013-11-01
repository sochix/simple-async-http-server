using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleServer;

namespace SimpleServerTest
{
    [TestClass]
    public class XmlSettingsReaderTest
    {

        [TestMethod]
        public void SimpleXmlSettingsReaderTest()
        {
            var settings = XmlSettingsReader.ReadSettings("ServerSettingsForTests\\server_config.xml");
            Assert.IsNotNull(settings);
            Assert.AreEqual(8888, settings.Port);
            Assert.IsFalse(settings.UseSqlDb);
            Assert.IsTrue(settings.PathToSqlFile.Equals("D:\\test.sqlite"));
            Assert.IsTrue(settings.PathToXmlFile.Equals("D:\\test.xml"));
        }

        [TestMethod]
        public void XmlSettingsReaderFromFilWithTxtExtensionTest()
        {
            var settings = XmlSettingsReader.ReadSettings("ServerSettingsForTests\\server_config_as_txt.txt");
            Assert.IsNotNull(settings);
            Assert.AreEqual(8888, settings.Port);
            Assert.IsFalse(settings.UseSqlDb);
            Assert.IsTrue(settings.PathToSqlFile.Equals("D:\\test.sqlite"));
            Assert.IsTrue(settings.PathToXmlFile.Equals("D:\\test.xml"));
        }

        [TestMethod]
        public void IncorrectSettingsFileTest()
        {
            bool wasException = false;
            try
            {
                var settings = XmlSettingsReader.ReadSettings("ServerSettingsForTests\\incorrect_server_config.xml");
            }
            catch (SimpleServerException)
            {
                wasException = true;
            }
            finally
            {
                Assert.IsTrue(wasException);
            }
            
        }

        [TestMethod]
        public void EmptyPathXmlSettingsReaderTest()
        {
            var settings = XmlSettingsReader.ReadSettings("");
            Assert.IsNull(settings);

            settings = XmlSettingsReader.ReadSettings(null);
            Assert.IsNull(settings);
        }

        [TestMethod]
        public void NotExsistingFileXmlSettingsReaderTest()
        {
            bool wasException = false;
            try
            {
                var settings = XmlSettingsReader.ReadSettings("not_exsisting_file.xml");
            }
            catch (FileNotFoundException)
            {
                wasException = true;
            }
            finally
            {
                Assert.IsTrue(wasException);   
            }            
        }
    }
}
