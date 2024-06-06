using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CoulsonEngine.Game.Dialogue
{
    [CreateAssetMenu(fileName = "DialogueSfxPreset", menuName = "CoulsonEngine/Registries/Dialogue Presets")]
    public class DialoguePresets : ScriptableObject
    {
        public DialoguePreset[] presets; 
    }

    [System.Serializable]
    public class DialoguePreset
    {
        public string name;
        public Sprite icon;
        public AudioClip[] sound;
    }
}
