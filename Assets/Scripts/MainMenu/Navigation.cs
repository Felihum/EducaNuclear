using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace EducaNuclear {
    public class Navigation : MonoBehaviour
    {
        public TransitionController transitionController;
        public GameObject configurationObj;

        public void OpenAlquimia()
        {
            if (SaveAndLoad.playerData.currentPhase >= 3)
            {
                transitionController.Transition("Alquimia1");
                //SceneManager.LoadScene("Alquimia1");
            }
        }

        public void OpenCorrida()
        {
            if (SaveAndLoad.playerData.currentPhase >= 4)
            {
                transitionController.Transition("TutorialCorrida");
                //SceneManager.LoadScene("Alquimia1");
            }
        }

        public void CloseTutorialCorrida()
        {
            transitionController.Transition("Factory");
        }

        public void OpenColeta()
        {
            if (SaveAndLoad.playerData.currentPhase >= 2)
            {
                transitionController.Transition("ColetaScene");
                //SceneManager.LoadScene("Alquimia1");
            }
        }

        public void OpenMainMenu()
        {
            transitionController.Transition("MainMenu");
            //SceneManager.LoadScene("MainMenu");
        }

        public void CloseGame()
        {
            Application.Quit();
        }

        public void OpenIntroScene()
        {
            transitionController.Transition("IntroScene");
            //SceneManager.LoadScene("IntroScene");
        }

        public void RestartGame()
        {
            transitionController.Transition(SceneManager.GetActiveScene().name);
        }

        public void NewGame()
        {
            PlayerData playerData = new(1, 0);
            PlayerPrefs.SetInt("TotalScore", 0);
            SaveAndLoad.SavePlayerData(playerData);
            transitionController.Transition("MainMenu");
        }

        public void OpenConfiguration()
        {
            configurationObj.SetActive(!configurationObj.activeSelf);
        }

        public void SetTimeScale()
        {
            Time.timeScale = 1;
        }
    }
}