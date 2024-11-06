using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace EducaNuclear
{
    public class TransitionController : MonoBehaviour
    {
        public Animator transitionAnim;

        public void Transition(string sceneName)
        {
            StartCoroutine(LoadScene(sceneName));
        }

        IEnumerator LoadScene(string sceneName)
        {
            transitionAnim.SetTrigger("Start");

            yield return new WaitForSeconds(0.5f);

            SceneManager.LoadScene(sceneName);
        }
    }
}