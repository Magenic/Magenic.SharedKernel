using System;
using System.Diagnostics;

using Humanizer;

namespace Magenic.SharedKernel
{
    /// <summary>
    /// Simple Stopwatch wrapper useful to measure time elapsed during execution of a code block.
    /// </summary>
    /// <remarks>This class is often used with logging.</remarks>
    public class CodeTimer : DisposableObject
    {
        #region Fields

        private readonly Stopwatch _timer;

        #endregion

        #region Creation

        /// <summary>
        /// Creates a new code timer for the given step/code/task.
        /// </summary>
        /// <param name="stepName">Concise and descriptive name of what's being timed.</param>
        public CodeTimer(string stepName)
        {
            StepName = stepName;
            _timer = Stopwatch.StartNew();
        }

        /// <summary>
        /// Creates a new code timer for the given step/code/task.
        /// </summary>
        /// <param name="stepName">Short, descriptive name of what's being timed.</param>
        public static CodeTimer Step(string stepName)
        {
            return new CodeTimer(stepName);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Returns timespan of elapsed time so far or if complete, total time it took.
        /// </summary>
        public TimeSpan Elapsed => _timer.Elapsed;

        /// <summary>
        /// Returns humanized string version of elapsed time i.e. "2 seconds".
        /// </summary>
        public string ElapsedText => _timer.Elapsed.Humanize();

        /// <summary>
        /// Returns step name and formatted text version of code time
        /// </summary>
        public string Message => _timer.IsRunning
            ? $"{StepName} running for {ElapsedText}."
            : $"{StepName} completed in {ElapsedText}.";

        /// <summary>
        /// Returns name for what's being timed.
        /// </summary>
        public string StepName { get; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Stops the timer if running and returns instance of class for additional 
        /// properties. Disposing this class stops the timer as well if not already stopped.
        /// </summary>
        /// <returns>Class instance.</returns>
        public CodeTimer Stop()
        {
            if (_timer.IsRunning)
            {
                _timer.Stop();
            }

            return this;
        }

        /// <summary>
        /// Returns step name and timing information; equivalent to Message property.
        /// </summary>
        /// <returns>String representing this code time.</returns>
        public override string ToString()
        {
            return Message;
        }

        #endregion

        #region Protected Methods

        protected override void DisposeManagedResources()
        {
            Stop();
        }

        #endregion 
    }
}
