namespace ExchangeRate
{
    public class ExchangeRateResponse
    {
        public string USDRate { get; set; }
        public string EURRate { get; set; }
        public string Source { get; set; }
        public string ExceptionMessage { get; set; }
        public ResponseStatus ResponseStatus { get; set; }

        public override string ToString()
        {
            switch (ResponseStatus)
            {
                case ResponseStatus.OK:
                    return string.Format("курс  USD= {0} EUR= {1}  иcточник= {2}", USDRate, EURRate, Source);
                case ResponseStatus.TaskCanceled:
                    return string.Format(" Задание отменено  иcточник= {0}", Source);
                case ResponseStatus.ClientTimeOut:
                    return string.Format(" Задание отменено из за таймаута иcточник= {0}", Source);
                default:
                    return string.Format(" ошибка = {0} иcточник= {1}", ExceptionMessage, Source);
            }
        }
    }
}