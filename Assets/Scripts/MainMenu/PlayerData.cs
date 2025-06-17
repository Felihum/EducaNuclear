using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EducaNuclear
{
    [Serializable]
    public class PlayerData
    {
        public int currentPhase;
        //public int punctuation;


        public PlayerData()
        {

        }

        public PlayerData(int currentPhase)
        {
            this.currentPhase = currentPhase;
            //this.punctuation = punctuation;
        }
    }
}
