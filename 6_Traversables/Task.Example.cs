using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Traversables.Task.Example
{

    public class Demo
    {
        public static async Task<object> GetUserDetails(string uri)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("User-Agent", "Test");
                var response = await httpClient.GetAsync(uri, default(CancellationToken));
                return (await response.Content.ReadAsStringAsync());
            }
        }

        public static async System.Threading.Tasks.Task Run()
        {

            var userDetailList= await new List<string> {
                "https://api.github.com/users/mojombo" ,
                "https://api.github.com/users/defunkt"
            }.Traverse(GetUserDetails);

        }

    }
}