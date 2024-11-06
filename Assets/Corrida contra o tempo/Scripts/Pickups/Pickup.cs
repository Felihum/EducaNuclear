using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Corrida
{
    // Superclasse para itens coletáveis durante o jogo
    // Não atribua esse script à objetos, crie uma Subclasse desse script
    public class Pickup : MonoBehaviour
    {
        public virtual void OnPickup()
        {
            // Elaborar em subclasses
            // Chamar base.OnPickup() no fim do override

            Destroy(gameObject);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player") && PlayerCorrida.Instance.State == PlayerState.Running)
            {
                OnPickup();
            }
        }

        protected virtual void Awake()
        {
            gameObject.layer = 8/*Pickups*/;
        }
    }


}
