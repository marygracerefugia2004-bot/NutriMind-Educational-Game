using System.Collections.Generic;

namespace NutriMind.Runtime.App.Http
{
    /// <summary>
    /// Request context passed to <see cref="IHttpRequestTransport"/>.
    /// Contains everything needed to issue one HTTP call except secret
    /// material that the transport already holds or receives separately.
    /// </summary>
    public class HttpTransportRequest
    {
        /// <summary>HTTP method, e.g. <c>GET</c>, <c>POST</c>, <c>PATCH</c>.</summary>
        public string Method { get; set; } = "GET";

        /// <summary>Full request URL.</summary>
        public string Url { get; set; } = string.Empty;

        /// <summary>JSON request body, or <c>null</c> for bodyless requests.</summary>
        public string? BodyJson { get; set; }

        /// <summary>Request headers to attach.  Must not contain secrets in logs.</summary>
        public Dictionary<string, string> Headers { get; set; } = new();

        /// <summary>Timeout in seconds.</summary>
        public int TimeoutSeconds { get; set; } = 30;
    }
}
