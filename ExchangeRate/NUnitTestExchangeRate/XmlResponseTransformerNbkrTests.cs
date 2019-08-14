using System.Text;
using ExchangeRate;
using ExchangeRate.Xml;
using NUnit.Framework;

namespace Tests
{
    public class XmlResponseTransformerNbkrTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TransformShouldReturnGoodExpectedResult()
        {
            var expectedValue = "курс  USD= 40 EUR= 60  иcточник= http://ya.ru";
            var xmlResponseTransformer = new XmlResponseTransformerNbkr();
            var exchangeRateSource = new ExchangeRateSource
            {
                Url = "http://ya.ru",
                SourceType = SourceTypeEnum.NBKR
            };

            var StringResponse = @"<?xml version=""1.0"" encoding=""windows - 1251""?>
                <CurrencyRates Name=""Daily Exchange Rates"" Date=""13.08.2019"">
                <Currency ISOCode = ""USD"">
                <Nominal> 1 </Nominal>
                <Value>40</Value>
                </Currency>
                <Currency ISOCode = ""EUR"">
                <Nominal> 1 </Nominal>
                <Value>60</Value>
                </Currency>
                <Currency ISOCode = ""KZT"">
                <Nominal> 1 </Nominal>
                <Value>0,1801</Value>
                </Currency>
                <Currency ISOCode = ""RUB"">
                <Nominal>1 </Nominal>
                <Value>1,0696</Value>
                </Currency>
                </CurrencyRates>";
            var bytesArrayResponse = Encoding.ASCII.GetBytes(StringResponse);
            var actualValue = xmlResponseTransformer.Transform(bytesArrayResponse, exchangeRateSource);
            Assert.AreEqual(expectedValue, actualValue.ToString());
        }

        [Test]
        public void TransformShouldReturnGoodExceptionResult()
        {
            var expectedValue =
                " ошибка = Unexpected end of file has occurred. The following elements are not closed: CurrencyRates. Line 18, position 28. иcточник= http://ya.ru";
            var xmlResponseTransformer = new XmlResponseTransformerNbkr();
            var exchangeRateSource = new ExchangeRateSource
            {
                Url = "http://ya.ru",
                SourceType = SourceTypeEnum.NBKR
            };

            var StringResponse = @"<?xml version=""1.0"" encoding=""windows - 1251""?>
                <CurrencyRates Name=""Daily Exchange Rates"" Date=""13.08.2019"">
                <Currency ISOCode = ""USD"">
                <Nominal> 1 </Nominal>
                <Value>40</Value>
                </Currency>
                <Currency ISOCode = ""EUR"">
                <Nominal> 1 </Nominal>
                <Value>60</Value>
                </Currency>
                <Currency ISOCode = ""KZT"">
                <Nominal> 1 </Nominal>
                <Value>0,1801</Value>
                </Currency>
                <Currency ISOCode = ""RUB"">
                <Nominal>1 </Nominal>
                <Value>1,0696</Value>
                </Currency>";
            var bytesArrayResponse = Encoding.ASCII.GetBytes(StringResponse);
            var actualValue = xmlResponseTransformer.Transform(bytesArrayResponse, exchangeRateSource);
            Assert.AreEqual(expectedValue, actualValue.ToString());
        }
    }
}