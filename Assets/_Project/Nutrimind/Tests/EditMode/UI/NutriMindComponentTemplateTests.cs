using NUnit.Framework;
using UnityEditor;
using UnityEngine.UIElements;
using NutriMind.Runtime.UI;

namespace NutriMind.Tests.EditMode.UI
{
    [TestFixture]
    public class NutriMindComponentTemplateTests
    {
        private const string ComponentsFolder =
            "Assets/_Project/Nutrimind/UI/Documents/Components";

        private const string OverlaysFolder =
            "Assets/_Project/Nutrimind/UI/Documents/Overlays";

        // ===============================================================
        // Existing component templates — already under UI/Documents/Components
        // ===============================================================

        [Test]
        public void NavBarTemplate_LoadsAtComponentsFolder()
        {
            var template = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(
                NutriMindAssetPaths.ComponentNavBar);
            Assert.That(template, Is.Not.Null,
                "NavBar.uxml must exist under UI/Documents/Components");
        }

        [Test]
        public void BottomNavTemplate_LoadsAtComponentsFolder()
        {
            var template = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(
                NutriMindAssetPaths.ComponentBottomNav);
            Assert.That(template, Is.Not.Null,
                "BottomNav.uxml must exist under UI/Documents/Components");
        }

        [Test]
        public void ScreenFrameTemplate_LoadsAtComponentsFolder()
        {
            var template = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(
                NutriMindAssetPaths.ComponentScreenFrame);
            Assert.That(template, Is.Not.Null,
                "ScreenFrame.uxml must exist under UI/Documents/Components");
        }

        // ===============================================================
        // Phase 09 — shared overlay templates that need creation
        // ===============================================================

        [Test]
        public void LoadingOverlayTemplate_LoadsAtOverlaysFolder()
        {
            var template = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(
                OverlaysFolder + "/LoadingOverlay.uxml");
            Assert.That(template, Is.Not.Null,
                "LoadingOverlay.uxml must exist under UI/Documents/Overlays. " +
                "This is a Phase 09 addition not yet implemented.");
        }

        [Test]
        public void DialogOverlayTemplate_LoadsAtOverlaysFolder()
        {
            var template = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(
                OverlaysFolder + "/DialogOverlay.uxml");
            Assert.That(template, Is.Not.Null,
                "DialogOverlay.uxml must exist under UI/Documents/Overlays. " +
                "This is a Phase 09 addition not yet implemented.");
        }

        [Test]
        public void ErrorOverlayTemplate_LoadsAtOverlaysFolder()
        {
            var template = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(
                OverlaysFolder + "/ErrorOverlay.uxml");
            Assert.That(template, Is.Not.Null,
                "ErrorOverlay.uxml must exist under UI/Documents/Overlays. " +
                "This is a Phase 09 addition not yet implemented.");
        }

        [Test]
        public void RewardOverlayTemplate_LoadsAtOverlaysFolder()
        {
            var template = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(
                NutriMindAssetPaths.OverlayReward);
            Assert.That(template, Is.Not.Null,
                "RewardOverlay.uxml must exist under UI/Documents/Overlays.");
        }

        // ===============================================================
        // Phase 09 — shared card templates under UI/Documents/Components
        // ===============================================================

        [Test]
        public void SubjectCardTemplate_LoadsAtComponentsFolder()
        {
            var template = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(
                ComponentsFolder + "/CardSubject.uxml");
            Assert.That(template, Is.Not.Null,
                "CardSubject.uxml must exist under UI/Documents/Components. " +
                "This is a Phase 09 addition not yet implemented.");
        }

        [Test]
        public void TermCardTemplate_LoadsAtComponentsFolder()
        {
            var template = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(
                ComponentsFolder + "/CardTerm.uxml");
            Assert.That(template, Is.Not.Null,
                "CardTerm.uxml must exist under UI/Documents/Components. " +
                "This is a Phase 09 addition not yet implemented.");
        }

        [Test]
        public void StationCardTemplate_LoadsAtComponentsFolder()
        {
            var template = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(
                ComponentsFolder + "/CardStation.uxml");
            Assert.That(template, Is.Not.Null,
                "CardStation.uxml must exist under UI/Documents/Components. " +
                "This is a Phase 09 addition not yet implemented.");
        }

        [Test]
        public void ProfileSummaryTemplate_LoadsAtComponentsFolder()
        {
            var template = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(
                ComponentsFolder + "/ProfileSummary.uxml");
            Assert.That(template, Is.Not.Null,
                "ProfileSummary.uxml must exist under UI/Documents/Components. " +
                "This is a Phase 09 addition not yet implemented.");
        }

