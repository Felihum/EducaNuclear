using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Corrida
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Player Data")]
    public class PlayerDataCorrida : ScriptableObject
    {
        public GameObject PrefabA { get { return GFX_A; } private set { GFX_A = value; } }
        public GameObject PrefabB { get { return GFX_B; } private set { GFX_B = value; } }
        [SerializeField] private GameObject GFX_A;
        [SerializeField] private GameObject GFX_B;

        public PlayerCharacter SelectedCharacter; //{ get; private set; }
    }

    public enum PlayerCharacter
    {
        A,
        B
    }
}
