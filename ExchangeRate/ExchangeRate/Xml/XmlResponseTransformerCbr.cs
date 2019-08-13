using System;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace ExchangeRate.Xml
{
    public class XmlResponseTransformerCbr : IResponseTransformer
    {
        /// <summary>
        ///     Thread Safe
        /// </summary>
        /// <param name="byteArray"></param>
        /// <param name="sourceUrl"></param>
        /// <returns></returns>
        public ExchangeRateResponse Transform(byte[] byteArray, ExchangeRateSource sourceUrl)
        {
            try
            {
                var encodingResponseContent = Encoding.UTF8.GetString(byteArray);
                var doc = XDocument.Parse(encodingResponseContent);
                var rates = from xe in doc.Root.Elements("Valute")
                    where xe.Element("CharCode").Value == "USD" ||
                          xe.Element("CharCode").Value == "EUR"
                    select xe;

                var exchangeRate = new ExchangeRateResponse
                {
                    Source = sourceUrl.Url,
                    ResponseStatus = ResponseStatus.OK
                };

                foreach (var node in rates)
                    switch (node.Element("CharCode").Value)
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

            catch (Exception ex)
            {
                var result = new ExchangeRateResponse
                {
                    ResponseStatus = ResponseStatus.OtherException,
                    ExceptionMessage = ex.Message,
                    Source = sourceUrl.Url
                };
                return result;
            }
        }
    }
}