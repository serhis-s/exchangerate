namespace ExchangeRate.Xml
{
    public interface ITransformerFactory
    {
        IResponseTransformer GetResponseTransformer(ExchangeRateSource exchangeRateSource);
    }
}