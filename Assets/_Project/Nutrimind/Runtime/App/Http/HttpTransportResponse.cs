using System.Collections.Generic;

namespace NutriMind.Runtime.App.Http
{
    /// <summary>
    /// Response returned by <see cref="IHttpRequestTransport"/>.
    /// Keeps transport concerns separate from provider-level error mapping.
    /// </summary>
    public class HttpTransportResponse
    {
        /// <summary>HTTP status code, or 0 for a network/transport failure.</summary>
        public long StatusCode { get; set; }

        /// <summary>Response body text, or empty.</summary>
        public string Body { get; set; } = string.Empty;

        /// <summary>True when the transport could not reach the server.</summary>
        public bool IsNetworkError { get; set; }

        /// <summary>True when an HTTP-level error status was returned.</summary>
        public bool IsHttpError => StatusCode >= 400;

        /// <summary>Transport-level error message (sanitized, no secrets).</summary>
        public string ErrorMessage { get; set; } = string.Empty;

        /// <summary>Response headers, when available.</summary>
        public Dictionary<string, string> ResponseHeaders { get; set; } = new();

        /// <summary>True when the response represents a transient failure that may be retried.</summary>
        public bool IsRetryableNetworkError => IsNetworkError || StatusCode == 0;

        /// <summary>True when the server returned a 5xx status.</summary>
        public bool IsServerError => StatusCode >= 500 && StatusCode <= 599;

        /// <summary>True when the server returned 408 or 429.</summary>
        public bool IsRateOrTimeoutError => StatusCode == 408 || StatusCode == 429;
    }
}
