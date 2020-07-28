using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace LanguageFeatures.Models
{
    public class MyAsyncMethods
    {
        public async static Task<long?> GetPageLength()
        {
            var httpClient = new HttpClient();
            var httpMessage = await httpClient.GetAsync("http://apress.com");

            return httpMessage.Content.Headers.ContentLength;
        }

        public async static IAsyncEnumerable<long?> GetPageLengths(List<string> output, params string[] urls)
        {
            var httpClient = new HttpClient();
            
            foreach (var url in urls)
            {
                output.Add($"Started request for {url}");
                var httpMessage = await httpClient.GetAsync($"http://{url}");
                output.Add($"Completed request for {url}");
                
                yield return httpMessage.Content.Headers.ContentLength;
            }
        }
    }
}