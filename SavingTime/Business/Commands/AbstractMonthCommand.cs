using CommandLine;

namespace SavingTime.Business.Commands;

public abstract class AbstractMonthCommand: BaseCommand
{
    [Option('m', "month", Required = false, HelpText = "Month of records to show. Format MM or yyyy-MM")]
    public string? Month { get; set; }
    protected DateTime? DateRef
    {
        get
        {
            if (string.IsNullOrEmpty(Month))
            {
                return null;
            }

            if (int.TryParse(Month, out var month))
            {
                return new DateTime(DateTime.Now.Year, month, 1);
            }

            var dateRefString = Month + "-01";

            return DateTime.ParseExact(
                dateRefString,
                "yyyy-MM-dd",
                System.Globalization.CultureInfo.InvariantCulture
            );
        }
    }
}
