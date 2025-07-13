using SavingTime.Migrations;

namespace SavingTime.Entities
{
    public class IssueRecord : BaseTimeRecord
    {
        public string Issue { get; set; }
        public string? Comment { get; set; }
        public bool Sent { get; set; } = false;
        public IssueRecord(
            DateTime time,
            TimeRecordType type,
            string issue,
            string? comment = null
         ) : base(time, type) {
            Issue = issue;
            Comment = comment;
        }
        public override string ToString()
        {
            string typeInfo = Type == TimeRecordType.Exit ? $"{Type} " : Type.ToString();
            var issue = Issue ?? "";

            return $"{typeInfo} - {Id} {Time} {issue} {Comment}";
        }
    }
}
