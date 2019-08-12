using System;
using System.IO;
using ExchangeRate;

namespace ExchangeRateApp
{
    public class Logger:ILogger
    {
        private readonly string logPath;

        public Logger(string logPath)
        {
            this.logPath = logPath;
            if (!Directory.Exists(logPath)) Directory.CreateDirectory(Path.GetDirectoryName(logPath));
        }

        public void AddLog(ExchangeRateResponse exchangeRateResponse)
        {
            using (var streamWriter = File.AppendText(logPath))
            {
                streamWriter.WriteLine("{0}  {1} ", DateTime.Now, exchangeRateResponse);
            }
        }
    }
}