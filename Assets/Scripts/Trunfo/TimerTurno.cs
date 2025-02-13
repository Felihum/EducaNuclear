using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Trunfo { 
    public class TimerTurno : MonoBehaviour
    {
        public float timeLeft;
        public float timeAux;
        public bool timerOn = false;
        public GameManager gameManager;

        public UIManager uiManager;
        public TMP_Text timerTxt;


        // Start is called before the first frame update
        void Start()
        {
            timerOn = true;
            timeAux = timeLeft;
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
                    {
                        timerOn = false;
                        uiManager.ShowResult("Inimigo venceu o turno!");
                        Debug.Log("Resultado: Inimigo venceu o turno.");
                        gameManager.TimerOver();
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

        public void ResetTimer(){
            timerOn = true;
            timeLeft = timeAux;
        }
    }
}


