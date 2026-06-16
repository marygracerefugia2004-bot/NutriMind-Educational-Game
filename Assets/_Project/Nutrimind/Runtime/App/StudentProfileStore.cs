namespace NutriMind.Runtime.App
{
    /// <summary>
    /// Typed store for the current student's profile information.
    /// Populated by the data-provider layer and reset on logout.
    /// </summary>
    public class StudentProfileStore
    {
        /// <summary>Display name of the student.</summary>
        public string? DisplayName { get; set; }

        /// <summary>Student identifier from the server.</summary>
        public string? StudentId { get; set; }

        /// <summary>Resets all state to defaults.</summary>
        public void Reset()
        {
            DisplayName = null;
            StudentId = null;
        }
    }
}
