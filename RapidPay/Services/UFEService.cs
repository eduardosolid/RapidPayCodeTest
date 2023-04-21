
namespace RapidPay.Services
{
    public interface IUFEService
    {
        public Task<decimal> GetLastFee();

    }
    public class UFEService : IUFEService
    {
        private readonly object ufeLock = new object();
        private decimal? _lastFee;
        private int _lastHourUpdated = 0;
        private int _lastDayUpdated = 0;
        public UFEService() { }

        public async Task<decimal> GetLastFee()
        {
            DateTime now = DateTime.Now;
            lock (ufeLock)
            {
                if (_lastFee == null)
                {
                    _lastFee = CalculateNewFee(_lastFee);
                    _lastDayUpdated = now.Day;
                    _lastHourUpdated = now.Hour;
                }
                else
                {
                    if (_lastHourUpdated != now.Hour || _lastDayUpdated != now.Day )
                    {
                        _lastFee = CalculateNewFee(_lastFee);
                        _lastDayUpdated = now.Day;
                        _lastHourUpdated = now.Hour;
                    }
                }
            }

            return (decimal)_lastFee;
        }

        private decimal CalculateNewFee(decimal? lastFee)
        {
            Random random = new Random();
            int randomInteger = random.Next(1, 200); // between 1 and 200, starting from 1 and not from 0 as the .Next() method can return the minValue
            decimal randomDecimal = ((decimal)randomInteger / (decimal)100) / (decimal)100;
            if (lastFee == null)
            {
                lastFee = randomDecimal;
            }
            else
            {
                lastFee = lastFee * randomDecimal;
            }

            return (decimal)lastFee;
        }
    }
}
