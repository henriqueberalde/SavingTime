using System.Net.Http.Json;

namespace Integrations
{
    public class JiraWorklog {
        public string Issue { get; set; }
        public string Message { get; set; }
        public DateTime DateTime { get; set; }

        public string DateTimeFormated
        {
            get { return DateTime.ToString("yyyy-MM-dd'T'HH:mm:ss.fff-0300"); }
        }
        public double TimeSpentInSeconds { get; set; }
    }

    public static class JiraIntegration
    {
        public static async Task PostWorklog(JiraWorklog worklog)
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri("https://lacuna.atlassian.net/") // TODO - PUT ON ENV
            };
            var token = "<token>";
            var path = $"rest/api/3/issue/{worklog.Issue}/worklog";
            var httpContent = JsonContent.Create(requestContent(worklog));
            var contentAsString = await httpContent.ReadAsStringAsync();
            var request = new HttpRequestMessage(HttpMethod.Post, path)
            {
                Content = httpContent
            };
            request.Headers.Add("Authorization", $"Basic {token}");

            var response = await client.SendAsync(request);

            if (response is null) {
                throw new Exception($"No response what created from POST {request.RequestUri}");
            }

            if (!response.IsSuccessStatusCode) {
                throw new Exception($"Error status code got from POST {request.RequestUri}. StatusCode: ${response.StatusCode}");
            }
        }

        private static dynamic requestContent(JiraWorklog worklog)
        {
            return new
            {
                Comment = new
                {
                    Content = new dynamic[] {
                        new {
                            Content = new dynamic[] {
                                new {
                                    Text = worklog.Message,
                                    Type = "text"
                                }
                            },
                            Type = "paragraph"
                        }
                    },
                    Type = "doc",
                    Version = 1
                },
                Started = worklog.DateTimeFormated,
                TimeSpentSeconds = worklog.TimeSpentInSeconds
            };
        }
    }
}