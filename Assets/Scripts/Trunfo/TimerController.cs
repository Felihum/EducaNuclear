using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


namespace Trunfo
{
    public class TimerController : MonoBehaviour
    {
        public float timeLeft;
        public bool timerOn = false;
        public GameObject endGame;
        public TMP_Text timerTxt;
        // Start is called before the first frame update
        void Start()
        {
            timerOn = true;
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