using System;

namespace ExchangeRate.Xml
{
    public class XmlTransformerFactory : ITransformerFactory
    {

        public IResponseTransformer GetResponseTransformer(ExchangeRateSource exchangeRateSource)
        {
            switch (exchangeRateSource.SourceType)
            {
                case SourseType.CBR:
                    return new XmlResponseTransformerCbr();
                case SourseType.NBKR:
                    return new XmlResponseTransformerNbkr();
            }
            return null;
        }
    }
}