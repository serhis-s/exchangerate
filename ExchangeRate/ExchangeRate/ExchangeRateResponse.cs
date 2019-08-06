namespace ExchangeRate
{
    public class ExchangeRateResponse
    {
        public string USDRate { get; set; }
        public string EURRate { get; set; }
        public string Source { get; set; }
        public string ResponseStatus { get; set; }

        public override string ToString()
        {
            if (ResponseStatus == "OK")
                return string.Format("курс  USD= {0} EUR= {1}  иcточник= {2}", USDRate, EURRate, Source);
            return string.Format(" {0}  иcточник= {1}", ResponseStatus, Source);
        }
    }
}