using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Trunfo{
    [Serializable]
    public class Carta : MonoBehaviour
    {
        public string Name;
        public float MassaAtomica;
        public float PontoDeFusao;
        public float AbundanciaCorpo;
        public float Eletronegatividade;

        public Carta(){

        }

        public Carta(string Name, float MassaAtomica, float PontoDeFusao, float AbundanciaCorpo, float Eletronegatividade){
            this.Name = Name;
            this.MassaAtomica = MassaAtomica;
            this.PontoDeFusao = PontoDeFusao;
            this.AbundanciaCorpo = AbundanciaCorpo;
            this.Eletronegatividade = Eletronegatividade;

        }
    }
}