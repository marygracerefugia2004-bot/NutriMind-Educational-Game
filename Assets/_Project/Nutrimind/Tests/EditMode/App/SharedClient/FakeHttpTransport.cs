using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NutriMind.Runtime.App.Http;

namespace NutriMind.Tests.EditMode.App
{
    /// <summary>
    /// Test double for <see cref="IHttpRequestTransport"/> that returns
    /// pre-configured responses without making real network calls.
    /// Records every request so tests can assert headers, methods, and URLs.
    /// </summary>
    public class FakeHttpTransport : IHttpRequestTransport
    {
        private readonly Queue<HttpTransportResponse> _responses = new();
        private readonly List<HttpTransportRequest> _requests = new();

        /// <summary>All requests received by this transport, in order.</summary>
        public IReadOnlyList<HttpTransportRequest> Requests => _requests;

        /// <summary>Number of requests received.</summary>
        public int RequestCount => _requests.Count;

        /// <summary>Enqueue a response to return for the next request.</summary>
        public void Enqueue(HttpTransportResponse response)
            => _responses.Enqueue(response);

        /// <summary>Enqueue a successful JSON response.</summary>
        public void EnqueueSuccess(long statusCode, string body)
            => Enqueue(new HttpTransportResponse { StatusCode = statusCode, Body = body });

        /// <summary>Enqueue a server error response with the given body.</summary>
        public void EnqueueError(long statusCode, string body)
            => Enqueue(new HttpTransportResponse { StatusCode = statusCode, Body = body, IsNetworkError = false });

        /// <summary>Enqueue a network-level failure.</summary>
        public void EnqueueNetworkError(string message = "Network error")
            => Enqueue(new HttpTransportResponse { StatusCode = 0, IsNetworkError = true, ErrorMessage = message });

        /// <summary>
        /// Returns the next enqueued response and records the request.
        /// </summary>
        public Task<HttpTransportResponse> SendAsync(HttpTransportRequest request, CancellationToken ct = default)
        {
            _requests.Add(request);

            if (_responses.Count == 0)
            {
                return Task.FromResult(new HttpTransportResponse
                {
                    StatusCode = 0,
                    IsNetworkError = true,
                    ErrorMessage = "No response configured for fake transport."
                });
            }

            return Task.FromResult(_responses.Dequeue());
        }

        /// <inheritdoc />
        public void Dispose()
        {
            _responses.Clear();
            _requests.Clear();
        }
    }
}
