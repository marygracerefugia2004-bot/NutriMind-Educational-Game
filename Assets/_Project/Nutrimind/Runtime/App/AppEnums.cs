namespace NutriMind.Runtime.App
{
    /// <summary>
    /// Canonical application states for the NutriMind game flow state machine.
    /// All 25 values are shared by both <see cref="DataProviderMode"/> variants.
    ///
    /// CONTRACT: AppState integer ordinals are NOT a stable serialization or
    /// persistence contract. Consumers MUST persist or refer to states by
    /// their enum name (Enum.ToString / Enum.Parse) or by an explicit external
    /// key rather than by numeric value. Enum member ordering may change as
    /// new states are inserted and existing ordinals may shift.
    /// </summary>
    public enum AppState
    {
        Starting,
        CheckingServer,
        MaintenanceBlocked,
        UpdateRequired,
        LoggedOut,
        Authenticating,
        Bootstrapping,
        MainMenu,
        SelectingSubject,
        SelectingTerm,
        LoadingWorld,
        InWorld,
        StartingStation,
        InStationTask,
        SubmittingAttempt,
        ShowingFeedback,
        RefreshingProgress,
        ReturningToWorld,
        ShowingMissionBrief,
        ShowingHintOverlay,
        ShowingReflection,
        ShowingRewardPresentation,
        SessionExpired,
        ConnectionUnavailable,
        FatalConfigurationError
    }

    /// <summary>
    /// Back-end data provider mode.
    /// Both modes share the same state machine and flow.
    /// </summary>
    public enum DataProviderMode
    {
        LocalDemoJson,
        Http
    }

    /// <summary>
    /// Subject categories available in the application.
    /// </summary>
    public enum SubjectType
    {
        LiteraQuest,
        ScienceQuest,
        HealthQuest
    }

    /// <summary>
    /// Lifecycle state of a gameplay station.
    /// </summary>
    public enum StationState
    {
        Unavailable,
        Locked,
        Unlocked,
        Started,
        AttemptPending,
        PendingReview,
        Completed,
        ArchivedOrUnpublished
    }

    /// <summary>
    /// Lightweight learning cycle phases for gameplay state representation.
    /// Future gameplay can map provider-driven narrative to these phases.
    /// </summary>
    public enum LearningCyclePhase
    {
        Discover,
        Practice,
        Apply,
        Review
    }
}
