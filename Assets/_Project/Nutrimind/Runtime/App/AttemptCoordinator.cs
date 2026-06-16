using System;

namespace NutriMind.Runtime.App
{
    /// <summary>
    /// Coordinates the lifecycle of a single attempt submission.
    /// Owns an <see cref="AttemptScope"/> and enforces at-most-one in-flight
    /// submission.  Duplicate submits are rejected until <see cref="RetryAttempt"/>
    /// or a new scope is created.  No scoring, progress, or reward authority.
    /// </summary>
    public class AttemptCoordinator
    {
        private AttemptScope _scope;
        private bool _isInFlight;

        /// <summary>
        /// Creates a new coordinator with a fresh attempt scope.
        /// </summary>
        public AttemptCoordinator()
        {
            _scope = new AttemptScope();
        }

        /// <summary>
        /// The current attempt scope.  Its <see cref="AttemptScope.ClientAttemptUuid"/>
        /// is stable across retries.
        /// </summary>
        public AttemptScope CurrentAttempt => _scope;

        /// <summary>
        /// Convenience accessor for the stable client-attempt UUID.
        /// </summary>
        public string ClientAttemptUuid => _scope.ClientAttemptUuid;

        /// <summary>
        /// Submits the current attempt.  The caller provides the answer payload
        /// (interpreted by the data-provider layer).
        /// </summary>
        /// <param name="answers">The answer payload to submit.</param>
        /// <returns>True if the submission was accepted; false if an attempt is already in-flight.</returns>
        public bool SubmitAttempt(object? answers = null)
        {
            if (_isInFlight)
                return false;

            _isInFlight = true;
            // Actual submission is delegated to the data-provider layer.
            // This method only coordinates lifecycle.
            return true;
        }

        /// <summary>
        /// Retries the current attempt, resetting the in-flight flag
        /// so the next <see cref="SubmitAttempt"/> call is accepted.
        /// The <see cref="ClientAttemptUuid"/> is preserved for idempotent
        /// server-side deduplication.
        /// </summary>
        public void RetryAttempt()
        {
            _scope.Retry();
            _isInFlight = false;
        }

        /// <summary>
        /// Resets the coordinator with a fresh attempt scope.
        /// </summary>
        public void Reset()
        {
            _scope = new AttemptScope();
            _isInFlight = false;
        }
    }
}
