namespace NutriMind.Runtime.App
{
    /// <summary>
    /// Typed store for authentication session state.
    /// Holds the current auth token, student identity returned
    /// from a successful login, and session-related flags that
    /// survive across scene transitions but reset on logout.
    /// </summary>
    public class AuthSessionState
    {
        /// <summary>Current authentication bearer token, if any.</summary>
        public string? Token { get; set; }

        // ──────────────────────────────────────────────────────────────
        //  Login-response identity data
        // ──────────────────────────────────────────────────────────────

        /// <summary>Server-assigned student ID (e.g. <c>"stu_101"</c>).</summary>
        public string? StudentId { get; set; }

        /// <summary>Student display name returned at login.</summary>
        public string? StudentName { get; set; }

        /// <summary>Masked LRN for display (e.g. <c>"1234••••9012"</c>).</summary>
        public string? LrnMasked { get; set; }

        /// <summary>Grade level returned from the server.</summary>
        public int? GradeLevel { get; set; }

        /// <summary>Language preference from login (e.g. <c>"en"</c>, <c>"tl"</c>).</summary>
        public string? LanguagePreference { get; set; }

        /// <summary>The token type returned by the server (typically <c>"Bearer"</c>).</summary>
        public string? TokenType { get; set; }

        /// <summary>
        /// True when the session has a non-null token and the student has
        /// been authenticated.  Shortcut for callers that only need a boolean check.
        /// </summary>
        public bool IsAuthenticated => !string.IsNullOrEmpty(Token);

        // ──────────────────────────────────────────────────────────────
        //  Lifecycle
        // ──────────────────────────────────────────────────────────────

        /// <summary>
        /// Populates session state from a successful login response.
        /// Sets the bearer token and all available student identity fields.
        /// </summary>
        public void ApplyLoginResponse(Dto.LoginResponseDto response)
        {
            if (response == null) return;

            Token = response.Token;
            TokenType = response.TokenType;

            if (response.Student != null)
            {
                StudentId = response.Student.Id;
                StudentName = response.Student.Name;
                LrnMasked = response.Student.LrnMasked;
                GradeLevel = response.Student.GradeLevel;
                LanguagePreference = response.Student.LanguagePreference;
            }
        }

        /// <summary>Resets all state to defaults.</summary>
        public void Reset()
        {
            Token = null;
            StudentId = null;
            StudentName = null;
            LrnMasked = null;
            GradeLevel = null;
            LanguagePreference = null;
            TokenType = null;
        }
    }
}
