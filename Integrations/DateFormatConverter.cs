using Newtonsoft.Json.Converters;

namespace SavingTime.Bussiness.Helpers
{
    public class DateFormatConverter : IsoDateTimeConverter
    {
        public DateFormatConverter(string format)
        {
            DateTimeFormat = format;
        }
    }
}
