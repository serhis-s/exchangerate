using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using ExchangeRate;
using ExchangeRate.Xml;
using Moq;
using Moq.Protected;
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

            var mockExchangeRateResponce = new ExchangeRateResponse
            {
                EURRate = "64",
                ResponseStatus = "OK",
                Source = "http://ya.ru",
                USDRate = "34"
            };
            var mockHttp = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            var mockResponseTransformer = new Mock<IResponseTransformer>();

            var cancelTokenSource = new CancellationTokenSource();
            var token = cancelTokenSource.Token;

            var urlRequest = new ExchangeRateSource();
            urlRequest.Url = "http://ya.ru";
            var xmlString = "someXmlString";


            mockHttp.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(xmlString)
                })
                .Verifiable();

            mockResponseTransformer.Setup(a => a.Transform(It.IsAny<byte[]>(), It.IsAny<string>()))
                .Returns(mockExchangeRateResponce);

            var client = new HttpClient(mockHttp.Object);
            var exchangeRateService = new ExchangeRateService(mockResponseTransformer.Object, client);
            var actualResult = exchangeRateService.AsyncLoadSingleExchangeRate(urlRequest, token);
            Assert.AreEqual(expectedResult, actualResult.Result.ToString());
        }


        [Test]
        public void AsyncLoadExchangeRateShouldReturnCanceledExceptionResult()
        {
            var expectedResult = " Отмена загрузки   иcточник= http://ya.ru";

            var mockHttp = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            var mockResponseTransformer = new Mock<IResponseTransformer>();

            var cancelTokenSource = new CancellationTokenSource();
            var token = cancelTokenSource.Token;
            var urlRequest = new ExchangeRateSource
            {
                Url = "http://ya.ru"
            };

            var mockExchangeRateResponset = new ExchangeRateResponse
            {
                EURRate = "",
                ResponseStatus = "",
                Source = "http://ya.ru",
                USDRate = ""
            };


            mockHttp.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                ).Throws(new OperationCanceledException(token))
                .Verifiable();

            mockResponseTransformer.Setup(a => a.Transform(It.IsAny<byte[]>(), It.IsAny<string>()))
                .Returns(mockExchangeRateResponset);

            var client = new HttpClient(mockHttp.Object);
            var exchangeRateService = new ExchangeRateService(mockResponseTransformer.Object, client);
            cancelTokenSource.Cancel();
            var actualResult = exchangeRateService.AsyncLoadSingleExchangeRate(urlRequest, token);
            Assert.AreEqual(expectedResult, actualResult.Result.ToString());
        }


        [Test]
        public void AsyncLoadExchangeRateHttpTimeoutExceptionShouldReturnHttpTimeoutExceptionResult()
        {
            var expectedResult = " Таймаут привышен   иcточник= http://ya.ru";

            var mockHttp = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            var mockResponseTransformer = new Mock<IResponseTransformer>();

            var cancelTokenSource = new CancellationTokenSource();
            var token = cancelTokenSource.Token;
            var urlRequest = new ExchangeRateSource();
            urlRequest.Url = "http://ya.ru";

            var mockExchangeRateResponset = new ExchangeRateResponse
            {
                EURRate = "",
                ResponseStatus = "",
                Source = "http://ya.ru",
                USDRate = ""
            };


            mockHttp.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                ).Throws(new OperationCanceledException())
                .Verifiable();

            mockResponseTransformer.Setup(a => a.Transform(It.IsAny<byte[]>(), It.IsAny<string>()))
                .Returns(mockExchangeRateResponset);

            var client = new HttpClient(mockHttp.Object);
            var exchangeRateService = new ExchangeRateService(mockResponseTransformer.Object, client);
            var actualResult = exchangeRateService.AsyncLoadSingleExchangeRate(urlRequest, token);
            Assert.AreEqual(expectedResult, actualResult.Result.ToString());
        }

        [Test]
        public void AsyncLoadExchangeRateDifferentExceptionShouldReturnHttpDifferentExceptionResult()
        {
            var expectedResult = " bad request  иcточник= http://ya.ru";

            var mockHttp = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            var mockResponseTransformer = new Mock<IResponseTransformer>();

            var cancelTokenSource = new CancellationTokenSource();
            var token = cancelTokenSource.Token;
            var urlRequest = new ExchangeRateSource();
            urlRequest.Url = "http://ya.ru";

            var mockExchangeRateResponset = new ExchangeRateResponse
            {
                EURRate = "",
                ResponseStatus = "",
                Source = "http://ya.ru",
                USDRate = ""
            };


            mockHttp.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                ).Throws(new HttpRequestException("bad request"))
                .Verifiable();

            mockResponseTransformer.Setup(a => a.Transform(It.IsAny<byte[]>(), It.IsAny<string>()))
                .Returns(mockExchangeRateResponset);

            var client = new HttpClient(mockHttp.Object);
            var exchangeRateService = new ExchangeRateService(mockResponseTransformer.Object, client);
            var actualResult = exchangeRateService.AsyncLoadSingleExchangeRate(urlRequest, token);
            Assert.AreEqual(expectedResult, actualResult.Result.ToString());
        }
    }
}