        [Test]
        public void SettingsRowTemplate_LoadsAtComponentsFolder()
        {
            var template = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(
                ComponentsFolder + "/SettingsRow.uxml");
            Assert.That(template, Is.Not.Null,
                "SettingsRow.uxml must exist under UI/Documents/Components. " +
                "This is a Phase 09 addition not yet implemented.");
        }

        [Test]
        public void ConnectionBadgeTemplate_LoadsAtComponentsFolder()
        {
            var template = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(
                ComponentsFolder + "/ConnectionBadge.uxml");
            Assert.That(template, Is.Not.Null,
                "ConnectionBadge.uxml must exist under UI/Documents/Components. " +
                "This is a Phase 09 addition not yet implemented.");
        }

        // ===============================================================
        // Phase 09 — shared interactive template focus/activation semantics
        // ===============================================================

        private static void AssertRootIsFocusable(string uxmlRelativePath, string displayName)
        {
            string fullPath = System.IO.Path.GetFullPath(uxmlRelativePath);
            Assert.That(System.IO.File.Exists(fullPath), Is.True,
                $"{displayName} not found at {uxmlRelativePath}");

            var lines = System.IO.File.ReadAllLines(fullPath);
            string rootLine = null;
            bool pastUxml = false;

            foreach (var rawLine in lines)
            {
                string line = rawLine.Trim();
                if (line.StartsWith("<ui:UXML")) { pastUxml = true; continue; }
                if (!pastUxml) continue;
                if (line.Length == 0 || line.StartsWith("<!--")) continue;

                rootLine = line;
                break;
            }

            Assert.That(rootLine, Is.Not.Null,
                $"Could not locate root element opening tag in {displayName}");

            int spaceIdx = rootLine.IndexOf(' ');
            int closeIdx = rootLine.IndexOf('>');
            int tagEndIdx = (spaceIdx >= 0 && closeIdx >= 0)
                ? (spaceIdx < closeIdx ? spaceIdx : closeIdx)
                : (spaceIdx >= 0 ? spaceIdx : closeIdx);
            string tagName = tagEndIdx > 0
                ? rootLine.Substring(0, tagEndIdx)
                : rootLine;

            bool isButtonRoot = tagName == "<ui:Button";
            bool hasFocusableTrue = rootLine.Contains("focusable=\"true\"");

            Assert.That(isButtonRoot || hasFocusableTrue, Is.True,
                $"{displayName} root ({tagName}) must be <ui:Button> "
                + "(implicitly focusable) or have focusable=\"true\" set explicitly.\n"
                + $"Current root line: {rootLine}");
        }

        [Test]
        public void SubjectCardTemplate_HasFocusableRoot()
        {
            AssertRootIsFocusable(
                ComponentsFolder + "/CardSubject.uxml",
                "CardSubject.uxml");
        }

        [Test]
        public void TermCardTemplate_HasFocusableRoot()
        {
            AssertRootIsFocusable(
                ComponentsFolder + "/CardTerm.uxml",
                "CardTerm.uxml");
        }

        [Test]
        public void StationCardTemplate_HasFocusableRoot()
        {
            AssertRootIsFocusable(
                ComponentsFolder + "/CardStation.uxml",
                "CardStation.uxml");
        }

        [Test]
        public void SettingsRowTemplate_HasFocusableRoot()
        {
            AssertRootIsFocusable(
                ComponentsFolder + "/SettingsRow.uxml",
                "SettingsRow.uxml");
        }

        // ===============================================================
        // Interactive template activation contract — strict Button root
        //
        // Interactive shared templates must use <ui:Button> as their root
        // element (built-in click/activate semantics), not merely a focusable
        // VisualElement. This ensures keyboard/gamepad "submit" works
        // without per-template manual wiring.
        // ===============================================================

        private static void AssertRootIsButton(string uxmlRelativePath, string displayName)
        {
            string fullPath = System.IO.Path.GetFullPath(uxmlRelativePath);
            Assert.That(System.IO.File.Exists(fullPath), Is.True,
                $"{displayName} not found at {uxmlRelativePath}");

            var lines = System.IO.File.ReadAllLines(fullPath);
            string rootLine = null;
            bool pastUxml = false;

            foreach (var rawLine in lines)
            {
                string line = rawLine.Trim();
                if (line.StartsWith("<ui:UXML")) { pastUxml = true; continue; }
                if (!pastUxml) continue;
                if (line.Length == 0 || line.StartsWith("<!--")) continue;

                rootLine = line;
                break;
            }

            Assert.That(rootLine, Is.Not.Null,
                $"Could not locate root element opening tag in {displayName}");

            bool isButtonRoot = rootLine.StartsWith("<ui:Button");

            Assert.That(isButtonRoot, Is.True,
                $"{displayName} root must be <ui:Button> for built-in activation semantics. "
                + $"Current root element line: {rootLine}. "
                + "A focusable VisualElement does not have click/activate built-in behavior. "
                + "This is a Phase 09 addition not yet implemented.");
        }

