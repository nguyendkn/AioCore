using System.Net;
using Flurl.Http;
using Polly;
using Polly.Retry;

namespace AioCore.Notion.Lib
{
    public class HttpCookieSession
    {
        private readonly FlurlClient _flurlClient;
        
        public CookieJar Cookies { get; } = new();

        public HttpCookieSession(Action<FlurlClient> configure)
        {
            _flurlClient = new FlurlClient(new HttpClient(new PolicyHandler()));
            configure(_flurlClient);
        }

        public IFlurlRequest CreateRequest(Uri uri)
            => new FlurlRequest(uri) { Client = _flurlClient }.WithCookies(Cookies);

        public IFlurlRequest CreateRequest(string uri)
            => new FlurlRequest(uri) { Client = _flurlClient }.WithCookies(Cookies);

        private class PolicyHandler : DelegatingHandler
        {
            private readonly AsyncRetryPolicy<HttpResponseMessage> _retryPolicy;

            public PolicyHandler()
            {
                InnerHandler = new HttpClientHandler();

                //retry on 502
                _retryPolicy = Policy
                    .HandleResult<HttpResponseMessage>(r => r.StatusCode == HttpStatusCode.BadGateway)
                    .WaitAndRetryAsync(5, retry => TimeSpan.FromSeconds(0.3));
            }

            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
                => _retryPolicy.ExecuteAsync(ct => base.SendAsync(request, ct), cancellationToken);
        }

        public HttpCookieSession WithCookies(string originUrl, Dictionary<string, string> dictionary)
        {
            foreach (var kp in dictionary)
                Cookies.AddOrReplace(kp.Key, kp.Value, originUrl);

            return this;
        }
    }
}