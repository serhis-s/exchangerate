using System.Text;
using ExchangeRate;
using ExchangeRate.Xml;
using NUnit.Framework;

namespace Tests
{
    public class XmlResponseTransformerCbrTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TransformShouldReturnGoodExpectedResult()
        {
            var expectedValue = "курс  USD= 40 EUR= 60  иcточник= http://ya.ru";
            var xmlResponseTransformer = new XmlResponseTransformerCbr();
            var exchangeRateSource = new ExchangeRateSource
            {
                Url = "http://ya.ru",
                SourceType = SourceTypeEnum.CBR
            };

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
            var actualValue = xmlResponseTransformer.Transform(bytesArrayResponse, exchangeRateSource);
            Assert.AreEqual(expectedValue, actualValue.ToString());
        }


        [Test]
        public void TransformShouldReturnExceptionResult()
        {
            var expectedValue =
                " ошибка = Unexpected end of file has occurred. The following elements are not closed: Valute, ValCurs. Line 16, position 17. иcточник= http://ya.ru";
            var xmlResponseTransformer = new XmlResponseTransformerCbr();
            var exchangeRateSource = new ExchangeRateSource
            {
                Url = "http://ya.ru",
                SourceType = SourceTypeEnum.CBR
            };

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
                ";
            var bytesArrayResponse = Encoding.ASCII.GetBytes(StringResponse);
            var actualValue = xmlResponseTransformer.Transform(bytesArrayResponse, exchangeRateSource);
            Assert.AreEqual(expectedValue, actualValue.ToString());
        }
    }
}