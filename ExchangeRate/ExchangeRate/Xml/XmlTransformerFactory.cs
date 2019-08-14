using System;

namespace ExchangeRate.Xml
{
    public class XmlTransformerFactory : ITransformerFactory
    {
        public IResponseTransformer GetResponseTransformer(ExchangeRateSource exchangeRateSource)
        {
            switch (exchangeRateSource.SourceType)
            {
                case SourceTypeEnum.CBR:
                    return new XmlResponseTransformerCbr();
                case SourceTypeEnum.NBKR:
                    return new XmlResponseTransformerNbkr();
                default:
                    throw new Exception("wrong source type");
            }
        }
    }
}