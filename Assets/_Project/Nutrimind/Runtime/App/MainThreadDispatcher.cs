using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace NutriMind.Runtime.App
{
    /// <summary>
    /// Injectable pure-C# dispatcher that queues <see cref="Action"/> callbacks
    /// on a virtual "main thread" and executes them on demand via
    /// <see cref="ExecutePending"/> or <see cref="Flush"/>.
    ///
    /// Thread-safe: <see cref="Post(Action)"/> and <see cref="Post(int, Action)"/>
    /// may be called from any background thread.  <see cref="ExecutePending"/> and
    /// <see cref="Flush"/> should be called from the main thread only.
    ///
    /// Supports <see cref="Invalidate"/> to discard queued callbacks and
    /// increment a generation counter so stale completions from previous
    /// lifecycle phases can be suppressed via the generation-bound overload.
    /// </summary>
    public class MainThreadDispatcher
    {
        private readonly ConcurrentQueue<Action> _pending = new();
        private volatile int _generation;
        private readonly object _flushLock = new();

        /// <summary>
        /// Monotonically increasing generation counter.
        /// Incremented each time <see cref="Invalidate"/> is called.
        /// </summary>
        public int Generation => _generation;

        /// <summary>
        /// Queues a callback to execute on the main thread.
        /// Thread-safe: may be called from any thread.
        /// </summary>
        /// <param name="callback">The action to execute later.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="callback"/> is null.</exception>
        public void Post(Action callback)
        {
            if (callback == null) throw new ArgumentNullException(nameof(callback));
            _pending.Enqueue(callback);
        }

        /// <summary>
        /// Queues a callback to execute on the main thread, tagged with the
        /// expected generation.  If the generation has been incremented (via
        /// <see cref="Invalidate"/>) before the callback executes, the callback
        /// is silently dropped.
        /// Thread-safe: may be called from any thread.
        /// </summary>
        /// <param name="expectedGeneration">
        /// The generation that must match the current generation at flush time
        /// for the callback to execute.  Obtain from <see cref="Generation"/>.
        /// </param>
        /// <param name="callback">The action to execute later.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="callback"/> is null.</exception>
        public void Post(int expectedGeneration, Action callback)
        {
            if (callback == null) throw new ArgumentNullException(nameof(callback));
            _pending.Enqueue(() =>
            {
                if (_generation == expectedGeneration)
                {
                    callback();
                }
            });
        }

        /// <summary>
        /// Executes all currently queued callbacks in FIFO order.
        /// Should be called from the main thread only.
        /// </summary>
        public void ExecutePending()
        {
            // Dequeue and execute all pending callbacks under a flush lock
            // to prevent concurrent flushes from interleaving.
            var batch = new List<Action>();
            lock (_flushLock)
            {
                while (_pending.TryDequeue(out var action))
                {
                    batch.Add(action);
                }
            }

            // Execute outside lock to avoid deadlocks if a callback calls Post.
            foreach (var action in batch)
            {
                action();
            }
        }

        /// <summary>
        /// Synonym for <see cref="ExecutePending"/>.
        /// </summary>
        public void Flush()
        {
            ExecutePending();
        }

        /// <summary>
        /// Discards all pending callbacks and increments the generation counter.
        /// Use when the current lifecycle phase is no longer valid (e.g. after
        /// a pause/resume or scene transition) so that late callbacks are
        /// silently dropped rather than executed.
        /// </summary>
        public void Invalidate()
        {
            lock (_flushLock)
            {
                while (_pending.TryDequeue(out _)) { }
                _generation++;
            }
        }
    }
}
