using System.Net;
using System;
using System.Reflection.Metadata;

namespace GitHubRestApi
{
    internal class Program
    {
        static void Main(string[] args)
        {
            GitHubRestApiHelper.PAT = "<pat>"; //https://docs.github.com/en/authentication/keeping-your-account-and-data-secure/creating-a-personal-access-token
            string UserName = "<username>";

            var repos = GitHubRestApiHelper.ListRepositories(UserName);

            foreach (var repo in repos)
            {
                Console.WriteLine($@"{repo.name} : {repo.url}");
                Console.WriteLine($@"{repo.description}");
            }
        }        
    }

 }