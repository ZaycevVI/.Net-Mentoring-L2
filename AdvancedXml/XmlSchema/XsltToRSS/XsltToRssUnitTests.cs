using System;
using System.Xml.Xsl;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace XsltToRSS
{
    [TestClass]
    public class XsltToRssUnitTests
    {
        [TestMethod]
        public void XsltToRssTest()
        {
            var xsl = new XslCompiledTransform();

            xsl.Load("XsltToRss.xslt");

            xsl.Transform("books.xml", null, Console.Out);
        }
    }
}
