using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Corrida
{
    public class Damager : MonoBehaviour
    {
        private void DamagePlayer()
        {
            if (PlayerCorrida.Instance.State == PlayerState.Running && !PlayerCorrida.Instance.IsInvincible)
            {
                PlayerCorrida.Instance.HurtPlayer();
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player")) 
            { 
                DamagePlayer();
            }
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player")) 
            {
                DamagePlayer();
            }
        }
    }
}
