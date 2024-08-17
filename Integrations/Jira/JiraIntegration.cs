using System.Net.Http.Json;
using Integrations.Jira.Classes;

namespace Integrations.Jira
{
    public class JiraIntegration
    {
        private readonly string Token;

        public JiraIntegration(string token)
        {
            Token = token;
        }

        public async Task PostWorklog(JiraWorklog worklog)
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri("https://lacuna.atlassian.net/") // TODO - PUT ON ENV
            };

            var path = $"rest/api/3/issue/{worklog.Issue}/worklog";
            var httpContent = JsonContent.Create(requestContent(worklog));
            var request = new HttpRequestMessage(HttpMethod.Post, path)
            {
                Content = httpContent
            };
            request.Headers.Add("Authorization", $"Basic {Token}");

            var response = await client.SendAsync(request);

            if (response is null)
            {
                throw new Exception($"No response what created from POST {request.RequestUri}");
            }

            if (!response.IsSuccessStatusCode)
            {
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