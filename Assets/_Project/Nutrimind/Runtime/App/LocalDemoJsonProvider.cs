using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NutriMind.Runtime.App.Dto;

namespace NutriMind.Runtime.App
{
    /// <summary>
    /// Placeholder local-demo provider.  Does not read demo JSON yet.
    /// All operations return a safe "not yet implemented" result.
    /// Concrete JSON reading and deserialization is expected in Phase 04+/08.
    /// Local demo fixture payloads must deserialize through the same DTOs
    /// used by the HTTP provider.
    /// </summary>
    public class LocalDemoJsonProvider : IGameDataProvider
    {
        private static DataResult<T> NotImplemented<T>() =>
            DataResult<T>.Fail(new DataProviderError("not_implemented",
                "Local demo provider is not yet implemented."));

        // ── Connectivity & Config ──────────────────────────────────

        /// <inheritdoc />
        public Task<DataResult<PingResponseDto>> PingAsync(CancellationToken ct = default)
            => Task.FromResult(NotImplemented<PingResponseDto>());

        /// <inheritdoc />
        public Task<DataResult<ApiConfigDto>> GetConfigAsync(CancellationToken ct = default)
            => Task.FromResult(NotImplemented<ApiConfigDto>());

        // ── Auth ────────────────────────────────────────────────────

        /// <inheritdoc />
        public Task<DataResult<LoginResponseDto>> LoginAsync(LoginRequestDto request, CancellationToken ct = default)
            => Task.FromResult(NotImplemented<LoginResponseDto>());

        /// <inheritdoc />
        public Task<DataResult<object>> LogoutAsync(CancellationToken ct = default)
            => Task.FromResult(NotImplemented<object>());

        // ── Bootstrap & Profile ─────────────────────────────────────

        /// <inheritdoc />
        public Task<DataResult<BootstrapDto>> GetBootstrapAsync(CancellationToken ct = default)
            => Task.FromResult(NotImplemented<BootstrapDto>());

        /// <inheritdoc />
        public Task<DataResult<StudentProfileDto>> GetProfileAsync(CancellationToken ct = default)
            => Task.FromResult(NotImplemented<StudentProfileDto>());

        // ── Settings ────────────────────────────────────────────────

        /// <inheritdoc />
        public Task<DataResult<SettingsDto>> GetSettingsAsync(CancellationToken ct = default)
            => Task.FromResult(NotImplemented<SettingsDto>());

        /// <inheritdoc />
        public Task<DataResult<SettingsDto>> PatchSettingsAsync(SettingsDto settings, CancellationToken ct = default)
            => Task.FromResult(NotImplemented<SettingsDto>());

        // ── Subjects, Terms, Stations ──────────────────────────────

        /// <inheritdoc />
        public Task<DataResult<List<SubjectDto>>> GetSubjectsAsync(CancellationToken ct = default)
            => Task.FromResult(NotImplemented<List<SubjectDto>>());

        /// <inheritdoc />
        public Task<DataResult<List<TermDto>>> GetTermsAsync(string subjectSlug, CancellationToken ct = default)
            => Task.FromResult(NotImplemented<List<TermDto>>());

        /// <inheritdoc />
        public Task<DataResult<List<StationDto>>> GetStationsAsync(string subjectSlug, int termNumber, CancellationToken ct = default)
            => Task.FromResult(NotImplemented<List<StationDto>>());

        // ── Station Content & Session ──────────────────────────────

        /// <inheritdoc />
        public Task<DataResult<StationContentDto>> GetStationContentAsync(string stationId, CancellationToken ct = default)
            => Task.FromResult(NotImplemented<StationContentDto>());

        /// <inheritdoc />
        public Task<DataResult<StationStartResponseDto>> StartStationAsync(string stationId, StationStartRequestDto request = null, CancellationToken ct = default)
            => Task.FromResult(NotImplemented<StationStartResponseDto>());

        // ── Attempts ───────────────────────────────────────────────

        /// <inheritdoc />
        public Task<DataResult<AttemptResponseDto>> SubmitAttemptAsync(string challengeId, AttemptRequestDto request, CancellationToken ct = default)
            => Task.FromResult(NotImplemented<AttemptResponseDto>());

        // ── Station Completion ─────────────────────────────────────

        /// <inheritdoc />
        public Task<DataResult<StationCompleteResponseDto>> CompleteStationAsync(string stationId, StationCompleteRequestDto request = null, CancellationToken ct = default)
            => Task.FromResult(NotImplemented<StationCompleteResponseDto>());

        // ── Progress & Rewards ──────────────────────────────────────

        /// <inheritdoc />
        public Task<DataResult<ProgressSummaryDto>> GetProgressSummaryAsync(CancellationToken ct = default)
            => Task.FromResult(NotImplemented<ProgressSummaryDto>());

        /// <inheritdoc />
        public Task<DataResult<RewardWalletDto>> GetRewardsAsync(CancellationToken ct = default)
            => Task.FromResult(NotImplemented<RewardWalletDto>());

        /// <inheritdoc />
        public Task<DataResult<UseRewardResponseDto>> UseRewardAsync(string rewardCode, UseRewardRequestDto request, CancellationToken ct = default)
            => Task.FromResult(NotImplemented<UseRewardResponseDto>());

        // ── Sync ────────────────────────────────────────────────────

        /// <inheritdoc />
        public Task<DataResult<SyncStatusDto>> GetSyncStatusAsync(CancellationToken ct = default)
            => Task.FromResult(NotImplemented<SyncStatusDto>());
    }
}