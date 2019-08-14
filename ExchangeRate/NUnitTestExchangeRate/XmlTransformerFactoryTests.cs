using System;
using ExchangeRate;
using ExchangeRate.Xml;
using ExchangeRateApp;
using Ninject;
using NUnit.Framework;

namespace Tests
{
    public class XmlTransformerFactoryTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void GetResponseTransformerShouldReturnTypeXmlResponseTransformerNbkr()
        {
            var expectedResult = new XmlResponseTransformerNbkr();
            var transformFactory = new XmlTransformerFactory();
            var exchangeRateSource = new ExchangeRateSource
            {
                Url = "http://ya.ru",
                SourceType = SourceTypeEnum.NBKR
            };

            var ac = transformFactory.GetResponseTransformer(exchangeRateSource);

            Assert.AreEqual(expectedResult.GetType(), ac.GetType());
        }

        [Test]
        public void GetResponseTransformerShouldReturnTypeXmlResponseTransformerCbr()
        {
            var expectedResult = new XmlResponseTransformerCbr();
            var transformFactory = new XmlTransformerFactory();
            var exchangeRateSource = new ExchangeRateSource
            {
                Url = "http://ya.ru",
                SourceType = SourceTypeEnum.CBR
            };

            var ac = transformFactory.GetResponseTransformer(exchangeRateSource);

            Assert.AreEqual(expectedResult.GetType(), ac.GetType());
        }

        [Test]
        public void GetResponseTransformerShouldReturnTypeXmlResponseException()
        {
            var expectedResult = "wrong source type";
            var actualResult = "";
            var transformFactory = new XmlTransformerFactory();
            var exchangeRateSource = new ExchangeRateSource
            {
                Url = "http://ya.ru",
                SourceType = SourceTypeEnum.TEST
            };

            try
            {
                transformFactory.GetResponseTransformer(exchangeRateSource);
            }
            catch (Exception ex)
            {
                actualResult = ex.Message;
            }

            Assert.AreEqual(expectedResult, actualResult);
        }
    }
}