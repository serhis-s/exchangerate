﻿using System.Collections.Generic;
using System.Configuration;
using System.Xml.Linq;
using ExchangeRate;

namespace ExchangeRateApp
{
    public static class SourceLoader
    {
        public static List<ExchangeRateSource> GetSources()
        {
            var list = new List<ExchangeRateSource>();
            var appSettings = ConfigurationManager.AppSettings["ConfigPath"];
            var xDoc = XDocument.Load(appSettings);

            var root = xDoc.Element("Sources");
            foreach (var node in root.Elements())
            {
                var newSource = new ExchangeRateSource
                {
                    Url = node.Attribute("path").Value,
                    SourceType = node.Attribute("type").Value
                };

                list.Add(newSource);
            }

            return list;
        }
    }
}