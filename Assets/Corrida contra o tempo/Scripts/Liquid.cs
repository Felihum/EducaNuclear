using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Corrida
{
    public class Liquid : MonoBehaviour
    {
        [Header("Parâmetros")]
        [SerializeField][Min(1f)] private float _speedDivider = 4f;
        private float AdjustedDivider => _speedDivider - 1f;
        [SerializeField][Min(0f)] private float _playerVelocityThreshold = 0.5f;

        public static bool SlowdownActive { get; private set; } = false;

        [Header("Referências")]
        [SerializeField] private GameObject _liquidParticleSystem;

        private ParticleSystem _particleSystem;
        private Transform _particleSystemTransform;

        private void ActivateSlowdown(Vector2 position)
        {
            if (PlayerCorrida.Instance != null)
                PlayerCorrida.Instance.MultiplierDecrease += AdjustedDivider;
            SlowdownActive = true;
            SetParticlePosition(position);
            _particleSystem.Play();
        }

        private void SetParticlePosition(Vector2 position)
        {
            _particleSystemTransform.position = position;
        }

        private void DeactivateSlowdown()
        {
            if (PlayerCorrida.Instance != null)
                PlayerCorrida.Instance.MultiplierDecrease -= AdjustedDivider;
            SlowdownActive = false;

            _particleSystem.Stop();
        }

        private void Start()
        {
            _particleSystemTransform = _liquidParticleSystem.transform;
            _particleSystem = _liquidParticleSystem.GetComponent<ParticleSystem>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player") && !SlowdownActive)
            {
                ActivateSlowdown(new Vector2(collision.transform.position.x, transform.position.y));
            }
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                if (PlayerCorrida.Instance != null)
                {
                    var playerVelocityX = Mathf.Abs(PlayerCorrida.Instance.VelocityRef.x);

                    if (playerVelocityX < _playerVelocityThreshold) // Desativa partículas/slowdown se o jogador parar
                    {
                        if (SlowdownActive)
                            DeactivateSlowdown();
                    }
                    else // Ajusta a posição das partículas e, caso necessário, ativa slowdown
                    {
                        var collisionPosition = new Vector2(collision.transform.position.x, transform.position.y);
                        
                        if (!SlowdownActive)
                            ActivateSlowdown(collisionPosition);
                        else
                            SetParticlePosition(collisionPosition);
                    }
                }
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                DeactivateSlowdown();
            }
        }
    }
}
