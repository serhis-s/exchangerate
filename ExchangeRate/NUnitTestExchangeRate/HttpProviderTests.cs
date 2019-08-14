using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ExchangeRate;
using Moq;
using Moq.Protected;
using NUnit.Framework;

namespace Tests
{
    public class HttpProviderTests
    {
        [Test]
        public void TransformShouldReturnExceptionResult()
        {
            var expectedResult = "some string";
            var responseString = "some string";
            var cancelTokenSource = new CancellationTokenSource();
            var token = cancelTokenSource.Token;
            var exchangeRateSource = new ExchangeRateSource();
            exchangeRateSource.Url = "http://ya.ru";

            var mockHttp = new Mock<HttpClientHandler>();

            mockHttp.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(responseString)
                })
                .Verifiable();
            var client = new HttpClient(mockHttp.Object);
            var httpProvider = new HttpClientProvider(client);
            var responseContext = httpProvider.GetResponseContext(exchangeRateSource, token);
            var actualResult = Encoding.UTF8.GetString(responseContext.Result);

            Assert.AreEqual(expectedResult, actualResult);
        }
    }
}