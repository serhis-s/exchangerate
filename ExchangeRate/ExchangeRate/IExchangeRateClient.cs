using System.Threading;
using System.Threading.Tasks;

namespace ExchangeRate
{
    public interface IExchangeRateClient
    {
        Task<byte[]> GetResponseContext(ExchangeRateSource exchangeRateSource, CancellationToken token);
    }
}