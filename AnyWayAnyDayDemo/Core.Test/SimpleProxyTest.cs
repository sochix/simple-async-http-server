using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleServer.Core;

namespace SimpleServerTest.Core.Test
{
    [TestClass]
    public class SimpleProxyTest
    {
        [TestMethod]
        public void ProxifySimplePageTest()
        {
            var proxy = new SimpleProxy();
            var result = proxy.Proxify("http://ya.ru");

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ProxifyEmptyUrlTest()
        {
            var proxy = new SimpleProxy();
            Assert.IsNull(proxy.Proxify(""));            
        }

        [TestMethod]
        public void ProxifyNullUrlTest()
        {
            var proxy = new SimpleProxy();
            Assert.IsNull(proxy.Proxify(null));
        }

        [TestMethod]
        public void ProxifyWrongUrlFormatTest()
        {
            var proxy = new SimpleProxy();
            Assert.IsNull(proxy.Proxify("ftp://ya.ru"));
        }

        [TestMethod]
        public void ProxifyNotExsistedIpTest()
        {
            var proxy = new SimpleProxy();
            Assert.IsNull(proxy.Proxify("256.256.256.0"));
        }

        [TestMethod]
        public void ProxifyNotExsistedUrlTest()
        {
            var proxy = new SimpleProxy();
            Assert.IsNull(proxy.Proxify("http://somethingnotexsisted.really"));
        }        
    }
}
