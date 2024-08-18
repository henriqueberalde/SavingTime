using SavingTime.Migrations;

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
        public override string ToString()
        {
            string typeInfo = Type == TimeRecordType.Exit ? $"{Type} " : Type.ToString();
            var context = Issue ?? "";

            return $"{typeInfo} - {Id} {Time} {context}";
        }
    }
}
