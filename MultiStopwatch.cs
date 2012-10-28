using System;
using System.Diagnostics;

namespace MultiStopWatchDemo
{
    /// <summary>
    /// Timing class that supports multiple start and stop calls and provides both total elapsed times and average alapsed times.
    /// </summary>
    public class MultiStopwatch
    {
        #region Data Fields and Constants

        /// <summary>
        /// The total elapsed time in completed start/stop calls, in raw Stopwatch ticks.
        /// </summary>
        private long _elapsed;

        /// <summary>
        /// The start time for the last Start call, in raw Stopwatch ticks.
        /// </summary>
        private long _startTimeStamp;

        /// <summary>
        /// Number of raw Stopwatch ticks per DateTime tick (100 nanoseconds).
        /// </summary>
        private static readonly double _stopWatchTicksPerDateTimeTick = Stopwatch.IsHighResolution
                                                                            ? 10000000.0/Stopwatch.Frequency
                                                                            : 1.0;

        /// <summary>
        /// Number of DateTime ticks per millisecond (10,000).
        /// </summary>
        private const long DATETIME_TICKS_PER_MILLISECOND = 10000L;

        #endregion

        #region Internal Helpers

        /// <summary>
        /// Return the number of Stopwatch ticks that have elapsed in the current running timer plus all previous timers.
        /// </summary>
        /// <returns>Number of Stopwatch ticks elapsed.</returns>
        private long GetRawElapsedTicks()
        {
            return IsRunning ? _elapsed + (Stopwatch.GetTimestamp() - _startTimeStamp) : _elapsed;
        }

        /// <summary>
        /// Return the number of DateTime ticks that have elapsed in the current running timer plus all previous timers.
        /// </summary>
        /// <returns>Number of DateTime ticks elapsed.</returns>
        private long GetElapsedDateTimeTicks()
        {
            return (long)(GetRawElapsedTicks() * _stopWatchTicksPerDateTimeTick);
        }

        #endregion

        /// <summary>
        /// Clears all elapsd time and the current running timer, if any, and starts a new timer.
        /// </summary>
        public void ResetAndStart()
        {
            Reset();
            Start();
        }

        /// <summary>
        /// Clears all elapsed time and clears the current running timer, if any.
        /// </summary>
        public void Reset()
        {
            Count = 0;
            _elapsed = 0L;
            IsRunning = false;
            _startTimeStamp = 0L;
        }

        /// <summary>
        /// Starts a new timer.
        /// </summary>
        /// <remarks>
        /// If a timer is already running, this method does nothing.
        /// </remarks>
        public void Start()
        {
            if (IsRunning)
            {
                return;
            }
            _startTimeStamp = Stopwatch.GetTimestamp();
            IsRunning = true;
            Count++;
        }

        /// <summary>
        /// Stops the running timer.
        /// </summary>
        /// <remarks>
        /// If no timer is running, this method does nothing.
        /// </remarks>
        public void Stop()
        {
            if (!IsRunning)
            {
                return;
            }
            _elapsed += Stopwatch.GetTimestamp() - _startTimeStamp;
            IsRunning = false;
        }

        /// <summary>
        /// Adds another stopwatch to this instance, increasing both the elapsed time and the count of runs.
        /// </summary>
        /// <param name="stopwatch">A running or ran Stopwatch to add to the current timer.</param>
        public void Add(Stopwatch stopwatch)
        {
            _elapsed += stopwatch.ElapsedTicks;
            Count++;
        }


        /// <summary>
        /// Adds another MultiStopwatch to this instance, increasing both the elapsed time and the count of runs, treating the added MultiStopwatch as a single instance.
        /// </summary>
        /// <param name="stopwatch">A running or ran MultiStopwatch to add to the current timer.</param>
        public void Add(MultiStopwatch stopwatch)
        {
            _elapsed += stopwatch.ElapsedTicks;
            Count++;
        }

        /// <summary>
        /// Adds time to the current MultiStopwatch, in Stopwatch ticks, and increases the number of instances.
        /// </summary>
        /// <param name="elapsed">Elapsed time to add in Stopwatch ticks.</param>
        public void Add(long elapsed)
        {
            _elapsed += elapsed;
            Count++;
        }

        /// <summary>
        /// Average elapsed time as TimeSpan.
        /// </summary>
        public TimeSpan Average
        {
            get
            {
                return new TimeSpan(GetElapsedDateTimeTicks() / Count);
            }
        }

        /// <summary>
        /// Average elapsed time in milliseconds.
        /// </summary>
        public long AverageMilliseconds
        {
            get
            {
                return Count == 0 ? 0 : (GetElapsedDateTimeTicks()/DATETIME_TICKS_PER_MILLISECOND)/Count;
            }
        }

        /// <summary>
        /// Average elapsed time in Stopwatch ticks.
        /// </summary>
        public long AverageTicks
        {
            get
            {
                return GetRawElapsedTicks() / Count;
            }
        }

        /// <summary>
        /// Number of timing runs.
        /// </summary>
        public int Count { get; private set; }

        /// <summary>
        /// Total elapsed time as TimeSpan.
        /// </summary>
        public TimeSpan Elapsed
        {
            get
            {
                return new TimeSpan(GetElapsedDateTimeTicks());
            }
        }

        /// <summary>
        /// Total elapsed time in milliseconds.
        /// </summary>
        public long ElapsedMilliseconds
        {
            get
            {
                return (GetElapsedDateTimeTicks() / DATETIME_TICKS_PER_MILLISECOND);
            }
        }

        /// <summary>
        /// Total elapsed time in Stopwatch ticks.
        /// </summary>
        public long ElapsedTicks
        {
            get
            {
                return GetRawElapsedTicks();
            }
        }

        /// <summary>
        /// Whether or not a timer is running.
        /// </summary>
        public bool IsRunning { get; private set; }
    }
}
