using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;

namespace BankReconciliation.Class
{
    public class HttpServiceManager
    {
        private readonly string _serviceBaseUrl;

        public HttpServiceManager(string serviceBaseUrl)
        {
            _serviceBaseUrl = serviceBaseUrl;
        }

        private HttpClient GetDefaultHttpClient()
        {
            var client = new HttpClient { BaseAddress = new Uri(_serviceBaseUrl) };
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return client;
        }

        public HttpResponseMessage PostToRemoteService<T>(string serviceUrl, T type)
        {
            HttpResponseMessage response;
            using (var client = GetDefaultHttpClient())
            {
                response = client.PostAsJsonAsync(serviceUrl, type).Result;
                if (!response.IsSuccessStatusCode)
                {
                    response.EnsureSuccessStatusCode();
                }
            }
            return response;
        }

        public HttpResponseMessage PostToRemoteService<T>(string serviceUrl, List<T> type)
        {
            HttpResponseMessage response;
            using (var client = GetDefaultHttpClient())
            {
                response = client.PostAsJsonAsync(serviceUrl, type).Result;
                if (!response.IsSuccessStatusCode)
                {
                    response.EnsureSuccessStatusCode();
                }
            }
            return response;
        }

        public T GetRemoteService<T>(string serviceUrl)
        {
            T resp = default(T);
            //HttpResponseMessage response;
            using (var client = GetDefaultHttpClient())
            {
                //var serviceUrl = string.Format("Collections/ReceiptNo/{0}", receiptNo);
                // HTTP GET
                var response = client.GetAsync(serviceUrl).Result;
                if (response.IsSuccessStatusCode)
                {
                    resp = response.Content.ReadAsAsync<T>().Result;
                }
                else
                    response.EnsureSuccessStatusCode();
            }
            return resp;
        }
    }
}
