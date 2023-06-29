using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Integrations
{
    public class LacunaConfiguration
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ExpectedProfileName { get; set; }
        public DateTime RecordDateTime { get; set; }
        public string RecordType { get; set; }

        public LacunaConfiguration(
            string username,
            string password,
            string expectedProfileName,
            DateTime recordDateTime,
            string type)
        {
            UserName = username;
            Password = password;
            ExpectedProfileName = expectedProfileName;
            RecordDateTime = recordDateTime;
            RecordType = type;
        }
    }

    public class LacunaIntegration
    {
        private ChromeDriver driver { get; set; }
        private LacunaConfiguration Configuration { get; set; }

        public LacunaIntegration(LacunaConfiguration config)
        {
            driver = new ChromeDriver();
            Configuration = config;
        }

        public void Login()
        {
            var url = "https://ponto.lacunasoftware.com/#/home";
            
            driver.Navigate().GoToUrl(url);

            Thread.Sleep(2000);

            driver.FindElement(By.XPath(PageLogin.InputUserName)).SendKeys(Configuration.UserName);
            driver.FindElement(By.XPath(PageLogin.InputPassword)).SendKeys(Configuration.Password);
            driver.FindElement(By.XPath(PageLogin.ButtonLogin)).Click();
            Thread.Sleep(2000);

            var pageProfileName = driver.FindElement(By.XPath(PageLanding.LinkProfile)).Text;
            if (pageProfileName != Configuration.ExpectedProfileName)
            {
                throw new Exception($"User login failed. Logged Profile: {pageProfileName} is not {Configuration.ExpectedProfileName}");
            }
        }

        public void LogWork(int line, string time, string timePlus10Minutes)
        {
            var url = "https://ponto.lacunasoftware.com/#/workLog";

            driver.Navigate().GoToUrl(url);
            Thread.Sleep(2000);

            var pageDateTime = driver.FindElement(By.XPath(PageWorkLog.H2ReferenceDate)).Text;
            var recordDateTime = Configuration.RecordDateTime;
            
            if (pageDateTime.Substring(0, 2) != recordDateTime.ToString("dd"))
            {
                throw new Exception($"Reference DateTime did not match. From Page: {pageDateTime}; From Record {recordDateTime}");
            }

            var inputLogIn = driver.FindElement(By.XPath(PageWorkLog.InputLogIn(line)));
            var inputLogOut = driver.FindElement(By.XPath(PageWorkLog.InputLogOut(line)));
            var element = Configuration.RecordType == "Entry" ? inputLogIn : inputLogOut;

            if (Configuration.RecordType == "Entry")
            {
                inputLogIn.Clear();
                inputLogIn.SendKeys(time);
                inputLogOut.Clear();
                inputLogOut.SendKeys(timePlus10Minutes);
            }
            else {
                inputLogOut.Clear();
                inputLogOut.SendKeys(time);
            }

            driver.FindElement(By.XPath(PageWorkLog.ButtonSubmit)).Click();
            Thread.Sleep(2000);

            driver.Quit();
        }
    }

    public static class PageLogin
    {
        public static string InputUserName = "//*[@id=\"myDiv\"]/input";
        public static string InputPassword = "//*[@id=\"content\"]/div/div/div[2]/div[1]/form/div[2]/div/input";
        public static string ButtonLogin = "//*[@id=\"content\"]/div/div/div[2]/div[1]/form/div[3]/button";
    }
    public static class PageLanding
    {
        public static string LinkProfile = "/html/body/div[1]/div/div[2]/ul[2]/li[1]/a";
        public static string ButtonLogNow = "//*[@id=\"content\"]/div/div/div[1]/div[2]/a";
    }
    public static class PageWorkLog
    {
        private static readonly int INDEX_AUX = 4;
        private static readonly int ENTRY_AUX = 2;
        private static readonly int EXIT_AUX = 3;

        public static string H2ReferenceDate = "//*[@id=\"content\"]/div/div/div[2]/div/h2";
        public static string ButtonSubmit = "//*[@id=\"content\"]/div/div/div[8]/div/button";

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