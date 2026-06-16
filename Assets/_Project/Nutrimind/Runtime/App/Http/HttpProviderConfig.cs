using System;

namespace NutriMind.Runtime.App.Http
{
    /// <summary>
    /// Public runtime-safe configuration for the HTTP game-data provider.
    /// Values can be set from code, a ScriptableObject wrapper, or build
    /// configuration.  No secrets (tokens, PINs) are stored here.
    /// </summary>
    [Serializable]
    public class HttpProviderConfig
    {
        /// <summary>
        /// Server base URL, e.g. <c>https://play.nutrimind.example</c>.
        /// Must not be null or whitespace; an invalid base URL causes
        /// every request to fail with a structured <see cref="DataProviderError"/>.
        /// Only <c>https://</c> is accepted for production LRN/PIN/bearer traffic.
        /// </summary>
        public string BaseUrl { get; set; } = string.Empty;

        /// <summary>
        /// API prefix appended to <see cref="BaseUrl"/>.  Defaults to
        /// <c>/api/v1</c> and must start with <c>/</c>.
        /// </summary>
        public string ApiPrefix { get; set; } = "/api/v1";

        /// <summary>
        /// Per-request timeout in seconds.  Defaults to 30.
        /// </summary>
        public int TimeoutSeconds { get; set; } = 30;

        /// <summary>
        /// Maximum retry attempts for transient safe/idempotent failures.
        /// Does not include the original request.  Defaults to 3.
        /// </summary>
        public int MaxRetries { get; set; } = 3;

        /// <summary>
        /// Base delay in milliseconds between retries when the server does not
        /// provide <c>retry_after_seconds</c>.  Defaults to 1000ms.
        /// </summary>
        public int DefaultRetryDelayMs { get; set; } = 1000;

        /// <summary>
        /// Upper bound in seconds for server-provided <c>retry_after_seconds</c>.
        /// Values above this are clamped.  Defaults to 300 (5 minutes).
        /// </summary>
        public int MaxRetryAfterSeconds { get; set; } = 300;

        /// <summary>
        /// Default polling interval in seconds when the server does not supply
        /// <c>next_poll_after_seconds</c>.  Defaults to 45.
        /// </summary>
        public int DefaultPollIntervalSeconds { get; set; } = 45;

        /// <summary>
        /// Validates that the configuration can be used to build URLs.
        /// Returns a structured error when invalid; otherwise <c>null</c>.
        /// </summary>
        public DataProviderError? Validate()
        {
            if (string.IsNullOrWhiteSpace(BaseUrl))
            {
                return new DataProviderError(
                    "CONFIGURATION_ERROR",
                    "Server base URL is not configured. Please check your connection settings.")
                {
                    Action = "show_offline_prompt"
                };
            }

            if (!Uri.TryCreate(BaseUrl, UriKind.Absolute, out var uri))
            {
                return new DataProviderError(
                    "CONFIGURATION_ERROR",
                    "Server base URL is not valid. Please check your connection settings.")
                {
                    Action = "show_offline_prompt"
                };
            }

            // Phase 04 requirement: HTTPS-only for LRN/PIN/bearer traffic.
            // Plaintext http:// is rejected as a configuration error.
            if (uri.Scheme != Uri.UriSchemeHttps)
            {
                return new DataProviderError(
                    "CONFIGURATION_ERROR",
                    "Server base URL must use HTTPS for security. Please check your connection settings.")
                {
                    Action = "show_offline_prompt"
                };
            }

            if (string.IsNullOrWhiteSpace(ApiPrefix) || !ApiPrefix.StartsWith("/"))
            {
                return new DataProviderError(
                    "CONFIGURATION_ERROR",
                    "API prefix must start with '/'. Please check your connection settings.")
                {
                    Action = "show_offline_prompt"
                };
            }

            return null;
        }

        /// <summary>
        /// Builds the full API URL for the given relative endpoint path.
        /// </summary>
        public string BuildUrl(string relativePath)
        {
            string baseUrl = BaseUrl.TrimEnd('/', ' ');
            string prefix = ApiPrefix.TrimEnd('/');
            string path = relativePath.StartsWith("/") ? relativePath : "/" + relativePath;
            return baseUrl + prefix + path;
        }
    }
}
