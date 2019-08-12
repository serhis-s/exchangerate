using System;

namespace ExchangeRate.Xml
{
    public class XmlTransformerFactory : ITransformerFactory
    {

        public IResponseTransformer GetResponseTransformer(ExchangeRateSource exchangeRateSource)
        {
            switch (exchangeRateSource.SourceType)
            {
                case "CBR":
                    return new XmlResponseTransformerCbr();
                case "NBKR":
                    return new XmlResponseTransformerNbkr();
            }
            return null;
        }
    }
}