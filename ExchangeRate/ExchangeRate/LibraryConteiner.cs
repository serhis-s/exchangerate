using System.Configuration;
using ExchangeRate.Xml;
using Ninject.Modules;

namespace ExchangeRate
{
    public class LibraryModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IClientProvider>().To<HttpClientProvider>();
            Bind<ITransformerFactory>().To<XmlTransformerFactory>();
            Bind<ILogger>().To<Logger>().InSingletonScope()
                .WithConstructorArgument("logPath", ConfigurationManager.AppSettings["LogPath"]);
            Bind<ExchangeRateService>().ToSelf();
        }
    }
}