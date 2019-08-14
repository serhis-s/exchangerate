using System;
using System.Threading;
using System.Threading.Tasks;
using ExchangeRate;
using Ninject;

namespace ExchangeRateApp
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            var kernel = new StandardKernel(new LibraryConteiner());

            var cancelTokenSource = new CancellationTokenSource();
            var token = cancelTokenSource.Token;
            var logger = kernel.Get<ILogger>();
            var list = SourceLoader.GetSources();
            var exchangeRateService = kernel.Get<ExchangeRateService>();
            var result = await exchangeRateService.AsyncLoadExchangeRate(list, token);
            cancelTokenSource.Cancel();
            logger.AddLog(result);
            Console.ReadKey();
        }
    }
}