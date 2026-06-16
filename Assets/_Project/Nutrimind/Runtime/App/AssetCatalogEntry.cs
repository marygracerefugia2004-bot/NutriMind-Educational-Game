namespace NutriMind.Runtime.App
{
    /// <summary>
    /// Represents a single entry in an asset catalog,
    /// mapping a stable <see cref="Key"/> to a <see cref="Path"/>.
    /// </summary>
    public class AssetCatalogEntry
    {
        /// <summary>Stable identifier for this asset.</summary>
        public string Key { get; }

        /// <summary>Asset path (e.g. a Resources path or addressable key).</summary>
        public string Path { get; }

        /// <summary>
        /// Creates a new catalog entry.
        /// </summary>
        public AssetCatalogEntry(string key, string path)
        {
            Key = key;
            Path = path;
        }
    }
}
