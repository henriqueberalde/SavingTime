using SavingTime.Entities;

namespace SavingTime.Bussiness.Helpers
{
    public static class ConsoleWriterHelper
    {
        public static void ShowRecorList(IEnumerable<BaseTimeRecord> list)
        {
            foreach (var time in list)
            {
                ConsoleColor color;
                if (time.Type == TimeRecordType.Entry)
                    color = ConsoleColor.DarkGreen;
                else
                    color = ConsoleColor.DarkRed;

                Console.ForegroundColor = color;
                Console.Write("\u2588 ");
                Console.ResetColor();
                Console.WriteLine(time.ToString());
            }
        }
    }
}
