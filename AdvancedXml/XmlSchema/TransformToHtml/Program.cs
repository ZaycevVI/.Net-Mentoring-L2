using System.IO;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace TransformToHtml
{
    class Program
    {
        public static void Main()
        {
            HtmlTransform();
        }

        private static void HtmlTransform()
        {
            var settings = new XsltSettings { EnableScript = true };
            var xsl = new XslCompiledTransform();
            xsl.Load("books.xslt", settings, null);

            var stream = new FileStream(@"D:\1.html", FileMode.Create);
            var doc = new XPathDocument("books.xml");

            xsl.Transform(doc, null, stream);
        }
    }
}
