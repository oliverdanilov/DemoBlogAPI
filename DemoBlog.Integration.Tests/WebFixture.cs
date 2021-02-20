using DemoBlogAPI;
using Flurl.Http;
using Flurl.Http.Configuration;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DemoBlog.Integration.Tests
{
    public class WebFixture : IAsyncLifetime, IDisposable
    {
        public HttpClient HttpClient { get; private set; }
        public string BaseUrl => _baseUrl;

        const string _baseUrl = "http://localhost:5000";
        private IWebHost _webServer;

        public WebFixture()
        {
            //Reference: https://devblogs.microsoft.com/nuget/deprecating-tls-1-0-and-1-1-on-nuget-org/
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;
        }
        private bool _isInitializedAsyncAlready = false;
        public async Task InitializeAsync()
        {
            // Temporary fix to not double call this method. Permanent solution: remove all references.
            if (_isInitializedAsyncAlready)
            {
                return;
            }
            _isInitializedAsyncAlready = true;


            // Start the Web Server
            _webServer = new WebHostBuilder()
                .UseEnvironment("Testing")
                .UseKestrel()
                .UseUrls(_baseUrl)
                .UseStartup<Startup>()
                .Build();
            await _webServer.StartAsync();

            // Set variables for convinient use.
            HttpClient = new HttpClient() { BaseAddress = new Uri(_baseUrl) }; 
        }
        public void Dispose()
        {
            _webServer?.Dispose();
            HttpClient?.Dispose();
        }

        public Task DisposeAsync()
        {
            Dispose();
            return Task.FromResult(0);
        }
    }
}
