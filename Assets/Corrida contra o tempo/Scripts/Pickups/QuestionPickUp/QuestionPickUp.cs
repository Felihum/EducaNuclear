using System.Collections;
using System.Collections.Generic;
using Trunfo;
using UnityEngine;

namespace Corrida
{
    public class QuestionPickUp : Pickup {

        [SerializeField] private GameObject telaDePergunta;
        public QuizManager quizManager;


        public void OpenQuestion()
        {
            Time.timeScale = 0f;
            telaDePergunta.SetActive(true);
            quizManager.GerarPergunta();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                OpenQuestion();
                GameObject.Destroy(gameObject);
            }
        }
    }
}
