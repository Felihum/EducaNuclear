using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Corrida
{
    // Superclasse para os dados de Pickups
    // Não crie dados dessa classe, crie uma subclasse
    public class PickupData : ScriptableObject
    {
        public new string name;
        public Sprite sprite;
        public PickupType type;
    }

    // Todos os tipos de Pickup devem ser listados aqui
    public enum PickupType
    {
        Speed,
        Time
    }
}
