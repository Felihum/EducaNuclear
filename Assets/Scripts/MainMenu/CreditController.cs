using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditController : MonoBehaviour
{
    [SerializeField] private GameObject creditObj;
    [SerializeField] private GameObject menuInicial;

    public void OpenCredits()
    {
        menuInicial.SetActive(false);
        creditObj.SetActive(true);
    }

    public void CloseCredits()
    {
        menuInicial.SetActive(true);
        creditObj.SetActive(false);
    }
}
