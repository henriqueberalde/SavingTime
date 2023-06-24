namespace SavingTime
{
    public class TimeRecord
    {
        public int Id { get; set; }
        public DateTime Time { get; set; }
        public TimeRecordType Type { get; set; }

        public TimeRecord()
        {
        }

        public TimeRecord(DateTime time, TimeRecordType type)
        {
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
