using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml.Schema;

namespace ExchangeRate.Xml
{
    public class XmlResponseTransformerNbkr : IResponseTransformer
    {
        public ExchangeRateResponse Transform(byte[] byteArray, ExchangeRateSource sourceUrl)
        {
            var encodingResponseContent = Encoding.UTF8.GetString(byteArray);
            var doc = XDocument.Parse(encodingResponseContent);

            try
            {
                var rates = from xe in doc.Root.Elements("Currency")
                    where xe.Attribute("ISOCode").Value == "USD" ||
                          xe.Attribute("ISOCode").Value == "EUR"
                    select xe;

                var exchangeRate = new ExchangeRateResponse
                {
                    Source = sourceUrl.Url,
                    ResponseStatus = ResponseStatus.OK
                };


                foreach (var node in rates)
                    switch (node.Attribute("ISOCode").Value)
                    {
                        case "USD":
                            exchangeRate.USDRate = node.Element("Value").Value;
                            break;
                        case "EUR":
                            exchangeRate.EURRate = node.Element("Value").Value;
                            break;
                    }

                return exchangeRate;
            }

            catch (XmlSchemaValidationException ex)
            {
                var result = new ExchangeRateResponse
                {
                    ResponseStatus = ResponseStatus.OtherException,
                    Source = sourceUrl.Url
                };
                return result;
            }
        }
    }
}