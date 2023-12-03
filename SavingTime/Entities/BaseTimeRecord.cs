namespace SavingTime.Entities
{
    public class BaseTimeRecord
    {
        public int Id { get; set; }
        private DateTime _time;
        public DateTime Time {
            get {
                return _time;
            }
            set {
                if (value.Second >= 30) {
                    value = value.AddMinutes(1);
                    value = new DateTime(
                        value.Year,
                        value.Month,
                        value.Day,
                        value.Hour,
                        value.Minute,
                        0
                    );
                }
                _time = value;
            }
        }
        public TimeRecordType Type { get; set; }

        public BaseTimeRecord(DateTime time, TimeRecordType type) {
            Time = time;
            Type = type;
        }
    }

    public enum TimeRecordType
    {
        Entry = 1,
        Exit = 2
    }
}
