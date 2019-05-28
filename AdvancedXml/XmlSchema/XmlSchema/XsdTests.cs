using System;
using System.Activities;
using System.Xml;
using System.Xml.Schema;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace XmlSchema
{
    [TestClass]
    public class XsdTests
    {
        private XmlReaderSettings _settings;

        [TestInitialize]
        public void Init()
        {
            _settings = new XmlReaderSettings();

            _settings.Schemas.Add("http://library.by/catalog", "books.xsd");
            _settings.ValidationEventHandler += (sender, e) =>
                {
                    Console.WriteLine("[{0}:{1}] {2}",
                        e.Exception.LineNumber, e.Exception.LinePosition, e.Message);

                    throw new ValidationException($"[{e.Exception.LineNumber}:" +
                                                  $"{e.Exception.LinePosition}] " +
                                                  $"{e.Message}");
                };

            _settings.ValidationFlags = _settings.ValidationFlags | XmlSchemaValidationFlags.ReportValidationWarnings;
            _settings.ValidationType = ValidationType.Schema;
        }


        [TestMethod]
        public void IsbnPatternCheck()
        {
            var reader = XmlReader.Create("books.xml", _settings);

            while (reader.Read());
        }

        [TestMethod]
        public void IsbnPatternCheckFailed()
        {
            var reader = XmlReader.Create("booksIsbnIncorrect.xml", _settings);

            try
            {
                while (reader.Read());
            }
            catch (ValidationException)
            {
                Assert.AreEqual(true, true);
                return;
            }

            Assert.AreEqual(false, true);
        }

        [TestMethod]
        public void GenreCheckFailed()
        {
            var reader = XmlReader.Create("booksGenreIncorrect.xml", _settings);

            try
            {
                while (reader.Read());
            }
            catch (ValidationException)
            {
                Assert.AreEqual(true, true);
                return;
            }

            Assert.AreEqual(false, true);
        }

        [TestMethod]
        public void PublishDateCheckFailed()
        {
            var reader = XmlReader.Create("booksDateIncorrect.xml", _settings);

            try
            {
                while (reader.Read()) ;
            }
            catch (ValidationException)
            {
                Assert.AreEqual(true, true);
                return;
            }

            Assert.AreEqual(false, true);
        }

        [TestMethod]
        public void UniqueIdCheckFailed()
        {
            var reader = XmlReader.Create("booksIdIncorrect.xml", _settings);

            try
            {
                while (reader.Read()) ;
            }
            catch (ValidationException)
            {
                Assert.AreEqual(true, true);
                return;
            }

            Assert.AreEqual(false, true);
        }
    }
}
