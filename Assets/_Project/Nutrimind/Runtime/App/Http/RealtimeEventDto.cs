using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace NutriMind.Runtime.App.Http
{
    /// <summary>
    /// Optional metadata-only realtime event envelope.  WebSocket is not
    /// required for core gameplay; events are hints that tell Unity to refresh
    /// authoritative data over HTTPS REST.
    /// </summary>
    public class RealtimeEventDto
    {
        [JsonProperty("event_id", NullValueHandling = NullValueHandling.Ignore)]
        public string? EventId { get; set; }

        [JsonProperty("event_type", NullValueHandling = NullValueHandling.Ignore)]
        public string? EventType { get; set; }

        [JsonProperty("contract_version", NullValueHandling = NullValueHandling.Ignore)]
        public string? ContractVersion { get; set; }

        [JsonProperty("server_time", NullValueHandling = NullValueHandling.Ignore)]
        public string? ServerTime { get; set; }

        [JsonProperty("student_scope", NullValueHandling = NullValueHandling.Ignore)]
        public RealtimeStudentScope? StudentScope { get; set; }

        [JsonProperty("revisions", NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, string>? Revisions { get; set; }

        [JsonProperty("action", NullValueHandling = NullValueHandling.Ignore)]
        public string? Action { get; set; }
    }

    /// <summary>
    /// Student/classroom scope inside a realtime event.
    /// </summary>
    public class RealtimeStudentScope
    {
        [JsonProperty("student_id", NullValueHandling = NullValueHandling.Ignore)]
        public string? StudentId { get; set; }

        [JsonProperty("classroom_id", NullValueHandling = NullValueHandling.Ignore)]
        public string? ClassroomId { get; set; }
    }

    /// <summary>
    /// Realtime service contract.  Implementations may use WebSocket, a no-op,
    /// or any future transport.  Events are metadata-only hints.
    /// </summary>
    public interface IRealtimeService : IDisposable
    {
        /// <summary>
        /// True when the service is connected and receiving events.
        /// </summary>
        bool IsConnected { get; }

        /// <summary>
        /// Raised for each parsed event.  Unknown event types are ignored by
        /// consumers; malformed events are non-fatal.
        /// </summary>
        event Action<RealtimeEventDto> EventReceived;

        /// <summary>
        /// Attempts to connect using the bearer token from the session.
        /// </summary>
        void Connect(string url, string token);

        /// <summary>
        /// Disconnects and releases resources.
        /// </summary>
        void Disconnect();
    }

    /// <summary>
    /// No-op fallback realtime service used when no WebSocket package is
    /// available or when realtime is disabled.  The provider falls back to
    /// <see cref="SyncPollingService"/>.
    /// </summary>
    public class NoOpRealtimeService : IRealtimeService
    {
        /// <inheritdoc />
        public bool IsConnected => false;

        /// <inheritdoc />
        public event Action<RealtimeEventDto> EventReceived
        {
            add { }
            remove { }
        }

        /// <inheritdoc />
        public void Connect(string url, string token)
        {
            // Realtime is optional; no connection is attempted.
            SafeDiagnostics.LogDiagnostic("NoOpRealtimeService: realtime is disabled (no transport).");
        }

        /// <inheritdoc />
        public void Disconnect()
        {
            // No-op.
        }

        /// <inheritdoc />
        public void Dispose()
        {
            // No-op.
        }
    }
}
