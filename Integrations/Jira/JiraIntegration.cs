using System.Net.Http.Json;
using Integrations.Jira.Classes;

namespace Integrations.Jira
{
    public static class JiraIntegration
    {
        public static async Task PostWorklog(JiraWorklog worklog)
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri("https://lacuna.atlassian.net/") // TODO - PUT ON ENV
            };
            var token = "Y2FybG9zYkBsYWN1bmFzb2Z0d2FyZS5jb206QVRBVFQzeEZmR0YwSEEtZ3FEODBJMlZnTzJzZi1Vclp5M090LWtrcE1EbFRBQVg5YWlvbUtDa2tZREFCdHI0RHJ4dzM0YkNQYjBWOGdCNWV1ZXR0U2EzWng4V1lKYTc4LUEyZHpUS1RjYjZyNldicnBURkVha19UQVJ5QW4xUUxuNjJjNXlESGdZMWR1NzhGMldPTUg5a01Zd1QxWlB6NlYza3N1aXl3MFhqeFd1VDdHbWM1b1FFPUQ3QTcxRTU1";
            var path = $"rest/api/3/issue/{worklog.Issue}/worklog";
            var httpContent = JsonContent.Create(requestContent(worklog));
            var contentAsString = await httpContent.ReadAsStringAsync();
            var request = new HttpRequestMessage(HttpMethod.Post, path)
            {
                Content = httpContent
            };
            request.Headers.Add("Authorization", $"Basic {token}");

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