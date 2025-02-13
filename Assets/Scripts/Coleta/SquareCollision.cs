using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Coleta
{
    public class SquareCollision : MonoBehaviour
    {
        public GameObject floor;
        public GameObject player;
        public int grabbedItems;
        public GameObject panel;
        public GameObject gameController;
        private int totalGrabbedItems;
        public Question question;
        public GameObject miniMapIcon;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "Player")
            {
                gameController.GetComponent<GameControllerScript>().grabbedItem = this.gameObject;
                gameController.GetComponent<GameControllerScript>().SetQuestion(question);

                //showing question
                panel.transform.GetChild(0).GetComponent<TMP_Text>().text = question.question;

                //showing options
                panel.transform.GetChild(1).transform.GetChild(0).GetComponent<TMP_Text>().text = question.options[0];
                panel.transform.GetChild(2).transform.GetChild(0).GetComponent<TMP_Text>().text = question.options[1];
                panel.transform.GetChild(3).transform.GetChild(0).GetComponent<TMP_Text>().text = question.options[2];
                panel.transform.GetChild(4).transform.GetChild(0).GetComponent<TMP_Text>().text = question.options[3];

                grabbedItems++;
                totalGrabbedItems++;
                PlayerPrefs.SetInt("grabbedItems", totalGrabbedItems);
                panel.SetActive(true);
            }
        }

    }

}
