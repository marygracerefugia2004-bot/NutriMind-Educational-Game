using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using NutriMind.Runtime.UI;

namespace NutriMind.Tests.EditMode.UI
{
    [TestFixture]
    public class NutriMindAssetPathTests
    {
        [Test]
        public void RuntimePanelSettings_LoadsAtExpectedPath()
        {
            var settings = AssetDatabase.LoadAssetAtPath<PanelSettings>(
                NutriMindAssetPaths.RuntimePanelSettings);

            Assert.That(settings, Is.Not.Null,
                "Runtime PanelSettings asset not found at {0}",
                NutriMindAssetPaths.RuntimePanelSettings);
        }

        [Test]
        public void AppShellUxml_LoadsAtExpectedPath()
        {
            var template = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(
                NutriMindAssetPaths.AppShell);

            Assert.That(template, Is.Not.Null,
                "AppShell UXML not found at {0}", NutriMindAssetPaths.AppShell);
        }

        [Test]
        public void AppShellUxml_ReferencesAllFoundationStyleSheets()
        {
            string fullPath = System.IO.Path.GetFullPath(NutriMindAssetPaths.AppShell);
            string content = System.IO.File.ReadAllText(fullPath);

            Assert.That(content, Does.Contain("Base.uss"),
                "AppShell.uxml should reference Base.uss via <Style src=...>");
            Assert.That(content, Does.Contain("Components.uss"),
                "AppShell.uxml should reference Components.uss via <Style src=...>");
            Assert.That(content, Does.Contain("Screens.uss"),
                "AppShell.uxml should reference Screens.uss via <Style src=...>");
            Assert.That(content, Does.Contain("Accessibility.uss"),
                "AppShell.uxml should reference Accessibility.uss via <Style src=...>");
        }

        [Test]
        public void AppShellUxml_ReferencesThemeStyleSheet()
        {
            string fullPath = System.IO.Path.GetFullPath(NutriMindAssetPaths.AppShell);
            string content = System.IO.File.ReadAllText(fullPath);

            Assert.That(content, Does.Contain("NutriMindTheme.uss"),
                "AppShell.uxml should reference NutriMindTheme.uss via <Style src=...> " +
                "so theme variables are available to all child elements");
        }

        [Test]
        public void AppShellUxml_DoesNotContainWebAriaAttributes()
        {
            string fullPath = System.IO.Path.GetFullPath(NutriMindAssetPaths.AppShell);
            string content = System.IO.File.ReadAllText(fullPath);

            Assert.That(content, Does.Not.Contain("aria-live"),
                "AppShell.uxml should not contain unsupported web ARIA attribute aria-live");
            Assert.That(content, Does.Not.Contain("role="),
                "AppShell.uxml should not contain unsupported web ARIA attribute role");
        }

        [Test]
        public void BaseStyleSheet_LoadsAtExpectedPath()
        {
            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(
                NutriMindAssetPaths.StyleBase);

            Assert.That(styleSheet, Is.Not.Null,
                "Base style sheet not found at {0}", NutriMindAssetPaths.StyleBase);
        }

        [Test]
        public void ComponentsStyleSheet_LoadsAtExpectedPath()
        {
            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(
                NutriMindAssetPaths.StyleComponents);

            Assert.That(styleSheet, Is.Not.Null,
                "Components style sheet not found at {0}", NutriMindAssetPaths.StyleComponents);
        }

        [Test]
        public void ScreensStyleSheet_LoadsAtExpectedPath()
        {
            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(
                NutriMindAssetPaths.StyleScreens);

            Assert.That(styleSheet, Is.Not.Null,
                "Screens style sheet not found at {0}", NutriMindAssetPaths.StyleScreens);
        }

        [Test]
        public void AccessibilityStyleSheet_LoadsAtExpectedPath()
        {
            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(
                NutriMindAssetPaths.StyleAccessibility);

            Assert.That(styleSheet, Is.Not.Null,
                "Accessibility style sheet not found at {0}", NutriMindAssetPaths.StyleAccessibility);
        }

        [Test]
        public void NavBarComponent_LoadsAtExpectedPath()
        {
            var template = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(
                NutriMindAssetPaths.ComponentNavBar);

            Assert.That(template, Is.Not.Null,
                "NavBar UXML not found at {0}", NutriMindAssetPaths.ComponentNavBar);
        }

        [Test]
        public void BottomNavComponent_LoadsAtExpectedPath()
        {
            var template = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(
                NutriMindAssetPaths.ComponentBottomNav);

            Assert.That(template, Is.Not.Null,
                "BottomNav UXML not found at {0}", NutriMindAssetPaths.ComponentBottomNav);
        }

