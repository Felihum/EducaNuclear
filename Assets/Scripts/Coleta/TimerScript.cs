using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SocialPlatforms.Impl;
using EducaNuclear;

namespace Coleta
{
    public class TimerScript : MonoBehaviour
    {
        public float timeLeft;
        public bool timerOn = false;
        public GameObject player;
        public GameObject timeIsUpPanel;
        public GameObject gameController;
        public GameObject endGame;
        public TMP_Text endGameTxt;

        private int totalGrabbedItems;
        private float points;

        public TMP_Text timerTxt;
        public TMP_Text pointsTxt;
        // Start is called before the first frame update
        void Start()
        {
            timerOn = false;
        }

        // Update is called once per frame
        void Update()
        {
            if (timerOn)
            {//execute while the time is running
                if (timeLeft > 0)
                {
                    timeLeft -= Time.deltaTime;
                    UpdateTimer(timeLeft);
                }
                else
                {//execute when the time has finished
                 //player.GetComponent<playerMovement>().enabled = false;
                    
                    totalGrabbedItems = gameController.GetComponent<GameControllerScript>().totalGrabbedItems;
                    Debug.Log("Time is UP!");
                    timeLeft = 0;

                    points = (totalGrabbedItems * 1000) * 0.1f;


                    if (totalGrabbedItems < 7)
                    {
                        timeIsUpPanel.SetActive(true);
                        pointsTxt.text = "Pontos: " + points + "\nPontuação minima não atingida!";
                    }
                    else
                    {
                        endGame.SetActive(true);
                        ScoreManager.UpdateBestColetaScore(1000);
                        endGameTxt.text = "Parabéns\nPontos ganhos: " + 1000;

                        PlayerData playerData;

                        if (SaveAndLoad.playerData.currentPhase <= 2)
                        {
                            playerData = new PlayerData(currentPhase: 3);
                        }
                        else
                        {
                            playerData = new PlayerData(currentPhase: SaveAndLoad.playerData.currentPhase);
                        }

                        SaveAndLoad.SavePlayerData(playerData);
                    }

                    timerOn = false;
                }
            }
        }

        public void UpdateTimer(float currentTime)
        {
            currentTime += 1;
            float minutes = Mathf.FloorToInt(currentTime / 60);
            float seconds = Mathf.FloorToInt(currentTime % 60);

            timerTxt.text = string.Format("{0:00}:{1:00}", minutes, seconds);
            PlayerPrefs.SetFloat("Time", timeLeft);
        }

        //this method is used when the player get a right answer -> receive more time;
        public void AddTime()
        {
            timeLeft += 10;
        }

        //this method is used when the player get a wrong answer -> lose time;
        public void ReduceTime()
        {
            timeLeft -= 10;
        }

    }
}

