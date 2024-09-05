namespace SavingTime.Business
{
    public class WorkedSlice
    {
        public Time Begin { get; set; }
        public Time End { get; set; }
        public TimeSpan total;

        public TimeSpan Total
        {
            get
            {
                return total;
            }
        }

        public WorkedSlice(string begin, string end)
        {
            Begin = new Time(begin);
            End = new Time(end);
            var beginTimeSpan = new TimeSpan(Begin.Hour, Begin.Minute, 0);
            var endTimeSpan = new TimeSpan(End.Hour, End.Minute, 0);
            total = endTimeSpan.Subtract(beginTimeSpan);
        }

        public WorkedSlice(Time begin, Time end)
        {
            Begin = begin;
            End = end;
        }

        public List<string> ToFractions()
        {
            var result = new List<string>();
            var currentTime = Begin;

            while(End.IsGraterThan(currentTime))
            {
                result.Add(currentTime.ToFraction());
                currentTime.AddMinutes(10);
            }

            return result;
        }
    }
}

