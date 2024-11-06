using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Corrida
{
    // Script para o Pickup de velocidade
    // Suas propriedades estão salvas em arquivo ScriptableObject (SpeedBuffData), altere as variáveis desse Pickup lá
    public class SpeedBuff : Pickup
    {
        public SpeedBuffData data;

        private float speedMultiplier = 0.1f;
        private float duration = 0.5f;

        public static bool effectIsActive = false;

        public override void OnPickup()
        {
            GameManager.Instance.AddEffect(duration, data.type, ApplyEffect, OnTimerEnd);

            base.OnPickup();
        }

        private void ApplyEffect()
        {
            PlayerCorrida.Instance.MultiplierIncrease += speedMultiplier;
            effectIsActive = true;
        }

        private void OnTimerEnd()
        {
            PlayerCorrida.Instance.MultiplierIncrease -= speedMultiplier;
            effectIsActive = false;
        }

        protected override void Awake()
        {
            // Aplicando os dados de SpeedBuffData ao objeto
            gameObject.name = data.name;
            
            var spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = data.sprite;

            speedMultiplier = data.multiplier;
            duration = data.duration;

            base.Awake();
        }
    }
}
