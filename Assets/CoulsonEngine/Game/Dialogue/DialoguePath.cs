using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// I forgot what tis was used for. If this was used for something important oopsie
namespace CoulsonEngine.Game.Dialogue
{
    [System.Serializable]
    public class DialoguePath
    {
        public string identifier = "mydialogue_Path1";
        public Dialogue dialogue;

        public DialoguePath(string identifier, Dialogue dialogue)
        {
            this.identifier = identifier;
            this.dialogue = dialogue;
        }
    }
}

