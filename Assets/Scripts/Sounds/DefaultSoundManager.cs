using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultSoundManager : MonoBehaviour, SoundManager
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] sounds;

    private Dictionary<string, AudioClip> soundsDatabase;

    #region MonoBehaviour methods

    private void Awake()
    {
        for (int i = 0; i < sounds.Length; ++i)
        {
            AudioClip sound = sounds[i];
            if (soundsDatabase.ContainsKey(sound.name))
                throw new Exception("An AudioClip named " + sound.name + " already exists.");

            soundsDatabase.Add(sound.name, sound);
        }
    }

    #endregion

    #region SoundManager implementation

    public void PlaySoundWithIdentifier(string identifier)
    {
        AudioClip sound;
        if (soundsDatabase.TryGetValue(identifier, out sound))
            audioSource.PlayOneShot(sound);
    }

    public void StopAllSounds()
    {
        audioSource.Stop();
    }

    #endregion
}
