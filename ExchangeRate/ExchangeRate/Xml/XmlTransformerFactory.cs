using System;

namespace ExchangeRate.Xml
{
    public class XmlTransformerFactory : ITransformerFactory
    {
        public IResponseTransformer GetResponseTransformer(ExchangeRateSource exchangeRateSource)
        {
            switch (exchangeRateSource.SourceType)
            {
                case SourseTypeEnum.CBR:
                    return new XmlResponseTransformerCbr();
                case SourseTypeEnum.NBKR:
                    return new XmlResponseTransformerNbkr();
                default:
                    throw new Exception("wrong source type");
            }
        }
    }
}