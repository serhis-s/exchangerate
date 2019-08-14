using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ExchangeRate
{
    public class HttpClientProvider : IClientProvider
    {
        private readonly HttpClient _httpClient;

        public HttpClientProvider(HttpClient httpClient)
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