namespace Integrations.Ponto.Classes
{
    public class PontoIntegrationOptions
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ExpectedProfileName { get; set; }
        public DateTime RecordDateTime { get; set; }
        public string RecordType { get; set; }

        public PontoIntegrationOptions(string userName, string password, string expectedProfileName, DateTime recordDateTime, string recordType) {
            UserName = userName;
            Password = password;
            ExpectedProfileName = expectedProfileName;
            RecordDateTime = recordDateTime;
            RecordType = recordType;
        }
    }
}