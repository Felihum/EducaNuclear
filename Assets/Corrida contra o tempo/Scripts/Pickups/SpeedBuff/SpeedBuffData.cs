using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Corrida
{
    // ScriptableObject para guardar as variáveis de SpeedBuff
    [CreateAssetMenu(menuName = "ScriptableObjects/SpeedBuff Data")]
    public class SpeedBuffData : PickupData
    {
        // public new string name;
        // public Sprite sprite;
        // public PickupType type;
        public float multiplier;
        public float duration;
    }
}
