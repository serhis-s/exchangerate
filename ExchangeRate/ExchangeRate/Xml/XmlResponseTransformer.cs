using System;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml.Schema;

namespace ExchangeRate.Xml
{
    public class XmlResponseTransformer : IResponseTransformer
    {
        public ExchangeRateResponse Transform(byte[] byteArray, string sourceUrl)
        {
            var encodingResponseContent = Encoding.UTF8.GetString(byteArray);
            var doc = XDocument.Parse(encodingResponseContent);
            var schemaSet = new XmlSchemaSet();
            schemaSet.Add(null, "XMLSchema.xsd");
            try
            {
                doc.Validate(schemaSet, null);

                var rates = from xe in doc.Root.Elements("Valute")
                    where xe.Element("CharCode").Value == "USD" ||
                          xe.Element("CharCode").Value == "EUR"
                    select xe;

                var exchangeRate = new ExchangeRateResponse
                {
                    Source = sourceUrl,
                    ResponseStatus = "OK"
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

            catch (XmlSchemaValidationException ex)
            {
                Console.WriteLine("Произошло исключение:   {0}", ex.Message);
                var result = new ExchangeRateResponse
                {
                    ResponseStatus = ex.Message,
                    Source = sourceUrl
                };
                return result;
            }
        }
    }
}