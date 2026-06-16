namespace NutriMind.Runtime.App
{
    /// <summary>
    /// Typed store for user settings and preferences.
    /// Houses audio, language, and general configuration choices
    /// that are loaded at session start and cleared on logout.
    /// </summary>
    public class SettingsStore
    {
        /// <summary>Preferred language code (e.g. "en", "fil").</summary>
        public string? Language { get; set; }

        /// <summary>Music volume (0–1). Default is 1.</summary>
        public float MusicVolume { get; set; } = 1f;

        /// <summary>Sound effects volume (0–1). Default is 1.</summary>
        public float SfxVolume { get; set; } = 1f;

        /// <summary>Resets all state to defaults.</summary>
        public void Reset()
        {
            Language = null;
            MusicVolume = 1f;
            SfxVolume = 1f;
        }
    }
}
