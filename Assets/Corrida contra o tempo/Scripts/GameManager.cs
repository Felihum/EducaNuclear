using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Corrida
{
    // Classe para manipular mecânicas gerais do jogo
    public class GameManager : MonoBehaviour
    {
        public GameState GameState { get; private set; }
        public static event Action CountdownSet;
        public static event Action GameplaySet;
        public static event Action FinishSet;

        [Header("Countdown")]
        [SerializeField][Range(1,10)] private int _countdownStartNumber = 3;
        [SerializeField][Range(0.3f,3f)] private float _delayBetweenCounts = 1f;
        [SerializeField][Range(0.3f,3f)] private float _delayCountdownEnd = 1f;
        [SerializeField] private string _goText = "VÁ!";
        private float _countdownTimer = 0f;
        private Coroutine _countdownCoroutine;
        public event Action CountdownOver;

        // Pickups
        private List<Coroutine> _timers = new();
        private List<bool> _activePickups = new();

        private event Action _tempEffectRemoved;
        private event Action<float> _pickupReset;

        public static GameManager Instance { get; private set; }

        #region GameState
        private void SetState(GameState state)
        {
            GameState = state;

            switch (state)
            {
                case GameState.Countdown:
                    CountdownSet?.Invoke();
                    break;

                case GameState.Gameplay:
                    GameplaySet?.Invoke(); 
                    break;

                case GameState.Finish:
                    FinishSet?.Invoke();
                    break;

                default:
                    break;
            }
        }
        #region Countdown
        private void StartCountdown()
        {
            if (_countdownCoroutine != null)
            {
                StopCoroutine(_countdownCoroutine);
                _countdownCoroutine = null;
            }

            if (GameState == GameState.Countdown)
            {
                _countdownCoroutine = StartCoroutine(CountdownRoutine());
            }
        }

        // Lógica do Countdown
        private IEnumerator CountdownRoutine()
        {
            _countdownTimer = 0f;
            int count = _countdownStartNumber;
            bool isCountingDown = true;
            bool nextCountIsQueued = true;

            while (isCountingDown)
            {
                if (count > 0)
                {
                    if (nextCountIsQueued)
                    {
                        UIManager.Instance.SetCountdownText(count.ToString());
                        nextCountIsQueued = false;
                    }
                    else if (_countdownTimer < _delayBetweenCounts)
                    {
                        yield return null;
                        _countdownTimer += Time.deltaTime;
                    }
                    else
                    {
                        _countdownTimer = 0f;
                        count--;
                        nextCountIsQueued = true;
                    }
                }
                else
                {
                    if (nextCountIsQueued)
                    {
                        UIManager.Instance.SetCountdownText(_goText);
                        nextCountIsQueued = false;
                        SetState(GameState.Gameplay);
                        
                    }
                    else
                    {
                        if (_countdownTimer < _delayCountdownEnd)
                        {
                            yield return null;
                            _countdownTimer += Time.deltaTime;
                        }
                        else
                        {
                            _countdownTimer = 0f;
                            UIManager.Instance.SetCountdownAnimHidden();
                            isCountingDown = false;
                        }
                    }
                }
            }

            CountdownOver?.Invoke();

            _countdownCoroutine = null;
            yield break;
        }
        #endregion
        private void OnGoalReached()
        {
            SetState(GameState.Finish);
        }

        private void OnGoalAnimationEnd()
        {
            FinishLevel();
        }

        private void FinishLevel()
        {
            if (GameState != GameState.Finish)
                SetState(GameState.Finish);

            if (UIManager.Instance != null)
                UIManager.Instance.OnLevelFinish();
        }

        #endregion
        #region Pickups
        // Método para ativar um efeito temporário
        // Chamado por Pickups. Lida com seus seus efeitos (aplica, remove, reseta) e inicia seu timer.
        public void AddEffect(float timerDuration, PickupType type, Action ApplyEffect, Action OnTimerEnd)
        {
            if (_activePickups[(int)type]) // Se o tipo de Pickup está ativo na lista activePickups (exemplo: PickupType.Speed)
            {
                _pickupReset?.Invoke(timerDuration);
            }
            else
            {
                ApplyEffect?.Invoke();
                _activePickups[(int)type] = true;

                var timer = StartCoroutine(EffectTimer(timerDuration, OnTimerEnd, (int)type, _timers.Count));
                _timers.Add(timer);
            }
        }

        // Timer dos efeitos de Pickups.
        private IEnumerator EffectTimer(float duration, Action OnTimerEnd, int type, int id)
        {
            void AdjustId() { id--; }
            _tempEffectRemoved += AdjustId;

            void ResetDuration(float newDuration) { duration = newDuration; }
            _pickupReset += ResetDuration;

            while (duration > 0)
            {
                yield return null;
                duration -= Time.deltaTime;
            }

            OnTimerEnd?.Invoke();

            _timers[id] = null;
            _timers.RemoveAt(id);
            _activePickups[type] = false;

            _tempEffectRemoved?.Invoke();
            _tempEffectRemoved -= AdjustId;
            _pickupReset -= ResetDuration;
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

            // Popular a lista de Pickups ativos com todos os tipos de Pickups em PickupType
            foreach (var type in Enum.GetValues(typeof(PickupType)))
            {
                _activePickups.Add(false);
            }
        }

        private void OnEnable()
        {
            if (Instance == null)
                Instance = this;

            Goal.GoalReached += OnGoalReached;
            Goal.GoalAnimationEnd += OnGoalAnimationEnd;
        }

        private void Start()
        {
            SetState(GameState.Countdown);
            StartCountdown();

            if (SceneManager.GetActiveScene().name == "Factory")
            {
                ScoreManager.UpdateCurrentCorridaTotalScore(-1);
            }
        }

        #if UNITY_EDITOR
        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Keypad0))
            {
                Application.targetFrameRate = -1;
            }
            if (Input.GetKeyDown(KeyCode.Keypad1))
            {
                Application.targetFrameRate = 5;
            }
            if (Input.GetKeyDown(KeyCode.Keypad2))
            {
                Application.targetFrameRate = 15;
            }
            if (Input.GetKeyDown(KeyCode.Keypad3))
            {
                Application.targetFrameRate = 30;
            }
            if (Input.GetKeyDown(KeyCode.Keypad4))
            {
                Application.targetFrameRate = 60;
            }
            if (Input.GetKeyDown(KeyCode.Keypad5))
            {
                Application.targetFrameRate = 120;
            }
            if (Input.GetKeyDown(KeyCode.Keypad6))
            {
                Application.targetFrameRate = 300;
            }
        }
        #endif

        private void OnDisable()
        {
            if (Instance == this)
                Instance = null;

            Goal.GoalReached -= OnGoalReached;
            Goal.GoalAnimationEnd -= OnGoalAnimationEnd;
        }
        #endregion
    }
}

public enum GameState
{
    Countdown,
    Gameplay,
    Finish
}