        [Test]
        public void ScreenFrameComponent_LoadsAtExpectedPath()
        {
            var template = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(
                NutriMindAssetPaths.ComponentScreenFrame);

            Assert.That(template, Is.Not.Null,
                "ScreenFrame UXML not found at {0}", NutriMindAssetPaths.ComponentScreenFrame);
        }

        [Test]
        public void ComponentsStyleSheet_HasGhostButtonRule()
        {
            string fullPath = System.IO.Path.GetFullPath(NutriMindAssetPaths.StyleComponents);
            string content = System.IO.File.ReadAllText(fullPath);

            Assert.That(content, Does.Contain(".nm-btn-ghost"),
                "Components.uss should define the .nm-btn-ghost class used by NavBar.uxml");
        }


        // ===============================================================
        // Phase 04A — Mission / NPC / Hint / Discovery / Reflection / Feedback
        // ===============================================================

        [Test]
        public void MissionBriefOverlay_LoadsAtExpectedPath()
        {
            var template = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(
                NutriMindAssetPaths.OverlayMissionBrief);

            Assert.That(template, Is.Not.Null,
                "MissionBriefOverlay.uxml not found at {0}", NutriMindAssetPaths.OverlayMissionBrief);
        }

        [Test]
        public void HintOverlay_LoadsAtExpectedPath()
        {
            var template = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(
                NutriMindAssetPaths.OverlayHint);

            Assert.That(template, Is.Not.Null,
                "HintOverlay.uxml not found at {0}", NutriMindAssetPaths.OverlayHint);
        }

        [Test]
        public void ReflectionOverlay_LoadsAtExpectedPath()
        {
            var template = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(
                NutriMindAssetPaths.OverlayReflection);

            Assert.That(template, Is.Not.Null,
                "ReflectionOverlay.uxml not found at {0}", NutriMindAssetPaths.OverlayReflection);
        }

        [Test]
        public void RewardOverlay_LoadsAtExpectedPath()
        {
            var template = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(
                NutriMindAssetPaths.OverlayReward);

            Assert.That(template, Is.Not.Null,
                "RewardOverlay.uxml not found at {0}", NutriMindAssetPaths.OverlayReward);
        }

        [Test]
        public void NpcDialogueComponent_LoadsAtExpectedPath()
        {
            var template = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(
                NutriMindAssetPaths.ComponentNpcDialogue);

            Assert.That(template, Is.Not.Null,
                "NpcDialogue.uxml not found at {0}", NutriMindAssetPaths.ComponentNpcDialogue);
        }

        [Test]
        public void DiscoveryCardComponent_LoadsAtExpectedPath()
        {
            var template = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(
                NutriMindAssetPaths.ComponentDiscoveryCard);

            Assert.That(template, Is.Not.Null,
                "DiscoveryCard.uxml not found at {0}", NutriMindAssetPaths.ComponentDiscoveryCard);
        }

        [Test]
        public void FeedbackPanelComponent_LoadsAtExpectedPath()
        {
            var template = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(
                NutriMindAssetPaths.ComponentFeedbackPanel);

            Assert.That(template, Is.Not.Null,
                "FeedbackPanel.uxml not found at {0}", NutriMindAssetPaths.ComponentFeedbackPanel);
        }

        [Test]
        public void AllPathConstants_AreNonEmpty()
        {
            Assert.That(NutriMindAssetPaths.AppShell, Is.Not.Empty);
            Assert.That(NutriMindAssetPaths.RuntimePanelSettings, Is.Not.Empty);
            Assert.That(NutriMindAssetPaths.StyleBase, Is.Not.Empty);
            Assert.That(NutriMindAssetPaths.StyleComponents, Is.Not.Empty);
            Assert.That(NutriMindAssetPaths.StyleScreens, Is.Not.Empty);
            Assert.That(NutriMindAssetPaths.StyleAccessibility, Is.Not.Empty);
            Assert.That(NutriMindAssetPaths.ComponentNavBar, Is.Not.Empty);
            Assert.That(NutriMindAssetPaths.ComponentBottomNav, Is.Not.Empty);
            Assert.That(NutriMindAssetPaths.ComponentScreenFrame, Is.Not.Empty);
            Assert.That(NutriMindAssetPaths.OverlayMissionBrief, Is.Not.Empty);
            Assert.That(NutriMindAssetPaths.OverlayHint, Is.Not.Empty);
            Assert.That(NutriMindAssetPaths.OverlayReflection, Is.Not.Empty);
            Assert.That(NutriMindAssetPaths.OverlayReward, Is.Not.Empty);
            Assert.That(NutriMindAssetPaths.ComponentNpcDialogue, Is.Not.Empty);
            Assert.That(NutriMindAssetPaths.ComponentDiscoveryCard, Is.Not.Empty);
            Assert.That(NutriMindAssetPaths.ComponentFeedbackPanel, Is.Not.Empty);
        }
    }
}
