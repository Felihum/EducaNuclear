using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Corrida
{
    public class PauseControler : MonoBehaviour
    {
        public static bool IsPaused;
        private float _resumeTimeScale; // Escala de tempo antes do jogo ser pausado

        [SerializeField] private Transform _pauseMenu;

        private void Awake()
        {
            IsPaused = false;
        }

        private void Update()
        {
            #if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (IsPaused)
                {
                    ResumeGame();
                }
                else
                {
                    PauseGame();
                }
            }
            #endif
        }

        public void PauseGame()
        {
            if (GameManager.Instance != null)
            {
                if (GameManager.Instance.GameState == GameState.Finish)
                    return;
            }

            _resumeTimeScale = Time.timeScale;

            _pauseMenu.gameObject.SetActive(true);
            if (UIManager.Instance != null) { UIManager.Instance.TogglePauseButton(false); }
            Time.timeScale = 0;
            IsPaused = true;
        }

        public void ResumeGame()
        {
            if (GameManager.Instance != null)
            {
                if (GameManager.Instance.GameState == GameState.Finish)
                    return;
            }

            _pauseMenu.gameObject.SetActive(false);
            if (UIManager.Instance != null) { UIManager.Instance.TogglePauseButton(true); }
            Time.timeScale = _resumeTimeScale;
            IsPaused = false;
        }
    }
}
