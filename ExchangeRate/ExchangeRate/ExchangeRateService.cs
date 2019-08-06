using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using ExchangeRate.Xml;

namespace ExchangeRate
{
    public class ExchangeRateService
    {
        private readonly HttpClient httpClient;
        private readonly IResponseTransformer responseTransformer;


        public ExchangeRateService(IResponseTransformer responseTransformer, HttpClient httpClient)
        {
            this.responseTransformer = responseTransformer;
            this.httpClient = httpClient;
        }

        public async Task<ExchangeRateResponse> AsyncLoadExchangeRate(ExchangeRateSource urlRequest,
            CancellationToken token)
        {
            try
            {
                using (var response = await httpClient.GetAsync(urlRequest.Url))
                {
                    token.ThrowIfCancellationRequested();
                    await Task.Delay(new Random().Next(2000), token);
                    var responseContent = response.Content.ReadAsByteArrayAsync();
                    var result = responseTransformer.Transform(responseContent.Result, urlRequest.Url);
                    token.ThrowIfCancellationRequested();
                    return result;
                }
            }

            catch (OperationCanceledException ex)
            {
                Console.WriteLine(ex.Message);
                if (token.IsCancellationRequested)
                {
                    var result = new ExchangeRateResponse
                    {
                        ResponseStatus = "Отмена загрузки ",
                        Source = urlRequest.Url
                    };
                    return result;
                }
                else
                {
                    var result = new ExchangeRateResponse
                    {
                        ResponseStatus = "Таймаут привышен ",
                        Source = urlRequest.Url
                    };
                    return result;
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                var result = new ExchangeRateResponse
                {
                    ResponseStatus = ex.Message,
                    Source = urlRequest.Url
                };
                return result;
            }
        }
    }
}