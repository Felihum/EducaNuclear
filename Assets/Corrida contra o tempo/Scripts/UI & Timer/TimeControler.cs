using UnityEngine;
using TMPro;
using System;
using System.Threading;

namespace Corrida
{
    public class TimeController : MonoBehaviour
    {
        public float TotalTimeSpan { get { return timeCount; } set { timeCount = (value < 0f) ? 0f : value; } }
        private float timeCount = 0f;
        private string formatedMinsAndSecs, formatedMillisecs;
        public bool timerIsRunning = false;
        private readonly float interfaceMaxValue = 6000f;

        [SerializeField] private TMP_Text time_run;
        [SerializeField] private TMP_Text time_runMilliseconds;

        public static TimeController Instance { get; private set; }

        public void StartTimer()
        {
            timeCount = 0f;
            timerIsRunning = true;
           
        }

        private void FormatTime(float time)
        {
            TimeSpan span = TimeSpan.FromSeconds(timeCount);
            float minutes = span.Minutes;
            float seconds = span.Seconds;
            float milliseconds = span.Milliseconds;

            formatedMinsAndSecs = string.Format("{0:00}:{1:00}", minutes, seconds);
            formatedMillisecs = string.Format(",{0:000}", milliseconds);
        }

        void UpdateTimer()
        {
            FormatTime(timeCount);

            time_run.text = formatedMinsAndSecs;
            time_runMilliseconds.text = formatedMillisecs;
        }

        private void MaxoutTimer()
        {
            time_run.text = "99:59";
            time_runMilliseconds.text = ",999";
        }

        public void StopTimer()
        {
            timerIsRunning = false;
        }

        /*private void OnGoalReached()
        {

        }*/

        private void Awake()
        {
            if (Instance != this && Instance != null)
            {
                Destroy(gameObject);
                return;
            }
        }

        private void OnEnable()
        {
            if (Instance == null)
                Instance = this;

            GameManager.GameplaySet += StartTimer;

        }

        private void Start()
        {
            
        }

        private void Update()
        {
            if (timerIsRunning)
            {
                if (timeCount < interfaceMaxValue)
                {
                    timeCount += Time.deltaTime;
                    UpdateTimer();
                }
                else
                {
                    timeCount = interfaceMaxValue - 0.0001f;
                    MaxoutTimer();
                    timerIsRunning = false;
                }
            }
        }

        private void OnDisable()
        {
            if (Instance == this)
                Instance = null;

            GameManager.GameplaySet -= StartTimer;
        }
    }
}
