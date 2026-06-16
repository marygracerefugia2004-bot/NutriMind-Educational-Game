using NUnit.Framework;
using NutriMind.Runtime.UI;

namespace NutriMind.Tests.EditMode.UI
{
    [TestFixture]
    public class NutriMindAccessibilityStyleTests
    {
        private string _accessibilityUss;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            string fullPath = System.IO.Path.GetFullPath(NutriMindAssetPaths.StyleAccessibility);
            _accessibilityUss = System.IO.File.ReadAllText(fullPath);
        }

        // ===============================================================
        // Large text
        // ===============================================================

        [Test]
        public void AccessibilityUss_HasLargeTextClass()
        {
            Assert.That(_accessibilityUss, Does.Contain(".nm-large-text"),
                "Accessibility.uss must define .nm-large-text for large text mode");
        }

        [Test]
        public void LargeTextMode_PreservesTypographicHierarchy()
        {
            Assert.That(_accessibilityUss, Does.Contain(".nm-large-text .nm-heading-3"),
                ".nm-large-text .nm-heading-3 must define a heading-3 font-size (e.g. 32px) "
                + "that is larger than body. The blanket .nm-large-text * { font-size: 20px } "
                + "shrinks headings, breaking hierarchy.");

            Assert.That(_accessibilityUss, Does.Contain(".nm-large-text .nm-heading-4"),
                ".nm-large-text .nm-heading-4 must define a font-size (e.g. 24px) "
                + "that is larger than body. The blanket .nm-large-text * { font-size: 20px } "
                + "shrinks headings, breaking hierarchy.");

            Assert.That(_accessibilityUss, Does.Contain(".nm-large-text .nm-body"),
                ".nm-large-text .nm-body must define a comfortable large-body font-size "
                + "(e.g. 20px) that is distinct from headings and captions.");

            Assert.That(_accessibilityUss, Does.Contain(".nm-large-text .nm-caption"),
                ".nm-large-text .nm-caption must define a font-size smaller than body. "
                + "The blanket .nm-large-text * { font-size: 20px } makes captions too large.");
        }

        // ===============================================================
        // Component-specific large-text overrides
        // ===============================================================

        [Test]
        public void LargeText_OverridesScreenHeaderTitle()
        {
            Assert.That(_accessibilityUss, Does.Contain(".nm-large-text .nm-screen-header-title"),
                ".nm-large-text .nm-screen-header-title must override font-size "
                + "in large-text mode. Currently Screens.uss defines font-size "
                + "via var(--nm-font-size-h4) with no .nm-large-text fallback. "
                + "This is a Phase 09 addition not yet implemented.");
        }

        [Test]
        public void LargeText_OverridesFieldLabel()
        {
            Assert.That(_accessibilityUss, Does.Contain(".nm-large-text .nm-field-label"),
                ".nm-large-text .nm-field-label must override font-size "
                + "in large-text mode. Currently Components.uss defines font-size "
                + "via var(--nm-font-size-small) with no .nm-large-text fallback. "
                + "This is a Phase 09 addition not yet implemented.");
        }

        [Test]
        public void LargeText_OverridesFieldHint()
        {
            Assert.That(_accessibilityUss, Does.Contain(".nm-large-text .nm-field-hint"),
                ".nm-large-text .nm-field-hint must override font-size "
                + "in large-text mode. This is a Phase 09 addition not yet implemented.");
        }

        [Test]
        public void LargeText_OverridesFieldError()
        {
            Assert.That(_accessibilityUss, Does.Contain(".nm-large-text .nm-field-error"),
                ".nm-large-text .nm-field-error must override font-size "
                + "in large-text mode. This is a Phase 09 addition not yet implemented.");
        }

        [Test]
        public void LargeText_OverridesPinField()
        {
            Assert.That(_accessibilityUss, Does.Contain(".nm-large-text .nm-pin-field"),
                ".nm-large-text .nm-pin-field must override font-size "
                + "in large-text mode. This is a Phase 09 addition not yet implemented.");
        }

