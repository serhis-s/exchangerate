using System.Threading;
using System.Threading.Tasks;

namespace ExchangeRate
{
    public interface IProvider
    {
        Task<byte[]> GetResponseContext(ExchangeRateSource exchangeRateSource, CancellationToken token);
    }
}