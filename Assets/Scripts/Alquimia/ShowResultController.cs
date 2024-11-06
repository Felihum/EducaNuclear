using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Alquimia
{
    public class ShowResultController : MonoBehaviour
    {
        [SerializeField] private GameObject timeText;
        [SerializeField] private GameObject triesText;

        private void Start()
        {
            timeText.transform.localScale = Vector2.zero;
            triesText.transform.localScale = Vector2.zero;
            StartCoroutine("ShowResults");
        }
        public IEnumerator ShowResults()
        {
            yield return new WaitForSeconds(1f);
            timeText.transform.LeanScale(Vector2.one, 0.5f);
            StartCoroutine("ShowTries");
        }

        private IEnumerator ShowTries()
        {
            yield return new WaitForSeconds(1f);
            triesText.transform.LeanScale(Vector2.one, 0.5f);
        }
    }
}
