using Microsoft.AspNetCore.Http;

namespace SportsStore.Infrastructure
{
    public static class UrlExtensions
    {
        public static string PathAndQuery(this HttpRequest httpRequest)
        {
            return httpRequest.QueryString.HasValue
                ? $"{httpRequest.Path}{httpRequest.QueryString}"
                : httpRequest.Path.ToString();
        }
    }
}