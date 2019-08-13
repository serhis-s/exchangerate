using System;
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
            var transformerFactory = new XmlTransformerFactory();
            var provider = new HttpProvider(new HttpClient());
            var list = SourceLoader.GetSources();
            var exchangeRateService = new ExchangeRateService(transformerFactory, provider, logger);
            var result = await exchangeRateService.AsyncLoadExchangeRate(list, token);
            cancelTokenSource.Cancel();
            logger.AddLog(result);
            Console.ReadKey();
        }
    }
}