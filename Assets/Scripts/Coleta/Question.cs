using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Coleta
{
    [Serializable]
    
    public class Question
    {
        public string question;
        public string answer;
        public string[] options;

        public Question()
        {

        }

        public Question(string question, string answer, string[] options)
        {
            this.question = question;
            this.answer = answer;
            this.options = options;
        }
    }
}

