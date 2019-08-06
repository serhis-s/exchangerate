using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using ExchangeRate;
using ExchangeRate.Xml;

namespace ExchangeRateApp
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            var cancelTokenSource = new CancellationTokenSource();
            var token = cancelTokenSource.Token;
            var logger = new Logger(ConfigurationManager.AppSettings["LogPath"]);
            var responseTransformer = new XmlResponseTransformer();
            var list = SourceLoader.GetSources();
            var exchangeRateService = new ExchangeRateService(responseTransformer, new HttpClient());
            var taskList = new List<Task<ExchangeRateResponse>>();
            foreach (var urlRequest in list)
                taskList.Add(Task.Run(async () =>
                        await exchangeRateService.AsyncLoadExchangeRate(urlRequest, token)
                    , token));

            var firstTask = await Task.WhenAny(taskList);
            cancelTokenSource.Cancel();

            await Task.WhenAll(taskList);

            foreach (var task in taskList) logger.AddLog(task.Result);

            Console.WriteLine(firstTask.Result.ToString());
            Console.ReadKey();
        }
    }
}