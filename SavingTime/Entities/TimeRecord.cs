namespace SavingTime.Entities
{
    public class TimeRecord
    {
        public int Id { get; set; }
        public DateTime Time { get; set; }
        public TimeRecordType Type { get; set; }
        public string? Context { get; set; }

        public TimeRecord(
            DateTime time,
            TimeRecordType type,
            string? context
        )
        {
            Time = time;
            Type = type;
            Context = context;
        }

        public override string ToString()
        {
            var context = "";

            if (Context is not null)
            {
                context = $"[{Context}] ";
            }

            return $"{context}{Type} - {Time}";
        }
    }

    public enum TimeRecordType
    {
        Entry = 1,
        Exit = 2
    }
}
