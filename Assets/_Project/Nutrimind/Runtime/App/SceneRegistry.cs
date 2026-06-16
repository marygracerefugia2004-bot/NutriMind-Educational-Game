using System;
using System.Collections.Generic;

namespace NutriMind.Runtime.App
{
    /// <summary>
    /// Registry that maps stable string keys to scene asset paths.
    /// Supports registration, lookup, and optional build-settings validation
    /// via an injectable set of known-build-scene paths.
    /// </summary>
    public sealed class SceneRegistry
    {
        private readonly Dictionary<string, string> _scenes = new();
        private readonly HashSet<string> _knownBuildScenes;

        /// <summary>
        /// Creates an empty registry.  All registered scenes are accepted;
        /// build-settings validation will always return false unless
        /// <paramref name="knownBuildScenes"/> is provided.
        /// </summary>
        public SceneRegistry() : this(null) { }

        /// <summary>
        /// Creates a registry with an optional set of known build-scene paths
        /// used by <see cref="ValidateSceneInBuildSettings"/>.
        /// </summary>
        /// <param name="knownBuildScenes">
        /// Scene paths that are considered "in build settings".
        /// May be null (all validation returns false).
        /// </param>
        public SceneRegistry(HashSet<string>? knownBuildScenes)
        {
            _knownBuildScenes = knownBuildScenes ?? new HashSet<string>();
        }

        /// <summary>
        /// Number of registered scene entries.
        /// </summary>
        public int Count => _scenes.Count;

        /// <summary>
        /// Registers a stable <paramref name="key"/> to a scene
        /// <paramref name="scenePath"/>.
        /// </summary>
        /// <param name="key">Stable identifier for the scene. Must not be null or empty.</param>
        /// <param name="scenePath">Asset path of the scene. Must not be null or empty.</param>
        /// <returns>True if the entry was added; false if the key already exists.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="key"/> or <paramref name="scenePath"/> is null.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="key"/> or <paramref name="scenePath"/> is empty.</exception>
        public bool RegisterScene(string key, string scenePath)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));
            if (scenePath == null) throw new ArgumentNullException(nameof(scenePath));
            if (key.Length == 0) throw new ArgumentException("Key must not be empty.", nameof(key));
            if (scenePath.Length == 0) throw new ArgumentException("Scene path must not be empty.", nameof(scenePath));

            if (_scenes.ContainsKey(key))
                return false;

            _scenes.Add(key, scenePath);
            return true;
        }

        /// <summary>
        /// Looks up a scene path by its stable key.
        /// </summary>
        /// <param name="key">The stable identifier.</param>
        /// <returns>The registered scene path, or null if the key is not registered.</returns>
        public string? GetScene(string key)
        {
            if (key == null) return null;

            _scenes.TryGetValue(key, out string? path);
            return path;
        }

        /// <summary>
        /// Tries to look up a scene path by its stable key.
        /// </summary>
        /// <param name="key">The stable identifier.</param>
        /// <param name="scenePath">The registered scene path, or null if not found.</param>
        /// <returns>True if the key was registered.</returns>
        public bool TryGetScene(string key, out string? scenePath)
        {
            if (key == null)
            {
                scenePath = null;
                return false;
            }

            return _scenes.TryGetValue(key, out scenePath);
        }

        /// <summary>
        /// Validates whether the given <paramref name="scenePath"/>
        /// was included in the known-build-scenes set provided at construction.
        /// </summary>
        /// <param name="scenePath">Scene asset path to check.</param>
        /// <returns>True if the path is in the known-build-scenes set.</returns>
        public bool ValidateSceneInBuildSettings(string scenePath)
        {
            return _knownBuildScenes.Contains(scenePath);
        }
    }
}
