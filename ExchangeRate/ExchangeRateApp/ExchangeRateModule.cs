using System.Configuration;
using System.Net.Http;
using ExchangeRate;
using ExchangeRate.Xml;
using Ninject.Modules;

namespace ExchangeRateApp
{
    public class ExchangeRateModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IExchangeRateClient>().To<HttpExchangeRateClient>();
            Bind<ITransformerFactory>().To<XmlTransformerFactory>();
            Bind<HttpExchangeRateClient>().ToSelf().WithConstructorArgument(new HttpClient());
            Bind<ILogger>().To<Logger>().InSingletonScope()
                .WithConstructorArgument("logPath", ConfigurationManager.AppSettings["LogPath"]);
            Bind<ExchangeRateService>().ToSelf();
        }
    }
}