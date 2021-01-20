using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace PlattCodingChallenge.Services
{
    public interface IHttpClientFactory
    {
        HttpClient CreateClient();
    }
    public class HttpClientFactory : IHttpClientFactory
    {        
        public HttpClient CreateClient()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://swapi.dev/api/");
            return client;
        }
    }
}
