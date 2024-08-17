using Integrations.Ponto.Classes;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Integrations.Ponto
{
    public class PontoIntegration
    {
        private ChromeDriver Driver { get; set; }
        private PontoIntegrationOptions Options { get; set; }

        public PontoIntegration(PontoIntegrationOptions options)
        {
            Driver = new ChromeDriver();
            Options = options;
        }

        public void Test()
        {
            var url = "https://ponto.lacunasoftware.com/#/home";

            Driver.Navigate().GoToUrl(url);
            Thread.Sleep(2000);
            Driver.Quit();
        }

        public void Login()
        {
            var url = "https://ponto.lacunasoftware.com/#/home";

            Driver.Navigate().GoToUrl(url);

            Thread.Sleep(2000);

            Driver.FindElement(By.XPath(PageLogin.InputUserName)).SendKeys(Options.UserName);
            Driver.FindElement(By.XPath(PageLogin.InputPassword)).SendKeys(Options.Password);
            Driver.FindElement(By.XPath(PageLogin.ButtonLogin)).Click();
            Thread.Sleep(2000);

            var pageProfileName = Driver.FindElement(By.XPath(PageLanding.LinkProfile)).Text;
            if (pageProfileName != Options.ExpectedProfileName)
            {
                throw new Exception($"User login failed. Logged Profile: {pageProfileName} is not {Options.ExpectedProfileName}");
            }
        }

        public void LogWork(int line, string time, string timePlus10Minutes)
        {
            var url = "https://ponto.lacunasoftware.com/#/workLog";

            Driver.Navigate().GoToUrl(url);
            Thread.Sleep(2000);

            var pageDateTime = Driver.FindElement(By.XPath(PageWorkLog.H2ReferenceDate)).Text;
            var recordDateTime = Options.RecordDateTime;

            if (pageDateTime.Substring(0, 2) != recordDateTime.ToString("dd"))
            {
                throw new Exception($"Reference DateTime did not match. From Page: {pageDateTime}; From Record {recordDateTime}");
            }

            var inputLogIn = Driver.FindElement(By.XPath(PageWorkLog.InputLogIn(line)));
            var inputLogOut = Driver.FindElement(By.XPath(PageWorkLog.InputLogOut(line)));
            var element = Options.RecordType == "Entry" ? inputLogIn : inputLogOut;

            if (Options.RecordType == "Entry")
            {
                inputLogIn.Clear();
                inputLogIn.SendKeys(time);
                inputLogOut.Clear();
                inputLogOut.SendKeys(timePlus10Minutes);
            }
            else
            {
                inputLogOut.Clear();
                inputLogOut.SendKeys(time);
            }

            var divIndex = Driver.FindElements(By.XPath("/html/body/div[2]/div/div/div")).Count;

            Driver.FindElement(By.XPath(PageWorkLog.ButtonSubmit(divIndex))).Click();
            Thread.Sleep(2000);

            Driver.Quit();
        }
    }
}