        [Test]
        public void LargeText_OverridesDialogTitle()
        {
            Assert.That(_accessibilityUss, Does.Contain(".nm-large-text .nm-dialog-title"),
                ".nm-large-text .nm-dialog-title must override font-size "
                + "in large-text mode. This is a Phase 09 addition not yet implemented.");
        }

        [Test]
        public void LargeText_OverridesDialogBody()
        {
            Assert.That(_accessibilityUss, Does.Contain(".nm-large-text .nm-dialog-body"),
                ".nm-large-text .nm-dialog-body must override font-size "
                + "in large-text mode. This is a Phase 09 addition not yet implemented.");
        }

        [Test]
        public void LargeText_OverridesErrorBox()
        {
            Assert.That(_accessibilityUss, Does.Contain(".nm-large-text .nm-error-box"),
                ".nm-large-text .nm-error-box must override font-size "
                + "in large-text mode. This is a Phase 09 addition not yet implemented.");
        }

        [Test]
        public void LargeText_OverridesSelectionControlLabel()
        {
            Assert.That(_accessibilityUss, Does.Contain(".nm-large-text .nm-selection-control-label"),
                ".nm-large-text .nm-selection-control-label must override font-size "
                + "in large-text mode. This is a Phase 09 addition not yet implemented.");
        }

        [Test]
        public void LargeText_OverridesConnectionBadge()
        {
            Assert.That(_accessibilityUss, Does.Contain(".nm-large-text .nm-connection-badge"),
                ".nm-large-text .nm-connection-badge must override font-size "
                + "in large-text mode. This is a Phase 09 addition not yet implemented.");
        }

        [Test]
        public void LargeText_OverridesProfileName()
        {
            Assert.That(_accessibilityUss, Does.Contain(".nm-large-text .nm-profile-name"),
                ".nm-large-text .nm-profile-name must override font-size "
                + "in large-text mode. This is a Phase 09 addition not yet implemented.");
        }

        [Test]
        public void LargeText_OverridesProfileDetail()
        {
            Assert.That(_accessibilityUss, Does.Contain(".nm-large-text .nm-profile-detail"),
                ".nm-large-text .nm-profile-detail must override font-size "
                + "in large-text mode. This is a Phase 09 addition not yet implemented.");
        }

        [Test]
        public void LargeText_OverridesSettingsRowLabel()
        {
            Assert.That(_accessibilityUss, Does.Contain(".nm-large-text .nm-settings-row-label"),
                ".nm-large-text .nm-settings-row-label must override font-size "
                + "in large-text mode. This is a Phase 09 addition not yet implemented.");
        }

        [Test]
        public void LargeText_OverridesSettingsRowValue()
        {
            Assert.That(_accessibilityUss, Does.Contain(".nm-large-text .nm-settings-row-value"),
                ".nm-large-text .nm-settings-row-value must override font-size "
                + "in large-text mode. This is a Phase 09 addition not yet implemented.");
        }

        [Test]
        public void LargeText_OverridesEmptyTitle()
        {
            Assert.That(_accessibilityUss, Does.Contain(".nm-large-text .nm-empty-title"),
                ".nm-large-text .nm-empty-title must override font-size "
                + "in large-text mode. This is a Phase 09 addition not yet implemented.");
        }

        [Test]
        public void LargeText_OverridesEmptyMessage()
        {
            Assert.That(_accessibilityUss, Does.Contain(".nm-large-text .nm-empty-message"),
                ".nm-large-text .nm-empty-message must override font-size "
                + "in large-text mode. This is a Phase 09 addition not yet implemented.");
        }

        [Test]
        public void LargeText_OverridesFeedback()
        {
            Assert.That(_accessibilityUss, Does.Contain(".nm-large-text .nm-feedback"),
                ".nm-large-text .nm-feedback must override font-size "
                + "in large-text mode. This is a Phase 09 addition not yet implemented.");
        }

