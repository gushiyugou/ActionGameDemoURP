using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

[CreateAssetMenu(fileName = "new CharacterSound",menuName = "Character/Asset/Sound")]
public class CharacterSound_SO : ScriptableObject
{
    [Serializable]
    private class Sound
    {
        public SoundType soundType;
        public AudioClip[] AudioClips;
    }

    [SerializeField] private List<Sound> configSoundsList = new List<Sound>();
   

    public AudioClip GetAudioClip(SoundType soundType)
    {
        if (configSoundsList.Count == 0) return null;
        switch (soundType)
        {
            case SoundType.Attack:
                return configSoundsList[0].AudioClips[UnityEngine.Random.Range(0, configSoundsList[0].AudioClips.Length)];
            case SoundType.Hit:
                return configSoundsList[1].AudioClips[UnityEngine.Random.Range(0, configSoundsList[1].AudioClips.Length)];
            case SoundType.Block:
                return configSoundsList[2].AudioClips[UnityEngine.Random.Range(0, configSoundsList[2].AudioClips.Length)];
            case SoundType.FootStep:
                return configSoundsList[3].AudioClips[UnityEngine.Random.Range(0, configSoundsList[3].AudioClips.Length)];
        }

        return null;
    }
}
