namespace NutriMind.Runtime.App
{
    /// <summary>
    /// Wraps the outcome of a data-provider operation.
    /// A successful result has <see cref="Success"/> = true and
    /// carries the payload in <see cref="Data"/>.
    /// A failed result has <see cref="Success"/> = false and
    /// describes the problem in <see cref="Error"/> (a <see cref="DataProviderError"/>
    /// with stable <see cref="DataProviderError.Code"/> and human-readable
    /// <see cref="DataProviderError.Message"/>).
    /// <see cref="ErrorMessage"/> is maintained for backward compatibility.
    /// </summary>
    /// <typeparam name="T">The payload type on success.</typeparam>
    public class DataResult<T>
    {
        /// <summary>True when the operation completed successfully.</summary>
        public bool Success { get; }

        /// <summary>
        /// Structured error information. Non-null when <see cref="Success"/> is false.
        /// Contains a stable <see cref="DataProviderError.Code"/> for programmatic handling.
        /// </summary>
        public DataProviderError Error { get; }

        /// <summary>
        /// Human-readable error description. Non-null when <see cref="Success"/> is false.
        /// Maintained for backward compatibility — prefer <see cref="Error"/> for new code.
        /// </summary>
        public string ErrorMessage => Error?.Message;

        /// <summary>The operation payload. Valid only when <see cref="Success"/> is true.</summary>
        public T Data { get; }

        private DataResult(bool success, T data, DataProviderError error)
        {
            Success = success;
            Data = data;
            Error = error;
        }

        /// <summary>Creates a successful result with the given payload.</summary>
        public static DataResult<T> Ok(T data) =>
            new DataResult<T>(true, data, null);

        /// <summary>
        /// Creates a failed result with the given error message and a stable
        /// error code of <c>"not_implemented"</c>.  Used by placeholder providers
        /// that have not yet implemented real data fetching.
        /// </summary>
        public static DataResult<T> Fail(string errorMessage) =>
            new DataResult<T>(false, default, new DataProviderError("not_implemented", errorMessage));

        /// <summary>
        /// Creates a failed result with a structured <see cref="DataProviderError"/>.
        /// </summary>
        public static DataResult<T> Fail(DataProviderError error) =>
            new DataResult<T>(false, default, error);
    }
}
