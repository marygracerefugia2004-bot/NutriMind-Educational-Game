using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace NutriMind.Runtime.App
{
    /// <summary>
    /// Validates an asset catalog for duplicate keys, missing mappings,
    /// and provides safe resolve/try-resolve semantics.
    /// Does not determine content availability (e.g. does not check whether
    /// a path actually exists on disk).
    /// </summary>
    public class AssetCatalogValidator
    {
        private readonly Dictionary<string, AssetCatalogEntry> _catalog = new();
        private readonly Dictionary<string, int> _registrationCount = new();

        /// <summary>
        /// Registers an asset entry in the catalog.
        /// Duplicate keys are silently ignored (the first registration is preserved);
        /// they are reported through <see cref="Validate"/> and <see cref="CheckForDuplicates"/>.
        /// </summary>
        public void Register(AssetCatalogEntry entry)
        {
            TrackRegistration(entry.Key);
            if (!_catalog.ContainsKey(entry.Key))
            {
                _catalog.Add(entry.Key, entry);
            }
        }

        /// <summary>
        /// Registers an asset entry by key and path.
        /// Duplicate keys are silently ignored (the first registration is preserved);
        /// they are reported through <see cref="Validate"/> and <see cref="CheckForDuplicates"/>.
        /// </summary>
        public void Register(string key, string path)
        {
            TrackRegistration(key);
            if (!_catalog.ContainsKey(key))
            {
                _catalog.Add(key, new AssetCatalogEntry(key, path));
            }
        }

        private void TrackRegistration(string key)
        {
            _registrationCount.TryGetValue(key, out int count);
            _registrationCount[key] = count + 1;
        }

        /// <summary>
        /// Validates the entire catalog and returns a <see cref="ValidationResult"/>
        /// with diagnostic information.
        /// Safe to call on an empty catalog — does not throw.
        /// Duplicate key detection uses registration-attempt counts.
        /// </summary>
        public ValidationResult Validate()
        {
            var issues = new List<string>();

            foreach (var kvp in _registrationCount)
            {
                if (kvp.Value > 1)
                {
                    issues.Add($"Duplicate asset key: '{kvp.Key}'");
                }
            }

            foreach (var kvp in _catalog)
            {
                if (string.IsNullOrEmpty(kvp.Value.Path))
                {
                    issues.Add($"Missing mapping for key: '{kvp.Key}'");
                }
            }

            return new ValidationResult(issues);
        }

        /// <summary>
        /// Checks whether the catalog contains duplicate keys.
        /// </summary>
        /// <returns>True if duplicates were found.</returns>
        public bool CheckForDuplicates()
        {
            foreach (var kvp in _registrationCount)
            {
                if (kvp.Value > 1)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Finds entries whose mapping (path) is null or empty.
        /// </summary>
        /// <returns>List of keys with missing mappings.</returns>
        public List<string> CheckForMissingMappings()
        {
            var missing = new List<string>();
            foreach (var kvp in _catalog)
            {
                if (string.IsNullOrEmpty(kvp.Value.Path))
                    missing.Add(kvp.Key);
            }
            return missing;
        }

        /// <summary>
        /// Resolves an asset entry by key.
        /// Returns null if the key is not registered.
        /// </summary>
        public AssetCatalogEntry? ResolveAsset(string key)
        {
            _catalog.TryGetValue(key, out var entry);
            return entry;
        }

        /// <summary>
        /// Tries to resolve an asset entry by key.
        /// </summary>
        /// <returns>True if the key was found.</returns>
        public bool TryResolve(string key, out AssetCatalogEntry? entry)
        {
            return _catalog.TryGetValue(key, out entry);
        }
    }

    /// <summary>
    /// Result of a catalog validation run, carrying diagnostic information
    /// about issues found.
    /// </summary>
    public class ValidationResult
    {
        /// <summary>List of issues found during validation.</summary>
        public IReadOnlyList<string> Issues { get; }

        /// <summary>True when the catalog has validation issues.</summary>
        public bool HasErrors => Issues.Count > 0;

        /// <summary>Synonym for <see cref="Issues"/>.</summary>
        public IReadOnlyList<string> Errors => Issues;

        /// <summary>Synonym for <see cref="Issues"/>.</summary>
        public IReadOnlyList<string> Diagnostics => Issues;

        /// <summary>Creates a validation result with the given list of issues.</summary>
        public ValidationResult(List<string> issues)
        {
            Issues = new ReadOnlyCollection<string>(issues ?? new List<string>());
        }
    }
}
