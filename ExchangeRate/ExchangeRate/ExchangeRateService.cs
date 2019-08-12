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
        ///     Thread safe
        /// </summary>
        private readonly ILogger _logger;

        private readonly IProvider _provider;
        private readonly IFactory _transformerFactory;


        public ExchangeRateService(IFactory transformerFactory, IProvider provider, ILogger logger)
        {
            _transformerFactory = transformerFactory;
            _provider = provider;
            _logger = logger;
        }


        public async Task<ExchangeRateResponse> AsyncLoadExchangeRate(List<ExchangeRateSource> list,
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
                await Task.Delay(new Random().Next(1000, 3000), token);
                var responseTransformer = _transformerFactory.GetResponseTransformer(urlRequest);
                var result = responseTransformer.Transform(response, urlRequest);
                token.ThrowIfCancellationRequested();
                return result;
            }

            catch (OperationCanceledException ex)
            {
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