        [Test]
        public void LargeText_OverridesRewardTitle()
        {
            Assert.That(_accessibilityUss, Does.Contain(".nm-large-text .nm-reward-title"),
                ".nm-large-text .nm-reward-title must override font-size "
                + "in large-text mode. This is a Phase 09 addition not yet implemented.");
        }

        [Test]
        public void LargeText_OverridesRewardSubtitle()
        {
            Assert.That(_accessibilityUss, Does.Contain(".nm-large-text .nm-reward-subtitle"),
                ".nm-large-text .nm-reward-subtitle must override font-size "
                + "in large-text mode. This is a Phase 09 addition not yet implemented.");
        }

        [Test]
        public void LargeText_OverridesRewardAmount()
        {
            Assert.That(_accessibilityUss, Does.Contain(".nm-large-text .nm-reward-amount"),
                ".nm-large-text .nm-reward-amount must override font-size "
                + "in large-text mode. This is a Phase 09 addition not yet implemented.");
        }

        // ===============================================================
        // Reduced motion
        // ===============================================================

        [Test]
        public void AccessibilityUss_HasReducedMotionClass()
        {
            Assert.That(_accessibilityUss, Does.Contain(".nm-reduced-motion"),
                "Accessibility.uss must define .nm-reduced-motion for reduced motion preferences");
        }

        // ===============================================================
        // High contrast
        // ===============================================================

        [Test]
        public void AccessibilityUss_HasHighContrastClass()
        {
            Assert.That(_accessibilityUss, Does.Contain(".nm-high-contrast"),
                "Accessibility.uss must define .nm-high-contrast for high contrast mode");
        }

        // ===============================================================
        // Touch target helpers
        // ===============================================================

        [Test]
        public void AccessibilityUss_HasTouchTargetAndroidLandscapeClass()
        {
            Assert.That(_accessibilityUss, Does.Contain(".nm-touch-android-landscape"),
                "Accessibility.uss must define .nm-touch-android-landscape for landscape touch targets");
        }

        // ===============================================================
        // Screen reader
        // ===============================================================

        [Test]
        public void AccessibilityUss_HasScreenReaderOnlyClass()
        {
            Assert.That(_accessibilityUss, Does.Contain(".nm-sr-only"),
                "Accessibility.uss must define .nm-sr-only for screen-reader-only elements");
        }

        // ===============================================================
        // Safe-area helper classes
        // ===============================================================

        [Test]
        public void AccessibilityUss_HasSafeAreaLeftClass()
        {
            Assert.That(_accessibilityUss, Does.Contain(".nm-safe-area-left"),
                "Accessibility.uss must define .nm-safe-area-left for left safe-area padding. " +
                "This is a Phase 09 addition not yet implemented.");
        }

        [Test]
        public void AccessibilityUss_HasSafeAreaRightClass()
        {
            Assert.That(_accessibilityUss, Does.Contain(".nm-safe-area-right"),
                "Accessibility.uss must define .nm-safe-area-right for right safe-area padding. " +
                "This is a Phase 09 addition not yet implemented.");
        }

        [Test]
        public void AccessibilityUss_HasSafeAreaTopClass()
        {
            Assert.That(_accessibilityUss, Does.Contain(".nm-safe-area-top"),
                "Accessibility.uss must define .nm-safe-area-top for top safe-area padding. " +
                "This is a Phase 09 addition not yet implemented.");
        }

        [Test]
        public void AccessibilityUss_HasSafeAreaBottomClass()
        {
            Assert.That(_accessibilityUss, Does.Contain(".nm-safe-area-bottom"),
                "Accessibility.uss must define .nm-safe-area-bottom for bottom safe-area padding. " +
                "This is a Phase 09 addition not yet implemented.");
        }

        [Test]
        public void AccessibilityUss_HasFocusRingClass()
        {
            Assert.That(_accessibilityUss, Does.Contain("focus"),
                "Accessibility.uss must define focus ring styles for keyboard/controller navigation");
        }
    }
}