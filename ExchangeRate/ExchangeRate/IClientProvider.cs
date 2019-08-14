using System.Threading;
using System.Threading.Tasks;

namespace ExchangeRate
{
    public interface IClientProvider
    {
        Task<byte[]> GetResponseContext(ExchangeRateSource exchangeRateSource, CancellationToken token);
    }
}