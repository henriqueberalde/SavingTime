namespace SavingTime.Entities
{
    public class IssueRecord : BaseTimeRecord
    {
        public string Issue { get; set; }
        public IssueRecord(
            DateTime time,
            TimeRecordType type,
            string issue
         ) : base(time, type) {
            Issue = issue;
        }
    }
}
