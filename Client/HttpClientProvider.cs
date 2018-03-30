using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace CogisoftConnector.Logic
{
    public static class HttpClientProvider
    {
        // Recommended way of usage for HttpClient class, as per:
        // https://docs.microsoft.com/en-us/azure/architecture/antipatterns/improper-instantiation/


        //Singleton pattern implementation, as per:
        // https://msdn.microsoft.com/en-us/library/ff650316.aspx
        private static volatile HttpClient _httpClient;
        private static object syncRoot = new Object();

        public static HttpClient HttpClient
        {
            get
            {
                if (_httpClient == null)
                {
                    lock (syncRoot)
                    {
                        if (_httpClient == null)
                        {
                            _httpClient = new HttpClient();
                        }
                    }
                }
                return _httpClient;
            }
        }
    }
}
