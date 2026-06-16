using UnityEngine;

namespace NutriMind.Runtime.UI
{
    public static class NutriMindSafeAreaUtility
    {
        public static RectOffset CalculatePadding()
        {
            return CalculatePadding(Screen.safeArea);
        }

        public static RectOffset CalculatePadding(Rect safeArea)
        {
            var screen = new Vector2(Screen.width, Screen.height);
            return GetPanelPadding(safeArea, screen, screen);
        }

        public static RectOffset GetPanelPadding(Rect safeArea, Vector2 panelResolution)
        {
            // Two-arg overload: assume panelResolution IS the screen resolution.
            return GetPanelPadding(safeArea, panelResolution, panelResolution);
        }

        /// <summary>
        /// Computes safe-area padding scaled from physical screen pixel space to
        /// panel-space (UI Toolkit virtual coordinates).
        /// </summary>
        /// <param name="safeArea">The device safe area in physical pixels.</param>
        /// <param name="screenResolution">Physical screen dimensions.</param>
        /// <param name="panelResolution">UI Toolkit panel reference resolution.</param>
        /// <returns>RectOffset with inset values clamped non-negative.</returns>
        public static RectOffset GetPanelPadding(Rect safeArea, Vector2 screenResolution, Vector2 panelResolution)
        {
            float scrW = screenResolution.x > 0f ? screenResolution.x : 1f;
            float scrH = screenResolution.y > 0f ? screenResolution.y : 1f;
            float panW = panelResolution.x > 0f ? panelResolution.x : 1f;
            float panH = panelResolution.y > 0f ? panelResolution.y : 1f;

            float scaleX = panW / scrW;
            float scaleY = panH / scrH;

            int left   = Mathf.RoundToInt(Mathf.Max(0f, safeArea.x * scaleX));
            int bottom = Mathf.RoundToInt(Mathf.Max(0f, safeArea.y * scaleY));
            int right  = Mathf.RoundToInt(Mathf.Max(0f, (scrW - (safeArea.x + safeArea.width)) * scaleX));
            int top    = Mathf.RoundToInt(Mathf.Max(0f, (scrH - (safeArea.y + safeArea.height)) * scaleY));

            return new RectOffset(left, right, top, bottom);
        }

        public static bool IsAndroidLandscape
        {
            get
            {
#if UNITY_ANDROID && !UNITY_EDITOR
                return Screen.width > Screen.height;
#else
                return false;
#endif
            }
        }
    }
}
