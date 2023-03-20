
using GitHubRestApi.Response;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;


namespace GitHubRestApi
{
    public static class GitHubRestApiHelper
    {
        static class RequestMethod
        {
            public const string GET = "GET";
        }    
        
        public static string? PAT { get; set; }

        public static List<Repository> ListRepositories(string username)
        {
            string requestUrl = $@"https://api.github.com/users/{username}/repos";
            return InvokeRestApiRequest<List<Repository>>(RequestMethod.GET, requestUrl);
        }

        static T InvokeRestApiRequest<T>(string requestMethod, string requestUrl, string requestBody = "")
        {
            HttpResponseMessage? requestResponse = null;
            string? responseContent = null;

            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",  PAT);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github+json"));
            httpClient.DefaultRequestHeaders.Add("X-GitHub-Api-Version", "2022-11-28");
            httpClient.DefaultRequestHeaders.Add("User-Agent", "Custom App");

            switch (requestMethod.ToUpper())
            {
                case RequestMethod.GET:
                    requestResponse = httpClient.GetAsync(requestUrl).Result;
                    break;
            }

            httpClient.Dispose();

            if (requestResponse != null)
            {
                if (requestResponse.IsSuccessStatusCode)
                    responseContent = requestResponse.Content.ReadAsStringAsync().Result;
                else
                {
                    if (requestResponse.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        throw new Exception("401: Unauthorized");
                    }

                    if (requestResponse.StatusCode == HttpStatusCode.NotFound)
                    {
                        throw new Exception("404: Not Found");
                    }

                    responseContent = requestResponse.Content.ReadAsStringAsync().Result;
                    throw new Exception(responseContent);
                }
            }
            if (requestResponse == null || responseContent == null)
                throw new Exception("Responce is null");

            return JsonConvert.DeserializeObject<T>(responseContent);
        }
    }
}
