using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ExchangeRate.Xml;

namespace ExchangeRate
{
    public class ExchangeRateService
    {
        /// <summary>
        /// Thread safe
        /// </summary>
        private readonly ILogger _logger;
        private readonly IProvider _provider;
        private readonly IResponseTransformer _responseTransformer;


        public ExchangeRateService(IResponseTransformer responseTransformer, IProvider provider, ILogger logger)
        {
            _responseTransformer = responseTransformer;
            _provider = provider;
            _logger = logger;
        }


        public async Task<ExchangeRateResponse> AsyncLoadSingleExchangeRate(List<ExchangeRateSource> list,
            CancellationToken token)
        {
            var taskList = new List<Task<ExchangeRateResponse>>();
            foreach (var urlRequest in list)
                taskList.Add(Task.Run(async () =>
                        await AsyncLoadSingleExchangeRate(urlRequest, token)
                    , token));
            var firstTask = await Task.WhenAny(taskList);
            return firstTask.Result;
        }


        private async Task<ExchangeRateResponse> AsyncLoadSingleExchangeRate(ExchangeRateSource urlRequest,
            CancellationToken token)
        {
            try
            {
                var response = await _provider.GetResponseContext(urlRequest, token);
                token.ThrowIfCancellationRequested();
                await Task.Delay(new Random().Next(5000), token);
                var result = _responseTransformer.Transform(response, urlRequest);
                token.ThrowIfCancellationRequested();
                return result;
            }

            catch (OperationCanceledException ex)
            {
                Console.WriteLine(ex.Message);
                ExchangeRateResponse result;
                if (token.IsCancellationRequested)
                    result = new ExchangeRateResponse
                    {
                        ResponseStatus = ResponseStatus.TaskCanceled,
                        Source = urlRequest.Url
                    };
                else
                    result = new ExchangeRateResponse
                    {
                        ResponseStatus = ResponseStatus.ClientTimeOut,
                        Source = urlRequest.Url
                    };

                lock (_logger)
                {
                    _logger.AddLog(result);
                }

                return null;
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                var result = new ExchangeRateResponse
                {
                    ResponseStatus = ResponseStatus.OtherException,
                    Source = urlRequest.Url
                };
                return result;
            }
        }
    }
}