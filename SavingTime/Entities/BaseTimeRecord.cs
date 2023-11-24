namespace SavingTime.Entities
{
    public class BaseTimeRecord
    {
        public int Id { get; set; }
        public DateTime Time { get; set; }
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
