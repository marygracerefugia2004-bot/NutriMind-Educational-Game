using System;

namespace NutriMind.Runtime.App
{
    /// <summary>
    /// Scene-navigation service that resolves scene paths from
    /// stable string keys via <see cref="SceneRegistry"/>.
    /// This is the primary API for runtime scene transitions —
    /// raw scene paths should not be used directly.
    /// </summary>
    public class NavigationService
    {
        private readonly SceneRegistry _registry;

        /// <summary>
        /// Creates a navigation service backed by the given scene registry.
        /// </summary>
        /// <param name="registry">The scene registry to resolve keys against.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="registry"/> is null.</exception>
        public NavigationService(SceneRegistry registry)
        {
            _registry = registry ?? throw new ArgumentNullException(nameof(registry));
        }

        /// <summary>
        /// Resolves a scene navigation key and returns a result object
        /// indicating whether the key is available and the resolved path.
        /// This is the primary key-based navigation API.
        /// </summary>
        /// <param name="sceneKey">Stable identifier for the target scene.</param>
        /// <returns>A <see cref="NavigationResult"/> with <see cref="NavigationResult.IsAvailable"/>
        /// set to true if the key was found, and <see cref="NavigationResult.ScenePath"/> containing
        /// the resolved scene path.</returns>
        public NavigationResult Navigate(string sceneKey)
        {
            if (sceneKey == null)
                return new NavigationResult(false, null);

            string? path = _registry.GetScene(sceneKey);
            return new NavigationResult(path != null, path);
        }

        /// <summary>
        /// Resolves a scene path from a stable navigation key.
        /// Returns null if the key is not registered.
        /// </summary>
        /// <param name="sceneKey">Stable identifier for the target scene.</param>
        /// <returns>The registered scene path, or null if the key is not found.</returns>
        public string? Resolve(string sceneKey)
        {
            if (sceneKey == null) return null;
            return _registry.GetScene(sceneKey);
        }

        /// <summary>
        /// Synonym for <see cref="Resolve"/> that provides a safe try-pattern.
        /// </summary>
        /// <param name="sceneKey">Stable identifier for the target scene.</param>
        /// <param name="scenePath">The resolved scene path, or null if not found.</param>
        /// <returns>True if the key was registered and a scene path was resolved.</returns>
        public bool TryResolve(string sceneKey, out string? scenePath)
        {
            return _registry.TryGetScene(sceneKey, out scenePath);
        }
    }

    /// <summary>
    /// Result of a <see cref="NavigationService.Navigate"/> call.
    /// Indicates whether the scene key was found and provides the resolved path.
    /// </summary>
    public class NavigationResult
    {
        /// <summary>
        /// Whether the scene key was registered and a path is available.
        /// </summary>
        public bool IsAvailable { get; }

        /// <summary>
        /// The resolved scene path, or null if the key was not found.
        /// </summary>
        public string? ScenePath { get; }

        /// <summary>
        /// Creates a navigation result.
        /// </summary>
        public NavigationResult(bool isAvailable, string? scenePath)
        {
            IsAvailable = isAvailable;
            ScenePath = scenePath;
        }
    }
}