        [Test]
        public void SubjectCardTemplate_HasButtonRoot()
        {
            AssertRootIsButton(
                ComponentsFolder + "/CardSubject.uxml",
                "CardSubject.uxml");
        }

        [Test]
        public void TermCardTemplate_HasButtonRoot()
        {
            AssertRootIsButton(
                ComponentsFolder + "/CardTerm.uxml",
                "CardTerm.uxml");
        }

        [Test]
        public void StationCardTemplate_HasButtonRoot()
        {
            AssertRootIsButton(
                ComponentsFolder + "/CardStation.uxml",
                "CardStation.uxml");
        }

        [Test]
        public void SettingsRowTemplate_HasButtonRoot()
        {
            AssertRootIsButton(
                ComponentsFolder + "/SettingsRow.uxml",
                "SettingsRow.uxml");
        }

        // ===============================================================
        // Phase 04A — Mission / NPC / Hint / Reflection overlay templates
        // ===============================================================

        [Test]
        public void MissionBriefOverlayTemplate_LoadsAtOverlaysFolder()
        {
            var template = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(
                OverlaysFolder + "/MissionBriefOverlay.uxml");
            Assert.That(template, Is.Not.Null,
                "MissionBriefOverlay.uxml must exist under UI/Documents/Overlays. " +
                "This is a Phase 04A addition not yet implemented.");
        }

        [Test]
        public void HintOverlayTemplate_LoadsAtOverlaysFolder()
        {
            var template = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(
                OverlaysFolder + "/HintOverlay.uxml");
            Assert.That(template, Is.Not.Null,
                "HintOverlay.uxml must exist under UI/Documents/Overlays. " +
                "This is a Phase 04A addition not yet implemented.");
        }

        [Test]
        public void ReflectionOverlayTemplate_LoadsAtOverlaysFolder()
        {
            var template = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(
                OverlaysFolder + "/ReflectionOverlay.uxml");
            Assert.That(template, Is.Not.Null,
                "ReflectionOverlay.uxml must exist under UI/Documents/Overlays. " +
                "This is a Phase 04A addition not yet implemented.");
        }

        [Test]
        public void NpcDialogueTemplate_LoadsAtComponentsFolder()
        {
            var template = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(
                ComponentsFolder + "/NpcDialogue.uxml");
            Assert.That(template, Is.Not.Null,
                "NpcDialogue.uxml must exist under UI/Documents/Components. " +
                "This is a Phase 04A addition not yet implemented.");
        }

        [Test]
        public void DiscoveryCardTemplate_LoadsAtComponentsFolder()
        {
            var template = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(
                ComponentsFolder + "/DiscoveryCard.uxml");
            Assert.That(template, Is.Not.Null,
                "DiscoveryCard.uxml must exist under UI/Documents/Components. " +
                "This is a Phase 04A addition not yet implemented.");
        }

        [Test]
        public void FeedbackPanelTemplate_LoadsAtComponentsFolder()
        {
            var template = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(
                ComponentsFolder + "/FeedbackPanel.uxml");
            Assert.That(template, Is.Not.Null,
                "FeedbackPanel.uxml must exist under UI/Documents/Components. " +
                "This is a Phase 04A addition not yet implemented.");
        }

        // ===============================================================
        // Phase 04A — Overlay named-element presence
        // ===============================================================

        [Test]
        public void MissionBriefOverlay_HasRequiredNamedElements()
        {
            string path = System.IO.Path.GetFullPath(OverlaysFolder + "/MissionBriefOverlay.uxml");
            string cnt = System.IO.File.ReadAllText(path);
            Assert.That(cnt, Does.Contain("mission-npc-name"), "Missing #mission-npc-name");
            Assert.That(cnt, Does.Contain("mission-title"), "Missing #mission-title");
            Assert.That(cnt, Does.Contain("mission-description"), "Missing #mission-description");
            Assert.That(cnt, Does.Contain("mission-objective"), "Missing #mission-objective");
            Assert.That(cnt, Does.Contain("mission-skip-button"), "Missing #mission-skip-button");
            Assert.That(cnt, Does.Contain("mission-begin-button"), "Missing #mission-begin-button");
        }

        [Test]
        public void HintOverlay_HasRequiredNamedElements()
        {
            string path = System.IO.Path.GetFullPath(OverlaysFolder + "/HintOverlay.uxml");
            string cnt = System.IO.File.ReadAllText(path);
            Assert.That(cnt, Does.Contain("hint-tier-label"), "Missing #hint-tier-label");
            Assert.That(cnt, Does.Contain("hint-title"), "Missing #hint-title");
            Assert.That(cnt, Does.Contain("hint-text"), "Missing #hint-text");
            Assert.That(cnt, Does.Contain("hint-try-again-button"), "Missing #hint-try-again-button");
        }

