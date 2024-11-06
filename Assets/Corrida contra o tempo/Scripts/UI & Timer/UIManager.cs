using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Corrida
{
    public class UIManager : MonoBehaviour
    {
        [Header("Countdown")]
        [SerializeField] private string isHiddenParamName = "IsHidden";
        [SerializeField] private string isPlayingParamName = "IsPlaying";
        [SerializeField] private string isIdleParamName = "IsIdle";
        private int isHiddenID, isPlayingID, isIdleID;
        private Coroutine countdownAnimationCoroutine = null;

        [Header("FinishCanvas")]
        [SerializeField][Min(0f)] private float firstTextBatchDelay = 1f;
        [SerializeField][Min(0f)] private float timeTextDelay = 1f;
        [SerializeField][Min(0f)] private float hitTextDelay = 1f;
        [SerializeField][Min(0f)] private float proceedButtonDelay = 1f;
        private Coroutine finishCoroutine;

        [Header("Referências")]
        [SerializeField] private TextMeshProUGUI LivesCount;
        
        [SerializeField] private GameObject gameplayCanvas;
        [SerializeField] private Button pauseButton;
        [SerializeField] private GameObject countdownText;
        private TextMeshProUGUI countdownTMP;
        private Animator countdownAnimator;

        [SerializeField] private GameObject finishCanvas;
        [SerializeField] private TextMeshProUGUI finishTimeStartText;
        [SerializeField] private TextMeshProUGUI finishTimeValueText;
        [SerializeField] private TextMeshProUGUI finishObstaclesHitStartText;
        [SerializeField] private TextMeshProUGUI finishObstaclesHitValueText;
        [SerializeField] private GameObject proceedButton;


       
       


        public static UIManager Instance { get; private set; }

        public void UpdateLivesUI(int lives)
        {
            LivesCount.text = lives.ToString();
        }

        public void TogglePauseButton(bool interactable)
        {
            pauseButton.interactable = interactable;
        }

        #region Countdown
        public void SetCountdownText(string countText)
        {
            countdownTMP.text = countText;
            SetCountdownAnimPlaying();
        }

        private void SetCountdownAnimHashes()
        {
            isHiddenID = Animator.StringToHash(isHiddenParamName);
            isPlayingID = Animator.StringToHash(isPlayingParamName);
            isIdleID = Animator.StringToHash(isIdleParamName);
        }

        private void SetCountdownAnimPlaying()
        {
            if (countdownAnimationCoroutine != null)
            {
                StopCoroutine(countdownAnimationCoroutine);
                countdownAnimationCoroutine = null;
            }

            countdownAnimationCoroutine = StartCoroutine(PlayCountdownAnimation());
        }

        private IEnumerator PlayCountdownAnimation()
        {
            bool isPlaying = countdownAnimator.GetBool(isPlayingParamName);

            if (isPlaying) // Resetar a animação caso já esteja rolando
            {
                SetCountdownAnimHidden();
                yield return null;
            }

            isPlaying = true;

            bool isHidden = false;
            bool isIdle = false;

            SetCountdownState(isPlaying, isHidden, isIdle);

            countdownAnimationCoroutine = null;
            yield break;
        }

        private void SetCountdownAnimIdle()
        {
            bool isPlaying = false;
            bool isIdle = true;
            bool isHidden = false;

            SetCountdownState(isPlaying, isIdle, isHidden);
        }

        public void SetCountdownAnimHidden()
        {
            bool isPlaying = false;
            bool isIdle = false;
            bool isHidden = true;

            SetCountdownState(isPlaying, isIdle, isHidden);
        }

        private void SetCountdownState(bool isPlaying, bool isIdle, bool isHidden)
        {
            countdownAnimator.SetBool(isPlayingID, isPlaying);
            countdownAnimator.SetBool(isIdleID, isIdle);
            countdownAnimator.SetBool(isHiddenID, isHidden);
        }
        #endregion
        #region Finish
        private void OnGoalReached()
        {
            TogglePauseButton(false);
        }

        public void OnLevelFinish()
        {
            HideGameplayInterface();
            UpdateFinishScreenValues();
            DisplayFinishScreen();
        }

        public void HideGameplayInterface()
        {
            gameplayCanvas.SetActive(false);
        }

        public void UpdateFinishScreenValues()
        {
            var time = TimeController.Instance.TotalTimeSpan;
            string formatedTime = FormatTime(time);
            string hitCount = PlayerCorrida.Instance.hitCount.ToString();

            finishTimeValueText.text = formatedTime;
            finishObstaclesHitValueText.text = hitCount;
        }

        private string FormatTime(float time)
        {
            TimeSpan span = TimeSpan.FromSeconds(time);
            float minutes = span.Minutes;
            float seconds = span.Seconds;
            float milliseconds = span.Milliseconds;
            return string.Format("{0:00}:{1:00},{2:000}", minutes, seconds, milliseconds);
        }

        public void DisplayFinishScreen()
        {
            finishCanvas.SetActive(true);
            finishTimeStartText.enabled = false;
            finishTimeValueText.enabled = false;
            finishObstaclesHitStartText.enabled = false;
            finishObstaclesHitValueText.enabled = false;
            proceedButton.SetActive(false);

            if (finishCoroutine != null) 
                StopCoroutine(finishCoroutine);

            finishCoroutine = StartCoroutine(FinishScreenAnimationRoutine());
        }

        private IEnumerator FinishScreenAnimationRoutine()
        {
            WaitForSeconds startDelay = new(firstTextBatchDelay);
            WaitForSeconds timeDelay = new(timeTextDelay);
            WaitForSeconds hitDelay = new(hitTextDelay);
            WaitForSeconds proceedDelay = new(proceedButtonDelay);

            yield return startDelay;

            finishTimeStartText.enabled = true;
            finishObstaclesHitStartText.enabled = true;
            yield return timeDelay;

            finishTimeValueText.enabled = true;
            yield return hitDelay;

            finishObstaclesHitValueText.enabled = true;
            yield return proceedDelay;

            proceedButton.SetActive(true);
            finishCoroutine = null;
            yield break;
        }

        #endregion
        #region Runtime
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            SetCountdownAnimHashes();
        }

        private void OnEnable()
        {
            if (Instance == null)
                Instance = this;

            countdownTMP = countdownText.GetComponent<TextMeshProUGUI>();
            countdownAnimator = countdownText.GetComponent<Animator>();

            CountdownSignaler.CountdownAnimationEnd += SetCountdownAnimIdle;
            Goal.GoalReached += OnGoalReached;
        }

        private void Start()
        {
            gameplayCanvas.SetActive(true);
            finishCanvas.SetActive(false);
        }

        private void OnDisable()
        {
            if (Instance == this)
                Instance = null;

            CountdownSignaler.CountdownAnimationEnd -= SetCountdownAnimIdle;
            Goal.GoalReached -= OnGoalReached;
        }
        #endregion
    }
}
