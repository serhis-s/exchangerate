using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ExchangeRate
{
    public class HttpExchangeRateClient : IExchangeRateClient
    {
        private readonly HttpClient _httpClient;

        public HttpExchangeRateClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<byte[]> GetResponseContext(ExchangeRateSource exchangeRateSource, CancellationToken token)
        {
            using (var httpResponse = await _httpClient.GetAsync(exchangeRateSource.Url, token))
            {
                var responseContext = await httpResponse.Content.ReadAsByteArrayAsync();
                return responseContext;
            }
        }
    }
}