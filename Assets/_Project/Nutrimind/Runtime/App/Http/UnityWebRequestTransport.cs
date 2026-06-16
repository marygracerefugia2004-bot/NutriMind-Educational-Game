using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace NutriMind.Runtime.App.Http
{
    /// <summary>
    /// Production HTTP transport backed by <see cref="UnityWebRequest"/>.
    /// Sanitizes logs and never writes tokens, PINs, or Authorization headers
    /// to diagnostics.
    /// </summary>
    public class UnityWebRequestTransport : IHttpRequestTransport
    {
        /// <inheritdoc />
        public async Task<HttpTransportResponse> SendAsync(HttpTransportRequest request, CancellationToken ct = default)
        {
            var uwr = new UnityWebRequest(request.Url, request.Method)
            {
                timeout = request.TimeoutSeconds
            };

            if (!string.IsNullOrEmpty(request.BodyJson))
            {
                byte[] bodyBytes = Encoding.UTF8.GetBytes(request.BodyJson);
                uwr.uploadHandler = new UploadHandlerRaw(bodyBytes)
                {
                    contentType = "application/json"
                };
            }

            uwr.downloadHandler = new DownloadHandlerBuffer();

            foreach (var header in request.Headers)
            {
                uwr.SetRequestHeader(header.Key, header.Value);
            }

            try
            {
                await uwr.SendWebRequest().WithCancellation(ct);
            }
            catch (OperationCanceledException)
            {
                uwr.Dispose();
                throw;
            }
            catch (System.Exception ex)
            {
                // Transport-level failure before an HTTP response.
                var failure = new HttpTransportResponse
                {
                    StatusCode = 0,
                    IsNetworkError = true,
                    ErrorMessage = "Network request failed.",
                    Body = string.Empty
                };

                SafeDiagnostics.LogDiagnostic(
                    $"UnityWebRequest transport error for {request.Method} {SanitizeUrl(request.Url)}: {SafeDiagnostics.SanitizeDiagnostics(ex.Message)}");

                uwr.Dispose();
                return failure;
            }

            var response = new HttpTransportResponse
            {
                StatusCode = uwr.responseCode,
                Body = uwr.downloadHandler?.text ?? string.Empty,
                IsNetworkError = uwr.result == UnityWebRequest.Result.ConnectionError || uwr.result == UnityWebRequest.Result.DataProcessingError,
                ErrorMessage = uwr.result == UnityWebRequest.Result.ProtocolError || uwr.result == UnityWebRequest.Result.ConnectionError
                    ? SafeDiagnostics.SanitizeDiagnostics(uwr.error)
                    : string.Empty
            };

            // Capture selected response headers safely.
            if (uwr.GetResponseHeader("Retry-After") is { } retryAfter)
                response.ResponseHeaders["Retry-After"] = retryAfter;
            if (uwr.GetResponseHeader("X-Request-Id") is { } requestId)
                response.ResponseHeaders["X-Request-Id"] = requestId;

            uwr.Dispose();
            return response;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            // UnityWebRequest instances are disposed per request.
        }

        /// <summary>
        /// Removes any query-string secrets from a URL before logging.
        /// </summary>
        private static string SanitizeUrl(string url)
        {
            if (string.IsNullOrEmpty(url)) return url;
            int queryIndex = url.IndexOf('?');
            return queryIndex >= 0 ? url.Substring(0, queryIndex) + "?***" : url;
        }
    }

    /// <summary>
    /// UnityWebRequest operation extension that supports external cancellation.
    /// </summary>
    internal static class UnityWebRequestAsyncOperationExtensions
    {
        public static async Task<UnityWebRequestAsyncOperation> WithCancellation(this UnityWebRequestAsyncOperation operation, CancellationToken ct)
        {
            using (ct.Register(() => operation.webRequest?.Abort()))
            {
                while (!operation.isDone)
                {
                    await Task.Yield();
                }
            }

            ct.ThrowIfCancellationRequested();
            return operation;
        }
    }
}
