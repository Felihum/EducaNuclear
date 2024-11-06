using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Alquimia
{
    public class WaveEffectScript : MonoBehaviour
    {
        private Material rippleMaterial;
        private float rippleStartTime;
        public float rippleDuration = 1.0f; // Duração total da animação
        public float maxRadius = 1.0f;      // Raio máximo da ondulação
        public float rippleWidth = 0.1f;    // Largura do anel de ondulação
        public float rippleSpeed = 2.0f;    // Velocidade da ondulação

        void Start()
        {
            rippleMaterial = GetComponent<Renderer>().material;
            rippleMaterial.SetFloat("_RippleWidth", rippleWidth);
            rippleMaterial.SetFloat("_RippleSpeed", rippleSpeed);
        }

        void OnMouseDown() // Ou qualquer outro método de seleção
        {
            rippleStartTime = Time.time;
            StartCoroutine(RippleAnimation());
        }

        IEnumerator RippleAnimation()
        {
            while (Time.time - rippleStartTime < rippleDuration)
            {
                float t = (Time.time - rippleStartTime) / rippleDuration;
                float currentRadius = Mathf.Lerp(0.0f, maxRadius, t);
                rippleMaterial.SetFloat("_RippleRadius", currentRadius);
                rippleMaterial.SetFloat("_RippleOpacity", 1.0f - t); // A ondulação se torna mais transparente com o tempo
                yield return null;
            }

            rippleMaterial.SetFloat("_RippleOpacity", 0.0f); // Certifique-se de que a ondulação desapareça no final
        }
    }
}
