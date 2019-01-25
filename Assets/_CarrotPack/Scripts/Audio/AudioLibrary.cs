using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;


namespace CarrotPack
{

    [CreateAssetMenu(menuName = "Royce/AudioLibrary", fileName = "AudioLibrary")]
    public class AudioLibrary : SerializedScriptableObject
    {
        [Header("Audio Library")]
        [DictionaryDrawerSettings(DisplayMode = DictionaryDisplayOptions.OneLine,
                IsReadOnly = false,
                KeyLabel = "Name",
                ValueLabel = "AudioClip")]
        [GUIColor(0.7f, 0.7f, 1)]
        public Dictionary<string, AudioClip> audioLibrary = new Dictionary<string, AudioClip>();
    
        public void InitLibrary()
        {

        }
    }
}

#else

namespace CarrotPack
{
    [CreateAssetMenu(menuName = "Royce/AudioLibrary", fileName = "AudioLibrary")]
    public class AudioLibrary : ScriptableObject
    {
        [Header("Audio Library")]

        public List<AudioClip> audioList = new List<AudioClip>();

        [HideInInspector] public Dictionary<string, AudioClip> audioLibrary = new Dictionary<string, AudioClip>();

        public void InitLibrary()
        {
            foreach(AudioClip audioClip in audioList)
            {
                audioLibrary.Add(audioClip.name, audioClip);
            }
        }
    }
}



#endif

