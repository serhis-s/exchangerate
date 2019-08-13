using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ExchangeRate;
using ExchangeRate.Xml;
using Moq;
using NUnit.Framework;

namespace Tests
{
    public class ExchangeRateServiceTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void AsyncLoadExchangeRateShouldReturnGoodResult()
        {
            var expectedResult = "курс  USD= 34 EUR= 64  иcточник= http://ya.ru";

            var cancelTokenSource = new CancellationTokenSource();
            var token = cancelTokenSource.Token;
            var urlRequest = new ExchangeRateSource();
            urlRequest.Url = "http://ya.ru";
            var xmlString = "someString";
            var bytesArrayResponse = Encoding.ASCII.GetBytes("some string");
            var list = new List<ExchangeRateSource>
            {
                new ExchangeRateSource
                {
                    Url = "http://ya.ru",
                    SourceType = SourseTypeEnum.CBR
                }
            };

            var mockLogger = new Mock<ILogger>();

            var mockExchangeRateResponce = new ExchangeRateResponse
            {
                EURRate = "64",
                ResponseStatus = ResponseStatus.OK,
                Source = "http://ya.ru",
                USDRate = "34"
            };

            var mockResponseTransformer = new Mock<IResponseTransformer>();
            mockResponseTransformer.Setup(a => a.Transform(It.IsAny<byte[]>(), It.IsAny<ExchangeRateSource>()))
                .Returns(mockExchangeRateResponce);

            var mockTransformFactory = new Mock<ITransformerFactory>();
            mockTransformFactory.Setup(a => a.GetResponseTransformer(It.IsAny<ExchangeRateSource>()))
                .Returns(mockResponseTransformer.Object);

            var mockProvider = new Mock<IProvider>();
            mockProvider.Setup(a => a.GetResponseContext(It.IsAny<ExchangeRateSource>(), token))
                .ReturnsAsync(bytesArrayResponse);

            var exchangeRateService =
                new ExchangeRateService(mockTransformFactory.Object, mockProvider.Object, mockLogger.Object);
            var actualResult = exchangeRateService.AsyncLoadExchangeRate(list, token);
            Assert.AreEqual(expectedResult, actualResult.Result.ToString());
        }


        [Test]
        public async Task AsyncLoadExchangeRateShouldLogCanceledTaskException()
        {
            var expectedResult = " Задание отменено  иcточник= http://ya.ru";

            var cancelTokenSource = new CancellationTokenSource();
            var token = cancelTokenSource.Token;

            var urlRequest = new ExchangeRateSource();
            urlRequest.Url = "http://ya.ru";
            var xmlString = "someString";
            var bytesArrayResponse = Encoding.ASCII.GetBytes("some string");
            var list = new List<ExchangeRateSource>
            {
                new ExchangeRateSource
                {
                    Url = "http://ya.ru",
                    SourceType = SourseTypeEnum.CBR
                },
                new ExchangeRateSource
                {
                    Url = "http://ya.ru",
                    SourceType = SourseTypeEnum.NBKR
                }
            };

            var mockLogger = new Mock<ILogger>();
            var actualResult = "";
            mockLogger.Setup(a => a.AddLog(It.IsAny<ExchangeRateResponse>())).Callback<ExchangeRateResponse>(ex =>
                actualResult = ex.ToString());

            var mockExchangeRateResponce = new ExchangeRateResponse
            {
                EURRate = "64",
                ResponseStatus = ResponseStatus.OK,
                Source = "http://ya.ru",
                USDRate = "34"
            };

            var mockResponseTransformer = new Mock<IResponseTransformer>();
            mockResponseTransformer.Setup(a => a.Transform(It.IsAny<byte[]>(), It.IsAny<ExchangeRateSource>()))
                .Returns(mockExchangeRateResponce);

            var mockTransformFactory = new Mock<ITransformerFactory>();
            mockTransformFactory.Setup(a => a.GetResponseTransformer(It.IsAny<ExchangeRateSource>()))
                .Returns(mockResponseTransformer.Object);

            var mockProvider = new Mock<IProvider>();

            mockProvider.Setup(a =>
                    a.GetResponseContext(It.IsAny<ExchangeRateSource>(), token))
                .ReturnsAsync(bytesArrayResponse);

            var exchangeRateService =
                new ExchangeRateService(mockTransformFactory.Object, mockProvider.Object, mockLogger.Object);

            var firs = await exchangeRateService.AsyncLoadExchangeRate(list, token);
            cancelTokenSource.Cancel();
            Assert.AreEqual(expectedResult, actualResult);
        }


        [Test]
        public async Task AsyncLoadExchangeRateShouldLogCanceledTimeoutException()
        {
            var expectedResult = " Задание отменено из за таймаута иcточник= http://ya.ru";

            var cancelTokenSource = new CancellationTokenSource();
            var token = cancelTokenSource.Token;
            var list = new List<ExchangeRateSource>
            {
                new ExchangeRateSource
                {
                    Url = "http://ya.ru",
                    SourceType = SourseTypeEnum.CBR
                }
            };

            var mockLogger = new Mock<ILogger>();
            var actualResult = "";
            mockLogger.Setup(a => a.AddLog(It.IsAny<ExchangeRateResponse>())).Callback<ExchangeRateResponse>(ex =>
                actualResult = ex.ToString());

            var mockTransformFactory = new Mock<ITransformerFactory>();
            var mockProvider = new Mock<IProvider>();
            mockProvider.Setup(a => a.GetResponseContext(It.IsAny<ExchangeRateSource>(), token))
                .Throws(new OperationCanceledException());

            var exchangeRateService =
                new ExchangeRateService(mockTransformFactory.Object, mockProvider.Object, mockLogger.Object);

            var firs = await exchangeRateService.AsyncLoadExchangeRate(list, token);
            Assert.AreEqual(expectedResult, actualResult);
        }
    }
}