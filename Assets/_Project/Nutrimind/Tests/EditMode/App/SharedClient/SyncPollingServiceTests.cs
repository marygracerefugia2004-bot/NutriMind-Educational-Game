using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using NutriMind.Runtime.App;
using NutriMind.Runtime.App.Dto;
using NutriMind.Runtime.App.Http;

namespace NutriMind.Tests.EditMode.App
{
    /// <summary>
    /// Unit tests for <see cref="SyncPollingService"/> using a fake transport.
    /// </summary>
    [TestFixture]
    public class SyncPollingServiceTests
    {
        [Test]
        public void SyncPollingService_ChangedEvent_FiresOnRevisionChange()
        {
            var transport = new FakeHttpTransport();
            transport.EnqueueSuccess(200, "{\"student_progress_revision\":\"rev_1\"}");
            transport.EnqueueSuccess(200, "{\"student_progress_revision\":\"rev_2\"}");

            var config = new HttpProviderConfig { BaseUrl = "https://test.example", DefaultPollIntervalSeconds = 0 };
            var session = new AuthSessionState { Token = "token" };
            var provider = new HttpProvider(config, session, transport);
            var polling = new SyncPollingService(provider, config, session);

            var changes = new List<SyncStatusDto>();
            polling.Changed += status => changes.Add(status);

            polling.Start();
            Thread.Sleep(200);
            polling.Stop();

            Assert.That(changes.Count, Is.GreaterThanOrEqualTo(1));
            Assert.That(changes[^1].StudentProgressRevision, Is.EqualTo("rev_2"));
        }

        [Test]
        public void SyncPollingService_NoChange_NoChangedEvent()
        {
            var transport = new FakeHttpTransport();
            transport.EnqueueSuccess(200, "{\"student_progress_revision\":\"rev_1\"}");
            transport.EnqueueSuccess(200, "{\"student_progress_revision\":\"rev_1\"}");

            var config = new HttpProviderConfig { BaseUrl = "https://test.example", DefaultPollIntervalSeconds = 0 };
            var session = new AuthSessionState { Token = "token" };
            var provider = new HttpProvider(config, session, transport);
            var polling = new SyncPollingService(provider, config, session);

            var changes = new List<SyncStatusDto>();
            polling.Changed += status => changes.Add(status);

            polling.Start();
            Thread.Sleep(200);
            polling.Stop();

            // First poll always fires Changed (_lastKnown == null -> rev_1 is a change).
            // Second poll with same revision rev_1 -> rev_1 does NOT fire Changed.
            Assert.That(changes.Count, Is.EqualTo(1));
            Assert.That(changes[0].StudentProgressRevision, Is.EqualTo("rev_1"));
        }

        [Test]
        public void SyncPollingService_Stop_DoesNotThrow()
        {
            var config = new HttpProviderConfig { BaseUrl = "https://test.example" };
            var session = new AuthSessionState { Token = "token" };
            var provider = new HttpProvider(config, session, new FakeHttpTransport());
            var polling = new SyncPollingService(provider, config, session);

            polling.Start();
            polling.Stop();
            polling.Dispose();

            Assert.Pass();
        }

        [Test]
        public void SyncPollingService_StartStopStart_WorksWithoutError()
        {
            // Verify that Stop() then Start() creates a fresh cancellation scope.
            var transport = new FakeHttpTransport();
            transport.EnqueueSuccess(200, "{\"student_progress_revision\":\"rev_1\"}");
            transport.EnqueueSuccess(200, "{\"student_progress_revision\":\"rev_2\"}");

            var config = new HttpProviderConfig { BaseUrl = "https://test.example", DefaultPollIntervalSeconds = 0 };
            var session = new AuthSessionState { Token = "token" };
            var provider = new HttpProvider(config, session, transport);
            var polling = new SyncPollingService(provider, config, session);

            // First start-stop cycle.
            polling.Start();
            Thread.Sleep(100);
            polling.Stop();

            // Second start should create a new CTS and resume polling.
            transport.EnqueueSuccess(200, "{\"student_progress_revision\":\"rev_3\"}");

            var changes = new List<SyncStatusDto>();
            polling.Changed += status => changes.Add(status);

            polling.Start();
            Thread.Sleep(100);
            polling.Stop();

            Assert.That(changes.Count, Is.GreaterThanOrEqualTo(1),
                "After restart the Changed event should fire when revision changes");
            Assert.That(changes[0].StudentProgressRevision, Is.EqualTo("rev_3"));

            polling.Dispose();
        }
    }
}
