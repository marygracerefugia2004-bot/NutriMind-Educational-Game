using System.Collections.Generic;
using Newtonsoft.Json;

namespace NutriMind.Runtime.App
{
    /// <summary>
    /// Structured error returned from a data-provider operation.
    /// Maps directly from the server error envelope shape documented
    /// in <c>docs/unity/04_SERVER_CONNECTION_AND_UNITY_API.md</c>.
    /// <para>
    /// All fields except <see cref="Code"/> and <see cref="Message"/> are optional
    /// and may be absent or null when the provider does not supply them.
    /// Unknown <see cref="Code"/> values and unknown <see cref="Action"/>
    /// values must be handled gracefully (fallback display, generic retry, etc.).
    /// </para>
    /// <para>
    /// This type must never expose raw stack traces, SQL, tokens, PINs,
    /// answer keys, scoring rules, or private audit data.
    /// </para>
    /// </summary>
    public class DataProviderError
    {
        /// <summary>
        /// Stable error code for programmatic handling
        /// (e.g. <c>"STATION_LOCKED"</c>, <c>"UNAUTHENTICATED"</c>,
        /// <c>"not_implemented"</c>).
        /// Must never be null.
        /// </summary>
        [JsonProperty("code")]
        public string Code { get; set; } = "unknown";

        /// <summary>
        /// Human-readable error description safe for student display.
        /// Must never be null.
        /// </summary>
        [JsonProperty("message")]
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Server request ID present on all errors. Can be shown
        /// in support details but must not expose internals to students.
        /// Optional; null when not supplied.
        /// </summary>
        [JsonProperty("request_id", NullValueHandling = NullValueHandling.Ignore)]
        public string? RequestId { get; set; }

        /// <summary>
        /// Whether the request can be retried with the same parameters.
        /// Optional; null when not supplied.
        /// </summary>
        [JsonProperty("retryable", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Retryable { get; set; }

        /// <summary>
        /// Arbitrary structured details. Keys are provider-specific.
        /// Optional; null or empty when not supplied.
        /// Student-safe: must never contain answer keys, scoring, secrets,
        /// or private data.
        /// </summary>
        [JsonProperty("details", NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, object>? Details { get; set; }

        /// <summary>
        /// Field-level validation errors. Keys are field names; values are
        /// error messages or message arrays.
        /// Optional; null or empty when not supplied.
        /// </summary>
        [JsonProperty("field_errors", NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, object>? FieldErrors { get; set; }

        /// <summary>
        /// Suggested retry delay in seconds when the server specifies one.
        /// Optional; null when not supplied.
        /// </summary>
        [JsonProperty("retry_after_seconds", NullValueHandling = NullValueHandling.Ignore)]
        public int? RetryAfterSeconds { get; set; }

        /// <summary>
        /// Suggested client action from the server's known action set.
        /// Maps to <see cref="ErrorAction"/> enum; unknown values
        /// fall back to <see cref="ErrorAction.Unknown"/>.
        /// Optional; null when not supplied.
        /// </summary>
        [JsonProperty("action", NullValueHandling = NullValueHandling.Ignore)]
        public string? Action { get; set; }

        // ──────────────────────────────────────────────────────────────
        //  Backward-compatible constructors
        // ──────────────────────────────────────────────────────────────

        /// <summary>
        /// Creates a new error instance with a stable code and message.
        /// All other fields default to null/empty.
        /// </summary>
        public DataProviderError(string code, string message)
        {
            Code = code;
            Message = message;
        }

        /// <summary>
        /// Parameterless constructor for JSON deserialization.
        /// Code defaults to <c>"unknown"</c> and Message defaults to empty string.
        /// </summary>
        public DataProviderError() { }

        /// <summary>
        /// Resolves the <see cref="Action"/> string to a typed
        /// <see cref="ErrorAction"/> enum value, falling back to
        /// <see cref="ErrorAction.Unknown"/> for unrecognized actions.
        /// </summary>
        public ErrorAction ResolvedAction =>
            Action switch
            {
                "login_again" => ErrorAction.LoginAgain,
                "retry" => ErrorAction.Retry,
                "wait_then_retry" => ErrorAction.WaitThenRetry,
                "refresh_sync_status" => ErrorAction.RefreshSyncStatus,
                "return_to_menu" => ErrorAction.ReturnToMenu,
                "show_offline_prompt" => ErrorAction.ShowOfflinePrompt,
                "contact_teacher" => ErrorAction.ContactTeacher,
                _ => ErrorAction.Unknown
            };

        /// <summary>
        /// Convenience: returns true when <see cref="Retryable"/>
        /// is explicitly true. Returns false when null or false.
        /// </summary>
        public bool IsRetryable => Retryable == true;
    }

    /// <summary>
    /// Known error actions that the server may suggest.
    /// Unknown values from the server must be handled as
    /// <see cref="Unknown"/> — never crash.
    /// </summary>
    public enum ErrorAction
    {
        Unknown,
        LoginAgain,
        Retry,
        WaitThenRetry,
        RefreshSyncStatus,
        ReturnToMenu,
        ShowOfflinePrompt,
        ContactTeacher
    }
}