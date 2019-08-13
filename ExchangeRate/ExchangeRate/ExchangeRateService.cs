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
        private readonly ITransformerFactory _transformerTransformerFactory;


        public ExchangeRateService(ITransformerFactory transformerTransformerFactory, IProvider provider,
            ILogger logger)
        {
            _transformerTransformerFactory = transformerTransformerFactory;
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
                token.ThrowIfCancellationRequested();
                var response = await _provider.GetResponseContext(urlRequest, token);
                await Task.Delay(new Random().Next(1000, 3000), token);
                var responseTransformer = _transformerTransformerFactory.GetResponseTransformer(urlRequest);
                var result = responseTransformer.Transform(response, urlRequest);
                token.ThrowIfCancellationRequested();
                return result;
            }

            catch (OperationCanceledException)
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
        }
    }
}