namespace Integrations.Ponto.Classes
{
    public static class PageWorkLog
    {
        private static readonly int INDEX_AUX = 4;
        private static readonly int ENTRY_AUX = 2;
        private static readonly int EXIT_AUX = 3;

        public static string H2ReferenceDate = "//*[@id=\"content\"]/div/div/div[2]/div/h2";
        private static string buttonSubmitSample = "//*[@id=\"content\"]/div/div/div[{{div_index}}]/div/button";

        public static string ButtonSubmit(int divIndex)
        {
            return buttonSubmitSample.Replace("{{div_index}}", divIndex.ToString());
        }

        public static string InputLogIn(int lineIndex = 0)
        {
            return Input(ENTRY_AUX, lineIndex);
        }

        public static string InputLogOut(int lineIndex = 0)
        {
            return Input(EXIT_AUX, lineIndex);
        }

        private static string Input(int aux, int lineIndex = 0)
        {
            var index = INDEX_AUX + lineIndex;

            return $"//*[@id=\"content\"]/div/div/div[{index}]/div[{aux}]/input";
        }
    }
}