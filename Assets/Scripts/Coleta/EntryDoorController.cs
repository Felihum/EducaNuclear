using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Coleta
{
    public class EntryDoorController : MonoBehaviour
    {
        //[SerializeField] private GameObject Spawner;
        [SerializeField] private GameObject ObjAberto;
        [SerializeField] private GameObject ObjFechado;
        [SerializeField] private SpriteRenderer ObjPorta;
        [SerializeField] private Sprite SpriteAberto;
        [SerializeField] private Sprite SpriteFechado;

        public bool isOpened;

        private void Update()
        {
            if (isOpened)
            {
                ObjPorta.sprite = SpriteAberto;
                ObjAberto.SetActive(true);
                ObjFechado.SetActive(false);
            }
            else
            {
                ObjPorta.sprite = SpriteFechado;
                ObjFechado.SetActive(true);
                ObjAberto.SetActive(false);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                if (other.gameObject.GetComponent<PlayerMovement>().haveKey)
                {
                    isOpened = true;
                }

                //other.gameObject.transform.position = Spawner.transform.position;
            }
            Debug.Log("Entrou");
        }
    }
}

