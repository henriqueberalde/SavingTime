using SavingTime.Entities;
using System.Text;

namespace SavingTime.Bussiness
{
    public class Summary
    {
        private readonly int _minHour;
        private readonly int _maxHour;

        public List<SummaryItem> Items { get; set; }

        public Summary(List<SummaryItem> items, int minHour, int maxHour)
        {
            Items = items;

            int? min = null;
            int? max = null;

            foreach (var item in Items)
            {
                if (min is null || min > item.MinHour) { min = item.MinHour; }
                if (max is null || max < item.MaxHour) { max = item.MaxHour; }
            }

            if (min is null || max is null) { throw new Exception("Min or max null error"); }
            _minHour = min.Value;
            _maxHour = max.Value;
        }

        public static Summary FromTimeRecordList(List<TimeRecord> list)
        {
            var items = new List<SummaryItem>();
            var grouping = list.GroupBy(x => x.Time.ToString("dd/MM/yyyy"));

            foreach (var g in grouping)
            {
                items.Add(SummaryItem.FromTimeRecordList(g.ToList()));
            }

            return new Summary(items, 9, 18);
        }

        public override string ToString()
        {
            var result = new StringBuilder();

            result.Append(ToStringHeader(_minHour, _maxHour));
            result.Append("\n");

            var list = Items;

            foreach (var item in list)
            {
                result.Append(item.ToString(_minHour, _maxHour));
                result.Append("\n");
            }

            return result.ToString();
        }

        private string ToStringHeader(int min, int max)
        {
            var timeList = new StringBuilder();
            var currentHour = _minHour;

            while (currentHour <= _maxHour)
            {
                timeList.Append($"[ {currentHour.ToString("D2")} ]");
                currentHour += 1;
            }

            return $"DATE  |{timeList}| TOTAL         | DIFF";
        }
    }
}
