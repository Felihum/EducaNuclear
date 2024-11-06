using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Corrida
{
    public class SpeedDebuff : Pickup
    {
        public float speedNegativeMultiplier = 1.5f;
        public float duration = 5f;

        public static bool effectIsActive = false;

        public override void OnPickup()
        {
            PlayerCorrida.Instance.MultiplierDecrease += speedNegativeMultiplier;
            effectIsActive = true;

            //GameManager.instance.AddEffect(duration, OnTimerEnd);


            base.OnPickup();
        }

        private void OnTimerEnd()
        {
            PlayerCorrida.Instance.MultiplierDecrease -= speedNegativeMultiplier;
            effectIsActive = false;
        }
    }
}
