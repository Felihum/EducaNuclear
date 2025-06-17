using EducaNuclear;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Corrida
{
    public class Goal : MonoBehaviour
    {
        [SerializeField] private string _goalReachedParamName = "GoalReached";
        [SerializeField] private GameObject _goalCamera;
        private BoxCollider2D _boxCollider2D;
        private Animator _animator;

        public static event Action GoalReached;
        public static event Action GoalAnimationEnd;


        

        private void OnGoalReached()
        {

            int score = 0;

            _goalCamera.SetActive(true);
            _boxCollider2D.enabled = false;

            _animator.SetTrigger(_goalReachedParamName);

            if (TimeController.Instance != null)
            {
                TimeController.Instance.StopTimer();

                if (TimeController.Instance.TotalTimeSpan <= 76)
                {
                    score = 9000;
                }
                if (TimeController.Instance.TotalTimeSpan > 76 && TimeController.Instance.TotalTimeSpan < 85)
                {
                    score = 5000;
                }
                if (TimeController.Instance.TotalTimeSpan > 85)
                {
                    score = 1000;
                }

                if (SceneManager.GetActiveScene().name == "Forest")
                {
                    ScoreManager.UpdateCurrentCorridaTotalScore(score);
                    ScoreManager.UpdateBestCorridaScore(ScoreManager.GetCurrentCorridaTotalScore());

                    Debug.Log("Finalizou a corrida: " + ScoreManager.GetBestCorridaScore() + " ---- " + ScoreManager.GetCurrentCorridaTotalScore());

                    SaveAndLoad.SavePlayerData(new PlayerData(currentPhase: 4));
                }
                else
                {
                    Debug.Log("Score Corrida: " + score);

                    ScoreManager.UpdateCurrentCorridaTotalScore(score);
                }
            }

            GoalReached?.Invoke();
        }

        private void OnAnimationEnd()
        {
            GoalAnimationEnd?.Invoke();
        }

        private void Awake()
        {
            if (_goalCamera != null && _goalCamera.activeInHierarchy)
                _goalCamera.SetActive(false);

            _boxCollider2D = GetComponent<BoxCollider2D>();
            _animator = GetComponent<Animator>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                OnAnimationEnd();
                OnGoalReached();
               
            }
        }
    }
}
