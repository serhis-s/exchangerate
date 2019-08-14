namespace ExchangeRate.Xml
{
    public interface IResponseTransformer
    {
        ExchangeRateResponse Transform(byte[] byteArray, ExchangeRateSource exchangeRateSource);
    }
}