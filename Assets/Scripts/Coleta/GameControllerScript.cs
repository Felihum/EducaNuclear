using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using EducaNuclear;

namespace Coleta
{
    public class GameControllerScript : MonoBehaviour
    {
        public GameObject player;
        public GameObject panel;
        public GameObject endGame;
        public GameObject pauseObj;
        public TMP_Text endGameTxt;
        public Navigation navigationRef;

        public GameObject timeIsUpPanel;
        public TMP_Text pointsTxt;
        public TMP_Text totalGrabbedItemsTXT;
        public TMP_Text timeIsUpTXT;

        public GameObject box;

        public TimerScript timerController;

        public int totalGrabbedItems;
        public GameObject grabbedItem;

        private Question question;
        private int score;

        [SerializeField] private SquareCollision[] squares;
        [SerializeField] private List<Question> questionsList;
        [SerializeField] private int totalItemsToGrab;

        // Start is called before the first frame update
        void Start()
        {
            squares = FindObjectsOfType<SquareCollision>();
            FetchQuestions();
            totalGrabbedItemsTXT.text = "Itens: " + totalGrabbedItems.ToString() + "/" + totalItemsToGrab.ToString();
        }

        //This method is used when u need to call this script and set the question using another script
        public void SetQuestion(Question question)
        {
            this.question = question;
        }

        private void FetchQuestions()
        {
            int i = 0;
            while (i < squares.Length)
            {
                Question questionToPutInGame = questionsList[Random.Range(0, questionsList.Count - 1)];
                squares[i].question = questionToPutInGame;
                questionsList.Remove(questionToPutInGame);
                i++;
            }
        }

        //This method is called when u click the button to answer a question;
        public void OnOptionChoose(TMP_Text opt)
        {
            panel.SetActive(false);

            if (question.answer == opt.text)
            {
                totalGrabbedItems++;
                timerController.GetComponent<TimerScript>().AddTime();
                totalGrabbedItemsTXT.text = "Itens: " + totalGrabbedItems.ToString() + "/" + totalItemsToGrab.ToString();
                grabbedItem.GetComponent<SquareCollision>().miniMapIcon.SetActive(false);
                //open the item chest
                Destroy(grabbedItem.gameObject);
                if (grabbedItem.CompareTag("Key"))
                {
                    player.gameObject.GetComponent<PlayerMovement>().haveKey = true;
                }
            }
            else
            {
                timerController.GetComponent<TimerScript>().ReduceTime();
            }

            if (totalGrabbedItems == totalItemsToGrab)
            {
                timerController.GetComponent<TimerScript>().timerOn = false;
                player.GetComponent<PlayerMovement>().enabled = false;

                float endGameTime = timerController.timeLeft;
                
                if (endGameTime >= 90)
                {
                    score = 9000;
                }
                else if (endGameTime < 90 && endGameTime >= 60)
                {
                    score = 6000;
                }
                else if (endGameTime < 60 && endGameTime >= 30)
                {
                    score = 3000;

                }
                else if (endGameTime < 30)
                {
                    score = 1000;
                }

                ScoreManager.UpdateBestColetaScore(score);

                endGameTxt.text = "Parabéns\nPontos ganhos: " + score.ToString();

                endGame.SetActive(true);

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
        }

        public void RestartGame()
        {
            navigationRef.RestartGame();
        }

        public void Pause()
        {
            timerController.GetComponent<TimerScript>().timerOn = !timerController.GetComponent<TimerScript>().timerOn;
            pauseObj.SetActive(!pauseObj.activeSelf);
        }

        public int GetScore()
        {
            return score;
        }
    }
}


