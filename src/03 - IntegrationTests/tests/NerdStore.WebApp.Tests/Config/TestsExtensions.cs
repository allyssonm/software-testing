using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;

namespace NerdStore.WebApp.Tests.Config
{
    public static class TestsExtensions
    {
        public static decimal OnlyNumbers(this string value)
        {
            return Convert.ToDecimal(new string(value.Where(char.IsDigit).ToArray()));
        }

        public static void AddJsonMediaType(this HttpClient client)
        {
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public static void AddToken(this HttpClient client, string token)
        {
            client.AddJsonMediaType();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }
}
