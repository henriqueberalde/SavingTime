namespace SavingTime.Entities
{
    public class TimeRecord : BaseTimeRecord
    {
        public TimeRecord(
            DateTime time,
            TimeRecordType type
        ) : base(time, type)
        { }

        public override string ToString()
        {
            string typeInfo = Type == TimeRecordType.Exit ? $"{Type} " : Type.ToString();
            var context = "";

            return $"{context}{typeInfo} - {Id} {Time}";
        }
    }
}
