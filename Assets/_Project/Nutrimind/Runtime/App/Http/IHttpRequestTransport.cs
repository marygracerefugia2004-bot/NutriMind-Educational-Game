using System;
using System.Threading;
using System.Threading.Tasks;

namespace NutriMind.Runtime.App.Http
{
    /// <summary>
    /// Transport abstraction for the HTTP provider.  Production uses
    /// <see cref="UnityWebRequestTransport"/>; tests can inject a fake
    /// transport to avoid real network calls.
    /// </summary>
    public interface IHttpRequestTransport : IDisposable
    {
        /// <summary>
        /// Sends one HTTP request and returns the transport response.
        /// Implementations must not mutate Unity objects from background threads.
        /// </summary>
        Task<HttpTransportResponse> SendAsync(HttpTransportRequest request, CancellationToken ct = default);
    }
}
