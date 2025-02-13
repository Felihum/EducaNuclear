using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Corrida
{
    [CreateAssetMenu(menuName = "ScriptableObjects/StarInvencible Data")]

    public class StarInvincibleData : PickupData
    {
        public float duration;
        public bool invincible;
    }

}
