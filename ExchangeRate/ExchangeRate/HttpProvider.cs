using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ExchangeRate
{
    public class HttpProvider : IProvider
    {
        private readonly HttpClient _httpClient;

        public HttpProvider(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<byte[]> GetResponseContext(ExchangeRateSource exchangeRateSource, CancellationToken token)
        {
            try
            {
                using (var httpResponse = await _httpClient.GetAsync(exchangeRateSource.Url, token))
                {
                    var responseContext = await httpResponse.Content.ReadAsByteArrayAsync();
                    return responseContext;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}