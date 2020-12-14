using System.Collections;
using UnityEngine;
using SkirmishWars.UnityRenderers;

namespace SkirmishWars.Tests
{
    /// <summary>
    /// Implements tests for a unity driven timer.
    /// </summary>
    public sealed class TimerTest : MonoBehaviour
    {
        #region Inspector Fields
        [Tooltip("The timer renderer that observes the underlying timer.")]
        [SerializeField] private TimerRenderer timerRenderer = null;
        [Tooltip("The testing behaviour to run.")]
        [SerializeField] private TestType test = TestType.TimerEvents;
        private enum TestType : byte
        {
            TimerEvents,
            TimerPause,
            TimeScalePause,
            TimeScaleSlowMotion
        }
        #endregion

        private void Start()
        {
            Timer timer = new Timer();
            timerRenderer.DrivingTimer = timer;

            switch (test)
            {
                case TestType.TimerEvents: TestTimerEvents(); break;
                case TestType.TimerPause: StartCoroutine(TestTimerPause()); break;
                case TestType.TimeScalePause: StartCoroutine(TestTimeScalePause()); break;
                case TestType.TimeScaleSlowMotion: TestSlowMotion(); break;
            }
            void TestTimerEvents()
            {
                timer.Duration = 5f;
                timer.Started += () => { Debug.Log($"Timer Started {Time.time}"); };
                timer.Elapsed += () => { Debug.Log($"Timer Ended {Time.time}"); };
                timer.Begin();
            }
            IEnumerator TestTimerPause()
            {
                timer.Duration = 10f;
                timer.Started += () => { Debug.Log($"Timer Started {Time.time}"); };
                timer.Elapsed += () => { Debug.Log($"Timer Ended {Time.time}"); };
                timer.Begin();
                yield return new WaitForSeconds(3f);
                timer.Pause();
                Debug.Log($"Timer Paused {Time.time}");
                yield return new WaitForSeconds(3f);
                timer.Resume();
                Debug.Log($"Timer Resumed {Time.time}");
            }
            IEnumerator TestTimeScalePause()
            {
                timer.Duration = 10f;
                timer.Started += () => { Debug.Log($"Timer Started {Time.time}"); };
                timer.Elapsed += () => { Debug.Log($"Timer Ended {Time.time}"); };
                timer.Begin();
                yield return new WaitForSeconds(3f);
                Time.timeScale = 0f;
                Debug.Log($"TimeScale Paused {Time.time}");
                yield return new WaitForSecondsRealtime(3f);
                Time.timeScale = 1f;
                Debug.Log($"TimeScale Resumed {Time.time}");
            }
            void TestSlowMotion()
            {
                Debug.Log($"TimeScale 50%");
                Time.timeScale = 0.5f;
                timer.Duration = 10f;
                timer.Started += () => { Debug.Log($"Timer Started {Time.time}"); };
                timer.Elapsed += () => { Debug.Log($"Timer Ended {Time.time}"); };
                timer.Begin();
            }
        }
    }
}
