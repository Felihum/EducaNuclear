using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace Corrida
{
    public class DerrotaManager : MonoBehaviour
    {
        public string telaDeJogo;
        public static TimeController timeController { get; set; }
       

        public void TentarNovamente()
        {
            SceneManager.LoadScene(telaDeJogo);
            TimeController.Instance.GetComponent<TimeController>().StartTimer();
            Time.timeScale = 1f;  
          
        }

        public void Sair()
        {
            Application.Quit();
        }
    }
}
