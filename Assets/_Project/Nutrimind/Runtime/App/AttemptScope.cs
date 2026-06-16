using System;

namespace NutriMind.Runtime.App
{
    /// <summary>
    /// Tracks a single attempt context, including the stable
    /// <see cref="ClientAttemptUuid"/> that survives retries.
    /// </summary>
    public class AttemptScope
    {
        /// <summary>
        /// Stable identifier for this attempt. Generated on construction
        /// and preserved across <see cref="Retry"/> calls.
        /// </summary>
        public string ClientAttemptUuid { get; private set; }

        /// <summary>
        /// Creates a new attempt scope with a fresh UUID.
        /// </summary>
        public AttemptScope()
        {
            ClientAttemptUuid = Guid.NewGuid().ToString("N");
        }

        /// <summary>
        /// Marks the current attempt as needing retry.
        /// The <see cref="ClientAttemptUuid"/> is preserved so the server
        /// can deduplicate the submission.
        /// </summary>
        public void Retry()
        {
            // UUID intentionally unchanged — retry reuses the same identifier
            // for idempotent submission.
        }
    }
}
