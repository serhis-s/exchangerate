using System.Text;
using ExchangeRate.Xml;
using NUnit.Framework;

namespace Tests
{
    public class XmlResponseTransformerTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TransformShouldReturnGoodExpectedResult()
        {
            var expectedValue = "курс  USD= 40 EUR= 60  иcточник= http://ya.ru";
            var xmlResponseTransformer = new XmlResponseTransformer();

            var StringResponse = @"<?xml version=""1.0"" encoding=""windows - 1251""?>
                <ValCurs Date=""06.08.2019"" name=""Foreign Currency Market"">
                <Valute ID = ""R01010"">
                <NumCode>036</NumCode>
                <CharCode>USD</CharCode>
                <Nominal>1</Nominal>
                <Name>euro</Name>
                <Value>40</Value>
                </Valute>
                <Valute ID = ""R01020A"">
                <NumCode>944</NumCode>
                <CharCode>EUR</CharCode>
                <Nominal>1</Nominal>
                <Name>baks</Name>
                <Value>60</Value>
                </Valute>
                <Valute ID = ""R01050A"">
                <NumCode>923</NumCode>
                <CharCode>TYG</CharCode>
                <Nominal>1</Nominal>
                <Name>tygrig</Name>
                <Value>666</Value>
                </Valute>
                </ValCurs>";
            var bytesArrayResponse = Encoding.ASCII.GetBytes(StringResponse);
            var actualValue = xmlResponseTransformer.Transform(bytesArrayResponse, "http://ya.ru");
            Assert.AreEqual(expectedValue, actualValue.ToString());
        }


        [Test]
        public void TransformWrongXMLSchemeShouldReturnExceptionResponse()
        {
            var expectedValue =
                " The element 'Valute' has invalid child element 'Value'. List of possible elements expected: 'CharCode'.  иcточник= http://ya.ru";
            var xmlService = new XmlResponseTransformer();
            var StringResponse = @"<?xml version=""1.0"" encoding=""windows - 1251""?>
                <ValCurs Date=""06.08.2019"" name=""Foreign Currency Market"">
                <Valute ID = ""R01010"">
                <NumCode>036</NumCode>
                <Value>60</Value>
                </Valute>
                </ValCurs>";
            var bytesArrayResponse = Encoding.ASCII.GetBytes(StringResponse);
            var actualValue = xmlService.Transform(bytesArrayResponse, "http://ya.ru");
            Assert.AreEqual(expectedValue, actualValue.ToString());
        }
    }
}