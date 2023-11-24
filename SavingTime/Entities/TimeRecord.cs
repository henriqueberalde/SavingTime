namespace SavingTime.Entities
{
    public class TimeRecord : BaseTimeRecord
    {
        public string? Context { get; set; }

        public TimeRecord(
            DateTime time,
            TimeRecordType type,
            string? context
        ) : base(time, type)
        {
            Context = context;
        }

        public override string ToString()
        {
            string typeInfo = Type == TimeRecordType.Exit ? $"{Type} " : Type.ToString();
            var context = "";

            if (Context is not null)
            {
                context = $"[{Context}] ";
            }

            return $"{context}{typeInfo} - {Time}";
        }
    }
}