        [Test]
        public void ReflectionOverlay_HasRequiredNamedElements()
        {
            string path = System.IO.Path.GetFullPath(OverlaysFolder + "/ReflectionOverlay.uxml");
            string cnt = System.IO.File.ReadAllText(path);
            Assert.That(cnt, Does.Contain("reflection-title"), "Missing #reflection-title");
            Assert.That(cnt, Does.Contain("reflection-question"), "Missing #reflection-question");
            Assert.That(cnt, Does.Contain("reflection-skip-button"), "Missing #reflection-skip-button");
            Assert.That(cnt, Does.Contain("reflection-continue-button"), "Missing #reflection-continue-button");
        }

        [Test]
        public void RewardOverlay_HasRequiredNamedElements()
        {
            string path = System.IO.Path.GetFullPath(NutriMindAssetPaths.OverlayReward);
            string cnt = System.IO.File.ReadAllText(path);
            Assert.That(cnt, Does.Contain("reward-icon"), "Missing #reward-icon");
            Assert.That(cnt, Does.Contain("reward-title"), "Missing #reward-title");
            Assert.That(cnt, Does.Contain("reward-subtitle"), "Missing #reward-subtitle");
            Assert.That(cnt, Does.Contain("reward-amount"), "Missing #reward-amount");
            Assert.That(cnt, Does.Contain("reward-continue-button"), "Missing #reward-continue-button");
        }

        [Test]
        public void NpcDialogue_HasRequiredNamedElements()
        {
            string path = System.IO.Path.GetFullPath(ComponentsFolder + "/NpcDialogue.uxml");
            string cnt = System.IO.File.ReadAllText(path);
            Assert.That(cnt, Does.Contain("npc-name"), "Missing #npc-name");
            Assert.That(cnt, Does.Contain("nm-npc-name"), "npc-name label should include dedicated .nm-npc-name class");
            Assert.That(cnt, Does.Contain("npc-portrait"), "Missing #npc-portrait");
            Assert.That(cnt, Does.Contain("npc-dialogue-text"), "Missing #npc-dialogue-text");
            Assert.That(cnt, Does.Contain("npc-dismiss-button"), "Missing #npc-dismiss-button");
        }

        [Test]
        public void DiscoveryCard_HasRequiredNamedElements()
        {
            string path = System.IO.Path.GetFullPath(ComponentsFolder + "/DiscoveryCard.uxml");
            string cnt = System.IO.File.ReadAllText(path);
            Assert.That(cnt, Does.Contain("discovery-icon"), "Missing #discovery-icon");
            Assert.That(cnt, Does.Contain("discovery-title"), "Missing #discovery-title");
            Assert.That(cnt, Does.Contain("discovery-text"), "Missing #discovery-text");
            Assert.That(cnt, Does.Contain("discovery-dismiss-button"), "Missing #discovery-dismiss-button");
        }

        [Test]
        public void FeedbackPanel_HasRequiredNamedElements()
        {
            string path = System.IO.Path.GetFullPath(ComponentsFolder + "/FeedbackPanel.uxml");
            string cnt = System.IO.File.ReadAllText(path);
            Assert.That(cnt, Does.Contain("feedback-icon"), "Missing #feedback-icon");
            Assert.That(cnt, Does.Contain("feedback-title"), "Missing #feedback-title");
            Assert.That(cnt, Does.Contain("feedback-message"), "Missing #feedback-message");
            Assert.That(cnt, Does.Contain("feedback-retry-button"), "Missing #feedback-retry-button");
            Assert.That(cnt, Does.Contain("feedback-continue-button"), "Missing #feedback-continue-button");
        }

        // ===============================================================
        // Phase 04A — Focusable root check for interactive component templates
        // ===============================================================

        [Test]
        public void NpcDialogueTemplate_HasFocusableRoot()
        {
            AssertRootIsFocusable(
                ComponentsFolder + "/NpcDialogue.uxml",
                "NpcDialogue.uxml");
        }

        [Test]
        public void DiscoveryCardTemplate_HasFocusableRoot()
        {
            AssertRootIsFocusable(
                ComponentsFolder + "/DiscoveryCard.uxml",
                "DiscoveryCard.uxml");
        }

        [Test]
        public void FeedbackPanelTemplate_HasFocusableRoot()
        {
            AssertRootIsFocusable(
                ComponentsFolder + "/FeedbackPanel.uxml",
                "FeedbackPanel.uxml");
        }
    }
}
